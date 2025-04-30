import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Login } from '../../models/login';
import { ApiService } from '../../service/api.service';
import { AuthService } from '../../service/auth.service';
import { Result } from '../../models/result';
import { SignupCar } from '../../models/signupCar';
import { SignupUser } from '../../models/signupUser';

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
    this.signupForm1 = this.formBuilder.group({
      nYApellido: ['', [Validators.required]],
      correo: ['',[Validators.required, Validators.email]],
      contrasena:['',[Validators.required]],
      contrasena2:['',[Validators.required]]
    })
    this.signupForm2 = this.formBuilder.group({
      matricula: ['', [Validators.required, Validators.pattern(/^\d{4}[BCDFGHJKLMNPRSTVWXYZ]{3}$/)]],
      tipo_vehiculo: ['', [Validators.required]],
      fecha_ITV: ['', [Validators.required]],
      tipo_combustible: ['', [Validators.required]]
    });
  }

  loginForm: FormGroup;
  email=""
  password=""
  rememberUser=false
  tieneCuenta:boolean = true;
  primerRellenoCorrecto:boolean = false;
  signupForm1: FormGroup;
  signupForm2:FormGroup;
  nYApellido:string = "";
  correo:string = "";
  imagenPerfil:File | null = null;
  contrasena:string = "";
  contrasena2:string = "";
  matricula:string = "";
  tipo_vehiculo:string = "";
  imagenFT:File | null = null;
  fecha_ITV:Date | null = null;
  tipo_combustible:string = "";


  abrirSelectorArchivoPerfil(): void {
    const input = document.getElementById('imagenPerfil') as HTMLInputElement;
    input.click();
  }
  abrirSelectorArchivoFT(): void {
    const input = document.getElementById('imagenFT') as HTMLInputElement;
    input.click();
  }
  
  alSeleccionarArchivoPerfil(evento: Event): void {
    const input = evento.target as HTMLInputElement;
    const archivo = input.files?.[0];
    if (archivo) {
      console.log('Archivo seleccionado:', archivo.name);  
      this.imagenPerfil = archivo
    }
  }
  alSeleccionarArchivoFT(evento: Event): void {
    const input = evento.target as HTMLInputElement;
    const archivo = input.files?.[0];
    if (archivo) {
      console.log('Archivo seleccionado:', archivo.name);  
      this.imagenFT = archivo
    }
  }

  async continuarRellenando(){
    const imagenPerfil = document.getElementById("imagenPerfil") as HTMLInputElement
    if(this.nYApellido != ""){
      if (this.correo != "" && this.correo.match(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/)) {
        if(this.contrasena != ""){
          if(this.contrasena2 != ""){
            if(this.contrasena == this.contrasena2){
              console.log("formulario es ",this.signupForm1)
              if(this.signupForm1.valid){
                const result = await this.apiService.get<Result>('Auth/emailExists',{ correo: this.correo })
                console.log(result)
                if(result.success){
                  if(imagenPerfil.files && imagenPerfil.files.length > 0){
                    this.imagenPerfil = imagenPerfil.files[0]
                  }
                  this.primerRellenoCorrecto = true
                }else{
                  alert("El correo ya esta registrado, mejor inicia sesión")
                }
              }else{
                alert("Algún campo incorrecto")
              }
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

  async registrarse(){
    if(this.signupForm2.valid){
      if(this.imagenFT != null){
        const User:SignupUser = {nombre: this.nYApellido.trim(), correo: this.correo.trim(), contrasena: this.contrasena.trim()}
        const Car:SignupCar={matricula: this.matricula.trim(), tipo_vehiculo: this.tipo_vehiculo.trim(), fecha_ITV: this.fecha_ITV, tipo_combustible: this.tipo_combustible.trim()}
        console.log("Estado de mi jwt: "+this.apiService.jwt);
        await this.authservice.register(User,Car,this.imagenPerfil,this.imagenFT)
        console.log("Estado de mi jwt despues de registro fallido: "+this.apiService.jwt);
        if(this.apiService.jwt){
          console.log("he entrado pro que si xD");
          await this.rememberfunction()
        }
      }else{
        alert("No puedes registrarte sin foto de la ficha tecnica")
      }
    }else{
      alert("Campos no validos")
    }
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
