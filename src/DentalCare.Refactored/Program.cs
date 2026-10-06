using System;
using System.Collections.Generic;
using DentaCare.Refactored.Entidades;
using DentaCare.Refactored.Enumeraciones;
using DentaCare.Refactored.Reglas.Tarifas;
using DentaCare.Refactored.Reglas.Penalizaciones;
using DentaCare.Refactored.Servicios;
using DentaCare.Refactored.Interfaces;
using DentaCare.Refactored.Modelos;

// Composición manual (Pure DI)
var reglasTarifa = new IReglaTarifa[] {
    new TarifaOrtodoncia(), new TarifaEndodoncia(), new TarifaCirugia(), new TarifaOdontopediatria(),
    new DescuentoEps(), new DescuentoPrepagada(), new RecargoPrimeraVez(), new RecargoRadiografia()
};

var reglasPenalizacion = new IReglaPenalizacion[] {
    new PenalizacionCancelacionTardia(), new RecargoCancelacionTardiaCirugia()
};

var calculadorCopago = new CalculadorCopago(reglasTarifa);
var calculadorPenalizacion = new CalculadorPenalizacion(reglasPenalizacion);
var validador = new ValidadorDisponibilidadPorBandera();
var generador = new GuidGeneradorIdentificador();
var reporte = new ReporteCitasEnMemoria();
var constructor = new ConstructorMensajesCita();
var notificador = new NotificadorEnMemoria();
var repositorio = new RepositorioEnMemoria();

var servicioAgendamiento = new ServicioAgendamiento(calculadorCopago, generador, validador, reporte, constructor, notificador, repositorio);
var servicioCancelacion = new ServicioCancelacion(calculadorPenalizacion, reporte, constructor, notificador, repositorio);

// Escenario de ejemplo
var paciente = new Paciente { Id = "p1", NombreCompleto = "Ana Pérez", Correo = "ana@correo.com", Celular = "3000000000", Convenio = Convenio.Eps, EsPrimeraVez = true };
var odontologo = new Odontologo { Id = "d1", Nombre = "Dr. Gómez", Especialidad = Especialidad.Cirugia, EstaDisponible = true };
var solicitud = new SolicitudAgendamiento(paciente, odontologo, DateTime.Now.AddHours(12), Especialidad.Cirugia, RequiereRadiografia: true);

var cita = servicioAgendamiento.Agendar(solicitud);
Console.WriteLine($"Cita agendada: {cita.Id}, Copago: {cita.CopagoCalculado:C2}");

var penalizacion = servicioCancelacion.Cancelar(cita, DateTime.Now);
Console.WriteLine($"Cita cancelada: {cita.Id}, Penalización aplicada: {penalizacion:C2}");

Console.WriteLine($"Total recaudado (memoria): {reporte.ObtenerTotalRecaudado():C2}");
Console.WriteLine($"Total canceladas (memoria): {reporte.ObtenerTotalCanceladas()}");
