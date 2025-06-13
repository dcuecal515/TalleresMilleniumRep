import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Login } from '../models/login';
import { Result } from '../models/result';
import { Token } from '../models/token';
import { SignupCar } from '../models/signupCar';
import { SignupUser } from '../models/signupUser';
import { FullUser } from '../models/FullUser';
import { Image } from '../models/Image';
import { Coche } from '../models/Coche';
import { NewCoche } from '../models/NewCoche';
import Swal from 'sweetalert2';

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

  async register(signupUser:SignupUser,imagenPerfil:File|null){
    
    const result=await this.api.postWithImage<Token>('Auth/signup', this.createForm(signupUser,imagenPerfil))
    if(result.success){
      console.log("Entró con accessToken: ",result.data.accessToken)
      this.api.jwt = result.data.accessToken;
    }else{
        Swal.fire({
                icon: 'error',
                title: 'Aviso',
                text: "Este correo ya esta enlazado con otra cuenta"
                });
    }
    return result 
  }

  async getFullUser(id:number){
    console.log("Id: "+id)
    const result = await this.api.get<FullUser>("User/full",{id: id})

    console.log("Resultados ",result,result.data)
    return result
  }

  async changeName(nom:string){
    console.log("Nombre: "+nom)
    const result = await this.api.post<string>("User/nombre",{nombre:nom})

    console.log("Result: ",result)

    return result
  }

  async changeEmail(mail:string){
    console.log("Email: "+mail)
    const result = await this.api.post<Result>("User/email",{email:mail})

    console.log("Result: ",result)

    return result
  }

  async changeContrasena(oldC:string, newC:string){
    console.log("OldContrasena: "+oldC)
    console.log("NewContrasena: "+newC)
    const result = await this.api.post<Result>("User/contrasena",{oldContrasena:oldC,newContrasena:newC})

    console.log("Result: ",result)

    return result
  }
  async changeImage(img:File){
    const result=await this.api.putWithImage<Image>('User/image',this.createFormImage(img))

    console.log(result)
    return result
  }

  async newCar(car:NewCoche,img:File){
    const result = await this.api.postWithImage<Coche>('User/coche',this.createFormCoche(car,img))

    console.log(result)
    return result
  }

  async deleteCar(matricula:string){
    const result = await this.api.delete<Result>('User/coche',{matricula:matricula})

    if(result.success){
      Swal.fire({
        icon: 'success',
        title: 'Aviso',
        text: "Se eliminó el coche con exito"
      });
    }else if(result.statusCode == 409){
      Swal.fire({
        icon: 'info',
        title: 'Aviso',
        text: "No existe el vehiculo seleccionado"
      });
    }else if(result.statusCode == 401){
      Swal.fire({
        icon: 'info',
        title: 'Aviso',
        text: "No tienes permiso de eliminar"
      });
    }else{
      Swal.fire({
        icon: 'error',
        title: 'Aviso',
        text: "Error desconocido"
      });
    }

    return result
  }

  createFormImage(image:File):FormData{
    console.log(image)
    const formdata = new FormData()
    formdata.append("image",image)
    return formdata
  }

  createForm(user:SignupUser,imagenPerfil:File) : FormData{
    const formdata = new FormData()
    console.log("Mi imagen es esta: ",imagenPerfil)
    formdata.append("nombre", user.nombre)
    formdata.append("email", user.correo)
    formdata.append("contrasena", user.contrasena)
    if(imagenPerfil){
      formdata.append("imagenPerfil",imagenPerfil)
    }
    console.log(formdata)
    return formdata;
  }
  createFormCoche(car:NewCoche,img:File) : FormData{
    const formdata = new FormData()
    formdata.append("tipo",car.tipo)
    formdata.append("matricula",car.matricula)
    formdata.append("fecha_itv",car.fecha_itv)
    formdata.append("combustible",car.combustible)
    formdata.append("kilometraje",car.kilometraje.toString())
    formdata.append("imagen",img)
    return formdata
  }
}
