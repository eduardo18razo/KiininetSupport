<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmFormularios.aspx.cs" Inherits="KiiniHelp.Users.ReportesGraficos.Formularios.FrmFormularios" %>
<%@ Register Src="~/UserControls/ReportesGraficos/Formularios/UcReporteFormularios.ascx" TagPrefix="uc1" TagName="UcReporteFormularios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcReporteFormularios runat="server" id="ucReporteFormularios" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
