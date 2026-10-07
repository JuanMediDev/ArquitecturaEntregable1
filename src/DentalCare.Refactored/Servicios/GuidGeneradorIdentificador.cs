using System;
using DentaCare.Refactored.Interfaces;

namespace DentaCare.Refactored.Servicios;

public sealed class GuidGeneradorIdentificador : IGeneradorIdentificador
{
    public string Generar() => Guid.NewGuid().ToString()[..8];
}
