import { VaccineService } from 'src/app/_services/vaccine.service';
import { AppliedVaccine } from './../../_models/applied-vaccine';
import { Vaccine } from './../../_models/vaccine';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AbstractControl, UntypedFormBuilder, UntypedFormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AccountService } from 'src/app/_services/account.service';
import { AlertService } from 'src/app/_services/alert.service';
import { DatePipe } from '@angular/common';
import { User } from 'src/app/_models/user';
import { Office } from 'src/app/_models/office';
import { OfficeService } from 'src/app/_services/office.service';
import Swal from 'sweetalert2';


@Component({ templateUrl: 'profile.component.html' })
export class ProfileComponent implements OnInit {
    form!: UntypedFormGroup;
    loading = false;
    submitted = false;
    minDate: Date = new Date();
    maxDate: Date = new Date();

    constructor(
        private formBuilder: UntypedFormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private accountService: AccountService,
        private vaccineService: VaccineService,
        private officesService: OfficeService,
        private alertService: AlertService,
        private dp: DatePipe
    ) { 
    }

    public offices: Office[] = [];
    public user: User = new User;
    ngOnInit() {
        this.minDate = new Date(1900, 0, 1);

        this.officesService.getAll().subscribe((res: any) => {
            this.offices = res.offices;
        });

        this.loadData();
        
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

    loadData() {
        this.accountService.myProfile().subscribe((res: any) => {
            this.user = res;
            this.form.patchValue({
                username: res.userName,
                fullName: res.fullName,
                dni: res.dni,
                address: res.address,
                birthDate: res.birthDate,
                phoneNumber: res.phoneNumber,
                email: res.email,
                gender: res.gender,
                belongsToRiskGroup: res.belongsToRiskGroup,
                preferedOfficeId: res.preferedOfficeId
            });

            if (this.user && this.user.role !== 'administrator') {
                this.form.controls['preferedOfficeId'].setValidators([Validators.required]);
            } else {
                this.form.controls['preferedOfficeId'].clearValidators();
            }
            this.form.controls['preferedOfficeId'].updateValueAndValidity();
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
        this.accountService.update(this.accountService.userValue.id, this.form.value)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.alertService.success('Perfil modificado correctamente', { keepAfterRouteChange: true });
                    this.router.navigate(['/'], { relativeTo: this.route });
                },
                error: (error: string) => {
                    this.alertService.error(error);
                    this.loading = false;
                }
            });
    }

    downloadVaccineCertificate(v: AppliedVaccine) {
        Swal
      .fire({
        title: 'Certificado de vacunación',
        text: 'Va a generar el certificado para: ' + v.vaccine.name,
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

    deleteVaccineQuestion(v: AppliedVaccine) {
        Swal
      .fire({
        title: '¿Está seguro?',
        text: 'Va a eliminar la vacuna: ' + v.vaccine.name,
        icon: 'warning',
        showCancelButton: true,
        cancelButtonText: 'No, cancelar',
        confirmButtonText: 'Si, eliminar!'
      })
      .then(result => {
        if (result.value) {
          this.deleteVaccine(v);
          
        }
      });
    }

    downloadCertificate(v: AppliedVaccine) {
        this.vaccineService.downloadCertificate(v)
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

    deleteVaccine(v: AppliedVaccine) {
        this.accountService.deleteVaccine(+this.accountService.userValue.id, v.id)
        .pipe(first())
        .subscribe({
            next: () => {
                Swal.fire('Eliminada!', 'Vacuna eliminada', 'success');
                this.loadData();
            },
            error: (error: string) => {
                this.alertService.error(error);
                this.loading = false;
            }
        });
    }

    addVaccine() {
        this.router.navigate(['account/profile/add-vaccine']);
    }
}