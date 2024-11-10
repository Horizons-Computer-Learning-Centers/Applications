import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import authSelectors from '../../../../../auth/^state/auth.selectors';
import appSelectors from '../../../../../^state/app.selectors';
import authActions from '../../../../../auth/^state/auth.actions';

@Component({
  selector: 'horizons-top',
  templateUrl: './top.component.html',
  styleUrls: ['./top.component.scss'],
})
export class TopComponent {
  isLoggedIn$ = this.store.select(authSelectors.isLoggedIn);
  isLoggedOut$ = this.store.select(authSelectors.isLoggedOut);
  isMobile$ = this.store.select(appSelectors.getIsMobile);
  user$ = this.store.select(authSelectors.user);

  constructor(private readonly store: Store) {}

  signOut() {
    this.store.dispatch(authActions.logout());
  }
}
