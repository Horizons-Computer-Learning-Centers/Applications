import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../shared/api/auth-api';
import { noop, tap } from 'rxjs';
import { Store } from '@ngrx/store';
import authActions from '../^state/auth.actions';
import { Router } from '@angular/router';
import { ResponseEnum } from '../../shared/enums/response.enum';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'horizons-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  submitted = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly authService: AuthService,
    private readonly store: Store,
    private readonly router: Router,
    private readonly toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
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
    this.authService
      .login(this.form.value)
      .pipe(
        tap((response: any) => {
          if (response.responseType === ResponseEnum.LoginSuccess) {
            this.toastr.success('Yay!!', 'Login success');

            this.store.dispatch(authActions.login({ user: response.result }));
            this.router.navigateByUrl('/welcome');
          } else {
            this.toastr.error('Login Error', response.message);
          }
        })
      )
      .subscribe(noop, (error: any) => {
        console.log(error);
      });
  }

  onReset() {
    this.submitted = false;
    this.form.reset();
  }
}
