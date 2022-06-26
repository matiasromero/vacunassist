import { NewAppointmentAdminComponent } from './../appointments/new-admin/new-appointment-admin.component';
import { AppointmentsFilter } from './../_models/filters/appointments-filter';
import { NewAppointmentModel } from './../_models/new-appointment';
import { Appointment } from './../_models/appointment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { NewConfirmedAppointmentRequest } from '../_models/new-confirmed-appointment';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AppointmentService {
    constructor(
        private http: HttpClient,
    ) {
      
    }

    getAll(filter: AppointmentsFilter) {
        const headers = new HttpHeaders().set(
            'Content-Type',
            'application/json; charset=utf-8'
          );
          
        let params = new HttpParams();
        if (filter.status)
          params = params.append('status', filter.status.toString());
        if (filter.fullName)
          params = params.append('fullName', filter.fullName.toString());
        if (filter.officeId)
          params = params.append('officeId', filter.officeId.toString());
        if (filter.vaccinatorId)
          params = params.append('vaccinatorId', filter.vaccinatorId.toString());
        if (filter.date)
          params = params.append('date', filter.date.toString());
          
        return this.http.get<Appointment[]>(`${environment.apiUrl}/appointments`, 
        {
           headers: headers,
        params: params
    });
    }

    getById(id: number) {
      return this.http.get<Appointment>(`${environment.apiUrl}/appointments/${id}`);
  }

    getByUser() {
        return this.http.get<Appointment[]>(`${environment.apiUrl}/users/my-appointments`);
    }

    newAppointment(model: NewAppointmentModel) {       
        return this.http.post<NewAppointmentModel>(`${environment.apiUrl}/appointments`, model);
    }

    newConfirmedAppointment(model: NewConfirmedAppointmentRequest) {       
      return this.http.post<NewConfirmedAppointmentRequest>(`${environment.apiUrl}/appointments/confirmed`, model);
  }

    cancel(a: Appointment) {
        return this.http.delete(`${environment.apiUrl}/appointments/${a.id}`);
    }

    update(id: number, model: NewConfirmedAppointmentRequest) {
      return this.http.put<NewConfirmedAppointmentRequest>(`${environment.apiUrl}/appointments/${id}`, model);
  }
}