import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { LayoutComponent } from './layout/layout.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AccountRoutingModule } from './account-routing.module';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    LayoutComponent,
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AccountRoutingModule
],
providers: [
  DatePipe
]
})
export class AccountModule { }
