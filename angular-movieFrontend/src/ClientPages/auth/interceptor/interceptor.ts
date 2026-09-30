import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { BehaviorSubject, catchError, filter, Observable, switchMap, take, throwError } from "rxjs";
import { Injectable, Injector } from '@angular/core';
import { AuthService } from "../../../Services/AuthService";
@Injectable()
export class interceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);
    constructor(private injector: Injector) {}
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const authService = this.injector.get(AuthService);
    const accessToken = authService.getAccessToken();

    if (accessToken) {
        request = this.addToken(request, accessToken);
    }
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        const isAuthEndpoint = request.url.includes('/Auth/RefreshToken') ||
                               request.url.includes('/Account/Login') || 
                               request.url.includes('/Account/Logout');
        if (error.status === 401 && !isAuthEndpoint) {
          return this.handle401Error(request, next, authService);
        }
        return throwError(() => error);
      })
    );
    }
    private handle401Error(request: HttpRequest<any>, next: HttpHandler, authService: AuthService): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);
      return authService.refreshTokens().pipe(
        switchMap(response => {
          this.isRefreshing = false;
          this.refreshTokenSubject.next(response.accessToken);

          return next.handle(this.addToken(request, response.accessToken));
        }),
        catchError(refreshError => {
          this.isRefreshing = false;
          authService.logout().subscribe();
          return throwError(() => refreshError);
        })
      );
    } else {
      return this.refreshTokenSubject.pipe(
        filter(token => token !== null),
        take(1),
        switchMap(newToken => {
          return next.handle(this.addToken(request, newToken!));
        })
      );
    }
  }
    private addToken(request: HttpRequest<any>, token: string) {
    return request.clone({
        headers: request.headers.set('Authorization', `Bearer ${token}`)
    });
    }
}
