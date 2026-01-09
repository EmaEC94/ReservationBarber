import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AbstractControl, UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '@core';
import { UnsubscribeOnDestroyAdapter } from '@shared';
import { MatButtonModule } from '@angular/material/button';
import { NgClass } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
 
@Component({
    selector: 'app-signin',
    templateUrl: './signin.component.html',
    styleUrls: ['./signin.component.scss'],
    imports: [
        RouterLink,
        FormsModule,
        ReactiveFormsModule,
        MatIconModule,
        MatFormFieldModule,
        NgClass,
        MatButtonModule,
    ]
})
export class SigninComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  loginForm!: UntypedFormGroup;
  submitted = false;
  error = '';
  hide = true;
  constructor(
    private formBuilder: UntypedFormBuilder,
    private router: Router,
    private authService: AuthService    
  ) {
    super();
  }
  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      email: [
        'barberDestiny@gmail.com',
        [Validators.required, Validators.email, Validators.minLength(5)],
      ],
      password: ['admin', Validators.required],
    });
  }

  get form(): { [key: string]: AbstractControl } {
    return this.loginForm.controls;
  }


  onSubmit() {
   this.submitted = true;
   this.authService.checkAuth(this.loginForm.value).subscribe(tokenData => {
      
         if (tokenData.isSuccess) {
              const token = tokenData.data;
              if (token) {
                localStorage.setItem('token', token);
                this.router.navigate(['/landing']);
              }
            } else {
              this.error = 'Usuario o contraseña incorrectas!';
            }
    });
  }
}
