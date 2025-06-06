package com.example.talleresmileniumapp.Views

import android.widget.Toast
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.core.content.ContextCompat.getString
import androidx.navigation.NavHostController
import com.example.talleresmileniumapp.Data.Routes
import com.example.talleresmileniumapp.Dialog.ToastMessage
import com.example.talleresmileniumapp.ViewModels.AuthState
import com.example.talleresmileniumapp.ViewModels.AuthViewModel
import com.example.talleresmileniumapp.ViewModels.ServiceViewModel

@Composable
fun Servicios(navController: NavHostController, authViewModel: AuthViewModel,serviceViewModel: ServiceViewModel){
    val context = LocalContext.current
    val accessToken by serviceViewModel.accessToken.collectAsState()

    val authState = authViewModel.authState.collectAsState()

    LaunchedEffect (authState.value){
        when(authState.value){
            is AuthState.Unauthenticated -> navController.navigate(Routes.Login.route)
            else -> Unit
        }
    }
    LaunchedEffect(accessToken) {
        accessToken?.let {
            serviceViewModel.getallservice()
        }
    }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .background(MaterialTheme.colorScheme.background),
        verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally,

        )
    {
        Text("Servicios")
    }
}