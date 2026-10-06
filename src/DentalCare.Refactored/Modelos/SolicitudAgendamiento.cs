using System;
using DentaCare.Refactored.Entidades;
using DentaCare.Refactored.Enumeraciones;

namespace DentaCare.Refactored.Modelos;

/// <summary>
/// Modelo para solicitar el agendamiento de una cita.
/// </summary>
public record SolicitudAgendamiento(
    Paciente Paciente, Odontologo Odontologo, DateTime FechaHora,
    Especialidad Especialidad, bool RequiereRadiografia);
