package com.example.talleresmileniumapp.Data

sealed class Routes (val route : String) {
    data object Principal : Routes("Principal");
    data object Login : Routes("Login");
    data object TasksManager : Routes("TasksManager");

}