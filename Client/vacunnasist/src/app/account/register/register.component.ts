import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AccountService } from 'src/app/_services/account.service';
import { AlertService } from 'src/app/_services/alert.service';
import { DatePipe } from '@angular/common';


@Component({ templateUrl: 'register.component.html' })
export class RegisterComponent implements OnInit {
    form!: FormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private accountService: AccountService,
        private alertService: AlertService,
        private dp: DatePipe
    ) { }

    ngOnInit() {
        this.form = this.formBuilder.group({
            fullName: ['', [Validators.required, Validators.maxLength(100)]],
            dni: ['', [Validators.required, Validators.maxLength(20)]],
            username: ['', [Validators.required, Validators.maxLength(20)]],
            password: ['', Validators.required],
            address: ['', [Validators.required, Validators.maxLength(200)]],
            birthDate: [this.dp.transform( new Date(), 'dd/MM/yyyy' ), [Validators.required, Validators.pattern(/^(0[1-9]|[1-2][0-9]|3[0-1])\/(0[1-9]|1[0-2])\/[0-9]{4}$/)]],
            phoneNumber: ['', [Validators.required, Validators.maxLength(30)]],
            email:['', [Validators.required, Validators.email, Validators.maxLength(30)]],
            gender: ['male', Validators.required],
            belongsToRiskGroup: [false, Validators.required]
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
        
        var dateParts = this.form.value.birthDate.split("/");
        var newDate = new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0]); 
        this.form.value.birthDate = this.dp.transform(newDate, 'yyyy-MM-dd');
        this.form.value.dni = String(this.form.value.dni);
        this.accountService.register(this.form.value)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.alertService.success('Registración correcta', { keepAfterRouteChange: true });
                    this.router.navigate(['../login'], { relativeTo: this.route });
                },
                error: error => {
                    this.alertService.error(error);
                    this.loading = false;
                }
            });
    }
}