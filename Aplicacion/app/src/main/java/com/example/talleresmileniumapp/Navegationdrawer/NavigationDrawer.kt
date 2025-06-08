package com.example.talleresmileniumapp.Navegationdrawer

import android.annotation.SuppressLint
import android.app.Activity
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Build
import androidx.compose.material.icons.filled.CheckCircle
import androidx.compose.material.icons.filled.DateRange
import androidx.compose.material.icons.filled.ExitToApp
import androidx.compose.material.icons.filled.Face
import androidx.compose.material.icons.filled.Home
import androidx.compose.material.icons.filled.Info
import androidx.compose.material.icons.filled.Menu
import androidx.compose.material.icons.filled.Person
import androidx.compose.material.icons.filled.ShoppingCart
import androidx.compose.material.icons.filled.Star
import androidx.compose.material.icons.outlined.Build
import androidx.compose.material.icons.outlined.CheckCircle
import androidx.compose.material.icons.outlined.DateRange
import androidx.compose.material.icons.outlined.ExitToApp
import androidx.compose.material.icons.outlined.Face
import androidx.compose.material.icons.outlined.Home
import androidx.compose.material.icons.outlined.Info
import androidx.compose.material.icons.outlined.Person
import androidx.compose.material.icons.outlined.ShoppingCart
import androidx.compose.material.icons.outlined.Star
import androidx.compose.material3.Badge
import androidx.compose.material3.DrawerValue
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.ModalDrawerSheet
import androidx.compose.material3.ModalNavigationDrawer
import androidx.compose.material3.NavigationDrawerItem
import androidx.compose.material3.NavigationDrawerItemDefaults
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBar
import androidx.compose.material3.rememberDrawerState
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.ImageBitmap
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.imageResource
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.example.talleresmileniumapp.ViewModels.AuthViewModel
import kotlinx.coroutines.launch
import com.example.talleresmileniumapp.Data.Routes
import com.example.talleresmileniumapp.Dialog.AlertDialog
import com.example.talleresmileniumapp.Principal
import com.example.talleresmileniumapp.R
import com.example.talleresmileniumapp.Room.TasksViewModel
import com.example.talleresmileniumapp.ViewModels.ServiceViewModel
import com.example.talleresmileniumapp.ViewModels.ProductViewModel
import com.example.talleresmileniumapp.ViewModels.ReservaViewModel
import com.example.talleresmileniumapp.ViewModels.UserViewModel
import com.example.talleresmileniumapp.Views.AddProduct
import com.example.talleresmileniumapp.Views.AddService
import com.example.talleresmileniumapp.Views.EditProduct
import com.example.talleresmileniumapp.Views.EditService
import com.example.talleresmileniumapp.Views.Login
import com.example.talleresmileniumapp.Views.ProductosYServicios
import com.example.talleresmileniumapp.Views.Reservas
import com.example.talleresmileniumapp.Views.TasksManager
import com.example.talleresmileniumapp.Views.Usuarios

