import { AppointmentsComponent } from './appointments.component';
import { MatChipsModule } from '@angular/material/chips';
import { MatAccordion, MatExpansionModule } from '@angular/material/expansion';
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
import { DateAdapter, MatNativeDateModule, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { AppointmentStatusPipe } from '../_helpers/appointment-status.pipe';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { MY_DATE_FORMATS } from '../account/account.module';
import { NewAppointmentAdminComponent } from './new-admin/new-appointment-admin.component';


@NgModule({
  declarations: [
    NewAppointmentComponent,
    MyAppointmentsComponent,
    NewAppointmentAdminComponent,
    AppointmentsComponent,
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
    MatTooltipModule,
    MatExpansionModule,
    MatChipsModule
],
providers: [
  DatePipe,
  { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
  { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS},
  { provide: MAT_DATE_LOCALE, useValue: 'es-AR'},
]
})
export class AppointmentsModule { }
