<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="FrmServiceArea.aspx.cs" Inherits="KiiniHelp.Publico.FrmServiceArea" %>

<%@ Register Src="~/UserControls/Seleccion/UcServiceArea.ascx" TagPrefix="uc1" TagName="UcServiceArea" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPublic" runat="server">
   
     <%--<uc1:UcServiceArea runat="server" id="UcServiceArea" />--%>

     <div style="width: 100%; height:100%; padding-top:120px;" class="text-center">
        <img src="../assets/images/construccion.jpg" alt="En construcción"/>
    </div>

</asp:Content>
