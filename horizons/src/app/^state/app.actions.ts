import { createAction, props } from '@ngrx/store';

export const changeViewport = createAction(
  '[App] Changed Viewport',
  props<{ isMobile: boolean }>()
);

export const showSpinner = createAction('[App] Show Spinner');
export const hideSpinner = createAction('[App] Hide Spinner');

const appActions = {
  changeViewport,
  showSpinner,
  hideSpinner,
};

export default appActions;
