import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Productlist } from '../models/productlist';
import { Servicio } from '../models/servicio';
import { environment } from '../../environments/environment.development';
import { querypage } from '../models/querypage';
import { Productlistreal } from '../models/productlistreal';
import { Productlistproduct } from '../models/productlistproduct';
import { Producto } from '../models/Producto';

@Injectable({
  providedIn: 'root'
})
export class ListService {

  constructor(private api: ApiService) { }

  async getallservice(Query: querypage) {
    const result = await this.api.get<Productlistreal>("Service", Query, 'json')
    if (result.data) {
      return result
    }
    return null;
  }
  async getallproduct(Query: querypage) {
    const result = await this.api.get<Productlistproduct>("Product", Query, 'json')
    if (result.data) {
      return result
    }
    return null;
  }

  async getservicio(id: string): Promise<Servicio | null> {
    let path = ""
      path = "Service/" + id
      const result = await this.api.get<Servicio>(path, {}, "json")
      if (result.data) {
        const servicio: Servicio = result.data
        servicio.imagen = environment.images + servicio.imagen
        return servicio
      }
      return null;
  }
  async getproducto(id: string): Promise<Producto | null> {
    let path = ""
    path = "Product/" + id
    const result = await this.api.get<Producto>(path, {}, "json")
    if (result.data) {
      const producto: Producto = result.data
      producto.imagen = environment.images + producto.imagen
      return producto
    }
    return null
  }
}

