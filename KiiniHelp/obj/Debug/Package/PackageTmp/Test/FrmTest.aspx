<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTest.aspx.cs" ValidateRequest="false" Inherits="KiiniHelp.Test.FrmTest" Culture="Auto" UICulture="Auto" %>

<%@ Register TagPrefix="ajx" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=18.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />

    <title>Test</title>
</head>
<body>
    <div>
        <form id="form1" runat="server" enctype="multipart/form-data">
            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="uppanel">
                <ContentTemplate>
                </ContentTemplate>
            </asp:UpdatePanel>

        </form>
    </div>
</body>
</html>
