import { ApplicationConfig, provideBrowserGlobalErrorListeners, APP_INITIALIZER } from '@angular/core';
import { provideRouter } from '@angular/router';
import { HTTP_INTERCEPTORS, provideHttpClient, withFetch, withInterceptorsFromDi } from '@angular/common/http';
import { routes } from './app.routes';
import { interceptor } from '../ClientPages/auth/interceptor/interceptor';
import { AuthService } from '../Services/AuthService'; // <-- Upewnij się, że ta ścieżka do Twojego AuthService jest poprawna
import { lastValueFrom } from 'rxjs';

// Funkcja, która odpali się na samym starcie (po F5), pobierze token z ciasteczka i wrzuci go do RAM-u
function initializeAuth(authService: AuthService) {
  return () => lastValueFrom(authService.refreshTokens()).catch((err) => {
    console.log('Brak aktywnej sesji (użytkownik niezalogowany) lub błąd odświeżania:', err);
    return null; // Ignorujemy błąd, żeby aplikacja poszła dalej dla niezalogowanego użytkownika
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
    // REJESTRACJA INITIALIZERA: To trzyma aplikację w locie, dopóki .NET nie odpowie na /refresh
    {
      provide: APP_INITIALIZER,
      useFactory: initializeAuth,
      deps: [AuthService],
      multi: true
    },
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes)
    // provideClientHydration(...) <-- USUNIĘTE (śmieć po SSR)
  ]
};