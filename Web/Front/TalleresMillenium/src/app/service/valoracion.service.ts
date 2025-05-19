import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Result } from '../models/result';
import { Enviovaloracion } from '../models/enviovaloracion';

@Injectable({
  providedIn: 'root'
})
export class ValoracionService {

  constructor(private api:ApiService) { }

  async postvaloracion(valoracion:Enviovaloracion):Promise<void>{
    console.log(valoracion)
    const result=this.api.post<Result>("review/service",valoracion)
  }
}
