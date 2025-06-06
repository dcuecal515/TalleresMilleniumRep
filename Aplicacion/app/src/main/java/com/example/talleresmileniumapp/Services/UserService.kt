package com.example.talleresmileniumapp.Services

import com.example.talleresmileniumapp.Models.User.UserResponse
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Header

interface UserService {
    @GET("/api/User")
    suspend fun getallUser(@Header("Authorization") token: String): Response<List<UserResponse>>
}