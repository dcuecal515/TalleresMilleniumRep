import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Login } from '../models/login';
import { Result } from '../models/result';
import { Token } from '../models/token';
import { SignupCar } from '../models/signupCar';
import { SignupUser } from '../models/signupUser';
import { FullUser } from '../models/FullUser';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private api:ApiService) { }

  async login(login:Login):Promise<Result<Token>> {
    const result=await this.api.post<Token>('Auth/login',login)
    if(result.success){
      this.api.jwt=result.data.accessToken;
    }
    return result
  }

  async register(signupUser:SignupUser,signupCar:SignupCar,imagenPerfil:File|null,imagenFT:File){
    
    const result=await this.api.postWithImage<Token>('Auth/signup', this.createForm(signupUser,signupCar,imagenPerfil,imagenFT))
    if(result.success){
      console.log("Entró con accessToken: ",result.data.accessToken)
      this.api.jwt = result.data.accessToken;
    }else{
      alert("Hubo un problema")
    }
    return result 
  }

  async getFullUser(id:number){
    console.log("Id: "+id)
    const result = await this.api.get<FullUser>("User/full",{id: id})

    console.log("Resultados ",result,result.data)
    return result
  }

  createForm(user:SignupUser,car:SignupCar,imagenPerfil:File,imagenFT:File) : FormData{
    const formdata = new FormData()
    console.log("Mi imagen es esta: ",imagenPerfil)
    console.log("Imagen ficha tecnica: ",imagenFT)
    formdata.append("nombre", user.nombre)
    formdata.append("email", user.correo)
    formdata.append("contrasena", user.contrasena)
    if(imagenPerfil){
      formdata.append("imagenPerfil",imagenPerfil)
    }
    formdata.append("matricula", car.matricula)
    formdata.append("tipo_vehiculo", car.tipo_vehiculo)
    console.log("Fecha ITV: ",car.fecha_ITV)
    formdata.append("fecha_ITV", car.fecha_ITV.toString())
    formdata.append("tipo_combustible", car.tipo_combustible)
    formdata.append("imagenFT", imagenFT)
    console.log(formdata)
    return formdata;
  }
}
