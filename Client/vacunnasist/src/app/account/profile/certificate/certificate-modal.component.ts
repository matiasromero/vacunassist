import { AccountService } from 'src/app/_services/account.service';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { User } from 'src/app/_models/user';


@Component({ templateUrl: 'certificate-modal.component.html' })
export class CertificateModalComponent implements OnInit {
  date: Date = new Date();
  
    constructor(
      public dialogRef: MatDialogRef<CertificateModalComponent>,
      public accountService: AccountService
    ) {
    }

    public user: User = new User();
  ngOnInit() {
    this.accountService.myProfile().subscribe(res => {
      this.user = res;
      });
  }
  
    onNoClick(): void {
      this.dialogRef.close();
    }

    print() {
      this.date = new Date();
      var divToPrint = document.getElementById('certificate');
      var newWin = window.open('', 'Print-Window');
      newWin?.document.open();
      newWin?.document.write('<html><link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.3.1/css/bootstrap.min.css" media="print"/><body onload="window.print()">' + divToPrint?.innerHTML + '</body></html>');
      newWin?.document.close();
      setTimeout(function() {
        newWin?.close();
      }, 10);
  }
}