import { Pipe, PipeTransform } from '@angular/core';
/*
 * Transforms user role to spanish text
 * Usage:
 *   user.role | userrole
 */
@Pipe({ name: 'appointmentstatus' })
export class AppointmentStatusPipe implements PipeTransform {
  transform(value: string): string {
    if (value == '0') {
      return 'Pendiente';
    }
    if (value == '1') {
      return 'Confirmado';
    }

    if (value == '2') {
        return 'Aplicada';
      }

    return 'Cancelado';
  }
}
