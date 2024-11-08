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
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(3)]],
    });
  }

  get f() {
    return this.form.controls;
  }

  onSubmit() {
    console.log(this.form);
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }

    this.authService.register(this.form.value).subscribe((response) => {
      console.log(response);
    });
  }

  onReset() {
    this.submitted = false;
    this.form.reset();
  }
}
