<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcMascaraCaptura.ascx.cs" Inherits="KiiniHelp.UserControls.Temporal.UcMascaraCaptura" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upMascara">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdMascara" />
        <asp:HiddenField runat="server" ID="hfComandoInsertar" />
        <asp:HiddenField runat="server" ID="hfComandoActualizar" />
        <asp:HiddenField runat="server" ID="hfRandom" />

        <div runat="server" id="divControles">
        </div>
        <asp:Button type="button" class="btn btn-primary btn-lg" runat="server" Text="Crear ticket" ID="btnGuardar" OnClick="btnGuardar_OnClick"/>
        <asp:HiddenField runat="server" ID="hfTicketGenerado"/>  
        <asp:HiddenField runat="server" ID="hfRandomGenerado"/>  
    </ContentTemplate>
</asp:UpdatePanel>
