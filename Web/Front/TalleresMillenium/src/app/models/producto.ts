import { Valoracion } from "./valoracion"

export interface Producto{
    id:number
    nombre:string
    descripcion:string
    imagen:string
    disponible:string
    valoracionesDto:Valoracion[]
}