using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using KiiniNet.Entities.Parametros;
using KinniNet.Core.Demonio;
using KinniNet.Core.Parametros;
using Timer = System.Timers.Timer;

namespace DemonioTicketsCorreo
{
    public partial class ServiceTicketsCorreo : ServiceBase
    {
        private Timer _intervaloEjecucion;
        public ServiceTicketsCorreo()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                BusinessParametros bparams = new BusinessParametros();
                ParametrosGenerales parametros = bparams.ObtenerParametrosGenerales();
                double intervalo = 60000;
                if (parametros != null)
                {
                    intervalo = parametros.FrecuenciaDemonioSegundos * 1000;
                    _intervaloEjecucion = new Timer(intervalo);
                }
                else
                {
                    _intervaloEjecucion = new Timer(intervalo);
                }

                _intervaloEjecucion.Elapsed += intervaloEjecucion_Elapsed;
                _intervaloEjecucion.Start();
                LogCorrect("KiiniNet Tickets Correo", "Tickets por correo", "Timer Start");
            }
            catch (Exception ex)
            {
                LogError("KiiniNet Tickets Correo", "Tickets por correo", ex.Message);
            }
        }
        void LogCorrect(string source, string application, string mensaje)
        {
            try
            {
                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, application);

                EventLog.WriteEntry(source, mensaje);
                EventLog.WriteEntry(source, mensaje,
                    EventLogEntryType.SuccessAudit, 234);
            }
            catch (Exception ex)
            {
                LogError("KiiniNet Tickets Correo", "Tickets por correo", ex.Message);
            }
        }

        void LogError(string source, string application, string mensaje)
        {
            try
            {
                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, application);

                EventLog.WriteEntry(source, mensaje);
                EventLog.WriteEntry(source, mensaje,
                    EventLogEntryType.Error, 234);
            }
            catch (Exception ex)
            {
                LogError("KiiniNet Tickets Correo", "Tickets por correo", ex.Message);
            }
        }
        void intervaloEjecucion_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Thread.Sleep(1000);
                LogCorrect("KiiniNet Tickets Correo", "Tickets por correo", "Iniciando Recepcion de correos");
                new BusinessTicketMailService().RecibeCorreos();
            }
            catch (Exception ex)
            {
                LogError("KiiniNet Tickets Correo", "Tickets por correo", ex.Message);

            }
        }

        protected override void OnStop()
        {
            try
            {
                _intervaloEjecucion.Stop();
            }
            catch (Exception ex)
            {
                LogError("KiiniNet Tickets Correo Tickets Correo", "Tickets por correo", ex.Message);
            }
        }
    }
}
