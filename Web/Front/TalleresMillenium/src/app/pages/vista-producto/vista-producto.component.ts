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

@Component({
  selector: 'app-vista-producto',
  standalone: true,
  imports: [HeaderComponent, FooterComponent,FormsModule,ReactiveFormsModule],
  templateUrl: './vista-producto.component.html',
  styleUrl: './vista-producto.component.css'
})
export class VistaProductoComponent {
  constructor(private route: ActivatedRoute,private list:ListService,private formBuilder: FormBuilder,private valoracionService:ValoracionService) {
    this.subidareview = this.formBuilder.group({
      texto: ['', [Validators.required]],
      puntuacion: ['', [Validators.required]]
    })
  }
  tipo: string
  id: string
  ruta: string
  servicio:Servicio
  media:number
  subidareview:FormGroup
  texto:string
  puntuacion:number
  decoded:User

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.tipo = params.get('tipo')
      this.id =params.get('id')
      //recibo el producto con un get segun su id ademas de tambien la valoracion
    });
    if(localStorage.getItem("token")){
          this.decoded=jwtDecode(localStorage.getItem("token"));
        }else if(sessionStorage.getItem("token")){
          this.decoded=jwtDecode(sessionStorage.getItem("token"));
        }
    this.getservicioproducto(this.id,this.tipo)
  }
  async getservicioproducto(id:string,tipo:string){
    let media=0
    const result= await this.list.getservicioproducto(id,tipo);
    if(result!=null){
      this.servicio=result
      console.log(this.servicio)
      console.log("Hola",this.servicio.valoracionesDto.length)
      for(let i=0; i<this.servicio.valoracionesDto.length; i++){
        media+=this.servicio.valoracionesDto[i].puntuacion
      }
      this.media=media
      console.log("MEDIA: "+this.media)
      console.log("SERVICIO O PRODUCTO:",this.servicio)
    }
  }
  async postreview(){
      console.log(this.subidareview.value);
    if(this.subidareview.valid){
      const valoracion:Enviovaloracion={Texto:this.subidareview.value.texto.trim(),Puntuacion: parseInt(this.subidareview.value.puntuacion),ServicioId:parseInt(this.id)}
      console.log(valoracion)
      await this.valoracionService.postvaloracion(valoracion)
      this.getservicioproducto(this.id,this.tipo)
    }else{
      alert("MACACO ESCRIBE O PUNTUA")
    }
    
  }
}
