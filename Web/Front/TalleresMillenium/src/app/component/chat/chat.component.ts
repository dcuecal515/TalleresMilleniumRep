import { Component } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { User } from '../../models/user';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { WebsocketService } from '../../service/websocket.service';
import { WebsocketMensaje } from '../../models/WebsocketMensaje';
import { Chat } from '../../models/Chat';
import { Mensaje } from '../../models/Mensaje';

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
  chatName:string = ""

  ngOnInit(): void {
    this.messageReceived$ = this.webSocketService.messageReceived.subscribe(async message => {
      if(message.message=="Te llego un mensaje"){
        const mensaje:Mensaje={UserName:message.userName,Texto:message.texto}
        const mensajes:Mensaje[] = []
        mensajes.push(mensaje)
        const chat:Chat={UserName:message.userName, Mensajes:mensajes}
        this.chats.push(chat)
      }
    });
    this.disconnected$ = this.webSocketService.disconnected.subscribe(() => this.isConnected = false);
    console.log("Rol: ",this.decoded.role)
  }

  enviar(){
    console.log("Mensaje: ",this.texto)
    if(this.texto!=""){
      if(this.decoded.role == "Admin"){
        if(this.chatName != ""){
          const mensaje:WebsocketMensaje={TypeMessage:"mensaje a otro" ,Identifier: "nombre",Identifier2:this.texto}
          // Convertir el objeto a JSON
          const jsonData = JSON.stringify(mensaje);
          console.log(JSON.stringify(mensaje));
          this.webSocketService.sendRxjs(jsonData);
        }
        else{
          // No tiene que llegar aqui
          alert("No estas en ningun chat")
        }
      }else{
        const mensajeWS:WebsocketMensaje={TypeMessage:"mensaje a admin" ,Identifier: this.texto, Identifier2: null}
        // Convertir el objeto a JSON
        const jsonData = JSON.stringify(mensajeWS);
        console.log(JSON.stringify(mensajeWS));
        this.webSocketService.sendRxjs(jsonData);
        const mensaje:Mensaje={UserName:this.decoded.name,Texto:this.texto}
        const mensajes:Mensaje[] = []
        mensajes.push(mensaje)
        const chat:Chat={UserName:this.decoded.name, Mensajes:mensajes}
        this.chats.push(chat)
      }
      
      this.texto = ""
    }
  }
  
}
