package com.example.talleresmileniumapp.Repositories

import com.example.talleresmileniumapp.Models.RetrofitApiInstance.userService
import com.example.talleresmileniumapp.Models.User.ChangeRolRequest
import com.example.talleresmileniumapp.Models.User.UserResponse

class UserRepository {
    suspend fun getalluser(token:String):List<UserResponse>?{
        val response= userService.getallUser("Bearer $token")

        return response.body()
    }
    suspend fun putadmin(token: String,id: String,rol: String){
        val request = ChangeRolRequest(id = id, rol = rol)
        userService.putadmin("Bearer $token",request)
    }
    suspend fun deleteuser(token: String,id: String){
        userService.deleteuser("Bearer $token",id)
    }
}