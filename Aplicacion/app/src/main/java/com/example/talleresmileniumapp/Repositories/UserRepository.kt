package com.example.talleresmileniumapp.Repositories

import com.example.talleresmileniumapp.Models.RetrofitApiInstance.serviceService
import com.example.talleresmileniumapp.Models.RetrofitApiInstance.userService
import com.example.talleresmileniumapp.Models.User.UserResponse

class UserRepository {
    suspend fun getalluser(token:String):List<UserResponse>?{
        val response= userService.getallUser("Bearer $token")

        return response.body()
    }
}