using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Interfaces;

/// <summary>
/// Orquesta el envío de notificaciones.
/// </summary>
public interface INotificador { void Notificar(MensajeNotificacion mensaje); }
