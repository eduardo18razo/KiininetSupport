<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcLogCopia.ascx.cs" Inherits="KiiniHelp.UserControls.UcLogCopia" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>

<div class="module" style="border:none;">
    <div class="col-md-12 col-sm-12 col-xs-12 module-inner">
       <%-- <br>--%>
        <H4>Accede a tu cuenta</H4> <%--lead--%>
        <%--<div class="" >--%>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="fhFallo" Value="False"/>
                    <div class="form-group email">
                        <label class="sr-only" for="login-email">Email or username</label>
                        <span class="fa fa-user icon"></span>
                        <asp:TextBox class="form-control login-email" ID="txtUsuario" placeholder="Usuario" runat="server" Style="text-transform: none" autofocus="autofocus"></asp:TextBox>
                    </div>
                    <div class="form-group password">
                        <label class="sr-only" for="login-password">Password</label>
                        <span class="fa fa-lock icon"></span>
                        <asp:TextBox type="password" class="form-control login-password" ID="txtpwd" placeholder="Contraseña" runat="server" TextMode="Password" Style="text-transform: none"></asp:TextBox>
                        <p class="forgot-password">
                            <asp:HyperLink NavigateUrl="~/Identificar.aspx" runat="server">¿Olvidaste tu contraseña?</asp:HyperLink>
                        </p>
                    </div>
                    <div class="form-group">
                        <asp:CustomValidator ErrorMessage="" OnServerValidate="OnServerValidate" runat="server"  />
                        <cc1:CaptchaControl ID="Captcha1" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="4" CssClass="col-sm-2"
                            CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240" 
                            FontColor="#D20B0C" NoiseColor="#B1B1B1" />
                        <asp:TextBox class="form-control" ID="txtCaptcha" runat="server" style="text-transform: uppercase"></asp:TextBox>
                    </div>
                    <asp:Button CssClass="btn btn-block btn-primary" ID="btnLogin" OnClick="btnLogin_Click" runat="server" Text="Entrar"></asp:Button>
                    <asp:CheckBox runat="server" Text="Recordarme" AutoPostBack="False" onclick="DontCloseMenu()" Visible="False"/>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--<form>--%>
            <%--</form>--%>



            <p class="alt-path">¿No tienes una cuenta? <asp:HyperLink runat="server" NavigateUrl="~/Registro.aspx">Regístrate aquí</asp:HyperLink></p>  
        </div>
    </div>
</div>
