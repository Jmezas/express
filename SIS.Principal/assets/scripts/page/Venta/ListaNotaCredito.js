
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
    })
    $("#txtBuscar").keyup(function () {

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
            "&numPag=" + numPaginas + "&allReg=" + AllReg + "&Cant=" + CantiFill + "&venta=" + 1);
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

        window.location.href = General.Utils.ContextPath('Venta/ReporteExcel?cliente=' + Filtro + "&FechaIncio=" + FechaInicio + "&FechaFin=" + FechaFin +
            "&numPag=" + numPaginas + "&allReg=" + AllReg + "&Cant=" + CantiFill + "&venta=" + 1);
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
            ListaOC();
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

            ListaOC();
        }
    });

    $("#tbVenta").on('click', 'tbody .evento', function () {
        var data = $(this).closest('tr').attr('data-id');
        //imprimirFacBol
        //ImprimirFacturaBolTikect
        var URL = General.Utils.ContextPath('Venta/ImprimirNotaCreditoDebito?Codigo=' + data + "&Envio=" + 1);
        // console.log(URL); 
        fileDownnload(URL);
    })



})
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
        url: General.Utils.ContextPath('venta/ListaNotaCreditoDebito'),
        dataType: 'json',
        data: { filtro: Filtro, FechaIncio: FechaInicio, FechaFin: FechaFin, numPag: numPaginas, allReg: AllReg, Cant: 10 },
        success: function (response) {
            console.log(response)
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

                    $tb.find('tbody').append(
                        '<tr data-id="' + item["IdNota"] + "|" + item["TipoVenta"] + '">' +
                        '<td>' + item["Serie"] + '-' + item["Numero"] + '</td>' +
                        '<td>' + item["TipoDocumento"]["Nombre"] + '</td>' +
                        '<td>' + item["cliente"]["Nombre"] + '</td>' +
                        '<td>' + item["Moneda"]["Nombre"] + '</td>' +
                        '<td>' + item["sFechaEmision"] + '</td>' +
                        '<td>' + formatNumber(item["cantidad"]) + '</td>' +
                        '<td>' + formatNumber(item["grabada"]) + '</td>' +
                        '<td>' + formatNumber(item["inafecta"]) + '</td>' +
                        '<td>' + formatNumber(item["exonerada"]) + '</td>' +
                        '<td>' + formatNumber(item["igv"]) + '</td>' +
                        '<td>' + formatNumber(item["totalVenta"]) + '</td>' +
                        '<td>' + formatNumber(item["descuento"]) + '</td>' +
                        '<td class="text-center">' +
                        '<button class="btn-crud btn btn-info  btn-sm evento"  data-toggle="modal" data-target="#Report"  title="Ver Detalle"><i class="fa fa-search"></i> </button>' +
                        '</td>' +
                        '</tr>'
                    );
                });
            }
            $("#TotalReg").val(response[0]["TotalReg"]);
            $('#pHelperProductos').html('Existe(n) ' + response[0]["TotalReg"] + ' resultado(s) para mostrar.');
            $("#lblNumPagina").html(DesPagina);
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
