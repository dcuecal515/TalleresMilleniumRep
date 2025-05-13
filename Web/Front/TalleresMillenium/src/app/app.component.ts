import { Component} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TranslateModule} from '@ngx-translate/core';
import { LanguageService } from './service/language.service';
import { WebsocketService } from './service/websocket.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,TranslateModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent{
  title = 'TalleresMillenium';
  type:'rxjs';
  constructor(private translate:LanguageService, private webSocketService:WebsocketService){
    console.log("HOLA FUNCIONO");
    if(localStorage.getItem("token") || sessionStorage.getItem("token")){
      console.log("Entro si tengo sesion iniciada")
      if(!this.webSocketService.isConnectedRxjs()){
        console.log("Entro si no estoy conectado")
        this.connectRxjs()
      }
    }
  }
  connectRxjs() {
    this.type = 'rxjs';
    this.webSocketService.connectRxjs();
  }
}
