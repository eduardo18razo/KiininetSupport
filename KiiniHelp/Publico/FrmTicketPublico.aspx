<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="FrmTicketPublico.aspx.cs" Inherits="KiiniHelp.Publico.FrmTicketPublico" %>

<%@ Register Src="~/UserControls/Altas/Formularios/UcFormulario.ascx" TagPrefix="uc1" TagName="UcFormulario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPublic" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfIdMascara" />
            <asp:HiddenField runat="server" ID="hfIdConsulta" />
            <asp:HiddenField runat="server" ID="hfIdEncuesta" />
            <asp:HiddenField runat="server" ID="hfIdSla" />
            <asp:HiddenField runat="server" ID="hfIdCanal" />
            <asp:HiddenField runat="server" ID="hfIdUsuarioSolicita" />
            <asp:Label runat="server" ID="lblTicketDescripcion" />

            <uc1:UcFormulario runat="server" ID="ucFormulario" />
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
