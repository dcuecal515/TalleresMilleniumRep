package com.example.talleresmileniumapp.Repositories

import com.example.talleresmileniumapp.Models.Auth.AuthenticationRequest
import com.example.talleresmileniumapp.Models.Auth.AuthenticationResponse
import com.example.talleresmileniumapp.Models.Auth.RetrofitApiInstance.authService
import com.example.talleresmileniumapp.Models.Auth.RetrofitApiInstance.userService
import com.example.talleresmileniumapp.Models.Auth.UserResponse

class AuthRepository() {
    suspend fun login(email: String, password: String): AuthenticationResponse? {
        val response = authService.authenticate(
            AuthenticationRequest(
                email = email,
                password = password
            )
        )
        return response.body()
    }

    suspend fun getAuthUser(token:String): UserResponse?{
        val response = userService.getAuthUser("Bearer $token")

        return response.body()
    }
}