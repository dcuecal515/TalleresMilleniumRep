package com.example.talleresmileniumapp.Views

import android.annotation.SuppressLint
import android.app.DatePickerDialog
import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
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
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.AllInbox
import androidx.compose.material.icons.filled.Build
import androidx.compose.material.icons.filled.Check
import androidx.compose.material.icons.filled.Delete
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.RadioButton
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
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.navigation.NavHostController
import com.example.talleresmileniumapp.Data.Routes
import com.example.talleresmileniumapp.Models.Product.ProductResponse
import com.example.talleresmileniumapp.Models.Reservas.ReservaResponse
import com.example.talleresmileniumapp.R
import com.example.talleresmileniumapp.Themes.misFormas
import com.example.talleresmileniumapp.ViewModels.AuthState
import com.example.talleresmileniumapp.ViewModels.AuthViewModel
import com.example.talleresmileniumapp.ViewModels.ReservaViewModel
import kotlinx.coroutines.launch
import java.time.LocalDate
import java.util.Calendar

@Composable
fun Reservas(navController: NavHostController, authViewModel: AuthViewModel,reservaViewModel: ReservaViewModel) {
    val context = LocalContext.current

    val authState = authViewModel.authState.collectAsState()
    val pagerState = rememberPagerState(pageCount = { 2 })
    val accessToken by reservaViewModel.accessToken.collectAsState()
    val coroutineScope = rememberCoroutineScope()
    val reservasespera by reservaViewModel.reservasespera.collectAsState()
    val reservasacpetadas by reservaViewModel.reservasfinal.collectAsState()
    val tabs = listOf("En espera","Aceptadas")

    LaunchedEffect(authState.value) {
        when (authState.value) {
            is AuthState.Unauthenticated -> navController.navigate(Routes.Login.route)
            else -> Unit
        }
    }
    LaunchedEffect(accessToken) {
        accessToken?.let {
            reservaViewModel.getallreservas()
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

        Text(text = context.getString(R.string.reservations_title), fontSize = 35.sp)


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
                                        0 -> Icon(
                                            imageVector = Icons.Default.AllInbox,
                                            contentDescription = null
                                        )

                                        1 -> Icon(
                                            imageVector = Icons.Default.Build,
                                            contentDescription = null
                                        )
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
                    0 -> AllReservaEsperaScreen(navController, snackbarHostState, reservasespera,reservaViewModel)
                    1 -> AllReservaAceptarScreen(snackbarHostState, reservasacpetadas, reservaViewModel)
                }
            }
        }

    }
}
@SuppressLint("StateFlowValueCalledInComposition", "CoroutineCreationDuringComposition")
@Composable
fun AllReservaEsperaScreen(
    navController: NavHostController,
    snackbarHostState: SnackbarHostState,
    reservas: List<ReservaResponse>?,
    reservaViewModel: ReservaViewModel
) {
    val context = LocalContext.current
    val coroutineScope = rememberCoroutineScope()
    var fechaSeleccionada by remember { mutableStateOf<LocalDate?>(null) }
    var showDialog by remember { mutableStateOf(false) }
    var selectedServicioId by remember { mutableStateOf<String?>(null) }
    var selectedReserva by remember { mutableStateOf<ReservaResponse?>(null) }

    Column(
        Modifier
            .fillMaxSize()
            .padding(16.dp),
        verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        if (reservaViewModel.reservasespera.value == null) {
            CircularProgressIndicator()
        } else if (reservaViewModel.reservasespera.value!!.isEmpty()) {
            Text("No hay reservas todavía")
        } else {
            LazyColumn {
                items(reservas!!) { reserva ->
                    ShowReserva(
                        reserva,
                        Icons.Default.Check,
                        "Aceptar",
                        Icons.Default.Delete,
                        "Eliminar",
                        onClickAction1 = {
                            coroutineScope.launch {
                                snackbarHostState.showSnackbar(
                                    "Vas a aceptar los servicios solicitados por el vehículo con matrícula: ${reserva.matricula}",
                                    duration = SnackbarDuration.Short
                                )
                                val calendar = Calendar.getInstance()
                                val datePickerDialog = DatePickerDialog(
                                    context,
                                    { _, year, month, day ->
                                        val selectedDate = LocalDate.of(year, month + 1, day)
                                        fechaSeleccionada = selectedDate
                                        coroutineScope.launch {
                                            snackbarHostState.showSnackbar(
                                                "Fecha seleccionada para ${reserva.matricula}: $selectedDate"
                                            )
                                            reservaViewModel.putAceptada(reserva.fecha, reserva.matricula, selectedDate.toString())
                                        }
                                    },
                                    calendar.get(Calendar.YEAR),
                                    calendar.get(Calendar.MONTH),
                                    calendar.get(Calendar.DAY_OF_MONTH)
                                ).apply {
                                    datePicker.minDate = System.currentTimeMillis()
                                }

                                datePickerDialog.show()
                            }
                        },
                        onClickAction2 = {
                            selectedReserva = reserva
                            selectedServicioId = null
                            showDialog = true
                        }
                    )
                }
            }

            if (showDialog && selectedReserva != null) {
                AlertDialog(
                    onDismissRequest = { showDialog = false },
                    confirmButton = {
                        Button(onClick = {
                            showDialog = false
                            selectedServicioId?.let { servicioId ->
                                coroutineScope.launch {
                                    reservaViewModel.deleteReserva(servicioId)
                                    snackbarHostState.showSnackbar("Servicio eliminado")
                                }
                            }
                        }) {
                            Text("Eliminar")
                        }
                    },
                    dismissButton = {
                        Button(onClick = { showDialog = false }) {
                            Text("Cancelar")
                        }
                    },
                    title = { Text("Selecciona un servicio") },
                    text = {
                        Column {
                            selectedReserva?.servicios?.forEach { servicio ->
                                Row(verticalAlignment = Alignment.CenterVertically) {
                                    RadioButton(
                                        selected = selectedServicioId == servicio.idcoche_servicio,
                                        onClick = { selectedServicioId = servicio.idcoche_servicio }
                                    )
                                    Text(servicio.nombre)
                                }
                            }
                        }
                    }
                )
            }
        }
    }
}


    @Composable
    fun ShowReserva(
        reserva: ReservaResponse,
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
            shape = misFormas.large,
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
                    if(reserva.tipo=="Coche"){
                        Image(
                            painter = painterResource(id = R.drawable.coche2),
                            contentDescription = "Imagen de coche",
                            modifier = Modifier
                                .size(100.dp)
                                .clip(misFormas.large)
                                .border(1.dp, MaterialTheme.colorScheme.outline, misFormas.large),

                            contentScale = ContentScale.Crop
                        )
                    }else if(reserva.tipo=="Camion"){
                        Image(
                            painter = painterResource(id = R.drawable.camion2),
                            contentDescription = "Imagen de camion",
                            modifier = Modifier
                                .size(100.dp)
                                .clip(misFormas.large)
                                .border(1.dp, MaterialTheme.colorScheme.outline, misFormas.large),

                            contentScale = ContentScale.Crop
                        )
                    }else if(reserva.tipo=="Autobus"){
                        Image(
                            painter = painterResource(id = R.drawable.camion2),
                            contentDescription = "Imagen de autobus",
                            modifier = Modifier
                                .size(100.dp)
                                .clip(misFormas.large)
                                .border(1.dp, MaterialTheme.colorScheme.outline, misFormas.large),

                            contentScale = ContentScale.Crop
                        )
                    }

                    Column(
                        modifier = Modifier.weight(1f),
                        verticalArrangement = Arrangement.spacedBy(4.dp)
                    ) {
                        Text(
                            text = reserva.matricula,
                            style = MaterialTheme.typography.titleMedium,
                            color = MaterialTheme.colorScheme.onSurface,
                            maxLines = 1
                        )
                        Column(
                            modifier = Modifier
                                .height(90.dp)
                                .verticalScroll(scrollState)
                        ) {
                            reserva.servicios.forEach { servicio ->
                                Text(
                                    text = servicio.nombre,
                                    style = MaterialTheme.typography.bodySmall,
                                    color = MaterialTheme.colorScheme.onSurfaceVariant
                                )
                                Spacer(modifier = Modifier.height(10.dp))
                            }
                        }
                        Text(
                            text = reserva.fecha,
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
                        shape = misFormas.large,
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
                        shape = misFormas.large,
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
    fun AllReservaAceptarScreen(snackbarHostState:SnackbarHostState, reservas : List<ReservaResponse>?, reservaViewModel: ReservaViewModel) {
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
            if (reservaViewModel.reservasfinal.value == null) {
                CircularProgressIndicator()
            }
            else if (reservaViewModel.reservasfinal.value!!.isEmpty()) {
                Text(context.getString(R.string.no_vehicles))
            }
            else {
                //Muestra todas las actividades
                LazyColumn {
                    items(reservas!!) { reserva ->
                        ShowReserva2(
                            reserva,
                            Icons.Default.Check,
                            context.getString(R.string.finished_text),
                            onClickAction1 = {
                                coroutineScope.launch {
                                    coroutineScope.launch {
                                        snackbarHostState.showSnackbar(
                                            context.getString(R.string.finish_service_text)+": "+reserva.matricula ,
                                            duration = SnackbarDuration.Short

                                        )
                                        reservaViewModel.putFinalizado(reserva.fecha,reserva.matricula)
                                    }


                                }
                            }
                        )
                    }
                }
            }

        }
    }

@Composable
fun ShowReserva2(
    reserva: ReservaResponse,
    buttonIcon1: ImageVector,
    textButton1: String,
    onClickAction1: () -> Unit,
) {
    val scrollState = rememberScrollState()

    Card(
        elevation = CardDefaults.cardElevation(defaultElevation = 8.dp),
        border = BorderStroke(1.dp, MaterialTheme.colorScheme.outline),
        colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.surface),
        shape = misFormas.large,
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
                if(reserva.tipo=="Coche"){
                    Image(
                        painter = painterResource(id = R.drawable.coche2),
                        contentDescription = "Imagen de coche",
                        modifier = Modifier
                            .size(100.dp)
                            .clip(misFormas.large)
                            .border(1.dp, MaterialTheme.colorScheme.outline, misFormas.large),

                        contentScale = ContentScale.Crop
                    )
                }else if(reserva.tipo=="Camion"){
                    Image(
                        painter = painterResource(id = R.drawable.camion2),
                        contentDescription = "Imagen de camion",
                        modifier = Modifier
                            .size(100.dp)
                            .clip(misFormas.large)
                            .border(1.dp, MaterialTheme.colorScheme.outline, misFormas.large),

                        contentScale = ContentScale.Crop
                    )
                }else if(reserva.tipo=="Autobus"){
                    Image(
                        painter = painterResource(id = R.drawable.camion2),
                        contentDescription = "Imagen de autobus",
                        modifier = Modifier
                            .size(100.dp)
                            .clip(misFormas.large)
                            .border(1.dp, MaterialTheme.colorScheme.outline, misFormas.large),

                        contentScale = ContentScale.Crop
                    )
                }

                Column(
                    modifier = Modifier.weight(1f),
                    verticalArrangement = Arrangement.spacedBy(4.dp)
                ) {
                    Text(
                        text = reserva.matricula,
                        style = MaterialTheme.typography.titleMedium,
                        color = MaterialTheme.colorScheme.onSurface,
                        maxLines = 1
                    )
                    Column(
                        modifier = Modifier
                            .height(90.dp)
                            .verticalScroll(scrollState)
                    ) {
                        reserva.servicios.forEach { servicio ->
                            Text(
                                text = servicio.nombre,
                                style = MaterialTheme.typography.bodySmall,
                                color = MaterialTheme.colorScheme.onSurfaceVariant
                            )
                            Spacer(modifier = Modifier.height(10.dp))
                        }
                    }
                    Text(
                        text = reserva.fecha,
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
                    shape = misFormas.medium,
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

            }
        }
    }
}
