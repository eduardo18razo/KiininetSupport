<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmEncuestaCalificacion.aspx.cs" Inherits="KiiniHelp.Users.ReportesGraficos.Encuestas.FrmEncuestaCalificacion" %>

<%@ Register Src="~/UserControls/ReportesGraficos/Encuestas/UcReporteEncuestasCalificacion.ascx" TagPrefix="uc1" TagName="UcReporteEncuestasCalificacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcReporteEncuestasCalificacion runat="server" id="UcReporteEncuestasCalificacion" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
