import { ApplicationConfig, provideBrowserGlobalErrorListeners, APP_INITIALIZER } from '@angular/core';
import { provideRouter } from '@angular/router';
import { HTTP_INTERCEPTORS, provideHttpClient, withFetch, withInterceptorsFromDi } from '@angular/common/http';
import { routes } from './app.routes';
import { interceptor } from '../ClientPages/auth/interceptor/interceptor';
import { AuthService } from '../Services/AuthService';
import { lastValueFrom } from 'rxjs';

function initializeAuth(authService: AuthService) {
  return () => lastValueFrom(authService.refreshTokens()).catch((err) => {
    console.log('Brak aktywnej sesji (użytkownik niezalogowany) lub błąd odświeżania:', err);
    return null;
  });
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(
      withFetch(),
      withInterceptorsFromDi()
    ),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: interceptor,
      multi: true
    },
    {
      provide: APP_INITIALIZER,
      useFactory: initializeAuth,
      deps: [AuthService],
      multi: true
    },
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes)
  ]
};