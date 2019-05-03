<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmCambiarContrasena.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Usuarios.FrmCambiarContrasena" %>

<%@ Register Src="~/UserControls/Operacion/UcCambiarContrasena.ascx" TagPrefix="uc1" TagName="UcCambiarContrasena" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UcCambiarContrasena runat="server" id="ucCambiarContrasena" />
</asp:Content>
