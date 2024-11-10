import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Store } from '@ngrx/store';
import appActions from '../../../^state/app.actions';
import appSelectors from '../../../^state/app.selectors';

@Injectable()
export class SpinnerInterceptor implements HttpInterceptor {
  constructor(private readonly store: Store) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    console.log(request);
    this.store.dispatch(appActions.showSpinner());

    return next.handle(request).pipe(
      tap((event: any) => {
        if (event.status !== undefined && event.status !== null) {
          console.log(event.status);
          this.store.dispatch(appActions.hideSpinner());
        }
      })
    );
  }
}
