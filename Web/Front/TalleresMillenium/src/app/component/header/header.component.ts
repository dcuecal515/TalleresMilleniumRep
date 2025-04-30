import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../models/user';
import { jwtDecode } from "jwt-decode";
import {TranslateModule} from '@ngx-translate/core';
import { LanguageService } from '../../service/language.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [TranslateModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {

  constructor(private router:Router,private translate: LanguageService){
    if(localStorage.getItem("token")){
      this.decoded=jwtDecode(localStorage.getItem("token"));
    }else if(sessionStorage.getItem("token")){
      this.decoded=jwtDecode(sessionStorage.getItem("token"));
    }else{
      router.navigateByUrl("")
      this.decoded=null
      
    }
  }
  decoded:User

  goToRoute(route: string) {
    this.router.navigateByUrl(route)
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
