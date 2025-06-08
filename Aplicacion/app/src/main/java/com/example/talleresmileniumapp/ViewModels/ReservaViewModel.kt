package com.example.talleresmileniumapp.ViewModels

import android.annotation.SuppressLint
import android.app.Application
import android.util.Log
import androidx.lifecycle.AndroidViewModel
import androidx.lifecycle.viewModelScope
import com.example.talleresmileniumapp.Models.Reservas.ReservaResponse
import com.example.talleresmileniumapp.Models.User.UserResponse
import com.example.talleresmileniumapp.Repositories.ReservaRepository
import com.example.talleresmileniumapp.ViewModels.AuthViewModel.Companion.accessTokenSaved
import com.example.talleresmileniumapp.ViewModels.AuthViewModel.Companion.authDataStore
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch
import java.time.LocalDate

class ReservaViewModel( application: Application) : AndroidViewModel(application) {
    @SuppressLint("StaticFieldLeak")
    val context = application.baseContext

    val reservaRepository = ReservaRepository()

    private val _accessToken = MutableStateFlow<String?>("")
    val accessToken: StateFlow<String?> = _accessToken
    private val _reservasespera = MutableStateFlow<List<ReservaResponse>?>(null)
    val reservasespera: StateFlow<List<ReservaResponse>?> = _reservasespera
    private val _reservasfinal = MutableStateFlow<List<ReservaResponse>?>(null)
    val reservasfinal: StateFlow<List<ReservaResponse>?> = _reservasfinal

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
    suspend fun getallreservas(){
        val token=_accessToken.value
        val hoy = LocalDate.now()
        val enespera = mutableListOf<ReservaResponse>()
        val aceptadas = mutableListOf<ReservaResponse>()
        if(token!=null){
            val reservasList=reservaRepository.getallCoche_Servicio(token)
            reservasList?.forEach { reserva ->
                if(reserva.estado=="Reservado"){
                   enespera.add(reserva)
                }else if(reserva.estado=="Aceptado" && reserva.fecha<=hoy.toString()){
                    aceptadas.add(reserva)
                }
            }
            _reservasespera.value=enespera
            _reservasfinal.value=aceptadas
            Log.i("reservas",reservasList.toString())
            Log.i("espera",_reservasespera.value.toString())
            Log.i("aceptadas",_reservasfinal.value.toString())
        }else{
            throw IllegalStateException("NO PUTO")
        }
    }
    suspend fun putAceptada(fechaantigua:String,matricula:String,fechanueva:String){
        val token=_accessToken.value
        if(token!=null){
            reservaRepository.putAceptada(token,fechaantigua,fechanueva,matricula)
        }else{
            throw IllegalStateException("NO PUTO")
        }
        getallreservas()
    }
    suspend fun putFinalizado(fechaantigua: String,matricula: String){
        val token=_accessToken.value
        if(token!=null){
            reservaRepository.putFinalizado(token,fechaantigua,matricula)
        }else{
            throw IllegalStateException("NO PUTO")
        }
        getallreservas()
    }
    suspend fun deleteReserva(id: String){
        val token=_accessToken.value
        if(token!=null){
            reservaRepository.deleteReserva(token,id)
        }else{
            throw IllegalStateException("NO PUTO")
        }
        getallreservas()
    }

}