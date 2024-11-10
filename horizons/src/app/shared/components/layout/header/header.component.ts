import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import authSelectors from '../../../../auth/^state/auth.selectors';

@Component({
  selector: 'horizons-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent {
  isLoggedIn$ = this.store.select(authSelectors.isLoggedIn);
  isLoggedOut$ = this.store.select(authSelectors.isLoggedOut);

  constructor(private readonly store: Store) {}
}
