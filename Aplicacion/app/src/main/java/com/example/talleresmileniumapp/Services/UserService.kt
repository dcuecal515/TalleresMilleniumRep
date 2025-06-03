package com.example.talleresmileniumapp.Services

import com.example.talleresmileniumapp.Models.Auth.UserResponse
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Header

interface UserService {
    //Método solo disponible con token
    @GET("/api/User/")
    suspend fun getAuthUser(@Header("Authorization") token: String): Response<UserResponse>
}