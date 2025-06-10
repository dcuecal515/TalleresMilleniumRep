import { Component,OnInit } from '@angular/core';
import { HeaderComponent } from '../../component/header/header.component';
import { FooterComponent } from '../../component/footer/footer.component';
import { TranslateModule } from '@ngx-translate/core';
import { ChatComponent } from "../../component/chat/chat.component";
import { LanguageService } from '../../service/language.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [HeaderComponent, FooterComponent, TranslateModule, ChatComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  constructor(private translate: LanguageService){

  }
ngOnInit() {
      this.translate.initLanguage()
  }
}
