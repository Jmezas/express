var Lista = {

    CargarOperacion: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaCombo"),
            dataType: 'json',
            data: { Id: 16 },
            success: function (response) {
                $('select[name="lstOperacion"]').empty();
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 

                    $.grep(response, function (oDocumento) {

                        $('select[name="lstOperacion"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
                    });
                }
            }

        });
    },
    CargarDoc: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaCombo"),
            dataType: 'json',
            data: { Id: 6 },
            success: function (response) {

                $("#lstTipoDoc").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 

                    $.grep(response, function (oDocumento) {

                        $('select[name="lstTipoDoc"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
                    });
                }
                $("#lstTipoDoc").val(1);
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
    CargarTipoDoc: function () {
        var sucursal = $("#sucursal").val();
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaComboId"),
            dataType: 'json',
            data: { flag: 9, Id: sucursal },
            success: function (response) {
                //console.log(response)
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 

                    $.grep(response, function (oDocumento) {

                        $('select[name="lstDocumento"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
                    });
                }
                changeDoc($("#lstDocumento").val())
                //$("#lstDocumento").val(2)
            }
        });
    },
    CargarAlmacen: function () {

        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaComboSucursal"),
            dataType: 'json',
            success: function (response) {

                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error  
                    $.grep(response, function (oDocumento) {
                        $('select[name="lstAlmacen"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
                    });
                }
            }

        });
    },
    CargarPago: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaCombo"),
            dataType: 'json',
            data: { Id: 19 },
            success: function (response) {

                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error  
                    $.grep(response, function (oDocumento) {
                        $('select[name="lstPago"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
                    });
                }
            }

        });
    },

    CargarCombo: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaCombo"),
            dataType: 'json',
            data: { Id: 6 },
            success: function (response) {

                $("#lstTipoDocCliente").empty()
                $("#lstTipoDocCliente").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 

                    $.grep(response, function (oDocumento) {

                        $('select[name="lstTipoDocCliente"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
                    });
                }
            }

        });
    },

    CargarDepartamento: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListarUbigeo"),
            dataType: 'json',
            data: { Acction: "DEPARTAMENTO", IdPais: '001', IdDep: "", IdProv: "", IdDis: "" },
            success: function (response) {
                $("#lstDepartamento").append($('<option>', { value: 0, text: 'Seleccione' }));
                $("#lstDistrito").append($('<option>', { value: 0, text: 'Seleccione' }));
                $("#lstProvincia").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 

                    $.grep(response, function (oDocumento) {
                        $('select[name="lstDepartamento"]').append($('<option>', { value: oDocumento["CodigoInei"], text: oDocumento["Nombre"] }));

                    });
                }
            }
        });
    },
    CargarBanco: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaCombo"),
            dataType: 'json',
            data: { Id: 26 },
            success: function (response) {
                $("#lstBanco").empty()
                $("#lstBanco").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error  
                    $.grep(response, function (oDocumento) {
                        $('select[name="lstBanco"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
                    });
                }

            }

        });
    },
    CargarTipoEnvio: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaCombo"),
            dataType: 'json',
            data: { Id: 27 },
            success: function (response) {
                $("#lstTipoEnvio").empty()
                $("#lstTipoEnvio").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error  
                    $.grep(response, function (oDocumento) {
                        $('select[name="lstTipoEnvio"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
                    });
                }
            }

        });
    },
    Vars: {
        Detalle: [],
        Pago: []
    }
}
$(function () {
    //Lista.CargarSerieDoc();
    Lista.CargarDoc();
    Lista.CargarMoneda();

    Lista.CargarTipoDoc();
    Lista.CargarPago();
    Lista.CargarCombo();
    Lista.CargarDepartamento();

    //INICIALIZAR CAMPOS
    $('#txtrecibida').Validate({ type: TypeValidation.Numeric, special: '.' });
    $('#txtPago').Validate({ type: TypeValidation.Numeric, special: '.' });
    $('#txtCostoEnvio').Validate({ type: TypeValidation.Numeric, special: '.' });

    $("#lstTipoEnvio").val(0);
    $('#fechaEnvio').val('');
    $("#txtGiftCard").val(0);

    $("#txtCambio").val('0.00');
    $("#controlPago").val('0.00');
    $("#sTotal").html('0.00');


    //$('#txtDocCli').select2({
    //    data: ["74539555-los lopez sac"]
    //});

    //ocultamos los div en modal pago
    $("#divBanco").hide();
    $("#divGiftCard").hide();
    $("#divEnvio").hide();
    $("#divCostoEnvio").hide();
    $("#divFechaEnvio").hide();

    $('#fechaEnvio').datepicker({
        dateFormat: 'dd/mm/yy',
        prevText: '<i class="fa fa-angle-left"></i>',
        nextText: '<i class="fa fa-angle-right"></i>'
    });


    $('#lstPago').select2({
        dropdownParent: $('#ModalEfectivo')
    });
    $('#lstTipoEnvio').select2({
        dropdownParent: $('#ModalEfectivo')
    });

    $('#lstTipoDocCliente').select2({
        dropdownParent: $('#ModalNuevo')
    });

    $('#lstDepartamento').select2({
        dropdownParent: $('#ModalNuevo')
    });
    $('#lstProvincia').select2({
        dropdownParent: $('#ModalNuevo')
    });
    $('#lstDistrito').select2({
        dropdownParent: $('#ModalNuevo')
    });

    $('#lstBanco').select2({
        dropdownParent: $('#ModalEfectivo')
    });


    Lista.CargarAlmacen();
    $("#lstDocumento").change(function () {
        changeDoc($("#lstDocumento").val())
    })

    $('#dFechaPago').datepicker({
        dateFormat: 'dd/mm/yy',
    });
    $("#dFechaPago").datepicker().datepicker("setDate", new Date());



    $("#lstDepartamento").change(function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListarUbigeo"),
            dataType: 'json',
            data: { Acction: "PROVINCIA", IdPais: '001', IdDep: $("#lstDepartamento").val(), IdProv: "", IdDis: "" },
            success: function (response) {
                $("#lstProvincia").empty();
                $("#lstDistrito").empty();
                $("#lstProvincia").append($('<option>', { value: 0, text: 'Seleccione' }));
                $("#lstDistrito").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 
                    $.grep(response, function (oDocumento) {
                        $('select[name="lstProvincia"]').append($('<option>', { value: oDocumento["CodigoInei"], text: oDocumento["Nombre"] }));

                    });
                }
            }

        });
    });

    $("#lstProvincia").change(function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListarUbigeo"),
            dataType: 'json',
            data: { Acction: "DISTRITO", IdPais: '001', IdDep: $("#lstDepartamento").val(), IdProv: $("#lstProvincia").val(), IdDis: "" },
            success: function (response) {
                $("#lstDistrito").empty();
                $("#lstDistrito").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 
                    $.grep(response, function (oDocumento) {
                        $('select[name="lstDistrito"]').append($('<option>', { value: oDocumento["CodigoInei"], text: oDocumento["Nombre"] }));

                    });
                }
            }

        });
    });





    //$("#producto").autocomplete({
    //    source: function (request, response) {
    //        var filtro = $("#producto").val();
    //        $.ajax({
    //            url: General.Utils.ContextPath('Shared/FiltroProducto'),
    //            type: "POST",
    //            dataType: "json",
    //            data: { filtro: filtro },
    //            success: function (data) {
    //                response($.map(data, function (item) {
    //                    return {
    //                        id: item.IdMaterial,
    //                        cod: item.Nombre,
    //                        label: item.Nombre + ' - ' + item.Codigo + ' - ' + item.PrecioVenta,
    //                        des: item.Nombre + '|' + item.Codigo + '|' + item.PrecioCompra + '|' + item.PrecioVenta,
    //                        cat: item.Categoria.Nombre,
    //                        mar: item.Marca.Nombre,
    //                        und: item.Unidad.Nombre,
    //                        ope: item.TipoOperacion.IdTipoOperacion + '|' + item.TipoOperacion.Nombre
    //                    };
    //                }));
    //            }
    //        });

    //    },

    //    minLength: 2,
    //    select: function (event, ui) {
    //        $("#txtProducto").val(ui.item ? ui.item.cod : $("#txtProducto").val());
    //        $('#IdProducto').val(ui.item.id);
    //    },
    //    change: function (event, ui) {

    //        Lista.Vars.Detalle.push({
    //            Material: {
    //                IdMaterial: ui.item.id,
    //                Codigo: ui.item.cod,
    //                Nombre: ui.item.des.toString().split('|')[1],
    //            },
    //            Categoria: ui.item.cat.toString(),
    //            Marca: ui.item.mar.toString(),
    //            Unidad: ui.item.und.toString(),
    //            Cantidad: 1,
    //            Precio: ui.item.des.toString().split('|')[3],
    //            Importe: ui.item.des.toString().split('|')[3] * 1,
    //            descuentopor: 0,
    //            descuento: 0,
    //            //operacion: 1,
    //            operacion: ui.item.ope.toString().split('|')[0],

    //            Almacen: { IdAlmacen: $("#lstAlmacen").val() },
    //            Sucursal: { IdSucursal: $("#lstSucursalCab").val() }
    //        });
    //        Lista.CargarOperacion();
    //        var $tb = $('#tbDetalle');
    //        $tb.find('tbody').empty();

    //        if (Lista.Vars.Detalle.length == 0) {
    //            $tb.find('tbody').append('<tr><td colspan="15">No existen registros</td></tr>')
    //        } else {

    //            $.grep(Lista.Vars.Detalle, function (oDetalle) {
    //                $tb.find('tbody').append(
    //                    '<tr data-index=' + oDetalle["Material"]["IdMaterial"] + '>' +
    //                    '<td>' + oDetalle["Material"]["Nombre"] + '</td>' +
    //                    '<td>' + oDetalle["Unidad"] + '</td>' +
    //                    '<td>' + '<select id="lstOperacion" name="lstOperacion" class="form-control form-control-sm select"></select>' + '</td>' +
    //                    '<td>' + '<input type="text" class="form-control form-control-sm desc" id="descuento"   value="' + oDetalle["descuento"] + '">' + '</td>' +
    //                    '<td>' + '<input type="text" class="form-control form-control-sm Cant" id="Cantidad"  value="' + (oDetalle["Cantidad"]) + '">' + '</td>' +
    //                    '<td>' + '<input type="text" class="form-control form-control-sm Price" id="Precio"  value="' + formatNumber(oDetalle["Precio"]) + '">' + '</td>' +
    //                    '<td>' + formatNumber(oDetalle["Importe"]) + '</td>' +
    //                    '<td class="text-center">' +
    //                    '<button type="submit" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>' +
    //                    '</td>' +
    //                    '</tr>'

    //                );
    //            });
    //        }

    //        $("#producto").val('')
    //        $("#producto").focus()
    //        $("#lstOperacion option[value='1']").attr("selected", true)
    //        CalcularTotales();
    //    }
    //});


    $("#txtDocCli").select2({
        ajax: {
            url: General.Utils.ContextPath('Shared/FiltroProvCli'),
            dataType: 'json',
            delay: 250,
            type: 'POST',
            data: function (params) {
                return {
                    filtro: params.term,
                    Tipo: $("#lstTipoDoc").val(),
                    flag: 2
                };
            },
            processResults: function (data, params) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            id: item.Id,
                            text: item.Text + '-' + item.Nombre,
                            des: item.Nombre + '|' + item.Dir
                        };
                    })
                };
            },
            cache: true
        },
        placeholder: 'Buscar Cliente',
        allowClear: true,
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        minimumInputLength: 1,
        language: {
            inputTooShort: function () {
                return "Buscar Cliente";
            }
        }
    });

    $('#txtDocCli').on('select2:select', function (e) {
        var data = e.params.data;
        //console.log(data)
        $('#hdfId').val(data.id);
        $('#cargarCliente').text(''); //limpiamos el label
    })



    $("#producto").select2({
        ajax: {
            url: General.Utils.ContextPath('Shared/FiltroProducto'),
            dataType: 'json',
            delay: 250,
            type: 'POST',
            data: function (params) {
                return {
                    filtro: params.term // search ter
                };
            },
            processResults: function (data, params) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.Nombre,
                            id: item.IdMaterial,
                            cod: item.Nombre,
                            des: item.Nombre + '|' + item.Codigo + '|' + item.PrecioCompra + '|' + item.PrecioVenta + '|' + item.Descuento,
                            cat: item.Categoria.Nombre,
                            mar: item.Marca.Nombre,
                            und: item.Unidad.Nombre,
                            ope: item.TipoOperacion.IdTipoOperacion + '|' + item.TipoOperacion.Nombre

                        };
                    })
                };
            },
            cache: true
        },
        placeholder: 'Busqueda producto',
        allowClear: true,
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        minimumInputLength: 1,
        language: {
            inputTooShort: function () {
                return "Busqueda producto";
            }
        }
    });

    $('#producto').on('select2:select', function (e) {
        $("#producto").empty();
        var data = e.params.data;

        if (BuscarDetalleEnTabla(data.id)) {
            General.Utils.ShowMessage(TypeMessage.Error, 'El producto ya existe en la tabla');
        } else {
            VerificarStock(data.id, 1, $("#lstAlmacen").val(), function (errorLanzado, datosDevueltos) {
                if (errorLanzado) // Ha habido un error, deberías manejarlo :/
                {
                    General.Utils.ShowMessage(TypeMessage.Error, 'Algo salio mal verificar');
                    return;
                }

                if (datosDevueltos.Id == 'error') {
                    General.Utils.ShowMessage(datosDevueltos.Id, datosDevueltos.Message);
                } else {

                    Lista.Vars.Detalle.push({
                        Material: {
                            IdMaterial: data.id,
                            Nombre: data.des.toString().split('|')[0],
                            codigo: data.des.toString().split('|')[1],
                        },
                        Categoria: data.cat.toString(),
                        Marca: data.mar.toString(),
                        Unidad: data.und.toString(),
                        Cantidad: 1,
                        Precio: data.des.toString().split('|')[3],
                        Importe: (data.des.toString().split('|')[3] * 1) - (data.des.toString().split('|')[4] * 1),
                        descuentopor: 0,
                        descuento: data.des.toString().split('|')[4],
                        //operacion: 1,
                        operacion: parseInt(data.ope.toString().split('|')[0]),
                        DesOperacion: data.ope.toString().split('|')[1],
                        Almacen: { IdAlmacen: $("#lstAlmacen").val() },
                        Sucursal: { IdSucursal: $("#lstSucursalCab").val() }
                    });

                    //console.log(Lista.Vars.Detalle)

                    Lista.CargarOperacion();
                    var $tb = $('#tbDetalle');
                    $tb.find('tbody').empty();

                    if (Lista.Vars.Detalle.length == 0) {
                        $tb.find('tbody').append('<tr><td colspan="15">No existen registros</td></tr>')
                    } else {

                        $.grep(Lista.Vars.Detalle, function (oDetalle) {
                            $tb.find('tbody').append(
                                '<tr data-index=' + oDetalle["Material"]["IdMaterial"] + '>' +
                                '<td>' + oDetalle["Material"]["codigo"] + '-' + oDetalle["Material"]["Nombre"] + '</td>' +
                                '<td>' + oDetalle["Unidad"] + '</td>' +
                                '<td>' + oDetalle["DesOperacion"] + '</td>' +
                                //'<td>' + '<select id="lstOperacion" name="lstOperacion" class="form-control select"></select>' + '</td>' +
                                '<td>' + '<input type="text" class="form-control form-control-sm desc" id="descuento"  value="' + oDetalle["descuento"] + '" disabled>' + '</td>' +
                                '<td>' + '<input type="text" class="form-control form-control-sm Cant" id="Cantidad"  value="' + (oDetalle["Cantidad"]) + '">' + '</td>' +
                                '<td>' + '<input type="text" class="form-control form-control-sm Price" id="Precio" disabled value="' + formatNumber(oDetalle["Precio"]) + '">' + '</td>' +
                                '<td>' + formatNumber(oDetalle["Importe"]) + '</td>' +
                                '<td class="text-center">' +
                                '<button type="submit" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>' +
                                '</td>' +
                                '</tr>'

                            );
                        });
                    }

                    $("#producto").empty();
                    $("#producto").focus()
                    $("#lstOperacion option[value='1']").attr("selected", true)
                    CalcularTotales();
                }
            })
        }

    });


    $('#tbDetalle').find('tbody').on('click', '.btn-danger', function () {
        var $btn = $(this);
        var $tb = $('#tbDetalle');

        var Id = $btn.closest('tr').attr('data-index');

        BuscarIndexDetalleEnTabla(Id);
        arrDetalle = Lista.Vars.Detalle.filter(function (x) {
            return x.Material.IdMaterial != Id;
        });

        Lista.Vars.Detalle = [];
        Lista.Vars.Detalle = arrDetalle;

        $tb.find('tbody').empty();
        if (Lista.Vars.Detalle.length == 0) {
            $tb.find('tbody').append('<tr><td colspan="9">No existen registros</td></tr>')

        } else {
            $.grep(Lista.Vars.Detalle, function (oDetalle) {
                $tb.find('tbody').append(
                    '<tr data-index=' + oDetalle["Material"]["IdMaterial"] + '>' +
                    '<td>' + oDetalle["Material"]["codigo"] + '-' + oDetalle["Material"]["Nombre"] + '</td>' +
                    '<td>' + oDetalle["Unidad"] + '</td>' +
                    '<td>' + oDetalle["DesOperacion"] + '</td>' +
                    //'<td>' + '<select id="lstOperacion" name="lstOperacion" class="form-control form-control-sm select"></select>' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm desc" id="descuento"  value="' + oDetalle["descuento"] + '" disabled>' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Cant" id="Cantidad"  value="' + formatNumber(oDetalle["Cantidad"]) + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Price" id="Precio" disabled value="' + formatNumber(oDetalle["Precio"]) + '">' + '</td>' +
                    '<td>' + formatNumber(oDetalle["Importe"]) + '</td>' +
                    '<td class="text-center">' +
                    '<button type="submit" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>' +
                    '</td>' +
                    '</tr>'
                );
            })
        }
        Lista.CargarOperacion();
        CalcularTotales();

    });
    $('#tbDetalle').on('change', '.select', function () {
        var row = $(this).closest('tr');
        var Id = $(this).closest('tr').attr('data-index');

        var input = Number($(this).val());
        //console.log(input)
        Lista.Vars.Detalle.map(function (data) {
            if (parseInt(data.Material.IdMaterial) == parseInt(Id)) {
                data.operacion = input
            }
        })

        CalcularTotales();
    });

    //ACtulizar cantidad
    $('#tbDetalle').on('change', '.Cant', function () {
        var row = $(this).closest('tr');
        var Id = $(this).closest('tr').attr('data-index');
        //var Cantidad = 0;
        //var input = parseInt($(this).val());

        var input = parseFloat($(this).val());

        //console.log($(this).val())
        //$('input', row).each(function () {
        //    input = 
        //});

        //VerificarStock(Id, input, $("#lstAlmacen").val(), function (errorLanzado, datosDevueltos) {
        //    if (errorLanzado) // Ha habido un error, deberías manejarlo :/
        //    {
        //        General.Utils.ShowMessage(TypeMessage.Error, 'Algo salio mal verificar');
        //        return;
        //    }

        //    if (datosDevueltos.Id == 'error') {
        //        General.Utils.ShowMessage(datosDevueltos.Id, datosDevueltos.Message);
        //    } else {

        //    }
        //});

        Lista.Vars.Detalle.map(function (data) {
            if (parseInt(data.Material.IdMaterial) == parseInt(Id)) {
                //data.Cantidad = parseInt(input);
                data.Cantidad = parseFloat(input);
                data.Importe = (input * data.Precio) - (data.descuento * parseFloat(input));
                data.descuento = data.descuento;
                data.descuentopor = 0;
            }
        })

        Lista.CargarOperacion();
        CalcularTotales();
        var $tb = $('#tbDetalle');
        $tb.find('tbody').empty();
        if (Lista.Vars.Detalle.length == 0) {
            $tb.find('tbody').append('<tr><td colspan="15">No existen registros</td></tr>')
        } else {

            $.grep(Lista.Vars.Detalle, function (oDetalle) {
                $tb.find('tbody').append(
                    '<tr data-index=' + oDetalle["Material"]["IdMaterial"] + '>' +
                    '<td>' + oDetalle["Material"]["codigo"] + '-' + oDetalle["Material"]["Nombre"] + '</td>' +
                    '<td>' + oDetalle["Unidad"] + '</td>' +
                    '<td>' + oDetalle["DesOperacion"] + '</td>' +
                    //'<td>' + '<select id="lstOperacion" name="lstOperacion" class="form-control form-control-sm select"></select>' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm desc" id="descuento" value="' + oDetalle["descuento"] + '" disabled>' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Cant" id="Cantidad"  value="' + (oDetalle["Cantidad"]) + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Price" id="Precio" disabled value="' + formatNumber(oDetalle["Precio"]) + '">' + '</td>' +
                    '<td>' + formatNumber(oDetalle["Importe"]) + '</td>' +
                    '<td class="text-center">' +
                    '<button type="submit" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>' +
                    '</td>' +
                    '</tr>'
                );
            });
        }
    });

    //Actulizar Precio
    $('#tbDetalle').on('change', '.Price', function () {
        var row = $(this).closest('tr');
        var Id = $(this).closest('tr').attr('data-index');

        var input = Number($(this).val());
        Lista.Vars.Detalle.map(function (data) {
            if (parseInt(data.Material.IdMaterial) == parseInt(Id)) {
                data.Importe = (input * data.Cantidad) * 1;
                data.Precio = parseFloat(input);
                data.descuento = 0;
                data.descuentopor = 0;
            }
        })
        CalcularTotales();
        Lista.CargarOperacion()
        var $tb = $('#tbDetalle');
        $tb.find('tbody').empty();
        if (Lista.Vars.Detalle.length == 0) {
            $tb.find('tbody').append('<tr><td colspan="15">No existen registros</td></tr>')
        } else {

            $.grep(Lista.Vars.Detalle, function (oDetalle) {
                $tb.find('tbody').append(
                    '<tr data-index=' + oDetalle["Material"]["IdMaterial"] + '>' +
                    '<td>' + oDetalle["Material"]["codigo"] + '-' + oDetalle["Material"]["Nombre"] + '</td>' +
                    '<td>' + oDetalle["Unidad"] + '</td>' +
                    '<td>' + oDetalle["DesOperacion"] + '</td>' +
                    //'<td>' + '<select id="lstOperacion" name="lstOperacion" class="form-control form-control-sm select"></select>' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm desc" id="descuento" value="' + oDetalle["descuento"] + '" disabled>' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Cant" id="Cantidad"  value="' + (oDetalle["Cantidad"]) + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Price" id="Precio" disabled value="' + formatNumber(oDetalle["Precio"]) + '">' + '</td>' +
                    '<td>' + formatNumber(oDetalle["Importe"]) + '</td>' +
                    '<td class="text-center">' +
                    '<button type="submit" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>' +
                    '</td>' +
                    '</tr>'

                );
            });
        }
    });

    //Actualizar descuento %
    $('#tbDetalle').on('change', '.por', function () {
        var row = $(this).closest('tr');
        var Id = $(this).closest('tr').attr('data-index');


        var descuento = $("#txtDescuento").val();
        var descuentoPorcentaje = $("#txtDescuentoPor").val();
        var precio = $("#txtPrecio").val();
        var cantidad = $("#txtCantidad").val();
        var Total = $("#hdf_TotalImporte").val();

        var resta = 0;
        var diferenciaDescuento = 0;
        var desUnidad = 0;

        diferenciaDescuento = (precio * descuentoPorcentaje) / 100;

        desUnidad = precio - diferenciaDescuento
        Total = Total * 1
        Total = desUnidad * cantidad;
        descuento = (precio * cantidad) - Total;


        $("#txtDescuento").val(descuento.toFixed(2));
        $("#hdfprecio").val(desUnidad.toFixed(2));
        $("#txtTotal").val(Total.toFixed(2));


        ////

        var diferenciaDescuento = 0;
        var desUnidad = 0;




        var input = Number($(this).val());
        Lista.Vars.Detalle.map(function (data) {
            if (parseInt(data.Material.IdMaterial) == parseInt(Id)) {

                diferenciaDescuento = (data.Precio * input) / 100;
                desUnidad = data.Precio - diferenciaDescuento

                data.Importe = (desUnidad * data.Cantidad);
                data.descuento = (data.Precio * data.Cantidad) - data.Importe;
                data.descuentopor = input;

            }
        })
        Lista.CargarOperacion();
        CalcularTotales();
        var $tb = $('#tbDetalle');
        $tb.find('tbody').empty();
        if (Lista.Vars.Detalle.length == 0) {
            $tb.find('tbody').append('<tr><td colspan="15">No existen registros</td></tr>')
        } else {

            $.grep(Lista.Vars.Detalle, function (oDetalle) {
                $tb.find('tbody').append(
                    '<tr data-index=' + oDetalle["Material"]["IdMaterial"] + '>' +
                    '<td>' + oDetalle["Material"]["codigo"] + '-' + oDetalle["Material"]["Nombre"] + '</td>' +
                    '<td>' + oDetalle["Unidad"] + '</td>' +
                    '<td>' + oDetalle["DesOperacion"] + '</td>' +
                    //'<td>' + '<select id="lstOperacion" name="lstOperacion" class="form-control form-control-sm select"></select>' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm desc" id="descuento"  value="' + oDetalle["descuento"] + '" disabled>' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Cant" id="Cantidad"  value="' + (oDetalle["Cantidad"]) + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Price" id="Precio" disabled value="' + formatNumber(oDetalle["Precio"]) + '">' + '</td>' +
                    '<td>' + formatNumber(oDetalle["Importe"]) + '</td>' +
                    '<td class="text-center">' +
                    '<button type="submit" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>' +
                    '</td>' +
                    '</tr>'
                );
            });
        }
    });

    $('#tbDetalle').on('change', '.desc', function () {
        var row = $(this).closest('tr');
        var Id = $(this).closest('tr').attr('data-index');
        var descuentoPorcentaje = 0;
        var descuentoMonto = 0;
        var desUnidad = 0;
        var input = Number($(this).val());

        Lista.Vars.Detalle.map(function (data) {
            if (parseInt(data.Material.IdMaterial) == parseInt(Id)) {
                descuentoPorcentaje = (input * 100) / (data.Precio * data.Cantidad);
                descuentoMonto = (data.Precio * descuentoPorcentaje) / 100;
                desUnidad = data.Precio - descuentoMonto
                data.Importe = (desUnidad * data.Cantidad);
                data.descuento = input;
                data.descuentopor = descuentoPorcentaje;
            }
        })

        Lista.CargarOperacion();
        CalcularTotales();
        var $tb = $('#tbDetalle');
        $tb.find('tbody').empty();
        if (Lista.Vars.Detalle.length == 0) {
            $tb.find('tbody').append('<tr><td colspan="15">No existen registros</td></tr>')
        } else {

            $.grep(Lista.Vars.Detalle, function (oDetalle) {
                $tb.find('tbody').append(
                    '<tr data-index=' + oDetalle["Material"]["IdMaterial"] + '>' +
                    '<td>' + oDetalle["Material"]["codigo"] + '-' + oDetalle["Material"]["Nombre"] + '</td>' +
                    '<td>' + oDetalle["Unidad"] + '</td>' +
                    '<td>' + oDetalle["DesOperacion"] + '</td>' +
                    //'<td>' + '<select id="lstOperacion" name="lstOperacion" class="form-control form-control-sm select"></select>' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm desc" id="descuento" value="' + oDetalle["descuento"] + '"disabled>' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Cant" id="Cantidad"  value="' + (oDetalle["Cantidad"]) + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Price" id="Precio" disabled value="' + formatNumber(oDetalle["Precio"]) + '">' + '</td>' +
                    '<td>' + formatNumber(oDetalle["Importe"]) + '</td>' +
                    '<td class="text-center">' +
                    '<button type="submit" class="btn btn-danger btn-xs"><i class="fa fa-trash"></i></button>' +
                    '</td>' +
                    '</tr>'

                );
            });
        }
    });

    $('#tbDetalle').on('change', '.select', function () {
        var row = $(this).closest('tr');
        var Id = $(this).closest('tr').attr('data-index');

        var input = Number($(this).val());
        //console.log(input)
        Lista.Vars.Detalle.map(function (data) {
            if (parseInt(data.Material.IdMaterial) == parseInt(Id)) {
                data.operacion = input
            }
        })
        CalcularTotales();

    });

    $("#btnEfectivo").click(function () {
        //agregamos atributo enabled cuando es efectivo
        if ($("#hdfId").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese el cliente');
        } else if (Lista.Vars.Detalle.length <= 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'No se ha ingresado el detalle');
        } else if ($("#lstDocumento").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'El tipo de documento no está seleccionado');
        } else if ($("#lstAlmacen").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'El almacén no está seleccionado');
        } else if ($("#lstMoneda").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'La moneda no está seleccionada');
        } else {
            //abrimos modal
            $("#btnMostrarModal").click();

            //limpiamos datos modal
            LimpiarModalEfectivo();

            //var $tb = $('#tbPago');
            //$tb.find('tbody').empty();
            ////$('#tbPago tbody > tr').empty();
            ////limpiamos array
            //Lista.Vars.Pago = [];

            $("#txtrecibida").prop('disabled', false);
            $("#btn10").prop('disabled', false);
            $("#btn20").prop('disabled', false);
            $("#btn50").prop('disabled', false);
            $("#btn100").prop('disabled', false);
            $("#btnLimpiar").prop('disabled', false);

            numero = 0

            //$("#txtrecibida").val('0.00');
            //$("#txtCambio").val('0.00');
            //$("#txtCostoEnvio").val('0.00');
            //$("#controlPago").val('0.00');
            //$("#sTotal").html('0.00');



            var total = parseFloat($("#Totales").val());
            $("#txtPago").val(total.toFixed(2));
            $("#txtrecibida").val(total.toFixed(2));
            $("#lstPago").val(1);
        }

    });

    //$("#btntargeta").click(function () {
    //    var total = parseFloat($("#Totales").val());
    //    $("#txtrecibida").val(total.toFixed(2))
    //    $("#txtCambio").val('0.00')
    //    $("#txtCostoEnvio").val('0.00')
    //    //agregamos atributo disabled cuando es tarjeta
    //    $("#txtrecibida").prop('disabled', true);
    //    $("#btn10").prop('disabled', true);
    //    $("#btn20").prop('disabled', true);
    //    $("#btn50").prop('disabled', true);
    //    $("#btn100").prop('disabled', true);
    //    $("#btnLimpiar").prop('disabled', true);

    //    var total = parseFloat($("#Totales").val());
    //    $("#txtPago").val(total.toFixed(2));
    //    $("#lstPago").val(2)
    //    $("#lstPago").trigger('change');
    //});

    //$("#btncredito").click(function () {
    //    $("#txtrecibida").val('0.00')
    //    $("#txtCostoEnvio").val('0.00')
    //    $("#txtCambio").val('0.00')

    //    var total = parseFloat($("#Totales").val());
    //    $("#txtPago").val(total.toFixed(2));
    //    $("#lstPago").val(3)
    //});

    $("#btnLimpiarFormulario").click(function () {
        LimpiarConteniedo();
    });

    var numero = 0;
    $("#btn10").click(function () {

        numero += 10;
        var total = $("#Totales").val();
        $("#txtrecibida").val(formatNumber(numero))
        var mostrarPago = numero - total
        $("#txtCambio").val(formatNumber(mostrarPago))

    });

    $("#btn20").click(function () {

        numero += 20;
        var total = $("#Totales").val();
        $("#txtrecibida").val(formatNumber(numero))
        var mostrarPago = numero - total
        $("#txtCambio").val(formatNumber(mostrarPago))
    });

    $("#btn50").click(function () {

        numero += 50;
        var total = $("#Totales").val();
        $("#txtrecibida").val(formatNumber(numero))
        var mostrarPago = numero - total
        $("#txtCambio").val(formatNumber(mostrarPago))
    });

    $("#btn100").click(function () {
        numero += 100;
        var total = $("#Totales").val();
        $("#txtrecibida").val(formatNumber(numero))
        var mostrarPago = numero - total
        $("#txtCambio").val(formatNumber(mostrarPago))
    });

    $("#btnLimpiar").click(function () {
        numero = 0
        $("#txtrecibida").val('0.00');

        LimpiarModalEfectivo();
    });



    $("#btnBuscar").click(function () {
        BuscarSunat();
    });

    $("#btnGrabar").click(function () {

        var $form = $("#ModalNuevo");
        var oDatos = General.Utils.SerializeForm($form);

        if ($("#lstTipoDocCliente").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'Seleccione tipo de documento');

        } else {
            if (General.Utils.ValidateForm($form)) {
                var oDatos = {
                    IdCliente: $("#hdfIdCliente").val(),
                    Id: $("#lstTipoDocCliente").val(),
                    NroDocumento: $("#txtDocumento").val(),
                    Razonsocial: $("#txtRazon").val(),
                    Telefono: $("#txtTelefono").val(),
                    Celular: $("#txtCelular").val(),
                    Email: $("#txtEmail").val(),
                    Direccion: $("#txtDireccion").val(),
                    Ubigeo: {
                        CodigoDepartamento: $("#lstDepartamento").val(),
                        CodigoProvincia: $("#lstProvincia").val(),
                        CodigoDistrito: $("#lstDistrito").val(),

                    },

                }

                $.ajax({
                    async: true,
                    type: 'post',
                    url: General.Utils.ContextPath('Mantenimiento/InstCliente'),
                    dataType: 'json',
                    data: oDatos,
                    beforeSend: General.Utils.StartLoading,
                    complete: General.Utils.EndLoading,
                    success: function (response) {
                        // console.log(response)

                        if (response["Id"] == TypeMessage.Success) {

                            Swal.fire(
                                'Exito!',
                                response.Message,
                                response.Id
                            )

                            //obtengo el id del cliente registrado para cargarlo en el input automaticamente
                            var infoCliente = response.Message.split('=')[1];

                            $("#hdfId").val(infoCliente.split('.')[0]); //cragamos el id del cliente en el input por defecto
                            var nombCliente = infoCliente.split('.')[1];

                            $('#cargarCliente').text(nombCliente); //cargamos el label

                            //$('#txtDocCli').select2({
                            //    allowClear: true,
                            //    data: [nombCliente]
                            //});

                            //$("#txtDocCli").on('select2:unselect', function (e) {
                            //    alert('hello data');
                            //});

                            // $('#txtDocCli').select2("destroy");



                        } else {

                            Swal.fire(
                                'Error!',
                                response.Message,
                                response.Id
                            )
                        }
                        $("[data-dismiss=modal]").trigger({ type: "click" });//cerrar modal
                        //Limpiar();
                    }
                });
            }
        }
    });

    $("#aAgregarCliente").click(function () {
        Limpiar();
    });

    $("#btnGuardar").click(function () {
        if ($("#hdfId").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese el cliente');
        } else if (Lista.Vars.Detalle.length <= 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'No se ha ingresado el detalle');
        } else if ($("#lstDocumento").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'El tipo de documento no está seleccionado');
        } else if ($("#lstAlmacen").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'El almacén no está seleccionado');
        } else if ($("#lstMoneda").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'La moneda no está seleccionada');
        } else if (Lista.Vars.Pago.length <= 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'No se ha ingresado el pago');
        } else if (parseFloat($("#controlPago").val()) !== parseFloat($("#txtPago").val())) {
            General.Utils.ShowMessage(TypeMessage.Error, 'El total pago no es igual a al total a cobrar');
        } else {
            var oDatos = {
                cliente: { IdCliente: $("#hdfId").val() },
                Comprobante: { Id: $("#lstDocumento").val() },
                moneda: { IdMoneda: $("#lstMoneda").val() },
                Documento: { IdDocumento: 3 },
                fechaEmision: $("#dFechaPago").val(),
                fechaPago: $("#dFechaPago").val(),
                serie: $("#txtSerie").val(),
                numero: $("#txtNumero").val(),
                cantidad: $("#mCant").html(),
                grabada: $("#subtotales").val(),
                inafecta: $("#inafecta").val(),
                exonerada: $("#exonerada").val(),
                gratuita: $("#gratuita").val(),
                igv: $("#igv").val(),
                total: $("#Totales").val(),
                descuento: $("#descuento").val(),
                cambio: 3.5,
                observacion: $("#txtObservacion").val(),
                Sucursal: { IdSucursal: $("#lstSucursalCab").val() },
                montoRecibido: $("#txtrecibida").val(),
                vuelto: $("#txtCambio").val(),
                metodoPago: $("#lstPago").val(),
                NotaPago: $("#txtNotaPago").val(),
                CanalesVenta: $("#lstTipoVenta").val(),
                CodigoProducto: $("#txtGiftCard").val(),
                idBanco: $("#lstBanco").val(),
                Envio: { Id: $("#lstTipoEnvio").val() },
                CostoEnvio: $("#txtCostoEnvio").val(),
                fechaEnvio: $("#fechaEnvio").val(),
            }
            console.log(oDatos);

            $.ajax({
                async: true,
                type: 'post',
                url: General.Utils.ContextPath('Venta/InstRegistrarPost'),
                dataType: 'json',
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                data: { oDatos: oDatos, pago: Lista.Vars.Pago, Detalle: Lista.Vars.Detalle },
                success: function (response) {
                    if (response.Id == 'success') {
                        $("[data-dismiss=modal]").trigger({ type: "click" });//cerrar modal
                        Swal.fire({
                            title: 'title',
                            html: response.Message,
                            icon: response.Id,
                            showCancelButton: true,

                            confirmButtonText: 'ok'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                document.getElementById('reporte').click();
                                var URL = General.Utils.ContextPath('Venta/imprimirFacBol?Id=' + response.Additionals[0] + "&Envio=" + 0 + "&venta=" + 0);
                                fileDownnload(URL);
                            }
                        })
                        LimpiarConteniedo();
                    } else {
                        Swal.fire(
                            'Alerta!',
                            response.Message,
                            response.Id
                        )
                    }
                }
            })
        }
    });

    //$("#hdfMonto").text('0.00');
    $("#btnbuscarCodigo").click(function () {
        if (!$("#txtGiftCard").val()) {
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese el codigo');
        } else {
            $.ajax({
                async: true,
                type: 'post',
                url: General.Utils.ContextPath('Shared/BuscarCodigo'),
                dataType: 'json',
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                data: { Codigo: $("#txtGiftCard").val() },
                success: function (response) {
                    if (response['Text'] == '0.00') {
                        $("#hdfMonto").text('código invalido, digite  otro código ');
                        $("#estGiftcard").val('invalido');
                    } else {
                        $("#EstadoGiftCard").text('Monto: ');
                        $("#hdfMonto").text(response['Text']);
                    }

                }
            });
        }

    });

    //input cantidad recibida
    //var vto = 0;
    //$("#txtrecibida").on('change', function () {
    //    if ($("#controlPago").val() > 0) {
    //        vto = $("#txtPago").val() - $("#controlPago").val();
    //        $("#txtCambio").val(($("#txtrecibida").val() - vto).toFixed(2));

    //    } else {
    //        var cmb = $("#txtrecibida") - $("#txtPago").val();
    //        $("#txtCambio").val(cmb.toFixed(2));

    //        vto = $("#txtrecibida").val();
    //    }

    //    if ($("#txtrecibida").val() <= $("#txtPago").val()) {
    //        $("#txtCambio").val('0.00');
    //    } else {
    //        $("#txtCambio").val(($("#txtrecibida").val() - $("#txtPago").val()).toFixed(2));
    //        vto = $("#txtrecibida").val();
    //    }
    //});


    //agregar pagos
    var IdPago = 0;

    $("#btnAgregarPago").click(function () {

        if ($("#txtrecibida").val() == '0.00') {
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese la cantidad recibida');
        }
        else if ($("#hdfMonto").text() == '0.00' || $("#estGiftcard").val() == 'invalido') {
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese un GIFT CARD válido');
        }
        //else if (parseFloat($("#txtrecibida").val()) > parseFloat($("#txtPago").val())) {
        //    General.Utils.ShowMessage(TypeMessage.Error, 'La cantidad recibida es mayor al total a cobrar');
        //}
        else if (parseFloat($("#controlPago").val()) == parseFloat($("#txtPago").val())) {
            $("#txtrecibida").val('0.00')
            General.Utils.ShowMessage(TypeMessage.Error, 'No se puede agregar más pagos');
        }
        else {

            var vto = 0;

            var mPago = parseFloat($("#txtPago").val());
            var mrecibida = parseFloat($("#txtrecibida").val());
            var mControl = parseFloat($("#controlPago").val());
            var vuelto = 0;

            if (mControl > 0) {
                vto = (mPago - mControl).toFixed(2);

                //monto recibido es mayor al vto
                if (mrecibida > vto) {
                    vuelto = mPago - mControl;
                    $("#txtCambio").val((mrecibida - vuelto).toFixed(2));
                } else {
                    $("#txtCambio").val('0.00');
                }

            } else {


                if (mrecibida > mPago) {
                    vto = parseFloat(mPago.toFixed(2));

                    vuelto = mrecibida - mPago;
                    $("#txtCambio").val(vuelto.toFixed(2));
                } else {
                    vto = parseFloat(mrecibida.toFixed(2));
                    $("#txtCambio").val('0.00');
                }
            }

            //if (parseFloat($("#txtrecibida").val()) <= parseFloat(vto)) {
            //    $("#txtCambio").val('0.00');
            //} else {
            //    $("#txtCambio").val((parseFloat($("#txtrecibida").val()) - parseFloat($("#txtPago").val())).toFixed(2));
            //    vto = parseFloat($("#txtrecibida").val());
            //}


            Lista.Vars.Pago.push({
                id: IdPago = IdPago + 1,
                Pago: vto,
                TipoPago: $("#lstPago").val(),
                PagoDes: $('select[name="lstPago"] option:selected').text(),
                banco: $("#lstBanco").val() == 0 ? null : $("#lstBanco").val(),
                codigoBanco: ($("#lstBanco").val() == 0 ? '' : $('select[name="lstBanco"] option:selected').text()) + $("#txtGiftCard").val(),
            })

            var $tb = $('#tbPago');
            $tb.find('tbody').empty();
            var pago = 0;
            $.grep(Lista.Vars.Pago, function (oDetalle) {
                numero = parseFloat(oDetalle["Pago"]);
                //var total = parseFloat($("#txtPago").val()); 
                //var mostrarPago = numero - total; 
                //$("#txtCambio").val(mostrarPago); 
                pago = pago + parseFloat(oDetalle["Pago"]);
                $("#controlPago").val(pago);
                $("#sTotal").html(pago);
            });


            if (pago == mPago) {
                $("#txtrecibida").val('0.00');
            } else {
                var rest = 0;

                rest = mPago - pago;
                $("#txtrecibida").val(rest.toFixed(2));
                $("#txtCambio").val("0.00");
            }

            $.grep(Lista.Vars.Pago, function (oDetalle) {

                $tb.find('tbody').append(
                    '<tr data-index=' + oDetalle["id"] + '>' +
                    '<td>' + oDetalle["Pago"] + '</td>' +
                    '<td>' + oDetalle["PagoDes"] + '</td>' +
                    '<td>' + oDetalle["codigoBanco"] + '</td>' +
                    '<td class="text-center">' +
                    '<button type="submit" class="btn btn-danger btn-sm limpiarTabla"><i class="fa fa-trash"></i></button>' +
                    '</td>' +
                    '</tr>'
                );
            });

        }
        //limpiamos select 
        console.log(Lista.Vars.Pago);
    });

    //elimiar datos de la tabla de pagos
    $('#tbPago').find('tbody').on('click', '.limpiarTabla', function () {

        //$("#controlPago").val('0.00'); 
        //$("#sTotal").html('0.00'); 

        $("#txtCambio").val("0.00");//limpio txtCambio

        var $btn = $(this);
        var $tb = $('#tbPago');

        var Id = $btn.closest('tr').attr('data-index');

        BuscarIdPago(Id);
        arrDetalle = Lista.Vars.Pago.filter(function (x) {
            return x.id != Id;
        });

        Lista.Vars.Pago = [];
        Lista.Vars.Pago = arrDetalle;

        $tb.find('tbody').empty();
        if (Lista.Vars.Pago.length == 0) {
            $("#txtCambio").val('0.00');
            $("#txtrecibida").val($("#txtPago").val());
            $("#controlPago").val('0.00');
            $("#sTotal").html('0.00');

            $tb.find('tbody').append('<tr><td colspan="9">No existen pagos ingresados</td></tr>')

        } else {
            var pagos = 0;
            $.grep(Lista.Vars.Pago, function (oDetalle) {
                pagos = pagos + parseFloat(oDetalle["Pago"])

            })

            var total = parseFloat($("#txtPago").val());
            //var mostrarPago = pagos - total;
            var mostrarPago = total - pagos;

            $("#txtrecibida").val(mostrarPago.toFixed(2));
            $("#controlPago").val(pagos.toFixed(2));
            $("#sTotal").html(pagos.toFixed(2));

            $.grep(Lista.Vars.Pago, function (oDetalle) {


                $tb.find('tbody').append(
                    '<tr data-index=' + oDetalle["id"] + '>' +
                    '<td>' + oDetalle["Pago"] + '</td>' +
                    '<td>' + oDetalle["PagoDes"] + '</td>' +
                    '<td>' + oDetalle["codigoBanco"] + '</td>' +
                    '<td class="text-center">' +
                    '<button type="submit" class="btn btn-danger btn-sm limpiarTabla"><i class="fa fa-trash"></i></button>' +
                    '</td>' +
                    '</tr>'
                );
            });
        }

    });


})
function fileDownnload(url) {
    var options = {
        height: "600px",
        page: '2'

    };
    PDFObject.embed(url, "#PDFViewer", options);
    // $("#myReportPrint").modal('show');

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


function LimpiarConteniedo() {

    // Lista.CargarSerieDoc();
    $("#lstTipoDoc").val(1);
    //$("#lstDocumento").val(2)
    $("#txtDocCli").val('');
    $("#hdfId").val(0);

    $("#MontoSubtotal").html('0.00');
    $("#subtotales").val(0);

    $("#MontoGratuita").html('0.00');
    $("#gratuita").val(0);

    $("#Montoexonerada").html('0.00');
    $("#exonerada").val(0);

    $("#Montoinafecta").html('0.00');
    $("#inafecta").val(0);

    $("#mCant").html('0.00');
    $("#dFechaAten").datepicker().datepicker("setDate", new Date());

    $("#MontoIGV").html('0.00');
    $("#igv").val(0);

    $("#Montodescuento").html('0.00');
    $("#descuento").val(0);

    $("#Totales").val(0);
    $("#MontoCalulado").html('0.00');

    $("#hMonedaComprobante").html('SOLES');
    $("#txtDocCli").empty();
    Lista.Vars.Detalle = [];
    var $tb = $('#tbDetalle');
    $tb.find('tbody').empty();
    if (Lista.Vars.Detalle.length == 0) {
        $tb.find('tbody').append('<tr><td colspan="11">No existen pagos ingresados</td></tr>')
    }
    changeDoc($("#lstDocumento").val());
    $('#cargarCliente').text(''); //limpiamos el labelS

    $("#txtPago").val('0')
    $('#txtPago').trigger('change');

    $("#lstBanco").val('0')
    $('#lstBanco').trigger('change');

    $("#divGiftCard").hide();
    $("#txtGiftCard").val('0');

    $("#lstTipoEnvio").val('0');


    $("#lstPago").val(1);

    $('#fechaEnvio').val('');

    $("#checkEnvio").prop("checked", false);

    $("#checkEnvio").trigger("change");

}

function LimpiarModalEfectivo() {
    $("#txtCambio").val('0.00');
    $("#controlPago").val('0.00');
    $("#txtGiftCard").val('0');
    $("#hdfMonto").text('');

    $("#txtCostoEnvio").val('0.00');
    $("#fechaEnvio").val('');
    $("#txtNotaPago").val('');
    $("#estGiftcard").val('');

    $("#lstPago").val(1);
    $("#lstPago").trigger("change");

    $("#lstBanco").val(0);

    $("#lstTipoEnvio").val('0');
    $("#lstTipoVenta").val(1);

    $("#sTotal").html('0.00');

    //limpiamos la tabla pago xsi tenga datos
    var $tb = $('#tbPago');
    $tb.find('tbody').empty();
    //limpiamos array
    Lista.Vars.Pago = [];

    $("#checkEnvio").prop("checked", false);
    $("#checkEnvio").trigger("change");


}

function BuscarDetalleEnTabla(iIdProducto) {
    var bFound = false;
    $.each(Lista.Vars.Detalle, function (index, item) {
        if (item["Material"].IdMaterial == iIdProducto) {
            bFound = true;
            return false;
        }
    });
    return bFound;
}

function BuscarIndexDetalleEnTabla(id) {
    for (var i = 0; i < Lista.Vars.Detalle.length; i += 1) {
        if (Lista.Vars.Detalle[i]["Material"]["IdMaterial"] == id) {
            return i;
        }
    }
    return -1;
}
function BuscarIdPago(id) {
    for (var i = 0; i < Lista.Vars.Pago.length; i += 1) {
        if (Lista.Vars.Pago[i]["id"] == id) {

            return i;
        }
    }
    return -1;
}
function CalcularTotales() {
    var TotalValorVta = ImpTotalVta = Igv = TotalSub = Cantidad = exonerada = inafecta = grabada = gratuita = 0;
    var desc = 0;
    var totalfac = 0;


    if ($("#CalIGV").prop('checked')) {
        $.grep(Lista.Vars.Detalle, function (oDetalle) {
            if (oDetalle["operacion"] === 1) {
                TotalValorVta = oDetalle["Importe"];  //TOTAL
                TotalValorVta = TotalValorVta * 1
                grabada += TotalValorVta

                var SubTotalCal = TotalValorVta / (18 / 100 + 1)//Subtotal
                SubTotalCal = SubTotalCal * 1;
                ImpTotalVta += SubTotalCal
            } else if (oDetalle["operacion"] === 2) {//inafecta
                TotalValorVta = oDetalle["Importe"];  //TOTAL
                TotalValorVta = TotalValorVta * 1
                inafecta += TotalValorVta

            } else if (oDetalle["operacion"] === 3) {//exonerada
                TotalValorVta = oDetalle["Importe"];  //TOTAL
                TotalValorVta = TotalValorVta * 1;
                exonerada += TotalValorVta;
            }
            else if (oDetalle["operacion"] === 4) {//gratuita
                TotalValorVta = oDetalle["Importe"];  //TOTAL
                TotalValorVta = TotalValorVta * 1;
                gratuita += TotalValorVta;
            }
            totalfac = grabada + exonerada + inafecta;
            Cantidad += parseFloat(oDetalle["Cantidad"]);
            desc += parseFloat(oDetalle["descuento"] * parseFloat(oDetalle["Cantidad"]));

        });

        IgvSuma = grabada - ImpTotalVta;


        $('#Montodescuento').html(formatNumber(desc)); // descuento
        $('#MontoSubtotal').html(formatNumber(ImpTotalVta)); // sub total 
        $('#MontoCalulado').html(formatNumber(totalfac));//esto! // Total
        $('#MontoIGV').html(IgvSuma.toFixed(2)); // igv 
        $('#mCant').html(Cantidad.toFixed(2));
        $('#subtotales').val(ImpTotalVta);
        $('#igv').val(IgvSuma);
        $('#Totales').val(totalfac);

        $('#Montoinafecta').html(formatNumber(inafecta));
        $('#inafecta').val(inafecta);

        $('#Montoexonerada').html(formatNumber(exonerada));
        $('#exonerada').val(exonerada);

        $('#MontoGratuita').html(formatNumber(gratuita));
        $('#gratuita').val(gratuita);

    }
    else {

        $.grep(Lista.Vars.Detalle, function (oDetalle) {
            var subTotal = oDetalle["Importe"];//sub total
            subTotal = subTotal * 1;
            TotalSub += subTotal;


            Cantidad += parseFloat(oDetalle["Cantidad"]);
        });

        Igv = TotalSub * 0.18;
        Igv = Igv * 1;

        Total = TotalSub + Igv;

        $('#Montodescuento').html(formatNumber(desc)); // descuento
        $('#MontoSubtotal').html(TotalSub.toFixed(2)); // sub total
        $('#MontoCalulado').html(Total.toFixed(2));//esto! // Total
        $('#MontoIGV').html(Igv.toFixed(2)); // igv
        $('#mCant').html(Cantidad);

        $('#subtotales').val(TotalSub);
        $('#igv').val(Igv);
        $('#Totales').val(Total);
    }


}
function Limpiar() {
    $("#hdfId").val(0);
    $("#txtDocumento").val('');
    $("#txtRazon").val('');
    $("#txtTelefono").val('');
    $("#txtCelular").val('');
    $("#txtEmail").val('');
    $("#txtDirecion").val('');
    $("#lstDepartamento").val('0');
    $("#lstTipoDoc").val('0');
    $("#txtGiftCard").val('0');

    $("#lstProvincia").empty();
    $("#lstProvincia").append($('<option>', { value: 0, text: 'Seleccione' }));
    $("#lstDistrito").empty();
    $("#lstDistrito").append($('<option>', { value: 0, text: 'Seleccione' }));

    $('#cargarCliente').text(''); //limpiamos el label
}

function BuscarSunat() {
    var RUC = $("#txtDocumento").val();

    if ($("#lstTipoDocCliente").val() == 3) {
        if (isNaN(RUC) || RUC < 10000000000 || RUC > 99999999999) {
            General.Utils.ShowMessage(TypeMessage.Error, 'El RUC debe contener 11 dígitos');
        } else {
            $.ajax({
                type: 'post',
                url: General.Utils.ContextPath('Shared/SearchSunat'),
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                data: {
                    numeroRuc: RUC

                },
                success: function (response) {
                    //console.log(response)
                    if (response == null) {
                        General.Utils.ShowMessage("error", "Nro. de ruc invalido, ingrese los datos manualmente");
                    } else {


                        $("#txtRazon").val(response.RazonSocial);
                        $("#txtDireccion").val(response.Direccion);
                    }
                }
            });
        }
    } else if ($("#lstTipoDocCliente").val() == 1) {
        if (isNaN(RUC) || RUC.length == 8) {


            $.ajax({
                type: 'post',
                url: General.Utils.ContextPath('Shared/SearchSunat'),
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                data: {
                    numeroRuc: RUC

                },
                success: function (response) {
                    //console.log(response)
                    if (response == null) {
                        General.Utils.ShowMessage("error", "Nro. de ruc invalido, ingrese los datos manualmente");
                    } else {


                        $("#txtRazon").val(response.RazonSocial);
                        $("#txtDireccion").val(response.Direccion);
                    }
                }
            });

        } else {
            General.Utils.ShowMessage(TypeMessage.Error, 'El DNI debe contener 8 dígitos');
        }
    } else {
        General.Utils.ShowMessage(TypeMessage.Error, 'digite manualmente');
    }

}

function BuscarDetalleEnTabla(iIdProducto) {
    var bFound = false;
    $.each(Lista.Vars.Detalle, function (index, item) {
        if (item["Material"].IdMaterial == iIdProducto) {
            bFound = true;
            return false;
        }
    });
    return bFound;
}
var VerificarStock = function (Producto, Cantidad, almacen, callback) {

    $.ajax({
        async: true,
        type: 'post',
        url: General.Utils.ContextPath('Venta/ConsultaStock'),
        dataType: 'json',
        data: { idProducto: Producto, almacen: almacen, iCantidad: Cantidad },
        success: function (response) {
            callback(null, response)

        }
    });

}


$('#lstPago').on('change', function () {
    //alert(this.value);
    //limpiamos campos xsi queden seelcionados o con data
    $("#hdfMonto").text('');
    $("#estGiftcard").val('');



    //si es 5 es transferencia
    if (this.value == 5) {
        $("#divBanco").show();
        Lista.CargarBanco();
    } else {
        $("#divBanco").hide();
        $("#lstBanco").val(0)
    }

    //si es 6 es giftcard
    if (this.value == 6) {
        $("#divGiftCard").show();
        $("#hdfMonto").text('0.00');
    } else {
        $("#divGiftCard").hide();
        $("#txtGiftCard").val('0');
        $("#hdfMonto").text('');
    }


});

//validamos el chek envio
$('input[name=checkEnvio]').change(function () {
    if ($('input[name=checkEnvio]').is(':checked')) {
        $("#divEnvio").show();
        $("#divCostoEnvio").show();
        $("#divFechaEnvio").show();

        Lista.CargarTipoEnvio();

    } else {
        $("#divEnvio").hide();
        $("#divCostoEnvio").hide();
        $("#divFechaEnvio").hide();

        $("#lstTipoEnvio").val(0);
        $('#txtCostoEnvio').val('');
        $('#fechaEnvio').val('');


    }
});

// Costo de envío 
$('#lstTipoEnvio').change(function () {

    var data = $('#lstTipoEnvio option:selected').html();
    var cort = data.split('-')[1];

    $('#txtCostoEnvio').val(cort);
});