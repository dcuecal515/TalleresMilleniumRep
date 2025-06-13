package com.example.talleresmileniumapp.ViewModels

import android.annotation.SuppressLint
import android.app.Application
import android.content.Context
import android.util.Log
import androidx.lifecycle.AndroidViewModel
import androidx.lifecycle.viewModelScope
import com.example.talleresmileniumapp.Models.Product.NewProduct
import com.example.talleresmileniumapp.Models.Product.ProductResponse
import com.example.talleresmileniumapp.Models.Service.NewService
import com.example.talleresmileniumapp.Models.Service.ServiceResponse
import com.example.talleresmileniumapp.Models.Service.UpdateService
import com.example.talleresmileniumapp.Repositories.ServiceRepository
import com.example.talleresmileniumapp.ViewModels.AuthViewModel.Companion.accessTokenSaved
import com.example.talleresmileniumapp.ViewModels.AuthViewModel.Companion.authDataStore
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.MultipartBody
import okhttp3.RequestBody
import okhttp3.RequestBody.Companion.asRequestBody
import java.io.File

class ServiceViewModel( application: Application) : AndroidViewModel(application){
    @SuppressLint("StaticFieldLeak")
    val context = application.baseContext

    val serviceRepository=ServiceRepository()

    private val _accessToken = MutableStateFlow<String?>("")
    val accessToken: StateFlow<String?> = _accessToken
    private val _services = MutableStateFlow<List<ServiceResponse>?>(null)
    val services: StateFlow<List<ServiceResponse>?> = _services
    private val _service = MutableStateFlow<ServiceResponse?>(null)
    val service: StateFlow<ServiceResponse?> = _service

    init {
        loadData()
    }

    //Obtiene los datos del Data Store
    private fun loadData() {
        viewModelScope.launch {
            // Recuperar los datos del DataStore
            context.authDataStore.data
                .collect { preferences ->
                    _accessToken.value = preferences[accessTokenSaved]
                }
        }
    }
    suspend fun getallservice(){
        val token=_accessToken.value
        if(token!=null){
            val serviceList=serviceRepository.getAllService(token)
            _services.value=serviceList
            Log.i("tag",_services.value.toString())
        }else{
            throw IllegalStateException("NO PUTO")
        }
    }

    fun selectService(service: ServiceResponse){
        _service.value = service
    }

    suspend fun updateService(updateService: UpdateService, file: File?){
        val token = _accessToken.value
        if(token != null){
            if(file != null){
                val requestFile: RequestBody = file.asRequestBody("image/*".toMediaTypeOrNull())
                val imagenPart: MultipartBody.Part = MultipartBody.Part.createFormData("Imagen", file.name, requestFile)
                serviceRepository.updateService(token,updateService,imagenPart)
                getallservice()
            }else{
                val imagenPart = null
                serviceRepository.updateService(token,updateService,imagenPart)
            }
        }else{
            throw IllegalStateException("Access token is null")
        }
    }

    suspend fun addService(newService: NewService, file: File){
        var token = _accessToken.value
        if(token != null){
            val requestFile: RequestBody = file.asRequestBody("image/*".toMediaTypeOrNull())
            val imagenPart: MultipartBody.Part = MultipartBody.Part.createFormData("Imagen",file.name,requestFile)
            serviceRepository.addService(token,newService,imagenPart)
            getallservice()
        }else{
            throw IllegalStateException("Access token is null")
        }
    }
}