import { createReducer, on } from '@ngrx/store';
import * as AuthActions from './auth.actions';
import { User } from '../../shared/interfaces/user';

export const AUTH_FEATURE_KEY = 'auth';

export interface AuthState {
  user: User | undefined;
}

export const initialAuthState: AuthState = {
  user: undefined,
};

export const authReducer = createReducer(
  initialAuthState,

  on(AuthActions.login, (state, action) => ({
    ...state,
    user: action.user,
  })),

  on(AuthActions.logout, (state, action) => ({
    ...state,
    user: undefined,
  }))
);
