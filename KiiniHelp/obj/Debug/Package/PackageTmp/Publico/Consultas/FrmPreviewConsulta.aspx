<%@ Page Title="Preview Consulta" Language="C#"MasterPageFile="~/Public.Master"  AutoEventWireup="true" CodeBehind="FrmPreviewConsulta.aspx.cs" ValidateRequest="false" Inherits="KiiniHelp.Publico.Consultas.FrmPreviewConsulta" %>

<%@ Register Src="~/UserControls/Preview/UcPreviewConsulta.ascx" TagPrefix="uc" TagName="UcPreviewConsulta" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPublic" runat="server">
    <uc:UcPreviewConsulta runat="server" id="UcPreviewConsulta" />
</asp:Content>


