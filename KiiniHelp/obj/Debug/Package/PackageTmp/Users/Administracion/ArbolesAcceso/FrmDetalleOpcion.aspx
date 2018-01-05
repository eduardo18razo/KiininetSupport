<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmDetalleOpcion.aspx.cs" Inherits="KiiniHelp.Users.Administracion.ArbolesAcceso.FrmDetalleOpcion" %>

<%@ Register Src="~/UserControls/Detalles/UcDetalleArbolAcceso.ascx" TagPrefix="uc1" TagName="UcDetalleArbolAcceso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UcDetalleArbolAcceso runat="server" ID="ucDetalleArbolAcceso" />
</asp:Content>
