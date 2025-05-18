import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { Calendar, ICRMCalendarResponse, URL_CRM_REGISTER_RESERVATION, URL_CRM_RESERVATION } from './calendar.model';
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
  ReservatioCalendarData!: ICRMCalendarResponse;



  constructor(private httpClient: HttpClient) {}

  get data(): Calendar[] {
    return this.dataChange.value;
  }

  getDialogData(): Calendar {
    return this.dialogData;
  }

  /** CRUD Methods */

  /** GET: Fetch all calendars */
  // getReservation(): r<Calendar[]> {
  //   return this.httpClient
  //     .get<Calendar[]>(this.API_URL)
  //     .pipe(catchError(this.errorHandler));
  // }

 
  async getReservation(): Promise<ICRMCalendarResponse>{
      
      const response = await fetch(URL_CRM_RESERVATION);
  
      if (!response.ok) {
        throw new Error(`Error ${response.status}: ${response.statusText}`);
      }
      
      const events = await response.json();
      this.reservationCalendar = events;
 
      return new Promise((resolve, reject) => {
        if (resolve) {       
        /*events.map((event: any): Calendar => ({
            ...event
        }));*/
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

    /*
    const response = await fetch(URL_CRM_RESERVATION);
  
    if (!response.ok) {
      throw new Error(`Error ${response.status}: ${response.statusText}`);
    }
    
    const events = await response.json();
    this.reservationCalendar = events;
    const eventReservation:any = null;
    return new Promise((resolve, reject) => {
      if (resolve) {       
      /*events.forEach((event: ICRMCalendarResponse, index:number ): any => ({

         eventReservation = [{
          id: event.data[index].rervationId,
          title: event.data[index].rervationId,
          start: new Date(event.data[index].Apointment), // Make sure to parse the date string
          end: new Date(event.data[index].Apointment+1), // Make sure to parse the date string
          className: event.data[index].Tittle,
          groupId: event.data[index].ClientId,
          details: event.data[index].Message,
          allDay: false // Default to false if not provided
      }]


      }));


  const eventReservations = events.map((event: ICRMCalendarResponse) => ({
    
      id: event.data[0].rervationId,
      title: event.data[0].Tittle,
      start: new Date(event.data[0].Apointment),
      end: new Date(new Date(event.data[0].Apointment).getTime() + 60 * 60 * 1000), 
      className: event.data[0].Message,
      groupId: event.data[0].ClientId,
      details: event.data[0].Note,
      allDay: false
    }));
      

         resolve(eventReservations);
      } else {
         reject(-1);
      }
   });*/
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
