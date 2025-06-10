import { Component } from '@angular/core';
import { CarritoService } from '../../service/carrito.service';
import { ElementoCarrito } from '../../models/ElementoCarrito';
import { ServicioCarrito } from '../../models/ServicioCarrito';

@Component({
  selector: 'app-carrito',
  standalone: true,
  imports: [],
  templateUrl: './carrito.component.html',
  styleUrl: './carrito.component.css'
})
export class CarritoComponent {
  constructor(private carritoService: CarritoService) {
    this.getCarrito()
  }

  carrito: ElementoCarrito[]
  existcarrito: boolean = false

  async getCarrito() {
    console.log(this.existcarrito)
    const result = await this.carritoService.getCarrito()
    if (result.success) {
      console.log("HOLA")
      this.carrito = result.data
      this.existcarrito = true
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
      alert("Ocurrio un error inesperado")
    }
  }

  async completarReserva(matricula: string) {
    const result = await this.carritoService.completarReserva(matricula)

    if (result.success) {
      this.carrito = this.carrito.filter(c => c.matricula !== matricula)
      alert("Reserva completada con exito")
    } else {
      alert("Ocurrio un error inesperado")
    }
  }
}
