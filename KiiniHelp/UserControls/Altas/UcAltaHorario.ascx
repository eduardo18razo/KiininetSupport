<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaHorario.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.UcAltaHorario" %>

<script>
    $(document).ready(function () {
        SetTable();
    });
    function SetTable() {
        var esalta = (document.getElementById("<%= FindControl("hfEsAlta").ClientID %>").value == 'True');
        if (esalta) {

            var isMouseDown = false,
                isHighlighted;
            $("#our_table tbody td")
                .mousedown(function() {
                    isMouseDown = true;
                    $(this).toggleClass("highlighted");
                    isHighlighted = $(this).hasClass("highlighted");
                    getSelectedHora();
                    return false; // prevent text selection
                })
                .mouseover(function() {
                    if (isMouseDown) {
                        $(this).toggleClass("highlighted", isHighlighted);
                        getSelectedHora();
                    }
                })
                .bind("selectstart", function() {
                    return false;
                });

            $(document)
                .mouseup(function() {
                    isMouseDown = false;
                });
            $("#our_table").addClass("disabled");
        } else {
            $("#our_table tbody td").unbind('mousemove');
            $("#our_table tbody td").unbind('mouseover');
        }
        SetSelection();
    }
    function SetSelection() {
        var dayLun = document.getElementById("<%= FindControl("hfLunes").ClientID %>").value.split(',');
        var dayMar = document.getElementById("<%= FindControl("hfMartes").ClientID %>").value.split(',');
        var dayMie = document.getElementById("<%= FindControl("hfMiercoles").ClientID %>").value.split(',');
        var dayJue = document.getElementById("<%= FindControl("hfJueves").ClientID %>").value.split(',');
        var dayVie = document.getElementById("<%= FindControl("hfViernes").ClientID %>").value.split(',');
        var daySab = document.getElementById("<%= FindControl("hfSabado").ClientID %>").value.split(',');
        var dayDom = document.getElementById("<%= FindControl("hfDomingo").ClientID %>").value.split(',');
        for (var lun = 0; lun <= dayLun.length - 1; lun++) {
            $("#lun" + dayLun[lun]).addClass("highlighted");
        }
        for (var mar = 0; mar <= dayMar.length - 1; mar++) {
            $("#mar" + dayMar[mar]).addClass("highlighted");
        }
        for (var mie = 0; mie <= dayMie.length - 1; mie++) {
            $("#mie" + dayMie[mie]).addClass("highlighted");
        }
        for (var jue = 0; jue <= dayJue.length - 1; jue++) {
            $("#jue" + dayJue[jue]).addClass("highlighted");
        }
        for (var vie = 0; vie <= dayVie.length - 1; vie++) {
            $("#vie" + dayVie[vie]).addClass("highlighted");
        }
        for (var sab = 0; sab <= daySab.length - 1; sab++) {
            $("#sab" + daySab[sab]).addClass("highlighted");
        }
        for (var dom = 0; dom <= dayDom.length - 1; dom++) {
            $("#dom" + dayDom[dom]).addClass("highlighted");
        }
        var esalta = (document.getElementById("<%= FindControl("hfEsAlta").ClientID %>").value == 'true');
        if (!esalta) {
            $("#our_table").addClass("disabled");
        }

    }
    function getSelectedHora() {
        var lunes = [];
        var martes = [];
        var miercoles = [];
        var jueves = [];
        var viernes = [];
        var sabado = [];
        var domingo = [];
        $("#our_table tbody td.highlighted").each(function () {
            if ($(this).hasClass('highlighted')) {
                var id = $(this).attr("id");
                if (id === undefined || id === null) return;
                var dia = id.substring(0, 3);
                var hora = id.substring(3);
                switch (dia) {
                    case "lun":
                        lunes.push(parseInt(hora));
                        break;
                    case "mar":
                        martes.push(parseInt(hora));
                        break;
                    case "mie":
                        miercoles.push(parseInt(hora));
                        break;
                    case "jue":
                        jueves.push(parseInt(hora));
                        break;
                    case "vie":
                        viernes.push(parseInt(hora));
                        break;
                    case "sab":
                        sabado.push(parseInt(hora));
                        break;
                    case "dom":
                        domingo.push(parseInt(hora));
                        break;
                    default:
                }

            }
        });
        lunes.sort(function (a, b) { return a - b; });
        martes.sort(function (a, b) { return a - b; });
        miercoles.sort(function (a, b) { return a - b; });
        jueves.sort(function (a, b) { return a - b; });
        viernes.sort(function (a
            , b) { return a - b; });
        sabado.sort(function (a, b) { return a - b; });
        domingo.sort(function (a, b) { return a - b; });

        var dayLun = document.getElementById("<%= FindControl("hfLunes").ClientID %>");
        var dayMar = document.getElementById("<%= FindControl("hfMartes").ClientID %>");
        var dayMie = document.getElementById("<%= FindControl("hfMiercoles").ClientID %>");
        var dayJue = document.getElementById("<%= FindControl("hfJueves").ClientID %>");
        var dayVie = document.getElementById("<%= FindControl("hfViernes").ClientID %>");
        var daySab = document.getElementById("<%= FindControl("hfSabado").ClientID %>");
        var dayDom = document.getElementById("<%= FindControl("hfDomingo").ClientID %>");
        dayLun.value = lunes;
        dayMar.value = martes;
        dayMie.value = miercoles;
        dayJue.value = jueves;
        dayVie.value = viernes;
        daySab.value = sabado;
        dayDom.value = domingo;
    }
