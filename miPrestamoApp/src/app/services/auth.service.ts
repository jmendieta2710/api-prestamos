// src/app/services/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, from } from 'rxjs';
import { tap, catchError, map } from 'rxjs/operators';
import { Storage } from '@ionic/storage-angular';
import { Platform } from '@ionic/angular';
import { Router } from '@angular/router'; // Se añade Router para el logout y redirección

// Importa el archivo de entorno si lo tienes configurado.
// Esto es una buena práctica para gestionar URLs de API entre entornos.
// Crea un archivo `environment.ts` en `src/environments/` si no lo tienes:
// export const environment = {
//   production: false,
//   apiUrl: 'http://192.168.1.14:5000/api' // URL base de tu API
// };
// Y en `environment.prod.ts`:
// export const environment = {
//   production: true,
//   apiUrl: 'https://tu-api-en-produccion.com/api' // URL de tu API en producción
// };
// import { environment } from '../../environments/environment';

// Si no usas environments.ts, define tu URL de la API aquí.
// ¡MUY IMPORTANTE! Esta debe ser la URL base de tu backend, no la de tu app Ionic.
// Basado en tu información anterior, debería ser algo como esto:
const BASE_API_URL = 'http://192.168.1.14:5000/api'; // <--- ¡CORREGIDO AQUÍ!

const JWT_TOKEN_KEY = 'jwtToken'; // Clave para guardar el token en Storage

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // authenticationState se usará para emitir si el usuario está o no autenticado
  authenticationState = new BehaviorSubject(false);
  private _storage: Storage | null = null; // Instancia del Storage

  constructor(
    private http: HttpClient,
    private storage: Storage, // Inyecta el servicio Storage de Ionic
    private platform: Platform, // Inyecta el servicio Platform para verificar si la plataforma está lista
    private router: Router // Inyecta Router para la navegación en logout
  ) {
    // Inicializa el storage cuando la plataforma esté lista para asegurar que esté disponible
    this.platform.ready().then(async () => {
      await this.initStorage(); // Espera a que el storage se inicialice
      await this.checkToken(); // Luego verifica el token al inicio
    });
  }

  // Método asíncrono para inicializar Ionic Storage
  async initStorage() {
    // Si ya existe una instancia de _storage, no la volvemos a crear
    if (!this._storage) {
      this._storage = await this.storage.create();
      console.log('Ionic Storage inicializado.');
    }
  }

  // Verifica si hay un token almacenado para determinar el estado de autenticación
  async checkToken() {
    // Asegúrate de que _storage esté inicializado antes de intentar usarlo
    if (this._storage) {
      const token = await this._storage.get(JWT_TOKEN_KEY);
      if (token) {
        // Aquí podrías añadir lógica para validar el token (ej. si ha expirado)
        this.authenticationState.next(true);
        console.log('Token JWT encontrado en almacenamiento.');
      } else {
        this.authenticationState.next(false);
        console.log('No se encontró token JWT en almacenamiento.');
      }
    } else {
      console.warn('Storage no inicializado al intentar verificar el token.');
      this.authenticationState.next(false);
    }
  }

  /**
   * Método para iniciar sesión en la API.
   * @param credentials Objeto con nombreUsuario y contrasena.
   * @returns Un Observable que emite la respuesta del login.
   */
  login(credentials: { nombreUsuario: string; contrasena: string }): Observable<{ token: string, message?: string }> {
    // CONSTRUCCIÓN CORRECTA DE LA URL:
    // Usa la URL base de tu API y añade el endpoint de login.
    // Si usas environment.ts: const url = `${environment.apiUrl}/Auth/login`;
    const url = `${BASE_API_URL}/Auth/login`; // <--- ¡CORREGIDO: API_URL ahora es BASE_API_URL!

    // Realiza la petición POST a tu backend
    return this.http.post<{ token: string, message?: string }>(url, credentials).pipe(
      tap(async (res) => { // 'res' ya será { token: string, message?: string }
        if (res.token && this._storage) { // Asegúrate de que el token existe y storage está listo
          await this._storage.set(JWT_TOKEN_KEY, res.token); // Guarda el token
          this.authenticationState.next(true); // Actualiza el estado de autenticación
          console.log('Login exitoso. Token guardado en almacenamiento.');
        }
      }),
      catchError(e => {
        console.error('Error en el servicio de login:', e);
        // Si el login falla, asegúrate de que el estado sea 'no autenticado'
        this.authenticationState.next(false);
        // También remueve cualquier token si la operación anterior fue fallida y quizás había un token obsoleto
        if (this._storage) {
          this._storage.remove(JWT_TOKEN_KEY);
        }
        // Propaga un error con un mensaje útil para el componente que llama
        throw new Error(e.error?.error || e.error?.message || 'Error en el inicio de sesión. Credenciales inválidas.');
      })
    );
  }

  /**
   * Método para cerrar sesión.
   * Elimina el token del almacenamiento y redirige al usuario.
   */
  async logout() {
    if (this._storage) {
      await this._storage.remove(JWT_TOKEN_KEY); // Elimina el token
      this.authenticationState.next(false); // Actualiza el estado de autenticación
      console.log('Sesión cerrada. Token eliminado del almacenamiento.');
      this.router.navigateByUrl('/login', { replaceUrl: true }); // Redirige al login
    } else {
      console.warn('Storage no inicializado al intentar cerrar sesión.');
      this.authenticationState.next(false); // Aun así, actualiza el estado
      this.router.navigateByUrl('/login', { replaceUrl: true });
    }
  }

  /**
   * Obtiene el token JWT almacenado.
   * @returns Una promesa que resuelve con el token o null.
   */
  getToken(): Promise<string | null> {
    return this._storage ? this._storage.get(JWT_TOKEN_KEY) : Promise.resolve(null);
  }

  /**
   * Observable que emite el estado actual de autenticación (true/false).
   * @returns Un Observable de boolean.
   */
  isAuthenticated(): Observable<boolean> {
    return this.authenticationState.asObservable();
  }
}
