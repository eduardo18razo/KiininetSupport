<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmEdicionFormulario.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Formularios.FrmEdicionFormulario" %>

<%@ Register Src="~/UserControls/Altas/Formularios/UcAltaFormulario.ascx" TagPrefix="uc1" TagName="UcAltaFormulario" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <h3 class="h6">
                <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink>
                / Editor de contenido / Nuevo artículo </h3>
            <hr />
            <uc1:UcAltaFormulario runat="server" id="ucAltaFormulario" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
