import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpErrorResponse,
  HttpResponse,
} from '@angular/common/http';
import { BehaviorSubject, catchError, Observable, of, throwError } from 'rxjs';
import { User } from '../models/user';
import { jwtDecode } from 'jwt-decode';
import {
  ICRMAuthResponse,
  URL_CRM_LOGIN_AUTH,
  URL_CRM_REGISTER_AUTH,
} from '@core/models/auth';
export interface ClientToken {
  nameid: string; // Email
  family_name: string; // Nombre de usuario
  given_name: string; // Email otra vez
  unique_name: string; // Id del cliente
  jti: string;
  iat: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  private users = [
    {
      id: 1,
      username: 'barberDestiny@gmail.com',
      password: 'admin',
      firstName: 'Barber Admin',
      lastName: 'Destiny',
      token: 'admin-token',
    },
  ];

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(
      JSON.parse(localStorage.getItem('currentUser') || '{}')
    );
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  login(username: string, password: string) {
    const user = this.users.find(
      (u) => u.username === username && u.password === password
    );

    if (!user) {
      return this.error('Username or password is incorrect');
    } else {
      localStorage.setItem('currentUser', JSON.stringify(user));
      this.currentUserSubject.next(user);
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
    localStorage.removeItem('token');
    this.currentUserSubject.next(this.currentUserValue);
    return of({ success: false });
  }

  checkAuth(tokenRequestDto: any): Observable<ICRMAuthResponse> {
    return this.http
      .post<ICRMAuthResponse>(URL_CRM_LOGIN_AUTH, tokenRequestDto)
      .pipe(catchError(this.errorHandler));
  }

  registerClient(tokenRequestDto: any): Observable<ICRMAuthResponse> {
    return this.http
      .post<ICRMAuthResponse>(URL_CRM_REGISTER_AUTH, tokenRequestDto)
      .pipe(catchError(this.errorHandler));
  }

  private errorHandler(error: HttpErrorResponse): Observable<never> {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.error(errorMessage);
    return throwError(() => new Error(errorMessage));
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  setToken(token: string): void {
    localStorage.setItem('token', token);
  }

  clearToken(): void {
    localStorage.removeItem('token');
  }

  getClientData(): ClientToken | null {
    const token = this.getToken();
    if (!token) return null;
    try {
      return jwtDecode<ClientToken>(token);
    } catch (error) {
      console.error('Error al decodificar token:', error);
      return null;
    }
  }

  getClientId(): number | null {
    return this.getClientData()
      ? Number(this.getClientData()!.unique_name)
      : null;
  }

  getClientEmail(): string | null {
    return this.getClientData()?.nameid ?? null;
  }

  getClientName(): string | null {
    return this.getClientData()?.family_name ?? null;
  }
}
