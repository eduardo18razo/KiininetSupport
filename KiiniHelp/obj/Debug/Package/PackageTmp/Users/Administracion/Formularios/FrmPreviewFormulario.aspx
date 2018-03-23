<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmPreviewFormulario.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Formularios.FrmPreviewFormulario1" %>

<%@ Register Src="~/UserControls/Altas/Formularios/UcPreviewFormulario.ascx" TagPrefix="uc1" TagName="UcPreviewFormulario" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UcPreviewFormulario runat="server" id="ucPreviewFormulario" />
</asp:Content>
