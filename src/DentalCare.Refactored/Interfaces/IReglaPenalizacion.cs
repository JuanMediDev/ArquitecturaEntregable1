using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Interfaces;

public interface IReglaPenalizacion
{
    bool Aplica(ContextoCancelacion contexto);
    decimal Calcular(ContextoCancelacion contexto);
}
