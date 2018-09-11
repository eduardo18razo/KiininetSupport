<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmGraficoCalificacion.aspx.cs" Inherits="KiiniHelp.Users.ReportesGraficos.Encuestas.FrmGraficoCalificacion" %>

<%@ Register Src="~/UserControls/ReportesGraficos/Encuestas/UcGraficoCalificaciones.ascx" TagPrefix="uc1" TagName="UcGraficoCalificaciones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcGraficoCalificaciones runat="server" ID="UcGraficoCalificaciones" />
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>
