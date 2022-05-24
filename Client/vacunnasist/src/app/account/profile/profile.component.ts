import { AppliedVaccine } from './../../_models/applied-vaccine';
import { Vaccine } from './../../_models/vaccine';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
    form!: FormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private accountService: AccountService,
        private officesService: OfficeService,
        private alertService: AlertService,
        private dp: DatePipe
    ) { 
    }

    public offices: Office[] = [];
    public user: User = new User;
    ngOnInit() {
        this.officesService.getAll().subscribe((res: any) => {
            this.offices = res.offices;
        });

        this.accountService.myProfile().subscribe(res => {
            this.user = res;
            this.form.patchValue({
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
    });
        
        this.form = this.formBuilder.group({
            fullName: ['', [Validators.required, Validators.maxLength(100)]],
            dni: ['', [Validators.required, Validators.maxLength(20)]],
            address: ['', [Validators.required, Validators.maxLength(200)]],
            birthDate: [new Date(), Validators.required],
            phoneNumber: ['', [Validators.required, Validators.maxLength(30)]],
            email:['', [Validators.required, Validators.email, Validators.maxLength(30)]],
            gender: ['male', Validators.required],
            belongsToRiskGroup: [false, Validators.required],
            preferedOfficeId: [null, Validators.required]
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
                error: error => {
                    this.alertService.error(error);
                    this.loading = false;
                }
            });
    }

    deleteVaccineQuestion(v: AppliedVaccine) {
        console.log(v);
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
          Swal.fire('Eliminada!', 'Vacuna eliminada', 'success');
        }
      });
    }

    deleteVaccine(v: any) {
        const index = this.user.vaccines.indexOf(v, 0);
        if (index > -1) {
           this.user.vaccines.splice(index, 1);
        }
    }

    addVaccine() {
        console.log('a');
        this.router.navigate(['account/profile/add-vaccine']);
    }
}