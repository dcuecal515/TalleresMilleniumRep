package com.example.talleresmileniumapp.Data

sealed class Routes (val route : String) {
    data object Principal : Routes("Principal");
    data object Login : Routes("Login");
    data object ProductosYServicios : Routes("ProductosYServicios");
    data object Usuarios : Routes("Usuarios");
    data object Reservas : Routes("Reservas");
    data object TasksManager : Routes("TasksManager");

}