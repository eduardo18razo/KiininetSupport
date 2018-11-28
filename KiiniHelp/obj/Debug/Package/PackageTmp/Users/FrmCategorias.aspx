<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmCategorias.aspx.cs" Inherits="KiiniHelp.Users.FrmCategorias" %>

<%@ Register Src="~/UserControls/Seleccion/UcCategoria.ascx" TagPrefix="uc1" TagName="UcCategoria" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UcCategoria runat="server" ID="UcCategoria" />
</asp:Content>
