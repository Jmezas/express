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
                changeDoc($("#lstDocumento").val())

            }
        });
    },
    CargarMoneda: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaCombo"),
            dataType: 'json',
            data: { Id: 10 },
            success: function (response) {
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 

                    $.grep(response, function (oDocumento) {

                        $('select[name="lstMoneda"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
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
    Lista.CargarMoneda();

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


    $('#tbDetalle').find('tbody').on('click', '.btn-warning', function () {

        var tipo_notacred = $('input[name=rbTipoNotaCred]:checked').val();

        if (tipo_notacred == '5' || tipo_notacred == '9') {
            $("#txtValorUnit").prop("disabled", false);
        } else if (tipo_notacred == '7') {
            $("#txtCantidad").prop("disabled", false);
        } else if (tipo_notacred == '3') {
            $("#txtProducto").prop("disabled", false);
        } else {
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese un tipo de Nota de Crédito que afecte al detalle');
            return false;
        }
        if ($('#IdProducto').val() > 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'Ya se encuentra modificando un Detalle');
        } else {
            var $btn = $(this);
            var $tb = $('#tbDetalle');
            var iIndex = $btn.closest('tr').attr('data-index');

            arrModificar = Lista.Vars.Detalle.filter(function (x) {
                return x.index == iIndex;
            });


            $('#IdDetalle').val(arrModificar[0].index);
            $('#IdProducto').val(arrModificar[0].material.Id);
            $('#txtProducto').val(arrModificar[0].material.Nombre);
            $('#UMProducto').val(arrModificar[0].material.Unidad.Nombre);

            $('#CantidadP').val(arrModificar[0].cantidad);
            $('#txtCantidad').val(arrModificar[0].cantidad);

            $('#ValorUnitP').val(arrModificar[0].precio);
            $('#txtValorUnit').val(arrModificar[0].precio);

            $('#DsctoUnitP').val(arrModificar[0].descuento);
            $('#DsctoUnitNC').val(arrModificar[0].descuento);

            $('#ValorVtaP').val(arrModificar[0].Importe);
            $('#ValorVtaNC').val(arrModificar[0].Importe);

            arrDetalle = Lista.Vars.Detalle.filter(function (x) {
                return x.index != iIndex;
            });

            Lista.Vars.Detalle = [];
            Lista.Vars.Detalle = arrDetalle;

            $tb.find('tbody').empty();

            if (Lista.Vars.Detalle.length == 0) {
                $tb.find('tbody').append('<tr><td colspan="13">No existen registros</td></tr>')
            } else {
                $.grep(Lista.Vars.Detalle, function (oDetalle) {
                    $tb.find('tbody').append(
                        '<tr data-index="' + oDetalle["index"] + '">' +
                        '<td class="text-center">' +
                        '<button class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>&nbsp;' +
                        '<button class="btn btn-warning btn-sm"><i class="fa fa-edit"></i></button>' +
                        '</td>' +
                        '<td>' + formatNumber(oDetalle["cantidad"]) + '</td>' +
                        '<td>' + oDetalle["material"]["Unidad"]["Nombre"] + '</td>' +
                        '<td>' + oDetalle["material"]["Nombre"] + '</td>' +
                        '<td>' + formatNumber(oDetalle["precio"]) + '</td>' +
                        '<td>' + formatNumber(oDetalle["descuento"]) + '</td>' +
                        '<td>' + formatNumber(oDetalle["Importe"]) + '</td>' +
                        '</tr>'
                    );
                })
            }
            CalcularTotales();
        }
    });

    $('#tbDetalle').find('tbody').on('click', '.btn-danger', function () {

        var tipo_notacred = $('input[name=rbTipoNotaCred]:checked').val();

        if (tipo_notacred == '5' || tipo_notacred == '9' || tipo_notacred == '7') {
            var $btn = $(this);
            var $tb = $('#tbDetalle');
            General.Utils.ShowConfirm(
                250,
                '&#191;Seguro(a) que desea eliminar el Detalle?',
                '',
                function () {
                    var iIndex = $btn.closest('tr').attr('data-index');
                    BuscarIndexDetalleEnTabla(iIndex);
                    arrDetalle = Lista.Vars.Detalle.filter(function (x) {
                        return x.index != iIndex;
                    });

                    Lista.Vars.Detalle = [];
                    Lista.Vars.Detalle = arrDetalle;

                    $tb.find('tbody').empty();

                    if (Lista.Vars.Detalle.length == 0) {
                        $tb.find('tbody').append('<tr><td colspan="13">No existen registros</td></tr>')
                    } else {
                        $.grep(Lista.Vars.Detalle, function (oDetalle) {
                            $tb.find('tbody').append(
                                '<tr data-index="' + oDetalle["index"] + '">' +
                                '<td class="text-center">' +
                                '<button class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>&nbsp;' +
                                '<button class="btn btn-warning btn-sm"><i class="fa fa-edit"></i></button>' +
                                '</td>' +
                                '<td>' + formatNumber(oDetalle["cantidad"]) + '</td>' +
                                '<td>' + oDetalle["material"]["Unidad"]["Nombre"] + '</td>' +
                                '<td>' + oDetalle["material"]["Nombre"] + '</td>' +
                                '<td>' + formatNumber(oDetalle["precio"]) + '</td>' +
                                '<td>' + formatNumber(oDetalle["descuento"]) + '</td>' +
                                '<td>' + formatNumber(oDetalle["Importe"]) + '</td>' +
                                '</tr>'
                            );
                        })
                    }
                    CalcularTotales();
                },
                function () { }
            );
        } else {
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese un tipo de Nota de Crédito que afecte al detalle');
        }

    });

    $('#btnAgregar').click(function () {

        var tipo_notacred = $('input[name=rbTipoNotaCred]:checked').val();

        if ($('#txtObservaciones').val().length <= 0) {
            $('#txtObservaciones').focus();
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese el Motivo o Sustento');
        } else {
            if (tipo_notacred == 1 || tipo_notacred == 2 || tipo_notacred == 6) {
                ObtenerComprobante($('#IdVenta').val());
            } else if (tipo_notacred == 4 || tipo_notacred == 8) {
                AgregarDsctoBon();
            } else if (tipo_notacred == 3 || tipo_notacred == 5 || tipo_notacred == 7 || tipo_notacred == 9) {
                AgregarDevDsctoDism();
            }
        }
    });

    function AgregarDevDsctoDism() {
        var tipo_notacred = $('input[name=rbTipoNotaCred]:checked').val();

        var iIdDetalle = $('#IdDetalle').val();
        var iIdProducto = $('#IdProducto').val();
        var vUMProducto = $('#UMProducto').val();
        var vNomProducto;
        if (tipo_notacred == 3) {
            vNomProducto = 'Dice: ' + $('#hdProducto').val() + '. Debe decir: ' + $('#txtProducto').val();
        } else {
            vNomProducto = $('#txtProducto').val();
        }


        var iCantidadP = $('#CantidadP').val()
        var iCantidad = $('#txtCantidad').val();

        var iDsctoUnitP = $('#DsctoUnitP').val();
        var iDsctoUnitNC = $('#DsctoUnitNC').val();

        var iPreVtaP = $('#PreVtaP').val();
        var iPreVtaNC = $('#PreVtaNC').val();

        var iValorVtaP = $('#ValorVtaP').val();
        var iValorVtaNC = $('#ValorVtaNC').val();

        var iValorUnitP = $('#ValorUnitP').val();
        var iValorUnit = $('#txtValorUnit').val();

        iCantidad = iCantidad * 1;
        iDsctoUnitNC = iDsctoUnitNC * 1;
        iValorUnit = iValorUnit * 1;

        iCantidadP = iCantidadP * 1;
        iDsctoUnitP = iDsctoUnitP * 1;
        iValorUnitP = iValorUnitP * 1;
        iValorVtaP = iValorVtaP * 1;
        iPreVtaP = iPreVtaP * 1;

        iPreVtaNC = iValorUnit * 1.18;
        iValorVtaNC = iValorUnit * iCantidad;

        if (isNull(iIdProducto) || iIdProducto == 0) {
            $('#txtProducto').focus();
            General.Utils.ShowMessage(TypeMessage.Error, 'Debe ingresar un producto');
        } else {
            if (BuscarDetalleEnTabla(iIdProducto)) {
                General.Utils.ShowMessage(TypeMessage.Error, 'El producto ya existe en la tabla');
            } else {

                var $tb = $('#tbDetalle');
                $tb.find('tbody').empty();

                Lista.Vars.Detalle.push({
                    index: iIdDetalle,
                    precio: iValorUnit,
                    cantidad: iCantidadP,
                    descuento: iDsctoUnitP,
                    descuentoPor: 0,
                    Importe: iValorVtaNC,
                    material: {
                        Nombre: vNomProducto,
                        Id: iIdProducto,
                        Unidad: {
                            Nombre: vUMProducto
                        }
                    },
                });

                $tb.find('tbody').empty();

                if (Lista.Vars.Detalle.length == 0) {
                    $tb.find('tbody').append('<tr><td colspan="13">No existen registros</td></tr>')
                } else {
                    $.grep(Lista.Vars.Detalle, function (oDetalle) {
                        $tb.find('tbody').append(
                            '<tr data-index="' + oDetalle["index"] + '">' +
                            '<td class="text-center">' +
                            '<button class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>&nbsp;' +
                            '<button class="btn btn-warning btn-sm"><i class="fa fa-edit"></i></button>' +
                            '</td>' +
                            '<td>' + formatNumber(oDetalle["cantidad"]) + '</td>' +
                            '<td>' + oDetalle["material"]["Unidad"]["Nombre"] + '</td>' +
                            '<td>' + oDetalle["material"]["Nombre"] + '</td>' +
                            '<td>' + formatNumber(oDetalle["precio"]) + '</td>' +
                            '<td>' + formatNumber(oDetalle["descuento"]) + '</td>' +
                            '<td>' + formatNumber(oDetalle["Importe"]) + '</td>' +
                            '</tr>'
                        );
                    })
                }

                $('#IdDetalle').val('');
                $('#IdProducto').val('');
                $('#UMProducto').val('');
                $('#txtProducto').val('');

                $('#CantidadP').val('');
                $('#txtCantidad').val('');

                $('#DsctoUnitP').val('');
                $('#DsctoUnitNC').val('');

                $('#PreVtaP').val('');
                $('#PreVtaNC').val('');

                $('#ValorVtaP').val('');
                $('#ValorVtaNC').val('');

                $('#ValorUnitP').val('');
                $('#txtValorUnit').val('');

                CalcularTotales();

            }
        }
    }

    function AgregarDsctoBon() {

        var nDsctoNBon = $('#txtDsctoTotal').val() * 1;

        var iIdDetalle = 1;
        var iIdProducto = 0;
        var vUMProducto = '';

        vUMProducto = 'NIU';

        var vNomProducto = $('#txtObservaciones').val();

        var iCantidadP = 1
        var iCantidad = 1;

        var iDsctoUnitP = 0.00;
        var iDsctoUnitNC = 0.00;

        var iPreVtaP = nDsctoNBon * 1;
        var iPreVtaNC = iPreVtaP;

        var iValorVtaP = nDsctoNBon / 1.18;
        var iValorVtaNC = iValorVtaP;

        var iValorUnitP = iValorVtaP;
        var iValorUnit = iValorVtaP;

        var $tb = $('#tbDetalle');
        $tb.find('tbody').empty();
        Lista.Vars.Detalle = [];

        Lista.Vars.Detalle.push({
            index: iIdDetalle,
            precio: iValorUnit,
            cantidad: iCantidadP,
            descuento: iDsctoUnitP,
            descuentoPor: 0,
            Importe: iValorVtaNC,
            material: {
                Nombre: vNomProducto,
                Id: iIdProducto,
                Unidad: {
                    Nombre: vUMProducto
                }
            },
        });

        $tb.find('tbody').empty();

        if (Lista.Vars.Detalle.length == 0) {
            $tb.find('tbody').append('<tr><td colspan="13">No existen registros</td></tr>')
        } else {
            $.grep(Lista.Vars.Detalle, function (oDetalle) {

                $tb.find('tbody').append(
                    '<tr data-index="' + oDetalle["index"] + '">' +
                    '<td class="text-center">' +
                    '<button class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>&nbsp;' +
                    '<button class="btn btn-warning btn-sm"><i class="fa fa-edit"></i></button>' +
                    '</td>' +
                    '<td>' + formatNumber(oDetalle["cantidad"]) + '</td>' +
                    '<td>' + oDetalle["material"]["Unidad"]["Nombre"] + '</td>' +
                    '<td>' + oDetalle["material"]["Nombre"] + '</td>' +
                    '<td>' + formatNumber(oDetalle["precio"]) + '</td>' +
                    '<td>' + formatNumber(oDetalle["descuento"]) + '</td>' +
                    '<td>' + formatNumber(oDetalle["Importe"]) + '</td>' +
                    '</tr>'
                );
            })
        }
        $('#IdDetalle').val('');
        $('#IdProducto').val('');
        $('#UMProducto').val('');
        $('#txtProducto').val('');

        $('#CantidadP').val('');
        $('#txtCantidad').val('');

        $('#DsctoUnitP').val('');
        $('#DsctoUnitNC').val('');

        $('#PreVtaP').val('');
        $('#PreVtaNC').val('');

        $('#ValorVtaP').val('');
        $('#ValorVtaNC').val('');

        $('#ValorUnitP').val('');
        $('#txtValorUnit').val('');


        $('#hTotalValorVta').html(iValorVtaP.toFixed(2));
        $("#subtotales").val(iValorVtaP)
        $('#hSumDscto').html("0.00");
        $('#hImpTotalVta').html(nDsctoNBon.toFixed(2));
        $("#Totales").val(nDsctoNBon)
        var Sumigv = nDsctoNBon - iValorVtaP;
        $('#hSumIgv').html(Sumigv.toFixed(2));
        $('#igv').html(Sumigv);


    }

    $("#btnGuardar").click(function () {

        if ($("#IdCliente").val() == null || $("#IdCliente").val() == undefined || $("#IdCliente").val() == "") {
            General.Utils.ShowMessage(TypeMessage.Error, 'seleccione Comporbante');
        } else if ($('#chekAlmacen').attr('checked') == 1) {
            if ($("#lstAlmacen").val() == 0) {
                General.Utils.ShowMessage(TypeMessage.Error, 'seleccione almácen');
            }
        } else if ($('#txtObservaciones').val().length <= 0) {
            $('#txtObservaciones').focus();
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese el Motivo o Sustento');
        } else {

            oDatos = {
                IdNota: 0,
                TipoDocumento: {
                    IdDocumento: $("#lstDocumento").val(),
                },
                Moneda: {
                    IdMoneda: $("#lstMoneda").val()
                },
                FechaEmision: $("#txtFecOp").val(),
                cliente: {
                    Id: $("#IdCliente").val()
                },
                grabada: $("#subtotales").val(),
                inafecta: $("#inafecta").val(),
                exonerada: $("#exonerada").val(),
                descuento: $("#descuento").val(),
                igv: $("#igv").val(),
                totalVenta: $("#Totales").val(),
                cantidad: $("#hCantidad").text(),
                Almacen: {
                    IdAlmacen: $("#lstAlmacen").val(),
                },
                AfectaStock: $("#chekAlmacen").is(':checked') === true ? 1 : 0,
                NotaCreditoDebito: {
                    Credito: $('input[name=rbTipoNotaCred]:checked').val()
                },
                Serie: $("#txtSerie").val(),
                Numero: $("#txtNumero").val(),
                Motivo: $("#txtObservaciones").val(),
                Observacion: $("#txtObservacion").val(),
                Venta: {
                    Id: $("#IdVenta").val()
                },
                TipoVenta: $('input[name=postVenta]:checked').val()
            }

            $.ajax({
                async: false,
                type: 'post',
                url: General.Utils.ContextPath('Venta/InstNotaCreditoDebito'),
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
                                document.getElementById('reporte').click();
                                var tipoVenta = $('input[name=postVenta]:checked').val()
                                var URL = General.Utils.ContextPath('Venta/ImprimirNotaCreditoDebito?Codigo=' + response.Additionals[0] + "|" + tipoVenta + "&Envio=" + 0);
                                fileDownnload(URL);
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
function fileDownnload(url) {
    var options = {
        height: "600px",
        page: '2'

    };
    PDFObject.embed(url, "#PDFViewer", options);


}
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
                if ($("#lstDocumento").val() == 5) {
                    $("#lstDocumento").val(5)
                    changeDoc(5)
                } else {
                    $("#lstDocumento").val(7)
                    changeDoc(7)
                }



            } else {
                $("#lstDocumento").val(6)
                if ($("#lstDocumento").val() == 6) {
                    $("#lstDocumento").val(6)
                    changeDoc(6)
                } else {
                    $("#lstDocumento").val(8)
                    changeDoc(8)
                }
                //$("#lstDocumento").val(6)
                //$("#lstDocumento").val(8)
                //changeDoc(8)
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

            CalcularTotales();
            if (Lista.Vars.Detalle.length == 0) {
                $td.find('tbody').append('<tr><td colspan="16">No existen registros</td></tr>')
            } else {
                $.grep(Lista.Vars.Detalle, function (oDetalle) {
                    $td.find('tbody').append(
                        '<tr data-index="' + oDetalle["index"] + '">' +
                        '<td class="text-center">' +
                        '<button class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>&nbsp;' +
                        '<button class="btn btn-warning btn-sm"><i class="fa fa-edit"></i></button>' +
                        '</td>' +
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
function CalcularTotales() {
    var TotalValorVta = ImpTotalVta = Igv = TotalSub = Cantidad = exonerada = inafecta = grabada = gratuita = 0;
    var totalfac = 0;
    var desc = 0;
    $.grep(Lista.Vars.Detalle, function (oDetalle) {

        TotalValorVta = oDetalle["Importe"];  //TOTAL
        TotalValorVta = TotalValorVta * 1
        grabada += TotalValorVta

        var SubTotalCal = TotalValorVta / (18 / 100 + 1)//Subtotal
        SubTotalCal = SubTotalCal * 1;
        ImpTotalVta += SubTotalCal

        totalfac = grabada + exonerada + inafecta;
        Cantidad += parseInt(oDetalle["cantidad"]);
        desc += parseInt(oDetalle["descuento"] * parseInt(oDetalle["cantidad"]));
    })
    IgvSuma = grabada - ImpTotalVta;

    $('#hCantidad').html(formatNumber(Cantidad));
    $('#hTotalValorVta').html(formatNumber(ImpTotalVta));//grabada
    $('#hTotalInafectas').html(formatNumber(exonerada));
    $('#hTotalExoneradas').html(formatNumber(inafecta));
    $('#hdfDescuento').html(formatNumber(desc));
    $('#hSumIgv').html(formatNumber(IgvSuma));
    $('#hImpTotalVta').html(formatNumber(totalfac));//total

    //Totales la base de datos
    $("#subtotales").val(ImpTotalVta);
    $("#exonerada").val(exonerada);
    $("#inafecta").val(inafecta);
    $("#gratuita").val(0.00);
    $("#descuento").val(desc);
    $("#igv").val(IgvSuma);
    $("#Totales").val(totalfac);

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
    Lista.CargarTipoMod();
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