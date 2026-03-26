import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Router } from "@angular/router";
import { BehaviorSubject, catchError, filter, Observable, switchMap, take, throwError } from "rxjs";
import { Injectable, Injector } from '@angular/core';
import { AuthService } from "../../../Services/AuthService";
@Injectable()
export class interceptor implements HttpInterceptor {
    private isRefreshing = false;
    private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);
    constructor(private injector: Injector, private router: Router) {}
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const authService = this.injector.get(AuthService)

    const accessToken = authService.getAccessToken();

    if (accessToken) {
        request = this.addToken(request, accessToken);
    }
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          console.warn('Token wygasł lub jest nieprawidłowy. Wylogowywanie...');
          authService.logout();
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
