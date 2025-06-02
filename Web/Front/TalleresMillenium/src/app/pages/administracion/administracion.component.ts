import { Component } from '@angular/core';
import { UserService } from '../../service/user.service';
import { Listuser } from '../../models/listuser';
import { ListService } from '../../service/list.service';
import { Product } from '../../models/product';
import { Service } from '../../models/service';
import { ChangeRol } from '../../models/changerol';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-administracion',
  standalone: true,
  imports: [],
  templateUrl: './administracion.component.html',
  styleUrl: './administracion.component.css'
})
export class AdministracionComponent {

  constructor(private Userservice: UserService, private Listservice: ListService) {
    this.getallUser()
  }
  listusers: Listuser[]
  verusuarios: boolean = true
  verproductos: boolean = false
  verservicios: boolean = false
  listproducts: Product[]
  listservice: Service[]

  async getallUser() {
    const result = await this.Userservice.getallUser()
    this.listusers = result.data
    console.log("HOLA", this.listusers)
    this.verusuarios = true
    this.verproductos = false
    this.verservicios = false
  }
  async getallproduct() {
    const result = await this.Listservice.getallProductWhithoutreview()
    this.listproducts = result.data
    console.log("HOLA", this.listproducts)
    this.verusuarios = false
    this.verproductos = true
    this.verservicios = false
  }
  async getallservicios() {
    const result = await this.Listservice.getallServiceWhithoutreview()
    this.listservice = result.data
    console.log("HOLA", this.listservice)
    this.verusuarios = false
    this.verproductos = false
    this.verservicios = true
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
  editproduct(producto: Product) {
    Swal.fire({
      title: 'Editar Producto',
      html:
        `<input type="text" id="nombre" class="swal2-input" placeholder="Nombre" value="${producto.nombre}">
  <textarea id="descripcion" class="swal2-textarea" placeholder="Descripción">${producto.descripcion}</textarea>

  <div style="text-align: left; margin-top:10px;">
    <label><input type="radio" name="estado" value="Disponible" ${producto.disponible == "Disponible" ? 'checked' : ''}> Disponible</label><br>
    <label><input type="radio" name="estado" value="No disponible" ${producto.disponible == "No disponible" ? 'checked' : ''}> No disponible</label>
    </div>

    <input type="file" id="imagen" accept="image/*" class="swal2-file"><br>`,
      confirmButtonText: 'Guardar',
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
          Swal.showValidationMessage('Nombre y descripción son obligatorios');
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
        this.getallproduct()
      }
    });
  }
  addproduct() {
    Swal.fire({
      title: 'Añadir Producto',
      html:
        `<input type="text" id="nombre" class="swal2-input" placeholder="Nombre">
  <textarea id="descripcion" class="swal2-textarea" placeholder="Descripción"></textarea>

  <div style="text-align: left; margin-top:10px;">
    <label><input type="radio" name="estado" value="Disponible"> Disponible</label><br>
    <label><input type="radio" name="estado" value="No disponible"> No disponible</label>
    </div>

    <input type="file" id="imagen" accept="image/*" class="swal2-file"><br>`,
      confirmButtonText: 'Guardar',
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
          Swal.showValidationMessage('Falta por introducir algun campo');
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
        this.getallproduct()
      }
    });
  }
  editservice(service: Service) {
    Swal.fire({
      title: 'Editar Servicio',
      html:
        `<input type="text" id="nombre" class="swal2-input" placeholder="Nombre" value="${service.nombre}">
  <textarea id="descripcion" class="swal2-textarea" placeholder="Descripción">${service.descripcion}</textarea>

    <input type="file" id="imagen" accept="image/*" class="swal2-file"><br>`,
      confirmButtonText: 'Guardar',
      focusConfirm: false,
      preConfirm: () => {
        const nombreInput = document.getElementById('nombre') as HTMLInputElement | null;
        const descripcionInput = document.getElementById('descripcion') as HTMLTextAreaElement | null;
        const imagenInput = document.getElementById('imagen') as HTMLInputElement | null;

        const nombre = nombreInput.value
        const descripcion = descripcionInput.value
        const imagen = imagenInput.files[0]
        if (!nombre || !descripcion) {
          Swal.showValidationMessage('Nombre y descripción son obligatorios');
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
        this.getallservicios()
      }
    });
  }
  addservice() {
    Swal.fire({
      title: 'Añadir Servicio',
      html:
        `<input type="text" id="nombre" class="swal2-input" placeholder="Nombre">
  <textarea id="descripcion" class="swal2-textarea" placeholder="Descripción"></textarea>
    <input type="file" id="imagen" accept="image/*" class="swal2-file"><br>`,
      confirmButtonText: 'Guardar',
      focusConfirm: false,
      preConfirm: () => {
        const nombreInput = document.getElementById('nombre') as HTMLInputElement | null;
        const descripcionInput = document.getElementById('descripcion') as HTMLTextAreaElement | null;
        const imagenInput = document.getElementById('imagen') as HTMLInputElement | null;

        const nombre = nombreInput.value
        const descripcion = descripcionInput.value
        const imagen = imagenInput.files[0]
        if (!nombre || !descripcion || !imagen) {
          Swal.showValidationMessage('Falta por introducir algun campo');
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
        this.getallservicios()
      }
    });
  }
}
