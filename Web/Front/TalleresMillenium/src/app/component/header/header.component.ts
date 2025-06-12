import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../models/user';
import { jwtDecode } from "jwt-decode";
import {TranslateModule} from '@ngx-translate/core';
import { LanguageService } from '../../service/language.service';
import { ApiService } from '../../service/api.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [TranslateModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit {

  constructor(private router:Router,private translate: LanguageService, private apiService:ApiService){
    if(localStorage.getItem("token")){
      this.decoded=jwtDecode(localStorage.getItem("token"));
    }else if(sessionStorage.getItem("token")){
      this.decoded=jwtDecode(sessionStorage.getItem("token"));
    }
  }
  decoded:User
  menuVisible = false;

  showMenu() {
    this.menuVisible = !this.menuVisible;
  }

  ngOnInit() {
      this.translate.initLanguage()
  }

  goToRoute(route: string) {
    this.router.navigateByUrl(route)
  }

  cerrarSesion() {
    this.apiService.deleteToken();
    this.router.navigateByUrl("inicio-sesion");
  }

  changelanguage(lang:string){
    this.translate.changeLanguage(lang);
  }
}
