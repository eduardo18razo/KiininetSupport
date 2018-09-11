using System.ServiceProcess;

namespace DemonioCierreTicket
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
            ServiceBase[] servicesToRun = new ServiceBase[] 
            { 
                new ServiceCierreTicket()

            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
