<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KiiniHelp.Default1" %>

<%@ Import Namespace="System.IO" %>

<%@ Register Src="~/UserControls/UcLogCopia.ascx" TagPrefix="uc1" TagName="UcLogCopia" %>
<%@ Register Src="~/UserControls/Temporal/UcTicketPortal.ascx" TagPrefix="uc1" TagName="UcTicketPortal" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kiinisupport</title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <link rel='stylesheet' href="assets/css/font.css" />
    <link rel="stylesheet" href="assets/css/font-awesome.css" />
    <link rel="stylesheet" href="assets/css/bootstrap.css" />
    <link href="assets/css/elegant-icons.css" rel="stylesheet" />
    <link href="assets/css/bootstrap-markdown.css" rel="stylesheet" />
    <link rel="stylesheet" href="assets/css/styles.css" />
    <link rel="stylesheet" href="assets/css/elusive-icons.css" />

    <script type="text/javascript">
        function MostrarPopup(modalName) {
            $(modalName).on('shown.bs.modal', function () {
                $(this).find('[autofocus]').focus();
            });
            $(modalName).modal({ backdrop: 'static', keyboard: false });
            $(modalName).modal('show');
            return true;
        };

        function CierraPopup(modalName) {
            $(modalName).modal('hide');
            return true;
        };
        function DontCloseMenu(event) {
            event.stopPropagation();
        };
        function load() {
            var dir = "http://localhost:2802/assets/carouselImage/";
            var fileextension = ".jpg";
            $.ajax({
                url: dir,
                success: function (data) {
                    $(data).find("a:contains(" + fileextension + ")").each(function () {
                        var filename = this.href.replace(window.location.host, "").replace("http:///", "");
                        $("body").append($("<img src=" + dir + filename + "></img>"));
                    });
                }
            });
        }
        function search(evt) {
            if (evt.charCode == 13) {
                this.__doPostBack(this.name, 'OnKeyPress');
                evt.preventDefault();
                return false; // returning false will prevent the event from bubbling up.
            }
            else {
                return true;
            }
        }
        function ShowLanding() {
            var landing = document.getElementById('updateProgress');
            if (landing != undefined) {
                landing.style.display = 'block';
            }
        }

        function HideLanding() {
            var landing = document.getElementById('updateProgress');
            if (landing != undefined) {
                landing.style.display = 'none';
            }
        }
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
            </Scripts>
        </asp:ScriptManager>
        <asp:UpdateProgress ID="updateProgress" runat="server" ClientIDMode="Static">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; background-color: rgba(0,0,0, .1); opacity: 1.0;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loading.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 25%; left: 40%; border: 10px;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <header class="header">
            <div class="branding ">
                <h1 class="logo text-center">
                    <asp:HyperLink NavigateUrl="~/Default.aspx" runat="server">
                        <asp:Image CssClass="logo-icon" ImageUrl="~/assets/images/logoBlanco.jpg" alt="icon" runat="server" />
                        <%-- <span class="nav-label"><span class="h3"><strong>--%>
                        <asp:Label runat="server" ID="lblBranding" Visible="false" />
                        <%--   </strong></span></span>--%>
                    </asp:HyperLink>
                </h1>
            </div>
            <div class="topbar bg_w_header">
                <div class="search-container" runat="server" visible="false">
                    <div id="main-search" style="padding-top: 20px;">
                        <i id="main-search-toggle" class="fa fa-search icon"></i>
                        <div id="main_search_input_wrapper" class="main_search_input_wrapper">
                            <asp:TextBox ID="main_search_input" ClientIDMode="Static" CssClass="main_search_input form-control" onkeypress="search(event)" placeholder="Buscar por palabra clave..." runat="server" />
                            <span id="clear-search" aria-hidden="true" class="fs1 icon icon_close_alt2 clear-search"></span>
                        </div>

                    </div>
                </div>
                <div class="navbar-tools">
                    <div class="utilities-container">
                        <div class="utilities">
                            <div class="item item-notifications">
                                <div class="dropdown-toggle" data-toggle="dropdown" aria-expanded="true" role="button">
                                    <span class="sr-only">Tickets</span> <span class="pe-icon fa fa-ticket icon" data-toggle="tooltip" data-placement="bottom" title="Tickets"></span>
                                </div>
                                <ul class="dropdown-menu wdropdown-ticket" role="menu" aria-labelledby="dropdownMenu-user" style="right: -45px">
                                    <li>
                                        <span class="arrow" style="right: 48px"></span>
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
                                    <span class="sr-only">Ingresa</span> <span class="pe-icon fa fa-sign-in icon" data-toggle="tooltip" data-placement="bottom" title="Ingresa"></span>
                                </div>
                                <div class="dropdown-menu wdropdown-login" role="menu" aria-labelledby="dropdownMenu-messages" style="right: -25px">
                                    <span class="arrow" style="right: 25px"></span>
                                    <div>
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <%-- <br />--%>
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <uc1:UcLogCopia runat="server" ID="UcLogCopia" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="social-btns col-md-12 col-sm-12 col-xs-12 col-md-offset-1 col-sm-offset-0 col-sm-offset-0">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </header>
        <div id="content-wrapper" class="content-wrapper">
            <div class="container-fluid">
                <div class="row" style="margin-top: 50px;">
                    <div class="module-content collapse in centra-carrusel" id="content-1">
                        <div class="module-content-inner no-padding-bottom no-padding-top">
                            <div class="carousel slide" data-ride="carousel" id="carouselPresenter">
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
                                <a class="left carousel-control" data-slide="prev" href="#carouselPresenter">
                                    <span class="glyphicon glyphicon-chevron-left"></span><span class="sr-only">Previous</span>
                                </a><a class="right carousel-control" data-slide="next" href="#carouselPresenter"><span class="glyphicon glyphicon-chevron-right"></span><span class="sr-only">Next</span></a>
                            </div>
                        </div>
                    </div>
                    <%--</div>--%>
                </div>
                <%--                <br />--%>
                <h4 class="title">Para ofrecerte un mejor servicio indicanos que tipo de usuario eres.</h4>
                <%--  <hr />
                <hr />--%>
                <div id="masonry" class="row">
                    <div class=" col-lg-4 col-md-4 col-sm-6 col-xs-12">
                        <section class="module ">
                            <div class="module-inner">
                                <div class="module-content collapse in" id="content-2">
                                    <div class="module-content-inner">
                                        <asp:LinkButton runat="server" ID="lnkBtnEmpleado" OnClick="lnkBtnEmpleado_OnClick">
                                            <img class="img-responsive margin-left" src="assets/images/users_icon/usuario_2.jpg" alt="Iconos de usuarios" /></asp:LinkButton>
                                        <h4 style="text-align: center">EMPLEADO</h4>
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
                                            <img class="img-responsive  margin-left" src="assets/images/users_icon/usuario_1.jpg" alt="Iconos de usuarios" /></asp:LinkButton>
                                        <h4 style="text-align: center">CLIENTE</h4>
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
                                            <img class="img-responsive  margin-left" src="assets/images/users_icon/usuario_3.jpg" alt="Iconos de usuarios" /></asp:LinkButton>
                                        <h4 style="text-align: center">PROVEEDOR</h4>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </div>
                </div>
                <hr />
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <section class="module">
                            <%--  <div class="module-inner">--%>
                            <div class="module-content">
                                <%--<div class="module-content-inner">--%>
                                <div class="help-section">
                                    <div>
                                        <h4 class="text-center title">¿Podemos ayudarte?</h4>
                                        <div role="form" class="search-box form-inline text-center">
                                            <label class="sr-only" for="help_search_form">Buscar</label>
                                            <div class="form-group">
                                                <asp:TextBox ID="help_search_form" name="search-form" CssClass="form-control help-search-form" onkeydown="return (event.keyCode!=13);" placeholder="Busca con una palabra clave..." runat="server" Width="220px" />
                                                <asp:LinkButton type="submit" CssClass="btn btn-primary btn-single-icon" runat="server"><i class="fa fa-search"></i></asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                    <div>
                                        <div class="row text-center"></div>
                                        <div class="row text-center"></div>
                                        <div class="row text-center"></div>
                                    </div>
                                    <div class="help-lead text-center">
                                        <h4 class="title">¿Aún necesitas ayuda?</h4>
                                        <a class="btn btn-primary" data-toggle="modal" data-target="#modal-new-ticket"><i class="fa fa-play-circle"></i>Generar un ticket </a>
                                    </div>
                                    <br />
                                </div>
                                <%--  </div>--%>
                                <%-- <br />--%>
                            </div>
                            <%-- </div>--%>
                        </section>
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
                                <asp:LinkButton class="close" runat="server" ID="btnCerrarTicket" OnClick="btnCerrarTicket_OnClick"><span aria-hidden="true">&times;</span></asp:LinkButton>
                                <h4 class="modal-title" id="modal-new-ticket-label">Crear Ticket Nuevo</h4>
                            </div>
                            <div class="modal-body">
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
                        <h4 class="modal-title" id="myModalLabel">
                            <%--<img img-responsive margin-left src="assets/images/icons/ok.png" alt="" /><br>--%>
                            Tu ticket se creo con éxito</h4>
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
        <footer id="footer" class="site-footer">
            <div class="copyright">&copy; 2017 Kiininet. Todos los derechos reservados </div>
            <div class="politicas-Privacidad">
                <asp:HyperLink runat="server" NavigateUrl="~/Default.aspx">Políticas de Privacidad </asp:HyperLink>
                <asp:Label runat="server" Text="  |  "></asp:Label>
                <asp:HyperLink runat="server" NavigateUrl="~/Default.aspx"> Términos y Condiciones</asp:HyperLink>
            </div>
        </footer>

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
