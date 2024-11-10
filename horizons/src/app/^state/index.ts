import { AppEntity } from './app.models';
import { ActionReducer, ActionReducerMap, MetaReducer } from '@ngrx/store';
import { routerReducer } from '@ngrx/router-store';
import { isDevMode } from '@angular/core';
import { appReducer } from './app.reducer';

export const appFeatureKey = 'app';

export const reducers: ActionReducerMap<AppEntity> = {
  router: routerReducer,
  app: appReducer,
};

export const metaReducers: MetaReducer<AppEntity>[] = isDevMode() ? [] : [];