@OptIn(ExperimentalMaterial3Api::class)
@SuppressLint("UnusedMaterial3ScaffoldPaddingParameter")
@Composable
fun NavigationDrawer(
    authViewModel: AuthViewModel,
    productViewModel: ProductViewModel,
    serviceViewModel: ServiceViewModel,
    userViewModel: UserViewModel,
    reservaViewModel: ReservaViewModel,
    tasksViewModel: TasksViewModel
){
    val navController = rememberNavController()
    val context = LocalContext.current
    val showDialog = remember { mutableStateOf(false) }
    val tasknotfinished by tasksViewModel.getCount().collectAsState(0)

    ///List of Navigation Items that will be clicked
    val items = listOf(
        NavigationItems(
            title = "Inicio"/*context.getString(R.string.home_text)*/,
            selectedIcon = Icons.Filled.Home,
            unselectedIcon = Icons.Outlined.Home,
            route = Routes.Principal.route
        ),
        NavigationItems(
            title = "Cuenta"/*context.getString(R.string.account_text)*/,
            selectedIcon = Icons.Filled.Person,
            unselectedIcon = Icons.Outlined.Person,
            route = Routes.Login.route
        ),
        NavigationItems(
            title = "Productos y Servicios"/*context.getString(R.string.account_text)*/,
            selectedIcon = Icons.Filled.ShoppingCart,
            unselectedIcon = Icons.Outlined.ShoppingCart,
            route = Routes.ProductosYServicios.route
        ),
        NavigationItems(
            title = "Usuarios"/*context.getString(R.string.account_text)*/,
            selectedIcon = Icons.Filled.Face,
            unselectedIcon = Icons.Outlined.Face,
            route = Routes.Usuarios.route
        ),
        NavigationItems(
            title = "Reservas"/*context.getString(R.string.account_text)*/,
            selectedIcon = Icons.Filled.DateRange,
            unselectedIcon = Icons.Outlined.DateRange,
            route = Routes.Reservas.route
        ),
        NavigationItems(
            title = "Tareas"/*context.getString(R.string.tasks_name)*/,
            selectedIcon = Icons.Filled.CheckCircle,
            unselectedIcon = Icons.Outlined.CheckCircle,
            route = Routes.TasksManager.route
        ),
        NavigationItems(
            title = "Salir"/*context.getString(R.string.exit_button_title)*/,
            selectedIcon = Icons.Filled.ExitToApp,
            unselectedIcon = Icons.Outlined.ExitToApp,
            route = null
        )
    )

    //Remember Clicked item state
    var selectedItemIndex by rememberSaveable {
        mutableStateOf(0)
    }

    //Remember the State of the drawer. Closed/ Opened
    val drawerState = rememberDrawerState(initialValue = DrawerValue.Closed)

    val scope = rememberCoroutineScope()

    ModalNavigationDrawer(
        drawerState = drawerState,
        drawerContent = {
            ModalDrawerSheet {
                Spacer(modifier = Modifier.height(16.dp)) //space (margin) from top
                items.forEachIndexed { index, item ->
                    NavigationDrawerItem(
                        label = { Text(text = item.title) },
                        selected = index == selectedItemIndex,
                        onClick = {
                            if(item.route != null) {
                                navController.navigate(item.route)
                            }
                            else{
                                showDialog.value = true
                            }

                            selectedItemIndex = index
                            scope.launch {
                                drawerState.close()
                            }
                        },
                        icon = {
                            Icon(
                                imageVector = if (index == selectedItemIndex) {
                                    item.selectedIcon
                                } else item.unselectedIcon,
                                contentDescription = item.title
                            )
                        },
                        badge = {
                            if(index == 5){
                                if(tasknotfinished>0){
                                    Badge(
                                        modifier = Modifier.padding(5.dp),
                                        containerColor = Color.Red,
                                        contentColor = Color.White,
                                        content = {Text(tasknotfinished.toString())}
                                    )
                                }

                            }
                        },
                        modifier = Modifier
                            .padding(NavigationDrawerItemDefaults.ItemPadding) //padding between items
                    )
                }

            }
        },
        gesturesEnabled = true
    ) {
        Scaffold(
            topBar = { //TopBar to show title
                TopAppBar(
                    title = {
                    },
                    navigationIcon = {
                        IconButton(onClick = {
                            scope.launch {
                                drawerState.apply {
                                    if (isClosed) open() else close()
                                }
                            }
                        }) {
                            Icon(  //Show Menu Icon on TopBar
                                imageVector = Icons.Default.Menu,
                                contentDescription = "Menu"
                            )
                        }
                    }
                )
            }
        ) {
            NavHost(navController = navController, startDestination = Routes.Login.route) {
                composable(Routes.Principal.route) { selectedItemIndex = 0
                    Principal(navController, authViewModel) }
                composable(Routes.Login.route) { selectedItemIndex = 1
                    Login(navController, authViewModel) }
                composable(Routes.ProductosYServicios.route) { selectedItemIndex = 2
                    ProductosYServicios(navController,authViewModel,productViewModel,serviceViewModel)
                }
                composable(Routes.Usuarios.route) { selectedItemIndex = 3
                    Usuarios(navController,authViewModel,userViewModel)
                }
                composable(Routes.Reservas.route) { selectedItemIndex = 4
                    Reservas(navController,authViewModel, reservaViewModel)
                }
                composable(Routes.EditProduct.route) { selectedItemIndex = -1
                    EditProduct(navController,authViewModel, productViewModel)
                }
                composable(Routes.AddProduct.route) { selectedItemIndex = -2
                    AddProduct(navController,authViewModel, productViewModel)
                }
                composable(Routes.EditService.route) { selectedItemIndex = -3
                    EditService(navController,authViewModel, serviceViewModel)
                }
                composable(Routes.AddService.route) { selectedItemIndex = -4
                    AddService(navController,authViewModel, serviceViewModel)
                }
                composable(Routes.TasksManager.route) { selectedItemIndex = 5
                    TasksManager(navController,tasksViewModel)
                }
            }
        }
        val activity = (LocalContext.current as? Activity)
        val painter = painterResource(id = R.drawable.ic_launcher_foreground)
        // Llama al AlertDialog genérico y lo muestra
        if (showDialog.value) {
            AlertDialog(
                title =  context.getString(R.string.exit_title),
                description =  context.getString(R.string.exit_description),
                icon = painter,
                confirmText =  context.getString(R.string.exit_confirm),
                dismissText = context.getString(R.string.exit_cancel),
                confirm = {
                    activity?.finish()
                    showDialog.value = false
                },
                dismiss = {
                    showDialog.value = false
                }
            )
        }
    }


}