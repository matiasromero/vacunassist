import { OfficeService } from 'src/app/_services/office.service';
import { first } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { AccountService } from 'src/app/_services/account.service';
import { AlertService } from 'src/app/_services/alert.service';
import { Location } from '@angular/common';


@Component({ templateUrl: 'edit-office.component.html' })
export class EditOfficeComponent implements OnInit {
    form!: UntypedFormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: UntypedFormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private accountService: AccountService,
        private officeService: OfficeService,
        private alertService: AlertService,
        private _location: Location
    ) { 
        if (this.accountService.userValue.role !== 'administrator') {
            this.router.navigate(['/']);
        }
    }

    public officeId?: number;

    ngOnInit() {

        this.officeId = parseInt(this.route.snapshot.paramMap.get('id')!);
        this.officeService.getById(this.officeId).subscribe(res => {
        this.form.patchValue({
            id: res.id,
            name: res.name,
            isActive: res.isActive
        });
    });

        this.form = this.formBuilder.group({
            name: ['', [Validators.required, Validators.maxLength(100)]],
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
        
        this.officeService.update(this.officeId!, this.form.value)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.alertService.success('Sede modificada correctamente', { keepAfterRouteChange: true });
                    this.router.navigate(['../../../offices'], { relativeTo: this.route });
                },
                error: error => {
                    this.alertService.error(error);
                    this.loading = false;
                }
            });
    }
}