var win;

function MostrarPopup(modalName) {

    $(modalName).on('shown.bs.modal', function () {
        $(this).find('[autofocus]').focus();
    });
    $(modalName).modal({ backdrop: 'static', keyboard: false });
    $(modalName).modal({ show: true });

    return true;
};
function CierraPopup(modalName) {
    $(modalName).modal('hide');
    return true;
};

function OpenWindow(url) {
    window.open(url, "test", 'type=fullWindow, fullscreen, height=700,width=760');
};
function ShowLanding() {
    var landing = document.getElementById('updateProgress');
    if (landing != undefined) {
        landing.style.display = 'block';
    }
}


function HideLanding() {
    var landing = document.getElementById('updateProgress');
    if (landing != undefined) {
        landing.style.display = 'none';
    }
}
function sum() {
    var totaldias = 0, totalhoras = 0, totalminutos = 0, totalsegundos = 0;
    $("#tblHeader > tbody > tr").each(function (indexRow) {
        var control;
        $(this).children("td").each(function (indexColumn) {
            switch (indexColumn) {
                case 2:
                    control = $(this).find("input[id*=txtDias]");
                    if (control != null) {
                        totaldias = totaldias + parseInt(control.val() === "" || control.val() === undefined ? 0 : control.val());
                    }
                    break;
                case 3:
                    control = $(this).find("input[id*=txtHoras]");
                    if (control != null) {
                        totalhoras = totalhoras + parseInt(control.val() === "" || control.val() === undefined ? 0 : control.val());
                    }
                    break;
                case 4:
                    control = $(this).find("input[id*=txtMinutos]");
                    if (control != null) {
                        totalminutos = totalminutos + parseInt(control.val() === "" || control.val() === undefined ? 0 : control.val());
                    }
                    break;
                case 5:
                    control = $(this).find("input[id*=txtSegundos]");
                    if (control != null) {
                        totalsegundos = totalsegundos + parseInt(control.val() === "" || control.val() === undefined ? 0 : control.val());
                    }
                    break;
            }

        });
    });

    $("#tblHeader > tfoot > tr").each(function (indexRow) {
        var control;
        $(this).children("td").each(function (indexColumn) {
            switch (indexColumn) {
                case 2:
                    control = $(this).find("input[id*=txtTotalDias]");
                    if (control != null) {
                        control.val(totaldias);
                    }
                    break;
                case 3:
                    control = $(this).find("input[id*=txtTotalHoras]");
                    if (control != null) {
                        control.val(totalhoras);
                    }
                    break;
                case 4:
                    control = $(this).find("input[id*=txtTotalMinutos]");
                    if (control != null) {
                        control.val(totalminutos);
                    }
                    break;
                case 5:
                    control = $(this).find("input[id*=txtTotalSegundos]");
                    if (control != null) {
                        control.val(totalsegundos);
                    }
                    break;
            }
        });
    });
}
function closeit() {
    win.close();
};
function HightSearch(content, serachText) {
    var srcStr = $("#" + content).html();
    var term = serachText;
    term = term.replace(/(\s+)/, "(<[^>]+>)*$1(<[^>]+>)*");
    var pattern = new RegExp("(" + term + ")", "gi");

    srcStr = srcStr.replace(pattern, "<mark>$1</mark>");
    srcStr = srcStr.replace(/(<mark>[^<>]*)((<[^>]+>)+)([^<>]*<\/mark>)/, "$1</mark>$2<mark>$4");

    $("#test").html(srcStr);
}

function hidden(tablename) {
    $(tablename + ' tr').on('mouseover', function () {
        $(this).find('#hiddenEditar').removeClass("hidden");
        $(this).find('#hiddenClonar').removeClass("hidden");
    }).on('mouseout', function () {
        $(this).find('#hiddenEditar').addClass("hidden");
        $(this).find('#hiddenClonar').addClass("hidden");
    });
}

function search(evt) {
    debugger;
    var charCode = (window.Event) ? evt.which : evt.keyCode;
    //if (evt.charCode == 13) {
    if (charCode == 13) {
        this.__doPostBack('Buscador', 'OnKeyPress');
        evt.preventDefault();
        return false;
    }
    else {
        return true;
    }
}
function sayHi(e) {
    e.preventDefault();
}

function DontCloseMenu(event) {
    event.stopPropagation();
};

function load() {
    var dir = "http://localhost:2802/assets/carouselImage/";
    var fileextension = ".jpg";
    $.ajax({
        url: dir,
        success: function (data) {
            $(data).find("a:contains(" + fileextension + ")").each(function () {
                var filename = this.href.replace(window.location.host, "").replace("http:///", "");
                $("body").append($("<img src=" + dir + filename + "></img>"));
            });
        }
    });
}
