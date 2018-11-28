<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcParametrosGenerales.ascx.cs" Inherits="KiiniHelp.UserControls.Configuracion.UcParametrosGenerales" %>

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
            <triggers>
                <asp:PostBackTrigger ControlID="btnUpload" />
            </triggers>
            <ContentTemplate>
                <div class="row">
                    <div>
                        <ol class="breadcrumb">
                            <li>
                                <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                            <li class="active">Parametros</li>
                        </ol>
                    </div>
                </div>

                <section class="module">
                    <div class="module-inner">
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <asp:HyperLink data-toggle="collapse" ID="pnlPrincipal" runat="server" CssClass="no-underline" NavigateUrl="#datosempresa">

                                                <div class="row">
                                                    <div class="col-lg-10 col-md-8 col-sm-8 col-xs-6">
                                                        <h3 class="TitulosAzul">
                                                            <asp:Label runat="server" ID="lblTitle" Text="Datos de la Empresa" /></h3>
                                                    </div>
                                                </div>
                                            </asp:HyperLink>
                                        </h4>
                                    </div>

                                    <asp:Panel runat="server" CssClass="panel-collapse " ID="datosempresa" ClientIDMode="Static">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">

                                                <div class="row">
                                                    <div class="col-lg-1 col-md-2 col-sm-6 col-xs-12">
                                                        <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="Upload" CssClass="ocultar" ClientIDMode="Static" />
                                                        <div class="form-group avatar" runat="server" id="divAvatar" visible="True">
                                                            <figure class="figure col-lg-10 col-md-12 col-sm-6 col-xs-6 center-content-div center-block centered">
                                                                <asp:Image CssClass="img-rounded img-responsive margin-top-25 center-block imageCircle image80" ImageUrl="~/assets/images/profiles/profile-1.png" ID="Image1" alt="imgPerfil" runat="server" />

                                                                <asp:Panel ID="Panel1" runat="server" Style="position: relative; overflow: Hidden; cursor: pointer; max-height: 165px; max-width: 165px;">
                                                                    <asp:FileUpload runat="server" ID="FileUpload2" Style="position: absolute; left: -20px; z-index: 2; opacity: 0; filter: alpha(opacity=0); cursor: pointer; height: 56px" Enabled="false" />
                                                                    <asp:LinkButton runat="server" Text="Editar" Style="margin-top: 10px;" ID="LinkButton1" ClientIDMode="Static" CssClass="btn btn-primary margin-top-10" />
                                                                </asp:Panel>
                                                            </figure>
                                                            <div class="col-lg-10 col-md-12 col-sm-6 col-xs-6 center-content-div center-block centered">
                                                                <label>180x65 PX (Recomendado)</label>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-11 col-md-11">
                                                        <div class="row" runat="server" id="div1">
                                                            <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12 padding-20-left">
                                                                <label>Nombre de la Empresa</label>
                                                                <asp:TextBox ID="txtNombreEmpresa" runat="server" CssClass="form-control" onkeypress="return ValidaCampo(this,1)" AutoPostBack="true" MaxLength="32" />
                                                            </div>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>
