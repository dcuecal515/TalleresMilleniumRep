import { Component } from '@angular/core';
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
export class HeaderComponent {

  constructor(private router:Router,private translate: LanguageService, private apiService:ApiService){
    if(localStorage.getItem("token")){
      this.decoded=jwtDecode(localStorage.getItem("token"));
    }else if(sessionStorage.getItem("token")){
      this.decoded=jwtDecode(sessionStorage.getItem("token"));
    }
  }
  decoded:User

  goToRoute(route: string) {
    this.router.navigateByUrl(route)
  }

  async cerrarSesion() {
    this.apiService.deleteToken();
    await this.router.navigateByUrl("inicio-sesion");
    window.location.reload()
  }

  changeoption(){
    const language=localStorage.getItem('language');
    if(language=='en'){
      this.changelanguage('es');
    }else{
      this.changelanguage('en');
    }
  }
  changelanguage(lang:string){
    this.translate.changeLanguage(lang);
  }
}
