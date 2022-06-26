import { CompleteAppointmentComponent } from './complete-appointment/complete-appointment.component';
import { EditAppointmentComponent } from './edit-appointment/edit-appointment.component';
import { AppointmentsComponent } from './appointments.component';
import { MyAppointmentsComponent } from './my-appointments/my-appointments.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from '../account/layout/layout.component';
import { NewAppointmentComponent } from './new/new-appointment.component';
import { NewAppointmentAdminComponent } from './new-admin/new-appointment-admin.component';
import { ConfirmAppointmentComponent } from './confirm/confirm-appointment.component';

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            {path: '', component:AppointmentsComponent},
            { path: 'new-appointment', component: NewAppointmentComponent },
            { path: 'my-appointments', component: MyAppointmentsComponent },
            { path: 'new', component: NewAppointmentAdminComponent },
            { path: 'confirm/:id', component: ConfirmAppointmentComponent },
            { path: 'edit/:id', component: EditAppointmentComponent },
            { path: 'complete/:id', component: CompleteAppointmentComponent },
        ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AppointmentsRoutingModule { }