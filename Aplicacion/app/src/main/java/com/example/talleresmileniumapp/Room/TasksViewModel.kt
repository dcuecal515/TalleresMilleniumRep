package com.example.talleresmileniumapp.Room

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.ViewModelProvider.AndroidViewModelFactory.Companion.APPLICATION_KEY
import androidx.lifecycle.viewModelScope
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.launch

class TasksViewModel(private val tasksRepository: TasksRepository) : ViewModel() {

    fun getAll(): Flow<List<Task>> = tasksRepository.getAll()

    fun getCount(): Flow<Int> = tasksRepository.getCount()

    fun updateTask(id : Int,description: String,priority: Int, finished: Boolean) = viewModelScope.launch {
        tasksRepository.updateTask(Task(id,description,priority,finished))
    }

    fun insertTask(description: String, priority: Int) = viewModelScope.launch {
        tasksRepository.insertTask(Task(description = description, priority = priority, finished = false))
    }

    fun deleteAllTasks(allTasks: List<Task>) = viewModelScope.launch {
        tasksRepository.deleteAllTasks(allTasks)
    }

    fun deleteOneTask(task: Task) = viewModelScope.launch {
        tasksRepository.deleteOneTask(task)
    }

    companion object {
        val Factory: ViewModelProvider.Factory = viewModelFactory {
            initializer {
                val application = (this[APPLICATION_KEY] as TasksAplication)
                TasksViewModel(application.container.tasksRepository)
            }
        }
    }
}