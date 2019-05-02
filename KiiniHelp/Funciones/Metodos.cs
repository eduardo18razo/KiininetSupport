using System;
using System.Collections.Generic;
using System.Linq;
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

        public static List<string> ValidaCapturaCatalogoCampus(int idTipoUsuario, string descripcion, int idColonia, string calle, string noExt, string noInt)
        {
            List<string> sb = new List<string>();
            if (idTipoUsuario == BusinessVariables.ComboBoxCatalogo.ValueSeleccione)
                sb.Add("Tipo de usuario es un campo obligatorio.");
            if (descripcion == string.Empty)
                sb.Add("Descripción es un campo obligatorio.");
            if (idColonia == BusinessVariables.ComboBoxCatalogo.ValueSeleccione)
                sb.Add("Colonia es un campo obligatorio.");
            if (calle == string.Empty)
                sb.Add("Calle es un campo obligatorio.");
            if (noExt == string.Empty)
                sb.Add("Número Exterior es un campo obligatorio.");
            //if (noInt == string.Empty)
            //    sb.AppendLine("Número Interior es un campo obligatorio.<br>");
            //if (sb.Count<=0)
            //{
            //    sb = null;
            //}
            return sb;
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

            public static Dictionary<string, DateTime> ObtenerFechas(int idTipoFecha, string fechaInicio, string fechaFin)
            {
                try
                {

                    if (fechaInicio.Trim() != string.Empty || fechaFin.Trim() != string.Empty)
                    {
                        if (fechaInicio.Length < 10)
                        {
                            string[] fechaParserInicio = fechaInicio.Split('/');
                            if (fechaParserInicio.Length < 3)
                                throw new Exception("Formato de fecha incorrecto dd/mm/yyyy");
                            fechaInicio = string.Empty;
                            for (int i = 0; i < fechaParserInicio.Length; i++)
                            {

                                if (i == 2)
                                {
                                    if (fechaParserInicio[i].Length < 4)
                                    {
                                        throw new Exception("Formato de fecha incorrecto dd/mm/yyyy");
                                    }
                                }
                                else
                                {
                                    fechaInicio += fechaParserInicio[i].PadLeft(2, '0') + '/';
                                }
                            }
                        }

                        if (fechaFin.Length < 10)
                        {
                            string[] fechaParserFin = fechaFin.Split('/');
                            if (fechaParserFin.Length < 3)
                                throw new Exception("Formato de fecha incorrecto dd/mm/yyyy");
                            fechaFin = string.Empty;
                            for (int i = 0; i < fechaParserFin.Length; i++)
                            {

                                if (i == 2)
                                {
                                    if (fechaParserFin[i].Length < 4)
                                    {
                                        throw new Exception("Formato de fecha incorrecto dd/mm/yyyy");
                                    }
                                }
                                else
                                {
                                    fechaFin += fechaParserFin[i].PadLeft(2, '0') + '/';
                                }
                            }
                        }
                    }

                    Dictionary<string, DateTime> result = null;

                    switch (idTipoFecha)
                    {
                        case 1:
                            if (fechaInicio.Trim() == string.Empty || fechaFin.Trim() == string.Empty)
                                return null;
                            if (DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", null) >
                                DateTime.ParseExact(fechaFin, "dd/MM/yyyy", null))
                                throw new Exception("Fecha Inicio no puede se mayor a Fecha Fin");
                            result = new Dictionary<string, DateTime>
                            {
                                {"inicio", DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", null)},
                                {"fin", DateTime.ParseExact(fechaFin, "dd/MM/yyyy", null)}
                            };
                            break;
                        case 2:
                            if (fechaInicio.Trim() == string.Empty || fechaFin.Trim() == string.Empty)
                                return null;
                            if (DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", null) >
                                DateTime.ParseExact(fechaFin, "dd/MM/yyyy", null))
                                throw new Exception("Fecha Inicio no puede se mayor a Fecha Fin");
                            result = new Dictionary<string, DateTime>
                            {
                                {"inicio", DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", null)},
                                {"fin", DateTime.ParseExact(fechaFin, "dd/MM/yyyy", null)}
                            };
                            break;
                        case 3:
                            if (fechaInicio.Trim() == string.Empty || fechaFin.Trim() == string.Empty)
                                return null;
                            if (DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", null) >
                                DateTime.ParseExact(fechaFin, "dd/MM/yyyy", null))
                                throw new Exception("Fecha Inicio no puede se mayor a Fecha Fin");
                            result = new Dictionary<string, DateTime>
                            {
                                {"inicio", DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", null)},
                                {"fin", DateTime.ParseExact(fechaFin, "dd/MM/yyyy", null)}
                            };
                            break;
                        case 4:
                            if (fechaInicio.Trim() == string.Empty || fechaFin.Trim() == string.Empty)
                                return null;
                            if (DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", null) >
                                DateTime.ParseExact(fechaFin, "dd/MM/yyyy", null))
                                throw new Exception("Fecha Inicio no puede se mayor a Fecha Fin");
                            result = new Dictionary<string, DateTime>
                            {
                                {"inicio", DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", null)},
                                {"fin", DateTime.ParseExact(fechaFin, "dd/MM/yyyy", null)}
                            };

                            break;
                    }
                    return result;
                }
                catch (FormatException)
                {
                    throw new Exception("Formato de fecha incorrecto dd/mm/yyyy");
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            public static bool ValidaFechas(string fechaInicio, string fechaFin)
            {
                bool result;
                try
                {
                    if (fechaInicio.Trim() != string.Empty || fechaFin.Trim() != string.Empty)
                    {
                        if (fechaInicio.Length < 10)
                        {
                            string[] fechaParserInicio = fechaInicio.Split('/');
                            fechaInicio = string.Empty;
                            for (int i = 0; i < fechaParserInicio.Length; i++)
                            {

                                if (i == 2)
                                {
                                    fechaInicio += fechaParserInicio[i].PadLeft(4, '0');
                                }
                                else
                                {
                                    fechaInicio += fechaParserInicio[i].PadLeft(2, '0') + '/';
                                }
                            }
                        }

                        if (fechaFin.Length < 10)
                        {
                            string[] fechaParserFin = fechaFin.Split('/');
                            fechaFin = string.Empty;
                            for (int i = 0; i < fechaParserFin.Length; i++)
                            {

                                if (i == 2)
                                {
                                    fechaFin += fechaParserFin[i].PadLeft(4, '0');
                                }
                                else
                                {
                                    fechaFin += fechaParserFin[i].PadLeft(2, '0') + '/';
                                }
                            }
                        }
                    }

                    if (DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", null) > DateTime.ParseExact(fechaFin, "dd/MM/yyyy", null))
                        throw new Exception("Fecha Inicio no puede se mayor a Fecha Fin");
                    result = true;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return result;
            }
        }
    }
}