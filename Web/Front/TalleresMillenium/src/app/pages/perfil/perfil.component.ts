import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../service/auth.service';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { User } from '../../models/user';
import { FullUser } from '../../models/FullUser';
import { Coche } from '../../models/Coche';
import { NewCoche } from '../../models/NewCoche';
import { environment } from '../../../environments/environment';
import { HeaderComponent } from '../../component/header/header.component';
import Swal from 'sweetalert2';
import { ApiService } from '../../service/api.service';
import { TranslateModule } from '@ngx-translate/core';
import { LanguageService } from '../../service/language.service';

@Component({
  selector: 'app-perfil',
  standalone: true,
  imports: [HeaderComponent,TranslateModule],
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.css'
})
export class PerfilComponent implements OnInit{
  constructor(private authService:AuthService, private router:Router,private apiService:ApiService,private translate:LanguageService){
    if(localStorage.getItem("token")){
      this.decoded=jwtDecode(localStorage.getItem("token"));
    }else if(sessionStorage.getItem("token")){
      this.decoded=jwtDecode(sessionStorage.getItem("token"));
    }
    if(this.decoded==null){
      this.router.navigateByUrl('');
    }
    this.getUser()
  }

  ngOnInit(){
      this.translate.initLanguage()
  }

  decoded:User
  user:FullUser | null = null
  matricula_actual:string = ""
  coche_actual:Coche | null = null

  async getUser(){
    const result = await this.authService.getFullUser(this.decoded.id)

    this.user = result.data;
    this.user.imagen = environment.images+this.user.imagen
    this.user.coches.forEach(coche => {
      coche.imagen = environment.images+coche.imagen
    });
    console.log(this.user.imagen)
    if(this.user.coches.length > 0){
      this.matricula_actual = this.user.coches[0].matricula
      this.coche_actual = this.user.coches[0]
    }
    

    console.log("Usuario: ",this.user)
  }

  seleccionar_coche(matricula:string){
    if(matricula != this.matricula_actual){
      this.matricula_actual = matricula
      this.user.coches.forEach(coche => {
        if(matricula == coche.matricula){
          this.coche_actual = coche
        }
      });
    }
  }

  async anadir_coche(){
    const { value: tipoRaw } = await Swal.fire({
      title: this.translate.instant('select-type'),
      input: 'radio',
      inputOptions: {
        coche: this.translate.instant('car'),
        autobus: this.translate.instant('bus'),
        camion: this.translate.instant('truck')
      },
      inputValidator: value => !value && this.translate.instant('select-type-error')
    });
  
    if (!tipoRaw) return;

    const tipo = tipoRaw.charAt(0).toUpperCase() + tipoRaw.slice(1);
  
    const { value: matricula } = await Swal.fire({
      title: this.translate.instant('title-matricula'),
      input: 'text',
      inputAttributes: {
        pattern: '\\d{4}[A-Z]{3}',
        placeholder: '1234ABC'
      },
      inputValidator: value => {
        if (!/^\d{4}[A-Z]{3}$/.test(value)) {
          return this.translate.instant('error-matricula');
        }
        return null;
      }
    });
  
    if (!matricula) return;
  
    const { value: fecha_itv } = await Swal.fire({
      title: this.translate.instant('title-fecha'),
      input: 'date',
      inputValidator: value => !value && this.translate.instant('error-fecha')
    });
  
    if (!fecha_itv) return;
  
    const { value: combustibleRaw } = await Swal.fire({
      title: this.translate.instant('select-type-fuel'),
      input: 'radio',
      inputOptions: {
        diesel: this.translate.instant('select-type-diesel'),
        gasolina: this.translate.instant('select-type-gasolina'),
        electrico: this.translate.instant('select-type-electrico')
      },
      inputValidator: value => !value && this.translate.instant('error-select-type-fuel')
    });
  
    if (!combustibleRaw) return;

    const combustible = combustibleRaw.charAt(0).toUpperCase() + combustibleRaw.slice(1);
  
    const { value: kilometraje } = await Swal.fire({
      title: this.translate.instant('input-mileage'),
      input: 'number',
      inputAttributes: {
        min: '0',
        step: '1'
      },
      inputValidator: (value) => {
        const num = Number(value);
        if (!value || isNaN(num) || num < 0) {
          return this.translate.instant('error-mileage');
        }
        return null;
      }
    });
  
    if (kilometraje === null) return;
  
    const { value: file } = await Swal.fire({
      title: this.translate.instant('image-tecnic'),
      input: 'file',
      inputAttributes: {
        accept: 'image/*'
      },
      inputValidator: value => !value && this.translate.instant('error-image-tecnic')
    });
  
    if (!file) return;
  
    Swal.fire(this.translate.instant('register-success'), this.translate.instant('register-success-text'), 'success');

    const newCoche:NewCoche = {
      tipo : tipo,
      matricula : matricula,
      fecha_itv : fecha_itv,
      combustible : combustible,
      kilometraje : kilometraje
    }

    const result = await this.authService.newCar(newCoche, file)

    result.data.imagen = environment.images+result.data.imagen

    this.user.coches.push(result.data)
    if(this.user.coches.length > 0){
      this.matricula_actual = this.user.coches[0].matricula
      this.coche_actual = this.user.coches[0]
    }
  }

