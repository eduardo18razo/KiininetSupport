<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmPreview.aspx.cs" Inherits="KiiniHelp.Preview.FrmPreview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <iframe runat="server" id="iPage" style="height: 96vh; width: 100%; border: none; background: transparent;"></iframe>
        </div>
    </form>
</body>
</html>
