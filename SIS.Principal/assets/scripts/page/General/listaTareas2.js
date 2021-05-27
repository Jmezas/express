
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
        $("#hdf_Pagina").val('1');
        ListaGeneral();
    })
    $("#txtEstado").keyup(function () {

        $("#txtEstado").val();
        $("#hdf_Pagina").val('1');
        ListaGeneral()
    });
    $("#txtetiqueta").keyup(function () {

        $("#txtetiqueta").val();
        $("#hdf_Pagina").val('1');
        ListaGeneral()
    });
    $("#txtTarea").keyup(function () {

        $("#txtTarea").val();
        $("#hdf_Pagina").val('1');
        ListaGeneral()
    });
    $("#txtlabel").keyup(function () {

        $("#txtlabel").val();
        $("#hdf_Pagina").val('1');
        ListaGeneral()
    });
    $("#txtId").keyup(function () {

        $("#txtId").val();
        $("#hdf_Pagina").val('1');
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
        var stado = $("#txtEstado").val(),
            etiqueta = $("#txtetiqueta").val(),
            tarea = $("#txtTarea").val(),
            label = $("#txtlabel").val(),
            id = $("#txtId").val(),
            FechaInicio = inicio,
            FechaFin = Fin,
            numPaginas = parseInt($("#hdf_Pagina").val()),
            AllReg = $("#IdTotal").is(':checked') === true ? 0 : 1;
        CantiFill = $("#TotalReg").val();

        window.location.href = General.Utils.ContextPath('General/ReportePDF?stado=' + stado + "&id=" + id +   "&etiqueta=" + etiqueta + "&tarea=" + tarea + "&label=" + label +
            "&FechaIncio=" + FechaInicio + "&FechaFin=" + FechaFin +
            "&numPag=" + numPaginas + "&allReg=" + AllReg + "&Cant=" + CantiFill);
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
        var stado = $("#txtEstado").val(),
            etiqueta = $("#txtetiqueta").val(),
            tarea = $("#txtTarea").val(),
            label = $("#txtlabel").val(),
            id = $("#txtId").val(),
            FechaInicio = inicio,
            FechaFin = Fin,
            numPaginas = parseInt($("#hdf_Pagina").val()),
            AllReg = $("#IdTotal").is(':checked') === true ? 0 : 1;
        CantiFill = $("#TotalReg").val();

        window.location.href = General.Utils.ContextPath('General/ReporteExcel?stado=' + stado + "&id=" + id + "&etiqueta=" + etiqueta + "&tarea=" + tarea + "&label=" + label +
            "&FechaIncio=" + FechaInicio + "&FechaFin=" + FechaFin +
            "&numPag=" + numPaginas + "&allReg=" + AllReg + "&Cant=" + CantiFill);
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
    var stado = $("#txtEstado").val(),
        etiqueta = $("#txtetiqueta").val(),
        tarea = $("#txtTarea").val(),
        label = $("#txtlabel").val(),
        id = $("#txtId").val(),
        FechaInicio = inicio,
        FechaFin = Fin,
        numPaginas = parseInt($("#hdf_Pagina").val()),
        AllReg = $("#IdTotal").is(':checked') === true ? 0 : 1;
    let DesPagina;
    $.ajax({
        async: true,
        type: 'post',
        url: General.Utils.ContextPath('General/ListaTareasdos'),
        dataType: 'json',
        data: { stado: stado, id: id,etiqueta: etiqueta, tarea: tarea, label: label, FechaIncio: FechaInicio, FechaFin: FechaFin, numPag: numPaginas, allReg: AllReg, Cant: 10 },
        success: function (response) {

            var $tb = $("#tbVenta");
            $tb.find('tbody').empty();
            if (response["Total"] == 0) {
                return false;
            }
            if (response.length == 0) {
                $tb.find('tbody').html('<tr><td colspan="20">No hay resultados para el filtro ingresado</td></tr>');
            } else {
                $("#hdf_TotalPagina").val(response[0].totalPagina);
                DesPagina = $("#hdf_Pagina").val() + "  de  " + response[0].totalPagina;
                $.grep(response, function (item) {

                    $tb.find('tbody').append(
                        '<tr data-id="' + item["intcheck"] + '">' +
                        '<td>' + item["item"] + '</td>' +
                        '<td>' + item["empleado"] + '</td>' +
                        '<td>' + item["stado"] + '</td>' + 
                        '<td>' + item["etiqueta"] + '</td>' +
                     
                        '<td>' + item["tarea"] + '</td>' +
                       
                        '<td>' + item["descripcion"] + '</td>' +
                        '<td>' + (item["id"]) + '</td>' +
                        '<td>' + (item["direcion"]) + '</td>' +
                        '<td>' + (item["inicio"]) + '</td>' +
                        '<td>' + (item["fin"]) + '</td>' +
                        '<td>' + (item["llegada"]) + '</td>' +
                        '<td>' + (item["duracion"]) + '</td>' +
                        '<td>' + '<a href="' + item["imgen"] + '" > ' + '<img src="' + item["imgen"] + '" width="100" height="100">' + '<a/>' + '</td>' +
                        '</tr>'
                    );
                });
            }
            $("#TotalReg").val(response[0]["total"]);
            $('#pHelperProductos').html('Existe(n) ' + response[0]["total"] + ' resultado(s) para mostrar.');
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
