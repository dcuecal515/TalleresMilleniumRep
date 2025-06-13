package com.example.talleresmileniumapp.ViewModels

import android.annotation.SuppressLint
import android.app.Application
import android.content.Context
import android.util.Log
import androidx.datastore.core.DataStore
import androidx.datastore.preferences.core.Preferences
import androidx.datastore.preferences.core.edit
import androidx.datastore.preferences.core.stringPreferencesKey
import androidx.datastore.preferences.preferencesDataStore
import androidx.lifecycle.AndroidViewModel
import androidx.lifecycle.viewModelScope
import com.example.talleresmileniumapp.Dialog.AuthErrorType
import com.example.talleresmileniumapp.Repositories.AuthRepository
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch
import com.auth0.android.jwt.JWT

private val dataStoreName = "talleres_milenium_app_authentication";

class AuthViewModel( application: Application) : AndroidViewModel(application){
    @SuppressLint("StaticFieldLeak")
    val context = application.baseContext

    companion object {
        val Context.authDataStore: DataStore<Preferences> by preferencesDataStore(
            dataStoreName
        )

        //Datos del usuario
        val userNameSaved = stringPreferencesKey("userName")
        val emailSaved = stringPreferencesKey("email")
        val accessTokenSaved = stringPreferencesKey("accessToken")

    }

    //Repositorio
    val auth = AuthRepository()

    //Variable para poder cambiar los estados
    private val _authState = MutableStateFlow<AuthState>(AuthState.Unauthenticated)
    //Variable para poder verlos en las vistas
    var authState: StateFlow<AuthState> = _authState
    private val _userName = MutableStateFlow<String?>("")
    val userName: StateFlow<String?> = _userName
    private val _email = MutableStateFlow<String?>("")
    val email: StateFlow<String?> = _email
    private val _accessToken = MutableStateFlow<String?>("")
    val accessToken: StateFlow<String?> = _accessToken

    init {
        loadData()
    }

    //Obtiene los datos del Data Store
    private fun loadData() {
        viewModelScope.launch {
            // Recuperar los datos del DataStore
            context.authDataStore.data
                .collect { preferences ->
                    _userName.value = preferences[userNameSaved]
                    _email.value = preferences[emailSaved]
                    _accessToken.value = preferences[accessTokenSaved]
                    if (!_accessToken.value.isNullOrEmpty()) {
                        _authState.value = AuthState.Authenticated
                    }
                }

        }
    }

    suspend fun saveData(userName: String, email: String, accessToken: String) {

        _userName.value = userName
        _email.value = email
        _accessToken.value = accessToken

        context.authDataStore.edit { preferences ->
            preferences[userNameSaved] = userName
            preferences[emailSaved] = email
            preferences[accessTokenSaved] = accessToken
        }
    }

    fun login(email : String, password : String){
        if (email.isEmpty() || password.isEmpty()){
            _authState.value = AuthState.Error(AuthErrorType.EMPTY_CREDENTIALS)
            return
        }
        viewModelScope.launch {
            loading()

            val response = auth.login(email,password)

            if (response != null){
                getUserDataAndSave(response.accessToken)
                _authState.value = AuthState.Authenticated

            } else {
                _authState.value = AuthState.Error(AuthErrorType.INVALID_CREDENTIALS)
            }
        }

    }

    fun signout(){

        viewModelScope.launch {
            saveData("", "", "")
            _authState.value = AuthState.Unauthenticated
        }

    }

    fun loading(){
        viewModelScope.launch{
            _authState.value = AuthState.Loading
        }
    }

    suspend fun getUserDataAndSave(accessToken: String) {
        val jwt = JWT(accessToken)
        val email = jwt.getClaim("email").asString()
        val name = jwt.getClaim("name").asString()

        if (email != null && name != null) {
            saveData(name,
                email,
                accessToken)
        }
    }

}

sealed class AuthState{
    object Authenticated : AuthState()
    object Unauthenticated : AuthState()
    object Loading : AuthState()
    data class Error(val errorType: AuthErrorType) : AuthState()
}