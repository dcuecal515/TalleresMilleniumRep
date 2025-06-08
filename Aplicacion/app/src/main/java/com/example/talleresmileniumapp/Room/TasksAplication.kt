package com.example.talleresmileniumapp.Room

import android.app.Application

class TasksAplication : Application() {

    lateinit var container: TasksContainer

    override fun onCreate() {
        super.onCreate()
        container = TasksContainer(this)
    }
}