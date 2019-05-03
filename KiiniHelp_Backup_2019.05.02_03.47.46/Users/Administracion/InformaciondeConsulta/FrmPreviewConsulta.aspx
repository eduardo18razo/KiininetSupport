<%@ Page Title="Preview Consulta" Language="C#"MasterPageFile="~/Usuarios.Master"  AutoEventWireup="true" CodeBehind="FrmPreviewConsulta.aspx.cs" ValidateRequest="false" Inherits="KiiniHelp.Users.Administracion.InformaciondeConsulta.FrmPreviewConsulta" %>

<%@ Register Src="~/UserControls/Preview/UcPreviewConsulta.ascx" TagPrefix="uc" TagName="UcPreviewConsulta" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc:UcPreviewConsulta runat="server" id="UcPreviewConsulta" />
</asp:Content>


