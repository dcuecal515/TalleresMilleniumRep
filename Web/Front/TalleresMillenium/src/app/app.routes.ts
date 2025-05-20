import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { InicioSesionComponent } from './pages/inicio-sesion/inicio-sesion.component';
import { SobreNosotrosComponent } from './pages/sobre-nosotros/sobre-nosotros.component';
import { TiendaComponent } from './pages/tienda/tienda.component';
import { VistaProductoComponent } from './pages/vista-producto/vista-producto.component';
import { PerfilComponent } from './pages/perfil/perfil.component';

export const routes: Routes = [
    {path:'',component:HomeComponent},
    {path:'inicio-sesion',component:InicioSesionComponent},
    {path:'sobre-nosotros',component:SobreNosotrosComponent},
    {path:'perfil',component:PerfilComponent},
    {path:'tienda/:tipo',component:TiendaComponent},
    {path:'tienda/:tipo/:id',component:VistaProductoComponent}
];
