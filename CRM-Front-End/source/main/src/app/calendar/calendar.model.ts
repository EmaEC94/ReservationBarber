import { formatDate } from '@angular/common';
import { number } from 'echarts';
import { environment } from 'environments/environment.development';
export class Calendar {
  rervationId: string;
  tittle: string;
  note: string;
  message: string | undefined;
  apointment: string;
  userBarberId: number;
  clientId: number;
  price: number;
  payment: string;

  constructor(calendar: Calendar) {
    {
      this.rervationId = calendar.rervationId || this.getRandomID();
      this.tittle = calendar.tittle || '';
      this.apointment = calendar.apointment
        ? new Date(calendar.apointment).toISOString()
        : new Date().toISOString();
      this.note = calendar.note || '';
      this.userBarberId = calendar.userBarberId || 0;
      this.clientId = calendar.clientId || 0;
      this.price = calendar.price || 0;
      this.payment = calendar.payment || '';
    }
  }
  public getRandomID(): string {
    const S4 = () => {
      return ((1 + Math.random()) * 0x10000) | 0;
    };
    return (S4() + S4()).toString(); // Convert to string
  }
}

export interface ICRMCalendarResponse {
  isSuccess: boolean;
  data: Calendar[];
  totalRecords: number;
  message: string;
  error: any;
}

export interface ICRMBarber {
  id: number;
  description: string;
}

export interface ICRMBarberFreeTime {
  availableHour: Date;
}
export interface ICRMBarberResponse {
  isSuccess: boolean;
  data: any;
  totalRecords: number;
  message: string;
  error: any;
}

export interface ICRMServicios {
  code: string;
  nomCorte: string;
  desCorte: string;
  precio: number;
}
export const es = {
  code: 'es',
  week: {
    dow: 1,
    doy: 4,
  },
  buttonText: {
    prev: 'Ant',
    next: 'Sig',
    today: 'Hoy',
    month: 'Mes',
    week: 'Semana',
    day: 'Día',
    list: 'Agenda',
  },
  weekText: 'Sm',
  allDayText: 'Todo el día',
  moreLinkText: 'más',
  noEventsText: 'No hay eventos para mostrar',
};

export const URL_CRM_RESERVATION = `${environment.apiCrm}api/Reservation?NumRecordsPage=10000`;
export const URL_CRM_REGISTER_RESERVATION = `${environment.apiCrm}api/Reservation/Register`;
export const URL_CRM_GET_ALL_BARBER = `${environment.apiCrm}api/User/Select`;
export const URL_CRM_GET_ALL_FREE_TIME_BARBER = `${environment.apiCrm}api/Reservation/ReservationAvaibleRequest`;