  cambiar_imagen(){
    Swal.fire({
      title: this.translate.instant('change-image'),
      html: `
        <input type="file" id="image-input" class="swal2-file" accept="image/*" />
        <img id="preview" src="" style="margin-top: 10px; max-width: 100%; display: none;" />
      `,
      showCancelButton: true,
      confirmButtonText: this.translate.instant('save'),
      cancelButtonText: this.translate.instant('cancel'),
      didOpen: () => {
        const input = document.getElementById('image-input') as HTMLInputElement;
        const preview = document.getElementById('preview') as HTMLImageElement;
  
        input.addEventListener('change', () => {
          const file = input.files?.[0];
          if (file) {
            const reader = new FileReader();
            reader.onload = () => {
              preview.src = reader.result as string;
              preview.style.display = 'block';
            };
            reader.readAsDataURL(file);
          }
        });
      },
      preConfirm: () => {
        const input = document.getElementById('image-input') as HTMLInputElement;
        const file = input.files?.[0];
        if (!file) {
          Swal.showValidationMessage(this.translate.instant('input-image'));
          return undefined;
        }
        return file;
      }
    }).then(async (result) => {
      if (result.isConfirmed && result.value) {
        const selectedFile = result.value as File;
  
        const result2 = await this.authService.changeImage(selectedFile)
        this.user.imagen = environment.images+result2.data.image
        Swal.fire(this.translate.instant('input-success-image'), '', 'success');
        this.apiService.deleteToken();
        this.router.navigateByUrl("inicio-sesion");
      }
    });
  }

  cambiar_nombre(){
    Swal.fire({
      title: this.translate.instant('change-name'),
      input: 'text',
      inputLabel: this.translate.instant('new-name'),
      inputPlaceholder: this.translate.instant('new-name-placeholder'),
      showCancelButton: true,
      confirmButtonText: this.translate.instant('save'),
      cancelButtonText: this.translate.instant('cancel'),
      inputValidator: (value) => {
        if (!value) {
          return this.translate.instant('warning-name');
        }
        return null;
      }
    }).then(async (result) => {
      if (result.isConfirmed) {
        const result2 = await this.authService.changeName(result.value)
        this.user.name = result.value;
        Swal.fire(`${this.translate.instant('new-name-succes')} ${result.value}`);
        this.apiService.deleteToken();
        this.router.navigateByUrl("inicio-sesion");
      }
    });
  }

  cambiar_email(){
    Swal.fire({
      title: this.translate.instant('change-email'),
      input: 'text',
      inputLabel: this.translate.instant('new-email'),
      inputPlaceholder:this.translate.instant('new-email-placeholder'),
      showCancelButton: true,
      confirmButtonText: this.translate.instant('save'),
      cancelButtonText:  this.translate.instant('cancel'),
      inputValidator: (value) => {
        if (!value) {
          return this.translate.instant('email-input-error');
        }
        return null;
      }
    }).then(async (result) => {
      if (result.isConfirmed) {
        const result2 = await this.authService.changeEmail(result.value)
        if(result2.success){
          this.user.email = result.value;
          Swal.fire(`${this.translate.instant('email-input-success')} ${result.value}`);
          this.apiService.deleteToken();
          this.router.navigateByUrl("inicio-sesion");
        }else{
          Swal.fire(this.translate.instant('update-email'))
        }
      }
    });
  }

  cambiar_contrasena(){
    Swal.fire({
      title: this.translate.instant('change-password'),
      html: `
        <input type="password" id="old-password" class="swal2-input" placeholder="${this.translate.instant('current-password')}">
        <input type="password" id="new-password" class="swal2-input" placeholder="${this.translate.instant('new-password')}">
      `,
      focusConfirm: false,
      showCancelButton: true,
      confirmButtonText: this.translate.instant('save'),
      cancelButtonText: this.translate.instant('cancel'),
      preConfirm: () => {
        const oldPassword = (document.getElementById('old-password') as HTMLInputElement).value;
        const newPassword = (document.getElementById('new-password') as HTMLInputElement).value;
  
        if (!oldPassword || !newPassword) {
          Swal.showValidationMessage(this.translate.instant('request-password'));
          return undefined;
        }
  
        return { oldPassword, newPassword };
      }
    }).then(async (result) => {
      if (result.isConfirmed) {
        const { oldPassword, newPassword } = result.value;
  
        const result2 = await this.authService.changeContrasena(oldPassword,newPassword)
        if(result2.success){
          Swal.fire(this.translate.instant('update-password'), '', 'success');
          this.apiService.deleteToken();
          this.router.navigateByUrl("inicio-sesion");
        }else{
          Swal.fire(this.translate.instant('bad-password'),'','error')
        }
      }
    });
  }

  ver_ficha(src:string){
    Swal.fire({
      showConfirmButton: false,    
      showCloseButton: true,       
      customClass: {
        popup: 'custom-swal-popup'
      },
      html: `
        <img src=${src}
            style="width: 100%; height: auto; object-fit: contain;" />
      `,
      width: '600px',
      padding: '0',
      background: '#fff'
    });
  }
    goToRoute(route: string) {
    this.router.navigateByUrl(route)
  }
}
