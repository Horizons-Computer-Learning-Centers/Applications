import { isDevMode, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { appRoutes } from './app.routes';
import { SharedModule } from './shared/shared.module';
import { API_BASE_URL } from './shared/api/api';
import { AUTH_BASE_URL } from './shared/api/auth-api';
import { AuthModule } from './auth/auth.module';
import { RootModule } from './^state/root.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { SpinnerInterceptor } from './shared/interceptors/spinner/spinner.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

const baseUrl = isDevMode()
  ? 'https://localhost:5001'
  : 'https://horizons-auth.azurewebsites.net';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    SharedModule,
    AuthModule,
    RouterModule.forRoot(appRoutes, { initialNavigation: 'enabledBlocking' }),
    RootModule,
  ],
  providers: [
    {
      provide: API_BASE_URL,
      useValue: baseUrl,
    },
    {
      provide: AUTH_BASE_URL,
      useValue: baseUrl,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: SpinnerInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
