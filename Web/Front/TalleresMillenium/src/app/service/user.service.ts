import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Listuser } from '../models/listuser';
import { Result } from '../models/result';
import { ChangeRol } from '../models/changerol';
import { environment } from '../../environments/environment.development';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private api:ApiService) { }

  async getallUser():Promise<Result<Listuser[]>>{
    const result= await this.api.get<Listuser[]>("User",{},'json')
    if(result.data){
      result.data.forEach(element => {
        element.imagen=environment.images+element.imagen
      });
      return result
    }
    return null
  }
  async changerol(Changerol:ChangeRol){
    const result= await this.api.put<Result>("User/change",Changerol,'json')
    if(result.success){
      return result
    }
    return null;
  }
  async deleteuser(id:number){
    const result= await this.api.delete<Result>("User",{id})
  }
}
