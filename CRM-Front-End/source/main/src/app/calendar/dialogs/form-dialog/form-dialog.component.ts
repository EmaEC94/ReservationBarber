import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { CalendarService } from '../../calendar.service';
import {
  UntypedFormControl,
  Validators,
  UntypedFormGroup,
  UntypedFormBuilder,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  Calendar,
  ICRMBarber,
  ICRMReservationResponse,
  ICRMServicios,
} from '../../calendar.model';
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
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';

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
    private fb: UntypedFormBuilder,
    private snackBar: MatSnackBar
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
    });

    // Cargar servicios
    this.calendarService.getAllServiceDB().then((response) => {
      const responseServices = response.data;
      this.serviceList = responseServices;
    });
  }

  showNotification(
    colorName: string,
    text: string,
    placementFrom: MatSnackBarVerticalPosition,
    placementAlign: MatSnackBarHorizontalPosition
  ) {
    this.snackBar.open(text, '', {
      duration: 2000,
      verticalPosition: placementFrom,
      horizontalPosition: placementAlign,
      panelClass: colorName,
    });
  }

  createCalendarForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.calendar.rervationId],
      Title: [this.calendar.title],
      Note: [this.calendar.note],
      Message: [this.calendar.message],
      StartDate: [this.calendar.startDate, [Validators.required]],
      EndDate: [this.calendar.endDate],
      selectedFreeTime: [this.calendar.selectedFreeTime, [Validators.required]],
      UserBarberId: [this.UserBarberId],
      SubCatalogId: [this.calendar.subCatalogId, [Validators.required]],
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
    if (!this.calendarForm.valid) {
      return;
    }

    // =========================
    // EDITAR RESERVA
    // =========================
    if (this.action === 'edit') {
      this.calendarService
        .updateCalendar(this.calendarForm.getRawValue())
        .subscribe({
          next: (response) => {
            this.dialogRef.close({
              data: response,
              action: 'edit',
            });
          },
          error: (error) => {
            console.error('Update Error:', error);
          },
        });

      return;
    }

    // =========================
    // CREAR RESERVA
    // =========================

    const appointmentValue: Date =
      this.calendarForm.controls['StartDate'].value;

    const selectedFreeTimeValue =
      this.calendarForm.controls['selectedFreeTime'].value;

    if (!appointmentValue || !selectedFreeTimeValue) {
      return;
    }

    // Fecha base (día seleccionado)
    const appointmentDate = new Date(appointmentValue);

    // Hora disponible (sin Z → local)
    const availableHour = selectedFreeTimeValue.availableHour.replace('Z', '');
    const selectedFreeTime = new Date(availableHour);

    // Hora local
    const hours = selectedFreeTime.getHours();
    const minutes = selectedFreeTime.getMinutes();

    // Fecha + hora combinada
    const startDateTime = new Date(
      appointmentDate.getFullYear(),
      appointmentDate.getMonth(),
      appointmentDate.getDate(),
      hours,
      minutes,
      0
    );

    // Función para ISO local (sin Z)
    const toLocalISO = (date: Date): string => {
      return (
        `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(
          2,
          '0'
        )}-${String(date.getDate()).padStart(2, '0')}T` +
        `${String(date.getHours()).padStart(2, '0')}:${String(
          date.getMinutes()
        ).padStart(2, '0')}:${String(date.getSeconds()).padStart(2, '0')}`
      );
    };

    const startISO = toLocalISO(startDateTime);

    // Servicio seleccionado
    let selectedServiceReserv!: ICRMServicios;

    this.serviceList.filter((serviceItem) => {
      if (serviceItem.subCatalogId == this.selectedService.subCatalogId) {
        selectedServiceReserv = serviceItem;
      }
    });

    const timeToMinutes = (time: string): number => {
      const [h, m, s] = time.split(':').map(Number);
      return h * 60 + m + Math.floor(s / 60);
    };

    const serviceDurationMinutes =
      typeof selectedServiceReserv.duration === 'number'
        ? selectedServiceReserv.duration
        : timeToMinutes(selectedServiceReserv.duration);

    const endDateTime = new Date(
      startDateTime.getTime() + serviceDurationMinutes * 60000
    );

    const endISO = toLocalISO(endDateTime);

    const dataClient: any | undefined = this.authService.getClientData();

    if (!dataClient || !selectedServiceReserv) {
      return;
    }

    // Payload final
    const calendarParamns = {
      ...this.calendarForm.getRawValue(),
      Title: selectedServiceReserv.name,
      Message: selectedServiceReserv.description,
      StartDate: startISO,
      EndDate: endISO,
      Price: selectedServiceReserv.price,
      UserBarberId: this.UserBarberId,
      ClientId: dataClient.unique_name,
      SubCatalogId: selectedServiceReserv.subCatalogId,
      State: 1,
    };

    // Enviar al backend
    this.calendarService.addCalendar(calendarParamns).subscribe({
      next: (response) => {
        this.dialogRef.close({ updated: true, data: response });
      },
      error: (error) => {
        console.error('Add Error:', error);
      },
    });
  }

  onBarberSelected(event: any): void {
    console.log('onBarberSelected');

    const selectedId = event.value;
    this.UserBarber = selectedId;
    this.UserBarberId = selectedId;

    const daySelected: Date = this.calendarForm.get('StartDate')?.value;
    //const formattedDate = daySelected.toISOString().split('T')[0];

    if (!daySelected) {
      console.warn('No hay fecha seleccionada aún.');
      return;
    }

    this.calendarService
      .getBarberFreeTime(daySelected, selectedId)
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
    console.log('onDateChange');
    this.selectedDateCR = event.value; // <-- guardo la fecha en variable TS
    this.calendarForm.get('StartDate')?.setValue(this.selectedDateCR); // <-- actualizo el FormControl
    console.log('Fecha Costa Rica seleccionada:', this.selectedDateCR);
  }

  async setFormControl(calendarData: any) {
    try {
      const reservationResponse = await this.calendarService.getReservationById(
        calendarData.rervationId
      );
      const reservation = reservationResponse.data;
      if (reservation) {
        this.calendarForm.patchValue({
          id: reservation.rervationId,
          Title: reservation.message,
          StartDate: reservation.startDate
            ? new Date(reservation.startDate.toString().replace('Z', ''))
            : null,
          EndDate: reservation.endDate
            ? new Date(reservation.endDate.toString().replace('Z', ''))
            : null,
          UserBarberId: reservation.userBarberId,
          ClientId: reservation.clientId,
          SubCatalogId: reservation.subCatalogId,
        });
      }
    } catch (error) {
      console.error('Error al cargar la reserva:', error);
    }
  }
}
