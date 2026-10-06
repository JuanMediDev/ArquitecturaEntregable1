using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Interfaces;

/// <summary>
/// Canal de envío de notificaciones (email, sms, etc.).
/// </summary>
public interface ICanalNotificacion { void Enviar(MensajeNotificacion mensaje); }
