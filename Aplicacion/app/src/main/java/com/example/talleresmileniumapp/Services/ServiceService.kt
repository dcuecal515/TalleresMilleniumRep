package com.example.talleresmileniumapp.Services

import com.example.talleresmileniumapp.Models.Service.ServiceResponse
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Header

interface ServiceService {
    @GET("/api/Service/full")
    suspend fun GetAllService(@Header("Authorization") token:String): Response<List<ServiceResponse>>
}