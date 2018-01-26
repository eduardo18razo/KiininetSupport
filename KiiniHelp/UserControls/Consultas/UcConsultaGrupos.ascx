<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaGrupos.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaGrupos" %>
<%@ Register Src="~/UserControls/Altas/UcAltaGrupoUsuario.ascx" TagPrefix="uc" TagName="UcAltaGrupoUsuario" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleGrupoUsuarios.ascx" TagPrefix="uc" TagName="UcDetalleGrupoUsuarios" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleGrupoOpciones.ascx" TagPrefix="uc" TagName="UcDetalleGrupoOpciones" %>

<div style="height: 100%;">
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>

            <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li class="breadcrumb-item">Atención</li>
                <li class="breadcrumb-item active">Grupos</li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblOrganización" Text="Grupos" /></h3>
                            </div>
                            <p>
                                Un grupo está conformado por un conjunto de usuarios (usuarios finales y operadores) que tienen privilegios comunes sobre las funciones dentro de su cuenta. El rol es la función que desempeña un usuario. Los usuarios deben estar asignados a por lo menos un grupo, pero pueden pertenecer a más de uno. Es importante tomar en cuenta que: 1) un grupo está ligado únicamente a un rol, 2) varios grupos pueden estar ligandos al mismo rol, 3) un usuario puede tener más de un rol y 4) un usuario puede pertenecer a más de un Grupo, solamente si tiene dicho rol.                            
                            </p>
                        </div>

                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">

                            <div class="form col-lg-5">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Consulta Grupos:</label>
                                    <div class="col-xs-10 col-sm-10 col-md-10 col-lg-10 no-padding-left no-margin-left">
                                        <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control no-padding-left no-margin-left" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                    </div>
                                    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2 margin-top-3">
                                        <asp:LinkButton runat="server" class="btn btn-primary btn-single-icon" OnClick="btnBuscar_OnClick"><i class="fa fa-search"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                            <div class="form col-xs-4 col-sm-4 col-md-4 col-lg-4 separador-vertical-derecho">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">... o Consulta por Tipo de Usuario y Rol</label>
                                    <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4  no-padding-left no-margin-left">
                                        <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control no-padding-left no-margin-left" Width="120px" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" />
                                    </div>

                                    <div class="col-xs-8 col-sm-8 col-md-8 col-lg-8  no-padding-left no-margin-left">
                                        <asp:DropDownList runat="server" ID="ddlTipoGrupo" CssClass="form-control" Style="width: 180px;" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTipoGrupo_OnSelectedIndexChanged" />
                                    </div>
                                </div>
                            </div>

                            <div class="form col-xs-3 col-sm-3 col-md-3 col-lg-3 text-center">
                                <div class="form-group margin-top-btn-consulta">

                                    <asp:LinkButton ID="btnDownload" runat="server" CssClass="btn btn-primary" OnClick="btnDownload_OnClick">
                                 <i class="fa fa-download"></i>  Descargar reporte</asp:LinkButton>

                                    <asp:LinkButton CssClass="btn btn-success" ID="btnNew" OnClick="btnNew_OnClick" runat="server">
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
                                            OnPageIndexChanging="gvPaginacion_PageIndexChanging" PagerSettings-PageButtonCount="25"
                                            BorderStyle="None" PagerSettings-Mode="Numeric" PageSize="15" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
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
                                                <asp:TemplateField HeaderText="Rol" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <label runat="server" class="ocultaTexto" title='<%# Eval("TipoGrupo.Descripcion")%>'><%# Eval("TipoGrupo.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Grupo" HeaderStyle-Width="30%">
                                                    <ItemTemplate>
                                                         <label runat="server" class="ocultaTexto" title='<%# Eval("Descripcion")%>'><%# Eval("Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Supervisor" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <label><%# (bool) Eval("TieneSupervisor") ? "SI" : "NO" %></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="7%">
                                                    <ItemTemplate>
                                                        <ul class="list list-unstyled" id="hiddenEnabled">
                                                            <li>
                                                                <asp:CheckBox runat="server" AutoPostBack="true" Checked='<%# (bool) Eval("Habilitado") %>' CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
                                                            </li>
                                                        </ul>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="7%">
                                                    <ItemTemplate>
                                                        <ul class="list list-unstyled hidden" id="hiddenEditar">
                                                            <li>
                                                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id")%>' OnClick="btnEditar_OnClick"><asp:Image runat="server" ImageUrl="~/assets/images/icons/editar.png" /> </asp:LinkButton>
                                                            </li>
                                                        </ul>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Usuarios" HeaderStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" Text="Ir" CommandArgument='<%# Eval("Id")%>' ID="lnkBtnDetalleUsuario" OnClick="lnkBtnDetalleUsuario_OnClick"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Opciones" HeaderStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" Text="Ir" CommandArgument='<%# Eval("Id")%>' ID="lnkBtnDetalleOpciones" OnClick="lnkBtnDetalleOpciones_OnClick"></asp:LinkButton>
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
    <%--MODAL GRUPO USUARIO--%>
    <div class="modal fade" id="modalAltaGrupoUsuarios" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upAltaGrupo" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog" style="height: 250px;">
                    <div class="modal-content">
                        <uc:UcAltaGrupoUsuario runat="server" ID="ucAltaGrupoUsuario" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--MODAL DETALLE GRUPO USUARIO--%>
    <div class="modal fade" id="modalDetalleGrupoUsuario" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog" style="height: 250px;">
                    <div class="modal-content">
                        <uc:UcDetalleGrupoUsuarios runat="server" ID="ucDetalleGrupoUsuarios" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--MODAL DETALLE GRUPO OPCIONES--%>
    <div class="modal fade" id="modalDetalleGrupoOpciones" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog" style="height: 250px;">
                    <div class="modal-content">
                        <uc:UcDetalleGrupoOpciones runat="server" ID="ucDetalleGrupoOpciones" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--MODAL SELECCION DE ROL--%>
    <div class="modal fade" id="modalSeleccionRol" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upSubRoles" runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-md">
                    <div class="modal-content">
                        <div class="modal-header" id="panelAlertaSeleccionRol" runat="server" visible="false">
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
                                <asp:Repeater runat="server" ID="rptErrorSeleccionRol">
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
                        <div class="panel panel-primary">
                            <div class="panel-heading">
                                <asp:Label runat="server" ID="lblTitleSubRoles"></asp:Label>
                            </div>
                            <div class="panel-body">
                                <asp:HiddenField runat="server" ID="hfOperacion" />
                                <div>
                                    <div class="form-group">
                                        <div class="form-group">
                                            <asp:CheckBoxList runat="server" ID="chklbxSubRoles" Checked="True" Visible="True" OnSelectedIndexChanged="chklbxSubRoles_OnSelectedIndexChanged" AutoPostBack="True" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>


