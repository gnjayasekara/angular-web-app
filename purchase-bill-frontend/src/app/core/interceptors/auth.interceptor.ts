import {
  HttpErrorResponse,
  HttpInterceptorFn,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const isBackendRequest =
    req.url.startsWith('http://localhost:5035/api/');

  const request = isBackendRequest
    ? req.clone({
        withCredentials: true,
      })
    : req;

  return next(request).pipe(
    catchError((error: HttpErrorResponse) => {
      if (
        error.status === 401 &&
        isBackendRequest &&
        !req.url.includes('/auth/login') &&
        !req.url.includes('/auth/me')
        ) {
        router.navigate(['/login']);
      }

      return throwError(() => error);
    })
  );
};