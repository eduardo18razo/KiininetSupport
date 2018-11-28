<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaNotasOpcion.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Notas.FrmConsultaNotasOpcion" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaNotasOpcion.ascx" TagPrefix="uc1" TagName="UcConsultaNotasOpcion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaNotasOpcion runat="server" id="ucConsultaNotasOpcion" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
