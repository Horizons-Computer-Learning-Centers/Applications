import { NgModule } from '@angular/core';

import { AuthRoutingModule } from './auth-routing.module';
import { LoginComponent } from './login/login.component';
import { SharedModule } from '../shared/shared.module';
import { RegisterComponent } from './register/register.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { AuthComponent } from './auth.component';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { AuthEffects } from './^state/auth.effects';
import { AUTH_FEATURE_KEY, authReducer } from './^state/auth.reducer';

@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    ConfirmEmailComponent,
    ResetPasswordComponent,
    ForgotPasswordComponent,
    AuthComponent,
  ],
  imports: [
    SharedModule,
    AuthRoutingModule,
    StoreModule.forFeature(AUTH_FEATURE_KEY, authReducer),
    EffectsModule.forFeature([AuthEffects]),
  ],
})
export class AuthModule {}
