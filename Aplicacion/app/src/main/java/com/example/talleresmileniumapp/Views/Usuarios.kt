package com.example.talleresmileniumapp.Views

import android.annotation.SuppressLint
import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowDownward
import androidx.compose.material.icons.filled.Create
import androidx.compose.material.icons.filled.Delete
import androidx.compose.material.icons.filled.Upgrade
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.SnackbarDuration
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.navigation.NavHostController
import coil3.compose.AsyncImage
import com.example.talleresmileniumapp.Data.Routes
import com.example.talleresmileniumapp.Models.Product.ProductResponse
import com.example.talleresmileniumapp.Models.User.UserResponse
import com.example.talleresmileniumapp.ViewModels.AuthState
import com.example.talleresmileniumapp.ViewModels.AuthViewModel
import com.example.talleresmileniumapp.ViewModels.UserViewModel
import kotlinx.coroutines.coroutineScope
import kotlinx.coroutines.launch

@SuppressLint("StateFlowValueCalledInComposition")
@Composable
fun Usuarios(navController: NavHostController, authViewModel: AuthViewModel,userViewModel: UserViewModel){
    val context = LocalContext.current

    val authState = authViewModel.authState.collectAsState()
    val accessToken by userViewModel.accessToken.collectAsState()
    val coroutineScope = rememberCoroutineScope()
    val users by userViewModel.users.collectAsState()


    LaunchedEffect (authState.value){
        when(authState.value){
            is AuthState.Unauthenticated -> navController.navigate(Routes.Login.route)
            else -> Unit
        }
    }
    LaunchedEffect(accessToken) {
        accessToken?.let {
            userViewModel.getalluser()
        }
    }
    Column(
        modifier = Modifier
            .fillMaxSize()
            .background(MaterialTheme.colorScheme.background),
        verticalArrangement = Arrangement.spacedBy(50.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        )
    {
        Spacer(modifier = Modifier.height(50.dp))

        Text(text = "Usuarios", fontSize = 35.sp)
        if (users == null) {
            CircularProgressIndicator()
        } else if (users!!.isEmpty()) {
            Text("No hay usuarios todavia")
        } else {
            //Muestra todas las actividades
            LazyColumn {
                items(users!!) { user ->
                    ShowUser(
                        user,
                        Icons.Default.Upgrade,
                        "Hacer Admin",
                        Icons.Default.Delete,
                        "Eliminar",
                        Icons.Default.ArrowDownward,
                        "Hacer Usuario",
                        onClickAction1 = {
                            coroutineScope.launch {
                                userViewModel.putadmin(user.id,user.rol)
                            }
                        },
                        onClickAction2 = {
                            coroutineScope.launch {
                                userViewModel.deleteuser(user.id)
                            }
                        },
                        onClickAction3 = {
                            coroutineScope.launch {
                                userViewModel.putadmin(user.id,user.rol)
                            }
                        }
                    )
                }
            }
        }
    }
}
@Composable
fun ShowUser(
    user: UserResponse,
    buttonIcon1: ImageVector,
    textButton1: String,
    buttonIcon2: ImageVector,
    textButton2: String,
    buttonIcon3: ImageVector,
    textButton3: String,
    onClickAction1: () -> Unit,
    onClickAction2: () -> Unit,
    onClickAction3: () -> Unit
){
    Card(
        elevation = CardDefaults.cardElevation(defaultElevation = 8.dp),
        border = BorderStroke(1.dp, MaterialTheme.colorScheme.outline),
        colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.surface),
        shape = RoundedCornerShape(16.dp),
        modifier = Modifier
            .fillMaxWidth()
            .padding(horizontal = 16.dp, vertical = 8.dp)
    ) {
        Column(
            modifier = Modifier
                .fillMaxWidth()
                .padding(16.dp),
            verticalArrangement = Arrangement.spacedBy(16.dp)
        ) {
            Row(
                modifier = Modifier.fillMaxWidth(),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(16.dp)
            ) {
                AsyncImage(
                    model = "http://10.0.2.2:5151${user.imagen}",
                    contentDescription = "Imagen del usuario",
                    modifier = Modifier
                        .size(100.dp)
                        .clip(RoundedCornerShape(12.dp))
                        .border(1.dp, MaterialTheme.colorScheme.outline, RoundedCornerShape(12.dp)),
                    contentScale = ContentScale.Crop
                )

                Column(
                    modifier = Modifier.weight(1f),
                    verticalArrangement = Arrangement.spacedBy(4.dp)
                ) {
                    Text(
                        text = "Email: ${user.email}",
                        style = MaterialTheme.typography.titleMedium,
                        color = MaterialTheme.colorScheme.onSurface,
                        maxLines = 1
                    )
                    Text(
                        text = "Nombre: ${user.name}",
                        style = MaterialTheme.typography.titleMedium,
                        color = MaterialTheme.colorScheme.onSurfaceVariant,
                        maxLines = 3
                    )
                }
            }
            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.End,
                verticalAlignment = Alignment.CenterVertically
            ) {
                if(user.rol=="User"){
                    Button(
                        onClick = onClickAction1,
                        shape = RoundedCornerShape(8.dp),
                        colors = ButtonDefaults.buttonColors(containerColor = MaterialTheme.colorScheme.primary)
                    ) {
                        Icon(
                            imageVector = buttonIcon1,
                            contentDescription = "Icono de acción",
                            tint = MaterialTheme.colorScheme.onPrimary
                        )
                        Spacer(modifier = Modifier.width(8.dp))
                        Text(text = textButton1, color = MaterialTheme.colorScheme.onPrimary)
                    }
                }else if(user.rol=="Admin" && user.email!="example@gmail.com"){
                    Button(
                        onClick = onClickAction3,
                        shape = RoundedCornerShape(8.dp),
                        colors = ButtonDefaults.buttonColors(containerColor = MaterialTheme.colorScheme.primary)
                    ) {
                        Icon(
                            imageVector = buttonIcon3,
                            contentDescription = "Icono de acción",
                            tint = MaterialTheme.colorScheme.onPrimary
                        )
                        Spacer(modifier = Modifier.width(8.dp))
                        Text(text = textButton3, color = MaterialTheme.colorScheme.onPrimary)
                    }
                }
                Spacer(modifier = Modifier.width(12.dp))
                if(user.email!="example@gmail.com" ){
                    Button(
                        onClick = onClickAction2,
                        shape = RoundedCornerShape(8.dp),
                        colors = ButtonDefaults.buttonColors(containerColor = MaterialTheme.colorScheme.secondary)
                    ) {
                        Icon(
                            imageVector = buttonIcon2,
                            contentDescription = "Icono secundario",
                            tint = MaterialTheme.colorScheme.onSecondary
                        )
                        Spacer(modifier = Modifier.width(8.dp))
                        Text(text = textButton2, color = MaterialTheme.colorScheme.onSecondary)
                    }
                }
            }
        }
    }
}