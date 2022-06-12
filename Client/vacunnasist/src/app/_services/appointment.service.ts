import { NewAppointmentModel } from './../_models/new-appointment';
import { Appointment } from './../_models/appointment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class AppointmentService {
    constructor(
        private http: HttpClient,
    ) {
      
    }

    getAll() {
        return this.http.get<Appointment[]>(`${environment.apiUrl}/appointments`);
    }

    newAppointment(model: NewAppointmentModel) {       
        return this.http.post<NewAppointmentModel>(`${environment.apiUrl}/appointments`, model);
    }
}