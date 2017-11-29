<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTestMain.aspx.cs" Inherits="KiiniHelp.Test.FrmTestMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" type="text/css" href="../bootstrap/MainMenu/css/normalize.css" />
    <link rel="stylesheet" type="text/css" href="../bootstrap/MainMenu/css/demo.css" />
    <link rel="stylesheet" type="text/css" href="../bootstrap/MainMenu/css/icons.css" />
    <link rel="stylesheet" type="text/css" href="../bootstrap/MainMenu/css/component.css" />
    <script src="../BootStrap/MainMenu/js/modernizr.custom.js"></script>
    <style>
        #full, #form1 {
            height: 100%;
        }
    </style>
</head>
<body>
    <div id="full">
        <form id="form1" runat="server">
            <asp:ScriptManager runat="server">
                <Scripts>
                    <asp:ScriptReference Path="~/BootStrap/MainMenu/js/classie.js" />
                    <asp:ScriptReference Path="~/BootStrap/MainMenu/js/mlpushmenu.js" />
                </Scripts>
            </asp:ScriptManager>
            <header style="background: red" class="clearFix">
                <span href="#" id="trigger" class="menu-trigger"></span>
                <br />
                <br />
                <br />

            </header>
            <div class="container">
                <!-- Push Wrapper -->
                <div class="mp-pusher" id="mp-pusher">

                    <!-- mp-menu -->
                    <nav id="mp-menu" class="mp-menu">
                        <div class="mp-level">
                            <h2 class="icon icon-world">All Categories</h2>
                            <ul>
                                <li class="icon icon-arrow-left">
                                    <a class="icon icon-display" href="#">Configuración</a><span class="tamanolista"></span>
                                    <div class="mp-level">
                                        <h2 class="icon icon-display">Configuración</h2>
                                        <a class="mp-back" href="#">Atras</a>
                                        <ul>
                                            <li class="icon icon-arrow-left">
                                                <a class="icon icon-phone" href="#">Nivel 1 - 1<span class="tamanolista"></span></a>
                                                <div class="mp-level">
                                                    <h2>Nivel 1 - 1</h2>
                                                    <a class="mp-back" href="#">Atras</a>
                                                    <ul>
                                                        <li><a href="#">Opcion 1</a></li>
                                                        <li><a href="#">Opcion 2</a></li>
                                                        <li><a href="#">Opcion 3</a></li>
                                                        <li><a href="#">Opcion 4</a></li>
                                                        <li class="icon icon-arrow-left">
                                                            <a class="icon icon-phone" href="#">Nivel 2 - 1</a>
                                                            <div class="mp-level">
                                                                <h2>Nivel 2 - 1</h2>
                                                                <a class="mp-back" href="#">Atras</a>
                                                                <ul>
                                                                    <li><a href="#">Opcion 1</a></li>
                                                                    <li><a href="#">Opcion 2</a></li>
                                                                    <li><a href="#">Opcion 3</a></li>
                                                                    <li><a href="#">Opcion 4</a></li>
                                                                </ul>
                                                            </div>
                                                        </li>
                                                    </ul>
                                                </div>

                                            </li>
                                            <li class="icon icon-arrow-left">
                                                <a class="icon icon-phone" href="#">Nivel 2 - 2</a>
                                                <div class="mp-level">
                                                    <h2>Nivel 1 - 2</h2>
                                                    <a class="mp-back" href="#">Atras</a>
                                                    <ul>
                                                        <li><a href="#">Opcion 1</a></li>
                                                        <li><a href="#">Opcion 2</a></li>
                                                        <li><a href="#">Opcion 3</a></li>
                                                        <li><a href="#">Opcion 4</a></li>
                                                        <li><a href="#">Opcion 5</a></li>
                                                    </ul>
                                                </div>
                                            </li>
                                            <li class="icon icon-arrow-left">
                                                <a class="icon icon-phone" href="#">Nivel 1 - 3</a>
                                                <div class="mp-level">
                                                    <h2>Nivel 1 - 3</h2>
                                                    <a class="mp-back" href="#">Atras</a>
                                                    <ul>
                                                        <li><a href="#">Opcion 1</a></li>
                                                        <li><a href="#">Opcion 2</a></li>
                                                        <li><a href="#">Opcion 3</a></li>
                                                        <li><a href="#">Opcion 4</a></li>
                                                    </ul>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </li>
                            </ul>

                        </div>
                    </nav>
                    <!-- /mp-menu -->

                    <div class="scroller">
                        <div class="scroller-inner">
                            <div class="content clearfix">
                                <div class="block block-40 clearfix">
                                </div>
                            </div>
                        </div>
                        <!-- /scroller-inner -->
                    </div>
                    <!-- /scroller -->

                </div>
                <!-- /pusher -->
            </div>
            <!-- /container -->

            <script>
                new mlPushMenu(document.getElementById('mp-menu'), document.getElementById('trigger'), {
                    type: 'overlap'
                });
            </script>
        </form>
    </div>
</body>
</html>
