using System;
using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Servicios;

public sealed class CanalSmsEnMemoria : ICanalNotificacion
{
    public void Enviar(MensajeNotificacion mensaje)
    {
        Console.WriteLine($"[SmsEnMemoria] Enviando SMS a {mensaje.Celular}");
    }
}
