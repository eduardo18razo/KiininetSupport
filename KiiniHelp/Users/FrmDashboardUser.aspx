<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmDashboardUser.aspx.cs" Inherits="KiiniHelp.Users.FrmDashboardUser" %>

<%@ Register Src="~/UserControls/Seleccion/UcUserSelect.ascx" TagPrefix="uc1" TagName="UcUserSelect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UcUserSelect runat="server" ID="UcUserSelect" />
</asp:Content>
