using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;
using DentaCare.Refactored.Enumeraciones;

namespace DentaCare.Refactored.Reglas.Tarifas;

public sealed class DescuentoEps : IReglaTarifa
{
    private const decimal FactorCopago = 0.30m;
    public int Orden => 20;

    public bool Aplica(SolicitudAgendamiento solicitud) => solicitud.Paciente.Convenio == Convenio.Eps;

    public decimal Aplicar(decimal valorActual, SolicitudAgendamiento solicitud) => valorActual * FactorCopago;
}
