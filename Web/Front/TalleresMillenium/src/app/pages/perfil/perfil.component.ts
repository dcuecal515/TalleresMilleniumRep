import { Component } from '@angular/core';
import { AuthService } from '../../service/auth.service';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { User } from '../../models/user';
import { FullUser } from '../../models/FullUser';
import { Coche } from '../../models/Coche';
import { environment } from '../../../environments/environment.development';

@Component({
  selector: 'app-perfil',
  standalone: true,
  imports: [],
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.css'
})
export class PerfilComponent {
  constructor(private authService:AuthService, public router:Router){
    if(localStorage.getItem("token")){
      this.decoded=jwtDecode(localStorage.getItem("token"));
    }else if(sessionStorage.getItem("token")){
      this.decoded=jwtDecode(sessionStorage.getItem("token"));
    }
    this.getUser()
  }

  decoded:User
  user:FullUser | null = null
  matricula_actual:string = ""
  coche_actual:Coche | null = null

  async getUser(){
    const result = await this.authService.getFullUser(this.decoded.id)

    this.user = result.data;
    this.user.imagen = environment.images+this.user.imagen
    this.user.coches.forEach(coche => {
      coche.imagen = environment.images+coche.imagen
    });
    console.log(this.user.imagen)
    this.matricula_actual = this.user.coches[0].matricula
    this.coche_actual = this.user.coches[0]

    console.log("Usuario: ",this.user)
  }

  seleccionar_coche(){

  }

  anadir_coche(){

  }

  cambiar_nombre(){

  }

  cambiar_email(){

  }

  cambiar_contrasena(){
    
  }
}
