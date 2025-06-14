// src/app/home/home.page.ts
import { Component, OnInit } from '@angular/core'; // Añade OnInit si vas a usarlo
import {
  IonHeader,
  IonToolbar,
  IonTitle,
  IonContent,
  IonButtons, // <-- Importa esto para los botones en la barra
  IonButton, // <-- Importa esto para el botón
  IonIcon // <-- Importa esto para el ícono del botón
} from '@ionic/angular/standalone';
import { CommonModule } from '@angular/common'; // Usualmente necesario para directivas como ngIf, ngFor
import { AuthService } from '../services/auth.service'; // <-- Importa tu AuthService
import { addIcons } from 'ionicons'; // Para registrar íconos
import { logOutOutline } from 'ionicons/icons'; // Importa el ícono de logout

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
  standalone: true,
  imports: [
    CommonModule, // Necesario para directivas comunes de Angular
    IonHeader,
    IonToolbar,
    IonTitle,
    IonContent,
    IonButtons, // Añadido para el botón de logout
    IonButton,  // Añadido para el botón de logout
    IonIcon     // Añadido para el ícono del botón de logout
  ],
})
export class HomePage implements OnInit { // Implementa OnInit si usas ngOnInit

  constructor(private authService: AuthService) {
    // Registra los íconos de Ionicons que vas a usar
    addIcons({ logOutOutline });
  }

  ngOnInit() {
    // Puedes añadir lógica aquí que se ejecute cuando la página Home se inicialice.
    // Por ejemplo, cargar datos del usuario o préstamos.
  }

  /**
   * Método para cerrar la sesión del usuario.
   * Llama al método logout del AuthService.
   */
  logout() {
    this.authService.logout();
  }
}
