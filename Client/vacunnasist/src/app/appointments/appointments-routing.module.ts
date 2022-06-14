import { AppointmentsComponent } from './appointments.component';
import { MyAppointmentsComponent } from './my-appointments/my-appointments.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from '../account/layout/layout.component';
import { NewAppointmentComponent } from './new/new-appointment.component';

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            {path: '', component:AppointmentsComponent},
            { path: 'new-appointment', component: NewAppointmentComponent },
            { path: 'my-appointments', component: MyAppointmentsComponent },
        ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AppointmentsRoutingModule { }