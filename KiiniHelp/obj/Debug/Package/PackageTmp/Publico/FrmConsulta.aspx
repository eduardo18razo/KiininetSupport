<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="FrmConsulta.aspx.cs" Inherits="KiiniHelp.Publico.FrmConsulta" %>
<%@ Register Src="~/UserControls/Preview/UcVisorConsultainformacion.ascx" TagPrefix="uc1" TagName="UcVisorConsultainformacion" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function OpenWindow(url) {
            window.open(url, "test", 'type=fullWindow, fullscreen, height=700,width=760');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPublic" runat="server">
    <h3 class="h6">
        <asp:HyperLink runat="server" NavigateUrl="~/Default.aspx">Home</asp:HyperLink>
        </h3>
    <hr />
    <uc1:UcVisorConsultainformacion runat="server" ID="ucVisorConsultainformacion" />
</asp:Content>
