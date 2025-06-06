package com.example.talleresmileniumapp.Repositories

import android.util.Log
import com.example.talleresmileniumapp.Models.Auth.AuthenticationRequest
import com.example.talleresmileniumapp.Models.Auth.AuthenticationResponse
import com.example.talleresmileniumapp.Models.RetrofitApiInstance.authService
import com.example.talleresmileniumapp.Models.RetrofitApiInstance.userService

class AuthRepository() {
    suspend fun login(email: String, password: String): AuthenticationResponse? {
        val response = authService.authenticate(
            AuthenticationRequest(
                Email = email,
                Password = password
            )
        )
        return response.body()
    }
}