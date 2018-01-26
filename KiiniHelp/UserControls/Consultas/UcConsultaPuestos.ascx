<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaPuestos.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaPuestos" %>
<%@ Register Src="~/UserControls/Altas/UcAltaPuesto.ascx" TagPrefix="uc1" TagName="UcAltaPuesto" %>
<div style="height: 100%;">
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>
            <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li class="breadcrumb-item">Administración</li>
                <li class="breadcrumb-item active">Puestos</li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Puestos" /></h3>
                            </div>
                            <p>
                                Los puestos representan las tareas y deberes específicos de un usuario dentro de la organización. Puedes crear puestos para facilitar la administración de los usuarios y sus eventos.                           
                            </p>
                        </div>

                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">

                            <div class="form col-lg-5">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Consulta Puestos:</label>
                                    <div class="col-xs-10 col-sm-10 col-md-10 col-lg-10 no-padding-left no-margin-left">
                                        <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control no-padding-left no-margin-left" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" placeholder="Busca con una palabra clave..." />
                                    </div>
                                    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2 margin-top-3">
                                        <asp:LinkButton runat="server" class="btn btn-primary btn-single-icon" OnClick="btnBuscar_OnClick"><i class="fa fa-search"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                            <div class="form col-xs-3 col-sm-3 col-md-3 col-lg-3 separador-vertical-derecho">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">... o Consulta por Tipo de Usuario</label>
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12  no-padding-left no-margin-left">
                                        <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control no-padding-left no-margin-left" Width="190px" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" />
                                    </div>
                                </div>
                            </div>

                            <div class="form col-xs-4 col-sm-4 col-md-4 col-lg-4 text-center">
                                <div class="form-group margin-top-btn-consulta">
                                    <asp:LinkButton ID="btnDownload" runat="server" CssClass="btn btn-primary" OnClick="btnDownload_OnClick">
                                 <i class="fa fa-download"></i>  Descargar reporte</asp:LinkButton>

                                    <asp:LinkButton runat="server" CssClass="btn btn-success" Text="Nuevo" OnClick="btnNew_OnClick">
                                        <i class="fa fa-plus"></i>Nuevo</asp:LinkButton>

                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </section>
            <div id="masonry" class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                <div class="module-wrapper masonry-item col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                    <section class="module module-headings">
                        <div class="module-inner">
                            <div class="module-content collapse in" id="content-1">
                                <div class="module-content-inner no-padding-bottom">
                                    <div class="table-responsive">
                                        <asp:GridView runat="server" ID="tblResults" AllowPaging="true" AutoGenerateColumns="false" Width="99%"
                                            OnPageIndexChanging="gvPaginacion_PageIndexChanging" PagerSettings-PageButtonCount="15"
                                            BorderStyle="None" PagerSettings-Mode="Numeric" PageSize="5" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                            PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" CssClass="table table-striped display alineaTablaIzquierda">
                                            <Columns>
                                                <asp:TemplateField HeaderText="TU" HeaderStyle-Width="25px">
                                                    <ItemTemplate>
                                                        <div class="altoFijo">
                                                            <button type="button" class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + Eval("TipoUsuario.Color") + " !important" %>'>
                                                                <%# Eval("TipoUsuario.Abreviacion") %></button>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Puesto" HeaderStyle-Width="60%">
                                                    <ItemTemplate>
                                                        <label runat="server" class="ocultaTexto " title='<%# Eval("Descripcion")%>'><%# Eval("Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <ul class="list list-unstyled" id="hiddenEnabled">
                                                            <li>
                                                                <asp:CheckBox runat="server" AutoPostBack="true" Checked='<%# (bool) Eval("Habilitado") %>' CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
                                                            </li>
                                                        </ul>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <ul class="list list-unstyled hidden" id="hiddenEditar">
                                                            <li>
                                                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id")%>' OnClick="btnEditar_OnClick"><asp:Image runat="server" ImageUrl="~/assets/images/icons/editar.png" /> </asp:LinkButton>
                                                            </li>
                                                        </ul>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>


                </div>
            </div>

            <script type="text/javascript">
                $(function () {
                    hidden('#' + "<%=tblResults.ClientID %>");
                        });
                        var prm = Sys.WebForms.PageRequestManager.getInstance();

                        prm.add_endRequest(function () {
                            hidden('#' + "<%=tblResults.ClientID %>");
                });

            </script>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownload" />
        </Triggers>
    </asp:UpdatePanel>
    <div id="modalAltaPuesto" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true" class="modal fade">
        <asp:UpdatePanel ID="upAltaPuesto" runat="server">
            <ContentTemplate>
                <div class="modal-dialog">
                    <div class="modal-content">
                        <uc1:UcAltaPuesto runat="server" ID="ucAltaPuesto" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
