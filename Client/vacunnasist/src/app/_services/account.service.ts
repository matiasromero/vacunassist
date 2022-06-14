import { AppliedVaccine } from './../_models/applied-vaccine';
import { AlertService } from 'src/app/_services/alert.service';
import { ChangePasswordModel } from './../_models/change-password';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, first } from 'rxjs/operators';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';
import { UsersFilter } from '../_models/filters/users-filter';

@Injectable({ providedIn: 'root' })
export class AccountService {
    private userSubject: BehaviorSubject<User | null>;
    public user: Observable<User | null>;

    constructor(
        private router: Router,
        private http: HttpClient,
        private alertService: AlertService
    ) {
        let storageUser = localStorage.getItem('user');
        this.userSubject = new BehaviorSubject<User | null>(storageUser ? JSON.parse(storageUser) : null);
        this.user = this.userSubject.asObservable();
    }

    public get userValue(): User {
        return <User>this.userSubject.value;
    }

    login(username: string, password: string) {
        return this.http.post<User>(`${environment.apiUrl}/accounts/authenticate`, { username, password })
            .pipe(map(user => {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('user', JSON.stringify(user));
                this.userSubject.next(user);
                return user;
            }));
    }

    logout() {
        // remove user from local storage and set current user to null
        localStorage.removeItem('user');
        this.userSubject.next(null);
        this.router.navigate(['/account/login']);
    }

    register(user: User) {
        return this.http.post(`${environment.apiUrl}/users/register`, user);
    }

    changePassword(credentials: ChangePasswordModel, user: User) {
        return this.http.post<ChangePasswordModel>(`${environment.apiUrl}/users/${user.id}/change-password`, credentials)
            .pipe(map(x => {
                // auto logout if the logged in user deleted their own record
                if (user.id == this.userValue.id) {
                    this.alertService.success('Contraseña cambiada correctamente', { keepAfterRouteChange: true });
                    this.logout();
                }
                return x;
            }));
    }

    resetPassword(userName: string) {
        const headers = new HttpHeaders().set(
            'Content-Type',
            'application/json; charset=utf-8'
          );

        return this.http.post(`${environment.apiUrl}/users/reset-password`, {userName: userName});
    }


    myProfile() {
        return this.http.get<User>(`${environment.apiUrl}/users/profile`)
        .pipe(
            map((u:any) =>{
                return u.user;
            })
            );
    }


    getAll(filter: UsersFilter) {
        const headers = new HttpHeaders().set(
            'Content-Type',
            'application/json; charset=utf-8'
          );
          
          let params = new HttpParams();
          if (filter.isActive !== undefined)
            params = params.append('isActive', filter.isActive.toString());
          if (filter.belongsToRiskGroup !== undefined) {
            params = params.append('belongsToRiskGroup', filter.belongsToRiskGroup!.toString());
          }
          if (filter.role)
            params = params.append('role', filter.role.toString());
            if (filter.userName)
            params = params.append('userName', filter.userName.toString());
            if (filter.email)
            params = params.append('email', filter.email.toString());
            if (filter.fullName)
            params = params.append('fullName', filter.fullName.toString());
          
        return this.http.get<User[]>(`${environment.apiUrl}/users`, 
        {
           headers: headers,
        params: params
    });
    }

    getById(id: number) {
        return this.http.get<User>(`${environment.apiUrl}/users/${id}`);
    }

    canBeDeleted(id: number): Observable<boolean> {
        return this.http.get<boolean>(`${environment.apiUrl}/users/${id}/can-delete`);
    }

    addVaccine(userId: number, appliedVaccine: any) {
        return this.http.post(`${environment.apiUrl}/users/${userId}/add-vaccine`, appliedVaccine);
    }

    deleteVaccine(userId: number, vaccineId: number) {
        return this.http.post(`${environment.apiUrl}/users/${userId}/delete-vaccine`, vaccineId);
    }

    update(id: number, params: any) {
        return this.http.put(`${environment.apiUrl}/users/${id}`, params)
            .pipe(map(x => {
                // update stored user if the logged in user updated their own record
                if (id == +this.userValue.id) {
                    // update local storage
                    const user = { ...this.userValue, ...params };
                    localStorage.setItem('user', JSON.stringify(user));

                    // publish updated user to subscribers
                    this.userSubject.next(user);
                }
                return x;
            }));
    }
}