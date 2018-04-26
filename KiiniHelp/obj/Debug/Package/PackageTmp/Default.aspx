<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KiiniHelp.Default1" %>

<%@ Import Namespace="System.IO" %>

<%@ Register Src="~/UserControls/UcLogCopia.ascx" TagPrefix="uc1" TagName="UcLogCopia" %>
<%@ Register Src="~/UserControls/Temporal/UcTicketPortal.ascx" TagPrefix="uc1" TagName="UcTicketPortal" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />

    <title>Kiinisupport</title>
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
            <div class="branding">
                <h1 class="logo text-center">
                    <asp:HyperLink NavigateUrl="~/Default.aspx" runat="server">
                        <asp:Image CssClass="logo-icon" ImageUrl="~/assets/images/logoBlanco.jpg" alt="icon" runat="server" />
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
                                <div class="carousel slide z-Index-9" data-ride="carousel" id="carouselPresenter">
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
                        <div class=" col-lg-4 col-md-4 col-sm-6 col-xs-12">
                            <section class="module ">
                                <div class="module-inner">
                                    <div class="module-content collapse in" id="content-2">
                                        <div class="module-content-inner">
                                            <asp:LinkButton runat="server" ID="lnkBtnEmpleado" OnClick="lnkBtnEmpleado_OnClick">
                                            <img class="img-responsive margin-auto" src="assets/images/users_icon/usuario_2.jpg" alt="Iconos de usuarios" /></asp:LinkButton>
                                            <h4 class="text-center">EMPLEADO</h4>
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </div>
                        <div class=" col-lg-4 col-md-4 col-sm-6 col-xs-12">
                            <section class="module ">
                                <div class="module-inner">
                                    <div class="module-content collapse in" id="content-3">
                                        <div class="module-content-inner">
                                            <asp:LinkButton runat="server" ID="lnkBtnCliente" OnClick="lnkBtnCliente_OnClick">
                                            <img class="img-responsive  margin-auto" src="assets/images/users_icon/usuario_1.jpg" alt="Iconos de usuarios" /></asp:LinkButton>
                                            <h4 class="text-center">CLIENTE</h4>
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-6 col-xs-12">
                            <section class="module">
                                <div class="module-inner">
                                    <div class="module-content collapse in" id="content-4">
                                        <div class="module-content-inner">
                                            <asp:LinkButton runat="server" ID="lnkBtnProveedor" OnClick="lnkBtnProveedor_OnClick">
                                            <img class="img-responsive  margin-auto" src="assets/images/users_icon/usuario_3.jpg" alt="Iconos de usuarios" /></asp:LinkButton>
                                            <h4 class="text-center">PROVEEDOR</h4>
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </div>
                    </div>
                    <%--<hr />--%>
                    <%--  <div class="row margin-bottom-10">
                        <div class="module-wrapper col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <section class="module module-headings">
                                <div class="module-inner">
                                    <div class="module-content">
                                        <div class="help-search">
                                            <h4 class="text-center title">¿Podemos ayudarte?</h4>
                                            <div class="row">
                                                <div class="search-box form-inline text-center">
                                                    <label class="sr-only" for="help_search_form">Buscar</label>
                                                    <div class="form-group col-lg-6 col-md-6 col-sm-10 col-xs-10 col-lg-offset-3 col-md-offset-3 col-sm-offset-1 col-xs-offset-1">
                                                        <div class="col-lg-8 col-md-8 col-sm-11 col-xs-11 text-right no-padding-top">
                                                            <asp:TextBox ID="help_search_form" name="search-form" CssClass="form-control help_search_form" onkeydown="return (event.keyCode!=13);" placeholder="Busca con una palabra clave..." runat="server" />
                                                        </div>
                                                        <div class="col-lg-4 col-md-4 col-sm-1 col-xs-1 text-left">
                                                            <asp:LinkButton CssClass="btn btn-primary btn-single-icon" runat="server"><i class="fa fa-search"></i></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="help-lead text-center">
                                                    <h4 class="title">¿Aún necesitas ayuda?</h4>
                                                    <a class="btn btn-primary" data-toggle="modal" data-target="#modal-new-ticket"><i class="fa fa-play-circle"></i>Generar un ticket </a>
                                                </div>
                                            </div>
                                            <br />
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </div>
                    </div>--%>
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
                        <h4 class="modal-title" id="myModalLabel">Tu ticket se creo con éxito</h4>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <hr />
                                <p class="h4">
                                    <strong>Tu no. de ticket:
                                        <asp:Label runat="server" ID="lblNoTicket" /></strong>
                                    <br>
                                </p>
                                <p class="h4">
                                    <strong>Clave de registro:
                                    <asp:Label runat="server" ID="lblRandom" /></strong>
                                </p>
                                <hr />
                                En breve recibirás un correo con los datos de tu ticket para que puedas dar seguimiento.
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer"></div>
                </div>
            </div>
        </div>

        <div class="row">
            <footer id="footer" class="site-footer">
                <div class="copyright">&copy; 2017 Kiininet. Todos los derechos reservados </div>
                <div class="politicas-Privacidad">
                    <asp:HyperLink runat="server" NavigateUrl="~/PoliticasPrivacidad.aspx">Políticas de Privacidad </asp:HyperLink>
                    <asp:Label runat="server" Text="  |  "></asp:Label>
                    <asp:HyperLink runat="server" NavigateUrl="~/TerminosyCondiciones.aspx"> Términos y Condiciones</asp:HyperLink>
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
        </script>
    </form>
</body>
</html>
