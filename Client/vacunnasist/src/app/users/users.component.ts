import { AppointmentService } from 'src/app/_services/appointment.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AccountService } from 'src/app/_services/account.service';
import { AlertService } from 'src/app/_services/alert.service';
import { DatePipe } from '@angular/common';
import { Appointment } from 'src/app/_models/appointment';
import Swal from 'sweetalert2';

@Component({ templateUrl: 'users.component.html' })
export class UsersComponent implements OnInit {
    form!: FormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private accountService: AccountService,
        private appointmentsService: AppointmentService,
        private alertService: AlertService,
        private dp: DatePipe
    ) { 
        // redirect to home if already logged in
        if (this.accountService.userValue.role !== 'administrator') {
            this.router.navigate(['/']);
        }
    }

    public appointments: Appointment[] = [];

    ngOnInit() {
       this.loadData();
    }

    loadData() {
        this.appointmentsService.getByUser().subscribe((res: any) => {
            this.appointments = res.appointments;
        });

    }

    cancelAppointmentQuestion(a: Appointment) {
        Swal
      .fire({
        title: '¿Está seguro?',
        text: 'Va a cancelar la solicitud del turno de la vacuna: ' + a.vaccineName,
        icon: 'warning',
        showCancelButton: true,
        cancelButtonText: 'No',
        confirmButtonText: 'Si, cancelar!'
      })
      .then(result => {
        if (result.value) {
          this.cancelAppointment(a);
          
        }
      });
    }

    cancelAppointment(a: Appointment) {
        this.appointmentsService.cancel(a)
        .pipe(first())
        .subscribe({
            next: () => {
                Swal.fire('Turno cancelado', 'Su turno ha sido dado de baja correctamente.', 'success');
                this.loadData();
                this.loading = false;
            },
            error: (error: string) => {
                this.alertService.error(error);
                this.loading = false;
            }
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
        
        this.form.value.birthDate = this.dp.transform(this.form.value.birthDate, 'yyyy-MM-dd');
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