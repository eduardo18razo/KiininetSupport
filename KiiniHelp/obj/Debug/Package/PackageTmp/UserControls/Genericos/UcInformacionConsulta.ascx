<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcInformacionConsulta.ascx.cs" Inherits="KiiniHelp.UserControls.Genericos.UcInformacionConsulta" %>
<%@ Import Namespace="KinniNet.Business.Utils" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <header class="modal-header" id="pnlAlertaGeneral" runat="server" visible="false">
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
        <div class="well">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <asp:Label runat="server" ID="lbltitleArbol" />
                </div>
                <div class="panel-body">
                    <asp:HiddenField runat="server" ID="dfIdGrupo" />
                    <asp:Repeater runat="server" ID="rptInformacionConsulta">
                        <ItemTemplate>
                            <div class="float-left widht25Perc">
                                <asp:Button runat="server" Text='<%# Eval("TipoInfConsulta.Descripcion") %>' CommandArgument='<%# Eval("Id") %>' ID="btnInformacion" OnClick="btnInformacion_OnClick" CommandName="0" CssClass="btn btn-primary btn-lg" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            $(document).ready(function () {
                <%--document.getElementById("<%= this.FindControl("btnCerrar").ClientID %>").click();--%>
                <%--var elementId = document.getElementById("<%= this.FindControl("hfCargaInicial").ClientID %>");
                    if (elementId.value === "False") {
                        $('#modalMuestraInformacion').modal({ backdrop: 'static', keyboard: false });
                        $('#modalMuestraInformacion').modal({ show: true });
                        elementId.value = "true";
                    }

                });--%>
            </script>
    </ContentTemplate>
</asp:UpdatePanel>

<%--MODAL Agrega Campo--%>
<div class="modal fade" id="modalMuestraInformacion" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upMostrarInformacion" runat="server">
        <ContentTemplate>
            <div class="modal-dialog modal-lg" style="width: 90%;">
                <div class="modal-content widht100">
                    <div class="modal-header" id="panelAlertaModal" runat="server" visible="False">
                        <div class="alert alert-danger">
                            <div>
                                <div class="float-left">
                                    <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                                </div> 
                                <div class="float-left margin-left-20">
                                    <h3>Error</h3>
                                </div>
                                <div class="clearfix clear-fix" />
                            </div>
                            <hr />
                            <asp:Repeater runat="server" ID="rptErrorModal">
                                <ItemTemplate>
                                    <%# Eval("Detalle")  %>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="panel panel-primary">
                        <asp:HiddenField runat="server" ID="hfIdArbol" />
                        <asp:HiddenField runat="server" ID="hfIdInformacion" />
                        <div runat="server" id="divPropuetario" visible="False">
                            <asp:Label runat="server" ID="lblContenido"></asp:Label>
                        </div>
                        <div runat="server" id="divInfoDocto" visible="False">
                            <iframe runat="server" id="ifDoctos" scrolling="yes" frameborder="0" style="border: none; overflow: hidden; width: 100%; height: 700px;" allowtransparency="true"></iframe>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <asp:Repeater runat="server" ID="rptDownloads">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text="" CssClass="col-sm-2 control-label" />
                                        <div class="col-sm-10">
                                            <asp:HyperLink runat="server" Text='<%# Eval("Archivo") %>' NavigateUrl='<%# ResolveUrl(string.Format("~/Downloads/FrmDownloads.aspx?file={0}", BusinessVariables.Directorios.RepositorioInformacionConsulta + "~" + Eval("Archivo"))) %>'></asp:HyperLink>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer text-center">
                        <asp:Button ID="btnCerrarModalInfo" runat="server" CssClass="btn btn-lg btn-danger" Text="Cerrar" OnClick="btnCerrarModalInfo_OnClick" />
                    </div>

                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</div>

