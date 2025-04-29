import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Login } from '../../models/login';
import { ApiService } from '../../service/api.service';
import { AuthService } from '../../service/auth.service';

@Component({
  selector: 'app-inicio-sesion',
  standalone: true,
  imports: [FormsModule,ReactiveFormsModule],
  templateUrl: './inicio-sesion.component.html',
  styleUrl: './inicio-sesion.component.css'
})
export class InicioSesionComponent {
  constructor(private formBuilder: FormBuilder,private authservice:AuthService,private apiService:ApiService,private router:Router){
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required,Validators.email]],
      password: ['', [Validators.required]]
    })
  }

  loginForm: FormGroup;
  email=""
  password=""
  rememberUser=false

  tieneCuenta:boolean = true
  continuarRellenando(){

  }
  async loginUser():Promise<void>{
    if(this.loginForm.valid){
      const Date:Login={email: this.email.trim(),password: this.password.trim()}//hace la interfaz
      console.log(Date)//mostrar interfaz
      await this.authservice.login(Date);
      if(this.apiService.jwt!="" && this.apiService.jwt!=null){
        await this.rememberfunction()
      }else{
        alert("Este usuario no existe")//poner sweetalert2
      }
    }else{
      alert("Campos invalidos")//poner sweetalert2
    }
  }

  rememberfunction(){
    if(this.rememberUser){
      console.log("Recordando al de forma permanente...")
      localStorage.setItem("token", this.apiService.jwt)
      console.log(localStorage.getItem("token"))
    }else{
      console.log("Recordando al de forma leve...")
      sessionStorage.setItem("token", this.apiService.jwt)
      console.log(sessionStorage.getItem("token"))
    }
    this.router.navigateByUrl("");
  }

}
