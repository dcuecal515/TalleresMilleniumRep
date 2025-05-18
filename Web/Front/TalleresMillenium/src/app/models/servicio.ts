import { Valoracion } from "./valoracion"

export interface Servicio{
    id:number
    nombre:string
    descripcion:string
    imagen:string
    valoraciones:Valoracion[]
}