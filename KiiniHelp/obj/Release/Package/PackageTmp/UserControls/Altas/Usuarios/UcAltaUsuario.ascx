<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaUsuario.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Usuarios.UcAltaUsuario" %>

<%@ Register Src="~/UserControls/Altas/UcAltaPuesto.ascx" TagPrefix="uc" TagName="UcAltaPuesto" %>
<%@ Register Src="~/UserControls/Altas/Organizaciones/UcAltaOrganizaciones.ascx" TagPrefix="uc" TagName="UcAltaOrganizaciones" %>
<%@ Register Src="~/UserControls/Altas/Ubicaciones/UcAltaUbicaciones.ascx" TagPrefix="uc" TagName="UcAltaUbicaciones" %>
<%@ Register Src="~/UserControls/Seleccion/UcRolGrupo.ascx" TagPrefix="uc" TagName="UcRolGrupo" %>


<style>
    .texto-normal {
        text-transform: none;
    }
    /* adapted from http://maxwells.github.io/bootstrap-tags.html */
    .tag {
        font-size: 14px;
        padding: .3em .4em .4em;
        margin: 0 .1em;
        background-color: transparent;
        border-color: #e6e7ea;
        color: #e6e7ea;
    }

        .tag a {
            color: #bbb;
            cursor: pointer;
            opacity: 0.6;
        }

            .tag a:hover {
                opacity: 1.0;
            }

        .tag .remove {
            vertical-align: bottom;
            top: 0;
        }

        .tag a {
            margin: 0 0 0 .3em;
        }

            .tag a .glyphicon-white {
                margin-bottom: 2px;
                color: #3aa7aa;
            }
</style>

    <script type="text/javascript">
        function UploadFile(fileUpload) {
            debugger;
        if (fileUpload.value != '') {
            document.getElementById("<%=btnUpload.ClientID %>").click();
        }
    }
</script>


