<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTestTelerik.aspx.cs" Inherits="KiiniHelp.Test.FrmTestTelerik" %>

<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/UserControls/Consultas/UcBusqueda.ascx" TagPrefix="uc1" TagName="UcBusqueda" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <uc1:UcBusqueda runat="server" id="UcBusqueda" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--<tc:RadEditor runat="server" ID="reContent" RenderMode="Lightweight" EditModes="Design" EnableResize="False">
                    <Tools>
                        <tc:EditorToolGroup>
                            <tc:EditorTool Name="Cut"></tc:EditorTool>
                            <tc:EditorTool Name="Copy"></tc:EditorTool>
                            <tc:EditorTool Name="Paste" Text="Pegar"></tc:EditorTool>
                            <tc:EditorTool Name="LinkManager" shortcut="CTRL+K"/>
                        </tc:EditorToolGroup>
                    </Tools>
                    <Modules>
                        <tc:EditorModule Name="RadEditorStatistics" Enabled="false"></tc:EditorModule>
                    </Modules>
                </tc:RadEditor>
                <asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_OnClick" Text="Enviar" />--%>
        <%--<div>
            <tc:RadDropDownTree runat="server" ID="ddlUsuarioAsignacion" AutoPostBack="True" RenderMode="Classic"
                DefaultMessage="-" >
                <DropDownSettings Width="350px" />
            </tc:RadDropDownTree>
            <tc:RadDropDownTree RenderMode="Lightweight" ID="RadDropDownTree1" runat="server" Width="350px" DefaultMessage="Select an entry from the list">
                <DropDownSettings Width="350px" />
            </tc:RadDropDownTree>
        </div>--%>
    </form>
</body>
</html>
