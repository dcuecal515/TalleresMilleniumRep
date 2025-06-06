package com.example.talleresmileniumapp.Views

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.pager.HorizontalPager
import androidx.compose.foundation.pager.rememberPagerState
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.List
import androidx.compose.material.icons.filled.Home
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.SnackbarHost
import androidx.compose.material3.SnackbarHostState
import androidx.compose.material3.Tab
import androidx.compose.material3.TabRow
import androidx.compose.material3.TabRowDefaults
import androidx.compose.material3.TabRowDefaults.tabIndicatorOffset
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.unit.dp
import androidx.navigation.NavHostController
import com.example.talleresmileniumapp.Data.Routes
import com.example.talleresmileniumapp.InsertTitle
import com.example.talleresmileniumapp.ViewModels.AuthState
import com.example.talleresmileniumapp.ViewModels.AuthViewModel
import com.example.talleresmileniumapp.ViewModels.ProductViewModel
import kotlinx.coroutines.launch

@Composable
fun Productos(navController: NavHostController, authViewModel: AuthViewModel, productViewModel: ProductViewModel){

    val pagerState = rememberPagerState(pageCount = { 2 })
    val coroutineScope = rememberCoroutineScope()
    val authState = authViewModel.authState.collectAsState()
    val productos by productViewModel.productos.collectAsState()
    val accessToken by productViewModel.accessToken.collectAsState()

    // Titulos de las paginas
    val tabs = listOf("Servicios", "Productos")

    LaunchedEffect (authState.value){
        when(authState.value){
            is AuthState.Unauthenticated -> navController.navigate(Routes.Login.route)
            is AuthState.Error -> navController.navigate(Routes.Login.route)
            else -> Unit
        }
    }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .background(MaterialTheme.colorScheme.background),
        verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally,

        )
    {
        InsertTitle("Productos")

        LaunchedEffect(accessToken) {
            accessToken?.let {
                productViewModel.getProducts()
            }
        }

        /*val snackbarHostState = remember { SnackbarHostState() }


        Scaffold(
            snackbarHost = {
                SnackbarHost(hostState = snackbarHostState)
            },
            modifier = Modifier.fillMaxSize(),
            topBar = {
                Column {
                    Spacer(modifier = Modifier.height(90.dp))
                    TabRow(
                        selectedTabIndex = pagerState.currentPage,
                        modifier = Modifier.fillMaxWidth(),
                        containerColor = MaterialTheme.colorScheme.background,
                        contentColor = MaterialTheme.colorScheme.onBackground,
                        indicator = { tabPositions ->
                            TabRowDefaults.apply {
                                HorizontalDivider(
                                    Modifier
                                        .height(2.dp)
                                        .padding(horizontal = 16.dp)
                                        .tabIndicatorOffset(tabPositions[pagerState.currentPage]),
                                    color = MaterialTheme.colorScheme.primary
                                )
                            }
                        },
                        divider = {}
                    ) {
                        tabs.forEachIndexed { index, title ->
                            Tab(
                                text = { Text(title) },
                                selected = pagerState.currentPage == index,
                                onClick = {
                                    coroutineScope.launch {
                                        pagerState.animateScrollToPage(index)
                                    }
                                },
                                icon = {
                                    when (index) {
                                        0 -> Icon(imageVector = Icons.Default.Home, contentDescription = null)
                                        1 -> Icon(imageVector = Icons.AutoMirrored.Filled.List, contentDescription = null)
                                    }
                                }
                            )
                        }
                    }
                }
            }
        ) { paddingValues ->
            HorizontalPager(
                state = pagerState,
                modifier = Modifier
                    .fillMaxSize()
                    .padding(paddingValues)
            ) { page ->
                when (page) {
                    0 -> AllProductsScreen(snackbarHostState, productos, productViewModel)
                    1 -> AllUserActivitiesScreen(snackbarHostState, userActivities!!, activitiesViewModel)
                }
            }
        }*/

    }
}