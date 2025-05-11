import { Component } from '@angular/core';
import { HeaderComponent } from '../../component/header/header.component';
import { ActivatedRoute,Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { FooterComponent } from '../../component/footer/footer.component';

@Component({
  selector: 'app-tienda',
  standalone: true,
  imports: [HeaderComponent,TranslateModule,FooterComponent],
  templateUrl: './tienda.component.html',
  styleUrl: './tienda.component.css'
})
export class TiendaComponent {
constructor(private route:ActivatedRoute,private router:Router){
  this.id=1
}
tipo:string
id:number
ruta:string
ngOnInit(){
  this.route.paramMap.subscribe(params => {
    this.tipo=params.get('tipo')
  });
}
search(){

}
goToContent(id: number) {
  if(this.tipo=="servicios"){
    console.log("hola")
    this.ruta=`tienda/servicio/${id}`
  }else if(this.tipo=="productos"){
    console.log("hola")
    this.ruta=`tienda/producto/${id}`
  }
  console.log(this.ruta)
  this.router.navigateByUrl(this.ruta)
}
}
