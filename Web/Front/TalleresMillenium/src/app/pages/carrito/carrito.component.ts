import { Component, OnInit } from '@angular/core';
import { CarritoService } from '../../service/carrito.service';
import { ElementoCarrito } from '../../models/ElementoCarrito';
import { ServicioCarrito } from '../../models/ServicioCarrito';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { HeaderComponent } from '../../component/header/header.component';
import { TranslateModule } from '@ngx-translate/core';
import { LanguageService } from '../../service/language.service';

@Component({
  selector: 'app-carrito',
  standalone: true,
  imports: [HeaderComponent,TranslateModule],
  templateUrl: './carrito.component.html',
  styleUrl: './carrito.component.css'
})
export class CarritoComponent implements OnInit{
  constructor(private carritoService: CarritoService,private router:Router,private translate:LanguageService) {
    if (localStorage.getItem("token") || sessionStorage.getItem("token")) {
      this.getCarrito()
    } else {
      this.router.navigateByUrl('')
    }
  }
  ngOnInit(){
      this.translate.initLanguage()
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
        title: this.translate.instant('warning'),
        text: this.translate.instant('error')
      });
    }
  }

  async completarReserva(matricula: string) {
    const result = await this.carritoService.completarReserva(matricula)

    if (result.success) {
      this.carrito = this.carrito.filter(c => c.matricula !== matricula)
      Swal.fire({
        icon: 'success',
        title: this.translate.instant('good'),
        text: this.translate.instant('well-booking')
      });
    } else {
      Swal.fire({
        icon: 'error',
        title: this.translate.instant('warning'),
        text: this.translate.instant('error')
      });
    }
  }
}
