import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {

  constructor(private translate: TranslateService) {
  }

  changeLanguage(lang: string) {
    this.translate.use(lang);
    localStorage.setItem('language', lang);
  }

  initLanguage(){
    const lang = localStorage.getItem('language') || 'es';
    this.translate.setDefaultLang(lang);
    this.translate.use(lang);
  }
  instant(key: string){
    return this.translate.instant(key);
  }
}
