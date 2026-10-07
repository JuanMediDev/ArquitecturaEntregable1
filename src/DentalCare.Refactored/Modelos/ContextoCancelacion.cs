using System;
using DentaCare.Refactored.Entidades;

namespace DentaCare.Refactored.Modelos;

public record ContextoCancelacion(Cita Cita, DateTime FechaHoraCancelacion)
{
    public TimeSpan Anticipacion => Cita.FechaHora - FechaHoraCancelacion;
}
