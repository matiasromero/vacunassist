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


@Component({ templateUrl: 'edit-appointment.component.html' })
export class EditAppointmentComponent implements OnInit {
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
        if (this.accountService.userValue.role !== 'administrator') {
            this.router.navigate(['/']);
        }
    }

    public vaccines: Vaccine[] = [];
    public patients: User[] = [];
    public vaccinators: User[] = [];
    public offices: Office[] = [];
    public minDate: Date = new Date();

    appointmentId?: number;

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
        this.form.patchValue({
            id: res.id,
            patientId: res.patientId,
            vaccineId: res.vaccineId,
            officeId: res.preferedOfficeId,
            vaccinatorId: res.vaccinatorId,
            date: res.date,
            time: date.toLocaleTimeString('en-US', { hour12: false })
        });
    });

        this.form = this.formBuilder.group({
            vaccineId: [null, Validators.required],
            patientId: [null, Validators.required], 
            officeId: [null, Validators.required], 
            vaccinatorId: [null, Validators.required],
            date: [new Date(), Validators.required],
            time: [null, Validators.required]
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
        var model = new NewConfirmedAppointmentRequest();
        model.officeId = this.form.get('officeId')?.value;
        model.patientId = this.form.get('patientId')?.value;
        model.vaccinatorId = this.form.get('vaccinatorId')?.value;
        model.vaccineId = this.form.get('vaccineId')?.value;
        model.date = this.dp.transform(this.form.value.date, 'yyyy-MM-dd')!;
        model.date = model.date +'T'+ this.form.get('time')?.value;
        model.currentId = this.appointmentId!;
        this.appointmentsService.update(model.currentId, model)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.alertService.success('Turno editado correctamente', { keepAfterRouteChange: true });
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