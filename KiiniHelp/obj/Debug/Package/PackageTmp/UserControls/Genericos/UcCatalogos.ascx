<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcCatalogos.ascx.cs" Inherits="KiiniHelp.UserControls.Genericos.UcCatalogos" %>
<asp:UpdatePanel ID="upCatlogos" runat="server">
    <ContentTemplate>
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label runat="server" ID="lblTitleCatalogo"></asp:Label></h4>
                    <div class="alert alert-warning" id="panelAlertaCatalogo" runat="server" visible="false">
                        <div>
                            <div class="float-left">
                                <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                            </div>
                            <div class="float-left">
                                <h3>Error</h3>
                            </div>
                            <div class="clearfix clear-fix" />
                        </div>
                        <asp:Repeater runat="server" ID="rptErrorCatalogo">
                            <ItemTemplate>
                                <div class="row">
                                    <ul>
                                        <li><%# Container.DataItem %></li>
                                    </ul>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div class="modal-body">
                    <div>
                        <asp:HiddenField runat="server" ID="hfCatalogo" />
                        <div>
                            <div class="form-group">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Tipo de Usuario</label>
                                    <asp:DropDownList runat="server" ID="ddlTipoUsuarioCatalogo" CssClass="DropSelect" />
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Descripcion</label>
                                    <asp:TextBox runat="server" ID="txtDescripcionCatalogo" placeholder="DESCRIPCION" class="form-control" />
                                </div>
                                <div class="form-group">
                                    <asp:CheckBox runat="server" ID="chkHabilitado" Checked="True" Visible="False" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnGuardarCatalogo" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnGuardarCatalogo_OnClick" />
                    <asp:Button ID="btnCancelarCatalogo" runat="server" CssClass="btn btn-danger" Text="Cancelar" data-dismiss="modal" OnClick="btnCancelarCatalogo_OnClick" />
                </div>
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnCancelarCatalogo" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
