<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaCatalogo.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Catalogos.FrmConsultaCatalogo" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaCatalogo.ascx" TagPrefix="uc1" TagName="UcConsultaCatalogo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upConsultas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaCatalogo runat="server" id="ucConsultaCatalogo" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
