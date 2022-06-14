import { Vaccine } from './../../../_models/vaccine';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AccountService } from 'src/app/_services/account.service';
import { AlertService } from 'src/app/_services/alert.service';
import { DatePipe } from '@angular/common';
import { User } from 'src/app/_models/user';
import { Office } from 'src/app/_models/office';
import { OfficeService } from 'src/app/_services/office.service';
import Swal from 'sweetalert2';
import { VaccineService } from 'src/app/_services/vaccine.service';


@Component({ templateUrl: 'add-vaccine.component.html' })
export class ProfileAddVaccineComponent implements OnInit {
    form!: UntypedFormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: UntypedFormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private accountService: AccountService,
        private vaccinesService: VaccineService,
        private alertService: AlertService,
        private dp: DatePipe
    ) { 
    }

    maxDate: Date = new Date();
    public vaccines: Vaccine[] = [];
    ngOnInit() {
        this.vaccinesService.getAll().subscribe((res: any) => {
            this.vaccines = res.vaccines;
        });
        
        this.form = this.formBuilder.group({
            vaccineId: [null, [Validators.required]],
            appliedDate: [new Date()],
            comment: ['', Validators.maxLength(200)],
        });
    }

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    onSubmit() {
        this.submitted = true;

        // reset alerts on submit
        this.alertService.clear();

        // stop here if form is invalid
        if (this.form.invalid) {
            return;
        }

        this.loading = true;
        
        this.form.value.appliedDate = this.dp.transform(this.form.value.appliedDate, 'yyyy-MM-dd');
        this.accountService.addVaccine(+this.accountService.userValue.id, this.form.value)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.alertService.success('Vacuna agregada', { keepAfterRouteChange: true });
                    this.router.navigate(['/account/profile'], { relativeTo: this.route });
                },
                error: error => {
                    this.alertService.error(error);
                    this.loading = false;
                }
            });
    }
}