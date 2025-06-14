import { Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard'; // Asegúrate de que esta ruta sea correcta

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login', // Esto es lo que quieres para el inicio
    pathMatch: 'full',
  },
  {
    path: 'login',
    loadComponent: () => import('./login/login.page').then((m) => m.LoginPage),
  },
  {
    // Esta es la ruta principal de tus pestañas.
    // El generador de Ionic debería haber creado 'tabs.page' en 'src/app/tabs/'
    path: 'tabs',
    loadComponent: () => import('./tabs/tabs.page').then((m) => m.TabsPage),
    canActivate: [AuthGuard], // Asegura que solo usuarios autenticados puedan acceder a las pestañas
    children: [
      {
        path: 'tab1',
        loadComponent: () => import('./tab1/tab1.page').then((m) => m.Tab1Page),
      },
      {
        path: 'tab2',
        loadComponent: () => import('./tab2/tab2.page').then((m) => m.Tab2Page),
      },
      {
        path: 'tab3',
        loadComponent: () => import('./tab3/tab3.page').then((m) => m.Tab3Page),
      },
      {
        path: '',
        redirectTo: '/tabs/tab1', // Cuando solo se accede a '/tabs', redirige a '/tabs/tab1'
        pathMatch: 'full',
      },
    ],
  },
  // Opcional: una ruta comodín para cualquier URL no reconocida que redirija al login
  {
    path: '**',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'tabs',
    loadComponent: () => import('./tabs/tabs.page').then( m => m.TabsPage)
  },
  {
    path: 'tab1',
    loadComponent: () => import('./tab1/tab1.page').then( m => m.Tab1Page)
  },
  {
    path: 'tab2',
    loadComponent: () => import('./tab2/tab2.page').then( m => m.Tab2Page)
  },
  {
    path: 'tab3',
    loadComponent: () => import('./tab3/tab3.page').then( m => m.Tab3Page)
  }
];
