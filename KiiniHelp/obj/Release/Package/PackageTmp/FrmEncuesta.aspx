<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="FrmEncuesta.aspx.cs" Inherits="KiiniHelp.FrmEncuesta" %>

<%@ Register Src="~/UserControls/Temporal/UcEncuestaCaptura.ascx" TagPrefix="uc1" TagName="UcEncuestaCaptura" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>KiiniNet</title>
    <link href="BootStrap/css/bootstrap.css" rel="stylesheet" />
    <link href="BootStrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="BootStrap/css/CheckBoxStyle.css" rel="stylesheet" />
    <link href="BootStrap/css/Calendar.css" rel="stylesheet" />
    <link href="BootStrap/css/DropDown.css" rel="stylesheet" />
    <link href="BootStrap/css/divs.css" rel="stylesheet" />
    <link href="BootStrap/css/FileInput.css" rel="stylesheet" />
    <link href="BootStrap/css/Headers.css" rel="stylesheet" />
    <link href="BootStrap/css/stylemainmenu.css" rel="stylesheet" />
    <script>
        function CerrarVentana() {
            debugger;
            window.close();
        }
    </script>
</head>
<body style="background: white;">
    <div id="full">
        <form id="form1" runat="server">
            <asp:ScriptManager ID="scripMain" runat="server" EnablePageMethods="true">
                <Scripts>
                    <asp:ScriptReference Path="~/BootStrap/js/jquery-2.1.1.min.js" />
                    <asp:ScriptReference Path="~/BootStrap/js/bootstrap.min.js" />
                    <asp:ScriptReference Path="~/BootStrap/js/super-panel.js" />
                </Scripts>
            </asp:ScriptManager>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="hfIdTicket" />
                    <asp:HiddenField runat="server" ID="hfIdTipoServicio" />
                    <asp:HiddenField runat="server" ID="hfIdEncuesta" />
                    <header>
                        <div class="alert alert-danger" id="panelAlert" runat="server" visible="False">
                            <div>
                                <div style="float: left">
                                    <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                                </div>
                                <div style="float: left">
                                    <h3>Error</h3>
                                </div>
                                <div class="clearfix clear-fix" />
                            </div>
                            <hr />
                            <asp:Repeater runat="server" ID="rptHeaderError">
                                <ItemTemplate>
                                    <%# Container.DataItem %>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </header>
                    <div class="panel panel-primary">
                        <div class="panel-body">
                            <uc1:UcEncuestaCaptura runat="server" ID="ucEncuestaCaptura" />
                        </div>
                        <div class="panel-footer">
                            <%--<asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_OnClick" Text="Guardar" CssClass="btn btn-lg btn-success" />
                    <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" CssClass="btn btn-lg btn-danger" />--%>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>
