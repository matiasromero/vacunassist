import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from '../account/layout/layout.component';
import { VaccinesComponent } from './vaccines.component';
import { NewVaccineComponent } from './new/new-vaccine.component';
import { EditVaccineComponent } from './edit/edit-vaccine.component';

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            { path: '', component: VaccinesComponent },
            { path: 'new', component: NewVaccineComponent },
            { path: 'edit/:id', component: EditVaccineComponent },
        ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class VaccinesRoutingModule { }