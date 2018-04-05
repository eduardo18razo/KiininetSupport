<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaFormularios.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Formularios.FrmConsultaFormularios" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaFormularios.ascx" TagPrefix="uc1" TagName="UcConsultaFormularios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaFormularios runat="server" id="UcConsultaFormularios" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>