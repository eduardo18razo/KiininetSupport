<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcTicketPortal.ascx.cs" Inherits="KiiniHelp.UserControls.Temporal.UcTicketPortal" %>
<%@ Register Src="~/UserControls/Altas/Usuarios/UcAltaUsuarioRapida.ascx" TagPrefix="uc1" TagName="UcAltaUsuarioRapida" %>


<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upMascara">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdMascara" />
        <asp:HiddenField runat="server" ID="hfComandoInsertar" />
        <asp:HiddenField runat="server" ID="hfComandoActualizar" />
        <asp:HiddenField runat="server" ID="hfRandom" />

        <div runat="server" id="divControles">
        </div>
        <div>
            <div class="margin-bottom-10">
                <uc1:UcAltaUsuarioRapida runat="server" ID="ucAltaUsuarioRapida" />
            </div>
            <div class="margin-bottom-10 text-center">
                <asp:Button type="button text-left" class="btn btn-primary" runat="server" Text="Crear ticket" ID="btnGuardar" OnClick="btnGuardar_OnClick" />
            </div>
        </div>
        <asp:HiddenField runat="server" ID="hfTicketGenerado" />
        <asp:HiddenField runat="server" ID="hfRandomGenerado" />
    </ContentTemplate>
</asp:UpdatePanel>
