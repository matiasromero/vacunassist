import { VaccineService } from 'src/app/_services/vaccine.service';
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
import { AppliedVaccine } from 'src/app/_models/applied-vaccine';

@Component({ templateUrl: 'my-appointments.component.html' })
export class MyAppointmentsComponent implements OnInit {
    form!: FormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private accountService: AccountService,
        private vaccineService: VaccineService,
        private appointmentsService: AppointmentService,
        private alertService: AlertService,
        private dp: DatePipe
    ) { 
        // redirect to home if already logged in
        if (this.accountService.userValue.role !== 'patient') {
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

    downloadVaccineCertificate(v: Appointment) {
        Swal
      .fire({
        title: 'Certificado de vacunación',
        text: 'Va a generar el certificado para: ' + v.vaccineName,
        icon: 'warning',
        showCancelButton: true,
        cancelButtonText: 'No, cancelar',
        confirmButtonText: 'Si, generar!'
      })
      .then(result => {
        if (result.value) {
          this.downloadCertificate(v);
        }
      });
    }

    downloadCertificate(v: Appointment) {
        this.vaccineService.downloadCertificateAppointment(v)
        .pipe(first())
        .subscribe({
            next: () => {
                Swal.fire('Certificado generado', 'Certificado generado correctamente.', 'success');
                this.loadData();
            },
            error: (error: string) => {
                this.alertService.error(error);
                this.loading = false;
            }
        });
    }
}