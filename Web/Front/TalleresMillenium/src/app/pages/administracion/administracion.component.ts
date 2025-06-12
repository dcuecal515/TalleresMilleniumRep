import { Component, OnInit } from '@angular/core';
import { UserService } from '../../service/user.service';
import { Listuser } from '../../models/listuser';
import { ListService } from '../../service/list.service';
import { Product } from '../../models/product';
import { Service } from '../../models/service';
import { ChangeRol } from '../../models/changerol';
import Swal from 'sweetalert2';
import { CocheServicioFullDto } from '../../models/cocheServicioFull';
import { ServicioCocheName } from '../../models/servicioCocheName';
import { DatePipe } from '@angular/common';
import { AceptarSolicitud } from '../../models/aceptarsolicitud';
import { FinalizarSolicitud } from '../../models/finalizarsolicitud';
import { HeaderComponent } from '../../component/header/header.component';
import { jwtDecode } from 'jwt-decode';
import { User } from '../../models/user';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { LanguageService } from '../../service/language.service';

@Component({
  selector: 'app-administracion',
  standalone: true,
  imports: [DatePipe, HeaderComponent,TranslateModule],
  providers:[DatePipe],
  templateUrl: './administracion.component.html',
  styleUrl: './administracion.component.css'
})
export class AdministracionComponent implements OnInit{

  constructor(private Userservice: UserService, private Listservice: ListService,private Datepipe:DatePipe,private router:Router,private translate:LanguageService) {
    if(localStorage.getItem("token")){
      this.decoded=jwtDecode(localStorage.getItem("token"));
    }else if(sessionStorage.getItem("token")){
      this.decoded=jwtDecode(sessionStorage.getItem("token"));
    }
    if(this.decoded==null || this.decoded.role!="Admin"){
      this.router.navigateByUrl('');
    }
    this.getallUser()
  }
  ngOnInit(){
      this.translate.initLanguage()
  }
  decoded:User
  listusers: Listuser[]
  verusuarios: boolean = true
  verproductos: boolean = false
  verservicios: boolean = false
  vercocheservicio:boolean = false
  listproducts: Product[]
  listservice: Service[]
  listcocheserviciosespera:CocheServicioFullDto[]
  listcocheserviciosfinal:CocheServicioFullDto[]
  listcocheservicios:CocheServicioFullDto[]
  hoy:Date

  async getallUser() {
    const result = await this.Userservice.getallUser()
    this.listusers = result.data
    console.log("HOLA", this.listusers)
    this.verusuarios = true
    this.verproductos = false
    this.verservicios = false
    this.vercocheservicio=false
  }
  async getallproduct() {
    const result = await this.Listservice.getallProductWhithoutreview()
    this.listproducts = result.data
    console.log("HOLA", this.listproducts)
    this.verusuarios = false
    this.verproductos = true
    this.verservicios = false
    this.vercocheservicio=false
  }

  async getallservicios() {
    const result = await this.Listservice.getallServiceWhithoutreview()
    this.listservice = result.data
    console.log("HOLA", this.listservice)
    this.verusuarios = false
    this.verproductos = false
    this.verservicios = true
    this.vercocheservicio=false
  }

  async putadmin(id: number, rol: string) {
    console.log("HOLA:" + id)
    if (rol == "Admin") {
      const Changerol: ChangeRol = { id: id, rol: "User" }
      const result = await this.Userservice.changerol(Changerol)
      if (result.success) {
        this.getallUser()
      }
    }
    if (rol == "User") {
      const Changerol: ChangeRol = { id: id, rol: "Admin" }
      const result = await this.Userservice.changerol(Changerol)
      if (result.success) {
        this.getallUser()
      }
    }
  }
  async deleteuser(id: number) {
    const result = await this.Userservice.deleteuser(id)
    this.getallUser()
  }
  async deleteproduct(id: number) {
    const result = await this.Listservice.deleteproduct(id)
    this.getallproduct()
  }

  async getallcocheservicio(){
    this.listcocheserviciosespera=[]
    this.listcocheserviciosfinal=[]
    const result = await this.Listservice.getallCocheService()
    if(result.success){
      const hoy= new Date();
      this.hoy=hoy
      this.hoy.setHours(0, 0, 0, 0);
      this.listcocheservicios=result.data
      console.log("BUENAS",this.listcocheservicios)
      this.listcocheservicios.forEach(element => {
        console.log("ESTO",element.estado)
        const fechaformateada= new Date(element.fecha);
        fechaformateada.setHours(0, 0, 0, 0);
        if(element.estado=="Reservado"){
          this.listcocheserviciosespera.push(element)
        }else if(element.estado=="Aceptado" && fechaformateada<=this.hoy ){
          this.listcocheserviciosfinal.push(element)
        }
      });
    }
    this.verusuarios = false
    this.verproductos = false
    this.verservicios = false
    this.vercocheservicio=true
  }
  async finishsolicitud(matricula:string,fecha:string){
    const finalizarsolicitud:FinalizarSolicitud={fechaantigua:fecha,matricula:matricula}
    console.log(finalizarsolicitud)
    await this.Userservice.finishsolicitud(finalizarsolicitud)
    this.getallcocheservicio()
  }

