import { Component, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { LanguageService } from '../../service/language.service';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [TranslateModule],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.css'
})
export class FooterComponent implements OnInit {
  constructor(private translate:LanguageService){}
  ngOnInit(){
      this.translate.initLanguage()
  }
}
