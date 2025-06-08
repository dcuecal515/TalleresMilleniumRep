package com.example.talleresmileniumapp.Models.Reservas



data class ReservaResponse (
    val estado: String,
    val fecha: String,
    val servicios: List<Servicios>,
    val matricula: String,
    val tipo: String
)