  editproduct(producto: Product) {
    Swal.fire({
      title: this.translate.instant('edit-product'),
      html:
        `<input type="text" id="nombre" class="swal2-input" placeholder="${this.translate.instant('name')}" value="${producto.nombre}">
  <textarea id="descripcion" class="swal2-textarea" placeholder="${this.translate.instant('description')}">${producto.descripcion}</textarea>

  <div style="text-align: left; margin-top:10px;">
    <label><input type="radio" name="estado" value="Disponible" ${producto.disponible == "Disponible" ? 'checked' : ''}> ${this.translate.instant('available')}</label><br>
    <label><input type="radio" name="estado" value="No disponible" ${producto.disponible == "No disponible" ? 'checked' : ''}> ${this.translate.instant('not available')}</label>
    </div>

    <input type="file" id="imagen" accept="image/*" class="swal2-file"><br>`,
      confirmButtonText: this.translate.instant('save'),
      focusConfirm: false,
      preConfirm: () => {
        const nombreInput = document.getElementById('nombre') as HTMLInputElement | null;
        const descripcionInput = document.getElementById('descripcion') as HTMLTextAreaElement | null;
        const estadoInput = document.querySelector('input[name="estado"]:checked') as HTMLInputElement | null;
        const imagenInput = document.getElementById('imagen') as HTMLInputElement | null;

        const nombre = nombreInput.value
        const descripcion = descripcionInput.value
        const estado = estadoInput.value
        const imagen = imagenInput.files[0]
        if (!nombre || !descripcion) {
          Swal.showValidationMessage(this.translate.instant('namedescription'));
          return false;
        }

        return {
          nombre,
          descripcion,
          disponible: estado,
          imagen
        };
      }
    }).then(async (result) => {
      if (result.isConfirmed) {
        console.log('Datos finales del producto:', result.value);
        const resultado = await this.Userservice.changeproduct(producto.id, result.value);
        if(resultado.success){
          this.getallproduct()
        }else{
          Swal.fire({
                      icon: 'info',
                      title: this.translate.instant('warning'),
                      text: this.translate.instant('product-exist')
                    });
        }
        
      }
    });
  }
  addproduct() {
    Swal.fire({
      title: this.translate.instant('add-product'),
      html:
        `<input type="text" id="nombre" class="swal2-input" placeholder="${this.translate.instant('name')}">
  <textarea id="descripcion" class="swal2-textarea" placeholder="${this.translate.instant('description')}"></textarea>

  <div style="text-align: left; margin-top:10px;">
    <label><input type="radio" name="estado" value="Disponible">  ${this.translate.instant('available')}</label><br>
    <label><input type="radio" name="estado" value="No disponible">  ${this.translate.instant('not-available')}</label>
    </div>

    <input type="file" id="imagen" accept="image/*" class="swal2-file"><br>`,
      confirmButtonText: this.translate.instant('save'),
      focusConfirm: false,
      preConfirm: () => {
        const nombreInput = document.getElementById('nombre') as HTMLInputElement | null;
        const descripcionInput = document.getElementById('descripcion') as HTMLTextAreaElement | null;
        const estadoInput = document.querySelector('input[name="estado"]:checked') as HTMLInputElement | null;
        const imagenInput = document.getElementById('imagen') as HTMLInputElement | null;

        const nombre = nombreInput.value
        const descripcion = descripcionInput.value
        const estado = estadoInput.value
        const imagen = imagenInput.files[0]
        if (!nombre || !descripcion || !estado || !imagen) {
          Swal.showValidationMessage(this.translate.instant('input-invalid'));
          return false;
        }

        return {
          nombre,
          descripcion,
          disponible: estado,
          imagen
        };
      }
    }).then(async (result) => {
      if (result.isConfirmed) {
        console.log('Datos finales del producto:', result.value);
        const resultado = await this.Userservice.addproduct(result.value);
        if(resultado.success){
          this.getallproduct()
        }else{
          Swal.fire({
                      icon: 'info',
                      title: this.translate.instant('warning'),
                      text: this.translate.instant('product-exist')
                    });
        }
        
      }
    });
  }
  editservice(service: Service) {
    Swal.fire({
      title: this.translate.instant('edit-service'),
      html:
        `<input type="text" id="nombre" class="swal2-input" placeholder="${this.translate.instant('name')}" value="${service.nombre}">
  <textarea id="descripcion" class="swal2-textarea" placeholder="${this.translate.instant('description')}">${service.descripcion}</textarea>

    <input type="file" id="imagen" accept="image/*" class="swal2-file"><br>`,
      confirmButtonText: this.translate.instant('save'),
      focusConfirm: false,
      preConfirm: () => {
        const nombreInput = document.getElementById('nombre') as HTMLInputElement | null;
        const descripcionInput = document.getElementById('descripcion') as HTMLTextAreaElement | null;
        const imagenInput = document.getElementById('imagen') as HTMLInputElement | null;

        const nombre = nombreInput.value
        const descripcion = descripcionInput.value
        const imagen = imagenInput.files[0]
        if (!nombre || !descripcion) {
          Swal.showValidationMessage(this.translate.instant('namedescription'));
          return false;
        }

        return {
          nombre,
          descripcion,
          imagen
        };
      }
    }).then(async (result) => {
      if (result.isConfirmed) {
        console.log('Datos finales del producto:', result.value);
        const resultado = await this.Userservice.changeservice(service.id, result.value);
        if(resultado.success){
          this.getallservicios()
        }else{
          Swal.fire({
                      icon: 'info',
                      title: this.translate.instant('warning'),
                      text: this.translate.instant('service-exist')
                    });
        }
      }
    });
  }
  addservice() {
    Swal.fire({
      title: this.translate.instant('add-service'),
      html:
        `<input type="text" id="nombre" class="swal2-input" placeholder="${this.translate.instant('name')}">
  <textarea id="descripcion" class="swal2-textarea" placeholder="${this.translate.instant('description')}"></textarea>
    <input type="file" id="imagen" accept="image/*" class="swal2-file"><br>`,
      confirmButtonText: this.translate.instant('save'),
      focusConfirm: false,
      preConfirm: () => {
        const nombreInput = document.getElementById('nombre') as HTMLInputElement | null;
        const descripcionInput = document.getElementById('descripcion') as HTMLTextAreaElement | null;
        const imagenInput = document.getElementById('imagen') as HTMLInputElement | null;

        const nombre = nombreInput.value
        const descripcion = descripcionInput.value
        const imagen = imagenInput.files[0]
        if (!nombre || !descripcion || !imagen) {
          Swal.showValidationMessage(this.translate.instant('input-invalid'));
          return false;
        }

        return {
          nombre,
          descripcion,
          imagen
        };
      }
    }).then(async (result) => {
      if (result.isConfirmed) {
        console.log('Datos finales del producto:', result.value);
        const resultado = await this.Userservice.addservice(result.value);
        if(resultado.success){
          this.getallservicios()
        }else{
          Swal.fire({
                      icon: 'info',
                      title: this.translate.instant('warning'),
                      text: this.translate.instant('service-exist')
                    });
        }
      }
    });
  }
  acceptsolicitud(matricula:string,fecha:string){
    Swal.fire({
    title: this.translate.instant('day-taller'),
    input: 'date',
    inputLabel: this.translate.instant('select-day'),
    inputAttributes: {
      min: new Date().toISOString().split('T')[0],
    },
    showCancelButton: true,
    confirmButtonText: this.translate.instant('confirm'),
    cancelButtonText: this.translate.instant('cancel'),
    inputValidator: (value) => {
      if (!value) {
        return this.translate.instant('date-error');
      }
      return null;
    }
  }).then( async (result) => {
    if (result.isConfirmed) {
      const fechaSeleccionada = result.value;
      console.log("resultado: ",result.value)
      const aceptarsolicitud:AceptarSolicitud={fechanueva:result.value,fechaantigua:fecha,matricula:matricula}
      console.log("Mi solicitud: ",aceptarsolicitud)
      await this.Userservice.acceptsolicitud(aceptarsolicitud)
      Swal.fire(`${this.translate.instant('confirm-day')} ${ this.Datepipe.transform(fechaSeleccionada, 'dd/MM/yyyy')}`);
      this.getallcocheservicio()
    }
  });
  }
  deletesolicitud(servicios:ServicioCocheName[]){
    const radiosHTML = servicios.map((item, index) => `
    <div style="margin-bottom: 8px;">
      <input type="radio" name="opcion" id="op${index}" value="${item.idcoche_servicio}">
      <label for="op${index}" style="display: inline-block; width: 200px; padding: 5px; box-sizing: border-box;">
        ${item.nombre}
      </label>
    </div>
  `).join('');

  Swal.fire({
    title: this.translate.instant('select-service-delete'),
    html: radiosHTML,
    showCancelButton: true,
    confirmButtonText: this.translate.instant('delete'),
    cancelButtonText:this.translate.instant('cancel'),
    preConfirm: () => {
      const selected = document.querySelector('input[name="opcion"]:checked')  as HTMLInputElement | null;
      if (!selected) {
        Swal.showValidationMessage(this.translate.instant('select-option'));
        return false;
      }
      return parseInt(selected.value);
    }
  }).then(async (result) => {
    if (result.isConfirmed && result.value != undefined) {
      const indexToRemove = result.value;
      await this.Userservice.deletesolicitud(indexToRemove)
      Swal.fire(`${this.translate.instant('service-delete')} ${indexToRemove}`);
      this.getallcocheservicio()
    }
  });
}
}

