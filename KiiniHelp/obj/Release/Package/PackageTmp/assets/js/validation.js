function trim(s) {
    return s.replace(/^\s+|\s+$/, '');
}

function ValidaRfc(rfcStr) {
    var strCorrecta;
    strCorrecta = rfcStr;
    var valid;
    if (rfcStr.length == 12) {
        valid = '^(([A-Z]|[a-z]){3})([0-9]{6})((([A-Z]|[a-z]|[0-9]){3}))';
    } else {
        valid = '^(([A-Z]|[a-z]|\s){1})(([A-Z]|[a-z]){3})([0-9]{6})((([A-Z]|[a-z]|[0-9]){3}))';
    }
    var validRfc = new RegExp(valid);
    var matchArray = strCorrecta.match(validRfc);
    if (matchArray == null) {
        alert('RFC no Valida');
        return "";
    }
    else {
        return rfcStr.toUpperCase();
    }

}


function ValidaCampo(objeto, tipo) {
    debugger;
    var longitudValor = objeto.value.length + 1;
    var subCadena = String.fromCharCode(window.event.keyCode).toUpperCase();
    var tCadena = objeto.value;
    var conPunto = 0;
    var valido = true;

    //alert("Codigo:" + window.event.keyCode)

    var cadena = "";
    var cadStr = '0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZ/-$%.,() ';
    switch (tipo) {
        case 1:  //Letras			    
            cadStr = 'ABCDEFGHIJKLMNÑOPQRSTUVWXYZ. ';
            break;
        case 2: //numeros
            cadStr = "0123456789";
            break;
        case 3:  //Letras y numeros
            cadStr = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ/-$%.,() ';
            break;
        case 4:  //con Decimales 
            cadStr = '0123456789.';
            break;
        case 5:  //Hora
            cadStr = '0123456789:';
            break;
        case 6:  //CURP y RFC
            cadStr = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ';
            break;
        case 7:  //Especial
            cadStr = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ- ';
            break;
        case 8:  //Telefono
            cadStr = '0123456789-';
            break;
        case 9:  //Fecha
            cadStr = '0123456789/';
            break;
        case 10:  //DOMICILIO
            cadStr = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ#. ';
            break;
        case 11:  //numeros SIN CEROS
            cadStr = '123456789';
            break;
        case 12: // SOLO numeroS O PALABRA 'ADMON' 
            cadStr = '0123456789ADMON';
            break;
        case 13:  //e-mail
            cadStr = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ@-_.';;
            break;
        case 14:  //USUARIO
            cadStr = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-_.';;
            break;
        case 15: //numeros
            cadStr = "0123456789, ";
            break;
    }

    if (tipo == 4) {
        for (i = 1; i <= tCadena.length; i++) {
            if (tCadena.substring(i, i - 1) == '.') {
                conPunto = 1;
                break;
            }
        }
    }
    //|| (window.event.keyCode == 209) || (window.event.keyCode == 241)
    if (longitudValor > 0) {
        for (i = 1; i <= cadStr.length; i++) {
            if ((cadStr.substring(i, i - 1) == subCadena)) {
                cadena = cadStr.substring(i, i - 1);

                if ((window.event.keyCode == 241) && ((tipo == 2) || (tipo == 4))) {
                    return false;
                }

                if (cadena == '.' && conPunto == '1') {
                    return false;
                }
            }
        }

        if (cadena.length == 0) {
            valido = false;
        }
        else {
            valido = true;
        }
        return valido;
    }
}


