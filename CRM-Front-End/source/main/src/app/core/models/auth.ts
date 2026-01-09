import { environment } from 'environments/environment.prod';

export interface ICRMAuthResponse {
  isSuccess: boolean;
  data: any;
  totalRecords: number;
  message: string;
  error: any;
}

export const URL_CRM_LOGIN_AUTH = `${environment.apiCrm}api/Auth/Login?authType=Cliente`;
export const URL_CRM_SIGN_UP = `${environment.apiCrm}api/Auth/LoginClient?authType=Cliente`;
export const URL_CRM_REGISTER_AUTH = `${environment.apiCrm}api/Client/Register`;
