<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTest.aspx.cs" ValidateRequest="false" Inherits="KiiniHelp.Test.FrmTest" Culture="Auto" UICulture="Auto" %>

<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=16.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel='stylesheet' href="/assets/css/font.css" />
    <link rel="stylesheet" href="/assets/css/font-awesome.css" />
    <link rel="stylesheet" href="/assets/css/pe-7-icons.css" />
    <link rel="stylesheet" href="/assets/css/pe-7-icons.css" />
    <link rel="stylesheet" href="/assets/css/bootstrap.css" />
    <link rel="stylesheet" href="/assets/css/styles.css" />
    <link rel="stylesheet" href="/assets/css/menuStyle.css" />
    <link rel="stylesheet" href="/assets/css/divs.css" />
    <link rel="stylesheet" href="/assets/css/checkBox.css" />
    <link rel="stylesheet" href="/assets/tmp/chosen.css" />
    <link rel="stylesheet" href="/assets/css/sumoselect.css" />
    <link rel="stylesheet" href="/assets/tmp/editor/ui/trumbowyg.css" />
    <link rel="stylesheet" href="/assets/tmp/editor/ui/trumbowyg.min.css" />
    <link rel="stylesheet" href="/assets/css/controls.css" />
    <link rel="stylesheet" href="/assets/tmp/jquery.tagsinput.min.css" />
    <style>
        option:hover {
            background: #C6C4BD;
        }

        .styleCalendar {
            left: 0 !important;
            z-index: 1000;
            background: #ffffff;
        }
    </style>
    <script>


        //function ShowLanding() {
        //    var landing = document.getElementById('updateProgress');
        //    if (landing != undefined) {
        //        landing.style.display = 'block';
        //    }
        //}

        //function HideLanding() {
        //    var landing = document.getElementById('updateProgress');
        //    if (landing != undefined) {
        //        landing.style.display = 'none';
        //    }
        //}
        //function ClientSideClick(myButton) {
        //    debugger;
        //    // Client side validation
        //    if (typeof (Page_ClientValidate) == 'function') {
        //        if (Page_ClientValidate() == false)
        //        { return false; }
        //    }

        //    //make sure the button is not of type "submit" but "button"
        //    if (myButton.getAttribute('type') == 'submit') {
        //        // disable the button
        //        myButton.disabled = true;
        //        myButton.className = "btn-inactive";
        //        myButton.value = "processing...";
        //    }
        //    return true;
        //}
    </script>
</head>
<body class="preload" style="background: #fff">
    <div id="full">
        <form id="form1" runat="server" enctype="multipart/form-data">
            <asp:ScriptManager runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
                <Scripts>
                    <asp:ScriptReference Path="~/assets/js/jquery.js" />
                    <asp:ScriptReference Path="~/assets/js/bootstrap.js" />
                    <asp:ScriptReference Path="~/assets/js/imagesloaded.js" />
                    <asp:ScriptReference Path="~/assets/js/masonry.js" />
                    <asp:ScriptReference Path="~/assets/js/main.js" />
                    <asp:ScriptReference Path="~/assets/js/modernizr.custom.js" />
                    <asp:ScriptReference Path="~/assets/js/pmenu.js" />
                    <asp:ScriptReference Path="~/assets/js/bootstrap-notify.js" />
                    <asp:ScriptReference Path="~/assets/js/bootstrap-notify.min.js" />
                    <asp:ScriptReference Path="~/assets/js/Notificaciones.js" />
                    <asp:ScriptReference Path="~/assets/tmp/chosen.jquery.js" />
                    <asp:ScriptReference Path="~/assets/tmp/editor/trumbowyg.min.js" />
                    <asp:ScriptReference Path="~/assets/tmp/editor/plugins/base64/trumbowyg.base64.min.js" />
                    <asp:ScriptReference Path="~/assets/tmp/editor/plugins/upload/trumbowyg.upload.min.js" />
                    <asp:ScriptReference Path="~/assets/tmp/jquery.tagsinput.min.js" />
                    <asp:ScriptReference Path="~/assets/js/jquery.sumoselect.min.js" />
                    <asp:ScriptReference Path="~/assets/js/validation.js" />
                </Scripts>
            </asp:ScriptManager>
            <script type="text/javascript">
                Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
                function BeginRequestHandler(sender, args) {
                    var oControl = args.get_postBackElement();
                    oControl.disabled = true;
                }
            </script>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="form-horizontal col-lg-6 col-lg-offset-3">
                        <div class="form-group">
                            <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" required />
                            <ajax:MaskedEditExtender ID="MaskedEditExtender2" runat="server"
                                TargetControlID="TextBox2"
                                Mask="9,999,999.99"
                                MessageValidatorTip="true"
                                OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError"
                                MaskType="Number"
                                InputDirection="LeftToRight"
                                AcceptNegative="None"
                                DisplayMoney="None"
                                ErrorTooltipEnabled="True" />
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" required />
                            <ajax:MaskedEditExtender ID="meeDate" runat="server"
                                TargetControlID="txtFecha"
                                Mask="99/99/9999"
                                UserDateFormat="DayMonthYear"
                                MessageValidatorTip="true"
                                OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError"
                                MaskType="Date"
                                InputDirection="LeftToRight"
                                AcceptNegative="None"
                                DisplayMoney="None"
                                ErrorTooltipEnabled="True" />
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" required MaxLength="10"></asp:TextBox>
                            <asp:ImageButton ID="imgPopup" ImageUrl="~/assets/images/like_S1.png" ImageAlign="Bottom"
                                runat="server" />
                            <ajax:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" CssClass="styleCalendar" TargetControlID="txtDOB" Format="dd/MM/yyyy" />
                        </div>
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtValorMaximo" required minlength="10" MaxLength="20" CssClass="form-control" Style="text-transform: none" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                        </div>
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="TextBox1" required type="number" min="1" max="2147483646" step="any" CssClass="form-control" Style="text-transform: none" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                        </div>
                        <div class="form-group">
                            <asp:CheckBox runat="server" Text="Sin Validación" AutoPostBack="True" ID="chkValidation" />
                        </div>
                        <div class="form-group">
                            <asp:Button Text="enter" ID="btnPrueba" runat="server" CssClass="btn btn-primary" OnClick="OnClick1" />
                        </div>
                        <div class="form-group">
                            <asp:Button Text="boton 2" runat="server" CssClass="btn btn-primary" OnClick="OnClick1" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>
