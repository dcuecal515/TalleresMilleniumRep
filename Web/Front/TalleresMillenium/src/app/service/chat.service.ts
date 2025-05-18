import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Chat } from '../models/Chat';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  constructor(private api:ApiService) { }

  async getChats(isAdmin:boolean){
    const result=await this.api.get<Chat[]>('Chat/all',isAdmin)

    console.log("Resultados ",result,result.data)

    return result;
  }

}
