using System;
using System.Collections.Generic;
using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Servicios;

public sealed class NotificadorCompuesto : INotificador
{
    private readonly IEnumerable<ICanalNotificacion> _canales;

    public NotificadorCompuesto(IEnumerable<ICanalNotificacion> canales)
    {
        _canales = canales ?? throw new ArgumentNullException(nameof(canales));
    }

    public void Notificar(MensajeNotificacion mensaje)
    {
        foreach (var canal in _canales)
        {
            canal.Enviar(mensaje);
        }
    }
}
