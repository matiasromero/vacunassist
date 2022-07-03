import { OfficesFilter } from './../_models/filters/offices-filter';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Office } from '../_models/office';
import { Observable } from 'rxjs';
import { UpdateOfficeRequest } from '../_models/update-office';

@Injectable({ providedIn: 'root' })
export class OfficeService {
    constructor(
        private http: HttpClient,
    ) {
      
    }

    getAll(filter: OfficesFilter) {
        const headers = new HttpHeaders().set(
            'Content-Type',
            'application/json; charset=utf-8'
          );
          
        let params = new HttpParams();
        if (filter.isActive !== undefined)
            params = params.append('isActive', filter.isActive.toString());
          if (filter.name)
            params = params.append('name', filter.name.toString());
          return this.http.get<Office[]>(`${environment.apiUrl}/offices`, 
          {
             headers: headers,
          params: params
      });
    }

    getById(id: number) {
        return this.http.get<Office>(`${environment.apiUrl}/offices/${id}`);
    }

    canBeDeleted(id: number): Observable<boolean> {
        return this.http.get<boolean>(`${environment.apiUrl}/offices/${id}/can-delete`);
    }

    newOffice(office: Office) {
        return this.http.post(`${environment.apiUrl}/offices`, office);
    }

    update(id: number, model: Office) {
        return this.http.put<UpdateOfficeRequest>(`${environment.apiUrl}/offices/${id}`, model);
    }

    cancel(id: number) {
        return this.http.delete(`${environment.apiUrl}/offices/${id}`);
    }
}