using System;

namespace KinniNet.Business.Utils
{
    public static class UtilsTicket
    {
        public static int? ObtenerRolAsignacionByIdNivel(int? idNivel)
        {
            int? result = null;
            try
            {
                switch (idNivel)
                {
                    case null:
                        result = null;
                        break;
                    case 1:
                        result = (int)BusinessVariables.EnumeradoresKiiniNet.EnumeradorNivelAsignacion.Supervisor;
                        break;
                    case 2:
                        result = (int)BusinessVariables.EnumeradoresKiiniNet.EnumeradorNivelAsignacion.PrimerNivel;
                        break;
                    case 3:
                        result = (int)BusinessVariables.EnumeradoresKiiniNet.EnumeradorNivelAsignacion.SegundoNivel;
                        break;
                    case 4:
                        result = (int)BusinessVariables.EnumeradoresKiiniNet.EnumeradorNivelAsignacion.TercerNivel;
                        break;
                    case 5:
                        result = (int)BusinessVariables.EnumeradoresKiiniNet.EnumeradorNivelAsignacion.CuartoNivel;
                        break;
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            return result;
        }

        public static string GeneraCampoRandom()
        {
            string result = null;
            try
            {
                Random obj = new Random();
                int longitud = BusinessVariables.ParametrosMascaraCaptura.CaracteresCampoRandom.Length;
                for (int i = 0; i < BusinessVariables.ParametrosMascaraCaptura.LongitudRandom; i++)
                {
                    result += BusinessVariables.ParametrosMascaraCaptura.CaracteresCampoRandom[obj.Next(longitud)];
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return result;
        }
    }
}
