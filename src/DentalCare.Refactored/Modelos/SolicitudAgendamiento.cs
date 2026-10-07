using System;
using DentaCare.Refactored.Entidades;
using DentaCare.Refactored.Enumeraciones;

namespace DentaCare.Refactored.Modelos;

public record SolicitudAgendamiento(
    Paciente Paciente, Odontologo Odontologo, DateTime FechaHora,
    Especialidad Especialidad, bool RequiereRadiografia);
