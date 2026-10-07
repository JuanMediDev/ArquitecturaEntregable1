using System;
using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Servicios;

public sealed class CanalCorreoEnMemoria : ICanalNotificacion
{
    public void Enviar(MensajeNotificacion mensaje)
    {
        Console.WriteLine($"[CorreoEnMemoria] Enviando a {mensaje.Correo}: {mensaje.Asunto}");
    }
}
