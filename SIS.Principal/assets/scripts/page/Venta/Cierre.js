$(function () {
    fechaActual();
    $("#hdf_Pagina").val('1');
    Lista.CargarMoneda();    

    setTimeout(function myfunction() {
        listadoAperturaTodo();
    }, 500);

    $("#btnCerrar").click(function () {

        var oDatos = {
            moneda: { IdMoneda: $("#lstMoneda").val() },
            estadoApertura: 2
        }        

        if (oDatos.moneda.IdMoneda == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'No ha seleccionado la moneda');
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
                    listadoAperturaTodo();

                    Limpiar();
                }
            });
        }
    });

    $("#btnAnterior").click(function () {
        let numPaginas = parseInt($("#hdf_Pagina").val());
        let TotalPagina = parseInt($("#hdf_TotalPagina").val());
        if (numPaginas == 0 || numPaginas == 1) {
            General.Utils.ShowMessage(TypeMessage.Information, 'Límite de Página..');

        } else {
            numPaginas = numPaginas - 1

            $("#hdf_Pagina").val(numPaginas);

            $("#lblNumPagina").html(numPaginas + '  de  ' + TotalPagina);
            listadoAperturaTodo();
        }

    });

    $("#btnSiguiente").click(function () {
        let numPaginas = parseInt($("#hdf_Pagina").val());
        let TotalPagina = parseInt($("#hdf_TotalPagina").val());

        if (TotalPagina == numPaginas) {
            General.Utils.ShowMessage(TypeMessage.Information, 'Límite de Página..');
        } else {
            numPaginas = numPaginas + 1
            $("#hdf_Pagina").val(numPaginas);
            $("#lblNumPagina").html(numPaginas + '  de  ' + TotalPagina);

            listadoAperturaTodo();
        }
    });
});

function fechaActual() {
    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();

    var fechaActual = (day < 10 ? '0' : '') + day + '/' + (month < 10 ? '0' : '') + month + '/' + d.getFullYear();
    $("#txtFecha").val(fechaActual);
}

function Limpiar() {
    $("#lstMoneda").val('0');
}

var Lista = {

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

function listadoAperturaTodo() {    
    var numPaginas = parseInt($("#hdf_Pagina").val()),
        AllReg = 0;
    let DesPagina;

    $.ajax({
        async: true,
        type: 'post',
        url: General.Utils.ContextPath('venta/ListaAperturaTodo'),
        dataType: 'json',
        data: { numPag: numPaginas, allReg: AllReg, Cant: 10 },
        success: function (response) {

            var $tb = $("#tbCierre");
            $tb.find('tbody').empty();

            if (response.length == 0) {
                $tb.find('tbody').html('<tr><td colspan="20">No hay registros disponibles</td></tr>');
                $('#pHelperProductos').html('');
                $("#lblNumPagina").html('0 de 0');
            } else {
                $("#hdf_TotalPagina").val(response[0].TotalPagina);
                DesPagina = $("#hdf_Pagina").val() + "  de  " + response[0].TotalPagina;
                $.grep(response, function (item) {

                    $tb.find('tbody').append(
                        '<tr>' +
                        '<td>' + item["usuario"]["Nombres"] + '</td>' +
                        '<td>' + item["caja"]["Nombre"] + '</td>' +
                        '<td>' + item["moneda"]["Nombre"] + '</td>' +
                        '<td>' + item["fechaApertura"] + '</td>' +
                        '<td>' + formatNumber(item["montoApertura"]) + '</td>' +
                        '<td>' + item["fechaCierre"] + '</td>' +
                        '<td>' + formatNumber(item["montoCierre"]) + '</td>' +
                        '<td>' + item["estadoApertura"] + '</td>' +
                        '</tr>'
                    );
                });
                $("#TotalReg").val(response[0]["TotalR"]);
                $('#pHelperProductos').html('Existe(n) ' + response[0]["TotalR"] + ' resultado(s) para mostrar.');
                $("#lblNumPagina").html(DesPagina);
            }

        }
    });
}