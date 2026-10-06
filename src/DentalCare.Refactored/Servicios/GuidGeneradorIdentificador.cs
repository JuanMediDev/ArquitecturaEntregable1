using System;
using DentaCare.Refactored.Interfaces;

namespace DentaCare.Refactored.Servicios;

/// <summary>
/// DIP: genera identificadores cortos a partir de Guid.
/// </summary>
public sealed class GuidGeneradorIdentificador : IGeneradorIdentificador
{
    public string Generar() => Guid.NewGuid().ToString()[..8];
}
