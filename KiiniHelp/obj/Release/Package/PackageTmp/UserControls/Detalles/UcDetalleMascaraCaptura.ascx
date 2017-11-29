<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcDetalleMascaraCaptura.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcDetalleMascaraCaptura" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upMascara">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdMascara"/>
        <asp:HiddenField runat="server" ID="hfIdTicket"/>
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label runat="server" ID="lblDescripcionMascara"></asp:Label>
            </div>
            <div class="panel-body">
                <div runat="server" id="divControles" class="form-horizontal">
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
