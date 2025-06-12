import { Component, OnInit } from '@angular/core';
import { FooterComponent } from '../../component/footer/footer.component';
import { HeaderComponent } from '../../component/header/header.component';
import { TranslateModule } from '@ngx-translate/core';
import { LanguageService } from '../../service/language.service';

@Component({
  selector: 'app-sobre-nosotros',
  standalone: true,
  imports: [HeaderComponent,FooterComponent,TranslateModule],
  templateUrl: './sobre-nosotros.component.html',
  styleUrl: './sobre-nosotros.component.css'
})
export class SobreNosotrosComponent implements OnInit{
  constructor(private translate:LanguageService){}
  ngOnInit(){
      this.translate.initLanguage()
  }
}
