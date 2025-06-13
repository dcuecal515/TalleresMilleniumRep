package com.example.talleresmileniumapp.Repositories

import com.example.talleresmileniumapp.Models.Product.NewProduct
import com.example.talleresmileniumapp.Models.RetrofitApiInstance.productService
import com.example.talleresmileniumapp.Models.RetrofitApiInstance.serviceService
import com.example.talleresmileniumapp.Models.Service.NewService
import com.example.talleresmileniumapp.Models.Service.ServiceResponse
import com.example.talleresmileniumapp.Models.Service.UpdateService
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.MultipartBody
import okhttp3.RequestBody.Companion.toRequestBody

class ServiceRepository {
    suspend fun getAllService(token:String): List<ServiceResponse>? {
        val response=serviceService.GetAllService("Bearer $token")

        return response.body()
    }

    suspend fun updateService(token: String, updateService: UpdateService, imagenPart: MultipartBody.Part?){
        val idBody = updateService.Id.toRequestBody("text/plain".toMediaTypeOrNull())
        val nombreBody = updateService.Nombre.toRequestBody("text/plain".toMediaTypeOrNull())
        val descripcionBody = updateService.Descripcion.toRequestBody("text/plain".toMediaTypeOrNull())
        serviceService.updateService("Bearer $token",idBody,nombreBody,descripcionBody,imagenPart)
    }

    suspend fun addService(token: String, newService: NewService, imagenPart: MultipartBody.Part?){
        val nombreBody = newService.Nombre.toRequestBody("text/plain".toMediaTypeOrNull())
        val descripcionBody = newService.Descripcion.toRequestBody("text/plain".toMediaTypeOrNull())
        serviceService.addService("Bearer $token",nombreBody,descripcionBody,imagenPart)
    }
}