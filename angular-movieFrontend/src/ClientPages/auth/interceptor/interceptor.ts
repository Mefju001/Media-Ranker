import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { catchError, Observable, throwError } from "rxjs";
import { Injectable, Injector } from '@angular/core';
import { AuthService } from "../../../Services/AuthService";
@Injectable()
export class interceptor implements HttpInterceptor {
  
    constructor(private injector: Injector) {}
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const authService = this.injector.get(AuthService)

    const accessToken = authService.getAccessToken();

    if (accessToken) {
        request = this.addToken(request, accessToken);
    }
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          const token = authService.getAccessToken();
          if (!token) {
            console.warn('Brak tokenu. Przekierowanie...');
            authService.logout();
          } else {
            console.error('Token prawdopodobnie wygasł na backendzie.');
            authService.logout();
          }
        }
        return throwError(() => error);
      })
    );
    }
    private addToken(request: HttpRequest<any>, token: string) {
    return request.clone({
        headers: request.headers.set('Authorization', `Bearer ${token}`)
    });
    }
}
