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
import { Producto } from '../../models/producto';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-vista-producto',
  standalone: true,
  imports: [HeaderComponent, FooterComponent, FormsModule, ReactiveFormsModule,CommonModule],
  templateUrl: './vista-producto.component.html',
  styleUrl: './vista-producto.component.css'
})
export class VistaProductoComponent {
  constructor(private route: ActivatedRoute, private list: ListService, private formBuilder: FormBuilder, private valoracionService: ValoracionService) {
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
    console.log(tipo)
    let media = 0
    let contador=0
    let result
    if (tipo == "servicio") {
      result = await this.list.getservicio(id);
    } else if (tipo == "producto") {
      result = await this.list.getproducto(id);
    }
    if (result != null) {
      console.log("Resultado actualizado:", result);
      if (tipo == "servicio") {
        this.servicio = result
        console.log(this.servicio)
        console.log("Hola", this.servicio.valoracionesDto.length)
        console.log("Media antes del bucle:"+media)
        for (let i = 0; i < this.servicio.valoracionesDto.length; i++) {
          media += this.servicio.valoracionesDto[i].puntuacion
          contador++
        }
        console.log("MEDIA despues del bucle: " + this.media)
        if(media>0){
          this.media = media/contador
        }else{
          media=0
        }
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
  async postreview() {
    console.log(this.subidareview.value);
    if (this.subidareview.valid) {
      const valoracion: Enviovaloracion = { Texto: this.subidareview.value.texto.trim(), Puntuacion: parseInt(this.subidareview.value.puntuacion), ServicioId: parseInt(this.id) }
      console.log(valoracion)
      if(this.tipo=="servicio"){
        await this.valoracionService.postvaloracion(valoracion)
      }else if(this.tipo == "producto"){
        await this.valoracionService.postvaloracionProduct(valoracion)
      }
      this.subidareview.reset();
      await this.getservicioproducto(this.id, this.tipo)
    } else {
      alert("MACACO ESCRIBE O PUNTUA")
    }
  }
  delay(ms: number) {
  return new Promise(resolve => setTimeout(resolve, ms));
}
}
