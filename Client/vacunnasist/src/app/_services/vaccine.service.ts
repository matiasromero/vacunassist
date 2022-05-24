import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Vaccine } from '../_models/vaccine';

@Injectable({ providedIn: 'root' })
export class VaccineService {
    constructor(
        private http: HttpClient,
    ) {
      
    }

    getAll() {
        return this.http.get<Vaccine[]>(`${environment.apiUrl}/vaccines`);
    }
}