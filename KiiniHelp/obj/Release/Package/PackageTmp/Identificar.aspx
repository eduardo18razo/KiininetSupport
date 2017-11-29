<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Identificar.aspx.cs" Inherits="KiiniHelp.Identificar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Kiinisupport</title>
    <link rel='stylesheet' href="assets/css/font.css" />
    <link rel="stylesheet" href="assets/css/font-awesome.css" />
    <link rel="stylesheet" href="assets/css/bootstrap.css" />
    <link href="assets/css/elegant-icons.css" rel="stylesheet" />
    <link href="assets/css/bootstrap-markdown.css" rel="stylesheet" />
    <link rel="stylesheet" href="assets/css/styles.css" />

</head>
<body class="layout_no_leftnav">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scripMain" runat="server" EnablePageMethods="true">
            <Scripts>
                <asp:ScriptReference Path="~/assets/js/jquery.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap.js" />
            </Scripts>
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upGeneral" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <section class="login-section auth-section">
                    <div class="container">
                        <div class="row">
                            <div class="form-box col-md-8 col-sm-12 col-xs-12 col-md-offset-2 col-sm-offset-0 xs-offset-0">
                                <h1 class="form-box-heading logo text-center"><span class="highlight">SOPORTE <% Response.Write(ConfigurationManager.AppSettings["Brand"]); %></span> </h1>
                                <div class="form-box-inner">
                                    <h2 class="title text-center">¿Olvidaste tu contraseña?</h2>
                                    <div class="row">
                                        <div class="form-container col-md-6 col-sm-12 col-xs-12">
                                            <div data-parsley-validate id="" class="form-horizontal">
                                                <div class="form-group email" style="top: 15px">
                                                    <div><span>Ingresa tu nombre de usuario, correo o celular:</span></div>
                                                </div>
                                                <div class="form-group email" style="top: 15px">
                                                    <label class="sr-only" for="login-email">Email or username</label>
                                                    <span class="fa fa-user icon"></span>
                                                    <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control login-email" Style="text-transform: none" placeholder="Nombre de usuario, correo o celular" />
                                                </div>
                                                <div class="form-group email" style="top: 15px">
                                                    <asp:Button CssClass="btn btn-block btn-primary" Text="Enviar" runat="server" Style="margin-top: 30px" OnClick="btnBuscar_OnClick" />

                                                    <%--<p class="alt-path"><a class="signup-link" href="login.html">Regresar a acceder a mi cuenta</a></p>--%>
                                                    <p class="alt-path">
                                                        <asp:HyperLink CssClass="signup-link" NavigateUrl="~/Default.aspx" runat="server">Regresar al inicio</asp:HyperLink>
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="copyright text-center">&copy; 2017 - Powered by Kiininet</div>
                    </div>
                </section>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
