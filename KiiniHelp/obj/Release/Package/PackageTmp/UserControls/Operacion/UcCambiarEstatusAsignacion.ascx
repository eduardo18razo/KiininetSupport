<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcCambiarEstatusAsignacion.ascx.cs" Inherits="KiiniHelp.UserControls.Operacion.UcCambiarEstatusAsignacion" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <header id="panelAlertaGeneral" runat="server" visible="false">
            Cambiar Estatus...
            <div class="alert alert-danger">

                <div>
                    <div style="float: left">
                        <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                    </div>
                    <div style="float: left">
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
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3>Cambio de estatus</h3>
            </div>
            <div class="panel panel-body">
                <asp:Label runat="server" Text="Ticket" ID="lblEsPropietrio" CssClass="col-xs-3" Visible="False" />
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" Text="Ticket" CssClass="col-xs-3" />
                        <div class="col-xs-8">
                            <asp:Label runat="server" ID="lblIdticket" CssClass="form-control"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" Text="Estatus Asignacion" CssClass="col-xs-3" />
                        <div class="col-xs-8">
                            <asp:DropDownList runat="server" ID="ddlEstatus" CssClass="DropSelect" AutoPostBack="True" OnSelectedIndexChanged="ddlEstatus_OnSelectedIndexChanged" />
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" Text="Comentarios" CssClass="col-xs-3" />
                        <div class="col-xs-8">
                            <asp:TextBox runat="server" ID="txtComentarios" CssClass="form-control" TextMode="MultiLine" Height="100px" />
                        </div>
                    </div>

                    <div class="form-group" runat="server" id="divUsuariosSupervisor" visible="False">
                        <div class="panel panel-primary">
                            <div class="panel-heading" role="tab" id="headingSupervisor">
                                <div class="row collapsed panel-title" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseSupervisor" aria-expanded="false" aria-controls="collapseFive" style="cursor: pointer">
                                    <div class="col-xs-12 col-sm-12">Usuarios Supervisor</div>
                                </div>
                            </div>
                            <div id="collapseSupervisor" class="panel-collapse" role="tabpanel" aria-labelledby="headingSupervisor">
                                <div class="panel-body">
                                    <asp:RadioButtonList runat="server" SelectionMode="Single" ID="rbtnlSupervisor" OnSelectedIndexChanged="rbtnlSupervisor_OnSelectedIndexChanged" AutoPostBack="True" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group" runat="server" id="divUsuariosNivel1" visible="False">
                        <div class="panel panel-primary">
                            <div class="panel-heading" role="tab" id="headingNivel1">
                                <div class="row collapsed panel-title" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseNivel1" aria-expanded="false" aria-controls="collapseOne" style="cursor: pointer">
                                    <div class="col-xs-12 col-sm-12">Usuarios Primer Nivel</div>
                                </div>
                            </div>
                            <div id="collapseNivel1" class="panel-collapse" role="tabpanel" aria-labelledby="headingNivel1">
                                <div class="panel-body">
                                    <asp:RadioButtonList runat="server" SelectionMode="Single" ID="rbtnlUsuariosGrupoNivel1" OnSelectedIndexChanged="rbtnlUsuariosGrupoNivel1_OnSelectedIndexChanged" AutoPostBack="True" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group" runat="server" id="divUsuariosNivel2" visible="False">
                        <div class="panel panel-primary">
                            <div class="panel-heading" role="tab" id="headingNivel2">
                                <div class="row collapsed panel-title" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseNivel2" aria-expanded="false" aria-controls="collapseTwo" style="cursor: pointer">
                                    <div class="col-xs-12 col-sm-12">Usuarios Segundo Nivel</div>
                                </div>
                            </div>
                            <div id="collapseNivel2" class="panel-collapse" role="tabpanel" aria-labelledby="headingNivel2">
                                <div class="panel-body">
                                    <asp:RadioButtonList runat="server" SelectionMode="Single" ID="rbtnlUsuariosGrupoNivel2" OnSelectedIndexChanged="rbtnlUsuariosGrupoNivel2_OnSelectedIndexChanged" AutoPostBack="True" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group" runat="server" id="divUsuariosNivel3" visible="False">
                        <div class="panel panel-primary">
                            <div class="panel-heading" role="tab" id="headingNivel3">
                                <div class="row collapsed panel-title" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseNivel3" aria-expanded="false" aria-controls="collapseThree" style="cursor: pointer">
                                    <div class="col-xs-12 col-sm-12">Usuarios Tercer Nivel</div>
                                </div>
                            </div>
                            <div id="collapseNivel3" class="panel-collapse" role="tabpanel" aria-labelledby="headingNivel3">
                                <div class="panel-body">
                                    <asp:RadioButtonList runat="server" SelectionMode="Single" ID="rbtnlUsuariosGrupoNivel3" OnSelectedIndexChanged="rbtnlUsuariosGrupoNivel3_OnSelectedIndexChanged" AutoPostBack="True" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group" runat="server" id="divUsuariosNivel4" visible="False">
                        <div class="panel panel-primary">
                            <div class="panel-heading" role="tab" id="headingNivel4">
                                <div class="row collapsed panel-title" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseNivel4" aria-expanded="false" aria-controls="collapseFour" style="cursor: pointer">
                                    <div class="col-xs-12 col-sm-12">Usuarios Cuarto Nivel</div>
                                </div>
                            </div>
                            <div id="collapseNivel4" class="panel-collapse" role="tabpanel" aria-labelledby="headingNivel4">
                                <div class="panel-body">
                                    <asp:RadioButtonList runat="server" SelectionMode="Single" ID="rbtnlUsuariosGrupoNivel4" OnSelectedIndexChanged="rbtnlUsuariosGrupoNivel4_OnSelectedIndexChanged" AutoPostBack="True" />
                                </div>
                            </div>
                        </div>
                    </div>



                </div>
            </div>
            <div class="panel-footer" style="text-align: center">
                <asp:Button ID="btnAceptar" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnAceptar_OnClick" />
                <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCancelar_OnClick" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
