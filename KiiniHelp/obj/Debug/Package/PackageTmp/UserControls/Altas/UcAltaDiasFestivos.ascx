<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaDiasFestivos.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.UcAltaDiasFestivos" %>
<%@ Register TagPrefix="ajx" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=16.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:UpdatePanel runat="server" ID="updateAltaAreas">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdDiaFeriado" Value="0" />
        <asp:HiddenField runat="server" ID="hdIdGrupoDias" Value="0" />
        <asp:HiddenField runat="server" ID="hfEsAlta" />
        <asp:HiddenField runat="server" ID="hfEditando" />
        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelar_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
            <h6 class="modal-title" id="modal-new-ticket-label">Nuevo Día Feriado</h6>
            <%--<hr class="bordercolor" />--%>
        </div>

        <div class="modal-body">
            <div class="row">
                <div class="row">
                    <div class="form-group col-sm-12 margin-top">
                        Nombre del Nuevo grupo de Dias Feriados<br />
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtDescripcionDias" MaxLength="50" class="form-control col-sm-3" onkeydown="return (event.keyCode!=13);" />
                        </div>
                    </div>
                   
                    <br />
                    <hr />
                    <div class="col-sm-12">
                         <hr />
                        Agrega los dias feriados correspondientes a este catálogo:
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
                                <asp:Button runat="server" CssClass="btn btn-primary" Style="margin-top: 5px; margin-left: 5px;" Text="Seleccionar" ID="btnSeleccionar" OnClick="btnSeleccionar_OnClick" />
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" Text="Agregar un Día Feriado" CssClass="control-label col-lg-12 col-md-12" />
                                <div class="col-lg-7 col-md-7">
                                    <asp:TextBox ID="txtDescripcionDia" runat="server" MaxLength="50" CssClass="form-control" onkeydown="return (event.keyCode!=13);" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" Text="Fecha" CssClass="control-label col-lg-12 col-md-12" />
                                <div class="col-lg-7 col-md-7">
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);" />
                                    <ajx:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDate" Format="dd/MM/yyyy"> </ajx:CalendarExtender>   
                                </div>
                                <asp:LinkButton runat="server" CssClass="btn btn-primary fa fa-calendar " Visible="False"></asp:LinkButton>
                            </div>
                            <div class="form-group">
                                <asp:LinkButton runat="server" ID="btnAddDiaDescanso" class="fa fa-plus-circle" Style="margin-top: 5px; margin-left: 5px;" OnClick="btnAddDiaDescanso_OnClick"></asp:LinkButton>
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
                        <div class="row form-control" style="margin-top: 20px;">
                            <asp:Label runat="server" ID="lblId" Text='<%# Eval("Id") %>' Visible="False" />
                            <asp:Label runat="server" Text='<%# Eval("Fecha", "{0:d}") %>' ID="lblFecha" CssClass="col-lg-2 col-md-2 col-sm-2" Style="padding-left: 0" />
                            <asp:Label runat="server" Text='<%# Eval("Descripcion") %>' ID="lblDescripcion" CssClass="col-lg-6 col-md-6 col-sm-6" />
                            <asp:LinkButton runat="server" Text="Editar" ID="lnkBtnEditar" class="btn" CommandArgument='<%# Eval("Fecha") %>' OnClick="lnkBtnEditar_OnClick"><i class="fa fa-pencil-square-o" aria-hidden="true"></i>Editar</asp:LinkButton>
                            <asp:Label runat="server" Text="|" ID="lblSeparador"></asp:Label>
                            <asp:LinkButton runat="server" Text="Borrar" ID="lbkBtnBorrar" class="btn" CommandArgument='<%# Eval("Fecha") %>' OnClick="lbkBtnBorrar_OnClick"><i class="fa fa-times" aria-hidden="true"></i>Borrar</asp:LinkButton>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <div class="clearfix"></div>
                <br />
                <div class="row text-right padding-20-bottom"> <%--style="padding-bottom: 20px;"--%>
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


