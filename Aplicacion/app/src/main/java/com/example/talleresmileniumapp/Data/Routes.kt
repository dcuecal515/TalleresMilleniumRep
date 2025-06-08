package com.example.talleresmileniumapp.Data

sealed class Routes (val route : String) {
    data object Principal : Routes("Principal");
    data object Login : Routes("Login");
    data object ProductosYServicios : Routes("ProductosYServicios");
    data object Usuarios : Routes("Usuarios");
    data object Reservas : Routes("Reservas");
    data object EditProduct : Routes("EditProduct");
    data object AddProduct : Routes("AddProduct");
    data object EditService : Routes("EditService");
    data object AddService : Routes("AddService");
    data object TasksManager : Routes("TasksManager");
}