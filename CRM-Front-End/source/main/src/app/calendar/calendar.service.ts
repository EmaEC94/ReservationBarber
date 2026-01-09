import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import {
  Calendar,
  ICRMBarber,
  ICRMResponse,
  ICRMCalendarResponse,
  ICRMServicios,
  URL_CRM_GET_ALL_BARBER,
  URL_CRM_GET_ALL_FREE_TIME_BARBER,
  URL_CRM_REGISTER_RESERVATION,
  URL_CRM_RESERVATION,
  URL_CRM_GET_ALL_RESERVATION_BY_ID,
  ICRMReservationResponse,
  URL_CRM_GET_ALL_SERVICES,
} from './calendar.model';
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse,
} from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { EventInput } from '@fullcalendar/core';
import { index } from 'd3';
import { number } from 'echarts';

@Injectable({
  providedIn: 'root',
})
export class CalendarService {
  private readonly API_URL = 'assets/data/calendar.json';
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  dataChange: BehaviorSubject<Calendar[]> = new BehaviorSubject<Calendar[]>([]);
  dialogData!: Calendar;
  reservationCalendar!: Calendar;
  barberList: ICRMBarber[] = [];
  ReservatioCalendarData!: ICRMCalendarResponse;

  constructor(private httpClient: HttpClient) {}

  get data(): Calendar[] {
    return this.dataChange.value;
  }

  getDialogData(): Calendar {
    return this.dialogData;
  }

  async getBarber(): Promise<ICRMResponse> {
    const response = await fetch(URL_CRM_GET_ALL_BARBER);

    if (!response.ok) {
      throw new Error(`Error ${response.status}: ${response.statusText}`);
    }
    const barberListResult = await response.json();
    this.barberList = barberListResult;

    return new Promise((resolve, reject) => {
      if (resolve) {
        resolve(barberListResult);
      } else {
        reject(-1);
      }
    });
  }
  async getBarberFreeTime(
    daySelected: Date,
    userId: number
  ): Promise<ICRMResponse> {
    
    const body = {
      daySelected: daySelected.toISOString().split('T')[0], // formato YYYY-MM-DD
      userId: userId,
    };

    const response = await fetch(URL_CRM_GET_ALL_FREE_TIME_BARBER, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(body),
    });

    if (!response.ok) {
      throw new Error(`Error ${response.status}: ${response.statusText}`);
    }

    const barberListResult = await response.json();
    this.barberList = barberListResult;

    return new Promise((resolve, reject) => {
      if (resolve) {
        resolve(barberListResult);
      } else {
        reject(-1);
      }
    });
  }

  async getReservation(parameter: any): Promise<ICRMCalendarResponse> {
    const queryString = new URLSearchParams(parameter).toString();
    const response = await fetch(`${URL_CRM_RESERVATION}?${queryString}`);

    if (!response.ok) {
      throw new Error(`Error ${response.status}: ${response.statusText}`);
    }

    const events = await response.json();
    this.reservationCalendar = events;

    return events;
  }

 /* async loadEvents(
    ReservatioCalendarData: ICRMCalendarResponse
  ): Promise<EventInput[]> {
 
    let resevationEvents: EventInput[] = [];
    let reservationEvent!: EventInput;

    ReservatioCalendarData.data.forEach((event: Calendar) => {
      reservationEvent = {
        id: event.rervationId,
        title: event.title,
        start: new Date(event.startDate), // Make sure to parse the date string
        end: new Date(event.endDate), // Make sure to parse the date string
        className: event.message,
        groupId: '1',
        details: event.note,
        allDay: false, // Default to false if not provided
      };

      resevationEvents.push(reservationEvent);
    });

    return resevationEvents;
  }*/

async loadEvents(
  ReservatioCalendarData: ICRMCalendarResponse
): Promise<EventInput[]> {

  return ReservatioCalendarData.data.map((event: Calendar) => {

    const start =
      event.startDate instanceof Date
        ? event.startDate
        : new Date(event.startDate.toString().replace('Z', ''));

    const end =
      event.endDate
        ? event.endDate instanceof Date
          ? event.endDate
          : new Date(event.endDate.toString().replace('Z', ''))
        : undefined;

    return {
      id: event.rervationId,
      title: event.title,
      start: start,
      end: end,
      className: event.message,
      groupId: '1',
      details: event.note,
      allDay: false,
    } as EventInput;
  });
}


  /** POST: Add a new calendar */
  addCalendar(calendar: Calendar): Observable<Calendar> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.httpClient
      .post<Calendar>(URL_CRM_REGISTER_RESERVATION, calendar, { headers })
      .pipe(
        map((response) => response),
        catchError((error: HttpErrorResponse) => {
          /*console.error(' Error al agregar calendario:', {
          status: error.status,
          statusText: error.statusText,
          message: error.message,
          error: error.error, // Aquí viene el mensaje real del backend
          url: error.url
        });*/

          // Puedes mostrar un mensaje más claro al usuario si querés
          return throwError(
            () =>
              new Error(
                `Error ${error.status}: ${error.statusText} - ${
                  error.error?.message || error.message
                }`
              )
          );
        })
      );
  }

  /** PUT: Update an existing calendar */
  updateCalendar(calendar: Calendar): Observable<Calendar> {
    return this.httpClient
      .put<Calendar>(`${URL_CRM_REGISTER_RESERVATION}`, calendar)
      .pipe(
        map((response) => {
          return calendar; // return response from API
        }),
        catchError(this.errorHandler)
      );
  }

  /** DELETE: Remove a calendar by ID */
  deleteCalendar(id: number): Observable<number> {
    return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
      map((response) => {
        return id; // return the ID of the deleted doctor
      }),
      catchError(this.errorHandler)
    );
  }

  async getReservationById(
    reservationID: number
  ): Promise<ICRMReservationResponse> {
    const response = await fetch(
      `${URL_CRM_GET_ALL_RESERVATION_BY_ID}/${reservationID} `
    );

    if (!response.ok) {
      throw new Error(`Error ${response.status}: ${response.statusText}`);
    }

    const reservation = await response.json();

    return new Promise((resolve, reject) => {
      if (resolve) {
        resolve(reservation);
      } else {
        reject(-1);
      }
    });
  }

  async getAllServiceDB(): Promise<ICRMResponse> {
    const response = await fetch(`${URL_CRM_GET_ALL_SERVICES}`);

    if (!response.ok) {
      throw new Error(`Error ${response.status}: ${response.statusText}`);
    }

    const servicesDB = await response.json();

    return new Promise((resolve, reject) => {
      if (resolve) {
        resolve(servicesDB);
      } else {
        reject(-1);
      }
    });
  }

  async getServiceById(reservationID: number): Promise<ICRMResponse> {
    const response = await fetch(
      `${URL_CRM_GET_ALL_SERVICES}/${reservationID}`
    );

    if (!response.ok) {
      throw new Error(`Error ${response.status}: ${response.statusText}`);
    }

    const servicesDB = await response.json();

    return new Promise((resolve, reject) => {
      if (resolve) {
        resolve(servicesDB);
      } else {
        reject(-1);
      }
    });
  }

  getAllServices(): Observable<ICRMServicios[]> {
    return this.httpClient
      .get<ICRMServicios[]>(this.API_URL)
      .pipe(catchError(this.errorHandler));
  }

  /** Error Handler */
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
}
