<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmGraficoInformacionConsulta.aspx.cs" Inherits="KiiniHelp.Users.Graficos.FrmGraficoInformacionConsulta" %>

<%@ Register Src="~/UserControls/Consultas/UcGraficoInformacionConsulta.ascx" TagPrefix="uc1" TagName="UcGraficoInformacionConsulta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcGraficoInformacionConsulta runat="server" id="UcGraficoInformacionConsulta" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
