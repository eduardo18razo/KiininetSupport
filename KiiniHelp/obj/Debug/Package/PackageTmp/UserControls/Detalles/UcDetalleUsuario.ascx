<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcDetalleUsuario.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcDetalleUsuario" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleUbicacion.ascx" TagPrefix="uc1" TagName="UcDetalleUbicacion" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleOrganizacion.ascx" TagPrefix="uc1" TagName="UcDetalleOrganizacion" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleGrupoUsuario.ascx" TagPrefix="uc1" TagName="UcDetalleGrupoUsuario" %>
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
<div style="height: 100%;">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <br>
            <h3 class="h6">
                <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink>
                / Empleado / Mi perfil</h3>
            <hr>
            <section class="module">
                <div class="row">
                    <div class="col-lg-8 col-md-9">
                        <div class="module-inner">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" Text="Mi Perfil"></asp:Label></h3>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="module">
                    <div class="row">
                        <div class="col-lg-12 col-md-12">
                            <div class="module-inner">
                                <div class="form-group avatar">
                                    <figure class="figure col-lg-1 col-md-2 col-sm-2 center-content-div">
                                        <asp:Image CssClass="img-rounded img-responsive" ImageUrl="~/assets/images/profiles/profile-square-1.png" alt="" runat="server" />
                                        <asp:LinkButton runat="server" Text="Cambiar"></asp:LinkButton>
                                    </figure>
                                    <div class="form-group col-lg-10 col-md-10 col-sm-10">
                                        <h2>
                                            <asp:Label runat="server" ID="lblnombreCompleto" CssClass="col-lg-11 col-md-10 col-sm-10" />
                                        </h2>
                                    </div>
                                    <div class="form-group col-lg-10 col-md-10 col-sm-10">
                                        <asp:Label runat="server" Text="Ultimo Acceso" CssClass="col-md-12"></asp:Label>
                                        <asp:Label runat="server" ID="lblFechaUltimoAcceso" Text="Fecha Ultimo Acceso" CssClass="col-md-12"></asp:Label>
                                    </div>
                                    
                                <hr />
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
                                        <asp:TextBox ID="txtAp" runat="server" CssClass="form-control" placeholder="Apellido paterno" MaxLength="32" />
                                    </div>
                                    <div class="col-lg-3 col-md-3">
                                        <asp:TextBox ID="txtAm" runat="server" CssClass="form-control" placeholder="Apellido materno" MaxLength="32" />
                                    </div>
                                    <div class="col-lg-3 col-md-3">
                                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Nombre" MaxLength="32" />
                                    </div>
                                    <div class="col-lg-3 col-md-3">
                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control texto-normal" MaxLength="12" Style="text-transform: none" />
                                    </div>
                                </div>

                                <!--FILA 2-->
                                <div class="row">
                                    <br />
                                    <div class="col-lg-3 col-md-3" runat="server" id="divPuesto">
                                        Puesto*
                                    <asp:DropDownList runat="server" ID="ddlPuesto" CssClass="form-control" />
                                    </div>
                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                        <br />
                                        <div class="form-inline">
                                            <label for="chkVip" class="col-lg-10 col-md-8 col-sm-8">VIP</label>
                                            <asp:CheckBox runat="server" Text="VIP" ID="chkVip" CssClass="chkIphone chkIphone col-lg-2 col-md-4 col-sm-4" />
                                        </div>
                                        <div class="form-inline">
                                            <label for="chkVip" class="col-lg-10 col-md-8 col-sm-8">Directorio activo</label>
                                            <asp:CheckBox runat="server" Text="Directorio Activo " ID="chkDirectoriActivo" CssClass="chkIphone chkIphone col-lg-2 col-md-4 col-sm-4" />
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
                                    <asp:Repeater runat="server" ID="rptTelefonos" OnItemDataBound="rptTelefonos_OnItemDataBound">
                                        <ItemTemplate>
                                            <div style="border-radius: 20px; margin-bottom: 5px; height: auto">
                                                <div class="row">
                                                    <div class="col-xs-5 col-md-3">
                                                        <asp:Label runat="server" ID="txtNumero" Text='<%# Eval("Numero") %>' CssClass="form-control" onkeypress="return ValidaCampo(this,2)" MaxLength="10" />
                                                    </div>
                                                    <div class="col-lg-3 col-md-3" runat="server" id="divExtension">
                                                        <asp:TextBox runat="server" ID="TextBox1" Text='<%# Eval("Extension") %>' CssClass="form-control" onkeypress="return ValidaCampo(this,15)" MaxLength="40" />
                                                    </div>
                                                    <div class="col-lg-3 col-md-3">
                                                        <asp:DropDownList runat="server" ID="ddlTipoTelefono" CssClass="form-control" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
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
                                    <asp:Repeater runat="server" ID="rptCorreos">
                                        <ItemTemplate>
                                            <asp:Label runat="server" CssClass="form-control" Text='<%# Eval("Correo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:Repeater>
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
                                    <div>
                                        <div class="table-responsive">
                                            <uc1:UcDetalleOrganizacion runat="server" ID="UcDetalleOrganizacion" />
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
                                    <div>
                                        <div class="table-responsive">
                                            <uc1:UcDetalleUbicacion runat="server" ID="UcDetalleUbicacion" />
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
                                <hr />
                                <div class="row">
                                    <uc1:UcDetalleGrupoUsuario runat="server" ID="UcDetalleGrupoUsuario" />

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
 <asp:Button runat="server" CssClass="btn btn-lg btn-danger" ID="btnCerrarModal" Text="Cerrar" OnClick="btnCerrarModal_OnClick" Visible="False" />
            <%--<div class="panel-footer" style="text-align: center">--%>
               
                <%--
                    <asp:Button runat="server" CssClass="btn btn-lg btn-danger" ID="btnModificar" Text="Cerrar" OnClick="btnCerrarModal_OnClick" />
                    <asp:Button runat="server" CssClass="btn btn-lg btn-danger" ID="Button2" Text="Cerrar" OnClick="btnCerrarModal_OnClick" />--%>
            <%--</div>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
