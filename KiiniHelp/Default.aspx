﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KiiniHelp.Default1" %>

<%@ Import Namespace="System.IO" %>

<%@ Register Src="~/UserControls/UcLogCopia.ascx" TagPrefix="uc1" TagName="UcLogCopia" %>
<%@ Register Src="~/UserControls/Temporal/UcTicketPortal.ascx" TagPrefix="uc1" TagName="UcTicketPortal" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
	<meta http-equiv="Pragma" content="no-cache" />
	<meta http-equiv="Expires" content="0" />
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
    <link rel="stylesheet" href="assets/css/fileupload.css" />
    <script src="assets/js/functions.js"></script>
    <script type="text/javascript">
        window.addEventListener("pageshow", function (evt) {
            if (evt.persisted) {
                setTimeout(function () {
                    window.location.reload();
                }, 10);
            }
        }, false);
    </script>
</head>
<body class="layout_no_leftnav">
    <form runat="server">
        <asp:ScriptManager ID="scripMain" runat="server" EnablePageMethods="true">
            <Scripts>
                <asp:ScriptReference Path="~/assets/js/jquery.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap-markdown.js" />
                <asp:ScriptReference Path="assets/js/imagesloaded.js" />
                <asp:ScriptReference Path="assets/js/masonry.js" />
                <asp:ScriptReference Path="assets/js/main.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap-notify.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap-notify.min.js" />
                <asp:ScriptReference Path="~/assets/js/Notificaciones.js" />
                <asp:ScriptReference Path="~/assets/js/validation.js" />
            </Scripts>
        </asp:ScriptManager>
        <asp:UpdateProgress ID="updateProgress" runat="server" ClientIDMode="Static">
            <ProgressTemplate>
                <div class="progressBar">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loading.gif" AlternateText="Loading ..." ToolTip="Loading ..." />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <header class="header">
            <div class="branding" >
                <h1 class="logo text-center" style="padding: 5px">
                    <asp:HyperLink NavigateUrl="~/Default.aspx" runat="server" style="height: 60px; width: 60px">
                        <asp:Image CssClass="logo-icon" ImageUrl="~/assets/images/IconoPage.png" alt="icon" runat="server" style="height: 60px; width: 60px"/>
                        <asp:Label runat="server" ID="lblBranding" Visible="false" />
                    </asp:HyperLink>
                </h1>
            </div>

            <div class="topbar bg_w_header">
                <div class="search-container margin-left-15" runat="server" visible="true">
                    <div id="main-search">
                        <i id="main-search-toggle" class="fa fa-search icon"></i>
                    </div>
                </div>

                <div class="navbar-tools">

                    <div class="utilities-container">
                        <div class="utilities">
                            <div class="item item-notifications">
                                <div class="dropdown-toggle" data-toggle="dropdown" aria-expanded="true" role="button">
                                    <span class="sr-only">Tickets</span>
                                    <span class="pe-icon fa fa-ticket icon" data-toggle="tooltip" data-placement="bottom" title="Tickets"></span>
                                </div>
                                <ul class="dropdown-menu wdropdown-ticket" role="menu" aria-labelledby="dropdownMenu-user">
                                    <li>
                                        <span class="arrow arrowTicket"></span>
                                        <a role="menuitem" data-toggle="modal" data-backdrop="static" data-keyboard="false" data-target="#modal-new-ticket">
                                            <span class="pe-icon pe-7s-plus icon"></span>Nuevo ticket
                                        </a>
                                    </li>
                                    <li>
                                        <asp:LinkButton role="menuitem" runat="server" ID="btnconsultarTicket" OnClick="btnconsultarTicket_OnClick">
                                            <span class="pe-icon pe-7s-look icon"></span>Consultar ticket
                                        </asp:LinkButton>
                                    </li>
                                </ul>
                            </div>

                            <div class="item item-messages dropdown">
                                <div class="dropdown-toggle" id="dropdownMenu-messages" data-toggle="dropdown" aria-expanded="true" role="button">
                                    <span class="sr-only">Ingresa</span>
                                    <span class="pe-icon fa fa-sign-in icon" data-toggle="tooltip" data-placement="bottom" title="Ingresa"></span>
                                </div>
                                <div class="dropdown-menu wdropdown-login" role="menu" aria-labelledby="dropdownMenu-messages">
                                    <span class="arrow arrowLogin"></span>
                                    <div>
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <uc1:UcLogCopia runat="server" ID="UcLogCopia" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div id="main_search_input_wrapper" class="main_search_input_wrapper">
                    <div class="row">
                        <div class="col-lg-9 col-md-9 col-sm-9 col-xs-8">
                            <asp:TextBox ID="main_search_input" ClientIDMode="Static" CssClass="main_search_input form-control" onkeypress="search(event)" placeholder="Buscar por palabra clave..." runat="server" />
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3 col-xs-4">
                            <span id="clear-search" aria-hidden="true" class="clear-search">Cancelar</span>
                        </div>
                    </div>
                </div>

            </div>
        </header>


        <div class="row">
            <div id="content-wrapper" class="content-wrapper">
                <div class="container-fluid">
                    <div class="row margin-top-100">
                        <div class="module-content collapse in centra-carrusel" id="content-1">
                            <div class="module-content-inner no-padding-bottom no-padding-top">
                                <div class="carousel z-Index-9" data-ride="carousel" id="carouselPresenter">
                                    <%
                                        string[] arch = Directory.GetFiles(ConfigurationManager.AppSettings["RepositorioCarousel"], "*.jpg", SearchOption.TopDirectoryOnly);
                                        int nNumOfDrives = arch.Length;

                                    %>
                                    <ol class="carousel-indicators">
                                        <% 
                                            for (int i = 0; i < nNumOfDrives; i++)
                                            {
                                                if (i == 0)
                                                    Response.Write(string.Format("<li data-target='#carouselPresenter' data-slide-to='{0}' class='active'></li>", i));
                                                else
                                                    Response.Write(string.Format("<li data-target='#carouselPresenter' data-slide-to='{0}'></li>", i));
                                            }
                                        %>
                                    </ol>
                                    <div class="carousel-inner">
                                        <%
                                            for (int image = 0; image < nNumOfDrives; image++)
                                            {
                                                FileInfo info = new FileInfo(arch[image]);
                                                if (image == 0)
                                                    Response.Write(string.Format("<div class='item active text-center' style='max-height:454px; max-width: 1200px'>\n" +
                                                                                    "<img src='assets/carouselImage/{0}' alt='' height='454' width='1200'/>\n" +
                                                                                "</div>\n", info.Name));
                                                else
                                                    Response.Write(string.Format("<div class='item text-center' style='max-height:454px; max-width: 1200px'>\n" +
                                                                                    "<img src='assets/carouselImage/{0}' alt='' height='454' width='1200'/>\n" +
                                                                                "</div>\n", info.Name));
                                            }
                                        %>
                                    </div>
                                    <a class="left carousel-control z-Index-9" data-slide="prev" href="#carouselPresenter">
                                        <span class="glyphicon glyphicon-chevron-left"></span><span class="sr-only">Previous</span>
                                    </a><a class="right carousel-control" data-slide="next" href="#carouselPresenter"><span class="glyphicon glyphicon-chevron-right"></span><span class="sr-only">Next</span></a>
                                </div>
                            </div>
                        </div>
                    </div>

                    <h1 class="subtitle text-center">Para ofrecerte un mejor servicio indicanos que tipo de usuario eres.</h1>

                    <div id="masonry" class="row">
                        <div class=" col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" id="divEmpleados">
                            <section class="module ">
                                <div class="module-inner">
                                    <div class="module-content collapse in" id="content-2">
                                        <div class="module-content-inner">
                                            <asp:LinkButton runat="server" ID="lnkBtnEmpleado" CssClass="text-theme" OnClick="lnkBtnEmpleado_OnClick">
                                            <img class="img-responsive margin-auto" src="assets/images/users_icon/usuario_2.jpg" alt="Iconos de usuarios" />
                                            <h4 class="text-center">EMPLEADO</h4>
                                            <%--<h6 class="text-center " runat="server">Click para continuar</h6> --%>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </div>
                        <div class=" col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" id="divClientes">
                            <section class="module ">
                                <div class="module-inner">
                                    <div class="module-content collapse in" id="content-3">
                                        <div class="module-content-inner">
                                            <asp:LinkButton runat="server" ID="lnkBtnCliente" CssClass="text-theme" OnClick="lnkBtnCliente_OnClick">
                                            <img class="img-responsive  margin-auto" src="assets/images/users_icon/usuario_1.jpg" alt="Iconos de usuarios" />
                                            <h4 class="text-center">CLIENTE</h4>
                                            <%--<h6 class="text-center " runat="server">Click para continuar</h6> --%>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-6 col-xs-12" runat="server" id="divProveedores">
                            <section class="module">
                                <div class="module-inner">
                                    <div class="module-content collapse in" id="content-4">
                                        <div class="module-content-inner">
                                            <asp:LinkButton runat="server" ID="lnkBtnProveedor" CssClass="text-theme" OnClick="lnkBtnProveedor_OnClick">
                                            <img class="img-responsive  margin-auto" src="assets/images/users_icon/usuario_3.jpg" alt="Iconos de usuarios" />
                                            <h4 class="text-center">PROVEEDOR</h4>
                                            <%--<h6 class="text-center " runat="server">Click para continuar</h6> --%>
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
        <div class="modal fade" id="modal-new-ticket" tabindex="-1" role="dialog" aria-labelledby="modal-new-ticket-label">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="modal-header">
                                <asp:LinkButton CssClass="close" runat="server" ID="btnCerrarTicket" OnClick="btnCerrarTicket_OnClick" Text='&times' />
                                <h6 id="modal-new-ticket-label" class="modal-title">Crear Ticket Nuevo</h6>
                            </div>
                            <div class="modal-body modalhidebarhorizontal">
                                <uc1:UcTicketPortal runat="server" ID="ucTicketPortal" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalExitoTicket" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:LinkButton class="close" runat="server" ID="btnCerrarExito" OnClick="btnCerrarExito_OnClick"><span aria-hidden="true">&times;</span></asp:LinkButton>
                        <h6 class="modal-title" id="myModalLabel">Ticket creado con éxito</h6>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <p class="h4">
                                    Tu no. de ticket:
                                        <asp:Label runat="server" ID="lblNoTicket" />
                                    <br>
                                </p>
                                <p class="h4">
                                    Clave de registro:
                                    <asp:Label runat="server" ID="lblRandom" />
                                </p>
                                <p class="h4">
                                    En breve recibirás un correo con los datos de tu ticket.
                                </p>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer"></div>
                </div>
            </div>
        </div>
        <%--<div class="modal fade" id="modalSessionAbierta" tabindex="-1" role="dialog" aria-labelledby="modalSessionAbiertaTitle" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:LinkButton class="close" runat="server" ID="btnCerramodalSessionAbierta" OnClick="btnCerramodalSessionAbierta_OnClick"><span aria-hidden="true">&times;</span></asp:LinkButton>
                        <h6 class="modal-title" id="modalSessionAbiertaTitle">Sesion abierta </h6>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <p class="h4">
                                    Tu sesión no fue cerrada correctamente o se encuentra abierta en otro equipo
                            <br />
                                    ¿Quieres continuar aquí?
                                    <br>
                                </p>
                                <p>
                                    <div class="row widht100 text-center">
                                        <asp:Button runat="server" CssClass="btn btn-primary margin-auto" Text="Aceptar" OnClick="OnClick"/>
                                    </div>
                                </p>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer"></div>
                </div>
            </div>
        </div>--%>

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
        <script>
            $('.carousel').carousel({
                interval: 3000
            });
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            function BeginRequestHandler(sender, args) {
                var oControl = args.get_postBackElement();
                oControl.disabled = true;
            }
            var Tawk_API = Tawk_API || {}, Tawk_LoadStart = new Date();
            (function () {
                var s1 = document.createElement("script"), s0 = document.getElementsByTagName("script")[0];
                s1.async = true;
                s1.src = 'https://embed.tawk.to/5b0c202a10b99c7b36d46354/default';
                s1.charset = 'UTF-8';
                s1.setAttribute('crossorigin', '*');
                s0.parentNode.insertBefore(s1, s0);
            })();
        </script>
    </form>
</body>
</html>
