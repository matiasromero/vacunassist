import { AppliedVaccine } from './../_models/applied-vaccine';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Vaccine } from '../_models/vaccine';
import { DownloadCertificateModel } from '../_models/download-certificate';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class VaccineService {
    constructor(
        private http: HttpClient,
    ) {
      
    }

    getAll() {
        return this.http.get<Vaccine[]>(`${environment.apiUrl}/vaccines`);
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
}