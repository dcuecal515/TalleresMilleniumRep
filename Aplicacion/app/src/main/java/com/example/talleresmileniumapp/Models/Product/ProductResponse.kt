package com.example.talleresmileniumapp.Models.Product

data class ProductResponse(
    val id: Int,
    val nombre: String,
    val descripcion: String,
    val disponible: String,
    val imagen: String
)