<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmBusqueda.aspx.cs" Inherits="KiiniHelp.Users.FrmBusqueda" %>

<%@ Register Src="~/UserControls/Consultas/UcBusqueda.ascx" TagPrefix="uc1" TagName="UcBusqueda" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UcBusqueda runat="server" ID="UcBusqueda" />
</asp:Content>
