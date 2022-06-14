import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AccountService } from 'src/app/_services/account.service';
import { AlertService } from 'src/app/_services/alert.service';
import { DatePipe } from '@angular/common';
import { DistinctValidator } from 'src/app/_helpers/distinct.validator';


@Component({ templateUrl: 'change-password.component.html' })
export class ChangePasswordComponent implements OnInit {
    form!: UntypedFormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: UntypedFormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private accountService: AccountService,
        private alertService: AlertService,
        private dp: DatePipe
    ) { }

    ngOnInit() {
        this.form = this.formBuilder.group({
            password: ['', Validators.required],
            newPassword: ['', [Validators.required]]
        }, 
        { 
            validator: DistinctValidator('password', 'newPassword')
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
       
        this.accountService.changePassword(this.form.value, this.accountService.userValue)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.alertService.success('Contraseña modificada correctamente', { keepAfterRouteChange: true });
                    this.router.navigate(['../login'], { relativeTo: this.route });
                },
                error: error => {
                    this.alertService.error(error);
                    this.loading = false;
                }
            });
    }
}