package com.example.talleresmileniumapp.Repositories

import com.example.talleresmileniumapp.Models.RetrofitApiInstance.serviceService
import com.example.talleresmileniumapp.Models.Service.ServiceResponse

class ServiceRepository {
    suspend fun getAllService(token:String): List<ServiceResponse>? {
        val response=serviceService.GetAllService("Bearer $token")

        return response.body()
    }
}