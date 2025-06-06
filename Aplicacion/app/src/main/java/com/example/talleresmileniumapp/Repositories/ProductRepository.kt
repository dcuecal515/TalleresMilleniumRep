package com.example.talleresmileniumapp.Repositories

import com.example.talleresmileniumapp.Models.Product.ProductResponse
import com.example.talleresmileniumapp.Models.RetrofitApiInstance.productService

class ProductRepository {
    suspend fun getProducts(token: String): List<ProductResponse>?{
        val response = productService.getFullProducts("Bearer $token")
        return response.body()
    }
}