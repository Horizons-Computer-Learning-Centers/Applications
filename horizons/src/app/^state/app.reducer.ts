import { AppEntity } from './app.models';
import { createReducer, on } from '@ngrx/store';
import appActions from './app.actions';

export interface AppState extends AppEntity {
  isMobile: boolean;
  spinner: boolean;
}

export const initialState: AppState = {
  isMobile: false,
  spinner: false,
};

export const appReducer = createReducer(
  initialState,
  on(appActions.changeViewport, (state, action) => ({
    ...state,
    isMobile: action.isMobile,
  })),
  on(appActions.showSpinner, (state, action) => ({
    ...state,
    spinner: true,
  })),
  on(appActions.hideSpinner, (state, action) => ({
    ...state,
    spinner: false,
  }))
);
