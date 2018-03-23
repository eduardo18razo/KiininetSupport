<%@ Page Title="Kiininet" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FrmUserSelect.aspx.cs" Inherits="KiiniHelp.Publico.FrmUserSelect" %>

<%@ Register Src="~/UserControls/Seleccion/UcUserSelect.ascx" TagPrefix="uc1" TagName="UcUserSelect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
   <%-- <link rel="stylesheet" href="../assets/css/styles_movil.css" />
    <link rel="stylesheet" href="../assets/css/menuStyle.css" />--%>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPublic" runat="server">
    <uc1:UcUserSelect runat="server" ID="UcUserSelect" />
</asp:Content>
