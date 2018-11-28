<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaUsuario.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Usuarios.UcAltaUsuario" %>

<%@ Register Src="~/UserControls/Altas/UcAltaPuesto.ascx" TagPrefix="uc" TagName="UcAltaPuesto" %>
<%@ Register Src="~/UserControls/Altas/Organizaciones/UcAltaOrganizaciones.ascx" TagPrefix="uc" TagName="UcAltaOrganizaciones" %>
<%@ Register Src="~/UserControls/Altas/Ubicaciones/UcAltaUbicaciones.ascx" TagPrefix="uc" TagName="UcAltaUbicaciones" %>
<%@ Register Src="~/UserControls/Seleccion/UcRolGrupo.ascx" TagPrefix="uc" TagName="UcRolGrupo" %>

<script type="text/javascript">
    function UploadFile(fileUpload) {
        if (fileUpload.value != '') {
            document.getElementById("<%=btnUpload.ClientID %>").click();
        }
    }
</script>


<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdatePanel ID="upGeneral" runat="server">

            <Triggers>
                <asp:PostBackTrigger ControlID="btnUpload" />
            </Triggers>
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hfIdUsuario" />
                <asp:HiddenField runat="server" ID="hfAlta" />
                <asp:HiddenField runat="server" ID="hfEsMoral" />
                <asp:HiddenField runat="server" ID="hdEsDetalle" />
                <asp:HiddenField runat="server" ID="hfHabilitado" />
                <asp:HiddenField runat="server" ID="hfEditaDetalle" />
                <asp:HiddenField runat="server" ID="hfGeneraUsuario" Value="true" />
                <asp:HiddenField runat="server" ID="hfConsultas" Value="false" />
                <!--MIGAS DE PAN-->
                <div class="row">
                    <div>
                        <ol class="breadcrumb">
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                            <li class="active">Mi perfil</li>
                        </ol>
                    </div>
                </div>
                <!--/MIGAS DE PAN-->

                <!--MÓDULO FORMULARIO-->
                <section class="module">
                    <div class="module-inner">
                        <div class="row">
                            <!--DATOS GENERALES-->
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <asp:HyperLink data-toggle="collapse" ID="pnlPrincipal" runat="server" CssClass="panel-toggle no-underline" NavigateUrl="#faq5_1">

                                                <div class="row">
                                                    <div class="col-lg-10 col-md-8 col-sm-8 col-xs-6">
                                                        <h3 class="TitulosAzul">
                                                            <asp:Label runat="server" ID="lblTitle" /></h3>
                                                        <div runat="server" id="divUltimoAcceso">
                                                            <asp:Label runat="server" Text="Último Acceso: " />
                                                            <asp:Label runat="server" ID="lblFechaUltimoAcceso" Text="Fecha Último Acceso" />
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-2 col-md-4 col-sm-4 col-xs-6 text-right">
                                                        <asp:LinkButton runat="server" Text="Editar" CssClass="btn btn-primary" ID="btnEditar" OnClick="btnEditar_OnClick" />
                                                    </div>
                                                </div>
                                            </asp:HyperLink>
                                        </h4>
                                    </div>

                                    <asp:Panel runat="server" CssClass="panel-collapse  collapse" ID="faq5_1" ClientIDMode="Static">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">

                                                <div class="row">
                                                    <div class="col-lg-1 col-md-2 col-sm-6 col-xs-12">
                                                        <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="Upload" CssClass="ocultar" ClientIDMode="Static" />
                                                        <div class="form-group avatar" runat="server" id="divAvatar" visible="True">
                                                            <figure class="figure col-lg-10 col-md-12 col-sm-6 col-xs-6 center-content-div center-block centered">
                                                                <asp:Image CssClass="img-rounded img-responsive margin-top-25 center-block imageCircle image80" ImageUrl="~/assets/images/profiles/profile-1.png" ID="imgPerfil" alt="imgPerfil" runat="server" />

                                                                <asp:Panel ID="PnlFsAttch" runat="server" Style="position: relative; overflow: Hidden; cursor: pointer; max-height: 165px; max-width: 165px;">
                                                                    <asp:FileUpload runat="server" ID="FileUpload1" Style="position: absolute; left: -20px; z-index: 2; opacity: 0; filter: alpha(opacity=0); cursor: pointer; height: 56px" Enabled="false" />
                                                                    <asp:LinkButton runat="server" Text="Editar" Style="margin-top: 10px;" ID="btnCambiarImagen" ClientIDMode="Static" CssClass="btn btn-primary margin-top-10" Visible="false" />
                                                                </asp:Panel>
                                                            </figure>
                                                            <div class="form-group col-sm-10">
                                                                <h2>
                                                                    <asp:Label runat="server" ID="lblnombreCompleto" CssClass="col-sm-11" />
                                                                </h2>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-11 col-md-11">
                                                        <div class="row" runat="server" id="divTipousuario">
                                                            <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12 padding-20-left">
                                                                <label>Tipo de usuario</label>
                                                                <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" AutoPostBack="true" Enabled="false" />
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 padding-20-left">
                                                                <label>Nombre <span style="color: red">*</span></label>
                                                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" onkeypress="return ValidaCampo(this,1)" AutoPostBack="true" MaxLength="32" OnTextChanged="txtAp_OnTextChanged" />
                                                            </div>

                                                            <div class="col-lg-3 col-md-3 padding-20-left">
                                                                <label>Apellido Paterno <span style="color: red">*</span></label>
                                                                <asp:TextBox ID="txtAp" runat="server" CssClass="form-control" onkeypress="return ValidaCampo(this,1)" AutoPostBack="true" MaxLength="32" OnTextChanged="txtAp_OnTextChanged" />
                                                            </div>

                                                            <div class="col-lg-3 col-md-3 padding-20-left">
                                                                <label>Apellido Materno</label>
                                                                <asp:TextBox ID="txtAm" runat="server" CssClass="form-control" onkeypress="return ValidaCampo(this,1)" MaxLength="32" />
                                                            </div>

                                                            <div class="col-lg-3 col-md-3 padding-20-left">
                                                                <label>Nombre usuario</label>
                                                                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control texto-normal text-no-transform" onkeypress="return ValidaCampo(this,14)" OnTextChanged="txtAp_OnTextChanged" MaxLength="30" AutoPostBack="True" />
                                                            </div>
                                                        </div>


                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 padding-20-left" runat="server" id="divPuesto">
                                                                <label>Puesto</label>
                                                                <asp:DropDownList runat="server" ID="ddlPuesto" CssClass="form-control" />
                                                            </div>
                                                            <div class="col-lg-1 col-md-1 margin-top-30">
                                                                <asp:LinkButton runat="server" CssClass="fa fa-plus-circle " ID="btnAddPuesto" OnClick="btnAddPuesto_OnClick" />
                                                            </div>

                                                            <div class="col-lg-2 col-md-2 margin-top-30">
                                                                <div class="form-inline">
                                                                    <label for="chkVip" class="col-lg-9 col-md-9 text-right padding-10-right">VIP</label>
                                                                    <asp:CheckBox runat="server" Text="VIP" ID="chkVip" CssClass="chkIphone padding-5-top" Width="30px" Enabled="false" />
                                                                </div>
                                                            </div>

                                                            <div class="col-lg-2 col-md-2 margin-top-30">
                                                                <div class="form-inline">
                                                                    <label for="chkDirectoriActivo" class="col-lg-9 col-md-9 text-right padding-10-right">Directorio activo</label>
                                                                    <asp:CheckBox runat="server" Text="Directorio Activo " ID="chkDirectoriActivo" CssClass="chkIphone padding-5-top" Width="30px" Enabled="false" />
                                                                </div>
                                                            </div>

                                                            <div class="col-lg-2 col-md-2 margin-top-30">
                                                                <div class="form-inline">
                                                                    <label for="chkPersonaFisica" class="col-lg-9 col-md-9 text-right padding-10-right">Persona Fisica</label>
                                                                    <asp:CheckBox runat="server" Text="Persona Fisica" ID="chkPersonaFisica" CssClass="chkIphone padding-5-top" Width="30px" Enabled="false" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>

                            <!--GRUPO TELÉFONOS DE CONTACTO -->
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">

                                <div class="panel panel-default">
                                    <div class="panel-heading panel-heading-theme-1">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" class="panel-toggle no-underline" href="#faq5_2">
                                                <div class="row">
                                                    <h3 class="TitulosAzul">Datos de Contacto</h3>
                                                </div>
                                            </a>
                                        </h4>
                                    </div>
                                    <asp:Panel runat="server" CssClass="panel-collapse  collapse" ID="faq5_2" ClientIDMode="Static">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">

                                                <div class="row">
                                                    <div class="col-lg-6 col-md-6">
                                                        <div class="col-lg-7 col-md-8 no-padding-left" runat="server">
                                                            <div class="row">
                                                                <asp:Label runat="server" ID="Label2"></asp:Label>
                                                                <div class="col-lg-3 col-md-4 no-padding-left">
                                                                    <label>Tipo Teléfono</label>
                                                                </div>

                                                                <div class="col-lg-5 col-md-6 no-padding-left">
                                                                    <label>Teléfono</label>
                                                                </div>
                                                                <div class="col-lg-3 col-md-3 no-padding-left" runat="server" id="divExtensionTitulo" visible="False">
                                                                    <label>Extensión</label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <br />
                                                        <div class="col-lg-7 col-md-8 no-padding-left" runat="server">
                                                            <div class="row">
                                                                <asp:Label runat="server" ID="lblNuevoTelefonoEdicion" Text="true" Visible="False"></asp:Label>
                                                                <asp:Label runat="server" ID="lblTipoTelefonoAnteriorEdicion" Text="true" Visible="False"></asp:Label>
                                                                <asp:Label runat="server" ID="lblTelefonoAnteriorEdicion" Text="true" Visible="False"></asp:Label>
                                                                <asp:Label runat="server" ID="lblExtensionAnteriorEdicion" Text="true" Visible="False"></asp:Label>
                                                                <asp:Label runat="server" ID="lblPrincipal" Visible="False"></asp:Label>
                                                                <div class="col-lg-3 col-md-4 no-padding-left">
                                                                    <asp:DropDownList runat="server" ID="ddlTipoTelefono" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoTelefono_OnSelectedIndexChanged" />
                                                                </div>

                                                                <div class="col-lg-5 col-md-6 no-padding-left">
                                                                    <asp:TextBox runat="server" ID="txtTelefono" CssClass="form-control" onkeypress="return ValidaCampo(this,2)" MaxLength="10" />
                                                                </div>
                                                                <div class="col-lg-3 col-md-3 no-padding-left" runat="server" id="divExtension" visible="False">
                                                                    <asp:TextBox runat="server" ID="txtExtension" Text='<%# Eval("Extension") %>' CssClass="form-control" onkeypress="return ValidaCampo(this,15)" MaxLength="10" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-4 col-md-4">
                                                            <asp:LinkButton runat="server" CssClass="fa fa-plus-circle margin-top-9" ID="btnAddTelefono" OnClick="btnAddTelefono_OnClick" Visible="false"></asp:LinkButton>
                                                            <asp:LinkButton runat="server" ID="lbtnCancelarEdicionTelefono" Text="Cancelar" Visible="False" OnClick="lblCancelarEdicionTelefono_OnClick"></asp:LinkButton>
                                                        </div>
                                                        <br />

                                                        <asp:Repeater ID="rptTelefonos" runat="server" OnItemDataBound="rptTelefonos_OnItemDataBound">
                                                            <ItemTemplate>
                                                                <div style="border-radius: 20px; margin-bottom: 5px; height: auto">
                                                                    <div class="row">
                                                                        <div class="col-lg-7 col-md-8 no-padding-left" runat="server">

                                                                            <asp:Label runat="server" Text='<%# bool.Parse(Eval("Principal").ToString()) %>' ID="lblPrincipal" Visible="False"></asp:Label>

                                                                            <div class="col-lg-3 col-md-4 no-padding-left">
                                                                                <asp:DropDownList runat="server" ID="ddlTipoTelefonoRepeater" CssClass="form-control" Enabled="False" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoTelefonoRepeater_OnSelectedIndexChanged" />
                                                                            </div>

                                                                            <div class="col-lg-5 col-md-6 no-padding-left">
                                                                                <asp:TextBox runat="server" ID="txtNumero" ReadOnly="True" Text='<%# Eval("Numero") %>' CssClass="form-control" onkeypress="return ValidaCampo(this,2)" MaxLength="10" />
                                                                            </div>

                                                                            <div class="col-lg-3 col-md-2 no-padding-left" runat="server" id="divExtension">
                                                                                <asp:TextBox runat="server" ID="txtExtension" ReadOnly="True" Text='<%# Eval("Extension") %>' CssClass="form-control" onkeypress="return ValidaCampo(this,15)" MaxLength="40" />
                                                                            </div>

                                                                        </div>
                                                                        <div class="col-lg-4 col-md-4">
                                                                            <div class="col-lg-2 col-md-6 no-padding-left">
                                                                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id")%>' ID="lbtnEditarTelefono" Visible="False" OnClick="lbtnEditarTelefono_OnClick">
                                                                                <asp:Image runat="server" CssClass="margin-top-9" ImageUrl="~/assets/images/icons/editar.png" />  
                                                                                </asp:LinkButton>
                                                                            </div>
                                                                            <div class="col-lg-3 col-md-6 no-padding-left margin-top-9">
                                                                                <asp:LinkButton runat="server" Text="Eliminar" CommandArgument='<%# Eval("Numero") %>' CommandName='<%# Eval("Extension") %>' ID="lbtnEliminarTelefono" OnClick="lbtnEliminarTelefono_OnClick"></asp:LinkButton>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                    <!--/GRUPO TELÉFONOS DE CONTACTO -->
                                                    <!--CORREOS DE CONTACTO -->
                                                    <div class="col-lg-6 col-md-6">
                                                        <asp:Label runat="server" ID="lblNuevoCorreoEdicion" Text="true" Visible="False"></asp:Label>
                                                        <asp:Label runat="server" ID="lblCorreoAnteriorEdicion" Text="true" Visible="False"></asp:Label>
                                                        <asp:Label runat="server" ID="lblCorreoPrincipalEdicion" Text="true" Visible="False"></asp:Label>
                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-padding-left">
                                                            <div class="row">
                                                                <div class="col-lg-12 col-md-12 no-padding-left">
                                                                    <label>Correo <span style="color: red">*</span></label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-8 col-md-8 col-sm-8 col-xs-8 no-padding-left">
                                                            <div class="col-lg-11 col-md-11 col-sm-11 col-xs-11 no-padding-left">
                                                                <asp:TextBox runat="server" ID="txtCorreoPrincipal" type="email" CssClass="form-control text-lowercase" onkeypress="return ValidaCampo(this,13)" />
                                                            </div>
                                                        </div>

                                                        <br />
                                                        <div class="col-lg-4 col-md-4">
                                                            <asp:LinkButton runat="server" CssClass="fa fa-plus-circle margin-top-9" ID="btnAddCorreo" OnClick="btnAddCorreo_OnClick" Visible="false"></asp:LinkButton>
                                                            <asp:LinkButton runat="server" ID="lbtnCancelarEdicionCorreo" Text="Cancelar" Visible="False" OnClick="lbtnCancelarEdicionCorreo_OnClick"></asp:LinkButton>
                                                        </div>
                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-padding-left">
                                                            <div class="row">
                                                                <div class="col-lg-12 col-md-12 no-padding-left">
                                                                    <label>Confirmar correo</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-8 col-md-8 col-sm-8 col-xs-8 no-padding-left">
                                                            <div class="col-lg-11 col-md-11 col-sm-11 col-xs-11 no-padding-left">
                                                                <asp:TextBox runat="server" ID="txtCorreoPrincipalConfirmacion" type="email" CssClass="form-control text-lowercase" onkeypress="return ValidaCampo(this,13)" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <asp:Repeater ID="rptCorreos" runat="server" OnItemDataBound="rptCorreos_OnItemDataBound">
                                                            <ItemTemplate>
                                                                <div style="border-radius: 20px; margin-bottom: 5px; height: auto">
                                                                    <div class="row">
                                                                        <div class="col-lg-8 col-md-8 col-sm-8 col-xs-8 no-padding-left">
                                                                            <div class="col-lg-11 col-md-11 col-sm-11 col-xs-11 no-padding-left">
                                                                                <asp:Label runat="server" Visible="False" ID="lblCorreoPrincipal" Text='<%# Eval("Obligatorio") %>'></asp:Label>
                                                                                <asp:TextBox runat="server" ID="txtCorreo" ReadOnly="True" Text='<%# Eval("Correo") %>' type="email" CssClass="form-control text-lowercase" onkeypress="return ValidaCampo(this,13)" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-lg-3 col-md-8">
                                                                            <div class="col-lg-3 col-md-6 no-padding-left">
                                                                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id")%>' ID="lbtnEditarCorreo" OnClick="lbtnEditarCorreo_OnClick" Visible="False">
                                                                                        <asp:Image CssClass="margin-top-9" runat="server" ImageUrl="~/assets/images/icons/editar.png" />
                                                                                </asp:LinkButton>
                                                                            </div>
                                                                            <div class="col-lg-3 col-md-6 no-padding-left margin-top-9">
                                                                                <asp:LinkButton runat="server" Text="Eliminar" CommandArgument='<%# Eval("Correo") %>' CommandName='<%# Eval("Obligatorio") %>' ID="lbtnEliminarCorreo" OnClick="lbtnEliminarCorreo_OnClick"></asp:LinkButton>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                    <!--/CORREOS DE CONTACTO -->
                                                </div>

                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>

                            </div>

                            <!--GRUPO ORGANIZACIÓN -->
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading panel-heading-theme-1">
                                        <h4 class="panel-title">
                                            <div class="row">
                                                <a data-toggle="collapse" class="panel-toggle no-underline" href="#faq5_3">
                                                    <div class="col-lg-10 col-md-8 col-sm-8">
                                                        <h3 class="TitulosAzul">Organización</h3>
                                                    </div>
                                                </a>
                                                <div class="col-lg-2 col-md-4 col-sm-4 text-right">
                                                    <asp:Button ID="btnModalOrganizacion" CssClass="btn btn-primary" runat="server" Text="Agregar" OnClick="btnModalOrganizacion_OnClick" />
                                                </div>
                                            </div>
                                        </h4>
                                    </div>


                                    <asp:Panel runat="server" CssClass="panel-collapse collapse" ID="faq5_3" ClientIDMode="Static">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <div class="row">
                                                    <div>
                                                        <div class="table-responsive">
                                                            <asp:Repeater runat="server" ID="rptOrganizacion">
                                                                <HeaderTemplate>
                                                                    <table class="table table-striped display">
                                                                        <thead>
                                                                            <tr>
                                                                                <th>
                                                                                    <asp:Label runat="server" ID="Label1">TU</asp:Label></th>
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
                                                                            <button type="button" class="btn btn-default-alt btn-circle"><%# Eval("TipoUsuario.Abreviacion") %></button>
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
                                    </asp:Panel>
                                </div>
                            </div>

                            <!--GRUPO UBICACIÓN DOMICILIO-->
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" runat="server" id="divDomicilio" visible="False">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <div class="row">
                                                <a data-toggle="collapse" class="panel-toggle no-underline" href="#faq_domicilio">
                                                    <div class="col-lg-10 col-md-8 col-sm-8">
                                                        <h3 class="TitulosAzul">Domicilio</h3>
                                                    </div>
                                                </a>
                                            </div>
                                        </h4>
                                    </div>

                                    <asp:Panel runat="server" CssClass="panel-collapse collapse" ID="faq_domicilio" ClientIDMode="Static">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <div class="row">

                                                    <div class="row">
                                                        <!--CALLE-->
                                                        <div class="form-group margin-top col-lg-6 col-md-8">
                                                            Calle*<br />
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtCalle" onkeydown="return (event.keyCode!=13);" MaxLength="99"></asp:TextBox>
                                                        </div>

                                                        <!--NUM EXT-->
                                                        <div class="form-group margin-top col-lg-2 col-md-2">
                                                            Número Exterior*<br />
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtNoExt" onkeydown="return (event.keyCode!=13);" MaxLength="20"></asp:TextBox>
                                                        </div>
                                                        <!--NUM INT-->
                                                        <div class="form-group margin-top col-lg-2 col-md-2">
                                                            Número Interior<br />
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtNoInt" onkeydown="return (event.keyCode!=13);" MaxLength="20"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <!--CP-->
                                                        <div class="form-group margin-top col-lg-2 col-md-2">
                                                            CP*<br />
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtCp" MaxLength="5" onkeydown="return (event.keyCode!=13);" AutoPostBack="True" OnTextChanged="txtCp_OnTextChanged" onkeypress="return ValidaCampo(this,2)"></asp:TextBox>
                                                        </div>

                                                        <!--Ingresar datos-->
                                                        <div class="form-group margin-top col-lg-2 col-md-3" runat="server" visible="False">
                                                            <asp:CheckBox runat="server" ID="chkDatosManual" Text="Ingresar datos de forma manual" AutoPostBack="True" />
                                                        </div>

                                                        <!--COLONIA-->
                                                        <div class="form-group margin-top col-lg-2 col-md-3">
                                                            Colonia*<br />
                                                            <asp:DropDownList runat="server" ID="ddlColonia" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlColonia_OnSelectedIndexChanged" />
                                                        </div>

                                                        <!--MUNICIPIO-->
                                                        <div class="form-group margin-top col-lg-2 col-md-3">
                                                            Municipio*<br />
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtMunicipio" onkeydown="return (event.keyCode!=13);" ReadOnly="True"></asp:TextBox>
                                                        </div>
                                                        <!--ESTADO-->
                                                        <div class="form-group margin-top col-lg-2 col-md-3">
                                                            Estado*<br />
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtEstado" onkeydown="return (event.keyCode!=13);" ReadOnly="True" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" runat="server" id="divUbicacion" visible="True">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <div class="row">
                                                <a data-toggle="collapse" class="panel-toggle no-underline" href="#faq5_4">
                                                    <div class="col-lg-10 col-md-8 col-sm-8">
                                                        <h3 class="TitulosAzul">Ubicación</h3>
                                                    </div>
                                                </a>
                                                <div class="col-lg-2 col-md-4 text-right">
                                                    <asp:Button CssClass="btn btn-primary" runat="server" ID="btnModalUbicacion" Text="Agregar" OnClick="btnModalUbicacion_OnClick" />
                                                </div>
                                            </div>
                                        </h4>
                                    </div>
                                    <asp:Panel runat="server" CssClass="panel-collapse collapse" ID="faq5_4" ClientIDMode="Static">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">

                                                <div class="row">
                                                    <div class="table-responsive">
                                                        <asp:Repeater runat="server" ID="rptUbicacion">
                                                            <HeaderTemplate>
                                                                <table class="table table-striped display" id="tblHeader">
                                                                    <thead>
                                                                        <tr>
                                                                            <th>
                                                                                <asp:Label runat="server" ID="Label1">TU</asp:Label></th>
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
                                                                        <button type="button" class="btn btn-default-alt btn-circle"><%# Eval("TipoUsuario.Abreviacion") %></button>
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
                                    </asp:Panel>
                                </div>
                            </div>

                            <!-- ROLES Y GRUPOS -->
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">

                                            <div class="row">
                                                <a data-toggle="collapse" class="no-underline" href="#faq5_5">
                                                    <div class="col-lg-10 col-md-8 col-sm-8">

                                                        <h3 class="TitulosAzul">Roles y Grupos</h3>
                                                    </div>
                                                </a>
                                                <div class="col-lg-2 col-md-4 text-right">
                                                    <asp:Button CssClass="btn btn-primary" runat="server" Text="Agregar" ID="btnModalRoles" OnClick="btnModalRoles_OnClick" />
                                                </div>
                                            </div>
                                        </h4>
                                    </div>
                                    <asp:Panel runat="server" CssClass="panel-collapse collapse" ID="faq5_5" ClientIDMode="Static">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <div class="row">

                                                    <asp:Repeater runat="server" ID="rptRoles" OnItemDataBound="rptRoles_OnItemDataBound">
                                                        <ItemTemplate>
                                                            <div class="row">
                                                                <div class="col-lg-12 col-md-12">
                                                                    <div runat="server" id="divRolesGrupos" class="col-lg-12 col-md-12">
                                                                        <asp:Label runat="server" Visible="False" ID="lblIdRol" Text='<%# Eval("IdRol") %>' />
                                                                        <span class="strong"><%# Eval("DescripcionRol") %></span>
                                                                        <div class="col-lg-12 col-md-12">
                                                                            <asp:Repeater runat="server" ID="rptGrupos" OnItemDataBound="rptGrupos_OnItemDataBound">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" Visible="False" ID="lblIdGrupo" Text='<%# Eval("IdGrupo") %>' />
                                                                                    <div class="form-group col-lg-4 col-md-4 padding-5">
                                                                                        <span class="tag label label-info">
                                                                                            <div class="row col-lg-4 col-md-4 padding-5">
                                                                                                <div class="widht100">
                                                                                                    <span style="font-weight: 400" class="text-left"><%# Eval("DescripcionGrupo") %></span>
                                                                                                    <asp:Repeater runat="server" ID="rptSubGrupos">
                                                                                                        <HeaderTemplate>
                                                                                                            <br />
                                                                                                        </HeaderTemplate>
                                                                                                        <ItemTemplate>
                                                                                                            <div class="padding-40-left text-left">
                                                                                                                <br />
                                                                                                                <%# Eval("Descripcion") %>
                                                                                                                <asp:LinkButton runat="server" class="remove el el-remove-circle" Visible="true" ID="btnRemoveRolSub" OnClick="btnRemoveRolSub_OnClick" CommandName='<%# Container.ItemIndex %>' CommandArgument='<%# Eval("Id") %>' />
                                                                                                            </div>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:Repeater>
                                                                                                    <asp:LinkButton runat="server" class="remove el el-remove-circle" ID="btnRemoveRol" Visible='<%# Eval("SubGrupos") == null %>' OnClick="btnRemoveRol_OnClick" CommandName='<%# Eval("IdTipoGrupo") %>' CommandArgument='<%# Eval("IdGrupo") %>' />
                                                                                                    <%--  --%>
                                                                                                </div>
                                                                                            </div>
                                                                                        </span>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <hr />
                                                        </ItemTemplate>
                                                    </asp:Repeater>

                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>

                            <!--Formularios -->
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="display: none">
                                >
                                <div class="panel panel-default">
                                    <div class="panel-heading panel-heading-theme-1">
                                        <h4 class="panel-title">
                                            <div class="row">
                                                <a data-toggle="collapse" class="panel-toggle no-underline" href="#faq5_3">
                                                    <div class="col-lg-10 col-md-8 col-sm-8">
                                                        <h3 class="TitulosAzul">Formularios</h3>
                                                    </div>
                                                </a>
                                                <div class="col-lg-2 col-md-4 col-sm-4 text-right">
                                                    <asp:Button ID="btnModalFormulario" CssClass="btn btn-primary" runat="server" Text="Agregar" />
                                                </div>
                                            </div>
                                        </h4>
                                    </div>


                                    <asp:Panel runat="server" CssClass="panel-collapse collapse" ID="Panel1" ClientIDMode="Static">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <div class="row">
                                                    <div>
                                                        <div class="table-responsive">
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>

                        </div>
                    </div>
                </section>
                <section class="module" id="divBtnGuardar" runat="server" visible="true">
                    <div class="module-inner">
                        <div class="row widht100 text-center">
                            <br />
                            <asp:Button runat="server" Text="Cancelar" ID="btnCancelarEdicion" CssClass="btn btn-default margin-right-15" OnClick="btnCancelarEdicion_OnClick" />
                            <asp:Button runat="server" Text="Guardar" ID="btnGuardar" CssClass="btn btn-primary margin-auto" OnClick="btnGuardar_OnClick" />
                        </div>
                    </div>
                    <br />
                </section>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--PUESTOS--%>
        <div class="modal fade" id="modalAreas" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true" style="overflow: hidden;">
            <asp:UpdatePanel ID="upModalAltaAreas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-dialog modal-md">
                        <div class="modal-content">
                            <uc:UcAltaPuesto runat="server" ID="ucAltaPuesto" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <%--ORGANIZACIONES--%>
        <div class="modal fade" id="modalOrganizacion" data-backdrop-limit="1" tabindex="-1" role="dialog" aria-labelledby="upload-avatar-title" aria-hidden="true" style="overflow: hidden;">
            <asp:UpdatePanel ID="upOrganizacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <uc:UcAltaOrganizaciones runat="server" ID="ucAltaOrganizaciones" />
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <%--UBICACIONES--%>
        <div class="modal fade" id="modalUbicacion" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true" style="overflow: hidden;">

            <asp:UpdatePanel ID="upUbicacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <uc:UcAltaUbicaciones runat="server" ID="ucAltaUbicaciones" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <%--ROLES--%>
        <div class="modal fade" id="modalRoles" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true" style="overflow: hidden;">
            <asp:UpdatePanel ID="upRoles" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc:UcRolGrupo runat="server" ID="ucRolGrupo" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
