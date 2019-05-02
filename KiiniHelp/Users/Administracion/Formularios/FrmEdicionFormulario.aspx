<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmEdicionFormulario.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Formularios.FrmEdicionFormulario" %>

<%@ Register Src="~/UserControls/Altas/Formularios/UcAltaFormulario.ascx" TagPrefix="uc1" TagName="UcAltaFormulario" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li>Centro de Soporte</li>
                <li>Formularios</li>
                <li class="active">Nuevo Formulario</li>
            </ol>

            <uc1:UcAltaFormulario runat="server" ID="ucAltaFormulario" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
