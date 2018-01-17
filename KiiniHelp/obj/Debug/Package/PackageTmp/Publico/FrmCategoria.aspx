<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="FrmCategoria.aspx.cs" Inherits="KiiniHelp.Publico.FrmCategoria" %>

<%@ Register Src="~/UserControls/Seleccion/UcCategoria.ascx" TagPrefix="uc1" TagName="UcCategoria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPublic" runat="server">
    <uc1:UcCategoria runat="server" id="UcCategoria" />  
</asp:Content>
