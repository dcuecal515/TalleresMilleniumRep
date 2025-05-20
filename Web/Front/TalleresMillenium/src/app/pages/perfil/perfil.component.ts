import { Component } from '@angular/core';
import { AuthService } from '../../service/auth.service';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { User } from '../../models/user';
import { FullUser } from '../../models/FullUser';

@Component({
  selector: 'app-perfil',
  standalone: true,
  imports: [],
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.css'
})
export class PerfilComponent {
  constructor(private authService:AuthService, private router:Router){
    if(localStorage.getItem("token")){
      this.decoded=jwtDecode(localStorage.getItem("token"));
    }else if(sessionStorage.getItem("token")){
      this.decoded=jwtDecode(sessionStorage.getItem("token"));
    }
    this.getUser()
  }

  decoded:User
  user:FullUser | null = null

  async getUser(){
    const result = await this.authService.getFullUser(this.decoded.id)

    this.user = result.data;

    console.log("Usuario: ",this.user)
  }
}
