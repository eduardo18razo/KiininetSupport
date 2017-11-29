using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace KinniNet.Business.Utils
{
    public static class BusinessCadenas
    {
        public static class Fechas
        {
            public static DateTime ObtenerFechaInicioSemana(int anio, int numeroSemana)
            {
                DateTime result;
                try
                {
                    DateTime jan1 = new DateTime(anio, 1, 1);
                    int daysOffset = DayOfWeek.Tuesday - jan1.DayOfWeek;
                    DateTime firstMonday = jan1.AddDays(daysOffset);
                    var cal = CultureInfo.CurrentCulture.Calendar;
                    int firstWeek = cal.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    int weekNum = numeroSemana;
                    if (firstWeek <= 1)
                    {
                        weekNum -= 1;
                    }

                    result = firstMonday.AddDays(weekNum*7 + 0 - 1);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }

            public static DateTime ObtenerFechaFinSemana(int anio, int numeroSemana)
            {
                DateTime result;
                try
                {
                    DateTime jan1 = new DateTime(anio, 1, 1);
                    int daysOffset = DayOfWeek.Tuesday - jan1.DayOfWeek;
                    DateTime firstMonday = jan1.AddDays(daysOffset);
                    var cal = CultureInfo.CurrentCulture.Calendar;
                    int firstWeek = cal.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    int weekNum = numeroSemana;
                    if (firstWeek <= 1)
                    {
                        weekNum -= 1;
                    }
                    result = firstMonday.AddDays(weekNum * 7 + 0 - 1).AddDays(6);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }

            public static List<DateTime> ObtenerRangoFechasNumeroSemana(int anio, int numeroSemana)
            {
                List<DateTime> result = new List<DateTime>();
                try
                {
                    DateTime jan1 = new DateTime(anio, 1, 1);
                    int daysOffset = DayOfWeek.Tuesday - jan1.DayOfWeek;
                    DateTime firstMonday = jan1.AddDays(daysOffset);
                    var cal = CultureInfo.CurrentCulture.Calendar;
                    int firstWeek = cal.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    int weekNum = numeroSemana;
                    if (firstWeek <= 1)
                    {
                        weekNum -= 1;
                    }
                    result.Add(firstMonday.AddDays(weekNum * 7 + 0 - 1));
                    result.Add(result[0].AddDays(6));
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }
        }
        public static class Cadenas
        {
            public static string ReemplazaAcentos(string cadena)
            {
                Regex a = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
                Regex e = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
                Regex i = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
                Regex o = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
                Regex u = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
                Regex n = new Regex("[ñ|Ñ]", RegexOptions.Compiled);
                cadena = a.Replace(cadena, "a");
                cadena = e.Replace(cadena, "e");
                cadena = i.Replace(cadena, "i");
                cadena = o.Replace(cadena, "o");
                cadena = u.Replace(cadena, "u");
                cadena = n.Replace(cadena, "n");
                return cadena;
            }

            public static string RemoverCaracteresEspeciales(string str)
            {
                return Regex.Replace(str, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
            }

            public static string FormatoBaseDatos(string cadena)
            {
                return Regex.Replace(ReemplazaAcentos(RemoverCaracteresEspeciales(cadena)), "[^0-9a-zA-Z]+", "");
            }
        }
        
    }
}
