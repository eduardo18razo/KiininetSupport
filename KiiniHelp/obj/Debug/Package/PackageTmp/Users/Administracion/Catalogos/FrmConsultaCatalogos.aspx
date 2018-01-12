<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaCatalogos.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Catalogos.FrmConsultaCatalogos" %>
<%@ Register Src="~/UserControls/Consultas/UcConsultaCatalogos.ascx" TagPrefix="uc1" TagName="UcConsultaCatalogos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upConsultas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaCatalogos runat="server" id="ucConsultaCatalogos" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
