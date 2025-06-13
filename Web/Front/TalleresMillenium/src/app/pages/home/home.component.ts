import { Component,OnInit } from '@angular/core';
import { HeaderComponent } from '../../component/header/header.component';
import { FooterComponent } from '../../component/footer/footer.component';
import { TranslateModule } from '@ngx-translate/core';
import { ChatComponent } from "../../component/chat/chat.component";
import { LanguageService } from '../../service/language.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [HeaderComponent, FooterComponent, TranslateModule, ChatComponent, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  constructor(private translate: LanguageService){

  }
  
  dias = [
    { nombre: 'Lunes', horario: '9h-19h' },
    { nombre: 'Martes', horario: '9h-19h' },
    { nombre: 'Miércoles', horario: '9h-19h' },
    { nombre: 'Jueves', horario: '9h-19h' },
    { nombre: 'Viernes', horario: '9h-19h' },
    { nombre: 'Sábado', horario: 'Cerrado' },
    { nombre: 'Domingo', horario: 'Cerrado' }
  ];
  ngOnInit() {
        this.translate.initLanguage()
    }

  ngOnDestroy(){}
}
