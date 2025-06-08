package com.example.talleresmileniumapp.Room

class TasksRepository(private val tasksDao: TasksDao) {

    fun getAll() = tasksDao.getAll()

    fun getCount() = tasksDao.getCount()

    suspend fun deleteOneTask(task: Task) = tasksDao.deleteOneTask(task)

    suspend fun updateTask(task: Task)
            = tasksDao.updateTask(task)

    suspend fun insertTask(task: Task)
            = tasksDao.insertTask(task)

    suspend fun deleteAllTasks(allTasks : List<Task>)
            = tasksDao.deleteTask(allTasks)
}