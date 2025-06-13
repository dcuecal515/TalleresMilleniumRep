package com.example.talleresmileniumapp.Services

import com.example.talleresmileniumapp.Models.Service.ServiceResponse
import okhttp3.MultipartBody
import okhttp3.RequestBody
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.Multipart
import retrofit2.http.POST
import retrofit2.http.PUT
import retrofit2.http.Part

interface ServiceService {
    @GET("/api/Service/full")
    suspend fun GetAllService(@Header("Authorization") token:String): Response<List<ServiceResponse>>

    @Multipart
    @PUT("/api/Service/change")
    suspend fun updateService(
        @Header("Authorization") token: String,
        @Part("Id") id: RequestBody,
        @Part("Nombre") nombre: RequestBody,
        @Part("Descripcion") descripcion: RequestBody,
        @Part imagen: MultipartBody.Part?
    ): Response<Unit>

    @Multipart
    @POST("/api/Service/new")
    suspend fun addService(
        @Header("Authorization") token: String,
        @Part("Nombre") nombre: RequestBody,
        @Part("Descripcion") descripcion: RequestBody,
        @Part imagen: MultipartBody.Part?
    ): Response<Unit>
}