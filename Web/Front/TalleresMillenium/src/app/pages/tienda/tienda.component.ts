import { Component } from '@angular/core';
import { HeaderComponent } from '../../component/header/header.component';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { FooterComponent } from '../../component/footer/footer.component';
import { Productlist } from '../../models/productlist';
import { ListService } from '../../service/list.service';
import { environment } from '../../../environments/environment.development';
import { querypage } from '../../models/querypage';
import { Productlistreal } from '../../models/productlistreal';
import { FormsModule } from '@angular/forms';
import { Productlistproduct } from '../../models/productlistproduct';
import { Productlist2 } from '../../models/productlist2';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tienda',
  standalone: true,
  imports: [HeaderComponent, TranslateModule, FooterComponent, FormsModule,CommonModule],
  templateUrl: './tienda.component.html',
  styleUrl: './tienda.component.css'
})
export class TiendaComponent {
  constructor(private route: ActivatedRoute, private router: Router, private Listservice: ListService) {
  }
  tipo: string
  id: number
  ruta: string
  listaservicios: Productlistreal
  listaserviciosultima: Productlist[]
  listaproductos: Productlistproduct
  listaproductosultima:Productlist2[]
  maximumPage: number;
  pageSize: number = 5
  actualPage: number = 1
  busqueda: string = ""
  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.tipo = params.get('tipo')
      if (this.tipo == "servicios") {
        if (sessionStorage.getItem("busqueda") != null) {
          this.busqueda = sessionStorage.getItem("busqueda")
        }
        this.getallservice({ busqueda: this.busqueda, ActualPage: this.actualPage, ServicePageSize: this.pageSize })
      } else if (this.tipo == "productos") {
        if (sessionStorage.getItem("busqueda") != null) {
          this.busqueda = sessionStorage.getItem("busqueda")
        }
        this.getallproduct({ busqueda: this.busqueda, ActualPage: this.actualPage, ServicePageSize: this.pageSize })
      } else {
        this.router.navigateByUrl("error")
      }
    });
  }
  search() {
    sessionStorage.setItem("busqueda", this.busqueda);
    if(this.tipo=="servicios"){
      this.getallservice({ busqueda: this.busqueda, ActualPage: this.actualPage, ServicePageSize: this.pageSize })
    } else if(this.tipo == "productos"){
      this.getallproduct({ busqueda: this.busqueda, ActualPage: this.actualPage, ServicePageSize: this.pageSize })
    }
  }
  goToContent(id: number) {
    if (this.tipo == "servicios") {
      this.ruta = `tienda/servicio/${id}`
    } else if (this.tipo == "productos") {
      this.ruta = `tienda/producto/${id}`
    }
    console.log(this.ruta)
    this.router.navigateByUrl(this.ruta)
  }
  async getallservice(Query: querypage) {
    const result = await this.Listservice.getallservice(Query)
    this.listaservicios = result.data
    console.log("antes", this.listaservicios)
    console.log("antes", this.listaservicios.serviceDtos)
    this.maximumPage = Math.ceil(this.listaservicios.totalservice / this.pageSize);
    this.listaserviciosultima = this.listaservicios.serviceDtos
    console.log("despues: ", this.listaserviciosultima)
    this.listaserviciosultima.forEach(listaservicio => {
      let valor = 0
      listaservicio.imagen = environment.images + listaservicio.imagen
      listaservicio.valoraciones.forEach(valoracion => {
        valor += valoracion
      })
      if (valor != 0) {
        listaservicio.valoracion = valor / listaservicio.valoraciones.length
      } else {
        listaservicio.valoracion = 0
      }
    });
    console.log("despues: ", this.listaserviciosultima)
  }

  async getallproduct(Query:querypage){
    const result=await this.Listservice.getallproduct(Query)
    this.listaproductos=result.data
    console.log("antes", this.listaproductos)
    console.log("antes", this.listaproductos.productDto)
    this.maximumPage = Math.ceil(this.listaproductos.totalproduct / this.pageSize);
    this.listaproductosultima = this.listaproductos.productDto
    console.log("despues: ", this.listaproductosultima)
    this.listaproductosultima.forEach(listaproducto => {
      let valor = 0
      listaproducto.imagen = environment.images + listaproducto.imagen
      listaproducto.valoraciones.forEach(valoracion => {
        valor += valoracion
      })
      if (valor != 0) {
        listaproducto.valoracion = valor / listaproducto.valoraciones.length
      } else {
        listaproducto.valoracion = 0
      }
    });
    console.log("despues: ", this.listaproductosultima)
  }

  async changeNumberOfGames() {
    const pagesSelect = document.getElementById("games-per-page") as HTMLInputElement | HTMLSelectElement;
    if (pagesSelect) {
      this.pageSize = parseInt(pagesSelect.value)
      this.actualPage = 1

      if(this.tipo=="servicios"){
        await this.getallservice({ busqueda: this.busqueda, ActualPage: this.actualPage, ServicePageSize: this.pageSize })
        this.maximumPage = Math.ceil(this.listaservicios.totalservice / this.pageSize);
      }else if(this.tipo=="productos"){
        await this.getallproduct({ busqueda: this.busqueda, ActualPage: this.actualPage, ServicePageSize: this.pageSize })
        this.maximumPage = Math.ceil(this.listaproductos.totalproduct / this.pageSize);
      }

      
      console.log("Hola" + this.maximumPage)
      // disabled botones prev y first
      const firstBtn = document.getElementById("firstBtn") as HTMLButtonElement
      const prevBtn = document.getElementById("prevBtn") as HTMLButtonElement
      firstBtn.disabled = true
      prevBtn.disabled = true
      if (this.actualPage < this.maximumPage) {
        // disabled botones next y last
        const nextBtn = document.getElementById("nextBtn") as HTMLButtonElement
        const lastBtn = document.getElementById("lastBtn") as HTMLButtonElement
        nextBtn.disabled = false
        lastBtn.disabled = false
      }
    }
  }

  async firstPage() {
    if (this.actualPage > 1) {
      this.actualPage = 1
      // Hacer disabled los botones prev y frist
      const firstBtn = document.getElementById("firstBtn") as HTMLButtonElement
      const prevBtn = document.getElementById("prevBtn") as HTMLButtonElement
      firstBtn.disabled = true
      prevBtn.disabled = true
      if (this.actualPage < this.maximumPage) {
        // hacer enabled los botones next y last
        const nextBtn = document.getElementById("nextBtn") as HTMLButtonElement
        const lastBtn = document.getElementById("lastBtn") as HTMLButtonElement
        nextBtn.disabled = false
        lastBtn.disabled = false
      }
      if(this.tipo=="servicios"){
        await this.getallservice({ busqueda: this.busqueda, ActualPage: this.actualPage, ServicePageSize: this.pageSize })
      }else if(this.tipo=="productos"){
        await this.getallproduct({ busqueda: this.busqueda, ActualPage: this.actualPage, ServicePageSize: this.pageSize })
      }
    }
  }

  async prevPage() {
    if (this.actualPage > 1) {
      this.actualPage = this.actualPage - 1
      if (this.actualPage == 1) {
        // hacer disabled los botones prev y first
        const firstBtn = document.getElementById("firstBtn") as HTMLButtonElement
        const prevBtn = document.getElementById("prevBtn") as HTMLButtonElement
        firstBtn.disabled = true
        prevBtn.disabled = true
      }
      if (this.actualPage < this.maximumPage) {
        // hacer enabled los botones next y last
        const nextBtn = document.getElementById("nextBtn") as HTMLButtonElement
        const lastBtn = document.getElementById("lastBtn") as HTMLButtonElement
        nextBtn.disabled = false
        lastBtn.disabled = false
      }
      if(this.tipo=="servicios"){
        await this.getallservice({ busqueda: this.busqueda, ActualPage: this.actualPage, ServicePageSize: this.pageSize })
      }else if(this.tipo=="productos"){
        await this.getallproduct({ busqueda: this.busqueda, ActualPage: this.actualPage, ServicePageSize: this.pageSize })
      }
    }
  }

  async nextPage() {
    if (this.actualPage < this.maximumPage) {
      this.actualPage = this.actualPage + 1
      // enabled botones prev y first
      const firstBtn = document.getElementById("firstBtn") as HTMLButtonElement
      const prevBtn = document.getElementById("prevBtn") as HTMLButtonElement
      firstBtn.disabled = false
      prevBtn.disabled = false
      if (this.actualPage == this.maximumPage) {
        // disabled botones next y last
        const nextBtn = document.getElementById("nextBtn") as HTMLButtonElement
        const lastBtn = document.getElementById("lastBtn") as HTMLButtonElement
        nextBtn.disabled = true
        lastBtn.disabled = true
      }
      if(this.tipo=="servicios"){
        await this.getallservice({ busqueda: this.busqueda, ActualPage: this.actualPage, ServicePageSize: this.pageSize })
      }else if(this.tipo=="productos"){
        await this.getallproduct({ busqueda: this.busqueda, ActualPage: this.actualPage, ServicePageSize: this.pageSize })
      }
    }
  }

  async lastPage() {
    if (this.actualPage < this.maximumPage) {
      this.actualPage = this.maximumPage
      // enable botones prev y first
      const firstBtn = document.getElementById("firstBtn") as HTMLButtonElement
      const prevBtn = document.getElementById("prevBtn") as HTMLButtonElement
      firstBtn.disabled = false
      prevBtn.disabled = false
      // disabled botones next y last
      const nextBtn = document.getElementById("nextBtn") as HTMLButtonElement
      const lastBtn = document.getElementById("lastBtn") as HTMLButtonElement
      nextBtn.disabled = true
      lastBtn.disabled = true
      if(this.tipo=="servicios"){
        await this.getallservice({ busqueda: this.busqueda, ActualPage: this.actualPage, ServicePageSize: this.pageSize })
      }else if(this.tipo=="productos"){
        await this.getallproduct({ busqueda: this.busqueda, ActualPage: this.actualPage, ServicePageSize: this.pageSize })
      }
    }
  }
}
