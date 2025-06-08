package com.example.talleresmileniumapp.Views

import android.annotation.SuppressLint
import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.pager.HorizontalPager
import androidx.compose.foundation.pager.rememberPagerState
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.AllInbox
import androidx.compose.material.icons.filled.Build
import androidx.compose.material.icons.filled.Create
import androidx.compose.material.icons.filled.Delete
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.SnackbarDuration
import androidx.compose.material3.SnackbarHost
import androidx.compose.material3.SnackbarHostState
import androidx.compose.material3.Tab
import androidx.compose.material3.TabRow
import androidx.compose.material3.TabRowDefaults
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.ImageBitmap
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.imageResource
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.navigation.NavHostController
import coil3.compose.AsyncImage
import com.example.talleresmileniumapp.Data.Routes
import com.example.talleresmileniumapp.Dialog.AlertDialog
import com.example.talleresmileniumapp.Models.Product.ProductResponse
import com.example.talleresmileniumapp.Models.Service.ServiceResponse
import com.example.talleresmileniumapp.R
import com.example.talleresmileniumapp.ViewModels.AuthState
import com.example.talleresmileniumapp.ViewModels.AuthViewModel
import com.example.talleresmileniumapp.ViewModels.ProductViewModel
import com.example.talleresmileniumapp.ViewModels.ServiceViewModel
import kotlinx.coroutines.launch

@Composable
fun ProductosYServicios(navController: NavHostController, authViewModel: AuthViewModel, productViewModel: ProductViewModel, serviceViewModel: ServiceViewModel){

    val context = LocalContext.current
    val pagerState = rememberPagerState(pageCount = { 2 })
    val coroutineScope = rememberCoroutineScope()
    val authState = authViewModel.authState.collectAsState()
    val productos by productViewModel.productos.collectAsState()
    val services by serviceViewModel.services.collectAsState()
    val accessToken by productViewModel.accessToken.collectAsState()

    // Titulos de las paginas
    val tabs = listOf(context.getString(R.string.products_text),context.getString(R.string.services_text))

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
        Spacer(modifier = Modifier.height(90.dp))

        Text(text = context.getString(R.string.ps_title), fontSize = 35.sp)

        LaunchedEffect(accessToken) {
            accessToken?.let {
                productViewModel.getProducts()
                serviceViewModel.getallservice()
            }
        }

        val snackbarHostState = remember { SnackbarHostState() }


        Scaffold(
            snackbarHost = {
                SnackbarHost(hostState = snackbarHostState)
            },
            modifier = Modifier.fillMaxSize(),
            topBar = {
                Column {
                    Spacer(modifier = Modifier.height(40.dp))
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
                                        0 -> Icon(imageVector = Icons.Default.AllInbox, contentDescription = null)
                                        1 -> Icon(imageVector = Icons.Default.Build, contentDescription = null)
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
                    0 -> AllProductsScreen(navController,snackbarHostState, productos, productViewModel)
                    1 -> AllServicesScreen(navController, snackbarHostState, services, serviceViewModel)
                }
            }
        }

    }
}

@SuppressLint("StateFlowValueCalledInComposition", "CoroutineCreationDuringComposition")
@Composable
fun AllProductsScreen(navController: NavHostController,snackbarHostState:SnackbarHostState, productos : List<ProductResponse>?, productViewModel: ProductViewModel) {
    val context = LocalContext.current
    var showDialog by remember { mutableStateOf(false) }
    var selectedProduct by remember { mutableStateOf<ProductResponse?>(null) }
    val coroutineScope = rememberCoroutineScope()

    Column(
        Modifier
            .fillMaxSize()
            .padding(16.dp)
        ,
        verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Button(
            onClick = {
                navController.navigate(Routes.AddProduct.route)
            },
            shape = RoundedCornerShape(8.dp),
            colors = ButtonDefaults.buttonColors(containerColor = MaterialTheme.colorScheme.primary)
        ) {
            Icon(
                imageVector = Icons.Default.Add,
                contentDescription = "Icono de acción",
                tint = MaterialTheme.colorScheme.onPrimary
            )
            Spacer(modifier = Modifier.width(8.dp))
            Text(text = context.getString(R.string.add_product_text), color = MaterialTheme.colorScheme.onPrimary)
        }

        if (productViewModel.productos.value == null) {
            // Muestra una barra circular mientras cargan actividades
            CircularProgressIndicator()
        }
        else if (productViewModel.productos.value!!.isEmpty()) {
            Text(context.getString(R.string.no_products_text))
        }
        else {
            val painter = painterResource(id = R.drawable.ic_launcher_foreground)

            if (showDialog && selectedProduct != null) {
                AlertDialog(
                    title = context.getString(R.string.delete_title),
                    description = context.getString(R.string.delete_description),
                    icon = painter,
                    confirmText = context.getString(R.string.exit_confirm),
                    dismissText = context.getString(R.string.exit_cancel),
                    confirm = {
                        coroutineScope.launch {
                            productViewModel.deleteProduct(selectedProduct!!.id)
                            showDialog = false
                            selectedProduct = null
                        }
                    },
                    dismiss = {
                        showDialog = false
                        selectedProduct = null
                    }
                )
            }
            //Muestra todas las actividades
            LazyColumn {
                items(productos!!) { producto ->
                    ShowProduct(
                        producto,
                        Icons.Default.Create,
                        context.getString(R.string.edit_text),
                        Icons.Default.Delete,
                        context.getString(R.string.delete_text),
                        onClickAction1 = {
                            coroutineScope.launch {
                                coroutineScope.launch {
                                    snackbarHostState.showSnackbar(
                                        context.getString(R.string.editing_product)+": "+producto.nombre ,
                                        duration = SnackbarDuration.Long
                                    )
                                }
                                productViewModel.selectProduct(producto)
                                navController.navigate(Routes.EditProduct.route)
                            }
                        },
                        onClickAction2 = {
                            coroutineScope.launch {
                                coroutineScope.launch {
                                    snackbarHostState.showSnackbar(
                                        context.getString(R.string.deleting_product)+": "+producto.nombre ,
                                        duration = SnackbarDuration.Long
                                    )
                                }
                                selectedProduct = producto
                                showDialog = true
                            }
                        }
                    )
                }
            }
        }

    }
}

