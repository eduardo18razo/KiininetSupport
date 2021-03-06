﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcCambiarContrasena.ascx.cs" Inherits="KiiniHelp.UserControls.Operacion.UcCambiarContrasena" %>

<asp:UpdatePanel ID="upGeneral" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <section class="module">
            <div class="module-inner">
                <div class="row">
                    <div class="module-heading">
                        <h3>Cambiar Contraseña</h3>
                    </div>
                </div>
                <div class="row">
                    <div class="form-horizontal text-left">
                        <div class="form-group no-margin-bottom text-left">
                            <asp:Label runat="server" ID="lblCaracteristicas" Text="La contraseña debe tener:" CssClass="control-label text-bold2" />
                            <ul runat="server" id="listParamtros" class="list styleList text-bold2">
                                <li runat="server" id="paramLongitud">
                                    <asp:Label runat="server" ID="lblLongitud" Text="Longitud minima de 8000 caracteres"></asp:Label></li>
                                <li runat="server" id="paramMayuscula">
                                    <asp:Label runat="server" ID="lblLongitudMayus" Text="1 Mayuscula"></asp:Label></li>
                                <li runat="server" id="paramNumero">
                                    <asp:Label runat="server" ID="Label6" Text="1 Numero"></asp:Label></li>
                                <li runat="server" id="paramEspecial">
                                    <asp:Label runat="server" ID="Label7" Text="1 Caracter especial"></asp:Label></li>
                            </ul>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" class="col-sm-4 col-md-2 col-lg-2 col-md-offset-2 col-lg-offset-2 control-label">Contraseña Actual</asp:Label>
                            <div class="col-sm-2 col-md-2 col-lg-2">
                                <asp:TextBox runat="server" ID="txtContrasenaActual" type="password" CssClass="form-control col-sm-2 col-md-2 col-lg-2 text-no-transform" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" class="col-sm-4 col-md-2 col-lg-2 col-md-offset-2 col-lg-offset-2 control-label">Contraseña Nueva</asp:Label>
                            <div class="col-sm-2 col-md-2 col-lg-2">
                                <asp:TextBox runat="server" ID="txtContrasenaNueva" type="password" CssClass="form-control text-no-transform" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" class="col-sm-4 col-md-2 col-lg-2 col-md-offset-2 col-lg-offset-2 control-label">Confirma Contraseña Nueva</asp:Label>
                            <div class="col-sm-2 col-md-2 col-lg-2">
                                <asp:TextBox runat="server" ID="txtConfirmaContrasenaNueva" type="password" CssClass="form-control text-no-transform" />
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-sm-2 col-md-2 col-lg-2 col-sm-offset-4 col-md-offset-4 col-lg-offset-4 text-right">
                                <br />
                                <asp:Button runat="server" ID="btnAceptar" Text="Cambiar" OnClick="btnAceptar_OnClick" CssClass="btn btn-success " />

                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </section>

    </ContentTemplate>
</asp:UpdatePanel>
