<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcLogCopia.ascx.cs" Inherits="KiiniHelp.UserControls.UcLogCopia" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="modal-header">
            <h6>Accede a tu cuenta</h6>
        </div>
        <div class="modal-body no-padding-top no-padding-bottom">
            <div class="row">
                <asp:HiddenField runat="server" ID="fhFallo" Value="False" />
                <div class="email">
                    <label class="sr-only" for="txtUsuario">Email or username</label>
                    <span class="fa fa-user icon iconLogin"></span>
                    <asp:TextBox class="form-control login-email padding-30-left text-no-transform" ID="txtUsuario" placeholder="Usuario" runat="server" autofocus="autofocus" />
                </div>
                <div class="password">
                    <label class="sr-only" for="txtpwd">Password</label>
                    <span class="fa fa-lock icon iconLogin"></span>
                    <asp:TextBox type="password" class="form-control login-password padding-30-left text-no-transform" ID="txtpwd" placeholder="Contraseña" runat="server" TextMode="Password"/>
                    <p class="forgot-password">
                        <asp:HyperLink NavigateUrl="~/Identificar.aspx" runat="server" CssClass="text-theme">¿Olvidaste tu contraseña?</asp:HyperLink>
                    </p>
                </div>
                <div class="form-group">
                    <asp:CustomValidator ErrorMessage="" OnServerValidate="OnServerValidate" runat="server" />
                    <cc1:CaptchaControl ID="Captcha1" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="4" CssClass="col-sm-10"
                        CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                        FontColor="#D20B0C" NoiseColor="#B1B1B1" />
                    <asp:TextBox CssClass="form-control text-uppercase" ID="txtCaptcha" runat="server" />
                </div>
                <asp:Button CssClass="btn btn-block btn-primary" ID="btnLogin" OnClick="btnLogin_Click" runat="server" Text="Entrar" />
                <asp:CheckBox runat="server" Text="Recordarme" AutoPostBack="False" onclick="DontCloseMenu()" Visible="False" />
            </div>
        </div>
        <div class="text-center">
            <p class="margin-top-10">
                <label>¿No tienes una cuenta?</label>
                <asp:HyperLink runat="server" NavigateUrl="~/Registro.aspx" CssClass="text-theme">Regístrate aquí</asp:HyperLink>
            </p>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

