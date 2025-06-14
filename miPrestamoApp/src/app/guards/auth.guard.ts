// src/app/guards/auth.guard.ts
import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators'; // <-- ¡IMPORTA 'map' AQUÍ!
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    // Aquí usamos el pipe con el operador map porque `isAuthenticated()` de tu AuthService
    // devuelve un Observable<boolean>.
    return this.authService.isAuthenticated().pipe(
      map(isAuthenticated => { // 'isAuthenticated' es el valor booleano emitido por el Observable
        if (isAuthenticated) {
          return true; // El usuario está autenticado, permite el acceso a la ruta
        } else {
          // El usuario NO está autenticado, redirige al login
          this.router.navigateByUrl('/login', { replaceUrl: true });
          return false;
        }
      })
    );
  }
}