package com.example.talleresmileniumapp.Services

import com.example.talleresmileniumapp.Models.Product.ProductResponse
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Header

interface ProductService {
    @GET("/api/Product/full")
    suspend fun getFullProducts(@Header("Authorization") token: String): Response<List<ProductResponse>>
}