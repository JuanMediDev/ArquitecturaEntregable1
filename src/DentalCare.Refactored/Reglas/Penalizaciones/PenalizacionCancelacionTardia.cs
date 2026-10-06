using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Reglas.Penalizaciones;

public class PenalizacionCancelacionTardia : IReglaPenalizacion
{
    public bool Aplica(ContextoCancelacion contexto) => contexto.Anticipacion.TotalHours < DentaCare.Refactored.Reglas.PoliticaCancelacion.HorasLimite;

    public decimal Calcular(ContextoCancelacion contexto) => 50.0m;
}
