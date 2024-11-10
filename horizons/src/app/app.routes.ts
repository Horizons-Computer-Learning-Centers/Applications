import { Route } from '@angular/router';

export const appRoutes: Route[] = [
  {
    path: '',
    loadChildren: () => import('./nav/nav.module').then((m) => m.NavModule),
  },
];
