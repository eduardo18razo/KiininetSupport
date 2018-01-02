<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcCambiarEstatusAsignacion.ascx.cs" Inherits="KiiniHelp.UserControls.Operacion.UcCambiarEstatusAsignacion" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfEsPropietario"/>
        <asp:HiddenField runat="server" ID="hfNivelAsignacion"/>
        <asp:HiddenField runat="server" ID="hfSubRolActual"/>
        <div class="modal-header">
            <asp:LinkButton CssClass="close" runat="server" OnClick="btnCancelar_OnClick" Text='&times' />
            <h2 class="modal-title" id="modal-new-ticket-label">
                <asp:Label runat="server" ID="lblBrandingModal" /></h2>
            <p class="text-center">
                <asp:Label runat="server" Text="Cambio de Asignación" />
            </p>
        </div>
        <div class="modal-body">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <asp:Label runat="server" Text="Ticket" CssClass="col-xs-3" />
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="lblIdticket" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" Text="Estatus Asignacion" CssClass="col-xs-3" />
                            <div class="col-xs-8">
                                <asp:DropDownList runat="server" ID="ddlEstatus" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlEstatus_OnSelectedIndexChanged" />
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label runat="server" Text="Comentarios" CssClass="col-xs-3" />
                            <div class="col-xs-8">
                                <asp:TextBox runat="server" ID="txtComentarios" CssClass="form-control" TextMode="MultiLine" Height="100px" />
                            </div>
                        </div>

                        <div class="form-group" runat="server" id="divUsuariosSupervisor" visible="False">
                            <asp:Label runat="server" Text="Usuarios supervisor" CssClass="col-xs-3" />
                            <div class="col-xs-8">
                                <asp:ListBox SelectionMode="Single" runat="server" ID="lstSupervisor" OnSelectedIndexChanged="lstSupervisor_OnSelectedIndexChanged" AutoPostBack="True" />
                            </div>
                        </div>

                        <div class="form-group" runat="server" id="divUsuariosNivel1" visible="False">
                            <asp:Label runat="server" Text="Usuarios primer nivel" CssClass="col-xs-3" />
                            <div class="col-xs-8">
                                <asp:ListBox SelectionMode="Single" runat="server" ID="lstUsuariosGrupoNivel1" OnSelectedIndexChanged="lstUsuariosGrupoNivel1_OnSelectedIndexChanged" AutoPostBack="True" />
                            </div>
                        </div>

                        <div class="form-group" runat="server" id="divUsuariosNivel2" visible="False">
                            <asp:Label runat="server" Text="Usuarios segundo nivel" CssClass="col-xs-3" />
                            <div class="col-xs-8">
                                <asp:ListBox SelectionMode="Single" runat="server" ID="lstUsuariosGrupoNivel2" OnSelectedIndexChanged="lstUsuariosGrupoNivel2_OnSelectedIndexChanged" AutoPostBack="True" />
                            </div>
                        </div>

                        <div class="form-group" runat="server" id="divUsuariosNivel3" visible="False">
                            <asp:Label runat="server" Text="Usuarios tercer nivel" CssClass="col-xs-3" />
                            <div class="col-xs-8">
                                <asp:ListBox SelectionMode="Single" runat="server" ID="lstUsuariosGrupoNivel3" OnSelectedIndexChanged="lstUsuariosGrupoNivel3_OnSelectedIndexChanged" AutoPostBack="True" />
                            </div>
                        </div>

                        <div class="form-group" runat="server" id="divUsuariosNivel4" visible="False">
                            <asp:Label runat="server" Text="Usuarios cuarto nivel" CssClass="col-xs-3" />
                            <div class="col-xs-8">
                                <asp:ListBox SelectionMode="Single" runat="server" ID="lstUsuariosGrupoNivel4" OnSelectedIndexChanged="lstUsuariosGrupoNivel4_OnSelectedIndexChanged" AutoPostBack="True" />
                            </div>
                        </div>
                    </div>
                    <p class="text-right margin-top-40">
                        <asp:Button ID="btnAceptar" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnAceptar_OnClick" />
                        <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCancelar_OnClick" Visible="False"/>
                    </p>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            $(function () {
                $('[id*=lstSupervisor]').multiselect({
                    includeSelectAllOption: false,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                });
            });
            $(function () {
                $('[id*=lstUsuariosGrupoNivel1]').multiselect({
                    includeSelectAllOption: false,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                });
            });
            $(function () {
                $('[id*=lstUsuariosGrupoNivel2]').multiselect({
                    includeSelectAllOption: false,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                });
            });
            $(function () {
                $('[id*=lstUsuariosGrupoNivel3]').multiselect({
                    includeSelectAllOption: false,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                });
            });
            $(function () {
                $('[id*=lstUsuariosGrupoNivel4]').multiselect({
                    includeSelectAllOption: false,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                });
            });
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function () {

                $('[id*=lstSupervisor]').multiselect({
                    includeSelectAllOption: false,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                });

                $('[id*=lstUsuariosGrupoNivel1]').multiselect({
                    includeSelectAllOption: false,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                });

                $('[id*=lstUsuariosGrupoNivel2]').multiselect({
                    includeSelectAllOption: false,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                });

                $('[id*=lstUsuariosGrupoNivel3]').multiselect({
                    includeSelectAllOption: false,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                });

                $('[id*=lstUsuariosGrupoNivel4]').multiselect({
                    includeSelectAllOption: false,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                });


            });

        </script>
    </ContentTemplate>
</asp:UpdatePanel>
