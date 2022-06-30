import { User } from 'src/app/_models/user';
import { Office } from 'src/app/_models/office';
import { OfficeService } from 'src/app/_services/office.service';
import { first } from 'rxjs/operators';
import { Vaccine } from './../../_models/vaccine';
import { VaccineService } from 'src/app/_services/vaccine.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { AccountService } from 'src/app/_services/account.service';
import { AlertService } from 'src/app/_services/alert.service';
import { AppointmentService } from 'src/app/_services/appointment.service';
import { DatePipe, Location } from '@angular/common';
import { trigger } from '@angular/animations';
import { OfficesFilter } from 'src/app/_models/filters/offices-filter';


@Component({ templateUrl: 'edit-user.component.html' })
export class EditUserComponent implements OnInit {
    form!: UntypedFormGroup;
    loading = false;
    submitted = false;
    minDate: Date = new Date();
    maxDate: Date = new Date();

    constructor(
        private formBuilder: UntypedFormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private accountService: AccountService,
        private officesService: OfficeService,
        private appointmentsService: AppointmentService,
        private alertService: AlertService,
        private dp: DatePipe,
        private _location: Location
    ) { 
        if (this.accountService.userValue.role !== 'administrator') {
            this.router.navigate(['/']);
        }
    }

    public offices: Office[] = [];
    public type: String = "patient";
    userId?: number;
    userRole?:string;
    user: User = new User();

    ngOnInit() {
        this.minDate = new Date(1900, 0, 1);

        let filter = new OfficesFilter();
        filter.isActive = true;
        this.officesService.getAll(filter).subscribe((res: any) => {
            this.offices = res.offices;
        });

        this.userId = parseInt(this.route.snapshot.paramMap.get('id')!);
        this.accountService.getById(this.userId).subscribe(res => {
            this.type = res.role;
            this.userRole = res.role;
            this.user = res;
        this.form.patchValue({
            id: res.id,
            username: res.userName,
                fullName: res.fullName,
                dni: res.dni,
                address: res.address,
                birthDate: res.birthDate,
                phoneNumber: res.phoneNumber,
                email: res.email,
                gender: res.gender,
                belongsToRiskGroup: res.belongsToRiskGroup,
                preferedOfficeId: res.preferedOfficeId,
            role: res.role,
            isActive: res.isActive
        });
    });

        this.form = this.formBuilder.group({
            username: ['', [Validators.required, Validators.maxLength(20)]],
            fullName: ['', [Validators.required, Validators.maxLength(100)]],
            dni: ['', [Validators.required, Validators.maxLength(20)]],
            address: ['', [Validators.required, Validators.maxLength(200)]],
            birthDate: [new Date(), Validators.required],
            phoneNumber: ['', [Validators.required, Validators.maxLength(30)]],
            email:['', [Validators.required, Validators.email, Validators.maxLength(30)]],
            gender: ['male', Validators.required],
            belongsToRiskGroup: [false, Validators.required],
            preferedOfficeId: [null]
        });
    }

    addVaccine() {
        this.router.navigate(['appointments/add-vaccine-to-user', this.userId]);
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
        
        this.form.value.birthDate = this.dp.transform(this.form.value.birthDate, 'yyyy-MM-dd');
        this.form.value.dni = String(this.form.value.dni);
        this.accountService.update(this.userId!, this.form.value)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.alertService.success((this.type == 'patient' ? 'Paciente ' : (this.type == 'vacunator' ? 'Vacunador ' : 'Usuario')) + ' modificado correctamente', { keepAfterRouteChange: true });
                    this.router.navigate(['../../../users'], { 
                        queryParams: {type: this.type, isActive: true, belongsToRiskGroup: false},
                     relativeTo: this.route });
                },
                error: error => {
                    this.alertService.error(error);
                    this.loading = false;
                }
            });
    }
}