@Composable
fun ShowProduct(
    product: ProductResponse,
    buttonIcon1: ImageVector,
    textButton1: String,
    buttonIcon2: ImageVector,
    textButton2: String,
    onClickAction1: () -> Unit,
    onClickAction2: () -> Unit
) {
    val scrollState = rememberScrollState()

    Card(
        elevation = CardDefaults.cardElevation(defaultElevation = 8.dp),
        border = BorderStroke(1.dp, MaterialTheme.colorScheme.outline),
        colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.surface),
        shape = RoundedCornerShape(16.dp),
        modifier = Modifier
            .fillMaxWidth()
            .padding(horizontal = 16.dp, vertical = 8.dp)
    ) {
        Column(
            modifier = Modifier
                .fillMaxWidth()
                .padding(16.dp),
            verticalArrangement = Arrangement.spacedBy(16.dp)
        ) {
            Row(
                modifier = Modifier.fillMaxWidth(),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(16.dp)
            ) {
                AsyncImage(
                    model = "http://10.0.2.2:5151${product.imagen}",
                    contentDescription = "Imagen del producto",
                    modifier = Modifier
                        .size(100.dp)
                        .clip(RoundedCornerShape(12.dp))
                        .border(1.dp, MaterialTheme.colorScheme.outline, RoundedCornerShape(12.dp)),
                    contentScale = ContentScale.Crop
                )

                Column(
                    modifier = Modifier.weight(1f),
                    verticalArrangement = Arrangement.spacedBy(4.dp)
                ) {
                    Text(
                        text = product.nombre,
                        style = MaterialTheme.typography.titleMedium,
                        color = MaterialTheme.colorScheme.onSurface,
                        maxLines = 1
                    )
                    Box(
                        modifier = Modifier
                            .height(60.dp)
                            .verticalScroll(scrollState)
                    ) {
                        Text(
                            text = product.descripcion,
                            style = MaterialTheme.typography.bodySmall,
                            color = MaterialTheme.colorScheme.onSurfaceVariant
                        )
                    }
                    Text(
                        text = product.disponible,
                        style = MaterialTheme.typography.bodyMedium,
                        color = MaterialTheme.colorScheme.onSurfaceVariant,
                        maxLines = 1
                    )
                }
            }

            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.End,
                verticalAlignment = Alignment.CenterVertically
            ) {
                Button(
                    onClick = onClickAction1,
                    shape = RoundedCornerShape(8.dp),
                    colors = ButtonDefaults.buttonColors(containerColor = MaterialTheme.colorScheme.primary)
                ) {
                    Icon(
                        imageVector = buttonIcon1,
                        contentDescription = "Icono de acción",
                        tint = MaterialTheme.colorScheme.onPrimary
                    )
                    Spacer(modifier = Modifier.width(8.dp))
                    Text(text = textButton1, color = MaterialTheme.colorScheme.onPrimary)
                }

                Spacer(modifier = Modifier.width(12.dp))

                Button(
                    onClick = onClickAction2,
                    shape = RoundedCornerShape(8.dp),
                    colors = ButtonDefaults.buttonColors(containerColor = MaterialTheme.colorScheme.secondary)
                ) {
                    Icon(
                        imageVector = buttonIcon2,
                        contentDescription = "Icono secundario",
                        tint = MaterialTheme.colorScheme.onSecondary
                    )
                    Spacer(modifier = Modifier.width(8.dp))
                    Text(text = textButton2, color = MaterialTheme.colorScheme.onSecondary)
                }
            }
        }
    }

}

