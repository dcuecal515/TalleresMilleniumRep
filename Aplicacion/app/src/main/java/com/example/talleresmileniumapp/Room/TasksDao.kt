package com.example.talleresmileniumapp.Room

import androidx.room.Dao
import androidx.room.Delete
import androidx.room.Insert
import androidx.room.Query
import androidx.room.Update
import kotlinx.coroutines.flow.Flow

@Dao
interface TasksDao {
    @Query("SELECT * FROM tasks")
    fun getAll(): Flow<List<Task>>

    @Query("SELECT COUNT(*) FROM tasks WHERE finished IS FALSE")
    fun getCount(): Flow<Int>

    @Update
    suspend fun updateTask(task: Task)

    @Insert
    suspend fun insertTask(task: Task)

    @Delete
    suspend fun deleteOneTask(task: Task)

    @Delete
    suspend fun deleteTask(allMyFriends: List<Task>)

}