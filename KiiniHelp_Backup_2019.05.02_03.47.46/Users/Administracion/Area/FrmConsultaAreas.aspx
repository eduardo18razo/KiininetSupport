<%@ Page Title="Areas" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaAreas.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Area.FrmConsultaAreas" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaAreas.ascx" TagPrefix="uc1" TagName="UcConsultaAreas" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaAreas runat="server" id="UcConsultaAreas" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>