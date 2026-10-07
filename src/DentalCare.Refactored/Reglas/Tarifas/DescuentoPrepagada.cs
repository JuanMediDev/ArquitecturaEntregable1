using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;
using DentaCare.Refactored.Enumeraciones;

namespace DentaCare.Refactored.Reglas.Tarifas;

public sealed class DescuentoPrepagada : IReglaTarifa
{
    private const decimal FactorDescuento = 0.10m;
    public int Orden => 20;

    public bool Aplica(SolicitudAgendamiento solicitud) => solicitud.Paciente.Convenio == Convenio.Prepagada;

    public decimal Aplicar(decimal valorActual, SolicitudAgendamiento solicitud) => valorActual * FactorDescuento;
}
