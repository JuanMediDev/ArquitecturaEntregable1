using System;
using DentaCare.Refactored.Entidades;

namespace DentaCare.Refactored.Modelos;

/// <summary>
/// Contexto usado en el cálculo de penalizaciones por cancelación.
/// </summary>
public record ContextoCancelacion(Cita Cita, DateTime FechaHoraCancelacion)
{
    public TimeSpan Anticipacion => Cita.FechaHora - FechaHoraCancelacion;
}
