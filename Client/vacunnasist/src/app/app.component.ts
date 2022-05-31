import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CertificateModalComponent } from './account/profile/certificate/certificate-modal.component';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';


@Component({ selector: 'app', templateUrl: 'app.component.html' })
export class AppComponent {
    user?: User;

    constructor(private accountService: AccountService,
        public dialog: MatDialog) {
        this.accountService.user.subscribe(x => this.user = <User>x);
    }

    logout() {
        this.accountService.logout();
    }

    generateCertificate() {
    this.openDialog();
    }

    openDialog(): void {
        const dialogRef = this.dialog.open(CertificateModalComponent, {
          width: '450px',
          height: '400px'
        });
    
        dialogRef.afterClosed().subscribe(result => {
          console.log('The dialog was closed');
        });
      }
}