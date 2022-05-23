import { FormGroup } from '@angular/forms';
    
export function DistinctValidator(controlName: string, matchingControlName: string){
    return (formGroup: FormGroup) => {
        const control = formGroup.controls[controlName];
        const matchingControl = formGroup.controls[matchingControlName];
        if (matchingControl.errors && !matchingControl.errors.distinctValidator) {
            return;
        }
        if (control.value !== matchingControl.value) {
            matchingControl.setErrors(null);
        } else {
            matchingControl.setErrors({ distinctValidator: true });
        }
    }
}