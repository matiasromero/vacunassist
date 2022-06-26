import { MatTabsModule } from '@angular/material/tabs';
import { EditUserComponent } from './edit/edit-user.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { NewUserComponent } from './new/new-user.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatRadioModule } from '@angular/material/radio';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatExpansionModule } from '@angular/material/expansion';
import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { UsersComponent } from './users.component';
import { ReactiveFormsModule } from '@angular/forms';
import { UsersRoutingModule } from './users-routing.module';
import { DateAdapter, MatNativeDateModule, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { MY_DATE_FORMATS } from '../account/account.module';



@NgModule({
  declarations: [
    UsersComponent,
    NewUserComponent,
    EditUserComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    UsersRoutingModule,
    MatIconModule,
    MatRadioModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatExpansionModule,
    MatButtonToggleModule,
    MatChipsModule,
    MatTooltipModule,
    MatNativeDateModule,
    MatInputModule,
    MatTabsModule
  ],
  providers: [
    DatePipe,
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
  { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS},
  { provide: MAT_DATE_LOCALE, useValue: 'es-AR'},
  ]

})
export class UsersModule { }
