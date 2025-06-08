package com.example.talleresmileniumapp

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.activity.viewModels
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.tooling.preview.Preview
import androidx.room.Room
import com.example.talleresmileniumapp.Navegationdrawer.NavigationDrawer
import com.example.talleresmileniumapp.Room.TasksDatabase
import com.example.talleresmileniumapp.Room.TasksRepository
import com.example.talleresmileniumapp.Room.TasksViewModel
import com.example.talleresmileniumapp.ViewModels.AuthViewModel
import com.example.talleresmileniumapp.ViewModels.ServiceViewModel
import com.example.talleresmileniumapp.ViewModels.ProductViewModel
import com.example.talleresmileniumapp.ViewModels.UserViewModel
import com.example.talleresmileniumapp.ui.theme.TalleresMileniumAppTheme

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            TalleresMileniumAppTheme {
                val authViewModel: AuthViewModel by viewModels()
                val serviceViewModel:ServiceViewModel by viewModels()
                val productViewModel : ProductViewModel by viewModels()
                val userViewModel:UserViewModel by viewModels()
                //Variables necesarias para task view model
                val context = LocalContext.current
                val db = Room.databaseBuilder(context, TasksDatabase::class.java, "tasks2").build()
                val tasksDao = db.TasksDao()
                val tasksRepository = TasksRepository(tasksDao)
                val tasksViewModel = TasksViewModel(tasksRepository)
                NavigationDrawer(authViewModel,productViewModel,serviceViewModel,userViewModel,tasksViewModel)
            }
        }
    }
}

