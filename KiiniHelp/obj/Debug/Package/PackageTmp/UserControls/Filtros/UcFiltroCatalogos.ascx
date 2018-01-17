<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroCatalogos.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.UcFiltroCatalogos" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upMascara">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdCatalogo"/>
        <asp:HiddenField runat="server" ID="hfComandoInsertar"/>
        <asp:HiddenField runat="server" ID="hfComandoActualizar"/>
        <asp:HiddenField runat="server" ID="hfRandom"/>
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label runat="server" ID="lblDescCatalogo"></asp:Label>
            </div>
            <div class="panel-body">
                <div runat="server" id="divControles">
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>