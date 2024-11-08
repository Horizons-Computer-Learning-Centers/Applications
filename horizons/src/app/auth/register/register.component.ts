import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService, RegistrationRequest } from '../../shared/api/auth-api';

@Component({
  selector: 'horizons-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  submitted = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly authService: AuthService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      firstName: ['', [Validators.required, Validators.email]],
      lastName: ['', [Validators.required, Validators.email]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  get f() {
    return this.form.controls;
  }

  onSubmit() {
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }

    const user: RegistrationRequest = {
      email: this.form.value.email,
      firstName: this.form.value.firstName,
      lastName: this.form.value.lastName,
      password: this.form.value.email,
      roles: ['User', 'Admin'],
    };

    this.authService.register(user).subscribe((response) => {
      console.log(response);
    });
  }

  onReset() {
    this.submitted = false;
    this.form.reset();
  }
}
