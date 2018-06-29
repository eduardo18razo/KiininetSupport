<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaDetalleFormulario.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Formularios.FrmConsultaDetalleFormulario" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaDetalleFormulario.ascx" TagPrefix="uc1" TagName="UcConsultaDetalleFormulario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaDetalleFormulario runat="server" id="UcConsultaDetalleFormulario" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
