import { NotifyComponent } from './notify/notify.component';
import { EditUserComponent } from './edit/edit-user.component';
import { NewUserComponent } from './new/new-user.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from '../account/layout/layout.component';
import { UsersComponent } from './users.component';

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            { path: '', component: UsersComponent },
            { path: 'new', component: NewUserComponent },
            { path: 'edit/:id', component: EditUserComponent },
            { path: 'notify', component: NotifyComponent },
        ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UsersRoutingModule { }