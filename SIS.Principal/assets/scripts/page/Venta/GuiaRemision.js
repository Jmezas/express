var Lista = {
    CargarTipoDoc: function () {
        var sucursal = $("#sucursal").val();
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaComboId"),
            dataType: 'json',
            data: { flag: 13, Id: sucursal },
            success: function (response) {
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error  
                    $.grep(response, function (oDocumento) { 
                        $('select[name="lstDocumento"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
                    });
                }
                changeDoc($("#lstDocumento").val())
            }

        });
    },
    Vars: {
        Detalle: []
    },
}

$(function () {
    Lista.CargarTipoDoc()
    $("#hdf_Pagina").val('1');

    $('input[name="txtFechaEmision"]').datepicker({
        dateFormat: 'dd/mm/yy',
        prevText: '<i class="fa fa-angle-left"></i>',
        nextText: '<i class="fa fa-angle-right"></i>',
        minDate: new Date()
    });

    $("#lstDocumento").change(function () {
        changeDoc($("#lstDocumento").val())
    })

    $("#txtNota").val('SE ENVIA SOBRE CON FACTURA CONTIENE PRODUCTO FOLIARES');
    $("#txtPartida").val("LOS ROSALES MZ. A LT.9-ASOC. SAN PEDRO DE GRAMADAL. PTE. PIEDRA-LIMA")
    $("#txtCajas").val('0.00');
    $("#txtBidones").val('0.00');
    $("#txtSacos").val('0.00');
    $("#txtCilindro").val('0.00');
    $("#txtPeso").val('0.00');
    $("#Estado").prop('checked', true);

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
    $("#txtRucEmpresa").autocomplete({
        source: function (request, response) {
            var vRuc = $("#txtRucEmpresa").val();
            console.log(vRuc)
            $.ajax({
                url: General.Utils.ContextPath('Venta/Buscartransporte'),
                type: "POST",
                dataType: "json",
                data: { pRuc: vRuc, Filtro2: vRuc },
                success: function (data) {
                    console.log(data)
                    response(jQuery.map(data, function (item) {
                        return {
                            id: item.IdTrasnporte,
                            cod: item.Ruc,
                            label: item.Ruc + ' - ' + item.RazonSocial,
                            des: item.RazonSocial + '|' + item.Direccion

                        };
                    }));
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            $('#IdTrasnporte').val(ui.item.id);
            $("#txtRucEmpresa").val(ui.item ? ui.item.cod : jQuery("#txtRucEmpresa").val());
            $('#IdTrasnporte').val(ui.item.id);
            console.log($('#IdTrasnporte').val());

        },

        change: function (event, ui) {
            //console.log(ui.item.des);
            $("#txtRucEmpresa").val(ui.item ? ui.item.cod : jQuery("#txtRucEmpresa").val());
            $("#txtEmpresa").val(ui.item ? ui.item.des.toString().split('|')[0] : '');
            $("#txtDirecciontr").val(ui.item ? ui.item.des.toString().split('|')[1] : '');
        }
    });


    //ACtulizar cantidad
    $('#tbDetalle').on('change', '.Cant', function () {

        var row = $(this).closest('tr');
        var Id = $(this).closest('tr').attr('data-index');
        //var Cantidad = 0;
        var input = Number($(this).val());;

        Lista.Vars.Detalle.map(function (data) {
            
            if (parseInt(data.Material.IdMaterial) == parseInt(Id)) {
                data.Bulto = parseFloat(input);
            }
        })

        var $tb = $('#tbDetalle');
        $tb.find('tbody').empty();
        if (Lista.Vars.Detalle.length == 0) {
            $tb.find('tbody').append('<tr><td colspan="15">No existen registros</td></tr>')
        } else {

            $.grep(Lista.Vars.Detalle, function (oDetalle) {
                $tb.find('tbody').append(
                    '<tr data-index="' + oDetalle["Material"]["IdMaterial"] + '">' +
                    '<td>' + formatNumber(oDetalle["cantidad"]) + '</td>' +
                    '<td>' + oDetalle["Material"]["Unidad"]["Nombre"] + '</td>' +
                    '<td>' + oDetalle["Material"]["Codigo"] + '-' + oDetalle["Material"]["Nombre"] + '</td>' +
                    '<td>' + formatNumber(oDetalle["precio"]) + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Cant" id="Cantidad" value="' + formatNumber(oDetalle["Bulto"]) + '">' + '</td>' +

                    '</tr>'
                );
            });
        }
    });
    $("#limpiar").click(function () {
        LimpiarConteniedo(); 
    })
    $("#btnProcesar").click(function () {  
        if ($("#txtPuntoLlegada").val() == "") {
            $("#txtPuntoLlegada").focus();
            General.Utils.ShowMessage(TypeMessage.Error, 'Por favor ingrese direccion de llegada');
        } else if ($("#txtFechaEmision").val() == "") {
            $("#txtFechaEmision").focus(); 
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese fecha de emisión');
        } else if ($("#IdVenta").val() == 0) {
            
            General.Utils.ShowMessage(TypeMessage.Error, 'Seleccione comprobante');
        } else if (!$("input[name=rbLineaNegocio]").is(":checked")) {
            General.Utils.ShowMessage(TypeMessage.Error, 'Seleccione Motivo de traslado');
        } else {
            var oDatos = {
                serie: $("#txtSerie").val(),
                numero: $("#txtNumero").val(),

                FechaRegistro: $("#txtFechaEmision").val(),

                Venta: {
                    Id: $("#IdVenta").val(),
                },
                TipoVenta: $('input[name=postVenta]:checked').val(),
                Cliente: {
                    Id: $("#IdCliente").val()
                },
                codigTraslado: $("input[name=rbLineaNegocio]:checked").val(),
                codigoEstado: '01',
                Transporte: { IdTrasnporte: $('#IdTrasnporte').val() },
                DirecionLlegada: $("#txtPuntoLlegada").val(),
                DirecionSalida: $("#txtPartida").val(),
                Nota: $("#txtNota").val(),
                Caja: $("#txtCajas").val(),
                Bidones: $("#txtBidones").val(),
                Sacos: $("#txtSacos").val(),
                Cilindro: $("#txtCilindro").val(),
                Peso: $("#txtPeso").val() 
            }
            console.log(oDatos)
            $.ajax({
                async: true,
                type: 'post',
                url: General.Utils.ContextPath('Gestion/RegistrarGuia'),
                dataType: 'json',
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                data: { oDatos: oDatos, Detalle: Lista.Vars.Detalle },
                success: function (response) {
                    if (response.Id == 'success') { 
                        Swal.fire({
                            title: 'title',
                            html: response.Message,
                            icon: response.Id,
                            showCancelButton: true, 
                            confirmButtonText: 'ok'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                LimpiarConteniedo();
                                document.getElementById('reporte').click(); 
                                var URL = General.Utils.ContextPath('Reportes/ImprimirGuia?Codigo=' + response.Additionals[0]);
                                fileDownnload(URL);
                            }
                        })
                        
                    }
                    else {
                        Swal.fire(
                            'Alerta!',
                            response.Message,
                            response.Id
                        )
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log(XMLHttpRequest,);
                }
            });
        
        }
    });

    $("#btnProcesar").click(function () {

    })


})
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
             
            //transporte
            $("#IdTrasnporte").val(response[0].Transporte.IdTrasnporte)
            $("#txtRucEmpresa").val(response[0].Transporte.Ruc)
            $("#txtEmpresa").val(response[0].Transporte.RazonSocial)
            $("#txtDirecciontr").val(response[0].Transporte.Direccion)

            $("#txtNomCli").val(response[0].Venta.cliente.Nombre)
            $("#txtDocCli").val(response[0].Venta.cliente.NroDocumento)
            $("#txtDireccion").val(response[0].Venta.cliente.Direccion)
            $("#txtPuntoLlegada").val(response[0].Venta.cliente.Direccion)
            $("#txtNumDocMod").val(response[0].Venta.serie + "-" + response[0].Venta.numero)
            $("#txtFecEmision").val(response[0].Venta.fechaEmision)



            var $td = $("#tbDetalle");
            $td.find('tbody').empty();
            console.log(response)
            $.grep(response, function (oDetalle) {
                Lista.Vars.Detalle.push({ 
                    precio: oDetalle["precio"],
                    cantidad: oDetalle["cantidad"],
                    descuento: oDetalle["descuento"],
                    descuentoPor: oDetalle["descuentoPor"],
                    Importe: oDetalle["Importe"],
                    Bulto: 0.00,
                    Peso: 0.00,
                    Material: {
                        Nombre: oDetalle["material"]["Nombre"],
                        Codigo: oDetalle["material"]["codigo"],
                        IdMaterial: oDetalle["material"]["IdMaterial"],
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
                        '<tr data-index="' + oDetalle["Material"]["IdMaterial"] + '">' +
                        '<td>' + formatNumber(oDetalle["cantidad"]) + '</td>' +
                        '<td>' + oDetalle["Material"]["Unidad"]["Nombre"] + '</td>' +
                        '<td>' + oDetalle["Material"]["Codigo"] + '-' + oDetalle["Material"]["Nombre"] + '</td>' +
                        '<td>' + formatNumber(oDetalle["precio"]) + '</td>' +
                        '<td>' + '<input type="text" class="form-control form-control-sm Cant" id="Cantidad" value="' + formatNumber(oDetalle["Bulto"]) + '">' + '</td>' +

                        '</tr>'
                    )
                })
            }
        }
    });
    $("[data-dismiss=modal]").trigger({ type: "click" });//cerrar modal.
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
function fileDownnload(url) {
    var options = {
        height: "600px",
        page: '2'

    };
    PDFObject.embed(url, "#PDFViewer", options);
    // $("#myReportPrint").modal('show');

}
function LimpiarConteniedo() {

    //Lista.CargarSerieDoc();
    $("#txtFechaEmision").val('');
    $("#lstTipoDoc").trigger('change');

    $("#lstTipoPago").val(0)
    $("#lstTipoPago").trigger('change');

    $("#IdCliente").val(0);
    $("#IdVenta").val(0);
    $("#IdComprobante").val(0);
    $("#txtNumDocMod").val('');
    $("#txtFecEmision").val('');
    $("#lstTipoDocCli").val('');
    $("#txtDocCli").val(''); 
    $("#txtNomCli").val(''); 
    $("#txtDireccion").val('');
    $("#txtPuntoLlegada").val(''); 
    $("#IdTrasnporte").val(0);
    $("#txtRucEmpresa").val('');
    $("#txtEmpresa").val('');
    $("#txtDirecciontr").val('');


    $("#txtCajas").val('0.00');
    $("#txtBidones").val('0.00');
    $("#txtSacos").val('0.00');
    $("#txtCilindro").val('0.00');
    $("#txtPeso").val('0.00');

    Lista.Vars.Detalle = [];
    var $tb = $('#tbDetalle');
    $tb.find('tbody').empty();
    if (Lista.Vars.Detalle.length == 0) {
        $tb.find('tbody').append('<tr><td colspan="11">No existen registros</td></tr>')
    }
    changeDoc($("#lstDocumento").val());

}
