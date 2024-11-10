import { Injectable } from '@angular/core';
import { createEffect, Actions, ofType } from '@ngrx/effects';
import { tap } from 'rxjs';
import * as AuthActions from './auth.actions';
import { Router } from '@angular/router';
import { StorageEnums } from '../../shared/enums/storage-enums';

@Injectable()
export class AuthEffects {
  login$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.login),
        tap((action) =>
          localStorage.setItem(StorageEnums.User, JSON.stringify(action.user))
        )
      ),
    { dispatch: false }
  );

  logout$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.logout),
        tap((action) => {
          localStorage.removeItem(StorageEnums.User);
          this.router.navigateByUrl('/auth/login');
        })
      ),
    { dispatch: false }
  );

  constructor(private actions$: Actions, private router: Router) {}
}
