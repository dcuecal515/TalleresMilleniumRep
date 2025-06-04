package com.example.talleresmileniumapp.Data

sealed class Routes (val route : String) {
    data object Principal : Routes("Principal");
    data object Login : Routes("Login");
    data object Servicios : Routes("Servicios");
    data object Productos : Routes("Productos");
    data object Usuarios : Routes("Usuarios");
    data object Reservas : Routes("Reservas");
    data object TasksManager : Routes("TasksManager");

}