import { AlertService } from 'src/app/_services/alert.service';
import { ChangePasswordModel } from './../_models/change-password';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';

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

    myProfile() {
        return this.http.get<User>(`${environment.apiUrl}/users/profile`)
        .pipe(
            map((u:any) =>{
                console.log(u);
                return u.user;
            })
            );
    }

    getAll() {
        return this.http.get<User[]>(`${environment.apiUrl}/users`);
    }

    getById(id: string) {
        return this.http.get<User>(`${environment.apiUrl}/users/${id}`);
    }

    update(id: string, params: any) {
        return this.http.put(`${environment.apiUrl}/users/${id}`, params)
            .pipe(map(x => {
                // update stored user if the logged in user updated their own record
                if (id == this.userValue.id) {
                    // update local storage
                    const user = { ...this.userValue, ...params };
                    localStorage.setItem('user', JSON.stringify(user));

                    // publish updated user to subscribers
                    this.userSubject.next(user);
                }
                return x;
            }));
    }

    delete(id: string) {
        return this.http.delete(`${environment.apiUrl}/users/${id}`)
            .pipe(map(x => {
                // auto logout if the logged in user deleted their own record
                if (id == this.userValue.id) {
                    this.logout();
                }
                return x;
            }));
    }
}