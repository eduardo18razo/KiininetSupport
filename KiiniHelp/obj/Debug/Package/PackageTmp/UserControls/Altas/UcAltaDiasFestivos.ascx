<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaDiasFestivos.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.UcAltaDiasFestivos" %>
<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=16.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>




<asp:UpdatePanel runat="server" ID="updateAltaAreas">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdDiaFeriado" Value="0" />
        <asp:HiddenField runat="server" ID="hdIdGrupoDias" Value="0" />
        <asp:HiddenField runat="server" ID="hfEsAlta" />
        <asp:HiddenField runat="server" ID="hfEditando" />
        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelar_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
            <h6 class="modal-title" id="modal-new-ticket-label">Nuevo día feriado</h6>
        </div>

        <div class="modal-body">
            <div class="row">
                <div class="row">
                    <div class="form-group col-sm-12">
                        <label class="col-sm-12 col-md-12 col-lg-12 no-padding-left">Nombre del nuevo grupo de días feriados</label><br />
                        <div class="col-sm-8 no-padding-left">
                            <asp:TextBox runat="server" ID="txtDescripcionDias" MaxLength="50" class="form-control col-sm-3" onkeydown="return (event.keyCode!=13);" />
                        </div>
                    </div>

                    <br />
                    <hr />
                    <div class="col-sm-12">
                        <hr />
                        Selecciona los días feriados que integran el nuevo grupo:
                    </div>
                </div>
                <div>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="form-group">
                                <asp:Label runat="server" Text="Dia Feriado" CssClass="control-label col-lg-12 col-md-12" />
                                <div class="col-lg-7 col-md-7">
                                    <asp:DropDownList runat="server" ID="ddlDiasFeriados" CssClass="form-control" />
                                </div>
                                <asp:Button runat="server" CssClass="btn btn-primary margin-top-6 margin-left-5" Height="29px" Text="Seleccionar" ID="btnSeleccionar" OnClick="btnSeleccionar_OnClick" />
                            </div>
                            <br />
                            <hr />
                            <div class="form-group">
                                <asp:Label runat="server" Text="Agrega nuevo un día feriado al grupo:" CssClass="control-label col-lg-12 col-md-12" />
                                <asp:Label runat="server" Text="Descripción" CssClass="control-label col-lg-12 col-md-12" />
                                <div class="col-lg-7 col-md-7">
                                    <asp:TextBox ID="txtDescripcionDia" runat="server" MaxLength="50" CssClass="form-control" onkeydown="return (event.keyCode!=13);" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" Text="Fecha" CssClass="control-label col-lg-12 col-md-12" />

                                <div class="col-lg-7 col-md-7">
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" step="1" onkeydown="return (event.keyCode!=13);" />
                                </div>

                                <div class="col-lg-2 col-md-2">
                                    <i class="fa fa-calendar fa-20x margin-top-6" id="imgPopup2" runat="server"></i>
                                    <ajax:CalendarExtender runat="server" ID="ctrlCalendar" TargetControlID="txtDate" Format="dd/MM/yyyy" PopupButtonID="imgPopup2"/>
                                    <ajax:maskededitextender id="MaskedEditExtender1" runat="server"
                                        targetcontrolid="txtDate" mask="99/99/9999"
                                        masktype="Date" messagevalidatortip="true"
                                        onfocuscssclass="MaskedEditFocus"
                                        clearmaskonlostfocus="false" oninvalidcssclass="MaskedEditError"
                                        inputdirection="LeftToRight" />
                                </div>

                                <asp:LinkButton runat="server" CssClass="btn btn-primary fa fa-calendar " Visible="False"></asp:LinkButton>
                            </div>
                            <div class="form-group">
                                <asp:LinkButton runat="server" ID="btnAddDiaDescanso" class="fa fa-plus-circle margin-left-5 margin-top-7" OnClick="btnAddDiaDescanso_OnClick"></asp:LinkButton>
                            </div>
                            <div class="clearfix"></div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <hr />
                <asp:Label runat="server" Text="Días seleccionados del nuevo grupo de días feriados" CssClass="control-label col-lg-12 col-md-12" />
                <br />
                <asp:Repeater runat="server" ID="rptDias" OnItemDataBound="rptDias_OnItemDataBound">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="row form-control margin-left-3 margin-top-5">
                            <asp:Label runat="server" ID="lblId" Text='<%# Eval("Id") %>' Visible="False" />
                            <asp:Label runat="server" Text='<%# Eval("Fecha", "{0:d}") %>' ID="lblFecha" CssClass="col-lg-2 col-md-2 col-sm-2 col-xs-2" />
                            <asp:Label runat="server" Text='<%# Eval("Descripcion") %>' ID="lblDescripcion" CssClass="col-lg-7 col-md-7 col-sm-7 col-xs-7" />
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3 text-right">
                                <asp:LinkButton runat="server" Text="Editar" ID="lnkBtnEditar" class="btn lnkBtn" CommandArgument='<%# Eval("Fecha") %>' OnClick="lnkBtnEditar_OnClick"><i class="fa fa-pencil-square-o" aria-hidden="true"></i></asp:LinkButton>
                                <asp:Label runat="server" Text="|" ID="lblSeparador"></asp:Label>
                                <asp:LinkButton runat="server" Text="Borrar" ID="lbkBtnBorrar" class="btn lnkBtn" CommandArgument='<%# Eval("Fecha") %>' OnClick="lbkBtnBorrar_OnClick"><i class="fa fa-times" aria-hidden="true"></i></asp:LinkButton>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <div class="clearfix"></div>
                <br />
                <div class="row text-right padding-20-bottom">
                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Guardar" ID="btnGuardar" OnClick="btnGuardar_OnClick" />
                </div>
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>


