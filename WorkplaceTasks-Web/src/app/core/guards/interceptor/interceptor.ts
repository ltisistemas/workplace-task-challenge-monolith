import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(private router: Router) { }

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    const unauthenticatedRoutes = ['/login', '/auth/login'];
    const isAuthRoute = unauthenticatedRoutes.some(route => request.url.includes(route));
    
    if (isAuthRoute) {
      return next.handle(request);
    }

    const token = localStorage.getItem('token');
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }

    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          // Limpar dados de autenticação
          localStorage.removeItem('token');
          localStorage.removeItem('user');
          
          // Redirecionar para login
          this.router.navigate(['/login']);
        }
        return throwError(() => error);
      })
    );
  }
}
