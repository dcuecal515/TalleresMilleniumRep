package com.example.talleresmileniumapp.Repositories

import com.example.talleresmileniumapp.Models.Product.NewProduct
import com.example.talleresmileniumapp.Models.Product.ProductResponse
import com.example.talleresmileniumapp.Models.Product.UpdateProduct
import com.example.talleresmileniumapp.Models.RetrofitApiInstance.productService
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.MultipartBody
import okhttp3.RequestBody.Companion.toRequestBody

class ProductRepository {
    suspend fun getProducts(token: String): List<ProductResponse>?{
        val response = productService.getFullProducts("Bearer $token")
        return response.body()
    }
    suspend fun updateProduct(token: String, updateProduct: UpdateProduct, imagenPart: MultipartBody.Part?){
        val idBody = updateProduct.Id.toRequestBody("text/plain".toMediaTypeOrNull())
        val nombreBody = updateProduct.Nombre.toRequestBody("text/plain".toMediaTypeOrNull())
        val descripcionBody = updateProduct.Descripcion.toRequestBody("text/plain".toMediaTypeOrNull())
        val disponibleBody = updateProduct.Disponible.toRequestBody("text/plain".toMediaTypeOrNull())
        productService.updateProduct("Bearer $token",idBody,nombreBody,descripcionBody,disponibleBody,imagenPart)
    }
    suspend fun deleteProduct(token: String, id: Int){
        productService.deleteProduct("Bearer $token",id)
    }

    suspend fun addProduct(token: String, newProduct: NewProduct, imagenPart: MultipartBody.Part?){
        val nombreBody = newProduct.Nombre.toRequestBody("text/plain".toMediaTypeOrNull())
        val descripcionBody = newProduct.Descripcion.toRequestBody("text/plain".toMediaTypeOrNull())
        val disponibleBody = newProduct.Disponible.toRequestBody("text/plain".toMediaTypeOrNull())
        productService.addProduct("Bearer $token",nombreBody,descripcionBody,disponibleBody,imagenPart)
    }
}