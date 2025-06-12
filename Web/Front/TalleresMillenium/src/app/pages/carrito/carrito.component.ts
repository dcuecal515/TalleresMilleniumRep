import { Component } from '@angular/core';
import { CarritoService } from '../../service/carrito.service';
import { ElementoCarrito } from '../../models/ElementoCarrito';
import { ServicioCarrito } from '../../models/ServicioCarrito';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { HeaderComponent } from '../../component/header/header.component';

@Component({
  selector: 'app-carrito',
  standalone: true,
  imports: [HeaderComponent],
  templateUrl: './carrito.component.html',
  styleUrl: './carrito.component.css'
})
export class CarritoComponent {
  constructor(private carritoService: CarritoService,private router:Router) {
    if (localStorage.getItem("token") || sessionStorage.getItem("token")) {
      this.getCarrito()
    } else {
      this.router.navigateByUrl('')
    }
  }

  carrito: ElementoCarrito[] = []

  async getCarrito() {
    const result = await this.carritoService.getCarrito()
    if (result.success) {
      console.log("HOLA")
      this.carrito = result.data
    }

  }

  async eliminar(matricula: string, nombreServicio: string) {
    const result = await this.carritoService.deleteService(matricula, nombreServicio)

    if (result.success) {
      this.carrito.forEach(e => {
        if (e.matricula === matricula) {
          e.servicios = e.servicios.filter(s => s.nombre !== nombreServicio);
        }
      });
      this.carrito = this.carrito.filter(e => e.servicios.length > 0);
    } else {
      Swal.fire({
        icon: 'error',
        title: 'Aviso',
        text: "Ocurrio un error inesperado"
      });
    }
  }

  async completarReserva(matricula: string) {
    const result = await this.carritoService.completarReserva(matricula)

    if (result.success) {
      this.carrito = this.carrito.filter(c => c.matricula !== matricula)
      Swal.fire({
        icon: 'success',
        title: 'Aviso',
        text: "Reserva completada con exito"
      });
    } else {
      Swal.fire({
        icon: 'error',
        title: 'Aviso',
        text: "Ocurrio un error inesperado"
      });
    }
  }
}
