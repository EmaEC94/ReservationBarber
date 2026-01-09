import { formatDate } from '@angular/common';
import { number } from 'echarts';
import { environment } from 'environments/environment.prod';
export class Calendar {
  rervationId: string;
  title: string;
  note: string;
  message: string | undefined;
  startDate!: Date | string;
  endDate?: Date | string | null;
  userBarberId: number;
  clientId: number;
  price: number;
  payment: string;
  subCatalogId: number;
  selectedFreeTime: string;

  constructor(calendar: Calendar) {
    {
      this.rervationId = calendar.rervationId || this.getRandomID();
      this.title = calendar.title || '';
      this.startDate = calendar.startDate ? new Date(calendar.startDate) : new Date();
      this.endDate = calendar.endDate ? new Date(calendar.endDate) : new Date();
      this.note = calendar.note || '';
      this.userBarberId = calendar.userBarberId || 0;
      this.clientId = calendar.clientId || 0;
      this.price = calendar.price || 0;
      this.payment = calendar.payment || '';
      this.subCatalogId = calendar.subCatalogId || 0;
      this.selectedFreeTime = calendar.selectedFreeTime || '';
    }
  }
  public getRandomID(): string {
    const S4 = () => {
      return ((1 + Math.random()) * 0x10000) | 0;
    };
    return (S4() + S4()).toString(); // Convert to string
  }
}

export interface ICRMServicios {
  subCatalogId: number;
  catalogId: number;
  name: string;
  code: string;
  price: number;
  description: string;
  state: number;
  stateSubCatalog: string;
  duration: string; 
}

export interface ICRMCalendarResponse {
  isSuccess: boolean;
  data: Calendar[];
  totalRecords: number;
  message: string;
  error: any;
}
export interface ICRMReservationResponse {
  isSuccess: boolean;
  data: Calendar;
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
export interface ICRMResponse {
  isSuccess: boolean;
  data: any;
  totalRecords: number;
  message: string;
  error: any;
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

export const URL_CRM_RESERVATION = `${environment.apiCrm}api/Reservation`; //?NumRecordsPage=10000
export const URL_CRM_REGISTER_RESERVATION = `${environment.apiCrm}api/Reservation/Register`;
export const URL_CRM_GET_ALL_BARBER = `${environment.apiCrm}api/User/Select`;
export const URL_CRM_GET_ALL_FREE_TIME_BARBER = `${environment.apiCrm}api/Reservation/ReservationAvaibleRequest`;
export const URL_CRM_GET_ALL_RESERVATION_BY_ID = `${environment.apiCrm}api/Reservation`;
export const URL_CRM_GET_ALL_SERVICES = `${environment.apiCrm}api/SubCatalog`;
