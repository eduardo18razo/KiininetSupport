<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmConfirmacionCorreo.aspx.cs" Inherits="KiiniHelp.FrmConfirmacionCorreo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />

    <title>Kiininet CXP</title>
    <link rel="Shortcut Icon" href="favicon.ico" type="image/x-icon" />

    <link rel='stylesheet' href="assets/css/font.css" />
    <link rel="stylesheet" href="assets/css/font-awesome.css" />
    <link rel="stylesheet" href="assets/css/pe-7-icons.css" />

    <link rel="stylesheet" href="assets/css/bootstrap.css" />
    <link rel="stylesheet" href="assets/css/styles.css" />
    <link rel="stylesheet" href="assets/css/medias.css" />
    <link rel="stylesheet" href="assets/css/menuStyle.css" />
    <link rel="stylesheet" href="assets/css/bootstrap-markdown.css" />
</head>
<body class="layout_no_leftnav">
    <form runat="server">
        <asp:ScriptManager ID="scripMain" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdateProgress ID="updateProgress" runat="server" ClientIDMode="Static">
            <ProgressTemplate>
                <div class="progressBar">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loading.gif" AlternateText="Loading ..." ToolTip="Loading ..." />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <header class="header">
            <div class="branding">
                <h1 class="logo text-center">
                    <asp:HyperLink NavigateUrl="~/Default.aspx" runat="server">
                        <asp:Image CssClass="logo-icon" ImageUrl="~/assets/images/logo.png" alt="icon" runat="server" />
                        <asp:Label runat="server" ID="lblBranding" Visible="false" />
                    </asp:HyperLink>
                </h1>
            </div>

            <div class="topbar bg_w_header">

                <div class="navbar-tools">
                </div>
            </div>
        </header>


        <div class="row">
            <div id="content-wrapper" class="content-wrapper">
                <div class="container-fluid">
                    <div id="masonry" class="row">
                        <div class=" col-lg-12 col-md-12 col-sm-12 col-xs-12" runat="server" id="divClientes">
                            <section class="module ">
                                <div class="module-inner">
                                    <div class="module-content collapse in" id="content-3">
                                        <div class="module-content-inner">
                                            <h1 class="subtitle text-center"><asp:Label runat="server" ID="lblMsg"></asp:Label> </h1>
                                            <asp:LinkButton runat="server" ID="lnkBtnContinuar" CssClass="text-theme" OnClick="lnkBtnContinuar_OnClick">
                                            <h4 class="text-center">Continuar</h4>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <footer id="footer" class="site-footer">
                <div class="copyright">&copy; Todos los derechos reservados </div>
                <div class="politicas-Privacidad">
                    <asp:HyperLink runat="server" NavigateUrl="~/PoliticasPrivacidad.aspx">Aviso de Privacidad</asp:HyperLink>
                    <asp:Label runat="server" Text="  |  " Visible="False"></asp:Label>
                    <asp:HyperLink runat="server" NavigateUrl="~/TerminosyCondiciones.aspx" Visible="False"> Términos y Condiciones</asp:HyperLink>
                </div>
            </footer>
        </div>
    </form>
</body>
</html>
