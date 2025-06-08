package com.example.talleresmileniumapp.Views

import android.net.Uri
import android.util.Log
import android.widget.Toast
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.Image
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.Button
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.RadioButton
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.unit.dp
import androidx.navigation.NavHostController
import coil.compose.rememberAsyncImagePainter
import com.example.talleresmileniumapp.Data.Routes
import com.example.talleresmileniumapp.Models.Product.NewProduct
import com.example.talleresmileniumapp.Models.Service.NewService
import com.example.talleresmileniumapp.R
import com.example.talleresmileniumapp.Themes.misFormas
import com.example.talleresmileniumapp.ViewModels.AuthState
import com.example.talleresmileniumapp.ViewModels.AuthViewModel
import com.example.talleresmileniumapp.ViewModels.ProductViewModel
import com.example.talleresmileniumapp.ViewModels.ServiceViewModel
import kotlinx.coroutines.launch

@Composable
fun AddService(navController: NavHostController, authViewModel: AuthViewModel, serviceViewModel: ServiceViewModel){

    val context = LocalContext.current
    val coroutineScope = rememberCoroutineScope()
    val authState = authViewModel.authState.collectAsState()

    var nombre by remember { mutableStateOf("") }
    var descripcion by remember { mutableStateOf("") }
    var selectedImageUri by remember { mutableStateOf<Uri?>(null) }

    // Para seleccionar imagen desde el sistema
    val imagePickerLauncher = rememberLauncherForActivityResult(
        contract = ActivityResultContracts.GetContent()
    ) { uri: Uri? ->
        selectedImageUri = uri
    }

    LaunchedEffect(authState.value) {
        when (authState.value) {
            is AuthState.Unauthenticated, is AuthState.Error -> navController.navigate(Routes.Login.route)
            else -> Unit
        }
    }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp)
            .verticalScroll(rememberScrollState()),
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Spacer(modifier = Modifier.height(90.dp))
        Text(context.getString(R.string.add_service_text), style = MaterialTheme.typography.headlineMedium)

        Spacer(modifier = Modifier.height(16.dp))

        OutlinedTextField(
            value = nombre,
            onValueChange = { nombre = it },
            label = { Text(context.getString(R.string.name_text)) },
            modifier = Modifier.fillMaxWidth()
        )

        Spacer(modifier = Modifier.height(8.dp))

        OutlinedTextField(
            value = descripcion,
            onValueChange = { descripcion = it },
            label = { Text(context.getString(R.string.description_text)) },
            modifier = Modifier.fillMaxWidth()
        )

        Spacer(modifier = Modifier.height(16.dp))

        Button(
            onClick = { imagePickerLauncher.launch("image/*") },
            modifier = Modifier.fillMaxWidth(),
            shape = misFormas.large
        ) {
            Text(context.getString(R.string.select_image_text))
        }

        selectedImageUri?.let { uri ->
            Spacer(modifier = Modifier.height(8.dp))
            Text(context.getString(R.string.selected_image_text)+": ${uri.lastPathSegment}")
            Spacer(modifier = Modifier.height(8.dp))
            Image(
                painter = rememberAsyncImagePainter(uri),
                contentDescription = context.getString(R.string.selected_image_text),
                modifier = Modifier
                    .size(150.dp)
                    .clip(misFormas.large)
            )
        }

        Spacer(modifier = Modifier.height(24.dp))

        Button(
            onClick = {
                when {
                    nombre.isBlank() || descripcion.isBlank() -> {
                        Toast.makeText(context, context.getString(R.string.complete_fields_text), Toast.LENGTH_SHORT).show()
                    }
                    selectedImageUri == null -> {
                        Toast.makeText(context, context.getString(R.string.please_select_image_text), Toast.LENGTH_SHORT).show()
                    }
                    else -> {
                        val file = uriToFile(context, selectedImageUri!!)
                        coroutineScope.launch {
                            serviceViewModel.addService(
                                NewService(
                                    nombre,
                                    descripcion
                                ),
                                file
                            )
                            navController.navigate(Routes.ProductosYServicios.route)
                        }
                    }
                }
            },
            modifier = Modifier.fillMaxWidth(),
            shape = misFormas.medium
        ) {
            Text(context.getString(R.string.add_service_text))
        }
    }
}