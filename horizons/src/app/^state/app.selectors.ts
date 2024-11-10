import { createFeatureSelector, createSelector } from '@ngrx/store';
import { AppState } from './app.reducer';
import { appFeatureKey } from './index';

export const selectAppState = createFeatureSelector<AppState>(appFeatureKey);

export const getIsMobile = createSelector(
  selectAppState,
  (state: AppState) => state.isMobile
);

export const spinnerStatus = createSelector(
  selectAppState,
  (state) => state.spinner
);

const appSelectors = {
  getIsMobile,
  spinnerStatus,
};

export default appSelectors;