<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdatePanel ID="upGeneral" runat="server">

            <Triggers>
                <asp:PostBackTrigger  ControlID="btnUpload" />
            </Triggers>
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hfIdUsuario" />
                <asp:HiddenField runat="server" ID="hfAlta" />
                <asp:HiddenField runat="server" ID="hfEsMoral" />
                <asp:HiddenField runat="server" ID="hdEsDetalle" />
                <asp:HiddenField runat="server" ID="hfEditaDetalle" />

                <!--MIGAS DE PAN-->
                <div class="row">
                    <div class="project-heading">
                        <h3 class="h6">
                            <asp:HyperLink NavigateUrl="~/Users/DashBoard.aspx" runat="server">Home</asp:HyperLink>
                            / Usuarios</h3>
                        <hr>
                        <h2><asp:Label runat="server" ID="lblTitle"></asp:Label></h2>
                        <br />
                    </div>
                </div>
                <!--/MIGAS DE PAN-->

                <!--MÓDULO FORMULARIO-->
                <div>
                    <section class="module">
                        <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="Upload" Style="display: none" ClientIDMode="Static"/>
                        <div class="form-group avatar" runat="server" id="divAvatar" visible="False">
                            <figure class="figure col-md-1 col-sm-2 col-xs-12 center-content-div">
                                <asp:Image CssClass="img-rounded img-responsive" ImageUrl="~/assets/images/profiles/profile-square-1.png" ID="imgPerfil" alt="" runat="server" />
                                
                                <asp:Panel ID="PnlFsAttch" runat="server" Style="position: relative; overflow: Hidden; cursor: pointer;">
                                    <asp:FileUpload runat="server" ID="FileUpload1" Style="position: absolute; left: -20px; z-index: 2; opacity: 0; filter: alpha(opacity=0); cursor: pointer" />
                                    
                                    <asp:LinkButton runat="server" Text="Cambiar" ID="btnCambiarImagen" ClientIDMode="Static"></asp:LinkButton>
                                    <%--<asp:Button ID="BtnFsAttch" BackColor="YellowGreen" ForeColor="Blue" runat="server" Text="Upload file" Font-Size="x-Small" Width="80px" Style="position: absolute; top: 0px; left: 0px; z-index: 1; font-style: italic" />--%>
                                </asp:Panel>
                            </figure>
                            <div class="form-group col-sm-10">
                                <h2>
                                    <asp:Label runat="server" ID="lblnombreCompleto" CssClass="col-sm-11" />
                                </h2>
                                <asp:LinkButton runat="server" Text="Editar" CssClass="btn btn-primary" ID="btnEditar" OnClick="btnEditar_OnClick" />
                            </div>
                            <div class="form-group col-sm-10">
                                <asp:Label runat="server" Text="Ultimo Acceso" CssClass="col-md-12"></asp:Label>
                                <asp:Label runat="server" ID="lblFechaUltimoAcceso" Text="Fecha Ultimo Acceso" CssClass="col-md-12"></asp:Label>
                            </div>

                            <hr />
                        </div>
                        <div class="row" runat="server" id="divTipousuario">
                            <div class="module-inner">
                                <div class="row">
                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3">
                                        <div class="form-group">
                                            <asp:Label runat="server" Text="Tipo de usuario" CssClass="col-md-4"></asp:Label>
                                            <div class="col-md-8">
                                                <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" AutoPostBack="true" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--GRUPO DATOS GENERALES -->
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <!--TÍTULO DATOS GENERALES-->
                                <div class="module-inner">
                                    <div class="module-heading">
                                        <h3 class="module-title">DATOS GENERALES</h3>
                                    </div>
                                    <br />

                                    <!--FILA 1-->
                                    <div class="row">
                                        <div class="col-lg-3 col-md-3">
                                            <asp:TextBox ID="txtAp" runat="server" CssClass="form-control" placeholder="Apellido paterno" onkeypress="return ValidaCampo(this,1)" AutoPostBack="true" MaxLength="32" OnTextChanged="txtAp_OnTextChanged" />
                                        </div>
                                        <div class="col-lg-3 col-md-3">
                                            <asp:TextBox ID="txtAm" runat="server" CssClass="form-control" placeholder="Apellido materno" onkeypress="return ValidaCampo(this,1)" MaxLength="32" />
                                        </div>
                                        <div class="col-lg-3 col-md-3">
                                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Nombre" onkeypress="return ValidaCampo(this,1)" AutoPostBack="true" MaxLength="32" OnTextChanged="txtAp_OnTextChanged" />
                                        </div>
                                        <div class="col-lg-3 col-md-3">
                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control texto-normal" onkeypress="return ValidaCampo(this,14)" OnTextChanged="txtAp_OnTextChanged" MaxLength="12" Style="text-transform: none" AutoPostBack="True" />
                                        </div>
                                    </div>

                                    <!--FILA 2-->
                                    <div class="row">
                                        <br />
                                        <div class="col-lg-3 col-md-3" runat="server" id="divPuesto">
                                            Puesto*
                                    <asp:DropDownList runat="server" ID="ddlPuesto" CssClass="form-control" />
                                        </div>
                                        <div class="col-lg-3 col-md-3">
                                            <br />
                                            <asp:LinkButton runat="server" CssClass="fa fa-plus-circle" ID="btnAddPuesto" OnClick="btnAddPuesto_OnClick" />
                                        </div>
                                        <div class="col-lg-3 col-md-3">
                                            <br />
                                            <div class="form-inline">
                                                <label for="chkVip" class="col-lg-10 col-md-10">VIP</label>
                                                <asp:CheckBox runat="server" Text="VIP" ID="chkVip" CssClass="chkIphone col-lg-2 col-md-2" />
                                            </div>
                                            <div class="form-inline">
                                                <label for="chkVip" class="col-lg-10 col-md-10">Directorio activo</label>
                                                <asp:CheckBox runat="server" Text="Directorio Activo " ID="chkDirectoriActivo" CssClass="chkIphone col-lg-2 col-md-2" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--/GRUPO DATOS GENERALES -->

                        <hr />

                        <!--GRUPO TELÉFONOS DE CONTACTO -->
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <!--TÍTULO DATOS GENERALES-->
                                <div class="module-inner">
                                    <div class="module-heading">
                                        <h3 class="module-title">TELÉFONOS DE CONTACTO</h3>
                                    </div>
                                    <br />
                                    <!--FILA 1-->
                                    <div class="row">
                                        <asp:Repeater ID="rptTelefonos" runat="server" OnItemDataBound="rptTelefonos_OnItemDataBound">
                                            <ItemTemplate>
                                                <div style="border-radius: 20px; margin-bottom: 5px; height: auto">
                                                    <div class="row">
                                                        <asp:Label runat="server" Text='<%# bool.Parse(Eval("Obligatorio").ToString()) %>' ID="lblObligatorio" Visible="False"></asp:Label>
                                                        <div class="col-lg-3 col-md-3">
                                                            <asp:TextBox runat="server" ID="txtNumero" Text='<%# Eval("Numero") %>' CssClass="form-control" onkeypress="return ValidaCampo(this,2)" MaxLength="10" />
                                                        </div>
                                                        <div class="col-lg-3 col-md-3" runat="server" id="divExtension">
                                                            <asp:TextBox runat="server" ID="txtExtension" Text='<%# Eval("Extension") %>' CssClass="form-control" onkeypress="return ValidaCampo(this,15)" MaxLength="40" />
                                                        </div>
                                                        <div class="col-lg-3 col-md-3">
                                                            <asp:DropDownList runat="server" ID="ddlTipoTelefono" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoTelefono_OnSelectedIndexChanged" />
                                                        </div>
                                                    </div>
                                                    <br />
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <div class="form-group">
                                            <asp:LinkButton runat="server" CssClass="fa fa-plus-circle" ID="btnAddTelefono" OnClick="btnAddTelefono_OnClick"></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--/GRUPO TELÉFONOS DE CONTACTO -->
                        <hr />

                        <!--GRUPO CORREO ELECTRÓNICO -->
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <!--TÍTULO DATOS GENERALES-->
                                <div class="module-inner">
                                    <div class="module-heading">
                                        <h3 class="module-title">CORREO ELECTRÓNICO</h3>
                                    </div>
                                    <br />
                                    <!--FILA 1-->
                                    <div class="row">
                                        <asp:Repeater ID="rptCorreos" runat="server">
                                            <ItemTemplate>
                                                <div style="border-radius: 20px; margin-bottom: 5px; height: auto">
                                                    <div class="row">
                                                        <div class="col-lg-3 col-md-3">
                                                            <asp:TextBox runat="server" ID="txtCorreo" Text='<%# Eval("Correo") %>' CssClass='<%# bool.Parse(Eval("Obligatorio").ToString()) ? "form-control obligatorio"  : "form-control"  %>' Style="text-transform: lowercase" onkeypress="return ValidaCampo(this,13)" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <div class="form-group">
                                            <asp:LinkButton runat="server" CssClass="fa fa-plus-circle" ID="btnAddCorreo" OnClick="btnAddCorreo_OnClick"></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--/GRUPO CORREO ELECTRÓNICO -->
                        <hr />

                        <!--GRUPO ORGANIZACIÓN -->
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <!--TÍTULO ORGANIZACIÓN-->
                                <div class="module-inner">
                                    <div class="module-heading">
                                        <h3 class="module-title">ORGANIZACIÓN</h3>
                                    </div>
                                    <br />

                                    <!--FILA 1-->
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12">
                                            <asp:Button ID="btnModalOrganizacion" CssClass="btn btn-primary" runat="server" Text="Seleccionar" OnClick="btnModalOrganizacion_OnClick" />
                                        </div>
                                    </div>
                                    <br />

                                    <div class="row">
                                        <div>
                                            <div class="table-responsive">
                                                <asp:Repeater runat="server" ID="rptOrganizacion">
                                                    <HeaderTemplate>
                                                        <table class="table table-striped display">
                                                            <thead>
                                                                <tr>
                                                                    <th>
                                                                        <asp:Label runat="server" ID="Label1">Tipo Usuario</asp:Label></th>
                                                                    <th>
                                                                        <asp:Label runat="server" ID="lblHolding">Nivel 1</asp:Label></th>
                                                                    <th>
                                                                        <asp:Label runat="server" ID="lblCompania">Nivel 2</asp:Label></th>
                                                                    <th>
                                                                        <asp:Label runat="server" ID="lblDireccion">Nivel 3</asp:Label></th>
                                                                    <th>
                                                                        <asp:Label runat="server" ID="lblSubDireccion">Nivel 4</asp:Label></th>
                                                                    <th>
                                                                        <asp:Label runat="server" ID="lblGerencia">Nivel 5</asp:Label></th>
                                                                    <th>
                                                                        <asp:Label runat="server" ID="lblSubGerencia">Nivel 6</asp:Label></th>
                                                                    <th>
                                                                        <asp:Label runat="server" ID="lblJefatura">Nivel 7</asp:Label></th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblIdOrganizacion" Text='<%# Eval("Id")%>' Visible="False" />
                                                                <button type="button" class="btn btn-default-alt btn-circle"><%# Eval("TipoUsuario.Descripcion").ToString().Substring(0,1) %></button>
                                                            </td>
                                                            <td><%# Eval("Holding.Descripcion")%></td>
                                                            <td><%# Eval("Compania.Descripcion")%></td>
                                                            <td><%# Eval("Direccion.Descripcion")%></td>
                                                            <td><%# Eval("SubDireccion.Descripcion")%></td>
                                                            <td><%# Eval("Gerencia.Descripcion")%></td>
                                                            <td><%# Eval("SubGerencia.Descripcion")%></td>
                                                            <td><%# Eval("Jefatura.Descripcion")%></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody>
                                            </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--/GRUPO ORGANIZACIÓN -->
                        <hr />

                        <!--GRUPO UBICACIÓN -->
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">

                                <!--TÍTULO UBICACIÓN-->
                                <div class="module-inner">
                                    <div class="module-heading">
                                        <h3 class="module-title">UBICACIÓN</h3>
                                    </div>
                                    <br />

                                    <!--FILA 1-->
                                    <div class="row">
                                        <div class="col-lg-4 col-md-4">
                                            <asp:Button CssClass="btn btn-primary" runat="server" ID="btnModalUbicacion" Text="Seleccionar" OnClick="btnModalUbicacion_OnClick" />
                                        </div>
                                    </div>
                                    <br />

                                    <div class="row">
                                        <div>
                                            <div class="table-responsive">
                                                <asp:Repeater runat="server" ID="rptUbicacion">
                                                    <HeaderTemplate>
                                                        <table class="table table-striped display" id="tblHeader">
                                                            <thead>
                                                                <tr>
                                                                    <th>
                                                                        <asp:Label runat="server" ID="Label1">Tipo Usuario</asp:Label></th>
                                                                    <th>
                                                                        <asp:Label runat="server" ID="lblNivel1">Nivel 1</asp:Label></th>
                                                                    <th>
                                                                        <asp:Label runat="server" ID="lblNivel2">Nivel 2</asp:Label></th>
                                                                    <th>
                                                                        <asp:Label runat="server" ID="lblNivel3">Nivel 3</asp:Label></th>
                                                                    <th>
                                                                        <asp:Label runat="server" ID="lblNivel4">Nivel 4</asp:Label></th>
                                                                    <th>
                                                                        <asp:Label runat="server" ID="lblNivel5">Nivel 5</asp:Label></th>
                                                                    <th>
                                                                        <asp:Label runat="server" ID="lblNivel6">Nivel 6</asp:Label></th>
                                                                    <th>
                                                                        <asp:Label runat="server" ID="lblNivel7">Nivel 7</asp:Label></th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblIdUbicacion" Text='<%# Eval("Id")%>' Visible="False" />
                                                                <button type="button" class="btn btn-default-alt btn-circle"><%# Eval("TipoUsuario.Descripcion").ToString().Substring(0,1) %></button>
                                                            </td>
                                                            <td><%# Eval("Pais.Descripcion")%></td>
                                                            <td><%# Eval("Campus.Descripcion")%></td>
                                                            <td><%# Eval("Torre.Descripcion")%></td>
                                                            <td><%# Eval("Piso.Descripcion")%></td>
                                                            <td><%# Eval("Zona.Descripcion")%></td>
                                                            <td><%# Eval("SubZona.Descripcion")%></td>
                                                            <td><%# Eval("SiteRack.Descripcion")%></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody>
                                            </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--/GRUPO UBICACIÓN -->
                        <hr />

                        <!-- ROLES Y GRUPOS -->
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">

                                <!--TÍTULO ROLES Y GRUPOS-->
                                <div class="module-inner">
                                    <div class="module-heading">
                                        <h3 class="module-title">ROLES Y GRUPOS</h3>
                                    </div>
                                    <br />

                                    <!--FILA 1-->
                                    <div class="row">
                                        <div class="col-lg-4 col-md-4">
                                            <asp:Button CssClass="btn btn-primary" runat="server" Text="Agregar" ID="btnModalRoles" OnClick="btnModalRoles_OnClick" />
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <asp:Repeater runat="server" ID="rptRoles" OnItemDataBound="rptRoles_OnItemDataBound">
                                            <ItemTemplate>
                                                <div class="row col-lg-12 col-md-12">
                                                    <div runat="server" id="divRolesGrupos">
                                                        <span><%# Eval("DescripcionRol") %></span>
                                                        <div class="row col-lg-12 col-md-12">
                                                            <asp:Repeater runat="server" ID="rptGrupos" OnItemDataBound="rptGrupos_OnItemDataBound">
                                                                <ItemTemplate>
                                                                    <div class="row col-lg-4 col-md-4" style="padding: 5px">
                                                                        <span class="tag label label-info">
                                                                            <div class="row col-lg-4 col-md-4" style="padding: 5px">
                                                                                <span><%# Eval("DescripcionGrupo") %></span>
                                                                                <asp:Repeater runat="server" ID="rptSubGrupos">
                                                                                    <HeaderTemplate>
                                                                                        <br />
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <%# Eval("Descripcion") %>
                                                                                    </ItemTemplate>
                                                                                </asp:Repeater>
                                                                                <asp:LinkButton runat="server" class="remove glyphicon glyphicon-remove-sign glyphicon-white" ID="btnRemove" OnClick="OnClick" CommandArgument='<%# Eval("IdGrupo") %>'></asp:LinkButton>
                                                                                <%--<a><i  class="remove glyphicon glyphicon-remove-sign glyphicon-white"></i></a>--%>
                                                                            </div>
                                                                        </span>
                                                                    </div>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <hr />
                                                                </FooterTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="width: 100%; text-align: right">
                            <asp:Button runat="server" Text="Guardar" ID="btnGuardar" CssClass="btn btn-primary margin-left" Style="margin-right: 45px" OnClick="btnGuardar_OnClick" />
                        </div>

                    </section>

                    <br />
                </div>
                <!--/MÓDULO FORMULARIO-->


            </ContentTemplate>
        </asp:UpdatePanel>
        <%--PUESTOS--%>
        <div class="modal fade" id="modalAreas" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true" style="overflow: hidden; top: 60px">
            <asp:UpdatePanel ID="upModalAltaAreas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-dialog modal-md">
                        <div class="modal-content">
                            <uc:ucaltapuesto runat="server" id="ucAltaPuesto" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <%--ORGANIZACIONES--%>
        <div class="modal fade" id="modalOrganizacion" data-backdrop-limit="1" tabindex="-1" role="dialog" aria-labelledby="upload-avatar-title" aria-hidden="true" style="overflow: hidden; top: 60px">
            <asp:UpdatePanel ID="upOrganizacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc:UcAltaOrganizaciones runat="server" id="ucAltaOrganizaciones" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <%--UBICACIONES--%>
        <div class="modal fade" id="modalUbicacion" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true" style="overflow: hidden; top: 60px">

            <asp:UpdatePanel ID="upUbicacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc:UcAltaUbicaciones runat="server" id="ucAltaUbicaciones" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <%--ROLES--%>
        <div class="modal fade" id="modalRoles" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true" style="overflow: hidden; top: 60px">
            <asp:UpdatePanel ID="upRoles" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc:UcRolGrupo runat="server" id="ucRolGrupo" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
