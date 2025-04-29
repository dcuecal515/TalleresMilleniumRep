import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../models/user';
import { jwtDecode } from "jwt-decode";

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {

  constructor(private router:Router){
    if(localStorage.getItem("token")){
      this.decoded=jwtDecode(localStorage.getItem("token"));
    }else if(sessionStorage.getItem("token")){
      this.decoded=jwtDecode(sessionStorage.getItem("token"));
    }else{
      router.navigateByUrl("login")
      this.decoded=null
      
    }
  }
  decoded:User

  goToRoute(route: string) {
    this.router.navigateByUrl(route)
  }
}