function validateEmail(fld) {
    var error = "";
    var tfld = trim(fld.value);                        // value of field with whitespace trimmed off
    var emailFilter = /^[^@]+@[^@.]+\.[^@]*\w\w$/;
    var illegalChars = /[\(\)\<\>\,\;\:\\\"\[\]]/;

    if (fld.value == "") {
        error = "You didn't enter an email address.\n";
    } else if (!emailFilter.test(tfld)) {              //test email for illegal characters
        error = "Please enter a valid email address.\n";
    } else if (fld.value.match(illegalChars)) {
        error = "The email address contains illegal characters.\n";
    } else {
        //fld.style.background = 'White';
    }
    return error;
}

function validatePhone(fld) {
    var error = "";
    var stripped = fld.value.replace(/[\(\)\.\-\ ]/g, '');

    if (fld.value == "") {
        error = "You didn't enter a phone number.\n";
    } else if (isNaN(parseInt(stripped))) {
        error = "The phone number contains illegal characters.\n";
    } else if (!(stripped.length == 10)) {
        error = "The phone number is the wrong length. Make sure you included an area code.\n";
    }
    return error;
}

function validateRFC(cual) {
    // var mensaje = "";
    var pat = /[a-z]|[A-Z]/;
    var pat2 = /[a-z]|[A-Z]|[0-9]/;
    var val = cual.split("-");
    if (val.length == 3) {
        if (val[0].length == 4) {
            if (!comp(val[0], pat)) {
                //alert( mensaje)
                return false;
            }
        }
        if (val[1].length == 6) {
            if (isNaN(val[1])) {
                //alert('no es un numero')
                return false;
            }
        }
        if (val[2].length == 3) {
            if (!comp(val[2], pat2)) {
                //alert(mensaje)
                return false;
            }
        } else {
            //alert(mensaje)
            return false;
        }
    }
    else {
        //alert(mensaje)
        return false;
    }
    return true;
}

function comp(cual, pa) {
    var m;
    for (m = 0; m < cual.length; m++) {
        if (!pa.test(cual.charAt(m))) {
            return false;
        }
    }
    return true;
}


var validationErrors = new Array();

function validate() {

    console.log('validating ' + form + '...');

    if (form == 'consultas1') {

        if (document.getElementById('validate001').value == '' &&
            document.getElementById('validate002').value == '' &&
            document.getElementById('validate003').value == '' &&
            document.getElementById('validate004').value == '') {
            //Busqueda
            validationErrors.push('SomethingMissing');
            return false;
        }

        date00 = new Date(document.getElementById('validate005').value);
        date01 = new Date(document.getElementById('validate006').value);
        if (date00 > date01) {
            //Fechas incorrectas
            validationErrors.push('SomethingMissing');
            return false;
        }
    }

    if (form == 'individual1') {

        if (document.getElementById('validate001').value == '' &&
            document.getElementById('validate002').value == '' &&
            document.getElementById('validate003').value == '') {
            //Nombre completo
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate004').selectedIndex == 'Dia') {
            //Falta dia
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate005').selectedIndex == 'Mes') {
            //Falta mes
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate006').selectedIndex == 'Año') {
            //Falta año
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate007').selectedIndex == 'Género') {
            //Falta género
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate008').selectedIndex == 'Soltero') {
            //Falta Estado Civil
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate009').value == '') {
            //Falta Correo electrónico
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (validateEmail(document.getElementById('validate009').value) != '') {
            //Correo electrónico Incorrecto
            validationErrors.push('SomethingMissing');
            return false;
        }

    }

    if (form == 'individual4') {

        if (document.getElementById('validate001').selectedIndex == 'Soltero') {
            //Falta Estado Civil
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate002').value == '') {
            //Calle
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate003').value == '') {
            //Exterior
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate005').value == '') {
            //Colonia
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate006').value == '') {
            //Codigo Postal
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate007').value == '') {
            //Delegación
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate008').value == '') {
            //Ciudad
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate100').selectedIndex == 'Estado') {
            //Falta Estado Civil
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate009').value == '') {
            //Celular
            validationErrors.push('SomethingMissing');
            return false;
        }
        if (validatePhone(document.getElementById('validate009').value) != '') {
            //Celular incorrecto
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate010').value == '') {
            //Tel Casa
            validationErrors.push('SomethingMissing');
            return false;
        }
        if (validatePhone(document.getElementById('validate010').value) != '') {
            //Tel Casa incorrecto
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate012').value == '') {
            //RFC
            validationErrors.push('SomethingMissing');
            return false;
        }
        if (!validateRFC(document.getElementById('validate012').value)) {
            //RFC incorrecto
            validationErrors.push('SomethingMissing');
            return false;
        }
    }

    if (form == 'masivos1') {

        if (document.getElementById('validate001').selectedIndex == 'Planes') {
            //Falta Plan
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate002').value == '') {
            //Nombre
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate003').value == '') {
            //Nombre
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate004').value == '') {
            //Nombre
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate005').selectedIndex == 'Dia') {
            //Falta dia
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate006').selectedIndex == 'Mes') {
            //Falta mes
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate007').selectedIndex == 'Año') {
            //Falta año
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate008').selectedIndex == 'Género') {
            //Falta género
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate009').selectedIndex == 'Soltero') {
            //Falta Ocupacion
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate010').value == '') {
            //Falta Correo electrónico
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (validateEmail(document.getElementById('validate010').value) != '') {
            //Correo electrónico Incorrecto
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate011').selectedIndex == 'Soltero') {
            //Falta Estado Civil
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate012').value == '') {
            //Calle
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate013').value == '') {
            //Exterior
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate015').value == '') {
            //Colonia
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate016').value == '') {
            //Codigo Postal
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate017').value == '') {
            //Delegación
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate018').value == '') {
            //Ciudad
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate019').selectedIndex == 'Estado') {
            //Falta Estado
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate020').value == '') {
            //Celular
            validationErrors.push('SomethingMissing');
            return false;
        }
        if (validatePhone(document.getElementById('validate020').value) != '') {
            //Celular incorrecto
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate021').value == '') {
            //Tel Casa
            validationErrors.push('SomethingMissing');
            return false;
        }
        if (validatePhone(document.getElementById('validate021').value) != '') {
            //Tel Casa incorrecto
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate023').value == '') {
            //RFC
            validationErrors.push('SomethingMissing');
            return false;
        }
        if (!validateRFC(document.getElementById('validate023').value)) {
            //RFC incorrecto
            validationErrors.push('SomethingMissing');
            return false;
        }


    }

    if (form == 'reportes1') {

        if (document.getElementById('agnt').value == '') {
            //Nombre del Agente
            validationErrors.push('SomethingMissing');
            return false;
        }

        var date00 = new Date(document.getElementById('desde2').value);
        var date01 = new Date(document.getElementById('hasta1').value);
        if (date00 > date01) {
            //Fechas incorrectas
            validationErrors.push('SomethingMissing');
            return false;
        }

    }

    if (form == 'movil-consultas1') {

        if (document.getElementById('validate001').value == '') {
            //Nombre
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate002').value == '') {
            //Paterno
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate003').value == '') {
            //Materno
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate004').value == '') {
            //Numero
            validationErrors.push('SomethingMissing');
            return false;
        }


        if (document.getElementById('validate005').selectedIndex == '0') {
            //Desde-Dia
            validationErrors.push('SomethingMissing');
            return false;
        }
        if (document.getElementById('validate006').selectedIndex == '0') {
            //Desde-Mes
            validationErrors.push('SomethingMissing');
            return false;
        }
        if (document.getElementById('validate007').selectedIndex == '0') {
            //Desde-Año
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate008').selectedIndex == '0') {
            //Hasta-Dia
            validationErrors.push('SomethingMissing');
            return false;
        }
        if (document.getElementById('validate009').selectedIndex == '0') {
            //Hasta-Mes
            validationErrors.push('SomethingMissing');
            return false;
        }
        if (document.getElementById('validate010').selectedIndex == '0') {
            //Hasta-Año
            validationErrors.push('SomethingMissing');
            return false;
        }

        date00 = new Date(document.getElementById('validate006').selectedIndex + '/' + document.getElementById('validate005').selectedIndex + '/' + document.getElementById('validate007').selectedIndex);
        date01 = new Date(document.getElementById('validate009').selectedIndex + '/' + document.getElementById('validate009').selectedIndex + '/' + document.getElementById('validate010').selectedIndex);

        console.log(date00 + ' y ' + date01);
        if (date00 > date01) {
            //Fechas incorrectas
            validationErrors.push('SomethingMissing');
            return false;
        }

    }


    if (form == 'movil-consultas1') {

        if (document.getElementById('validate001').value == '') {
            //Nombre
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate002').value == '') {
            //Paterno
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate003').value == '') {
            //Materno
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate004').selectedIndex == '0') {
            //DOB-Dia
            validationErrors.push('SomethingMissing');
            return false;
        }
        if (document.getElementById('validate005').selectedIndex == '0') {
            //DOB-Mes
            validationErrors.push('SomethingMissing');
            return false;
        }
        if (document.getElementById('validate006').selectedIndex == '0') {
            //DOB-Año
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate007').selectedIndex == '0') {
            //Genero
            validationErrors.push('SomethingMissing');
            return false;
        }

        if (document.getElementById('validate008').selectedIndex == '0') {
            //Ocupacion
            validationErrors.push('SomethingMissing');
            return false;
        }

    }


    return true;
}