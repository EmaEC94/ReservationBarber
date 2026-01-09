import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';

import {
  UntypedFormControl,
  Validators,
  UntypedFormGroup,
  UntypedFormBuilder,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';

import {
  OwlDateTimeModule,
  OwlNativeDateTimeModule,
} from '@danielmoncada/angular-datetime-picker';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import * as moment from 'moment-timezone';
import { AuthService } from '@core/service/auth.service';
import {
  Calendar,
  ICRMBarber,
  ICRMServicios,
} from 'app/calendar/calendar.model';
import { CalendarService } from 'app/calendar/calendar.service';

export interface DialogData {
  id: number;
  action: string;
  calendar: Calendar;
  barberList: ICRMBarber[];
}

@Component({
  selector: 'app-form-dialog',
  templateUrl: './form-dialog.component.html',
  styleUrls: ['./form-dialog.component.scss'],
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatDialogContent,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule,
    OwlDateTimeModule,
    OwlNativeDateTimeModule,
    MatDialogClose,
  ],
})
export class FormDialogComponent {
  action: string;
  dialogTitle: string;
  calendarForm: UntypedFormGroup;
  calendar: Calendar;
  barberList: ICRMBarber[] = [];
  UserBarber!: ICRMBarber;
  UserBarberId!: number;
  serviceList: ICRMServicios[] = [];
  showDeleteBtn: boolean;
  selectedBarber!: ICRMBarber;
  selectedService!: ICRMServicios;
  freeTimeBarber: any;
  selectedFreeTime: any;
  selectedDateCR: Date = new Date();

  constructor(
    public dialogRef: MatDialogRef<FormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public calendarService: CalendarService,
    public authService: AuthService,
    private fb: UntypedFormBuilder
  ) {
    // Set action and dialog title
    this.action = data.action;
    this.dialogTitle =
      this.action === 'edit' ? data.calendar.title : 'Nueva Reserva';

    // Set the calendar object (either from existing data or a blank one)
    this.calendar =
      this.action === 'edit' ? data.calendar : new Calendar({} as Calendar);

    // Determine if the delete button should be shown
    this.showDeleteBtn = this.action === 'edit';
    // Initialize the form
    this.calendarForm = this.createCalendarForm();
    this.setFormControl(this.calendar);
  }

  ngOnInit() {
    // Cargar barberos
    this.calendarService.getBarber().then((resultBarbers) => {
      this.barberList = resultBarbers.data;
      console.log('barberList:', this.barberList);
    });

    // Cargar servicios
    this.calendarService.getAllServiceDB().then((response) => {
      const responseServices = response.data;
      this.serviceList = responseServices;
    });
  }

  createCalendarForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.calendar.rervationId],
      Title: [this.calendar.title],
      Note: [this.calendar.note],
      Message: [this.calendar.message],
      Apointment: [this.calendar.startDate, [Validators.required]],
      UserBarberId: [this.UserBarberId],
      servicioReservado: [''],
      Price: [this.calendar.price],
      Payment: [this.calendar.payment],
    });
  }

  getErrorMessage(control: UntypedFormControl): string {
    if (control.hasError('required')) {
      return 'This field is required';
    }
    return '';
  }

  submit() {
    if (this.calendarForm.valid) {
      if (this.action === 'edit') {
        // Update existing calendar event
        this.calendarService
          .updateCalendar(this.calendarForm.getRawValue())
          .subscribe({
            next: (response) => {
              const updatedResponse = {
                data: response,
                action: 'edit',
              };
              this.dialogRef.close(updatedResponse);
            },
            error: (error) => {
              console.error('Update Error:', error);
              // Optionally display an error message to the user
            },
          });
      } else {
        const apointment = new Date(
          this.calendarForm.controls['Apointment'].value
        );
        const selectedServiceReserv: ICRMServicios | undefined =
          this.serviceList.find(
            (serviceItem) => serviceItem.code === String(this.selectedService)
          );
        const dataClient: any | undefined = this.authService.getClientData();

        if (dataClient) {
          // Add new calendar event
          const calendarParamns = {
            ...this.calendarForm.getRawValue(),
            Message: selectedServiceReserv?.description,
            Apointment: apointment,
            Title: selectedServiceReserv?.name,
            Price: selectedServiceReserv?.price,
            UserBarberId: this.UserBarberId,
            ClientId: dataClient.unique_name,
          };

          this.calendarService.addCalendar(calendarParamns).subscribe({
            next: (response) => {
              this.dialogRef.close({ updated: true });
              this.dialogRef.close(response); // Close dialog and return newly added doctor data
            },
            error: (error) => {
              console.log('Add Error:', error);
              // Optionally display an error message to the user
            },
          });
        }
      }
    }
  }

  onBarberSelected(event: any): void {
    const selectedId = event.value; // Este es el ID del barbero
    const daySelected: Date = new Date();
    this.UserBarber = selectedId;
    this.UserBarberId = selectedId;
    this.calendarService
      .getBarberFreeTime(daySelected, event.value)
      .then((response) => {
        this.freeTimeBarber = response.data;
      });
  }

  deleteEvent() {
    if (this.calendarForm.valid) {
      this.calendarService
        .deleteCalendar(this.calendarForm.getRawValue())
        .subscribe({
          next: (response) => {
            const updatedResponse = {
              data: response,
              action: 'delete',
            };
            // Close dialog and pass the updated response with the extra message
            this.dialogRef.close(updatedResponse);
          },
          error: (error) => {
            // console.error('Update Error:', error);
            // Optionally display an error message to the user
          },
        });
    }
  }

  onNoClick(): void {
    this.dialogRef.close(); // Close dialog without any action
  }

  onDateChange(event: any) {
    this.selectedDateCR = event.value; // <-- guardo la fecha en variable TS
    this.calendarForm.get('Apointment')?.setValue(this.selectedDateCR); // <-- actualizo el FormControl
    console.log('Fecha Costa Rica seleccionada:', this.selectedDateCR);
  }

  setFormControl(calendarData: any) {
    this.calendarService
      .getReservationById(calendarData.rervationId)
      .then((response) => {
        const reservationData = response.data;
        this.calendarForm.get('id')?.setValue(reservationData.rervationId);
        this.calendarForm.get('Title')?.setValue(reservationData.message);
        this.calendarForm
          .get('Apointment')
          ?.setValue(reservationData.startDate);
        this.calendarForm.get('Price')?.setValue(100);
        this.calendarForm
          .get('UserBarberId')
          ?.setValue(reservationData.userBarberId);
        this.calendarForm.get('ClientId')?.setValue(reservationData.clientId);
        console.log(response);
      });
  }
}
