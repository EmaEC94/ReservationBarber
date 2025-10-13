import { formatDate } from '@angular/common';
import { number } from 'echarts';
import { environment } from 'environments/environment.development';

export interface ICRMAuthResponse {
  isSuccess: boolean;
  data: any;
  totalRecords: number;
  message: string;
  error: any;
}

export const URL_CRM_LOGIN_AUTH = `${environment.apiCrm}api/Auth/Login?authType=Cliente`;
