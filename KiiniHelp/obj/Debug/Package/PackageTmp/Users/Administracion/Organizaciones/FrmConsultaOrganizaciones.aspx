<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaOrganizaciones.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Organizaciones.FrmConsultaOrganizaciones" %>

<%@ MasterType VirtualPath="~/Usuarios.master" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaOrganizacion.ascx" TagPrefix="uc1" TagName="UcConsultaOrganizacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaOrganizacion runat="server" ID="UcConsultaOrganizacion" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
