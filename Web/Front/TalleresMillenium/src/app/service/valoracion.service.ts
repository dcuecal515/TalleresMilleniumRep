import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Result } from '../models/result';
import { Enviovaloracion } from '../models/enviovaloracion';

@Injectable({
  providedIn: 'root'
})
export class ValoracionService {

  constructor(private api:ApiService) { }

  async postvaloracion(valoracion:Enviovaloracion){
    console.log(valoracion)
    const result= await this.api.post<Result>("review/service",valoracion)
    console.log("HOLA",result)
    return result
  }
  async postvaloracionProduct(valoracion:Enviovaloracion){
    console.log(valoracion)
    const result= await this.api.post<Result>("review/product",valoracion)
    return result
  }
}
