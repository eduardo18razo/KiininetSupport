using System;

namespace KiiniHelp.UserControls.Altas.Tickets
{
    public partial class UcTicketAgente : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //TODO: Se elimina para bloque de boton al click
            //btnLevantar.OnClientClick = "this.disabled = document.getElementById('form1').checkValidity(); if(document.getElementById('form1').checkValidity()){ " + Page.ClientScript.GetPostBackEventReference(btnLevantar, null) + ";}";  
        }
    }
}