import { first } from 'rxjs/operators';
import { Vaccine } from './../../_models/vaccine';
import { VaccineService } from 'src/app/_services/vaccine.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { AccountService } from 'src/app/_services/account.service';
import { AlertService } from 'src/app/_services/alert.service';
import { AppointmentService } from 'src/app/_services/appointment.service';


@Component({ templateUrl: 'new-appointment.component.html' })
export class NewAppointmentComponent implements OnInit {
    form!: UntypedFormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: UntypedFormBuilder,
        private router: Router,
        private accountService: AccountService,
        private vaccinesServices: VaccineService,
        private appointmentsService: AppointmentService,
        private alertService: AlertService
    ) { 
        if (this.accountService.userValue.role !== 'patient') {
            this.router.navigate(['/']);
        }
    }

    public vaccines: Vaccine[] = [];

    ngOnInit() {
        this.vaccinesServices.getAll().subscribe((res: any) => {
            this.vaccines = res.vaccines.filter((x:Vaccine) => x.canBeRequested);
        });

        this.form = this.formBuilder.group({
            vaccineId: [null, Validators.required]
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
        
        this.appointmentsService.newAppointment(this.form.value)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.alertService.success('Solicitud de turno cargada correctamente', { keepAfterRouteChange: true });
                    this.loading = false;
                },
                error: error => {
                    this.alertService.error(error);
                    this.loading = false;
                }
            });
    }
}