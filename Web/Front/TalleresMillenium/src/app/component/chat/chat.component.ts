import { Component } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { User } from '../../models/user';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { WebsocketService } from '../../service/websocket.service';
import { WebsocketMensaje } from '../../models/WebsocketMensaje';
import { Chat } from '../../models/Chat';
import { Mensaje } from '../../models/Mensaje';
import { ChatService } from '../../service/chat.service';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent {
  constructor(private webSocketService: WebsocketService, private chatService: ChatService) {
    if (localStorage.getItem("token")) {
      this.decoded = jwtDecode(localStorage.getItem("token"));
    } else if (sessionStorage.getItem("token")) {
      this.decoded = jwtDecode(sessionStorage.getItem("token"));
    }
    this.obtenerChats();
  }
  messageReceived$: Subscription;
  disconnected$: Subscription;
  decoded: User
  texto: string = ""
  isConnected: boolean = false;
  chats: Chat[] = []
  chatName: string = ""
  chatAbierto: boolean = false

  ngOnInit(): void {
    if (localStorage.getItem("token") || sessionStorage.getItem("token")) {
      this.messageReceived$ = this.webSocketService.messageReceived.subscribe(async message => {
        if (message.message == "Te llego un mensaje") {
          const mensaje: Mensaje = { userName: message.userName, texto: message.texto }

          this.chats.forEach(chat => {
            if (chat.username == message.userName) {
              chat.mensajes.push(mensaje)
            }
          });
        }
        if (message.message == "Te llego un mensaje de admin") {
          const mensaje: Mensaje = { userName: message.userName, texto: message.texto }
          this.chats[0].mensajes.push(mensaje)
        }
      });
      this.disconnected$ = this.webSocketService.disconnected.subscribe(() => this.isConnected = false);
      console.log("Rol: ", this.decoded.role)
    }
  }

  async obtenerChats() {
    if (this.decoded) {
      console.log("rol antes de pedir chats: ", this.decoded.role)
      const result = await this.chatService.getChats(this.decoded.role == "Admin")
      if (result.data == null) {
        this.chats = []
      } else {
        this.chats = result.data
      }

    }
  }

  enviar() {
    console.log("Mensaje: ", this.texto)
    if (this.texto != "") {
      if (this.decoded.role == "Admin") {
        if (this.chatName != "") {
          const mensajeWS: WebsocketMensaje = { TypeMessage: "mensaje a otro", Identifier: this.chatName, Identifier2: this.texto }
          // Convertir el objeto a JSON
          const jsonData = JSON.stringify(mensajeWS);
          console.log(JSON.stringify(mensajeWS));
          this.webSocketService.sendRxjs(jsonData);

          const mensaje: Mensaje = { userName: this.decoded.name, texto: this.texto }

          this.chats.forEach(chat => {
            if (chat.username == this.chatName) {
              chat.mensajes.push(mensaje)
            }
          });

        }
        else {
          // No tiene que llegar aqui
          alert("No estas en ningun chat")
        }
      } else {
        const mensajeWS: WebsocketMensaje = { TypeMessage: "mensaje a admin", Identifier: this.texto, Identifier2: null }
        // Convertir el objeto a JSON
        const jsonData = JSON.stringify(mensajeWS);
        console.log(JSON.stringify(mensajeWS));
        this.webSocketService.sendRxjs(jsonData);

        const mensaje: Mensaje = { userName: this.decoded.name, texto: this.texto }
        if (this.chats.length == 0) {
          const mensajes: Mensaje[] = []
          mensajes.push(mensaje)
          const chat: Chat = { username: this.decoded.name, mensajes: mensajes }
          this.chats.push(chat)
        } else {
          this.chats[0].mensajes.push(mensaje)
        }
      }

      this.texto = ""
    }
  }
  ngOnDestroy(): void {
    if (localStorage.getItem("token") || sessionStorage.getItem("token")) {
      this.messageReceived$.unsubscribe();
      this.disconnected$.unsubscribe();
    }
  }
}
