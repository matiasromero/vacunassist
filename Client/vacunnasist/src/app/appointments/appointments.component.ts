import { Vaccine } from './../_models/vaccine';
import { OfficeService } from './../_services/office.service';
import { UsersFilter } from 'src/app/_models/filters/users-filter';
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
import { User } from '../_models/user';
import { AppointmentsFilter } from '../_models/filters/appointments-filter';
import { Office } from '../_models/office';
import { OfficesFilter } from '../_models/filters/offices-filter';

@Component({ templateUrl: 'appointments.component.html' })
export class AppointmentsComponent implements OnInit {
    formFilter!: FormGroup;
    loading = false;
    submitted = false;
  role: string;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private accountService: AccountService,
        private officesService: OfficeService,
        private appointmentService: AppointmentService,
        private alertService: AlertService,
        private dp: DatePipe
    ) { 
        // redirect to home if already logged in
        if (this.accountService.userValue.role == 'patient') {
            this.router.navigate(['/']);
        }

        this.role = accountService.userValue.role;


        this.formFilter = this.formBuilder.group({
            fullName: ['', [Validators.maxLength(100)]],
            status: [''],
            date: [null],
            officeId: [''],
            vaccinatorId: ['']
        });
        

            this.route.queryParams.subscribe((params) => {
                if (params.fullName) {
                    this.formFilter.controls.fullName.setValue(params.fullName, {
                        onlySelf: true,
                      });
                }
                if (params.status) {
                  this.formFilter.controls.status.setValue(params.status);
              }
              if (params.date) {
                this.formFilter.controls.date.setValue(params.date, {
                    onlySelf: true,
                  });
            }
            if (params.officeId) {
              this.formFilter.controls.officeId.setValue(+params.officeId);
          }
          
          if (params.vaccinatorId) {
            this.formFilter.controls.vaccinatorId.setValue(+params.vaccinatorId);
        }
                this.loadData();
              });
    }

    public appointments: Appointment[] = [];
    public filter = new AppointmentsFilter();
    maxDate: Date = new Date();
    public vaccinators: User[] = [];
    public offices: Office[] = [];

    ngOnInit() {
      let filter = new UsersFilter();
      filter.role = 'vacunator';
      this.accountService.getAll(filter).subscribe((res: any) => {
        this.vaccinators = res.users;
    });

    let officesFilter = new OfficesFilter();
    officesFilter.isActive = true;
    this.officesService.getAll(officesFilter).subscribe((res: any) => {
      this.offices = res.offices;
  });
    }

    loadData() {
        const fullName = this.formFilter.get('fullName')?.value;
        const status = this.formFilter.get('status')?.value;
        const vaccinatorId = this.formFilter.get('vaccinatorId')?.value;
        const date = this.formFilter.get('date')?.value;
        const officeId = this.formFilter.get('officeId')?.value;
        this.filter.fullName =fullName;
        this.filter.status =status;
        this.filter.vaccinatorId=vaccinatorId;
        this.filter.date=date;
        this.filter.officeId=officeId;
        this.appointmentService.getAll(this.filter).subscribe((res: Appointment[]) => {
            this.appointments = res;
        });
    }

    panelOpenState = false;

    // convenience getter for easy access to form fields
    get f() { return this.formFilter.controls; }

    applyFilter() {
        this.submitted = true;

        // reset alerts on submit
        this.alertService.clear();

        // stop here if form is invalid
        if (this.formFilter.invalid) {
            return;
        }

        this.loading = true;

        const fullName = this.formFilter.get('fullName')?.value;
        const status = this.formFilter.get('status')?.value;
        const vaccinatorId = this.formFilter.get('vaccinatorId')?.value;
        const officeId = this.formFilter.get('officeId')?.value;
        const date = this.dp.transform(this.formFilter.get('date')?.value, 'yyyy-MM-dd');
        const queryParams: any = {};
          if (fullName) {
            queryParams.fullName = fullName;
          }
          if (status) {
            queryParams.status = status;
          }
          if (vaccinatorId) {
            queryParams.vaccinatorId = vaccinatorId;
          }
          if (officeId) {
            queryParams.officeId = officeId;
          }
          if (date) {
            queryParams.date = date;
          }
          this.router.navigate(['/appointments'], {
            queryParams,
          });

          this.loading = false;
    }

    clear() {
      // reset alerts on submit
      this.alertService.clear();

      this.formFilter.controls.fullName.setValue('', {
        onlySelf: true,
      });
      this.formFilter.controls.status.setValue('', {
        onlySelf: true,
      });
      this.formFilter.controls.officeId.setValue('', {
        onlySelf: true,
      });
      this.formFilter.controls.vaccinatorId.setValue('', {
        onlySelf: true,
      });
      this.formFilter.controls.date.setValue(null, {
        onlySelf: true,
      });

      this.applyFilter();
  }

  addAppointment() {
    this.router.navigate(['appointments','new']);
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
    this.appointmentService.cancel(a)
    .pipe(first())
    .subscribe({
        next: () => {
            Swal.fire('Turno cancelado', 'El turno ha sido dado de baja correctamente.', 'success');
            this.loadData();
            this.loading = false;
        },
        error: (error: string) => {
            this.alertService.error(error);
            this.loading = false;
        }
    });
}

confirmAppointment(a: Appointment) {
  this.router.navigate(['appointments','confirm', a.id]);
}

editAppointment(a: Appointment) {
  this.router.navigate(['appointments','edit', a.id]);
}

completeAppointment(a: Appointment) {
  this.router.navigate(['appointments','complete', a.id]);
}


}