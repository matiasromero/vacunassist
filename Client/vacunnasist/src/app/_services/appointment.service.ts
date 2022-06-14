import { AppointmentsFilter } from './../_models/filters/appointments-filter';
import { NewAppointmentModel } from './../_models/new-appointment';
import { Appointment } from './../_models/appointment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';

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
        console.log(filter);
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

    getByUser() {
        return this.http.get<Appointment[]>(`${environment.apiUrl}/users/my-appointments`);
    }

    newAppointment(model: NewAppointmentModel) {       
        return this.http.post<NewAppointmentModel>(`${environment.apiUrl}/appointments`, model);
    }

    cancel(a: Appointment) {
        return this.http.delete(`${environment.apiUrl}/appointments/${a.id}`);
    }
}