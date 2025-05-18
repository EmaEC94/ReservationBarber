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
import { Calendar } from '../../calendar.model';
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

export interface DialogData {
  id: number;
  action: string;
  calendar: Calendar;
}

@Component({
  selector: 'app-form-dialog',
  templateUrl: './form-dialog.component.html',
  styleUrls: ['./form-dialog.component.scss'],
  imports: [
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
  ]
})
export class FormDialogComponent {
  action: string;
  dialogTitle: string;
  calendarForm: UntypedFormGroup;
  calendar: Calendar;
  showDeleteBtn: boolean;

  constructor(
    public dialogRef: MatDialogRef<FormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public calendarService: CalendarService,
    private fb: UntypedFormBuilder
  ) {
    // Set action and dialog title
    this.action = data.action;
    this.dialogTitle =
      this.action === 'edit' ? data.calendar.tittle : 'Nueva Reserva';

    // Set the calendar object (either from existing data or a blank one)
    this.calendar =
      this.action === 'edit' ? data.calendar : new Calendar({} as Calendar);

    // Determine if the delete button should be shown
    this.showDeleteBtn = this.action === 'edit';

    // Initialize the form
    this.calendarForm = this.createCalendarForm();
  }

  createCalendarForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.calendar.rervationId],
      Tittle: [this.calendar.tittle, [Validators.required]],
      Note: [this.calendar.note],
      Message: [this.calendar.message, [Validators.required]],
      Apointment: [this.calendar.apointment, [Validators.required]],
      //UserBarberId: [this.calendar.userBarberId],
      //ClientId: [this.calendar.clientId],
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
          const apointment = new Date(this.calendarForm.controls["Apointment"].value);

        // Add new calendar event
        const calendarParamns = {
          ...this.calendarForm.getRawValue(),
          Apointment: apointment,
          UserBarberId: 2,
          ClientId: 4,
        };
        this.calendarService
          .addCalendar(calendarParamns)
          .subscribe({
            next: (response) => {
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
}
