import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Productlist } from '../models/productlist';
import { Servicio } from '../models/servicio';
import { environment } from '../../environments/environment.development';
import { querypage } from '../models/querypage';
import { Productlistreal } from '../models/productlistreal';

@Injectable({
  providedIn: 'root'
})
export class ListService {

  constructor(private api:ApiService) { }

  async getallservice(Query:querypage){
    const result=await this.api.get<Productlistreal>("Service",Query,'json')
    if(result.data){
      return result
    }
    return null;
  }

  async getservicioproducto(id:string,tipo:string):Promise<Servicio | null>{
    let path=""
    if(tipo=="servicio"){
      path="Service/"+id
      const result = await this.api.get<Servicio>(path,{},"json")
      if(result.data){
      const servicio:Servicio=result.data
      servicio.imagen=environment.images+servicio.imagen
      return servicio
    }
    return null;
    }else if(tipo=="producto"){
      path="Product/"+id
    }
    return null;
  }
}
