using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using System.Windows.Forms;
using KinniNet.Core.Demonio;
using Timer = System.Timers.Timer;

namespace KiiniNet.Services.Windows
{
    public partial class ServiceTicketMail : ServiceBase
    {
        private readonly Timer _intervaloEjecucion;
        private const string ServiceApplicationName = "Service Mail Support";

        public ServiceTicketMail()
        {
            InitializeComponent();
            _intervaloEjecucion = new Timer(10000);
            _intervaloEjecucion.Elapsed += intervaloEjecucion_Elapsed;
        }

        void intervaloEjecucion_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                new BusinessTicketMailService().RecibeCorreos();
            }
            catch (Exception ex)
            {
                Log("KiiniNet", ServiceApplicationName, ex.Message);
            }
        }

        void Log(string source, string application, string mensaje)
        {
            try
            {
                MessageBox.Show(mensaje);
                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, application);

                EventLog.WriteEntry(source, mensaje);
                EventLog.WriteEntry(source, mensaje,
                    EventLogEntryType.Warning, 234);
            }
            catch (Exception ex)
            {
                Log("KiiniNet", ServiceApplicationName, ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            Debugger.Launch();
            try
            {
                _intervaloEjecucion.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log("KiiniNet", ServiceApplicationName, ex.Message);
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
                Log("KiiniNet", ServiceApplicationName, ex.Message);
            }
        }
    }
}
