import { VaccineService } from 'src/app/_services/vaccine.service';
import { User } from 'src/app/_models/user';
import { Office } from 'src/app/_models/office';
import { OfficeService } from 'src/app/_services/office.service';
import { first } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { AccountService } from 'src/app/_services/account.service';
import { AlertService } from 'src/app/_services/alert.service';
import { AppointmentService } from 'src/app/_services/appointment.service';
import { DatePipe, Location } from '@angular/common';


@Component({ templateUrl: 'edit-vaccine.component.html' })
export class EditVaccineComponent implements OnInit {
    form!: UntypedFormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: UntypedFormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private accountService: AccountService,
        private vaccineService: VaccineService,
        private alertService: AlertService,
        private dp: DatePipe,
        private _location: Location
    ) { 
        if (this.accountService.userValue.role !== 'administrator') {
            this.router.navigate(['/']);
        }
    }

    public vaccineId?: number;

    ngOnInit() {

        this.vaccineId = parseInt(this.route.snapshot.paramMap.get('id')!);
        this.vaccineService.getById(this.vaccineId).subscribe(res => {
        this.form.patchValue({
            id: res.id,
            name: res.name,
            canBeRequested: res.canBeRequested,
            isActive: res.isActive
        });
    });

        this.form = this.formBuilder.group({
            name: ['', [Validators.required, Validators.maxLength(100)]],
            canBeRequested: [true, Validators.required],
            isActive: [true, Validators.required]
        });
    }

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }
    
    backClicked()
    {
        this._location.back();
      }

    onSubmit() {
        this.submitted = true;

        // reset alerts on submit
        this.alertService.clear();

        // stop here if form is invalid
        if (this.form.invalid) {
            return;
        }

        this.loading = true;
        
        this.vaccineService.update(this.vaccineId!, this.form.value)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.alertService.success('Vacuna modificada correctamente', { keepAfterRouteChange: true });
                    this._location.back();
                },
                error: error => {
                    this.alertService.error(error);
                    this.loading = false;
                }
            });
    }
}