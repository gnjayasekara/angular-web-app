import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'purchase-bill',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/purchase-bill/purchase-bill').then(
        (m) => m.PurchaseBillComponent
      ),
  },
];