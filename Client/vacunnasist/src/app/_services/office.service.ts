import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Office } from '../_models/office';

@Injectable({ providedIn: 'root' })
export class OfficeService {
    constructor(
        private http: HttpClient,
    ) {
      
    }

    getAll() {
        return this.http.get<Office[]>(`${environment.apiUrl}/offices`);
    }
}