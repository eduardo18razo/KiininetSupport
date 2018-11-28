<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmReportes.aspx.cs" Inherits="KiiniHelp.Users.FrmReportes" %>

<%@ Register Src="~/UserControls/Seleccion/UcReportSelect.ascx" TagPrefix="uc1" TagName="UcReportSelect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UcReportSelect runat="server" id="UcReportSelect" />
</asp:Content>
