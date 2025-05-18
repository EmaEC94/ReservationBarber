import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Reservaciones } from '@core/models/reservation';
import { BehaviorSubject, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BarberService {
  private currentReservacionesSubject: BehaviorSubject<Reservaciones>;
  public currentUser: Observable<Reservaciones>;

  private users = [
    {
      id: 1,
      username: 'admin@lorax.com',
      password: 'admin',
      firstName: 'Sarah',
      lastName: 'Smith',
      token: 'admin-token',
    },
  ];

  constructor(private http: HttpClient) {
    this.currentReservacionesSubject = new BehaviorSubject<Reservaciones>(
      JSON.parse(localStorage.getItem('currentUser') || '{}')
    );
    this.currentUser = this.currentReservacionesSubject.asObservable();
  }

  public get currentUserValue(): Reservaciones {
    return this.currentReservacionesSubject.value;
  }

  login(username: string, password: string) {

    const user = this.users.find((u) => u.username === username && u.password === password);

    if (!user) {
      return this.error('Username or password is incorrect');
    } else {
      localStorage.setItem('currentUser', JSON.stringify(user));
     //this.currentReservacionesSubject.next(user);
      return this.ok({
        id: user.id,
        username: user.username,
        firstName: user.firstName,
        lastName: user.lastName,
        token: user.token,
      });
    }
  }
  ok(body?: {
    id: number;
    username: string;
    firstName: string;
    lastName: string;
    token: string;
  }) {
    return of(new HttpResponse({ status: 200, body }));
  }
  error(message: string) {
    return throwError(message);
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.currentReservacionesSubject.next(this.currentUserValue);
    return of({ success: false });
  }
}
