using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;
using DentaCare.Refactored.Entidades;
using DentaCare.Refactored.Enumeraciones;

namespace DentaCare.Refactored.Reglas.Penalizaciones;

public sealed class RecargoCancelacionTardiaCirugia : IReglaPenalizacion
{
    public bool Aplica(ContextoCancelacion contexto)
    {
        return contexto.Anticipacion.TotalHours < DentaCare.Refactored.Reglas.PoliticaCancelacion.HorasLimite
               && contexto.Cita.Odontologo.Especialidad == Especialidad.Cirugia;
    }

    public decimal Calcular(ContextoCancelacion contexto) => 40.0m;
}
