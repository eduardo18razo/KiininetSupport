<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmMapaNavegacion.aspx.cs" Inherits="KiiniHelp.Users.Administracion.MapaNavegacion.FrmMapaNavegacion" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaMapa.ascx" TagPrefix="uc1" TagName="UcConsultaMapa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaMapa runat="server" id="UcConsultaMapa" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
