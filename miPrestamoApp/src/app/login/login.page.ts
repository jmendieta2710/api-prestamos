// src/app/login/login.page.ts
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { LoadingController, AlertController, IonicModule } from '@ionic/angular'; // <-- Importa IonicModule
import { FormsModule } from '@angular/forms'; // <-- Importa FormsModule
import { CommonModule } from '@angular/common'; // <-- ¡Asegúrate de importar CommonModule!

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
  standalone: true,
  imports: [IonicModule, FormsModule, CommonModule] // <-- ¡VERIFICA ESTO!
})
export class LoginPage implements OnInit {
  username = '';
  password = '';
  errorMessage: string | null = null;

  constructor(
    private authService: AuthService,
    private router: Router,
    private loadingController: LoadingController,
    private alertController: AlertController
  ) { }

  ngOnInit() {
    this.authService.isAuthenticated().subscribe(isAuthenticated => {
      if (isAuthenticated) {
        this.router.navigateByUrl('/tabs/tab1', { replaceUrl: true });
      }
    });
  }

  async login() {
    if (!this.username || !this.password) {
      this.errorMessage = 'Por favor, introduce nombre de usuario y contraseña.';
      return;
    }

    const loading = await this.loadingController.create({
      message: 'Iniciando sesión...',
    });
    await loading.present();

    this.errorMessage = null;

    this.authService.login({
  nombreUsuario: this.username.trim(),
  contrasena: this.password
    }).subscribe({
      next: async (res) => {
        await loading.dismiss();
        this.router.navigateByUrl('/tabs/tab1', { replaceUrl: true });
      },
      error: async (err) => {
        await loading.dismiss();
        this.errorMessage = err.message || 'Credenciales inválidas. Inténtalo de nuevo.';
        console.error('Error en el login:', err);
      }
    });
  }
}