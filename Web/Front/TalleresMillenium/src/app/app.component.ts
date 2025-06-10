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
  constructor(private translate:LanguageService, private webSocketService:WebsocketService){}
  
}
