<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaPreticket.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.UcAltaPreticket" %>
<div style="height: 100%;">
    <asp:UpdatePanel runat="server" ID="updateAltaPreticket">
        <ContentTemplate>
            <header id="panelAlerta" runat="server" visible="false">
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
                            <ul>
                                <li><%# Container.DataItem %></li>
                            </ul>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </header>
            <div class="panel panel-primary" style="height: 400px;width: 930px">
                <div class="panel-heading">
                    Generar Notificación
                </div>
                <div class="panel-body" style="height: 305px;width: 930px">
                    <div class="form-horizontal">
                        <asp:HiddenField runat="server" ID="hfIdArbol" />
                        <asp:HiddenField runat="server" ID="hfIdUsuarioSolicito" />
                        <asp:HiddenField runat="server" ID="hfIdUsuarioLevanta" />
                        <div class="form-group">
                            <asp:Label runat="server" Text="Observaciones" CssClass="col-sm-2 control-label" />
                            <div class="col-sm-10">
                                <asp:TextBox TextMode="MultiLine" Rows="10" Columns="50" MaxLength="499" runat="server" ID="txtObservaciones" CssClass="form-control" onkeydown="return (event.keyCode!=13);" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer text-center">
                    <asp:Button runat="server" CssClass="btn btn-success" Text="Guardar" ID="btnGuardar" OnClick="btnGuardar_OnClick" />
                    <asp:Button runat="server" CssClass="btn btn-danger" Text="Limpiar" ID="btnLimpiar" OnClick="btnLimpiar_OnClick" />
                    <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="modalExito" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true" style="width: 930px; height: 360px;overflow-y: auto">
        <asp:UpdatePanel ID="upConfirmacion" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="panel panel-primary">
                            <div class="panel-heading">
                                <h1>Generación de Ticket</h1>
                            </div>
                            <div class="panel.panel-body">
                                <div class="form-group">
                                    <asp:Label runat="server" Text="Se ha generado correctamente el ticket No.:" />
                                    <asp:TextBox runat="server" ID="lblNoTicket" CssClass="form-control" ReadOnly="True" />
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" Text="Con clave:" ID="lblDescRandom" />
                                    <asp:TextBox runat="server" ID="lblRandom" CssClass="form-control" ReadOnly="True" />
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button runat="server" CssClass="btn btn-danger" ID="btnCerrar" Text="Cerrar" OnClick="btnCerrar_OnClick" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
