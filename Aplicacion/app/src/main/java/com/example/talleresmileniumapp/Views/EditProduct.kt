package com.example.talleresmileniumapp.Views

import android.content.Context
import android.net.Uri
import android.util.Log
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
import com.example.talleresmileniumapp.Models.Product.UpdateProduct
import com.example.talleresmileniumapp.R
import com.example.talleresmileniumapp.Themes.misFormas
import com.example.talleresmileniumapp.ViewModels.AuthState
import com.example.talleresmileniumapp.ViewModels.AuthViewModel
import com.example.talleresmileniumapp.ViewModels.ProductViewModel
import kotlinx.coroutines.launch
import java.io.File
import java.io.FileOutputStream
import java.io.InputStream

@Composable
fun EditProduct(navController: NavHostController, authViewModel: AuthViewModel, productViewModel: ProductViewModel)
{
    val context = LocalContext.current

    val authState = authViewModel.authState.collectAsState()
    val coroutineScope = rememberCoroutineScope()
    val producto by productViewModel.producto.collectAsState()

    var nombre by remember { mutableStateOf(producto?.nombre ?: "") }
    var descripcion by remember { mutableStateOf(producto?.descripcion ?: "") }
    var disponible by remember { mutableStateOf(producto?.disponible ?: "") }

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
        Text(context.getString(R.string.edit_product_text), style = MaterialTheme.typography.headlineMedium)

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

        Spacer(modifier = Modifier.height(8.dp))

        Text(context.getString(R.string.availability_text), style = MaterialTheme.typography.titleMedium)

        Row(
            verticalAlignment = Alignment.CenterVertically,
            modifier = Modifier
                .fillMaxWidth()
                .padding(top = 8.dp, bottom = 8.dp)
        ) {
            RadioButton(
                selected = disponible == "Disponible",
                onClick = { disponible = "Disponible" }
            )
            Text(context.getString(R.string.available_text), modifier = Modifier.clickable { disponible = "Disponible" })

            Spacer(modifier = Modifier.width(16.dp))

            RadioButton(
                selected = disponible == "No disponible",
                onClick = { disponible = "No disponible"
                            Log.i("TAG","Si funciona")
                          }
            )
            Text(context.getString(R.string.not_available_text), modifier = Modifier.clickable { disponible = "No disponible" })
        }

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
                if(selectedImageUri != null){
                    val file = uriToFile(context, selectedImageUri!!)
                    coroutineScope.launch {
                        productViewModel.updateProduct(
                            UpdateProduct(
                                producto!!.id.toString(),
                                nombre,
                                descripcion,
                                disponible
                            ),
                            file
                        )
                    }
                }else{
                    val file = null
                    coroutineScope.launch {
                        productViewModel.updateProduct(
                            UpdateProduct(
                                producto!!.id.toString(),
                                nombre,
                                descripcion,
                                disponible
                            ),
                            file
                        )
                    }
                }
                navController.navigate(Routes.ProductosYServicios.route)

            },
            modifier = Modifier.fillMaxWidth(),
            shape = misFormas.medium
        ) {
            Text(context.getString(R.string.save_text))
        }
    }
}

fun uriToFile(context: Context, uri: Uri): File {
    val inputStream: InputStream? = context.contentResolver.openInputStream(uri)
    val file = File.createTempFile("temp_image", ".jpg", context.cacheDir)
    val outputStream = FileOutputStream(file)

    inputStream?.use { input ->
        outputStream.use { output ->
            input.copyTo(output)
        }
    }

    return file
}