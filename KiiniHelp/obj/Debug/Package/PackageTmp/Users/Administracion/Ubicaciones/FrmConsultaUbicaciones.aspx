<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaUbicaciones.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Ubicaciones.FrmConsultaUbicaciones" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaUbicaciones.ascx" TagPrefix="uc1" TagName="UcConsultaUbicaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaUbicaciones runat="server" ID="UcConsultaUbicaciones" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
