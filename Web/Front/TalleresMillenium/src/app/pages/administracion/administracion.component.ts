import { Component } from '@angular/core';
import { UserService } from '../../service/user.service';
import { Listuser } from '../../models/listuser';
import { ListService } from '../../service/list.service';
import { Product } from '../../models/product';
import { Service } from '../../models/service';
import { ChangeRol } from '../../models/changerol';

@Component({
  selector: 'app-administracion',
  standalone: true,
  imports: [],
  templateUrl: './administracion.component.html',
  styleUrl: './administracion.component.css'
})
export class AdministracionComponent {

  constructor(private Userservice:UserService, private Listservice:ListService){
    this.getallUser()
  }
  listusers:Listuser[]
  verusuarios:boolean=true
  verproductos:boolean=false
  verservicios:boolean=false
  listproducts:Product[]
  listservice:Service[]

  async getallUser(){
    const result= await this.Userservice.getallUser()
    this.listusers=result.data
    console.log("HOLA",this.listusers)
    this.verusuarios=true
    this.verproductos=false
    this.verservicios=false
  }
  async getallproduct(){
    const result=await this.Listservice.getallProductWhithoutreview()
    this.listproducts=result.data
    console.log("HOLA",this.listproducts)
    this.verusuarios=false
    this.verproductos=true
    this.verservicios=false
  }
  async getallservicios(){
    const result=await this.Listservice.getallServiceWhithoutreview()
    this.listservice=result.data
    console.log("HOLA",this.listservice)
    this.verusuarios=false
    this.verproductos=false
    this.verservicios=true
  }
  async putadmin(id:number,rol:string){
    console.log("HOLA:"+id)
    if(rol=="Admin"){
      const Changerol:ChangeRol={id:id,rol:"User"}
      const result=await this.Userservice.changerol(Changerol)
      if(result.success){
        this.getallUser()
      }
    }
    if(rol=="User"){
      const Changerol:ChangeRol={id:id,rol:"Admin"}
      const result=await this.Userservice.changerol(Changerol)
      if(result.success){
        this.getallUser()
      }
    }
  }
  async deleteuser(id:number){
    const result=await this.Userservice.deleteuser(id)
    this.getallUser()
  }
}
