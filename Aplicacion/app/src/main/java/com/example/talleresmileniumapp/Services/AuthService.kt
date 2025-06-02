package com.example.talleresmileniumapp.Services

import com.example.talleresmileniumapp.Models.Auth.AuthenticationRequest
import com.example.talleresmileniumapp.Models.Auth.AuthenticationResponse
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST

interface AuthService {
    @POST("/api/Auth/iniciarAdmin")
    suspend fun authenticate(@Body authRequest: AuthenticationRequest): Response<AuthenticationResponse>
}