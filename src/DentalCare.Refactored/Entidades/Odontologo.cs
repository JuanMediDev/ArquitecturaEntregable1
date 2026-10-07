using DentaCare.Refactored.Enumeraciones;

namespace DentaCare.Refactored.Entidades;

public sealed class Odontologo
{
    public string Id { get; init; } = string.Empty;
    public string Nombre { get; init; } = string.Empty;
    public Especialidad Especialidad { get; init; }
    public bool EstaDisponible { get; init; }
}
