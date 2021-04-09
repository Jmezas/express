var Lista = {
    CargarAlmacen: function () {

        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaComboSucursal"),
            dataType: 'json',
            success: function (response) {
                $('select[name="lstAlmacen"]').empty();
                $("#lstAlmacen").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error  
                    $.grep(response, function (oDocumento) {

                        $('select[name="lstAlmacen"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
                    });
                }
            }

        });
    },

    CargarTipoMod: function () {
        var sucursal = $("#sucursal").val();
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaComboId"),
            dataType: 'json',
            data: { flag: 10, Id: sucursal },
            success: function (response) {
                console.log(response)
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 

                    $.grep(response, function (oDocumento) {

                        $('select[name="lstDocumento"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
                    });
                }


            }
        });
    },

    Vars: {
        Detalle: []
    },
}
$(function () {
    Lista.CargarAlmacen();
    Lista.CargarTipoMod();

    $("#hdf_Pagina").val('1');
    $('#txtFecOp').datepicker({
        dateFormat: 'dd/mm/yy',
        prevText: '<i class="fa fa-angle-left"></i>',
        nextText: '<i class="fa fa-angle-right"></i>'
    });
    $("#txtFecOp").datepicker().datepicker("setDate", new Date());

    $("#chekAlmacen").change(function () {
        if ($("#chekAlmacen").prop('checked')) {

            $('#lstAlmacen').removeAttr('disabled');

        } else {
            $('#lstAlmacen').val(0)
            $('#lstAlmacen').attr('disabled', 'disabled');
        }
    })
    $("#buscarComprobante").click(function () {

        $("#hdf_Pagina").val('1')
        $('#fechaInicio').val('')
        $('#fechaFin').val('')
        $("#txtBuscar").val('')
        ListaGeneral();
    })

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

    $("#btnFiltrar").click(function () {
        ListaGeneral();
    })
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
        ObtenerComprobante(data)
    })
    ///**select  to  **//
    $("input[name=rbTipoNotaCred]:radio").change(function () {
        var tipo_notacred = $('input[name=rbTipoNotaCred]:checked').val();

        $("#txtDsctoTotal").prop("disabled", true);
        $("#txtProducto").prop("disabled", true);
        $("#txtCantidad").prop("disabled", true);
        $("#txtValorUnit").prop("disabled", true);

        if (tipo_notacred == '4' || tipo_notacred == '8') {
            $("#txtDsctoTotal").prop("disabled", false);
        }
    });



    $("#btnAnual").click(function () {

        if ($("#IdCliente").val() == null || $("#IdCliente").val() == undefined || $("#IdCliente").val() == "") {
            General.Utils.ShowMessage(TypeMessage.Error, 'seleccione Comporbante');
        } else if ($("#lstAlmacen").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'seleccione almácen');

        } else if ($('#txtObservacion').val().length <= 0) {
            $('#txtObservacion').focus();
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese el Motivo o Sustento');
        } else {

            oDatos = {
                FechaEmision: $("#txtFecOp").val(),

                Almacen: {
                    IdAlmacen: $("#lstAlmacen").val(),
                },
                motivo: $("#txtObservacion").val(),

                IdVenta: $("#IdVenta").val(),
                id: $('input[name=postVenta]:checked').val(),
                EstadoEnvio:'B'
            }
            $.ajax({
                async: false,
                type: 'post',
                url: General.Utils.ContextPath('Venta/anularVenta'),
                dataType: 'json',
                data: { oDatos: oDatos, Detalle: Lista.Vars.Detalle },
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                success: function (response) {
                    console.log(response)
                    if (response.Id != "error") { // Si la petición no emitió error

                        Swal.fire({
                            title: 'title',
                            html: response.Message,
                            icon: response.Id,
                            showCancelButton: true,

                            confirmButtonText: 'ok'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                location.reload();
                            }
                        })

                    } else {
                        Swal.fire(
                            'Error ',
                            response.Message,
                            response.Id
                        )
                    }
                }
            });

        }
        Limpiar();

    })
    $("#btnEliminar").click(function () {

        if ($("#IdCliente").val() == null || $("#IdCliente").val() == undefined || $("#IdCliente").val() == "") {
            General.Utils.ShowMessage(TypeMessage.Error, 'seleccione Comporbante');
        } else if ($("#lstAlmacen").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'seleccione almácen');

        } else if ($('#txtObservacion').val().length <= 0) {
            $('#txtObservacion').focus();
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese el Motivo o Sustento');
        } else {

            oDatos = {
                FechaEmision: $("#txtFecOp").val(),

                Almacen: {
                    IdAlmacen: $("#lstAlmacen").val(),
                },
                motivo: $("#txtObservacion").val(),

                IdVenta: $("#IdVenta").val(),
                id: $('input[name=postVenta]:checked').val(),
                EstadoEnvio: 'I'
            }
            $.ajax({
                async: false,
                type: 'post',
                url: General.Utils.ContextPath('Venta/anularVenta'),
                dataType: 'json',
                data: { oDatos: oDatos, Detalle: Lista.Vars.Detalle },
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                success: function (response) {
                    console.log(response)
                    if (response.Id != "error") { // Si la petición no emitió error

                        Swal.fire({
                            title: 'title',
                            html: response.Message,
                            icon: response.Id,
                            showCancelButton: true,

                            confirmButtonText: 'ok'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                location.reload();
                            }
                        })

                    } else {
                        Swal.fire(
                            'Error ',
                            response.Message,
                            response.Id
                        )
                    }
                }
            });

        }
        Limpiar();

    }) 
})

