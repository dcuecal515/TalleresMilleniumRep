import { Coche } from "./Coche"
export interface FullUser{
    email:string,
    name:string,
    imagen:string,
    coches:Coche[]
}