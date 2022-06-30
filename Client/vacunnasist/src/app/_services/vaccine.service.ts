import { UpdateVaccineRequest } from './../_models/update-vaccine';
import { AppliedVaccine } from './../_models/applied-vaccine';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Vaccine } from '../_models/vaccine';
import { DownloadCertificateModel } from '../_models/download-certificate';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Appointment } from '../_models/appointment';
import { VaccinesFilter } from '../_models/filters/vaccines-filter';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class VaccineService {
    constructor(
        private http: HttpClient,
    ) {
      
    }

    getAll(filter: VaccinesFilter) {
        const headers = new HttpHeaders().set(
            'Content-Type',
            'application/json; charset=utf-8'
          );
          
        let params = new HttpParams();
        if (filter.isActive !== undefined)
            params = params.append('isActive', filter.isActive.toString());
          if (filter.canBeRequested !== undefined && filter.canBeRequested !== null) {
            console.log(filter.canBeRequested);
            params = params.append('canBeRequested', filter.canBeRequested!.toString());
          }
          if (filter.name)
            params = params.append('name', filter.name.toString());
          return this.http.get<Vaccine[]>(`${environment.apiUrl}/vaccines`, 
          {
             headers: headers,
          params: params
      });
    }

    downloadCertificate(appliedVaccine: AppliedVaccine) {
        const headers = new HttpHeaders().set(
            'Content-Type',
            'application/json; charset=utf-8'
          );

          let model = new DownloadCertificateModel();
          model.id = appliedVaccine.id;
        return this.http.post<DownloadCertificateModel>(`${environment.apiUrl}/users/generate-certificate`, model);
    }

    downloadCertificateAppointment(appointment: Appointment) {
        const headers = new HttpHeaders().set(
            'Content-Type',
            'application/json; charset=utf-8'
          );

          let model = new DownloadCertificateModel();
          model.id = appointment.id;
        return this.http.post<DownloadCertificateModel>(`${environment.apiUrl}/users/generate-certificate-appointment`, model);
    }

    getById(id: number) {
        return this.http.get<Vaccine>(`${environment.apiUrl}/vaccines/${id}`);
    }

    canBeDeleted(id: number): Observable<boolean> {
        return this.http.get<boolean>(`${environment.apiUrl}/vaccines/${id}/can-delete`);
    }

    newVaccine(vaccine: Vaccine) {
        return this.http.post(`${environment.apiUrl}/vaccines`, vaccine);
    }

    update(id: number, model: Vaccine) {
        return this.http.put<UpdateVaccineRequest>(`${environment.apiUrl}/vaccines/${id}`, model);
    }

    cancel(id: number) {
        return this.http.delete(`${environment.apiUrl}/vaccines/${id}`);
    }
}