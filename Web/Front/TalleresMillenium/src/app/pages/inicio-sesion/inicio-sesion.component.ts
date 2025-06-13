import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Login } from '../../models/login';
import { ApiService } from '../../service/api.service';
import { AuthService } from '../../service/auth.service';
import { SignupUser } from '../../models/signupUser';
import { WebsocketService } from '../../service/websocket.service';
import Swal from 'sweetalert2';
import { TranslateModule } from '@ngx-translate/core';
import { LanguageService } from '../../service/language.service';

@Component({
  selector: 'app-inicio-sesion',
  standalone: true,

  imports: [FormsModule,ReactiveFormsModule,TranslateModule],
  templateUrl: './inicio-sesion.component.html',
  styleUrl: './inicio-sesion.component.css'
})
export class InicioSesionComponent implements OnInit{
  constructor(private formBuilder: FormBuilder,private authservice:AuthService,private apiService:ApiService,private router:Router, private webSocketService:WebsocketService,private translate:LanguageService){
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
  ngOnInit(){
      this.translate.initLanguage()
  }
  loginForm: FormGroup;
  email="";
  password="";
  rememberUser=false;
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
  imagenPerfil_nombre:string
  imagenFT_nombre:string
  type:'rxjs';


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
      this.imagenPerfil_nombre=archivo.name
      this.imagenPerfil = archivo
    }
  }
  alSeleccionarArchivoFT(evento: Event): void {
    const input = evento.target as HTMLInputElement;
    const archivo = input.files?.[0];
    if (archivo) {
      console.log('Archivo seleccionado:', archivo.name);
      this.imagenFT_nombre=archivo.name
      this.imagenFT = archivo
    }
  }

  async registrarse(){
    const imagenPerfil = document.getElementById("imagenPerfil") as HTMLInputElement
    if(this.nYApellido != ""){
      if (this.correo != "" && this.correo.match(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/)) {
        if(this.contrasena != ""){
          if(this.contrasena2 != ""){
            if(this.contrasena == this.contrasena2){
              console.log("formulario es ",this.signupForm1)
              if(this.signupForm1.valid){
                if(imagenPerfil.files && imagenPerfil.files.length > 0){
                  this.imagenPerfil = imagenPerfil.files[0]
                }
                const User:SignupUser = {nombre: this.nYApellido.trim(), correo: this.correo.trim(), contrasena: this.contrasena.trim()}
                await this.authservice.register(User,this.imagenPerfil)
                console.log("Estado de mi jwt despues de registro fallido: "+this.apiService.jwt);
                if(this.apiService.jwt){
                  console.log("he entrado pro que si xD");
                  await this.rememberfunction()
                }
              }else{
                Swal.fire({
                            icon: 'error',
                            title: this.translate.instant('warning'),
                            text:  this.translate.instant('input-bad')
                          });
              }
            }
            else{
              Swal.fire({
                icon: 'error',
                title: this.translate.instant('warning'),
                text: this.translate.instant('password-error-equals')
              });
            }
          }
          else{
            Swal.fire({
              icon: 'error',
              title: this.translate.instant('warning'),
              text: this.translate.instant('password-null2')
            });
          }
        }
        else{
          Swal.fire({
            icon: 'error',
            title: this.translate.instant('warning'),
            text: this.translate.instant('password-null')
          });
        }
      }
      else{
        Swal.fire({
          icon: 'error',
          title: this.translate.instant('warning'),
          text: this.translate.instant('email-null')
        });
      }
    }
    else{
      Swal.fire({
        icon: 'error',
        title: this.translate.instant('warning'),
        text:  this.translate.instant('name-null')
      });
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
        Swal.fire({
          icon: 'error',
          title: this.translate.instant('warning'),
          text: this.translate.instant('user-null')
        });
      }
    }else{
      Swal.fire({
        icon: 'error',
        title: this.translate.instant('warning'),
        text: this.translate.instant('input-invalid')
      });
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
    this.connectRxjs()
    this.router.navigateByUrl("");
  }

  connectRxjs() {
    this.type = 'rxjs';
    this.webSocketService.connectRxjs();
  }
}
