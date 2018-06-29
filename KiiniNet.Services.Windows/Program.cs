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
//#if DEBUG
//            ServiceNotificacion myService = new ServiceNotificacion();
//            myService.OnDebugg();
//            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
//#else

            ServiceBase[] servicesToRun = new ServiceBase[] 
            { 
                new ServiceNotificacion() 

            };
            ServiceBase.Run(servicesToRun);
//#endif
        }
    }
}
