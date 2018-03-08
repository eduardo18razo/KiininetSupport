<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmNodoConsultas.aspx.cs" Inherits="KiiniHelp.Users.General.FrmNodoConsultas" %>

<%@ Register Src="~/UserControls/Preview/UcVisorConsultainformacion.ascx" TagPrefix="uc1" TagName="UcVisorConsultainformacion" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script>
         function OpenWindow(url) {
             window.open( url , "test", 'type=fullWindow, fullscreen, height=700,width=760');
         }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UcVisorConsultainformacion runat="server" id="ucVisorConsultainformacion" />
</asp:Content>
