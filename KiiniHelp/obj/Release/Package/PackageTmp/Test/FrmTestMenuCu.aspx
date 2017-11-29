<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTestMenuCu.aspx.cs" Inherits="KiiniHelp.Test.FrmTestMenuCu" %>

<%@ Register Src="~/UserControls/UcLogIn.ascx" TagPrefix="uc1" TagName="UcLogIn" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="shortcut icon" href="assets/images/favicon.ico">
    <link href="//fonts.googleapis.com/css?family=Open+Sans:300italic,400italic,600italic,700italic,800italic,400,300,600,700,800" rel='stylesheet' type='text/css'>
    <link rel="stylesheet" href="../on/css/bootstrap.css">
    <link rel="stylesheet" href="../on/css/metisMenu.css">
    <link rel="stylesheet" href="../on/css/font-awesome.css">
    <link rel="stylesheet" href="../on/css/elegant-icons.css">
    <link rel="stylesheet" href="../on/css/pe-7-icons.css">
    <link rel="stylesheet" href="../on/css/pe-7-icons-helper.css">
    <link rel="stylesheet" href="../on/css/styles.css">
    <link rel="stylesheet" href="../on/css/help.css">
</head>
<body class="layout-no-leftnav" data-trigger="">
    <form runat="server">
        <!--INICIA HEADER-->
        <header class="header">
            <div class="branding float-left">
                <h1 class="logo text-center">
                    <a href="index.html">
                        <img class="logo-icon" src="assets/images/logo-icon.svg" alt="icon" />
                        <span class="nav-label">
                            <span class="h3"><strong>Bancremex</strong></span></span>
                    </a>
                </h1>
            </div>
            <div class="topbar bg_w_header">
                <!--INICIA MENU COLAPSABLE-->
                
                <!--TERMINA MENU COLAPSABLE-->
                <!--INICIA BUSCADOR-->
                <div class="search-container">
                    <form id="main-search" class="main-search">
                        <i id="main-search-toggle" class="fa fa-search icon"></i>
                        <div id="main-search-input-wrapper" class="main-search-input-wrapper">
                            <input type="text" id="main-search-input" class="main-search-input form-control" placeholder="Buscar por palabra clave...">

                            <span id="clear-search" aria-hidden="true" class="fs1 icon icon_close_alt2 clear-search"></span>
                        </div>
                    </form>
                    <!--TERMINA BUSCADOR-->
                </div>
                <!--INICIA HERRAMIENTAS-->
                <div class="navbar-tools">
                    <div class="utilities-container">
                        <div class="utilities">

                            <!--INICIA TICKET-->
                            <div class="item item-notifications">
                                <div class="dropdown-toggle" id="dropdownMenu-notifications" data-toggle="dropdown" aria-expanded="true" role="button">
                                    <span class="sr-only">Tickets</span>
                                    <span class="pe-icon fa fa-ticket icon" data-toggle="tooltip" data-placement="bottom" title="Tickets"></span>
                                </div>
                                <ul class="dropdown-menu wdropdown-ticket" role="menu" aria-labelledby="dropdownMenu-user">
                                    <li><span class="arrow"></span><a role="menuitem" data-toggle="modal" data-target="#modal-new-ticket"><span class="pe-icon pe-7s-plus icon"></span>Nuevo ticket</a></li>
                                    <li><a role="menuitem" href="guest_ticket_con.html"><span class="pe-icon pe-7s-look icon"></span>Consultar ticket</a></li>
                                </ul>
                            </div>
                            <!--TERMINA TICKET-->

                            <!--INICIA REGISTRATE-->
                            <div class="item item-notifications">
                                <a href="signup.html"><span class="sr-only">Regístrate</span>
                                    <span class="pe-icon fa fa-book icon" data-toggle="tooltip" data-placement="bottom" title="Regístrate"></span></a>

                            </div>
                            <!--TERMINA REGISTRATE-->
                            <!--INICIA LOGIN-->
                            <div class="item item-messages dropdown">
                                <div class="dropdown-toggle" id="dropdownMenu-messages" data-toggle="dropdown" aria-expanded="true" role="button">
                                    <span class="sr-only">Ingresa</span>
                                    <span class="pe-icon fa fa-sign-in icon" data-toggle="tooltip" data-placement="bottom" title="Ingresa"></span>
                                </div>
                                <div class="dropdown-menu wdropdown-login" role="menu" aria-labelledby="dropdownMenu-messages">
                                    <span class="arrow"></span>
                                    <div class="message-items no-overflow">


                                        <div class="form-container col-md-12 col-sm-12 col-xs-12">
                                            <br>
                                            <p class="lead">Accede a tu cuenta</p>
                                            <uc1:UcLogIn runat="server" id="UcLogIn" />
                                        </div>

                                        <div class="social-btns col-md-12 col-sm-12 col-xs-12 col-md-offset-1 col-sm-offset-0 col-sm-offset-0">
                                            <div class="divider"><span>O ingresa con</span></div>
                                            <ul class="list-unstyled social-login">

                                                <li>
                                                    <button class="social-btn facebook-btn btn" type="button"><i class="fa fa-facebook"></i>Facebook</button>
                                                </li>

                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!--TERMINA LOGIN-->
                        </div>
                    </div>
                    <!--TERMINA HERRAMIENTAS-->

                    <!--INICIA PERFIL-->
                    <div class="user-container dropdown">
                        <div class="dropdown-toggle" id="dropdownMenu-user" data-toggle="dropdown" aria-expanded="true" role="button">
                            <img src="assets/images/profiles/profile-1.png" alt="" />
                            <i class="fa fa-caret-down"></i>
                        </div>
                        <ul class="dropdown-menu" role="menu" aria-labelledby="dropdownMenu-user">
                            <li><span class="arrow"></span><a role="menuitem" href="#"><span class="pe-icon pe-7s-user icon"></span>Mi perfil</a></li>
                            <li><a role="menuitem" href="signup.html"><span class="pe-icon pe-7s-paper-plane icon"></span>Registrarse</a></li>
                            <li><a role="menuitem" href="login.html"><span class="fa fa-sign-in icon"></span>Ingresar</a></li>
                            <li><a role="menuitem" href="index.html"><span class="fa fa-sign-out icon"></span>Salir</a></li>
                        </ul>
                    </div>
                    <!--TERMINA PERFIL-->
                </div>
            </div>
        </header>
        <!--TERMINA HEADER-->
        <div id="content-wrapper" class="content-wrapper view projects-view ">
            <div class="container-fluid">

                <div class="row">

                    <br>
                    <br>
                    <br>
                    <!--INICIA CARRUSEL-->
                    <div class="module-content collapse in" id="content-1">
                        <div class="module-content-inner no-padding-bottom">

                            <div class="carousel slide bootstrap-carousel" data-ride="carousel" id="carousel-example-generic">
                                <!-- Indicators -->
                                <ol class="carousel-indicators">
                                    <li class="active" data-slide-to="0" data-target="#carousel-example-generic"></li>
                                    <li data-slide-to="1" data-target="#carousel-example-generic"></li>
                                    <li data-slide-to="2" data-target="#carousel-example-generic"></li>
                                </ol>
                                <!-- Wrapper for slides -->
                                <div class="carousel-inner">
                                    <div class="item active">
                                        <img src="assets/images/carousels/slide-1.jpg" alt="">
                                    </div>
                                    <div class="item">
                                        <img src="assets/images/carousels/slide-2.jpg" alt="">
                                    </div>
                                    <div class="item">
                                        <img src="assets/images/carousels/slide-3.jpg" alt="">
                                    </div>
                                </div>
                                <!-- Controls -->
                                <a class="left carousel-control" data-slide="prev" href="#carousel-example-generic"><span class="glyphicon glyphicon-chevron-left"></span><span class="sr-only">Previous</span></a> <a class="right carousel-control" data-slide="next" href="#carousel-example-generic"><span class="glyphicon glyphicon-chevron-right"></span><span class="sr-only">Next</span></a>
                            </div>
                        </div>
                    </div>
                    <!--TERMINA CARRUSEL-->
                </div>
                <br>
                <!--INICIA TITULO
