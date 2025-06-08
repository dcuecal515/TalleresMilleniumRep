package com.example.talleresmileniumapp.Dialog

import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.ui.graphics.ImageBitmap
import androidx.compose.ui.graphics.painter.Painter

@Composable
//Alert Dialog genérico
fun AlertDialog(title : String,
                description : String,
                icon : Painter,
                confirmText : String,
                dismissText : String,
                confirm: () -> Unit,
                dismiss: () -> Unit)
{

    androidx.compose.material3.AlertDialog(
        icon = { Icon(icon, contentDescription = description) },

        onDismissRequest = {
            dismiss()
        },
        title = {
            Text(title)
        },
        text = {
            Text(description)
        },
        confirmButton = {
            TextButton(
                onClick = {
                    confirm()
                }
            ) {
                Text(confirmText)
            }
        },
        dismissButton = {
            TextButton(
                onClick = {
                    dismiss()
                }
            ) {
                Text(dismissText)
            }
        },
    )

}