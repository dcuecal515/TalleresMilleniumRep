import { Component } from '@angular/core';
import { HeaderComponent } from '../../component/header/header.component';
import { FooterComponent } from "../../component/footer/footer.component";
import { ActivatedRoute } from '@angular/router';
import { ListService } from '../../service/list.service';
import { Servicio } from '../../models/servicio';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ValoracionService } from '../../service/valoracion.service';
import { Enviovaloracion } from '../../models/enviovaloracion';
import { User } from '../../models/user';
import { jwtDecode } from "jwt-decode";
import { Producto } from '../../models/Producto';
import { CarritoService } from '../../service/carrito.service';
import { CocheR } from '../../models/CocheR';
import Swal from 'sweetalert2';
import { Reserva } from '../../models/Reserva';

@Component({
  selector: 'app-vista-producto',
  standalone: true,
  imports: [HeaderComponent, FooterComponent, FormsModule, ReactiveFormsModule],
  templateUrl: './vista-producto.component.html',
  styleUrl: './vista-producto.component.css'
})
export class VistaProductoComponent {
  constructor(private route: ActivatedRoute, private list: ListService, private formBuilder: FormBuilder, private valoracionService: ValoracionService, private carritoService: CarritoService) {
    this.subidareview = this.formBuilder.group({
      texto: ['', [Validators.required]],
      puntuacion: ['', [Validators.required]]
    })
  }
  tipo: string
  id: string
  ruta: string
  servicio: Servicio
  producto: Producto
  media: number
  subidareview: FormGroup
  texto: string
  puntuacion: number
  decoded: User
  coches:CocheR[] | null = null

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.tipo = params.get('tipo')
      this.id = params.get('id')
      this.getservicioproducto(this.id, this.tipo)
    });
    if (localStorage.getItem("token")) {
      this.decoded = jwtDecode(localStorage.getItem("token"));
    } else if (sessionStorage.getItem("token")) {
      this.decoded = jwtDecode(sessionStorage.getItem("token"));
    }
  }
  async getservicioproducto(id: string, tipo: string) {
    console.log("me hago")
    console.log(tipo)
    let media = 0
    let result
    if (tipo == "servicio") {
      result = await this.list.getservicio(id);
    } else if (tipo == "producto") {
      result = await this.list.getproducto(id);
    }
    console
    if (result != null) {
      if (tipo == "servicio") {
        this.servicio = result
        console.log(this.servicio)
        console.log("Hola", this.servicio.valoracionesDto.length)
        for (let i = 0; i < this.servicio.valoracionesDto.length; i++) {
          media += this.servicio.valoracionesDto[i].puntuacion
        }
        this.media = media
        console.log("MEDIA: " + this.media)
        console.log("SERVICIO O PRODUCTO:", this.servicio)
      } else if (tipo == "producto") {
        this.producto = result
        console.log(this.producto)
        console.log("Hola", this.producto.valoracionesDto.length)
        for (let i = 0; i < this.producto.valoracionesDto.length; i++) {
          media +=this.producto.valoracionesDto[i].puntuacion
        }
        this.media = media
        console.log("MEDIA: " + this.media)
        console.log("SERVICIO O PRODUCTO:", this.producto)
      }
    }
  }

  async reservar(){
    const result = await this.carritoService.getCoches()

    console.log("Resultado: ",result.data)

    this.coches = result.data

    if (this.coches != null) {
      if (this.coches.length === 1) {
        const cocheUnico = this.coches[0];
        console.log('Coche unico:', cocheUnico);
        const reserva:Reserva = {
          matricula: cocheUnico.matricula,
          servicioId: this.servicio.id
        }
        this.carritoService.reservar(reserva)
      } else {
        const htmlRadios = `
          <div style="display: flex; gap: 20px; justify-content: center; flex-wrap: wrap;">
            ${this.coches.map(coche => `
              <label style="display: flex; flex-direction: column; align-items: center; padding: 10px; border: 1px solid #ccc; border-radius: 10px; width: 120px; cursor: pointer;">
                <div style="font-weight: bold; margin-bottom: 4px;">${coche.tipo}</div>
                <div style="font-size: 0.9em; color: #555;">${coche.matricula}</div>
                <input type="radio" name="coche" value="${coche.matricula}" style="margin-top: 10px;" />
              </label>
            `).join('')}
          </div>
        `;
    
        Swal.fire({
          title: 'Selecciona un coche',
          html: htmlRadios,
          showCancelButton: true,
          confirmButtonText: 'Seleccionar',
          preConfirm: () => {
            const selected = (document.querySelector('input[name="coche"]:checked') as HTMLInputElement)?.value;
            if (!selected) {
              Swal.showValidationMessage('¡Debes seleccionar un coche!');
              return null;
            }
            return selected;
          }
        }).then(result => {
          if (result.isConfirmed && result.value) {
            const cocheSeleccionado = this.coches.find(c => c.matricula === result.value);
            console.log('Coche seleccionado:', cocheSeleccionado);
            const reserva:Reserva = {
              matricula: cocheSeleccionado.matricula,
              servicioId: this.servicio.id
            }
            this.carritoService.reservar(reserva)
          }
        });
      }
    }        
  }

  async postreview() {
    console.log(this.subidareview.value);
    if (this.subidareview.valid) {
      const valoracion: Enviovaloracion = { Texto: this.subidareview.value.texto.trim(), Puntuacion: parseInt(this.subidareview.value.puntuacion), ServicioId: parseInt(this.id) }
      console.log(valoracion)
      await this.valoracionService.postvaloracion(valoracion)
      this.getservicioproducto(this.id, this.tipo)
    } else {
      alert("MACACO ESCRIBE O PUNTUA")
    }

  }
}
