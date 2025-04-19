import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { SobreNosotrosComponent } from './pages/sobre-nosotros/sobre-nosotros.component';
import { InicioSesionComponent } from './pages/inicio-sesion/inicio-sesion.component';

export const routes: Routes = [
    {path:'',component:HomeComponent},
    {path:'inicio-sesion',component:InicioSesionComponent},
    {path:'sobre-nosotros',component:SobreNosotrosComponent}
];
