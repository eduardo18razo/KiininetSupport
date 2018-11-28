<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcLogIn.ascx.cs" Inherits="KiiniHelp.UserControls.UcLogIn" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<%--<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
<header class="" id="pnlAlerta" runat="server" visible="False">
    <div class="alert alert-danger">
        <div>
            <div class="float-left">
                <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
            </div>
            <div class="float-left">
                <h3>Error</h3>
            </div>
            <div class="clearfix clear-fix" />
        </div>
        <hr />
        <asp:Repeater runat="server" ID="rptErrorGeneral">
            <ItemTemplate>
                <%# Eval("Detalle")  %>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</header>
<div class="panel panel-primary">
    <div class="panel-heading text-primary text-center">
        <h3>Acceder a mi cuenta Kiininet</h3>
    </div>
    <div class="panel-body">
        <div class="form-horizontal" role="form">
            <div class="form-group">
                <label class="col-sm-2 control-label" for="txtUsuario">Usuario</label>
                <div class="col-sm-10">
                    <asp:TextBox CssClass="form-control obligatorio text-no-transform" ID="txtUsuario" placeholder="UserName" runat="server" autofocus="autofocus"/>
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-2 control-label" for="txtpwd">Contraseña</label>
                <div class="col-sm-10">
                    <asp:TextBox CssClass="form-control obligatorio text-no-transform" ID="txtpwd" placeholder="Enter Password" runat="server" TextMode="Password" autocomplete="off"/>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-2 control-label"></div>
                <div class="col-sm-10">
                    <asp:LinkButton class="btn btn-primary" ID="lnkBtnRecuperar" runat="server" Text="¿Olvidaste tu contraseña?" OnClick="lnkBtnRecuperar_OnClick"></asp:LinkButton>
                </div>
            </div>
            <div class="form-group">
                <asp:CustomValidator ErrorMessage="" OnServerValidate="OnServerValidate" runat="server" />
                <cc1:CaptchaControl ID="Captcha1" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="4" CssClass="col-sm-2"
                    CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                    FontColor="#D20B0C" NoiseColor="#B1B1B1" />
            </div>
            <div class="form-group">
                <div class="col-sm-3">
                    <asp:TextBox class="form-control obligatorio" ID="txtCaptcha" runat="server" autocomplete="off"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
    <div>
        <div class="panel-footer text-center">
            <asp:Button CssClass="btn btn-success" ID="btnLogin" OnClick="btnLogin_Click" runat="server" Text="Acceder"></asp:Button>
            <asp:Button CssClass="btn btn-danger" ID="btnCacelar" OnClick="btnCacelar_OnClick" runat="server" Text="Cancelar"></asp:Button>
        </div>
    </div>
</div>
<%--</ContentTemplate>
</asp:UpdatePanel>--%>
