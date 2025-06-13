import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { CocheR } from '../models/CocheR';
import { Router } from '@angular/router';
import { Reserva } from '../models/Reserva';
import { Result } from '../models/result';
import { ServicioCarrito } from '../models/ServicioCarrito';
import { ElementoCarrito } from '../models/ElementoCarrito';
import { environment } from '../../environments/environment';
import Swal from 'sweetalert2';
import { LanguageService } from './language.service';

@Injectable({
  providedIn: 'root'
})
export class CarritoService {

  constructor(private api:ApiService,private router:Router,private translate:LanguageService) {}

  async getCoches(){
    const result=await this.api.get<CocheR[]>('Coche_Servicio/cochesR')

    if(result.data == null || result.data.length == 0){
      this.router.navigateByUrl("perfil")
      Swal.fire({
                  icon: 'info',
                  title: this.translate.instant('warning'),
                  text: this.translate.instant('error-coche')
                });
      return null
    }

    return result
  }

  async reservar(reserva:Reserva){
    const result = await this.api.post<Result>('Coche_Servicio/reserva',reserva)
    console.log("Result: ",result)

    if(result.success){
      Swal.fire({
        icon: 'success',
        title: this.translate.instant('good'),
        text: this.translate.instant('reserve-good')
      });
    }else if(result.statusCode == 409){
      Swal.fire({
        icon: 'info',
        title: this.translate.instant('warning'),
        text: this.translate.instant('error-service')
      });
    }else if(result.statusCode == 401){
      Swal.fire({
        icon: 'info',
        title: this.translate.instant('warning'),
        text: this.translate.instant('error-vehicule')
      });
    }else{
      Swal.fire({
        icon: 'error',
        title: this.translate.instant('warning'),
        text: this.translate.instant('error-unknow')
      });
    }
  }

  async getCarrito(){
    const result = await this.api.get<ElementoCarrito[]>('Coche_Servicio/carrito')
    console.log("Result: ",result)
    
    result.data.forEach(e => {
      e.servicios.forEach(s => {
        s.imagen = environment.images+s.imagen
      });
    });

    return result
  }

  async deleteService(matricula:string,nombreServicio:string){
    const result = await this.api.delete<Result>('Coche_Servicio/eliminarServicio',{matricula:matricula,nombreServicio:nombreServicio})

    console.log("Result: ",result)

    return result
  }

  async completarReserva(matricula:string){
    const result = await this.api.post<Result>('Coche_Servicio/completarReserva',{matricula:matricula})

    console.log("Result: ",result)

    return result
  }
}
