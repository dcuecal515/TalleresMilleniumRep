import { Component } from '@angular/core';
import { HeaderComponent } from '../../component/header/header.component';
import { FooterComponent } from "../../component/footer/footer.component";
import { ActivatedRoute } from '@angular/router';
import { ListService } from '../../service/list.service';
import { Servicio } from '../../models/servicio';

@Component({
  selector: 'app-vista-producto',
  standalone: true,
  imports: [HeaderComponent, FooterComponent],
  templateUrl: './vista-producto.component.html',
  styleUrl: './vista-producto.component.css'
})
export class VistaProductoComponent {
  constructor(private route: ActivatedRoute,private list:ListService) { }
  tipo: string
  id: string
  ruta: string
  servicio:Servicio
  media:number
  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.tipo = params.get('tipo')
      this.id =params.get('id')
      this.getservicioproducto(this.id,this.tipo)
      //recibo el producto con un get segun su id ademas de tambien la valoracion
    });
  }
  async getservicioproducto(id:string,tipo:string){
    let media=0
    const result= await this.list.getservicioproducto(id,tipo);
    if(result!=null){
      this.servicio=result
      for(let i=0; i<this.servicio.valoraciones.length; i++){
        media+=this.servicio.valoraciones[i].puntuacion
      }
      this.media=media
      console.log("MEDIA: "+this.media)
      console.log("SERVICIO O PRODUCTO:"+this.servicio)
    }
  }
}