function changeDoc(doc) {
    $.ajax({
        async: true,
        type: 'post',
        url: General.Utils.ContextPath("venta/SerieNumero"),
        dataType: 'json',
        data: { documento: doc },
        success: function (response) {
            $("#txtSerie").val(response.Text)
            $("#txtNumero").val(response.Nombre)
        }
    });
}
function ListaGeneral() {
    var listaVenta = ""
    if ($('input[name=postVenta]:checked').val() == 2) {

        listaVenta = "ListaVenta"
    } else {
        listaVenta = "ListaVentaPOST"
    }
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
        url: General.Utils.ContextPath(`venta/${listaVenta}`),
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

                    $tb.find('tbody').append(
                        '<tr data-id="' + item["IdVenta"] + '">' +
                        '<td>' + item["serie"] + '-' + item["numero"] + '</td>' +
                        '<td>' + item["Documento"]["Nombre"] + '</td>' +
                        '<td>' + item["cliente"]["Nombre"] + '</td>' +
                        '<td>' + item["fechaEmision"] + '</td>' +

                        '<td>' + formatNumber(item["grabada"]) + '</td>' +
                        '<td>' + formatNumber(item["inafecta"]) + '</td>' +
                        '<td>' + formatNumber(item["exonerada"]) + '</td>' +
                        '<td>' + formatNumber(item["igv"]) + '</td>' +
                        '<td>' + formatNumber(item["total"]) + '</td>' +
                        '<td class="text-center">' +
                        '<button class="btn-crud btn btn-info  btn-sm evento"  title="Ver Detalle"><i class="fa fa-check"></i> </button>' +
                        '</td>' +
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

function ObtenerComprobante(Id) {
    Lista.Vars.Detalle = [];

    var ObtenerCom = "";
    if ($('input[name=postVenta]:checked').val() == 2) {

        ObtenerCom = "ObtenerComprobanteVenta"
    } else {
        ObtenerCom = "ObtenerComprobante"
    }

    $.ajax({
        async: true,
        type: 'post',
        url: General.Utils.ContextPath(`venta/${ObtenerCom}`),
        dataType: 'json',
        data: { Id: Id },
        success: function (response) {

            $("#IdCliente").val(response[0].Venta.cliente.Id)
            $("#IdVenta").val(response[0].Venta.Id)
            $("#lstTipoDocCli").val(response[0].Comprobante.Nombre)
            $("#IdComprobante").val(response[0].Venta.Comprobante.Codigo)

            if ($("#IdComprobante").val() == '01') {
                $("#lstDocumento").val(5)
                changeDoc(5)
            } else {
                $("#lstDocumento").val(6)
                changeDoc(6)
            }

            $("#txtNomCli").val(response[0].Venta.cliente.Nombre)
            $("#txtDocCli").val(response[0].Venta.cliente.NroDocumento)
            $("#txtDireccion").val(response[0].Venta.cliente.Direccion)
            $("#txtNumDocMod").val(response[0].Venta.serie + "-" + response[0].Venta.numero)
            $("#txtFecEmision").val(response[0].Venta.fechaEmision)
            var $td = $("#tbDetalle");
            $td.find('tbody').empty();

            $.grep(response, function (oDetalle) {
                Lista.Vars.Detalle.push({
                    index: oDetalle["IdVentaDetalle"],
                    precio: oDetalle["precio"],
                    cantidad: oDetalle["cantidad"],
                    descuento: oDetalle["descuento"],
                    descuentoPor: oDetalle["descuentoPor"],
                    Importe: oDetalle["Importe"],
                    material: {
                        Nombre: oDetalle["material"]["Nombre"],
                        Id: oDetalle["material"]["IdMaterial"],
                        Unidad: {
                            Nombre: oDetalle["material"]["Unidad"]["Nombre"]
                        }
                    },
                })
            })

           
            if (Lista.Vars.Detalle.length == 0) {
                $td.find('tbody').append('<tr><td colspan="16">No existen registros</td></tr>')
            } else {
                $.grep(Lista.Vars.Detalle, function (oDetalle) {
                    $td.find('tbody').append(
                        '<tr data-index="' + oDetalle["index"] + '">' +
                        '<td>' + formatNumber(oDetalle["cantidad"]) + '</td>' +
                        '<td>' + oDetalle["material"]["Unidad"]["Nombre"] + '</td>' +
                        '<td>' + oDetalle["material"]["Nombre"] + '</td>' +
                        '<td>' + formatNumber(oDetalle["precio"]) + '</td>' +
                        '<td>' + formatNumber(oDetalle["descuento"]) + '</td>' +
                        '<td>' + formatNumber(oDetalle["Importe"]) + '</td>' +
                        '</tr>'
                    )
                })
            }
        }
    });
    $("[data-dismiss=modal]").trigger({ type: "click" });//cerrar modal.
}
 

function BuscarDetalleEnTabla(iIdProducto) {
    var bFound = false;
    $.each(Lista.Vars.Detalle, function (index, item) {
        if (item["material"]["Id"] == iIdProducto) {
            bFound = true;
            return false;
        }
    });
    return bFound;
}

function BuscarIndexDetalleEnTabla(id) {
    for (var i = 0; i < Lista.Vars.Detalle.length; i += 1) {
        if (Lista.Vars.Detalle[i]["index"] == id) {
            return i;
        }
    }
    return -1;
}
function Limpiar() {
    $("#txtNumDocMod").val('');
    $("#txtFecEmision").val('');
    $("#lstTipoDocCli").val('');
    $("#txtDocCli").val('');
    $("#txtNomCli").val('');
    $("#txtDireccion").val('');
    $("#IdCliente").val('');
    $("#IdVenta").val('');
    $("#txtObservaciones").val('');

    $("#subtotales").val('');
    $("#inafecta").val('');
    $("#exonerada").val('');
    $("#gratuita").val('');
    $("#descuento").val('');
    $("#igv").val('');
    $("#Totales").val('');

    $("#hTotalValorVta").html('0.00');
    $("#hTotalInafectas").html('0.00');
    $("#hTotalExoneradas").html('0.00');
    $("#hdfDescuento").html('0.00');
    $("#hSumIgv").html('0.00');
    $("#hImpTotalVta").html('0.00');


    $("#txtFecOp").datepicker().datepicker("setDate", new Date());
    Lista.Vars.Detalle = [];
    var $tb = $('#tbDetalle');
    $tb.find('tbody').empty();
    if (Lista.Vars.Detalle.length == 0) {
        $tb.find('tbody').append('<tr><td colspan="11">No existen registros</td></tr>')
    }
    $('input[name=rbTipoNotaCred][value=1]').prop('checked', 'checked');
    $('input[name=postVenta][value=1]').prop('checked', 'checked');
    $('input[name=chekAlmacen]').prop('checked', 'checked');
}