import { Appointment } from './../../_models/appointment';
import { OfficeService } from './../../_services/office.service';
import { Office } from './../../_models/office';
import { first } from 'rxjs/operators';
import { Vaccine } from './../../_models/vaccine';
import { VaccineService } from 'src/app/_services/vaccine.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { AccountService } from 'src/app/_services/account.service';
import { AlertService } from 'src/app/_services/alert.service';
import { AppointmentService } from 'src/app/_services/appointment.service';
import { User } from 'src/app/_models/user';
import { UsersFilter } from 'src/app/_models/filters/users-filter';
import { NewConfirmedAppointmentRequest } from 'src/app/_models/new-confirmed-appointment';
import { DatePipe } from '@angular/common';
import Swal from 'sweetalert2';

@Component({ templateUrl: 'complete-appointment.component.html' })
export class CompleteAppointmentComponent implements OnInit {
    form!: UntypedFormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: UntypedFormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private accountService: AccountService,
        private vaccinesServices: VaccineService,
        private appointmentsService: AppointmentService,
        private officesService: OfficeService,
        private alertService: AlertService,
        private dp: DatePipe
    ) { 
        if (this.accountService.userValue.role !== 'vacunator') {
            this.router.navigate(['/']);
        }
    }

    public vaccines: Vaccine[] = [];
    public patients: User[] = [];
    public vaccinators: User[] = [];
    public offices: Office[] = [];
    public minDate: Date = new Date();

    appointmentId?: number;

    vaccineName?:string;
    patientName?:string;
    date?:string;

    ngOnInit() {

        this.appointmentId = parseInt(this.route.snapshot.paramMap.get('id')!);

        this.officesService.getAll().subscribe((res: any) => {
            this.offices = res.offices;
        });
        this.vaccinesServices.getAll().subscribe((res: any) => {
            this.vaccines = res.vaccines.filter((x:Vaccine) => x.canBeRequested);
        });

        let filter1 = new UsersFilter();
      filter1.role = 'vacunator';
      this.accountService.getAll(filter1).subscribe((res: any) => {
        this.vaccinators = res.users;
    });

    let filter2 = new UsersFilter();
      filter2.role = 'patient';
      this.accountService.getAll(filter2).subscribe((res: any) => {
        this.patients = res.users;
    });

    this.appointmentsService.getById(this.appointmentId).subscribe((res: Appointment) => {
        let date = new Date(res.date!);
        this.patientName = res.patientName;
        this.vaccineName = res.vaccineName;
        this.date = this.dp.transform(res.date, 'yyyy-MM-dd HH:mm')!;

        this.form.patchValue({
            id: res.id,
            patientId: res.patientId,
            vaccineId: res.vaccineId,
            officeId: res.preferedOfficeId,
            vaccinatorId: res.vaccinatorId,
            date: res.date,
            comment: res.comment,
            time: date.toLocaleTimeString('en-US', { hour12: false })
        });
    });

        this.form = this.formBuilder.group({
            vaccineId: [null, Validators.required],
            patientId: [null, Validators.required], 
            officeId: [null, Validators.required], 
            vaccinatorId: [null, Validators.required],
            date: [new Date(), Validators.required],
            time: [null, Validators.required],
            comment: [null, Validators.maxLength(200)]
        });
    }

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    complete() {
        Swal
        .fire({
          title: '¿Está seguro?',
          text: 'Va a confirmar que aplicó la vacuna ' + this.vaccineName + ' a ' + this.patientName + ' en el turno: ' + this.date,
          icon: 'success',
          showCancelButton: true,
          cancelButtonText: 'No',
          confirmButtonText: 'Si, confirmar!'
        })
        .then(result => {
          if (result.value) {
            this.doConfirm();
          }
        });
    }

    doConfirm() {
        let model = new NewConfirmedAppointmentRequest();
        model.currentId = this.appointmentId;
        model.status = 2;
        model.comment = this.form.get('comment')?.value;
        this.appointmentsService.update(model.currentId!, model)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.alertService.success('Vacunada aplicada correctamente', { keepAfterRouteChange: true });
                    this.loading = false;
                    this.router.navigate(['../../'], { relativeTo: this.route });
                },
                error: error => {
                    this.alertService.error(error);
                    this.loading = false;
                }
            });
    }

    cancel() {
        Swal
        .fire({
          title: '¿Está seguro?',
          text: 'Va a confirmar que NO ha aplicado la vacuna ' + this.vaccineName + ' a ' + this.patientName + ' en el turno: ' + this.date,
          icon: 'error',
          showCancelButton: true,
          cancelButtonText: 'No',
          confirmButtonText: 'Si, cancelar!'
        })
        .then(result => {
          if (result.value) {
            this.doCancel();
          }
        });
    }

    doCancel() {
        let model = new NewConfirmedAppointmentRequest();
        model.currentId = this.appointmentId;
        model.status = 3;
        model.comment = this.form.get('comment')?.value;
        this.appointmentsService.update(model.currentId!, model)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.alertService.warn('Turno cancelado correctamente', { keepAfterRouteChange: true });
                    this.loading = false;
                    this.router.navigate(['../../'], { relativeTo: this.route });
                },
                error: error => {
                    this.alertService.error(error);
                    this.loading = false;
                }
            });
    }
}