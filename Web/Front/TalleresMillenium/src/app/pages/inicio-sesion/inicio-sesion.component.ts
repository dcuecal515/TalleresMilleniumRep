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

  abrirSelectorArchivo(): void {
    const input = document.getElementById('imagenPerfil') as HTMLInputElement;
    input.click();
  }
  
  alSeleccionarArchivo(evento: Event): void {
    const input = evento.target as HTMLInputElement;
    const archivo = input.files?.[0];
    if (archivo) {
      console.log('Archivo seleccionado:', archivo.name);   
    }
  }

  tieneCuenta:boolean = true;
  primerRellenoCorrecto:boolean = false;
  nYApellido:string = "";
  correo:string = "";
  imagenPerfil:File | null = null;
  contrasena:string = "";
  contrasena2:string = "";


  continuarRellenando(){
    const nomYape = document.getElementById("nomYape") as HTMLInputElement
    const correo = document.getElementById("correo") as HTMLInputElement
    const imagenPerfil = document.getElementById("imagenPerfil") as HTMLInputElement
    const contrasena = document.getElementById("contrasena") as HTMLInputElement
    const contrasena2 = document.getElementById("contrasena2") as HTMLInputElement
    if(nomYape.value != ""){
      if (correo.value != "" && correo.value.match(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/)) {
        if(contrasena.value != ""){
          if(contrasena2.value != ""){
            if(contrasena.value == contrasena2.value){
              if(imagenPerfil.files && imagenPerfil.files.length > 0){
                this.imagenPerfil = imagenPerfil.files[0]
              }
              this.primerRellenoCorrecto = true
            }
            else{
              alert("Las contraseñas tienen que ser iguales")
            }
          }
          else{
            alert("El segundo campo contraseña no puede estar vacio")
          }
        }
        else{
          alert("El campo contraseña no puede estar vacio")
        }
      }
      else{
        alert("El campo correo no es valido")
      }
    }
    else{
      alert("El campo nombre no puede estar vacio")
    }
  }

  btn_atras(){
    this.primerRellenoCorrecto=false
  }

  registrarse(){

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
