import { formatDate } from '@angular/common';
import { number } from 'echarts';
import { environment } from 'environments/environment.development';

export interface ICRMSingUpResponse {
  isSuccess: boolean;
  data: any;
  totalRecords: number;
  message: string;
  error: any;
}

export const URL_CRM_SIGN_UP = `${environment.apiCrm}api/Auth/LoginClient?authType=Cliente`;
