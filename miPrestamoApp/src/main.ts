// src/main.ts
// Modifica esta línea para que incluya 'importProvidersFrom'
import { enableProdMode, importProvidersFrom } from '@angular/core'; // <-- ¡AÑADIDO importProvidersFrom AQUÍ!
import { bootstrapApplication } from '@angular/platform-browser';
import { RouteReuseStrategy, provideRouter, withPreloading, PreloadAllModules } from '@angular/router';
import { IonicRouteStrategy, provideIonicAngular } from '@ionic/angular/standalone';
import { provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS } from '@angular/common/http';
import { IonicStorageModule } from '@ionic/storage-angular';
import { AuthInterceptor } from './app/interceptors/auth.interceptor';
import { AuthService } from './app/services/auth.service';

// Importa tus rutas de la aplicación.
import { routes } from './app/app.routes';
import { AppComponent } from './app/app.component';
import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

bootstrapApplication(AppComponent, {
  providers: [
    { provide: RouteReuseStrategy, useClass: IonicRouteStrategy },
    provideIonicAngular(),

    provideHttpClient(withInterceptorsFromDi()),
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },

    importProvidersFrom(IonicStorageModule.forRoot()), // Esta línea ahora debería ser reconocida

    AuthService,

    provideRouter(routes, withPreloading(PreloadAllModules)),
  ],
}).catch(err => console.log(err));