﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmParametrosGenerales.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Configuracion.FrmParametrosGenerales" %>

<%@ Register Src="~/UserControls/Configuracion/UcParametrosGenerales.ascx" TagPrefix="uc1" TagName="UcParametrosGenerales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upConsultas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcParametrosGenerales runat="server" ID="UcParametrosGenerales" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>