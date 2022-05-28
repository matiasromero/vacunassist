import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';


@Component({ templateUrl: 'password-reset-modal.component.html' })
export class PasswordResetModalComponent {
    constructor(
      public dialogRef: MatDialogRef<PasswordResetModalComponent>,
      @Inject(MAT_DIALOG_DATA) public data: {email: string}
    ) {
    }
  
    onNoClick(): void {
      this.dialogRef.close();
    }
  }