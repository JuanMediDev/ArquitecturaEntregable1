using System.Collections.Generic;
using System.Linq;
using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;

namespace DentaCare.Refactored.Servicios;

/// <summary>
/// SRP: orquesta reglas de tarifa para calcular el copago.
/// </summary>
public sealed class CalculadorCopago : ICalculadorCopago
{
    private readonly IEnumerable<IReglaTarifa> _reglas;
    private const decimal Base = 100m;

    public CalculadorCopago(IEnumerable<IReglaTarifa> reglas)
    {
        _reglas = reglas ?? Enumerable.Empty<IReglaTarifa>();
    }

    public decimal Calcular(SolicitudAgendamiento solicitud)
    {
        decimal valor = Base;
        foreach (var regla in _reglas.OrderBy(r => r.Orden))
        {
            if (regla.Aplica(solicitud)) valor = regla.Aplicar(valor, solicitud);
        }

        return decimal.Round(valor, 2);
    }
}
