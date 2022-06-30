import { OfficesComponent } from './offices.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatRadioModule } from '@angular/material/radio';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatExpansionModule } from '@angular/material/expansion';
import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { DateAdapter, MatNativeDateModule, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { MY_DATE_FORMATS } from '../account/account.module';
import { OfficesRoutingModule } from './offices-routing.module';
import { NewOfficeComponent } from './new/new-office.component';
import { EditOfficeComponent } from './edit/edit-office.component';



@NgModule({
  declarations: [
    OfficesComponent,
    NewOfficeComponent,
    EditOfficeComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    OfficesRoutingModule,
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
export class OfficesModule { }
