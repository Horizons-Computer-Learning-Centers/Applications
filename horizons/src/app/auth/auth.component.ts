import { Component } from '@angular/core';
import authSelectors from './^state/auth.selectors';
import { Store } from '@ngrx/store';
import appSelectors from '../^state/app.selectors';

@Component({
  selector: 'horizons-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss'],
})
export class AuthComponent {
  isLoggedIn$ = this.store.select(authSelectors.isLoggedIn);
  isLoggedOut$ = this.store.select(authSelectors.isLoggedOut);
  isMobile$ = this.store.select(appSelectors.getIsMobile);

  constructor(private readonly store: Store) {}
}
