package com.example.talleresmileniumapp.ViewModels

import android.annotation.SuppressLint
import android.app.Application
import android.util.Log
import androidx.lifecycle.AndroidViewModel
import androidx.lifecycle.viewModelScope
import com.example.talleresmileniumapp.Dialog.AuthErrorType
import com.example.talleresmileniumapp.Models.Product.ProductResponse
import com.example.talleresmileniumapp.Repositories.ProductRepository
import com.example.talleresmileniumapp.ViewModels.AuthViewModel.Companion.accessTokenSaved
import com.example.talleresmileniumapp.ViewModels.AuthViewModel.Companion.authDataStore
import com.example.talleresmileniumapp.ViewModels.AuthViewModel.Companion.emailSaved
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

class ProductViewModel (application: Application) : AndroidViewModel(application){
    @SuppressLint("StaticFieldLeak")
    val context = application.baseContext

    val productR = ProductRepository()


    //Datos del usuario
    private val _accessToken = MutableStateFlow<String?>("")
    val accessToken: StateFlow<String?> = _accessToken

    // Datos de los productos
    private val _productos = MutableStateFlow<List<ProductResponse>?>(null)
    val productos: StateFlow<List<ProductResponse>?> = _productos

    init {
        loadData()

    }

    private fun loadData() {
        viewModelScope.launch {
            context.authDataStore.data
                .collect { preferences ->
                    _accessToken.value = preferences[accessTokenSaved]
                }
        }
    }

    suspend fun getProducts(){
        val token = _accessToken.value
        if(token != null){
            val productList = productR.getProducts(token)
            _productos.value = productList
            Log.i("TAG",_productos.value.toString())
        }else{
            throw IllegalStateException("Access token is null")
        }
    }
}