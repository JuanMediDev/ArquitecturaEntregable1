using DentaCare.Refactored.Enumeraciones;

namespace DentaCare.Refactored.Entidades;

public sealed class Paciente
{
    public string Id { get; init; } = string.Empty;
    public string NombreCompleto { get; init; } = string.Empty;
    public string Correo { get; init; } = string.Empty;
    public string Celular { get; init; } = string.Empty;
    public Convenio Convenio { get; init; }
    public bool EsPrimeraVez { get; init; }
}
