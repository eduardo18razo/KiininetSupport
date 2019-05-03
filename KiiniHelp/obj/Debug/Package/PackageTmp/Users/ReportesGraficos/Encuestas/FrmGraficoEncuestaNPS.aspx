<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmGraficoEncuestaNPS.aspx.cs" Inherits="KiiniHelp.Users.ReportesGraficos.Encuestas.FrmGraficoEncuestaNPS" %>

<%@ Register Src="~/UserControls/ReportesGraficos/Encuestas/UcGraficoNPS.ascx" TagPrefix="uc1" TagName="UcGraficoNPS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcGraficoNPS runat="server" ID="UcGraficoNPS" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
