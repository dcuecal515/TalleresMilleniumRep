package com.example.talleresmileniumapp.Repositories

import android.util.Log
import com.example.talleresmileniumapp.Models.Auth.AuthenticationRequest
import com.example.talleresmileniumapp.Models.Auth.AuthenticationResponse
import com.example.talleresmileniumapp.Models.RetrofitApiInstance.authService
import com.example.talleresmileniumapp.Models.RetrofitApiInstance.userService
import com.example.talleresmileniumapp.Models.User.UserResponse

class AuthRepository() {
    suspend fun login(email: String, password: String): AuthenticationResponse? {
        val response = authService.authenticate(
            AuthenticationRequest(
                Email = email,
                Password = password
            )
        )
        Log.i("TAG",response.body().toString())
        return response.body()
    }

    suspend fun getAuthUser(token:String): UserResponse?{
        val response = userService.getAuthUser("Bearer $token")

        return response.body()
    }
}