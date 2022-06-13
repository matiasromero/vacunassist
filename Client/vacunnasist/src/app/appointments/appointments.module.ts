import { MatTooltipModule } from '@angular/material/tooltip';
import { MyAppointmentsComponent } from './my-appointments/my-appointments.component';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { CommonModule, DatePipe } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { NewAppointmentComponent } from './new/new-appointment.component';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { AppointmentsRoutingModule } from './appointments-routing.module';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { AppointmentStatusPipe } from '../_helpers/appointment-status.pipe';


@NgModule({
  declarations: [
    NewAppointmentComponent,
    MyAppointmentsComponent,
    AppointmentStatusPipe
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AppointmentsRoutingModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatNativeDateModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatTooltipModule
],
providers: [
  DatePipe
]
})
export class AppointmentsModule { }
