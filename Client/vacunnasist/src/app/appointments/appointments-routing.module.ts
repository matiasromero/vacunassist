import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from '../account/layout/layout.component';
import { NewAppointmentComponent as NewAppointmentComponent } from './new/new-appointment.component';

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            { path: 'new-appointment', component: NewAppointmentComponent },
        ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AppointmentsRoutingModule { }