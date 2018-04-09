<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaEncuestas.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Encuestas.FrmConsultaEncuestas" %>

<%@ Register Src="~/UserControls/Consultas/UcConsultaEncuesta.ascx" TagPrefix="uc1" TagName="UcConsultaEncuesta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:UcConsultaEncuesta runat="server" id="UcConsultaEncuesta" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
