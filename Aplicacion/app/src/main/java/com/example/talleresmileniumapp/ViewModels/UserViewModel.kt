package com.example.talleresmileniumapp.ViewModels

import android.annotation.SuppressLint
import android.app.Application
import android.util.Log
import androidx.lifecycle.AndroidViewModel
import androidx.lifecycle.viewModelScope
import com.example.talleresmileniumapp.Models.User.UserResponse
import com.example.talleresmileniumapp.Repositories.UserRepository
import com.example.talleresmileniumapp.ViewModels.AuthViewModel.Companion.accessTokenSaved
import com.example.talleresmileniumapp.ViewModels.AuthViewModel.Companion.authDataStore
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

class UserViewModel( application: Application) : AndroidViewModel(application){
    @SuppressLint("StaticFieldLeak")
    val context = application.baseContext

    val userRepository= UserRepository()

    private val _accessToken = MutableStateFlow<String?>("")
    val accessToken: StateFlow<String?> = _accessToken
    private val _users = MutableStateFlow<List<UserResponse>?>(null)
    val services: StateFlow<List<UserResponse>?> = _users

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
    suspend fun getalluser(){
        val token=_accessToken.value
        if(token!=null){
            val userList=userRepository.getalluser(token)
            _users.value=userList
            Log.i("tag",_users.value.toString())
        }else{
            throw IllegalStateException("NO PUTO")
        }
    }
}