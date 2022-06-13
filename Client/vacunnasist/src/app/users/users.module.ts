import { MatIconModule } from '@angular/material/icon';
import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { UsersComponent } from './users.component';
import { ReactiveFormsModule } from '@angular/forms';
import { UsersRoutingModule } from './users-routing.module';



@NgModule({
  declarations: [
    UsersComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    UsersRoutingModule,
    MatIconModule
  ],
  providers: [
    DatePipe
  ]

})
export class UsersModule { }
