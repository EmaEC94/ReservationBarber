import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { Calendar, ICRMBarber, ICRMBarberResponse, ICRMCalendarResponse, ICRMServicios, URL_CRM_GET_ALL_BARBER, URL_CRM_GET_ALL_FREE_TIME_BARBER, URL_CRM_REGISTER_RESERVATION, URL_CRM_RESERVATION } from './calendar.model';
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
  barberList:ICRMBarber[] = [];
  ReservatioCalendarData!: ICRMCalendarResponse;



  constructor(private httpClient: HttpClient) {}

  get data(): Calendar[] {
    return this.dataChange.value;
  }

  getDialogData(): Calendar {
    return this.dialogData;
  }

  /** CRUD Methods */
  /** GET: Fetch all barber of Destiny */
  // getReservation(): r<Calendar[]> {
  //   return this.httpClient
  //     .get<Calendar[]>(this.API_URL)
  //     .pipe(catchError(this.errorHandler));
  // }

  
  async getBarber(): Promise<ICRMBarberResponse>{
      
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
  async getBarberFreeTime(daySelected: Date, userId: number): Promise<ICRMBarberResponse>{
      
    const body = {
        daySelected: daySelected.toISOString().split('T')[0], // formato YYYY-MM-DD
        userId: userId
      };

      const response = await fetch(URL_CRM_GET_ALL_FREE_TIME_BARBER, {
        method: 'POST', // o 'PUT' si aplica
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(body)
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
  async getReservation(): Promise<ICRMCalendarResponse>{
      
      const response = await fetch(URL_CRM_RESERVATION);
  
      if (!response.ok) {
        throw new Error(`Error ${response.status}: ${response.statusText}`);
      }
      
      const events = await response.json();
      this.reservationCalendar = events;
 
      return new Promise((resolve, reject) => {
        if (resolve) {       
           resolve(events);
        } else {
           reject(-1);
        }
     });
  }

  async loadEvents(ReservatioCalendarData: ICRMCalendarResponse): Promise<EventInput[]> {
   /* const response = await fetch(this.API_URL);
    const events = await response.json();
   */
    let resevationEvents: EventInput[] = [];
    let reservationEvent!: EventInput;
   
    ReservatioCalendarData.data.forEach((event:Calendar) => {
    reservationEvent = {
      id: event.rervationId,
      title: event.tittle,
      start: new Date(event.apointment), // Make sure to parse the date string
      end: new Date(event.apointment), // Make sure to parse the date string
      className: event.message,
      groupId: '1',
      details: event.note,
      allDay: false, // Default to false if not provided
    };

      resevationEvents.push(reservationEvent);

    });
    
    return resevationEvents;
  }

  /** POST: Add a new calendar */
  addCalendar(calendar: Calendar): Observable<Calendar> {


    return this.httpClient.post<Calendar>(URL_CRM_REGISTER_RESERVATION,calendar).pipe(
      map((response) => {
        return calendar; // return response from API
      }),
      catchError(this.errorHandler)
    );
  }

  /** PUT: Update an existing calendar */
  updateCalendar(calendar: Calendar): Observable<Calendar> {
    return this.httpClient.put<Calendar>(`${URL_CRM_REGISTER_RESERVATION}`, calendar).pipe(
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

  /*getAllServices(): Observable<ICRMServicios[]>  {
    return this.httpClient.get<ICRMServicios[]>(`${this.API_URL}`).pipe(
      map((response) => {
        return response; // return the ID of the deleted doctor
      }),
      catchError(this.errorHandler)
    );
  }*/

  getAllServices(): Observable<ICRMServicios[]> {
  return this.httpClient.get<ICRMServicios[]>(this.API_URL)
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
