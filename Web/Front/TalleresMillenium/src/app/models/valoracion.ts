import { User } from "./user";

export interface Valoracion{
    id: number,
    texto: string,
    puntuacion: number,
    usuarioId: number,
    productoId: number,
    servicioId: number,
    user: User
}