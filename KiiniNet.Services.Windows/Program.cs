using System.ServiceProcess;

namespace KiiniNet.Services.Windows
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;

            ServicesToRun = new ServiceBase[] 
            { 
                new ServiceNotificacion() 

            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
