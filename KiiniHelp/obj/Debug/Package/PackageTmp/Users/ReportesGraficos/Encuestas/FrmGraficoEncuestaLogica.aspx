<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmGraficoEncuestaLogica.aspx.cs" Inherits="KiiniHelp.Users.ReportesGraficos.Encuestas.FrmGraficoEncuestaLogica" %>

<%@ Register Src="~/UserControls/ReportesGraficos/Encuestas/UcGraficoLogica.ascx" TagPrefix="uc1" TagName="UcGraficoLogica" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcGraficoLogica runat="server" id="UcGraficoLogica" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
