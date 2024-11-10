import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService, RegistrationRequest } from '../../shared/api/auth-api';
import { ToastrService } from 'ngx-toastr';
import { ResponseEnum } from '../../shared/enums/response.enum';
import { Router } from '@angular/router';

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
    private readonly authService: AuthService,
    private readonly toastr: ToastrService,
    private readonly router: Router
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

    this.authService.register(this.form.value).subscribe((response: any) => {
      if (response.responseType === ResponseEnum.UserCreated) {
        this.toastr.success('User successfully registered!');

        setTimeout(() => {
          this.router.navigateByUrl('/auth/login');
        }, 2000);
      } else {
        this.toastr.error('Error registering user', response.message);
      }
    });
  }

  onReset() {
    this.submitted = false;
    this.form.reset();
  }
}
