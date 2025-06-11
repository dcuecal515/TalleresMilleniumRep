import { Component } from '@angular/core';
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

@Component({
  selector: 'app-perfil',
  standalone: true,
  imports: [HeaderComponent],
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.css'
})
export class PerfilComponent {
  constructor(private authService:AuthService, public router:Router){
    if(localStorage.getItem("token")){
      this.decoded=jwtDecode(localStorage.getItem("token"));
    }else if(sessionStorage.getItem("token")){
      this.decoded=jwtDecode(sessionStorage.getItem("token"));
    }
    this.getUser()
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
      title: 'Selecciona el tipo de vehículo',
      input: 'radio',
      inputOptions: {
        coche: 'Coche',
        autobus: 'Autobús',
        camion: 'Camión'
      },
      inputValidator: value => !value && 'Debes seleccionar un tipo'
    });
  
    if (!tipoRaw) return;

    const tipo = tipoRaw.charAt(0).toUpperCase() + tipoRaw.slice(1);
  
    const { value: matricula } = await Swal.fire({
      title: 'Introduce la matrícula',
      input: 'text',
      inputAttributes: {
        pattern: '\\d{4}[A-Z]{3}',
        placeholder: '1234ABC'
      },
      inputValidator: value => {
        if (!/^\d{4}[A-Z]{3}$/.test(value)) {
          return 'Formato incorrecto. Usa 4 números y 3 letras mayúsculas (ej: 1234ABC)';
        }
        return null;
      }
    });
  
    if (!matricula) return;
  
    const { value: fecha_itv } = await Swal.fire({
      title: 'Fecha de la última ITV',
      input: 'date',
      inputValidator: value => !value && 'Debes seleccionar una fecha'
    });
  
    if (!fecha_itv) return;
  
    const { value: combustibleRaw } = await Swal.fire({
      title: 'Selecciona el tipo de combustible',
      input: 'radio',
      inputOptions: {
        diesel: 'Diésel',
        gasolina: 'Gasolina',
        electrico: 'Eléctrico'
      },
      inputValidator: value => !value && 'Debes seleccionar un tipo de combustible'
    });
  
    if (!combustibleRaw) return;

    const combustible = combustibleRaw.charAt(0).toUpperCase() + combustibleRaw.slice(1);
  
    const { value: kilometraje } = await Swal.fire({
      title: 'Introduce el kilometraje',
      input: 'number',
      inputAttributes: {
        min: '0',
        step: '1'
      },
      inputValidator: (value) => {
        const num = Number(value);
        if (!value || isNaN(num) || num < 0) {
          return 'Debes introducir un número válido';
        }
        return null;
      }
    });
  
    if (kilometraje === null) return;
  
    const { value: file } = await Swal.fire({
      title: 'Sube la ficha técnica (imagen)',
      input: 'file',
      inputAttributes: {
        accept: 'image/*'
      },
      inputValidator: value => !value && 'Debes subir una imagen'
    });
  
    if (!file) return;
  
    Swal.fire('Coche registrado', 'Todos los datos fueron introducidos correctamente.', 'success');

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
      title: 'Cambiar imagen de perfil',
      html: `
        <input type="file" id="image-input" class="swal2-file" accept="image/*" />
        <img id="preview" src="" style="margin-top: 10px; max-width: 100%; display: none;" />
      `,
      showCancelButton: true,
      confirmButtonText: 'Guardar',
      cancelButtonText: 'Cancelar',
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
          Swal.showValidationMessage('Debes seleccionar una imagen');
          return undefined;
        }
        return file;
      }
    }).then(async (result) => {
      if (result.isConfirmed && result.value) {
        const selectedFile = result.value as File;
  
        const result2 = await this.authService.changeImage(selectedFile)
        this.user.imagen = environment.images+result2.data.image
  
        Swal.fire('Imagen cargada', '', 'success');
      }
    });
  }

  cambiar_nombre(){
    Swal.fire({
      title: 'Cambiar nombre',
      input: 'text',
      inputLabel: 'Nuevo nombre',
      inputPlaceholder: 'Escribe tu nuevo nombre',
      showCancelButton: true,
      confirmButtonText: 'Guardar',
      cancelButtonText: 'Cancelar',
      inputValidator: (value) => {
        if (!value) {
          return '¡El nombre no puede estar vacío!';
        }
        return null;
      }
    }).then(async (result) => {
      if (result.isConfirmed) {
        const result2 = await this.authService.changeName(result.value)
        this.user.name = result.value;
        Swal.fire(`Nombre actualizado a: ${result.value}`);
      }
    });
  }

  cambiar_email(){
    Swal.fire({
      title: 'Cambiar email',
      input: 'text',
      inputLabel: 'Nuevo email',
      inputPlaceholder: 'Escribe tu nuevo email',
      showCancelButton: true,
      confirmButtonText: 'Guardar',
      cancelButtonText: 'Cancelar',
      inputValidator: (value) => {
        if (!value) {
          return '¡El email no puede estar vacío!';
        }
        return null;
      }
    }).then(async (result) => {
      if (result.isConfirmed) {
        const result2 = await this.authService.changeEmail(result.value)
        if(result2.success){
          this.user.email = result.value;
          Swal.fire(`Email actualizado a: ${result.value}`);
        }else{
          Swal.fire("El correo ya esta registrado")
        }
        
      }
    });
  }

  cambiar_contrasena(){
    Swal.fire({
      title: 'Cambiar contraseña',
      html: `
        <input type="password" id="old-password" class="swal2-input" placeholder="Contraseña actual">
        <input type="password" id="new-password" class="swal2-input" placeholder="Nueva contraseña">
      `,
      focusConfirm: false,
      showCancelButton: true,
      confirmButtonText: 'Cambiar',
      cancelButtonText: 'Cancelar',
      preConfirm: () => {
        const oldPassword = (document.getElementById('old-password') as HTMLInputElement).value;
        const newPassword = (document.getElementById('new-password') as HTMLInputElement).value;
  
        if (!oldPassword || !newPassword) {
          Swal.showValidationMessage('Debes completar ambos campos');
          return undefined;
        }
  
        return { oldPassword, newPassword };
      }
    }).then(async (result) => {
      if (result.isConfirmed) {
        const { oldPassword, newPassword } = result.value;
  
        const result2 = await this.authService.changeContrasena(oldPassword,newPassword)
        if(result2.success){
          Swal.fire('Contraseña actualizada', '', 'success');
        }else{
          Swal.fire('Contraseña incorrecta','','error')
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
