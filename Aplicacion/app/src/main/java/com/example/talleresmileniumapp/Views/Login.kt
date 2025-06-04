package com.example.talleresmileniumapp.Views

import android.widget.Toast
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Button
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.core.content.ContextCompat.getString
import androidx.navigation.NavHostController
import com.example.talleresmileniumapp.Data.Routes
import com.example.talleresmileniumapp.Dialog.ToastMessage
import com.example.talleresmileniumapp.InsertTitle
import com.example.talleresmileniumapp.R
import com.example.talleresmileniumapp.Themes.misFormas
import com.example.talleresmileniumapp.ViewModels.AuthState
import com.example.talleresmileniumapp.ViewModels.AuthViewModel

@Composable
fun Login(navController: NavHostController, authViewModel: AuthViewModel){

    val context = LocalContext.current

    var email by remember {
        mutableStateOf("")
    }

    var password by remember {
        mutableStateOf("")
    }


    val authState = authViewModel.authState.collectAsState()

    LaunchedEffect (authState.value){
        when(authState.value){
            is AuthState.Authenticated -> navController.navigate(Routes.Principal.route)
            is AuthState.Error -> {val messageResId = ToastMessage.ToastMessage.getStringResourceId((authState.value as AuthState.Error).errorType)
                Toast.makeText(context, getString(context,messageResId), Toast.LENGTH_SHORT).show()
                authViewModel.signout()}
            else -> Unit
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
        InsertTitle(context.getString(R.string.loginTitle))

        Spacer( modifier = Modifier.height(20.dp))

        OutlinedTextField(
            value = email,
            onValueChange = {
                email = it
            },
            label = {
                Text(text = context.getString(R.string.emailWord))
            }
        )

        Spacer( modifier = Modifier.height(12.dp))

        OutlinedTextField(
            value = password,
            onValueChange = {
                password = it
            },
            label = {
                Text(text = context.getString(R.string.passWord))
            }
        )

        Spacer( modifier = Modifier.height(50.dp))

        // Botón para inicar sesión
        Button(
            onClick = { authViewModel.login(email,password) },
            shape = misFormas.medium,
        ) {
            Text(
                text = context.getString(R.string.loginTitle),
                style = MaterialTheme.typography.headlineSmall,
            )
        }

        Spacer( modifier = Modifier.height(20.dp) )


    }

}