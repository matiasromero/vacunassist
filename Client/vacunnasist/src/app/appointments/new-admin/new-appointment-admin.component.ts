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
import { VaccinesFilter } from 'src/app/_models/filters/vaccines-filter';
import { OfficesFilter } from 'src/app/_models/filters/offices-filter';


@Component({ templateUrl: 'new-appointment-admin.component.html' })
export class NewAppointmentAdminComponent implements OnInit {
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
    public maxDate: Date = new Date();

    ngOnInit() {
        let officesFilter = new OfficesFilter();
        officesFilter.isActive = true;
        this.officesService.getAll(officesFilter).subscribe((res: any) => {
            this.offices = res.offices;
        });
        let filter = new VaccinesFilter();
        filter.isActive = true;
        filter.canBeRequested = true;
        this.vaccinesServices.getAll(filter).subscribe((res: any) => {
            this.vaccines = res.vaccines;
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

    changePatient(patientId: number) {
        this.accountService.getById(patientId).subscribe((u: User) => {
            let filter = new VaccinesFilter();
        filter.isActive = true;
        filter.canBeRequested = true;
            this.vaccinesServices.getAll(filter).subscribe((res: any) => {
                this.vaccines = res.vaccines.filter((x:Vaccine) => {
                    let v = new Vaccine();
                    v.id = x.id;
                    v.name = x.name;
                    return v.canApply(u.age, u.belongsToRiskGroup);
                });
                if (!this.vaccines.find(v => {
                    return v.id == this.form.get('vaccineId')?.value;
                })) {
                    this.form.patchValue({
                        vaccineId: null
                    });
                }
                if (this.form.get('vaccineId')?.value) {
                    let v = new Vaccine();
                    v.id = this.form.get('vaccineId')?.value.toString();
                    this.minDate = v.getMinDate(u.age, u.belongsToRiskGroup);
                    this.maxDate = v.getMaxDate(u.age, u.belongsToRiskGroup);
                }
            });
        });
    }

    changeVaccine(vaccineId: number) {
        this.accountService.getById(this.form.get('patientId')?.value).subscribe((u: User) => {
                    let v = new Vaccine();
                    v.id = vaccineId.toString();
                    this.minDate = v.getMinDate(u.age, u.belongsToRiskGroup);
                    this.maxDate = v.getMaxDate(u.age, u.belongsToRiskGroup);
            });
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
        var model = new NewConfirmedAppointmentRequest();
        model.officeId = this.form.get('officeId')?.value;
        model.patientId = this.form.get('patientId')?.value;
        model.vaccinatorId = this.form.get('vaccinatorId')?.value;
        model.vaccineId = this.form.get('vaccineId')?.value;
        model.date = this.dp.transform(this.form.value.date, 'yyyy-MM-dd')!;
        model.date = model.date +'T'+ this.form.get('time')?.value + ':00.000';
        this.appointmentsService.newConfirmedAppointment(model)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.alertService.success('Turno cargado correctamente', { keepAfterRouteChange: true });
                    this.loading = false;
                    this.router.navigate(['../'], { relativeTo: this.route });
                    
                },
                error: error => {
                    this.alertService.error(error);
                    this.loading = false;
                }
            });
    }
}