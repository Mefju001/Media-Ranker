import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { HTTP_INTERCEPTORS, provideHttpClient, withFetch, withInterceptorsFromDi } from '@angular/common/http';
import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { interceptor } from '../ClientPages/auth/interceptor/interceptor';
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
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),provideClientHydration(withEventReplay())
  ]
};


