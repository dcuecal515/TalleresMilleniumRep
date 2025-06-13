import { ServicioCocheName } from "./servicioCocheName";
export interface CocheServicioFullDto {
    estado: string;
    fecha: string; 
    matricula: string;
    tipo: string;
    servicios: ServicioCocheName[];
}