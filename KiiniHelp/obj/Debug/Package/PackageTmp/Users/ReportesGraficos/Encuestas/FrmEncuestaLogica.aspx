<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmEncuestaLogica.aspx.cs" Inherits="KiiniHelp.Users.ReportesGraficos.Encuestas.FrmEncuestaLogica" %>

<%@ Register Src="~/UserControls/ReportesGraficos/Encuestas/UcReporteEncuestasLogica.ascx" TagPrefix="uc1" TagName="UcReporteEncuestasLogica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcReporteEncuestasLogica runat="server" id="UcReporteEncuestasLogica" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
