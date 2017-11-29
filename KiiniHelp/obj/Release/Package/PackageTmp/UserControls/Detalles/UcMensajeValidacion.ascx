<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcMensajeValidacion.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcMensajeValidacion" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleUsuario.ascx" TagPrefix="uc1" TagName="UcDetalleUsuario" %>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3><asp:Label runat="server" ID="lblMensaje"/></h3>
            </div>
            <div class="panel-body">
                <uc1:UcDetalleUsuario runat="server" id="ucDetalleUsuario" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
