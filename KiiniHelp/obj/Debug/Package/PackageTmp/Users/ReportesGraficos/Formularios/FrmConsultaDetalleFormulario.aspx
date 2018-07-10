<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaDetalleFormulario.aspx.cs" Inherits="KiiniHelp.Users.ReportesGraficos.Formularios.FrmConsultaDetalleFormulario" %>

<%@ Register Src="~/UserControls/ReportesGraficos/Formularios/UcReporteDetalleFormulario.ascx" TagPrefix="uc1" TagName="UcReporteDetalleFormulario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcReporteDetalleFormulario runat="server" id="UcReporteDetalleFormulario" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
