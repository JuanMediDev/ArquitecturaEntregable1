using DentaCare.Refactored.Entidades;
using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Interfaces;

/// <summary>
/// Servicio de orquestación para agendar citas.
/// </summary>
public interface IServicioAgendamiento { Cita Agendar(SolicitudAgendamiento solicitud); }
