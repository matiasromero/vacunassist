import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_helpers/auth.guard';

const accountModule = () => import('./account/account.module').then(x => x.AccountModule);
const usersModule = () => import('./users/users.module').then(x => x.UsersModule);
const appointmentsModule = () => import('./appointments/appointments.module').then(x => x.AppointmentsModule);
const vaccinesModule = () => import('./vaccines/vaccines.module').then(x => x.VaccinesModule);

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'account', loadChildren: accountModule },
  { path: 'users', loadChildren: usersModule, canActivate: [AuthGuard] },
  { path: 'appointments', loadChildren: appointmentsModule, canActivate: [AuthGuard] },
  { path: 'vaccines', loadChildren: vaccinesModule, canActivate: [AuthGuard] },
  // otherwise redirect to home
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
