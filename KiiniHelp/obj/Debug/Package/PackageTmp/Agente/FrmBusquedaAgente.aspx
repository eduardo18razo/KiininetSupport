<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="FrmBusquedaAgente.aspx.cs" Inherits="KiiniHelp.Agente.FrmBusquedaAgente" %>

<%@ Register Src="~/UserControls/Consultas/UcBusqueda.ascx" TagPrefix="uc1" TagName="UcBusqueda" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPublic" runat="server">
    <uc1:UcBusqueda runat="server" ID="UcBusqueda" />
</asp:Content>
