<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmGeoChart.aspx.cs" Inherits="KiiniHelp.Users.Graficos.FrmGeoChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script type='text/javascript' src='https://www.gstatic.com/charts/loader.js'></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        function getValue(valor) {
            var hf = parent.document.getElementById("ContentPlaceHolder1_hfRegion");
            hf.value = valor;
            parent.document.getElementById("ContentPlaceHolder1_btnDetalleGeografico").click();
            document.getElementById('<%=btnSelectRegion.ClientID %>').click();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        
        <div id="chart_container">
        </div>
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        <asp:Button runat="server" ID="btnSelectRegion" OnClick="btnSelectRegion_OnClick" Visible="False" />
    </form>
</body>
</html>