<h2 class="title text-left">Para ofrecerte un mejor servicio indicanos que tipo de usuario eres.</h2>
<hr>
<!--TERMINA TITULO-->
                <hr>
                <!--INICIA USUARIOS-->
                <div id="masonry" class="row">
                    <div class="module-wrapper masonry-item col-lg-4 col-md-4 col-sm-6 col-xs-12">
                        <section class="module project-module">
                            <div class="module-inner">
                                <div class="module-content collapse in" id="content-2">
                                    <div class="module-content-inner">
                                        <a href="user_select.html">
                                            <img class="img-responsive margin-left" src="assets/images/users_icon/usuario_2.jpg" alt="Iconos de usuarios" /></a>
                                        <h4 style="text-align: center">EMPLEADO</h4>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </div>
                    <div class="module-wrapper masonry-item col-lg-4 col-md-4 col-sm-6 col-xs-12">
                        <section class="module project-module">
                            <div class="module-inner">
                                <div class="module-content collapse in" id="content-2">
                                    <div class="module-content-inner">
                                        <a href="user_select.html">
                                            <img class="img-responsive  margin-left" src="assets/images/users_icon/usuario_1.jpg" alt="Iconos de usuarios" /></a>
                                        <h4 style="text-align: center">CLIENTE</h4>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </div>
                    <div class="module-wrapper masonry-item col-lg-4 col-md-4 col-sm-6 col-xs-12">
                        <section class="module project-module">
                            <div class="module-inner">
                                <div class="module-content collapse in" id="content-2">
                                    <div class="module-content-inner">
                                        <a href="user_select.html">
                                            <img class="img-responsive  margin-left" src="assets/images/users_icon/usuario_3.jpg" alt="Iconos de usuarios" /></a>
                                        <h4 style="text-align: center">PROVEEDOR</h4>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </div>
                </div>
                <!--TERMINA USUARIOS-->
                <hr>
                <!--INICIA BUSCADOR-->
                <div class="row">
                    <div class="module-wrapper col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <section class="module module-headings">
                            <div class="module-inner">
                                <div class="module-content">
                                    <div class="module-content-inner">
                                        <div class="help-section">
                                            <div class="help-search">
                                                <h3 class="text-center title">¿Podemos ayudarte?</h3>
                                                <form class="search-box form-inline text-center margin-bottom-lg">
                                                    <label class="sr-only" for="help-search-form">Buscar</label>
                                                    <div class="form-group">
                                                        <input id="help-search-form" name="search-form" type="text" class="form-control help-search-form" placeholder="Busca con una palabra clave...">
                                                        <button type="submit" class="btn btn-primary btn-single-icon"><i class="fa fa-search"></i></button>
                                                    </div>
                                                </form>
                                            </div>

                                            <div class="help-category-wrapper margin-bottom-lg">
                                                <div class="row text-center"></div>

                                                <div class="row text-center"></div>

                                                <div class="row text-center"></div>

                                            </div>
                                            <div class="help-lead text-center margin-bottom-lg">
                                                <h4 class="subtitle">¿Aún necesitas ayuda?</h4>
                                                <a class="btn btn-primary" role="menuitem" data-toggle="modal" data-target="#modal-new-ticket"><i class="fa fa-play-circle"></i>
                                                    Generar un ticket
									</a>
                                            </div>

                                        </div>

                                    </div>

                                </div>

                            </div>
                        </section>

                    </div>

                </div>

            </div>
            <!--TERMINA BUSCADOR-->
        </div>

        </div>


	</div>

	</div>
	
