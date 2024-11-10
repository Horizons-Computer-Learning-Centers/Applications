import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService, ResetPasswordRequest } from '../../shared/api/auth-api';
import { catchError, of } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'horizons-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent implements OnInit {
  successMessage: any;
  email: string = '';
  token: string = '';

  form: FormGroup = new FormGroup({});
  submitted = false;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly authService: AuthService,
    private readonly fb: FormBuilder
  ) {
    this.form = this.fb.group({
      email: [''],
      token: [''],
      newPassword: ['', [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.email = this.route.snapshot.paramMap.get('email') || '';
    this.token = this.route.snapshot.paramMap.get('token') || '';
  }

  onSubmit() {
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }

    if (this.email && this.token) {
      this.token = decodeURIComponent(this.token);

      const updatedForm = {
        ...this.form.value,
        token: this.token,
        email: this.email,
      };

      this.authService
        .resetPassword(updatedForm as ResetPasswordRequest)
        .pipe(
          catchError((error) => {
            // Handle the error here
            console.error('Error confirming email:', error);
            this.successMessage = error;
            return of(null);
          })
        )
        .subscribe((response) => {
          if (response) {
            // Handle successful confirmation here
            this.successMessage = response;
          }
        });
    }
  }
}
