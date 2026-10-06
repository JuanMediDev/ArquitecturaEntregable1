using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Interfaces;

/// <summary>
/// Regla para calcular penalizaciones por cancelación.
/// </summary>
public interface IReglaPenalizacion
{
    bool Aplica(ContextoCancelacion contexto);
    decimal Calcular(ContextoCancelacion contexto);
}
