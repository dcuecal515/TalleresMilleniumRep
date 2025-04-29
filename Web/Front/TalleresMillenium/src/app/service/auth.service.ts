import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Login } from '../models/login';
import { Result } from '../models/result';
import { Token } from '../models/token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private api:ApiService) { }

  async login(login:Login):Promise<Result<Token>> {
    const result=await this.api.post<Token>('Auth/login',login)
    if(result.success){
      this.api.jwt=result.data.accessToken;
    }
    return result
  }
}
