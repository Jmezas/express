$(function () {
    fechaActual();
    Lista.CargarMoneda();
    
    $("#btnApertura").click(function () {

        var oDatos = {
            usuario: { Usuario: $("#lstUsuario").val() },
            caja: { Id: $("#lstCaja").val() },
            moneda: { IdMoneda: $("#lstMoneda").val() },
            montoApertura: $("#txtMonto").val(),
            estadoApertura: 1
        }
        

        if (oDatos.usuario.Usuario == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'No ha seleccionado el usuario');
        }
        else if (oDatos.caja.Id == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'No ha seleccionado la caja');
        }
        else if (oDatos.moneda.IdMoneda == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'No ha seleccionado la moneda');
        }
        else if (oDatos.montoApertura.length == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'No ha ingresado el monto de apertura');
        }
        else {

            $.ajax({
                async: true,
                type: 'post',
                url: General.Utils.ContextPath('Venta/RegistrarApertura'),
                dataType: 'json',


                data: { Apertura: oDatos },
                success: function (response) {
                    if (response["Id"] == TypeMessage.Success) {

                        Swal.fire(
                            'Exito!',
                            response.Message,
                            response.Id
                        )

                    } else {

                        Swal.fire(
                            'Error!',
                            response.Message,
                            response.Id
                        )
                    }
                    listadoApertura();

                    Limpiar();
                }
            });
        }
    });

    setTimeout(function myfunction() {
        Lista.CargarUsuario();
        Lista.CargarCaja();
        listadoApertura();
    }, 500);
});

function Limpiar() {
    $("#lstUsuario").val('0');
    $("#lstCaja").val('0');
    $("#lstMoneda").val('0');
    $("#txtMonto").val('');
}
function fechaActual() {
    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();

    var fechaActual = (day < 10 ? '0' : '') + day + '/' + (month < 10 ? '0' : '') + month + '/' + d.getFullYear();
    $("#txtFecha").val(fechaActual);
}
var Lista = {
    CargarUsuario: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Venta/ListaComboUsuario"),
            dataType: 'json',           
            success: function (response) {

                $("#lstUsuario").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 

                    $.grep(response, function (oDocumento) {
                        $('select[name="lstUsuario"]').append($('<option>', { value: oDocumento["Usuario"], text: oDocumento["Nombres"] }));
                    });
                }
            }

        });
    },
    CargarCaja: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Venta/ListadoCaja"),
            dataType: 'json',           
            success: function (response) {

                $("#lstCaja").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 

                    $.grep(response, function (data) {
                        console.log(data);
                        $('select[name="lstCaja"]').append($('<option>', { value: data["Id"], text: data["Text"] }));
                    });
                }
            }

        });
    },
    CargarMoneda: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Venta/ListaMoneda"),
            dataType: 'json',
            success: function (response) {
                $("#lstMoneda").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error                
                    $.grep(response, function (data) {
                        $('select[name="lstMoneda"]').append($('<option>', { value: data["IdMoneda"], text: data["Nombre"] }));
                    });
                }
            }

        });
    }
}

function listadoApertura() {  

    $.ajax({
        async: true,
        type: 'post',
        url: General.Utils.ContextPath('venta/ListadoApertura'),
        dataType: 'json',        
        success: function (response) {

            var $tb = $("#tbApertura");
            $tb.find('tbody').empty();

            if (response.length == 0) {
                $tb.find('tbody').html('<tr><td colspan="20">No hay ninguna apertura de caja</td></tr>');
            } else {
                $.grep(response, function (item) {

                    $tb.find('tbody').append(
                        '<tr>' +
                        '<td>' + item["fechaApertura"] + '</td>' +
                        '<td>' + item["usuario"]["Nombres"] + '</td>' +
                        '<td>' + item["caja"]["Nombre"] + '</td>' +
                        '<td>' + item["moneda"]["Nombre"] + '</td>' +
                        '<td>' + formatNumber(item["montoApertura"]) + '</td>' +
                        '</tr>'
                    );
                });
            }
        }
    });
}