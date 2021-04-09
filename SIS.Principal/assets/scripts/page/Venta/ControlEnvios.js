
$(function () {
    $("#hdf_Pagina").val('1');
    ListaGeneral();

    $('#fechaFin').datepicker({
        dateFormat: 'dd/mm/yy',
        prevText: '<i class="fa fa-angle-left"></i>',
        nextText: '<i class="fa fa-angle-right"></i>'
    });

    $('#fechaInicio').datepicker({
        dateFormat: 'dd/mm/yy',
        prevText: '<i class="fa fa-angle-left"></i>',
        nextText: '<i class="fa fa-angle-right"></i>'
    });

    $("#btnFiltrar").click(function () {
        ListaGeneral();
        //window.$('#Report').modal();
        // $('#Report').modal('show');
    })
    $("#txtBuscar").change(function () {

        $("#txtBuscar").val();
        ListaGeneral()
    });
    $("#btnFactura").click(function () {
        var inicio = $('#fechaInicio').val().split(/\//);
        inicio = [inicio[1], inicio[0], inicio[2]].join('/');

        var Fin = $('#fechaFin').val().split(/\//);
        Fin = [Fin[1], Fin[0], Fin[2]].join('/');

        if (inicio == "//") {
            inicio = "";
        }
        if (Fin == "//") {
            Fin = "";
        }
        var Filtro = $("#txtBuscar").val(),
            FechaInicio = inicio,
            FechaFin = Fin,
            numPaginas = parseInt($("#hdf_Pagina").val()),
            AllReg = $("#IdTotal").is(':checked') === true ? 0 : 1,
            CantiFill = $("#TotalReg").val();

        window.location.href = General.Utils.ContextPath('Venta/ReportePDF?filtro=' + Filtro + "&FechaIncio=" + FechaInicio + "&FechaFin=" + FechaFin +
            "&numPag=" + numPaginas + "&allReg=" + AllReg + "&Cant=" + CantiFill + "&venta=" + 0);
    });

    $("#btnExcel").click(function () {
        var inicio = $('#fechaInicio').val().split(/\//);
        inicio = [inicio[1], inicio[0], inicio[2]].join('/');

        var Fin = $('#fechaFin').val().split(/\//);
        Fin = [Fin[1], Fin[0], Fin[2]].join('/');

        if (inicio == "//") {
            inicio = "";
        }
        if (Fin == "//") {
            Fin = "";
        }

        var Filtro = $("#txtBuscar").val(),
            FechaInicio = inicio,
            FechaFin = Fin,
            numPaginas = parseInt($("#hdf_Pagina").val()),
            AllReg = $("#IdTotal").is(':checked') === true ? 0 : 1,
            CantiFill = $("#TotalReg").val();
        window.location.href = General.Utils.ContextPath('Venta/ReporteExcelControlEnvios?cliente=' + Filtro + "&FechaIncio=" + FechaInicio + "&FechaFin=" + FechaFin +
            "&numPag=" + numPaginas + "&allReg=" + AllReg + "&Cant=" + CantiFill + "&venta=" + 0);
    });


    $("#btnAnteror").click(function () {
        let numPaginas = parseInt($("#hdf_Pagina").val());
        let TotalPagina = parseInt($("#hdf_TotalPagina").val());
        if (numPaginas == 0 || numPaginas == 1) {
            General.Utils.ShowMessage(TypeMessage.Information, 'Límite de Página..');

        } else {
            numPaginas = numPaginas - 1

            $("#hdf_Pagina").val(numPaginas);

            $("#lblNumPagina").html(numPaginas + '  de  ' + TotalPagina);
            ListaGeneral();
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

            ListaGeneral();
        }
    });

    $("#tbVenta").on('click', 'tbody .evento', function () {
        var data = $(this).closest('tr').attr('data-id');
        //imprimirFacBol
        //ImprimirFacturaBolTikect
        var URL = General.Utils.ContextPath('Venta/imprimirFacBol?Id=' + data + "&Envio=" + 1 + "&venta=" + 0);
        // console.log(URL); 
        fileDownnload(URL);
    })


    //cambiar de estado a enviado
    $("#tbVenta").on('click', 'tbody .Enviado', function () {
        var data = $(this).closest('tr').attr('data-id');
        $("#hdfId").val(data);
        var Nombre = $(this).parents("tr").find("td").eq(0).text();
        $("#Nombre").text(Nombre);

        Swal.fire({
            title: ' ',
            text: '¿Los productos del comprobante ' + Nombre + ' ya fueron enviados?',
            icon: 'success',
            showCancelButton: true,
            confirmButtonText: 'Confirmar',
            cancelButtonColor: '#d33',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    async: true,
                    type: 'post',
                    url: General.Utils.ContextPath('venta/EstadoEnviadoPOS'),
                    dataType: 'json',
                    data: { IdVenta: $("#hdfId").val(), Motivo: "", Flag: 1 }, //1 => estado enviado
                    success: function (response) {
                        console.log(response)
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

                        ListaGeneral();
                    }
                });

            }
        })
    });

    //cambiar de estado a cancelado
    $("#tbVenta").on('click', 'tbody .Cancelado', function () {
        var data = $(this).closest('tr').attr('data-id');
        $("#hdfId").val(data);
        var Nombre = $(this).parents("tr").find("td").eq(0).text();
        $("#Nombre").text(Nombre);

        Swal.fire({
            title: ' ',
            text: '¿Estas seguro de cancelar el evío del comprobante' + Nombre + ' ?',
            input: 'text',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Confirmar',
            cancelButtonColor: '#d33',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            },
            inputAttributes: {
                autocapitalize: 'off'
            }, preConfirm: (login) => {
                $.ajax({
                    async: true,
                    type: 'post',
                    url: General.Utils.ContextPath('venta/EstadoEnviadoPOS'),
                    dataType: 'json',
                    data: { IdVenta: $("#hdfId").val(), Motivo: login, Flag: 2 }, //1 => estado cancelado
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

                        ListaGeneral();
                    }
                });
            },
            allowOutsideClick: () => !Swal.isLoading()
        })

    });
});

