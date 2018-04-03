<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTest.aspx.cs" ValidateRequest="false" Inherits="KiiniHelp.Test.FrmTest" Culture="Auto" UICulture="Auto" %>
<%@ Register TagPrefix="ctrlExterno" Namespace="Winthusiasm.HtmlEditor" Assembly="Winthusiasm.HtmlEditor" %>
<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=16.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<script type="text/javascript">
        function OnClientEntryAdding(sender, args) {
            var path = args.get_entry().get_fullPath();
            var s = path.split("/");
            if (s.length == 1) {
                args.set_cancel(true);
            }

            else {
                sender.closeDropDown();
            }
        }
    </script>--%>
</head>
<body>
    <div id="full">
        <form id="form1" runat="server" enctype="multipart/form-data">
            <asp:ScriptManager ID="scripMain" runat="server" EnablePageMethods="true"/>
            <ctrlExterno:HtmlEditor runat="Server" ID="txtEditor" />
            <ajax:AsyncFileUpload ID="afuArchivo" runat="server" CssClass="FileUploadClass" UploaderStyle="Traditional" ClientIDMode="AutoID" PersistFile="True" ViewStateMode="Enabled" />
        </form>
    </div>
</body>
</html>
