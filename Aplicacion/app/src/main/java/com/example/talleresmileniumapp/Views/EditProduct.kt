package com.example.talleresmileniumapp.Views

import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.navigation.NavHostController
import com.example.talleresmileniumapp.Data.Routes
import com.example.talleresmileniumapp.ViewModels.AuthState
import com.example.talleresmileniumapp.ViewModels.AuthViewModel

@Composable
fun EditProduct(navController: NavHostController, authViewModel: AuthViewModel){

    val authState = authViewModel.authState.collectAsState()

    LaunchedEffect (authState.value){
        when(authState.value){
            is AuthState.Unauthenticated -> navController.navigate(Routes.Login.route)
            is AuthState.Error -> navController.navigate(Routes.Login.route)
            else -> Unit
        }
    }



}