using System.Collections.Generic;
using System.Linq;
using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Servicios;

public sealed class CalculadorPenalizacion : ICalculadorPenalizacion
{
    private readonly IEnumerable<IReglaPenalizacion> _reglas;

    public CalculadorPenalizacion(IEnumerable<IReglaPenalizacion> reglas)
    {
        _reglas = reglas ?? Enumerable.Empty<IReglaPenalizacion>();
    }

    public decimal Calcular(ContextoCancelacion contexto)
    {
        return _reglas
            .Where(r => r.Aplica(contexto))
            .Sum(r => r.Calcular(contexto));
    }
}
