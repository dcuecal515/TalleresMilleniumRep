import { Coche_Servicio } from "./Coche_Servicio"
export interface Coche{
    tipo:string,
    imagen:string,
    matricula:string,
    fecha_itv:string,
    combustible:string,
    kilometraje:number,
    servicios:Coche_Servicio[]
}