import { AccountService } from 'src/app/_services/account.service';
import { ChangePasswordModel } from './../../_models/change-password';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { AlertService } from 'src/app/_services/alert.service';
import { MatDialog } from '@angular/material/dialog';
import { PasswordResetModalComponent } from '../profile/password-reset-modal/password-reset-modal.component';
import { UsersFilter } from 'src/app/_models/filters/users-filter';
import { first, map } from 'rxjs/operators';
import { User } from 'src/app/_models/user';
import Swal from 'sweetalert2';

@Component({ templateUrl: 'password-recovery.component.html' })
export class PasswordRecoveryComponent implements OnInit {
    form!: UntypedFormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: UntypedFormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private alertService: AlertService,
        private accountService: AccountService,
        public dialog: MatDialog
    ) { }

    ngOnInit() {
        this.form = this.formBuilder.group({
            userName: ['', [Validators.required, Validators.maxLength(30)]],
            email: ['', [Validators.required, Validators.email, Validators.maxLength(30)]]
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

        let filter = new UsersFilter();
        filter.userName = this.form.get('userName')?.value;
        filter.email = this.form.get('email')?.value;
        console.log(filter);
        this.accountService.getAll(filter).pipe(
            map((u:any) => u.users)
        ).subscribe({
            next: (res:User[]) => {
                if (res.length == 1 && filter.userName) {
                    this.accountService.resetPassword(filter.userName)
                    .pipe(first())
            .subscribe({
                next: () => {
                    Swal.fire('Email enviado', 'Contraseña modificada, por favor revise su casilla de correo', 'success');
                    //this.openDialog();
                }
            });
                } else {
                    this.alertService.error('Usuario/Email no encontrado');
                }
                this.loading = false;
            },
            error: error => {
                this.alertService.error(error);
                this.loading = false;
            }
        });
    }

    openDialog(): void {
        const dialogRef = this.dialog.open(PasswordResetModalComponent, {
          width: '450px',
          height: '300px',
          data: { email: this.form.get('email')?.value,
                userName: this.form.get('userName')?.value
            }

        });
    
        dialogRef.afterClosed().subscribe(result => {
          console.log('The dialog was closed');
        });
      }
}