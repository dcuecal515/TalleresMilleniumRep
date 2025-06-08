package com.example.talleresmileniumapp.Repositories

import com.example.talleresmileniumapp.Models.Reservas.ReservaFinalRequest
import com.example.talleresmileniumapp.Models.Reservas.ReservaRequest
import com.example.talleresmileniumapp.Models.Reservas.ReservaResponse
import com.example.talleresmileniumapp.Models.RetrofitApiInstance.reservaService

class ReservaRepository {
    suspend fun getallCoche_Servicio(token: String): List<ReservaResponse>?{
        val response= reservaService.getallCoche_Servicio("Bearer $token")
        return response.body()
    }
    suspend fun putAceptada(token: String,fechaantigua:String,fechanueva: String,matricula:String){
        val request= ReservaRequest(fechanueva=fechanueva,fechaantigua=fechaantigua,matricula=matricula)
        reservaService.putAceptada("Bearer $token",request)
    }
    suspend fun putFinalizado(token: String,fechaantigua:String,matricula:String){
        val request= ReservaFinalRequest(fechaantigua=fechaantigua,matricula=matricula)
        reservaService.putFinalizado("Bearer $token",request)
    }
}