@SuppressLint("StateFlowValueCalledInComposition", "CoroutineCreationDuringComposition")
@Composable
fun AllServicesScreen(navController: NavHostController,snackbarHostState:SnackbarHostState, services : List<ServiceResponse>?, serviceViewModel: ServiceViewModel) {
    val context = LocalContext.current

    val coroutineScope = rememberCoroutineScope()

    Column(
        Modifier
            .fillMaxSize()
            .padding(16.dp)
        ,
        verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Button(
            onClick = {
                navController.navigate(Routes.AddService.route)
            },
            shape = RoundedCornerShape(8.dp),
            colors = ButtonDefaults.buttonColors(containerColor = MaterialTheme.colorScheme.primary)
        ) {
            Icon(
                imageVector = Icons.Default.Add,
                contentDescription = "Icono de acción",
                tint = MaterialTheme.colorScheme.onPrimary
            )
            Spacer(modifier = Modifier.width(8.dp))
            Text(text = context.getString(R.string.add_service_text), color = MaterialTheme.colorScheme.onPrimary)
        }
        if (serviceViewModel.services.value == null) {
            // Muestra una barra circular mientras cargan actividades
            CircularProgressIndicator()
        }
        else if (serviceViewModel.services.value!!.isEmpty()) {
            Text(context.getString(R.string.no_services_text))
        }
        else {
            //Muestra todas las actividades
            LazyColumn {
                items(services!!) { service ->
                    ShowService(
                        service,
                        Icons.Default.Create,
                        context.getString(R.string.edit_text),
                    ) {
                        coroutineScope.launch {
                            coroutineScope.launch {
                                snackbarHostState.showSnackbar(
                                    context.getString(R.string.editing_service)+": "+service.nombre ,
                                    duration = SnackbarDuration.Long
                                )
                            }
                            serviceViewModel.selectService(service)
                            navController.navigate(Routes.EditService.route)
                        }
                    }
                }
            }
        }

    }
}

@Composable
fun ShowService(
    service: ServiceResponse,
    buttonIcon: ImageVector,
    textButton: String,
    onClickAction: () -> Unit
) {
    Card(
        elevation = CardDefaults.cardElevation(defaultElevation = 8.dp),
        border = BorderStroke(1.dp, MaterialTheme.colorScheme.outline),
        colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.surface),
        shape = RoundedCornerShape(16.dp),
        modifier = Modifier
            .fillMaxWidth()
            .padding(horizontal = 16.dp, vertical = 8.dp)
    ) {
        Column(
            modifier = Modifier
                .fillMaxWidth()
                .padding(16.dp),
            verticalArrangement = Arrangement.spacedBy(16.dp)
        ) {
            Row(
                modifier = Modifier.fillMaxWidth(),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(16.dp)
            ) {
                AsyncImage(
                    model = "http://10.0.2.2:5151${service.imagen}",
                    contentDescription = "Imagen del producto",
                    modifier = Modifier
                        .size(100.dp)
                        .clip(RoundedCornerShape(12.dp))
                        .border(1.dp, MaterialTheme.colorScheme.outline, RoundedCornerShape(12.dp)),
                    contentScale = ContentScale.Crop
                )

                Column(
                    modifier = Modifier.weight(1f),
                    verticalArrangement = Arrangement.spacedBy(4.dp)
                ) {
                    Text(
                        text = service.nombre,
                        style = MaterialTheme.typography.titleMedium,
                        color = MaterialTheme.colorScheme.onSurface,
                        maxLines = 1
                    )
                    Text(
                        text = service.descripcion,
                        style = MaterialTheme.typography.bodySmall,
                        color = MaterialTheme.colorScheme.onSurfaceVariant,
                        maxLines = 3
                    )
                }
            }

            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.End,
                verticalAlignment = Alignment.CenterVertically
            ) {
                Button(
                    onClick = onClickAction,
                    shape = RoundedCornerShape(8.dp),
                    colors = ButtonDefaults.buttonColors(containerColor = MaterialTheme.colorScheme.primary)
                ) {
                    Icon(
                        imageVector = buttonIcon,
                        contentDescription = "Icono de acción",
                        tint = MaterialTheme.colorScheme.onPrimary
                    )
                    Spacer(modifier = Modifier.width(8.dp))
                    Text(text = textButton, color = MaterialTheme.colorScheme.onPrimary)
                }
            }
        }
    }

}