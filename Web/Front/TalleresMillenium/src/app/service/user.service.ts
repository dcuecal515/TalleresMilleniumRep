import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Listuser } from '../models/listuser';
import { Result } from '../models/result';
import { ChangeRol } from '../models/changerol';
import { environment } from '../../environments/environment.development';
import { User } from '../models/user';
import { Product } from '../models/product';
import { NewProduct } from '../models/NewProduct';
import { Service } from '../models/service';
import { NewService } from '../models/newservice';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private api:ApiService) { }

  async getallUser():Promise<Result<Listuser[]>>{
    const result= await this.api.get<Listuser[]>("User",{},'json')
    if(result.data){
      result.data.forEach(element => {
        element.imagen=environment.images+element.imagen
      });
      return result
    }
    return null
  }
  async changerol(Changerol:ChangeRol){
    const result= await this.api.put<Result>("User/change",Changerol,'json')
    if(result.success){
      return result
    }
    return null;
  }
  async deleteuser(id:number){
    const result= await this.api.delete<Result>("User",{id})
  }
  async changeproduct(id:number,product:Product){
    const result=await this.api.putWithImage("Product/change",this.createform(id,product),'json')
  }
  async addproduct(newproduct:NewProduct){
    const result=await this.api.postWithImage("Product/new",this.createproduct(newproduct))
  }
  async changeservice(id:number,service:Service){
    const result=await this.api.putWithImage("Service/change",this.createformservice(id,service),'json')
  }
  async addservice(newService:NewService){
    const result=await this.api.postWithImage("Service/new",this.createservice(newService))
  }
  createform(id:number,product:Product){
    const formdata = new FormData()
    formdata.append("id",id.toString())
    formdata.append("nombre",product.nombre)
    formdata.append("descripcion",product.descripcion)
    formdata.append("imagen",product.imagen)
    formdata.append("disponible",product.disponible)
    console.log(formdata)
    return formdata
  }
  createproduct(newproduct:NewProduct){
    const formdata = new FormData()
    formdata.append("nombre",newproduct.nombre)
    formdata.append("descripcion",newproduct.descripcion)
    formdata.append("imagen",newproduct.imagen)
    formdata.append("disponible",newproduct.disponible)
    console.log(formdata)
    return formdata
  }
  createformservice(id:number,service:Service){
    const formdata = new FormData()
    formdata.append("id",id.toString())
    formdata.append("nombre",service.nombre)
    formdata.append("descripcion",service.descripcion)
    formdata.append("imagen",service.imagen)
    console.log(formdata)
    return formdata
  }
  createservice(newservice:NewService){
    const formdata = new FormData()
    formdata.append("nombre",newservice.nombre)
    formdata.append("descripcion",newservice.descripcion)
    formdata.append("imagen",newservice.imagen)
    console.log(formdata)
    return formdata
  }
}