</div>
        <!--INICIA MODAL NUEVO TICKET-->
        <div class="modal fade" id="modal-new-ticket" tabindex="-1" role="dialog" aria-labelledby="modal-new-ticket-label">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="modal-new-ticket-label">
                            <img img-responsive margin-left src="assets/images/icons/new_ticket.png" alt="" /><br>
                            Crear Ticket Nuevo</h4>
                    </div>
                    <div class="modal-body">
                        <form>
                            <div class="form-group">
                                <label class="col-sm-12 control-label">¿Cuál es tu solicitud?</label>
                                <div class="col-sm-8">
                                    <label class="radio-inline">
                                        <input type="radio" name="inlineRadioOptions" id="inlineRadio1" value="option1">
                                        Servicio/Consulta
										
                                    </label>
                                    <label class="radio-inline">
                                        <input type="radio" name="inlineRadioOptions" id="inlineRadio2" value="option2">
                                        Problema
										
                                    </label>
                                </div>
                            </div>
                            <br>
                            <br>
                            <hr>
                            <div class="form-group">
                                <label class="col-sm-12 control-label">Área de atención</label>
                                <div class="col-sm-10">
                                    <select class="form-control">
                                        <option>Ninguna</option>
                                        <option>Administración</option>
                                        <option>Servicio al Cliente</option>
                                        <option>Cuentas Especiales</option>
                                        <option>Gerencia</option>
                                    </select>
                                </div>
                            </div>
                            <br>
                            <br>
                            <br>
                            <hr>
                            <div class="form-group">
                                <label class="sr-only">Ticket</label>
                                <input type="text" class="form-control" placeholder="Nombre">
                            </div>
                            <div class="form-group">
                                <label class="sr-only">Ticket</label>
                                <input type="text" class="form-control" placeholder="Correo electrónico">
                            </div>
                            <div class="form-group">
                                <label class="sr-only">Ticket</label>
                                <input type="text" class="form-control" placeholder="Asunto">
                            </div>
                            <div class="form-group">
                                <label class="sr-only">Description</label>
                                <textarea class="form-control" rows="2" placeholder="Escribe un comentario..."></textarea>
                            </div>
                            <hr>
                            <div class="form-group">
                                <label for="exampleInputFile">Agregar un archivo</label>
                                <input type="file" id="exampleInputFile">
                            </div>
                            <hr>
                            <div class="checkbox remember">
                                <label>
                                    <input type="checkbox">
                                    No soy un Robot
								
                                </label>
                            </div>
                            <button type="button" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#myModal" data-dismiss="modal">
                                Crear ticket
							
                            </button>

                        </form>
                    </div>
                </div>
            </div>



        </div>
        <!--TERMINA MODAL NUEVO TICKET-->
        <!--INICIA MODAL RESPUESTA NUEVO TICKET-->
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h3 class="modal-title" id="myModalLabel">
                            <img img-responsive margin-left src="assets/images/icons/ok.png" alt="" /><br>
                            </i>Tu ticket se creo con éxito</h3>
                    </div>
                    <div class="modal-body">
                        <hr>
                        <p class="h4"><strong>Tu no. de ticket: 01</strong><br>
                        </p>
                        <p class="h4"><strong>Clave de registro: 234D45</strong></p>
                        <hr>
                        En breve recibirás un correo con los datos de tu ticket para que puedas dar seguimiento.
										
                    </div>
                    <div class="modal-footer">
                    </div>
                </div>
            </div>
        </div>
        <!--INICIA MODAL RESPUESTA NUEVO TICKET-->
        <!--TERMINA MODALES-->
        <!--INICIA FOOTER-->
        <footer id="footer" class="site-footer">
            <div class="copyright">Kiinisuppor &copy; 2017 - <a href="http://www.kiininet.com" target="_blank">Powered by Kiinenet</a></div>
        </footer>
        <!--TERMINA FOOTER-->

        <!--INICIA SELECCION DE ROLES-->

        <div id="side-panel" class="side-panel">
            <div class="side-panel-inner">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="title text-center"><i class="fa fa-users"></i>Roles</h4>
                <div class="items-wrapper">
                    <div class="item">
                        <div class="symbol-holder">
                            <button class="icon-container btn btn-info btn-circle">
                                <i class="icon fa fa-user"></i>
                            </button>
                        </div>
                        <div class="content-holder">
                            <div class="subject-line">
                                <strong>ADMINISTRADOR</strong>
                            </div>
                            <div class="time-stamp">Desde el 15 de agosto</div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="symbol-holder">
                            <button class="icon-container btn btn-info btn-circle">
                                <i class="icon fa fa-user"></i>
                            </button>
                        </div>
                        <div class="content-holder">
                            <div class="subject-line">
                                <strong>ADMINISTRADOR</strong>
                            </div>
                            <div class="time-stamp">Desde el 15 de agosto</div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="symbol-holder">
                            <button class="icon-container btn btn-info btn-circle">
                                <i class="icon fa fa-user"></i>
                            </button>
                        </div>
                        <div class="content-holder">
                            <div class="subject-line">
                                <strong>ADMINISTRADOR</strong>
                            </div>
                            <div class="time-stamp">Desde el 15 de agosto</div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="symbol-holder">
                            <button class="icon-container btn btn-info btn-circle">
                                <i class="icon fa fa-user"></i>
                            </button>
                        </div>
                        <div class="content-holder">
                            <div class="subject-line">
                                <strong>ADMINISTRADOR</strong>
                            </div>
                            <div class="time-stamp">Desde el 15 de agosto</div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="symbol-holder">
                            <button class="icon-container btn btn-info btn-circle">
                                <i class="icon fa fa-user"></i>
                            </button>
                        </div>
                        <div class="content-holder">
                            <div class="subject-line">
                                <strong>ADMINISTRADOR</strong>
                            </div>
                            <div class="time-stamp">Desde el 15 de agosto</div>
                        </div>
                    </div>
                    <div class="item">
                        <div class="symbol-holder">
                            <button class="icon-container btn btn-info btn-circle">
                                <i class="icon fa fa-user"></i>
                            </button>
                        </div>
                        <div class="content-holder">
                            <div class="subject-line">
                                <strong>ADMINISTRADOR</strong>
                            </div>
                            <div class="time-stamp">Desde el 15 de agosto</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!--FIN SELECCION DE ROLES-->
        <!--JS-->
        <script src="../on/js/jquery.js"></script>
        <script src="../on/js/bootstrap.js"></script>
        <script src="../on/js/metisMenu.js"></script>
        <script src="../on/js/imagesloaded.js"></script>
        <script src="../on/js/masonry.js"></script>
        <script src="../on/js/pace.js"></script>
        <script src="../on/js/main.js"></script>
    </form>
</body>
</html>