</script>
<style>
    .tdHorario {
        height: 25px;
        width: 20px;
        text-align: center;
        vertical-align: middle;
        background-color: #fff;
        border: 2px solid #ecf0f1;
    }

    .no-border {
        border: none !important;
    }

    .highlighted {
       background-color: #3FA9F5;
    }

    .transparente {
        background: transparent;
        border: none !important;
    }

    .headertd {
        background: transparent;
        border: none !important;
    }

    .footerHorario {
        background: transparent;
        height: 40px;
        width: 1px;
        border: none !important;
    }

    .footerLabel {
        transform: rotate(-80deg);
        background: transparent;
        border: none !important;
        width: 1px;
        position: absolute;
        margin-top: 8px;
    }
</style>
<asp:UpdatePanel runat="server" ID="updateAltaAreas">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hdIdHorario" />
        <asp:HiddenField runat="server" ID="hfEsAlta" />
        <asp:HiddenField runat="server" ID="hfLunes" />
        <asp:HiddenField runat="server" ID="hfMartes" />
        <asp:HiddenField runat="server" ID="hfMiercoles" />
        <asp:HiddenField runat="server" ID="hfJueves" />
        <asp:HiddenField runat="server" ID="hfViernes" />
        <asp:HiddenField runat="server" ID="hfSabado" />
        <asp:HiddenField runat="server" ID="hfDomingo" />
        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelar_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
            <h6 class="modal-title" id="modal-new-ticket-label">
                <asp:Label runat="server" ID="lbltitulo" />
            </h6>
        </div>
        <div class="modal-body">
            <div class="row">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row center-block">
                            <div class="row"> <%--center-block center-content-div centered--%>
                                <div class="form-group col-sm-10 margin-top-10">
                                    Ingresa el nombre para el Horario<br />
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control col-sm-3" MaxLength="50" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                    </div>
                                </div>
                                <br />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <label>Selecciona las horas laborales por cada día de la semana.</label>
                 <br />
                <div>
                    <asp:UpdatePanel runat="server" ID="upTableHorario" UpdateMode="Conditional">
                        <ContentTemplate>

                            
                            <table class="table table-responsive disabled" id="our_table" style="background: transparent">
                                <tbody>
                                    <tr id="row1">
                                        <td class="headertd">Lun</td>
                                        <td class="tdHorario" id="lun0"></td>
                                        <td class="tdHorario" id="lun1"></td>
                                        <td class="tdHorario" id="lun2"></td>
                                        <td class="tdHorario" id="lun3"></td>
                                        <td class="tdHorario" id="lun4"></td>
                                        <td class="tdHorario" id="lun5"></td>
                                        <td class="tdHorario" id="lun6"></td>
                                        <td class="tdHorario" id="lun7"></td>
                                        <td class="tdHorario" id="lun8"></td>
                                        <td class="tdHorario" id="lun9"></td>
                                        <td class="tdHorario" id="lun10"></td>
                                        <td class="tdHorario" id="lun11"></td>
                                        <td class="tdHorario" id="lun12"></td>
                                        <td class="tdHorario" id="lun13"></td>
                                        <td class="tdHorario" id="lun14"></td>
                                        <td class="tdHorario" id="lun15"></td>
                                        <td class="tdHorario" id="lun16"></td>
                                        <td class="tdHorario" id="lun17"></td>
                                        <td class="tdHorario" id="lun18"></td>
                                        <td class="tdHorario" id="lun19"></td>
                                        <td class="tdHorario" id="lun20"></td>
                                        <td class="tdHorario" id="lun21"></td>
                                        <td class="tdHorario" id="lun22"></td>
                                        <td class="tdHorario" id="lun23"></td>
                                    </tr>
                                    <tr id="row2">
                                        <td class="headertd">Mar</td>
                                        <td class="tdHorario" id="mar0"></td>
                                        <td class="tdHorario" id="mar1"></td>
                                        <td class="tdHorario" id="mar2"></td>
                                        <td class="tdHorario" id="mar3"></td>
                                        <td class="tdHorario" id="mar4"></td>
                                        <td class="tdHorario" id="mar5"></td>
                                        <td class="tdHorario" id="mar6"></td>
                                        <td class="tdHorario" id="mar7"></td>
                                        <td class="tdHorario" id="mar8"></td>
                                        <td class="tdHorario" id="mar9"></td>
                                        <td class="tdHorario" id="mar10"></td>
                                        <td class="tdHorario" id="mar11"></td>
                                        <td class="tdHorario" id="mar12"></td>
                                        <td class="tdHorario" id="mar13"></td>
                                        <td class="tdHorario" id="mar14"></td>
                                        <td class="tdHorario" id="mar15"></td>
                                        <td class="tdHorario" id="mar16"></td>
                                        <td class="tdHorario" id="mar17"></td>
                                        <td class="tdHorario" id="mar18"></td>
                                        <td class="tdHorario" id="mar19"></td>
                                        <td class="tdHorario" id="mar20"></td>
                                        <td class="tdHorario" id="mar21"></td>
                                        <td class="tdHorario" id="mar22"></td>
                                        <td class="tdHorario" id="mar23"></td>
                                    </tr>
                                    <tr id="row3">
                                        <td class="headertd">Mie</td>
                                        <td class="tdHorario" id="mie0"></td>
                                        <td class="tdHorario" id="mie1"></td>
                                        <td class="tdHorario" id="mie2"></td>
                                        <td class="tdHorario" id="mie3"></td>
                                        <td class="tdHorario" id="mie4"></td>
                                        <td class="tdHorario" id="mie5"></td>
                                        <td class="tdHorario" id="mie6"></td>
                                        <td class="tdHorario" id="mie7"></td>
                                        <td class="tdHorario" id="mie8"></td>
                                        <td class="tdHorario" id="mie9"></td>
                                        <td class="tdHorario" id="mie10"></td>
                                        <td class="tdHorario" id="mie11"></td>
                                        <td class="tdHorario" id="mie12"></td>
                                        <td class="tdHorario" id="mie13"></td>
                                        <td class="tdHorario" id="mie14"></td>
                                        <td class="tdHorario" id="mie15"></td>
                                        <td class="tdHorario" id="mie16"></td>
                                        <td class="tdHorario" id="mie17"></td>
                                        <td class="tdHorario" id="mie18"></td>
                                        <td class="tdHorario" id="mie19"></td>
                                        <td class="tdHorario" id="mie20"></td>
                                        <td class="tdHorario" id="mie21"></td>
                                        <td class="tdHorario" id="mie22"></td>
                                        <td class="tdHorario" id="mie23"></td>
                                    </tr>
                                    <tr id="row4">
                                        <td class="headertd">Jue</td>
                                        <td class="tdHorario" id="jue0"></td>
                                        <td class="tdHorario" id="jue1"></td>
                                        <td class="tdHorario" id="jue2"></td>
                                        <td class="tdHorario" id="jue3"></td>
                                        <td class="tdHorario" id="jue4"></td>
                                        <td class="tdHorario" id="jue5"></td>
                                        <td class="tdHorario" id="jue6"></td>
                                        <td class="tdHorario" id="jue7"></td>
                                        <td class="tdHorario" id="jue8"></td>
                                        <td class="tdHorario" id="jue9"></td>
                                        <td class="tdHorario" id="jue10"></td>
                                        <td class="tdHorario" id="jue11"></td>
                                        <td class="tdHorario" id="jue12"></td>
                                        <td class="tdHorario" id="jue13"></td>
                                        <td class="tdHorario" id="jue14"></td>
                                        <td class="tdHorario" id="jue15"></td>
                                        <td class="tdHorario" id="jue16"></td>
                                        <td class="tdHorario" id="jue17"></td>
                                        <td class="tdHorario" id="jue18"></td>
                                        <td class="tdHorario" id="jue19"></td>
                                        <td class="tdHorario" id="jue20"></td>
                                        <td class="tdHorario" id="jue21"></td>
                                        <td class="tdHorario" id="jue22"></td>
                                        <td class="tdHorario" id="jue23"></td>
                                    </tr>
                                    <tr id="row5">
                                        <td class="headertd">Vie</td>
                                        <td class="tdHorario" id="vie0"></td>
                                        <td class="tdHorario" id="vie1"></td>
                                        <td class="tdHorario" id="vie2"></td>
                                        <td class="tdHorario" id="vie3"></td>
                                        <td class="tdHorario" id="vie4"></td>
                                        <td class="tdHorario" id="vie5"></td>
                                        <td class="tdHorario" id="vie6"></td>
                                        <td class="tdHorario" id="vie7"></td>
                                        <td class="tdHorario" id="vie8"></td>
                                        <td class="tdHorario" id="vie9"></td>
                                        <td class="tdHorario" id="vie10"></td>
                                        <td class="tdHorario" id="vie11"></td>
                                        <td class="tdHorario" id="vie12"></td>
                                        <td class="tdHorario" id="vie13"></td>
                                        <td class="tdHorario" id="vie14"></td>
                                        <td class="tdHorario" id="vie15"></td>
                                        <td class="tdHorario" id="vie16"></td>
                                        <td class="tdHorario" id="vie17"></td>
                                        <td class="tdHorario" id="vie18"></td>
                                        <td class="tdHorario" id="vie19"></td>
                                        <td class="tdHorario" id="vie20"></td>
                                        <td class="tdHorario" id="vie21"></td>
                                        <td class="tdHorario" id="vie22"></td>
                                        <td class="tdHorario" id="vie23"></td>
                                    </tr>
                                    <tr id="row6">
                                        <td class="headertd">Sab</td>
                                        <td class="tdHorario" id="sab0"></td>
                                        <td class="tdHorario" id="sab1"></td>
                                        <td class="tdHorario" id="sab2"></td>
                                        <td class="tdHorario" id="sab3"></td>
                                        <td class="tdHorario" id="sab4"></td>
                                        <td class="tdHorario" id="sab5"></td>
                                        <td class="tdHorario" id="sab6"></td>
                                        <td class="tdHorario" id="sab7"></td>
                                        <td class="tdHorario" id="sab8"></td>
                                        <td class="tdHorario" id="sab9"></td>
                                        <td class="tdHorario" id="sab10"></td>
                                        <td class="tdHorario" id="sab11"></td>
                                        <td class="tdHorario" id="sab12"></td>
                                        <td class="tdHorario" id="sab13"></td>
                                        <td class="tdHorario" id="sab14"></td>
                                        <td class="tdHorario" id="sab15"></td>
                                        <td class="tdHorario" id="sab16"></td>
                                        <td class="tdHorario" id="sab17"></td>
                                        <td class="tdHorario" id="sab18"></td>
                                        <td class="tdHorario" id="sab19"></td>
                                        <td class="tdHorario" id="sab20"></td>
                                        <td class="tdHorario" id="sab21"></td>
                                        <td class="tdHorario" id="sab22"></td>
                                        <td class="tdHorario" id="sab23"></td>
                                    </tr>
                                    <tr id="row0">
                                        <td class="headertd">Dom</td>
                                        <td class="tdHorario" id="dom0"></td>
                                        <td class="tdHorario" id="dom1"></td>
                                        <td class="tdHorario" id="dom2"></td>
                                        <td class="tdHorario" id="dom3"></td>
                                        <td class="tdHorario" id="dom4"></td>
                                        <td class="tdHorario" id="dom5"></td>
                                        <td class="tdHorario" id="dom6"></td>
                                        <td class="tdHorario" id="dom7"></td>
                                        <td class="tdHorario" id="dom8"></td>
                                        <td class="tdHorario" id="dom9"></td>
                                        <td class="tdHorario" id="dom10"></td>
                                        <td class="tdHorario" id="dom11"></td>
                                        <td class="tdHorario" id="dom12"></td>
                                        <td class="tdHorario" id="dom13"></td>
                                        <td class="tdHorario" id="dom14"></td>
                                        <td class="tdHorario" id="dom15"></td>
                                        <td class="tdHorario" id="dom16"></td>
                                        <td class="tdHorario" id="dom17"></td>
                                        <td class="tdHorario" id="dom18"></td>
                                        <td class="tdHorario" id="dom19"></td>
                                        <td class="tdHorario" id="dom20"></td>
                                        <td class="tdHorario" id="dom21"></td>
                                        <td class="tdHorario" id="dom22"></td>
                                        <td class="tdHorario" id="dom23"></td>
                                    </tr>
                                </tbody>
                                <tfoot>
                                    <tr class="footerHorario">
                                        <td class="transparente"></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">00:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">01:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">02:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">03:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">04:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">05:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">06:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">07:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">08:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">09:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">10:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">11:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">12:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">13:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">14:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">15:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">16:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">17:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">18:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">19:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">20:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">21:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">22:00</span></td>
                                        <td class="tdHorario no-border" style="padding-top: 20px; background: transparent"><span class="footerLabel">23:00</span></td>
                                    </tr>
                                </tfoot>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <hr />
                </div>
            </div>
            <%--<asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Repeater runat="server" ID="rptHorarios">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="row form-control" style="margin-top: 5px; height: 48px">
                                <asp:Label runat="server" ID="lblId" Text='<%# Eval("Id") %>' Visible="False" />
                                <asp:Label runat="server" ID="lblIdHorario" Text='<%# Eval("IdHorario") %>' Visible="False" />
                                <asp:Label runat="server" ID="lblDia" Text='<%# Eval("Dia") %>' Visible="False" />
                                <asp:Label runat="server" Text='<%# (int)Eval("Dia") == 1 ? "Lunes" : (int)Eval("Dia") == 2 ? "MARTES" : (int)Eval("Dia") == 3 ? "MIERCOLES" : (int)Eval("Dia") == 4 ? "JUEVES" : (int)Eval("Dia") == 5 ? "VIERNES" : (int)Eval("Dia") == 6 ? "SABADO" : "DOMINGO"%>' CssClass="col-sm-4" />
                                <asp:Label runat="server" ID="lblHoraInicio" Text='<%# Eval("HoraInicio") %>' CssClass="col-sm-2" />
                                <asp:Label runat="server" Text=" - " CssClass="col-sm-1"></asp:Label>
                                <asp:Label runat="server" ID="lblHoraFin" Text='<%# Eval("HoraFin") %>' CssClass="col-sm-2" />
                                <asp:Button runat="server" class="btn btn-danger col-sm-2 glyphicon-remove" CommandArgument='<%# Eval("Dia") %>' CommandName='<%# Eval("HoraInicio") %>' Text="Eliminar" ID="btnEliminar" OnClick="btnEliminar_OnClick" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </ContentTemplate>
            </asp:UpdatePanel>--%>
            <br />
            <div class="row text-right" style="padding-bottom: 20px;">
                <asp:Button runat="server" ID="btnAgregar" Text="Guardar" CssClass="btn btn-primary" OnClick="btnAceptar_OnClick" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
