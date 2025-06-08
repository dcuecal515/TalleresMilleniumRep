package com.example.talleresmileniumapp.Services

import com.example.talleresmileniumapp.Models.Reservas.ReservaFinalRequest
import com.example.talleresmileniumapp.Models.Reservas.ReservaRequest
import com.example.talleresmileniumapp.Models.Reservas.ReservaResponse
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.PUT

interface ReservaService {
    @GET("/api/Coche_Servicio")
    suspend fun getallCoche_Servicio(@Header("Authorization") token: String): Response<List<ReservaResponse>>
    @PUT("/api/Coche_Servicio/aceptar")
    suspend fun putAceptada(@Header("Authorization") token: String,@Body request: ReservaRequest)
    @PUT("/api/Coche_Servicio/finalizar")
    suspend fun putFinalizado(@Header("Authorization") token: String,@Body request: ReservaFinalRequest)
}