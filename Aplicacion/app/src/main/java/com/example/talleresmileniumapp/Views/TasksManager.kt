package com.example.talleresmileniumapp.Views

import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.scaleOut
import androidx.compose.animation.shrinkVertically
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import com.example.talleresmileniumapp.R
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.outlined.Delete
import androidx.compose.material3.Button
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.Checkbox
import androidx.compose.material3.DropdownMenu
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.text.style.TextDecoration
import androidx.compose.ui.unit.dp
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavHostController
import com.example.talleresmileniumapp.Room.TasksViewModel
import kotlinx.coroutines.delay
import androidx.compose.foundation.lazy.items
import com.example.talleresmileniumapp.Themes.misFormas

@Composable
fun TasksManager(navController: NavHostController,
                 viewModel: TasksViewModel = viewModel(factory = TasksViewModel.Factory)
) {

    val context = LocalContext.current

    val taskList by viewModel.getAll().collectAsState(initial = emptyList())
    var taskDescription by remember { mutableStateOf("") }
    var taskPriority by remember { mutableIntStateOf(0) }

    val cardColor = Color(0xFF919198)
    val lowPriorityColor = Color(0xFF116913)
    val mediumPriorityColor = Color(0xFF0E2059)
    val highPriorityColor = Color(0xFFE30B1F)


    Column (
        modifier = Modifier
            .fillMaxSize()
            .background(MaterialTheme.colorScheme.background)
            .padding(top = 30.dp) ,// Márgenes alrededor de la tarjeta
        verticalArrangement = Arrangement.spacedBy(18.dp, Alignment.CenterVertically),
        horizontalAlignment = Alignment.CenterHorizontally
    )  {

        Spacer(modifier = Modifier.height(90.dp))
        Text(context.getString(R.string.task_list_text), style = MaterialTheme.typography.headlineMedium)

        LazyColumn(
            modifier = Modifier.height(500.dp),
            verticalArrangement = Arrangement.Top,
        ) {
            //Recorre todas las tareas y las muestra por pantalla
            items(taskList) { task ->
                var isVisible by remember { mutableStateOf(true) }
                if(!isVisible){
                    LaunchedEffect(task.id) {
                        delay(100)
                        viewModel.deleteOneTask(task)
                        isVisible = true
                    }
                }
                AnimatedVisibility(
                    visible = isVisible,
                    exit = scaleOut() + shrinkVertically(shrinkTowards = Alignment.CenterVertically)
                ) {
                    Card(
                        elevation = CardDefaults.cardElevation(
                            defaultElevation = 12.dp
                        ),
                        colors = CardDefaults.cardColors(
                            containerColor = cardColor
                        ),
                        shape = misFormas.medium,
                        modifier = Modifier
                            .fillMaxWidth()
                            .padding(start = 16.dp, end = 16.dp, top = 8.dp, bottom = 8.dp) // Márgenes alrededor de la tarjeta
                            .clickable {
                                viewModel.updateTask(
                                    task.id,
                                    task.description,
                                    task.priority,
                                    !task.finished
                                )
                            }

                    ) {
                        Row(verticalAlignment = Alignment.CenterVertically
                        ){
                            Checkbox(
                                checked = task.finished,
                                onCheckedChange = {
                                    viewModel.updateTask(
                                        task.id,
                                        task.description,
                                        task.priority,
                                        !task.finished
                                    )
                                }
                            )
                            Column (
                                modifier = Modifier.padding(16.dp)

                            ) {
                                when (task.priority) {
                                    1 -> Text(text = context.getString(R.string.low_priority_text),
                                        style = MaterialTheme.typography.bodyMedium,
                                        color = lowPriorityColor,
                                        fontWeight = FontWeight.Bold
                                    )
                                    2 -> Text(text = context.getString(R.string.medium_priority_text),
                                        style = MaterialTheme.typography.bodyMedium,
                                        color = mediumPriorityColor,
                                        fontWeight = FontWeight.Bold
                                    )
                                    3 -> Text(text = context.getString(R.string.high_priority_text),
                                        style = MaterialTheme.typography.bodyMedium,
                                        color = highPriorityColor,
                                        fontWeight = FontWeight.Bold
                                    )
                                }

                                Spacer(modifier = Modifier.height(10.dp))

                                if(task.finished){
                                    Text(text = task.description,
                                        modifier = Modifier.width(250.dp),
                                        style = MaterialTheme.typography.bodyMedium,
                                        color = Color.Black,
                                        textAlign = TextAlign.Justify,
                                        textDecoration = TextDecoration.LineThrough
                                    )
                                }else{
                                    Text(text = task.description,
                                        modifier = Modifier.width(250.dp),
                                        style = MaterialTheme.typography.bodyMedium ,
                                        color = Color.Black,
                                        textAlign = TextAlign.Justify
                                    )
                                }
                            }
                            Icon(
                                modifier = Modifier.clickable {
                                    isVisible = false
                                },
                                imageVector = Icons.Outlined.Delete,
                                contentDescription = "Delete",
                                tint = MaterialTheme.colorScheme.onError
                            )
                        }

                    }
                }

            }
        }


        val scrollState = rememberScrollState()

        Column(
            modifier = Modifier
                .fillMaxWidth()
                .background(MaterialTheme.colorScheme.background)
                .verticalScroll(scrollState),
            verticalArrangement = Arrangement.spacedBy(18.dp, Alignment.CenterVertically),
            horizontalAlignment = Alignment.CenterHorizontally
        ){
            // Campo de texto para la descripción
            OutlinedTextField(
                value = taskDescription,
                shape = misFormas.small,
                onValueChange = { taskDescription = it },
                label = { Text(context.getString(R.string.task_description_text)) }
            )

            // Dropdown para la prioridad
            GenerateDropDown(taskPriority) { priority ->
                taskPriority = priority // Actualiza el entero seleccionado
            }

            //Botones para guardar una tarea o para eliminarlas todas
            Row{
                Button(
                    shape = misFormas.small,
                    modifier = Modifier.width(150.dp)
                        .padding(end =  10.dp),
                    onClick = {
                        viewModel.insertTask(taskDescription, taskPriority)
                        taskDescription = ""
                    })
                {
                    Text(text = context.getString(R.string.save_text))
                }

                Button(shape = misFormas.small,
                    modifier = Modifier.width(160.dp),
                    onClick = {
                        viewModel.deleteAllTasks(taskList)
                        taskDescription = ""
                    },
                ) {
                    Text(text = context.getString(R.string.deleteall_text))
                }
            }
        }
    }

}


//Funcion que genera el dropdown
@Composable
fun GenerateDropDown(selectedPriority: Int, onPrioritySelected: (Int) -> Unit) {

    val context = LocalContext.current


    val opciones = listOf(
        context.getString(R.string.low_priority_text) to 1,
        context.getString(R.string.medium_priority_text) to 2,
        context.getString(R.string.high_priority_text) to 3)

    var expanded by remember { mutableStateOf(false) }

    Column {
        OutlinedButton(onClick = { expanded = true },
            shape = misFormas.small,
        ) {
            Text(text = opciones.firstOrNull { it.second == selectedPriority }?.first ?: context.getString(R.string.select_priority_text))
        }

        DropdownMenu(
            expanded = expanded,
            onDismissRequest = { expanded = false }
        ) {
            //Recorre la lista creando items con sus valores
            opciones.forEach() {(text, value) ->
                DropdownMenuItem(
                    text = { Text(text) },
                    onClick = {
                        onPrioritySelected(value) // Pasa el valor entero seleccionado
                        expanded = false
                    }
                )
            }
        }
    }
}