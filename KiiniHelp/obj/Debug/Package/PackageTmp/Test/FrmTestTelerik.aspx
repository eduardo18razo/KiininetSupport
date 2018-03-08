<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTestTelerik.aspx.cs" Inherits="KiiniHelp.Test.FrmTestTelerik" %>

<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <div>
            <tc:RadDropDownTree runat="server" ID="ddlUsuarioAsignacion" AutoPostBack="True" RenderMode="Classic"
                DefaultMessage="-" >
                <DropDownSettings Width="350px" />
            </tc:RadDropDownTree>
            <tc:RadDropDownTree RenderMode="Lightweight" ID="RadDropDownTree1" runat="server" Width="350px" DefaultMessage="Select an entry from the list">
                <DropDownSettings Width="350px" />
            </tc:RadDropDownTree>
        </div>
    </form>
</body>
</html>
