package com.example.talleresmileniumapp.Models.Auth

import com.example.talleresmileniumapp.Services.AuthService
import com.example.talleresmileniumapp.Services.UserService
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory


object RetrofitApiInstance {
    private const val BASE_URL = "https://localhost:7027"

    private val retrofit: Retrofit by lazy {
        Retrofit.Builder()
            .baseUrl(RetrofitApiInstance.BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
    }

    val authService: AuthService by lazy {
        retrofit.create(AuthService::class.java)
    }

    val userService: UserService by lazy {
        retrofit.create(UserService::class.java)
    }
}