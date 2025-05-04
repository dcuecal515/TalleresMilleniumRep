import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { InicioSesionComponent } from './pages/inicio-sesion/inicio-sesion.component';
import { SobreNosotrosComponent } from './pages/sobre-nosotros/sobre-nosotros.component';
import { ChatComponent } from './component/chat/chat.component';

export const routes: Routes = [
    {path:'',component:HomeComponent},
    {path:'inicio-sesion',component:InicioSesionComponent},
    {path:'sobre-nosotros',component:SobreNosotrosComponent},
    {path:'chat',component:ChatComponent}
];
