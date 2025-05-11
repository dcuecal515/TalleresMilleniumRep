import { Component } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { User } from '../../models/user';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { WebsocketService } from '../../service/websocket.service';
import { WebsocketMensaje } from '../../models/WebsocketMensaje';
import { Chat } from '../../models/Chat';

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
  chats:Chat[] = []

  ngOnInit(): void {
    this.messageReceived$ = this.webSocketService.messageReceived.subscribe(async message => {

    });
    this.disconnected$ = this.webSocketService.disconnected.subscribe(() => this.isConnected = false);
  }

  enviar(){
    console.log("Mensaje: ",this.texto)
    if(this.texto!=""){
      if(this.decoded.rol == "Admin"){
        const mensaje:WebsocketMensaje={TypeMessage:"mensaje a otro" ,Identifier: "nombre",Identifier2:this.texto}
        // Convertir el objeto a JSON
        const jsonData = JSON.stringify(mensaje);
        console.log(JSON.stringify(mensaje));
        this.webSocketService.sendRxjs(jsonData);
      }else{
        const mensaje:WebsocketMensaje={TypeMessage:"mensaje a admin" ,Identifier: this.texto, Identifier2: null}
        // Convertir el objeto a JSON
        const jsonData = JSON.stringify(mensaje);
        console.log(JSON.stringify(mensaje));
        this.webSocketService.sendRxjs(jsonData);
      }
      
      this.texto = ""
    }
  }
  
}
