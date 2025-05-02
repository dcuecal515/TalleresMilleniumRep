import { Component } from '@angular/core';
import { HeaderComponent } from '../../component/header/header.component';
import { ActivatedRoute } from '@angular/router';
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
constructor(private route:ActivatedRoute){}
tipo:string

ngOnInit(){
  this.route.paramMap.subscribe(params => {
    this.tipo=params.get('tipo')
  });
}
search(){

}
}
