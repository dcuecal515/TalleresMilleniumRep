import { Routes } from '@angular/router';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { InicioSesionComponent } from './inicio-sesion/inicio-sesion.component';
import { SobreNosotrosComponent } from './sobre-nosotros/sobre-nosotros.component';

export const routes: Routes = [
    {path:'',component:LandingPageComponent},
    {path:'inicio-sesion',component:InicioSesionComponent},
    {path:'sobre-nosotros',component:SobreNosotrosComponent}
];
