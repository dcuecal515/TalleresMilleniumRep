package com.example.talleresmileniumapp.ViewModels

import android.annotation.SuppressLint
import android.app.Application
import android.content.Context
import android.util.Log
import androidx.datastore.core.DataStore
import androidx.datastore.preferences.core.Preferences
import androidx.datastore.preferences.core.stringPreferencesKey
import androidx.datastore.preferences.preferencesDataStore
import androidx.lifecycle.AndroidViewModel
import androidx.lifecycle.viewModelScope
import com.example.talleresmileniumapp.Dialog.AuthErrorType
import com.example.talleresmileniumapp.Models.Product.ProductResponse
import com.example.talleresmileniumapp.Models.Product.UpdateProduct
import com.example.talleresmileniumapp.Repositories.ProductRepository
import com.example.talleresmileniumapp.ViewModels.AuthViewModel.Companion.accessTokenSaved
import com.example.talleresmileniumapp.ViewModels.AuthViewModel.Companion.authDataStore
import com.example.talleresmileniumapp.ViewModels.AuthViewModel.Companion.emailSaved
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.MultipartBody
import okhttp3.RequestBody
import okhttp3.RequestBody.Companion.asRequestBody
import java.io.File

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
    private val _producto = MutableStateFlow<ProductResponse?>(null)
    val producto: StateFlow<ProductResponse?> = _producto

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

    fun selectProduct(producto: ProductResponse){
        _producto.value = producto
    }

    suspend fun updateProduct(updateProduct: UpdateProduct, file: File?){
        val token = _accessToken.value
        Log.i("TAG","Disponible: "+updateProduct.Disponible)
        if(token != null){
            if(file != null){
                val requestFile: RequestBody = file.asRequestBody("image/*".toMediaTypeOrNull())
                val imagenPart: MultipartBody.Part = MultipartBody.Part.createFormData("Imagen", file.name, requestFile)
                productR.updateProduct(token,updateProduct,imagenPart)
            }else{
                val imagenPart = null
                productR.updateProduct(token,updateProduct,imagenPart)
            }
        }else{
            throw IllegalStateException("Access token is null")
        }
    }

    suspend fun deleteProduct(id: Int){
        val token = _accessToken.value
        if(token != null){
            productR.deleteProduct(token,id)
            getProducts()
        }else{
            throw IllegalStateException("Access token is null")
        }
    }
}