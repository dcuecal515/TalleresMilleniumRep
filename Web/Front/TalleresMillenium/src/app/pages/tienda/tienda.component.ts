import { Component } from '@angular/core';
import { HeaderComponent } from '../../component/header/header.component';
import { ActivatedRoute,Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { FooterComponent } from '../../component/footer/footer.component';
import { Productlist } from '../../models/productlist';
import { ListService } from '../../service/list.service';
import { environment } from '../../../environments/environment.development';

@Component({
  selector: 'app-tienda',
  standalone: true,
  imports: [HeaderComponent,TranslateModule,FooterComponent],
  templateUrl: './tienda.component.html',
  styleUrl: './tienda.component.css'
})
export class TiendaComponent {
constructor(private route:ActivatedRoute,private router:Router,private Listservice:ListService){
}
tipo:string
id:number
ruta:string
listaservicios:Productlist[]=[]
ngOnInit(){
  this.route.paramMap.subscribe(params => {
    this.tipo=params.get('tipo')
  });
  if(this.tipo=="servicios"){
    this.getallservice()
  }else if(this.tipo=="productos"){

  }
}
search(){

}
goToContent(id: number) {
  if(this.tipo=="servicios"){
    this.ruta=`tienda/servicio/${id}`
  }else if(this.tipo=="productos"){
    this.ruta=`tienda/producto/${id}`
  }
  console.log(this.ruta)
  this.router.navigateByUrl(this.ruta)
}
async getallservice(){
  const result=await this.Listservice.getallservice()
  this.listaservicios=result.data
  console.log("antes",this.listaservicios)
  this.listaservicios.forEach(listaservicio => {
    listaservicio.imagen=environment.images+listaservicio.imagen
  });
  console.log("despues: ",this.listaservicios)
}
}
