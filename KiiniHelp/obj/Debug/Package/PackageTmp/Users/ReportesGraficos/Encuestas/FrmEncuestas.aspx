<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmEncuestas.aspx.cs" Inherits="KiiniHelp.Users.ReportesGraficos.Encuestas.FrmEncuestas" %>

<%@ Register Src="~/UserControls/ReportesGraficos/Encuestas/UcReporteEncuestas.ascx" TagPrefix="uc1" TagName="UcReporteEncuestas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcReporteEncuestas runat="server" ID="UcReporteEncuestas" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
