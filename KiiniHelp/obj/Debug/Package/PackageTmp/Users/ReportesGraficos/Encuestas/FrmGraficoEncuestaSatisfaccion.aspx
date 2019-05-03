<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmGraficoEncuestaSatisfaccion.aspx.cs" Inherits="KiiniHelp.Users.ReportesGraficos.Encuestas.FrmGraficoEncuestaSatisfaccion" %>

<%@ Register Src="~/UserControls/ReportesGraficos/Encuestas/UcGraficoSatisfaccion.ascx" TagPrefix="uc1" TagName="UcGraficoSatisfaccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcGraficoSatisfaccion runat="server" id="UcGraficoSatisfaccion" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
