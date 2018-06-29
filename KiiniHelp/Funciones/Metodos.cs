using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using KinniNet.Business.Utils;

namespace KiiniHelp.Funciones
{
    public static class Metodos
    {
        public static class Strings
        {
            public static string CastToEnum(string input)
            {
                if (String.IsNullOrEmpty(input))
                    throw new ArgumentException("Cadena vacia");
                string[] cadena = input.Trim().Split(' ');
                input = string.Empty;
                foreach (string word in cadena)
                {
                    input += word.First().ToString() + word.Trim().Substring(1);
                }

                return input;
            }
        }

        public static class Enumeradores
        {
            public static T GetStringEnum<T>(string cadena)
            {
                return (T)Enum.Parse(typeof(T), Strings.CastToEnum(cadena.ToLower()));
            }
            public static T GetValueEnumFromString<T>(string cadena)
            {
                return (T)Enum.Parse(typeof(T), Strings.CastToEnum(cadena.ToLower()));
            }
        }
        public static void LimpiarCombo(DropDownList ddl)
        {
            try
            {
                ddl.Items.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void LimpiarRadioList(RadioButtonList rbtnlst)
        {
            try
            {
                rbtnlst.Items.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void LimpiarListBox(ListBox rbtnlst)
        {
            try
            {
                rbtnlst.Items.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void LlenaComboCatalogo(DropDownList ddl, object datos, string descripcion = "Descripcion")
        {
            try
            {
                ddl.Items.Clear();
                ddl.DataSource = datos;
                ddl.DataTextField = descripcion;
                ddl.DataValueField = "Id";
                ddl.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static void LlenaComboDuracionEnumerador(DropDownList ddl)
        {
            try
            {
                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(BusinessVariables.ComboBoxCatalogo.ValueSeleccione, BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione);
                foreach (BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion foo in Enum.GetValues(typeof(BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion)))
                {
                    data.Add((int)foo, foo.ToString());
                }
                ddl.DataSource = data;
                ddl.DataTextField = "Value";
                ddl.DataValueField = "Key";
                ddl.DataBind();

                //Array duraciones = Enum.GetValues(typeof(BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion));

                //foreach (BusinessVariables.EnumeradoresKiiniNet.EnumTiempoDuracion duracion in duraciones)
                //{
                //    ddl.Items.Add(new ListItem(duracion.ToString(), ((int)duracion).ToString()));
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void LlenaListBoxCatalogo(CheckBoxList chkLbx, object datos)
        {
            try
            {
                if (chkLbx.DataSource != null)
                {
                    chkLbx.DataSource = null;
                    chkLbx.DataBind();
                }
                chkLbx.DataSource = datos;
                chkLbx.DataTextField = "Descripcion";
                chkLbx.DataValueField = "Id";
                chkLbx.DataBind();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static void LlenaListBoxCatalogo(ListBox lbx, object datos)
        {
            try
            {
                if (lbx.DataSource != null)
                {
                    lbx.DataSource = null;
                    lbx.DataBind();
                }
                lbx.DataSource = datos;
                lbx.DataTextField = "Descripcion";
                lbx.DataValueField = "Id";
                lbx.DataBind();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static void LlenaListBoxCatalogo(RadioButtonList rbtnLbx, object datos)
        {
            try
            {
                if (rbtnLbx.DataSource != null)
                {
                    rbtnLbx.DataSource = null;
                    rbtnLbx.DataBind();
                }
                rbtnLbx.DataSource = datos;
                rbtnLbx.DataTextField = "Descripcion";
                rbtnLbx.DataValueField = "Id";
                rbtnLbx.DataBind();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static void FiltraCombo(DropDownList ddlFiltro, DropDownList ddlLlenar, object source)
        {
            try
            {
                ddlLlenar.Items.Clear();
                if (ddlFiltro.SelectedValue != BusinessVariables.ComboBoxCatalogo.ValueSeleccione.ToString())
                {
                    ddlLlenar.Enabled = true;
                    LlenaComboCatalogo(ddlLlenar, source);
                }
                else
                {
                    ddlLlenar.DataSource = null;
                    ddlLlenar.DataBind();
                }

                ddlLlenar.Enabled = ddlLlenar.DataSource != null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool ValidaCapturaCatalogoCampus(int idTipoUsuario, string descripcion, int idColonia, string calle, string noExt, string noInt)
        {
            StringBuilder sb = new StringBuilder();
            if (idTipoUsuario == BusinessVariables.ComboBoxCatalogo.ValueSeleccione)
                sb.AppendLine("Tipo de usuario es un campo obligatorio.<br>");
            if (descripcion == string.Empty)
                sb.AppendLine("Descripción es un campo obligatorio.<br>");
            if (idColonia == BusinessVariables.ComboBoxCatalogo.ValueSeleccione)
                sb.AppendLine("Colonia es un campo obligatorio.<br>");
            if (calle == string.Empty)
                sb.AppendLine("Calle es un campo obligatorio.<br>");
            if (noExt == string.Empty)
                sb.AppendLine("Número Exterior es un campo obligatorio.<br>");
            //if (noInt == string.Empty)
            //    sb.AppendLine("Número Interior es un campo obligatorio.<br>");
            if (sb.ToString() != string.Empty)
                throw new Exception(sb.ToString());
            return true;
        }

        public static bool ValidaCapturaCatalogo(string descripcion)
        {
            if (descripcion.Trim() == string.Empty)
                throw new Exception("Descripción es un campo obligatorio.");
                
            return true;
        }

        public static bool HashPermission()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return true;
        }

        public static class ManejoFechas
        {

            public static  Dictionary<string, DateTime> ObtenerFechas(int idTipoFecha, string fechaInicio, string fechaFin)
            {
                try
                {
                    Dictionary<string, DateTime> result = null;

                    switch (idTipoFecha)
                    {
                        case 1:
                            if (fechaInicio.Trim() == string.Empty || fechaFin.Trim() == string.Empty)
                                return null;
                            if (DateTime.Parse(fechaInicio) > DateTime.Parse(fechaFin))
                                throw new Exception("Fecha Inicio no puede se mayor a Fecha Fin");
                            result = new Dictionary<string, DateTime>
                        {
                            {"inicio", Convert.ToDateTime(fechaInicio)},
                            {"fin", Convert.ToDateTime(fechaFin)}
                        };
                            break;
                        case 2:
                            if (fechaInicio.Trim() == string.Empty || fechaInicio.Trim() == string.Empty)
                                return null;
                            int anioInicialSemana = Convert.ToInt32(fechaInicio.Split('-')[0]);
                            int semanaInicialSemana = Convert.ToInt32(fechaInicio.Split('-')[1].Substring(1));
                            int anioFinSemana = Convert.ToInt32(fechaFin.Split('-')[0]);
                            int semanaFinSemana = Convert.ToInt32(fechaFin.Split('-')[1].Substring(1));

                            result = new Dictionary<string, DateTime>
                        {
                            {"inicio", BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anioInicialSemana, semanaInicialSemana)},
                            {"fin", BusinessCadenas.Fechas.ObtenerFechaFinSemana(anioFinSemana, semanaFinSemana)}
                        };

                            break;
                        case 3:
                            if (fechaInicio.Trim() == string.Empty || fechaInicio.Trim() == string.Empty)
                                return null;
                            int anioInicialMes = Convert.ToInt32(fechaInicio.Split('-')[0]);
                            int mesInicialMes = Convert.ToInt32(fechaInicio.Split('-')[1]);
                            int anioFinMes = Convert.ToInt32(fechaFin.Split('-')[0]);
                            int mesFinMes = Convert.ToInt32(fechaFin.Split('-')[1]);
                            if ((anioInicialMes > anioFinMes) || (mesInicialMes > mesFinMes))
                                throw new Exception("Fecha Inicio no puede se mayor a Fecha Fin");

                            result = new Dictionary<string, DateTime>
                        {
                            {"inicio", new DateTime(anioInicialMes, mesInicialMes, 1)},
                            {"fin", new DateTime(anioFinMes, mesFinMes, DateTime.DaysInMonth(anioFinMes, mesFinMes))}
                        };

                            break;
                        case 4:
                            if (fechaInicio.Trim() == string.Empty || fechaInicio.Trim() == string.Empty)
                                return null;
                            if (int.Parse(fechaInicio) > int.Parse(fechaFin))
                                throw new Exception("Año Inicio no puede se mayor a año Fin");
                            result = new Dictionary<string, DateTime>
                        {
                            {"inicio", new DateTime(int.Parse(fechaInicio), 01, 1)},
                            {"fin", new DateTime(int.Parse(fechaFin), 12, DateTime.DaysInMonth(int.Parse(fechaFin), 12))}
                        };
                            break;
                    }
                    return result;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }
}