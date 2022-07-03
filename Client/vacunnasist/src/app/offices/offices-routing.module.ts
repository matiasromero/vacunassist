import { OfficesComponent } from './offices.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from '../account/layout/layout.component';
import { NewOfficeComponent } from './new/new-office.component';
import { EditOfficeComponent } from './edit/edit-office.component';

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            { path: '', component: OfficesComponent },
            { path: 'new', component: NewOfficeComponent },
            { path: 'edit/:id', component: EditOfficeComponent },
        ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class OfficesRoutingModule { }