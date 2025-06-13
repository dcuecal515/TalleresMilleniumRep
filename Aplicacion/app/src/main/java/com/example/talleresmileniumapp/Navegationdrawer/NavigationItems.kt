package com.example.talleresmileniumapp.Navegationdrawer

import androidx.compose.ui.graphics.vector.ImageVector

data class NavigationItems(
    val title: String,
    val selectedIcon: ImageVector,
    val unselectedIcon: ImageVector,
    val route: String? = null
)
