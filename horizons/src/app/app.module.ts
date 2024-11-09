import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { appRoutes } from './app.routes';
import { SharedModule } from './shared/shared.module';
import { API_BASE_URL } from './shared/api/api';
import { AUTH_BASE_URL } from './shared/api/auth-api';
import { AuthModule } from './auth/auth.module';
import { environment } from '../environments/environment.development';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    SharedModule,
    AuthModule,
    RouterModule.forRoot(appRoutes, { initialNavigation: 'enabledBlocking' }),
  ],
  providers: [
    {
      provide: API_BASE_URL,
      useValue: 'https://horizons-api.azurewebsites.net',
    },
    {
      provide: AUTH_BASE_URL,
      useValue: environment.apiUrl,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
