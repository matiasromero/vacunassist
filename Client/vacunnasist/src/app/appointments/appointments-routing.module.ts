import { AppointmentsComponent } from './appointments.component';
import { MyAppointmentsComponent } from './my-appointments/my-appointments.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from '../account/layout/layout.component';
import { NewAppointmentComponent } from './new/new-appointment.component';
import { NewAppointmentAdminComponent } from './new-admin/new-appointment-admin.component';

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            {path: '', component:AppointmentsComponent},
            { path: 'new-appointment', component: NewAppointmentComponent },
            { path: 'my-appointments', component: MyAppointmentsComponent },
            { path: 'new', component: NewAppointmentAdminComponent },
        ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AppointmentsRoutingModule { }