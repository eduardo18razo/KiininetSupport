<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaDiasFestivos.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.UcAltaDiasFestivos" %>

<asp:UpdatePanel runat="server" ID="updateAltaAreas">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdDiaFeriado" Value="0" />
        <asp:HiddenField runat="server" ID="hdIdGrupoDias" Value="0" />
        <asp:HiddenField runat="server" ID="hfEsAlta" />
        <asp:HiddenField runat="server" ID="hfEditando" />
        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelar_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
            <h2 class="modal-title" id="modal-new-ticket-label">NUEVO DÍAS DESCANSO</h2>
            <hr class="bordercolor" />
        </div>

        <div class="modal-body">
            <div class="row center-block">
                <div class="row center-block center-content-div centered">
                    <div class="form-group margin-top">
                        Nombre del Nuevo grupo de Dias Feriados<br />
                    </div>
                    <div class="col-sm-6 col-lg-offset-3">
                        <asp:TextBox runat="server" ID="txtDescripcionDias" placeholder="DESCRIPCION" MaxLength="50" class="form-control col-sm-3" onkeydown="return (event.keyCode!=13);" />
                    </div>
                    <div class="clearfix"></div>
                    <br />
                    <br />
                    <div class="col-sm-8 col-lg-offset-2">
                        Agrega los dias feriados correspondientes a este catálogo:
                    </div>
                </div>
                <div class="bg-grey">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="form-group">
                                <asp:Label runat="server" Text="Dia Feriado" CssClass="control-label col-lg-12 col-md-12" />
                                <div class="col-lg-5 col-md-5">
                                    <asp:DropDownList runat="server" ID="ddlDiasFeriados" CssClass="form-control" />
                                </div>
                                <asp:Button runat="server" CssClass="btn btn-primary" Text="Seleccionar" ID="btnSeleccionar" OnClick="btnSeleccionar_OnClick" />
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" Text="Agregar un Día Feriado" CssClass="control-label col-lg-12 col-md-12" />
                                <div class="col-lg-5 col-md-5">
                                    <asp:TextBox ID="txtDescripcionDia" runat="server" MaxLength="50" CssClass="form-control" onkeydown="return (event.keyCode!=13);" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" Text="Fecha" CssClass="control-label col-lg-12 col-md-12" />
                                <div class="col-lg-5 col-md-5">
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" type="date" step="1"  onkeydown="return (event.keyCode!=13);"/>
                                </div>
                                <asp:LinkButton runat="server" CssClass="btn btn-primary fa fa-calendar " Visible="False"></asp:LinkButton>
                            </div>
                            <div class="form-group">
                                <asp:LinkButton runat="server" ID="btnAddDiaDescanso" class="fa fa-plus-circle" OnClick="btnAddDiaDescanso_OnClick"></asp:LinkButton>
                            </div>
                            <div class="clearfix"></div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <hr />
                <asp:Repeater runat="server" ID="rptDias" OnItemDataBound="rptDias_OnItemDataBound">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="row form-control" style="margin-top: 5px; height: 48px">
                            <asp:Label runat="server" ID="lblId" Text='<%# Eval("Id") %>' Visible="False" />
                            <asp:Label runat="server" Text='<%# Eval("Fecha", "{0:d}") %>' ID="lblFecha" CssClass="col-lg-2 col-md-2 col-sm-2" style="padding-left: 0"/>
                            <asp:Label runat="server" Text='<%# Eval("Descripcion") %>' ID="lblDescripcion" CssClass="col-lg-6 col-md-6 col-sm-6" />
                            <asp:LinkButton runat="server" Text="Editar" ID="lnkBtnEditar" CommandArgument='<%# Eval("Fecha") %>' OnClick="lnkBtnEditar_OnClick" />
                            <asp:Label runat="server" Text="|" ID="lblSeparador"></asp:Label>
                        <asp:LinkButton runat="server" Text="Borrar" ID="lbkBtnBorrar" CommandArgument='<%# Eval("Fecha") %>' OnClick="lbkBtnBorrar_OnClick" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <div class="clearfix"></div>
                <br />
                <div class="row text-right">
                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Guardar" ID="btnGuardar" OnClick="btnGuardar_OnClick" />
                </div>
            </div>
        </div>


        <%--<div class="modal-footer">
            
            <asp:Button ID="btnAceptar" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnAceptar_OnClick" />
            <asp:Button ID="btnLimpiar" runat="server" CssClass="btn btn-danger" Text="Limpiar" OnClick="btnLimpiar_OnClick" />
            <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCancelar_OnClick" />
        </div>
        --%>
    </ContentTemplate>
</asp:UpdatePanel>