function ListaGeneral() {
    var inicio = $('#fechaInicio').val().split(/\//);
    inicio = [inicio[1], inicio[0], inicio[2]].join('/');

    var Fin = $('#fechaFin').val().split(/\//);
    Fin = [Fin[1], Fin[0], Fin[2]].join('/');

    if (inicio == "//") {
        inicio = "";
    }
    if (Fin == "//") {
        Fin = "";
    }
    var Filtro = $("#txtBuscar").val(),
        FechaInicio = inicio,
        FechaFin = Fin,
        numPaginas = parseInt($("#hdf_Pagina").val()),
        AllReg = $("#IdTotal").is(':checked') === true ? 0 : 1;
    let DesPagina;
    $.ajax({
        async: true,
        type: 'post',
        url: General.Utils.ContextPath('venta/ListaVentaPOSEnvios'),
        dataType: 'json',
        beforeSend: General.Utils.StartLoading,
        complete: General.Utils.EndLoading,
        data: { filtro: Filtro, FechaIncio: FechaInicio, FechaFin: FechaFin, numPag: numPaginas, allReg: AllReg, Cant: 10 },
        success: function (response) {

            var $tb = $("#tbVenta");
            $tb.find('tbody').empty();
            if (response["Total"] == 0) {
                return false;
            }
            if (response.length == 0) {
                $tb.find('tbody').html('<tr><td colspan="20">No hay resultados para el filtro ingresado</td></tr>');
            } else {
                $("#hdf_TotalPagina").val(response[0].TotalPagina);
                DesPagina = $("#hdf_Pagina").val() + "  de  " + response[0].TotalPagina;
                $.grep(response, function (item) {

                    if (item["EstadoEnvio"] == "ENVIADO" || item["EstadoEnvio"] == "CANCELADO") {
                        $tb.find('tbody').append(
                            '<tr data-id="' + item["IdVenta"] + '">' +
                            '<td>' + item["serie"] + '-' + item["numero"] + '</td>' +
                            '<td>' + item["Documento"]["Nombre"] + '</td>' +
                            '<td>' + item["cliente"]["Nombre"] + '</td>' +
                            '<td>' + item["moneda"]["Nombre"] + '</td>' +
                            '<td>' + item["fechaEmision"] + '</td>' +
                            '<td>' + formatNumber(item["cantidad"]) + '</td>' +
                            '<td>' + formatNumber(item["grabada"]) + '</td>' +
                            //'<td>' + formatNumber(item["inafecta"]) + '</td>' +
                            //'<td>' + formatNumber(item["exonerada"]) + '</td>' +
                            '<td>' + formatNumber(item["igv"]) + '</td>' +
                            '<td>' + formatNumber(item["total"]) + '</td>' +
                            '<td>' + formatNumber(item["descuento"]) + '</td>' +
                            '<td>' + item["Envio"]["Nombre"] + '</td>' +
                            '<td>' + formatNumber(item["CostoEnvio"]) + '</td>' +
                            '<td>' + item["fechaEnvio"] + '</td>' +
                            '<td>' + item["EstadoEnvio"] + '</td>' +
                            '<td class="text-center">' + (item["EstadoEnvio"] == "ENVIADO" ? "<i class='fa fa-check'></i>" : "<i class='fa fa-times'></i>" ) +'</td>' + 
                            '</tr>'
                        );
                    } else {
                        $tb.find('tbody').append(
                            '<tr data-id="' + item["IdVenta"] + '">' +
                            '<td>' + item["serie"] + '-' + item["numero"] + '</td>' +
                            '<td>' + item["Documento"]["Nombre"] + '</td>' +
                            '<td>' + item["cliente"]["Nombre"] + '</td>' +
                            '<td>' + item["moneda"]["Nombre"] + '</td>' +
                            '<td>' + item["fechaEmision"] + '</td>' +
                            '<td>' + formatNumber(item["cantidad"]) + '</td>' +
                            '<td>' + formatNumber(item["grabada"]) + '</td>' +
                            //'<td>' + formatNumber(item["inafecta"]) + '</td>' +
                            //'<td>' + formatNumber(item["exonerada"]) + '</td>' +
                            '<td>' + formatNumber(item["igv"]) + '</td>' +
                            '<td>' + formatNumber(item["total"]) + '</td>' +
                            '<td>' + formatNumber(item["descuento"]) + '</td>' +
                            '<td>' + item["Envio"]["Nombre"] + '</td>' +
                            '<td>' + formatNumber(item["CostoEnvio"]) + '</td>' +
                            '<td>' + item["fechaEnvio"] + '</td>' +
                            '<td>' + item["EstadoEnvio"] + '</td>' +
                            '<td class="text-center"  width="8%">' +
                            '<button class="btn-crud btn btn-success btn-sm Enviado" data-toggle="tooltip" data-placement="top" title="Enviado"><i class="fa fa-thumbs-up"></i> </button>' +
                            "&nbsp; " +
                            '<button class="btn-crud btn btn-danger btn-sm Cancelado" data-toggle="tooltip" data-placement="top" title="Cancelado"><i class="fa fa-thumbs-down"></i> </button>' +
                            '</td>' +
                            '</tr>'
                        );
                    }
                });

                $("#TotalReg").val(response[0]["TotalR"]);
                $('#pHelperProductos').html('Existe(n) ' + response[0]["TotalR"] + ' resultado(s) para mostrar.');
                $("#lblNumPagina").html(DesPagina);
            }
        }
    });
}
function fileDownnload(url) {
    var options = {
        height: "600px",
        page: '2'
    };
    PDFObject.embed(url, "#PDFViewer", options);
    // $("#myReportPrint").modal('show');

}
