import { Component } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { User } from '../../models/user';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { WebsocketService } from '../../service/websocket.service';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent {
  constructor(private webSocketService:WebsocketService){
    if(localStorage.getItem("token")){
      this.decoded=jwtDecode(localStorage.getItem("token"));
    }else if(sessionStorage.getItem("token")){
      this.decoded=jwtDecode(sessionStorage.getItem("token"));
    }
  }
  messageReceived$:Subscription;
  disconnected$: Subscription;
  decoded:User
  texto:string=""
  isConnected: boolean = false;

  ngOnInit(): void {
    this.messageReceived$ = this.webSocketService.messageReceived.subscribe(async message => {

    });
    this.disconnected$ = this.webSocketService.disconnected.subscribe(() => this.isConnected = false);
  }

  enviar(){
    console.log("Mensaje: ",this.texto)
    if(this.texto!=""){

      this.texto = ""
    }
  }
  
}
