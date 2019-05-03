<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmEncuestaSatisfaccion.aspx.cs" Inherits="KiiniHelp.Users.ReportesGraficos.Encuestas.FrmEncuestaSatisfaccion" %>

<%@ Register Src="~/UserControls/ReportesGraficos/Encuestas/UcReporteEncuestasSatisfaccion.ascx" TagPrefix="uc1" TagName="UcReporteEncuestasSatisfaccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcReporteEncuestasSatisfaccion runat="server" id="UcReporteEncuestasSatisfaccion" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
