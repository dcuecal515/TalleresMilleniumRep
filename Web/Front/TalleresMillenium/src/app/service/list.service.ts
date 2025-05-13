import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Productlist } from '../models/productlist';

@Injectable({
  providedIn: 'root'
})
export class ListService {

  constructor(private api:ApiService) { }

  async getallservice(){
    const result=await this.api.get<Productlist[]>("Service",{},'json')
    if(result.data){
      return result
    }
    return null;
  }
}
