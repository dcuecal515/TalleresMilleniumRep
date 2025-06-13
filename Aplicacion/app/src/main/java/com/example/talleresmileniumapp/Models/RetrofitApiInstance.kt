package com.example.talleresmileniumapp.Models

import com.example.talleresmileniumapp.Services.AuthService
import com.example.talleresmileniumapp.Services.ServiceService
import com.example.talleresmileniumapp.Services.ProductService
import com.example.talleresmileniumapp.Services.ReservaService
import com.example.talleresmileniumapp.Services.UserService
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

object RetrofitApiInstance {
    private const val BASE_URL = "https://talleresmilemiun.runasp.net"

    private val retrofit: Retrofit by lazy {
        Retrofit.Builder()
            .baseUrl(BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
    }

    val authService: AuthService by lazy {
        retrofit.create(AuthService::class.java)
    }

    val userService: UserService by lazy {
        retrofit.create(UserService::class.java)
    }
    
    val serviceService:ServiceService by lazy{
        retrofit.create(ServiceService::class.java)
    }

    val productService: ProductService by lazy {
        retrofit.create(ProductService::class.java)
    }
    val reservaService: ReservaService by lazy {
        retrofit.create(ReservaService::class.java)
    }
}