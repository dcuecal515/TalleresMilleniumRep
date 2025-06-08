package com.example.talleresmileniumapp.Services

import com.example.talleresmileniumapp.Models.Product.ProductResponse
import okhttp3.MultipartBody
import okhttp3.RequestBody
import retrofit2.Response
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.Multipart
import retrofit2.http.POST
import retrofit2.http.PUT
import retrofit2.http.Part
import retrofit2.http.Query

interface ProductService {
    @GET("/api/Product/full")
    suspend fun getFullProducts(@Header("Authorization") token: String): Response<List<ProductResponse>>

    @Multipart
    @PUT("/api/Product/change")
    suspend fun updateProduct(
        @Header("Authorization") token: String,
        @Part("Id") id: RequestBody,
        @Part("Nombre") nombre: RequestBody,
        @Part("Descripcion") descripcion: RequestBody,
        @Part("Disponible") disponible: RequestBody,
        @Part imagen: MultipartBody.Part?
    ): Response<Unit>

    @DELETE("/api/Product")
    suspend fun deleteProduct(@Header("Authorization") token: String, @Query("id") id:Int)

    @Multipart
    @POST("/api/Product/new")
    suspend fun addProduct(
        @Header("Authorization") token: String,
        @Part("Nombre") nombre: RequestBody,
        @Part("Descripcion") descripcion: RequestBody,
        @Part("Disponible") disponible: RequestBody,
        @Part imagen: MultipartBody.Part?
        ): Response<Unit>
}