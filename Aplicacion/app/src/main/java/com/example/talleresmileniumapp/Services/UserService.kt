package com.example.talleresmileniumapp.Services

import com.example.talleresmileniumapp.Models.User.ChangeRolRequest
import com.example.talleresmileniumapp.Models.User.UserResponse
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.PUT
import retrofit2.http.Query

interface UserService {
    @GET("/api/User")
    suspend fun getallUser(@Header("Authorization") token: String): Response<List<UserResponse>>
    @PUT("/api/User/change")
    suspend fun putadmin(@Header("Authorization") token: String,@Body request: ChangeRolRequest):Response<Unit>
    @DELETE("/api/User")
    suspend fun deleteuser(@Header("Authorization") token: String,@Query("id") id: String):Response<Unit>
}