<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaFeriados.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Feriados.FrmConsultaFeriados" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaDiasFestivos.ascx" TagPrefix="uc1" TagName="UcConsultaDiasFestivos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaDiasFestivos runat="server" id="ucConsultaDiasFestivos" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>