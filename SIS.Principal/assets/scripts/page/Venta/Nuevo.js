
var Lista = {
    //CargarSerieDoc: function () {

    //    $.ajax({
    //        async: true,
    //        type: 'post',
    //        url: General.Utils.ContextPath("venta/SerieNumero"),
    //        dataType: 'json',
    //        data: { documento: 1 },
    //        success: function (response) {

    //            $("#txtSerie").val(response.Text)
    //            $("#txtNumero").val(response.Nombre)
    //        }
    //    });
    //},
    CargarCombo: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaCombo"),
            dataType: 'json',
            data: { Id: 6 },
            success: function (response) {
                //Para venta

                $("#lstTipoDoc").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 

                    $.grep(response, function (oDocumento) {

                        $('select[name="lstTipoDoc"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
                    });
                }

                //Para modal
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
    CargarTipoPago: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaCombo"),
            dataType: 'json',
            data: { Id: 9 },
            success: function (response) {

                $("#lstTipoPago").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 

                    $.grep(response, function (oDocumento) {

                        $('select[name="lstTipoPago"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
                    });

                    $("#lstTipoPago").val(3)
                }
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
    CargarTipoDoc: function () {
        var sucursal = $("#sucursal").val();
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaComboId"),
            dataType: 'json',
            data: { flag: 9, Id: sucursal },
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
    CargarVendedor: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaCombo"),
            dataType: 'json',
            data: { Id: 28 },
            success: function (response) {

                $("#lstVendedor").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 

                    $.grep(response, function (oDocumento) {

                        $('select[name="lstVendedor"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
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
    //Lista.CargarSerieDoc();
    Lista.CargarCombo();
    Lista.CargarTipoPago();
    Lista.CargarMoneda();
    Lista.CargarTipoDoc();
    Lista.CargarAlmacen();
    Lista.CargarVendedor();

    Lista.CargarPago(); //cargar pago modal cobrar
    Lista.CargarDepartamento();

    $('input[name="descuentoPor"]').Validate({ type: TypeValidation.Numeric, special: '.' });
    $('input[name="descuento"]').Validate({ type: TypeValidation.Numeric, special: '.' });
    $('input[name="txtPrecio"]').Validate({ type: TypeValidation.Numeric, special: '.' });
    $('input[name="txtCantidad"]').Validate({ type: TypeValidation.Numeric, special: '.' });


    //Para que los select funcionen en el modal
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



    $('#dFechaAten').datepicker({
        dateFormat: 'dd/mm/yy',
        prevText: '<i class="fa fa-angle-left"></i>',
        nextText: '<i class="fa fa-angle-right"></i>'
    });

    $('#lstPago').select2({
        dropdownParent: $('#ModalCobrar')
    });


    $("#dFechaAten").datepicker().datepicker("setDate", new Date());

    $('#dFechaPago').datepicker({
        dateFormat: 'dd/mm/yy',
    });
    $("#dFechaPago").datepicker().datepicker("setDate", new Date());


    $("#lstMoneda").change(function () {
        $("#hMonedaComprobante").html($("#lstMoneda option:selected").text());
    });


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


    $("#lstTipoPago").change(function () {
        var TipoPago = parseInt($("#lstTipoPago").val())

        var fecha = $('#dFechaPago').val().split(/\//);
        fecha = [fecha[1], fecha[0], fecha[2]].join('/');


        switch (TipoPago) {
            case 1:
                var TuFecha = new Date();
                //dias a sumar
                var dias = parseInt(15);
                //nueva fecha sumada
                TuFecha.setDate(TuFecha.getDate() + dias);
                //formato de salida para la fecha
                var FechaGold = ((TuFecha.getDate() < 10 ? '0' + TuFecha.getDate() : TuFecha.getDate())) + '/' +
                    ((TuFecha.getMonth() + 1) < 10 ? '0' + (TuFecha.getMonth() + 1) : (TuFecha.getMonth() + 1)) + '/' + TuFecha.getFullYear();

                $("#dFechaPago").val(FechaGold)

                break;
            case 2:
                var TuFecha = new Date();
                //dias a sumar
                var dias = parseInt(30);
                //nueva fecha sumada
                TuFecha.setDate(TuFecha.getDate() + dias);
                //formato de salida para la fecha
                var FechaGold = ((TuFecha.getDate() < 10 ? '0' + TuFecha.getDate() : TuFecha.getDate())) + '/' +
                    ((TuFecha.getMonth() + 1) < 10 ? '0' + (TuFecha.getMonth() + 1) : (TuFecha.getMonth() + 1)) + '/' + TuFecha.getFullYear();

                $("#dFechaPago").val(FechaGold)
                break;
            case 3:


                $("#dFechaPago").datepicker().datepicker("setDate", new Date());
                break;
            case 4:
                var TuFecha = new Date();
                //dias a sumar
                var dias = parseInt(120);
                //nueva fecha sumada
                TuFecha.setDate(TuFecha.getDate() + dias);
                //formato de salida para la fecha
                var FechaGold = ((TuFecha.getDate() < 10 ? '0' + TuFecha.getDate() : TuFecha.getDate())) + '/' +
                    ((TuFecha.getMonth() + 1) < 10 ? '0' + (TuFecha.getMonth() + 1) : (TuFecha.getMonth() + 1)) + '/' + TuFecha.getFullYear();

                $("#dFechaPago").val(FechaGold)
                break;
            case 5:
                var TuFecha = new Date();
                //dias a sumar
                var dias = parseInt(60);
                //nueva fecha sumada
                TuFecha.setDate(TuFecha.getDate() + dias);
                //formato de salida para la fecha
                var FechaGold = ((TuFecha.getDate() < 10 ? '0' + TuFecha.getDate() : TuFecha.getDate())) + '/' +
                    ((TuFecha.getMonth() + 1) < 10 ? '0' + (TuFecha.getMonth() + 1) : (TuFecha.getMonth() + 1)) + '/' + TuFecha.getFullYear();

                $("#dFechaPago").val(FechaGold)
                break;
            case 6:
                var TuFecha = new Date();
                //dias a sumar
                var dias = parseInt(90);
                //nueva fecha sumada
                TuFecha.setDate(TuFecha.getDate() + dias);
                //formato de salida para la fecha
                var FechaGold = ((TuFecha.getDate() < 10 ? '0' + TuFecha.getDate() : TuFecha.getDate())) + '/' +
                    ((TuFecha.getMonth() + 1) < 10 ? '0' + (TuFecha.getMonth() + 1) : (TuFecha.getMonth() + 1)) + '/' + TuFecha.getFullYear();

                $("#dFechaPago").val(FechaGold)
                break;
            case 7:
                var TuFecha = new Date();
                //dias a sumar
                var dias = parseInt(45);
                //nueva fecha sumada
                TuFecha.setDate(TuFecha.getDate() + dias);
                //formato de salida para la fecha
                var FechaGold = ((TuFecha.getDate() < 10 ? '0' + TuFecha.getDate() : TuFecha.getDate())) + '/' +
                    ((TuFecha.getMonth() + 1) < 10 ? '0' + (TuFecha.getMonth() + 1) : (TuFecha.getMonth() + 1)) + '/' + TuFecha.getFullYear();

                $("#dFechaPago").val(FechaGold)
                break;
            case 8:
                var TuFecha = new Date();
                //dias a sumar
                var dias = parseInt(7);
                //nueva fecha sumada
                TuFecha.setDate(TuFecha.getDate() + dias);
                //formato de salida para la fecha
                var FechaGold = ((TuFecha.getDate() < 10 ? '0' + TuFecha.getDate() : TuFecha.getDate())) + '/' +
                    ((TuFecha.getMonth() + 1) < 10 ? '0' + (TuFecha.getMonth() + 1) : (TuFecha.getMonth() + 1)) + '/' + TuFecha.getFullYear();

                $("#dFechaPago").val(FechaGold)
                break;
            case 9:
                var TuFecha = new Date();
                //dias a sumar
                var dias = parseInt(30);
                //nueva fecha sumada
                TuFecha.setDate(TuFecha.getDate() + dias);
                //formato de salida para la fecha
                var FechaGold = ((TuFecha.getDate() < 10 ? '0' + TuFecha.getDate() : TuFecha.getDate())) + '/' +
                    ((TuFecha.getMonth() + 1) < 10 ? '0' + (TuFecha.getMonth() + 1) : (TuFecha.getMonth() + 1)) + '/' + TuFecha.getFullYear();

                $("#dFechaPago").val(FechaGold)
                break;
            case 10:
                var TuFecha = new Date();
                //dias a sumar
                var dias = parseInt(15);
                //nueva fecha sumada
                TuFecha.setDate(TuFecha.getDate() + dias);
                //formato de salida para la fecha
                var FechaGold = ((TuFecha.getDate() < 10 ? '0' + TuFecha.getDate() : TuFecha.getDate())) + '/' +
                    ((TuFecha.getMonth() + 1) < 10 ? '0' + (TuFecha.getMonth() + 1) : (TuFecha.getMonth() + 1)) + '/' + TuFecha.getFullYear();

                $("#dFechaPago").val(FechaGold)
                break;
            default:
            // code block
        }
    });

    $("#txtDocCli").autocomplete({
        source: function (request, response) {
            if ($("#lstTipoDoc").val() == 0) {
                General.Utils.ShowMessage(TypeMessage.Warning, 'Seleccione Documento');
            } else {
                var vRuc = $("#txtDocCli").val();
                var Tipodoc = $("#lstTipoDoc").val();
                $.ajax({
                    url: General.Utils.ContextPath('Shared/FiltroProvCli'),
                    type: "POST",
                    dataType: "json",
                    data: { filtro: vRuc, Tipo: Tipodoc, flag: 2 },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                id: item.Id,
                                cod: item.Text,
                                label: item.Text + ' - ' + item.Nombre,
                                des: item.Nombre + '|' + item.Dir + '|' + item.Linea
                            };
                        }));
                    }
                });
            }
        },
        minLength: 2,
        select: function (event, ui) {
            $('#hdfId').val(ui.item.id);
            $("#txtDocCli").val(ui.item ? ui.item.cod : $("#txtDocCli").val());
            $('#hdfId').val(ui.item.id);
            console.log($('#hdfId').val());

        },
        change: function (event, ui) {
            $("#txtDocCli").val(ui.item ? ui.item.cod : jQuery("#txtDocCli").val()); 
            $("#txtNomCli").val(ui.item ? ui.item.des.toString().split('|')[0] : '');
            $("#txtDireccionInicio").val(ui.item ? ui.item.des.toString().split('|')[1] : '');
            $("#lstVendedor").val(ui.item.des.toString().split('|')[2])
        }
    });

    //material
    $("#txtProducto").autocomplete({
        source: function (request, response) {

            var filtro = $("#txtProducto").val();
            $.ajax({
                url: General.Utils.ContextPath('Shared/FiltroProducto'),
                type: "POST",
                dataType: "json",
                data: { filtro: filtro }, 
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            id: item.IdMaterial,
                            cod: item.Nombre,
                            label: item.Nombre + ' - ' + item.Codigo,
                            des: item.Nombre + '|' + item.Codigo + '|' + item.PrecioCompra + '|' + item.PrecioVenta + '|' + item.Descuento,
                            cat: item.Categoria.Nombre,
                            mar: item.Marca.Nombre,
                            und: item.Unidad.Nombre,
                            ope: item.TipoOperacion.IdTipoOperacion + '|' + item.TipoOperacion.Nombre
                        };
                    }));
                }
            });

        },
        minLength: 2,
        select: function (event, ui) {
            $('#IdProducto').val(ui.item.id);
            $("#txtProducto").val(ui.item ? ui.item.cod : $("#txtProducto").val());
            $('#IdProducto').val(ui.item.id);


        },
        change: function (event, ui) {
            $("#txtProducto").val(ui.item ? ui.item.cod : jQuery("#txtProducto").val());
            $("#sCodigo").val(ui.item ? ui.item.des.toString().split('|')[1] : '');
            $("#txtPrecio").val(ui.item ? ui.item.des.toString().split('|')[3] : '');
            $("#sCategoria").val(ui.item ? ui.item.cat.toString() : '');
            $("#sMarca").val(ui.item ? ui.item.mar.toString() : '');
            $("#sUnidad").val(ui.item ? ui.item.und.toString() : '');
            $("#txtCantidad").val('');
            $("#txtTotal").val('');
            $("#txtDescuentoPor").val('');
            $("#txtDescuento").val(parseFloat(ui.item.des.toString().split('|')[4]));
            $("#txtCantidad").focus();
            $("#ITipoOperacion").val(parseInt(ui.item.ope.toString().split('|')[0]));
            $("#sDescTipoOperacion").val(ui.item.ope.toString().split('|')[1]);

        }
    });

    //Material
    var table = ""
    $("#btnbuscar").click(function () {

        table = $("#tbMaterial").DataTable({
            select: true,
            pageLength: 10,
            processing: true,
            serverSide: true,
            filter: true,
            bSort: true,
            orderMulti: false,
            destroy: true,
            language: {
                "lengthMenu": "Mostrar _MENU_ registros por p&aacute;gina",
                "zeroRecords": "No se encontraron datos.",
                "info": "Mostrando la p&aacute;gina _PAGE_ de _PAGES_",
                "infoEmpty": "No hay registros disponibles",
                "infoFiltered": "(filtrando _MAX_ total de registros)",
                "search": "Buscar:",
                "paging": "false",

                "paginate": {
                    "first": "Primero",
                    "previous": "Anterior",
                    "next": "Siguiente",
                    "last": "&Uacute;timo"
                },
            },
            dom: 'Bfrtip',
            ajax: {
                url: General.Utils.ContextPath('Mantenimiento/ListadoMaterial'),
                type: "POST",
                datatype: "json",
                data: function (d) {

                }
            },

            columns: [
                { "data": "IdMaterial", "name": "IdMaterial" },
                { "data": "Codigo", "name": "Codigo" },
                { "data": "Nombre", "name": "Nombre" },
                { "data": "Unidad.Nombre", "name": "Unidad" },
                { "data": "Marca.Nombre", "name": "Marca" },
                { "data": "Modelo.Nombre", "name": "Modelo" },

            ],
            buttons: [],
            'columnDefs': [
                {
                    'targets': 0,
                    'checkboxes': {
                        'selectRow': true
                    }
                }
            ],
            'select': {

                'style': 'multi'
            },
            'fnCreatedRow': function (nRow, aData, iDataIndex) {
                $(nRow).attr('data-id', aData.IdMaterial); // or whatever you choose to set as the id
                $(nRow).attr('id', 'id_' + aData.IdMaterial); // or whatever you choose to set as the id
            },
        });
    });


    $("#lstDocumento").change(function () {
        changeDoc($("#lstDocumento").val())
    })

    $('#tbMaterial tbody').on('click', 'tr', function () {
        $(this).toggleClass('selected');


    });
    ///
    $("#txtCantidad").on('change', function () {
        var descuento = $("#txtDescuento").val();
        var descuentoPorcentaje = $("#txtDescuentoPor").val();
        let Precio = $('#txtPrecio').val();
        let Cantidad = $('#txtCantidad').val();
        let Total = $('#txtTotal').val();
        var desUnidad = 0;

        descuentoPorcentaje = (descuento * 100) / (Precio * Cantidad);
        desUnidad = Precio - descuento;
        //Total = Total * 1
        //Total = Precio * Cantidad
        Total = desUnidad * Cantidad;

        $("#txtDescuento").val(descuento);
        $("#txtTotal").val(Total.toFixed(2));
        $("#txtDescuentoPor").val(descuentoPorcentaje);
    });

    $("#txtPrecio").on('change', function () {
        let Precio = $('#txtPrecio').val();
        let Cantidad = $('#txtCantidad').val();
        let Total = $('#txtTotal').val();

        Total = Precio * Cantidad;

        $('#txtTotal').val(Total.toFixed(2));
        $("#txtreal").val(Precio);
        $("#hdfprecio").val(Precio);
        $("#txtDescuentoPor").val(0.00);
    });

    $('#txtDescuentoPor').on('change', function () {
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

    });

    $('#txtDescuento').on('change', function () {
        var descuento = $("#txtDescuento").val();
        var descuentoPorcentaje = $("#txtDescuentoPor").val();
        var precio = $("#txtPrecio").val();
        var cantidad = $("#txtCantidad").val();

        var resta = 0;
        var Total = 0;
        var descuentoMonto = 0;

        var desUnidad = 0;

        descuentoPorcentaje = (descuento * 100) / (precio * cantidad);

        descuentoMonto = (precio * descuentoPorcentaje) / 100;

        desUnidad = precio - descuentoMonto;

        // = Total * 1
        Total = desUnidad * cantidad;
        // resta = Total - descuento;

        //alert(Total);   
        //alert(descuento);
        $("#hdfprecio").val(desUnidad.toFixed(2));
        $("#txtDescuentoPor").val(descuentoPorcentaje.toFixed(2));
        $("#txtTotal").val(Total.toFixed(2));
    });

    ///agregar del modal productos
    $("#btnAceptar").click(function () {
        var form = this;

        var rows_selected = table.column(0).checkboxes.selected();

        // Iterar sobre todas las casillas seleccionadas
        $.each(rows_selected, function (index, rowId) {
            // Create a hidden element 
            $(form).append(
                $('<input>')
                    .attr('type', 'hidden')
                    .attr('name', 'id[]')
                    .val(rowId)
            );
            //console.log(rowId);

            VerificarStock(rowId, 1, $("#lstAlmacen").val(), function (errorLanzado, datosDevueltos) {

                if (errorLanzado) // Ha habido un error, deberías manejarlo :/
                {
                    General.Utils.ShowMessage(TypeMessage.Error, 'Algo salio mal verificar');
                    return;
                }
                if (datosDevueltos.Id == 'error') {
                    General.Utils.ShowMessage(datosDevueltos.Id, datosDevueltos.Message);
                } else {
                    Obtener(rowId)
                }
            })

        });


        setTimeout(function () { Lista.CargarOperacion(); }, 1000);
        // Eliminar elementos añadidos
        $('input[name="id\[\]"]', form).remove();

    });

    $("#lstAlmacen").change(function () {
        Lista.Vars.Detalle.map(function (data) {
            data.Almacen.IdAlmacen = $("#lstAlmacen").val()
        });
    });

    $("#btnAgregar").click(function () {

        var iIdProducto = parseInt($("#IdProducto").val());
        var producto = $("#txtProducto").val()
        if ($("#txtProducto").val() == "") {
            General.Utils.ShowMessage(TypeMessage.Error, 'No se a cargado el producto');
        } else if ($("#txtCantidad").val() == "") {
            General.Utils.ShowMessage(TypeMessage.Error, 'Debe ingresar la cantidad');
        } else if (BuscarDetalleEnTabla(iIdProducto)) {
            General.Utils.ShowMessage(TypeMessage.Error, `El producto  ${producto} ya existe en la tabla`);
        } else {

            Lista.Vars.Detalle.push({
                Material: {
                    IdMaterial: $("#IdProducto").val(),
                    Codigo: $("#sCodigo").val(),
                    Nombre: $("#txtProducto").val(),
                },
                Categoria: $("#sCategoria").val(),
                Marca: $("#sMarca").val(),
                Unidad: $("#sUnidad").val(),
                Cantidad: parseFloat($("#txtCantidad").val()),
                Precio: $("#txtPrecio").val(),
                Importe: $("#txtTotal").val(),
                descuentopor: $("#txtDescuentoPor").val(),
                descuento: $("#txtDescuento").val(),
                // operacion: 1,
                operacion: parseInt($("#ITipoOperacion").val()),
                DesOperacion: $("#sDescTipoOperacion").val(),

                Almacen: { IdAlmacen: $("#lstAlmacen").val() },
                Sucursal: { IdSucursal: $("#lstSucursalCab").val() }
            });

            console.log(Lista.Vars.Detalle)
            Lista.CargarOperacion();


            $("#IdProducto").val('0')
            $("#txtProducto").val('')
            $("#sCodigo").val('')
            $("#sCategoria").val('')
            $("#sMarca").val('')
            $("#sUnidad").val('')
            $("#txtCantidad").val('0.00')
            $("#txtPrecio").val('0.00')
            $("#txtDescuentoPor").val('0.00')
            $("#txtDescuento").val('0.00')
            $("#txtTotal").val('0.00')
            $("#ITipoOperacion").val('0')
            $("#sDescTipoOperacion").val('')


            var $tb = $('#tbDetalle');
            $tb.find('tbody').empty();

            if (Lista.Vars.Detalle.length == 0) {
                $tb.find('tbody').append('<tr><td colspan="15">No existen registros</td></tr>')
            } else {

                $.grep(Lista.Vars.Detalle, function (oDetalle) {
                    $tb.find('tbody').append(
                        '<tr data-index=' + oDetalle["Material"]["IdMaterial"] + '>' +
                        '<td>' + oDetalle["Material"]["Codigo"] + '</td>' +
                        '<td>' + oDetalle["Material"]["Nombre"] + '</td>' +
                        '<td>' + oDetalle["Unidad"] + '</td>' +
                        '<td>' + oDetalle["DesOperacion"] + '</td>' +
                        // '<td>' + '<select id="lstOperacion" name="lstOperacion" class="form-control select"></select>' + '</td>' +
                        //'<td>' + '<input type="text" class="form-control por" id="descuentoPor" value="' + oDetalle["descuentopor"] + '">' + '</td>' +
                        '<td>' + '<input type="text" class="form-control form-control-sm desc" id="descuento"   value="' + oDetalle["descuento"] + '">' + '</td>' +
                        '<td>' + '<input type="text" class="form-control form-control-sm Cant" id="Cantidad"  value="' + formatNumber(oDetalle["Cantidad"]) + '">' + '</td>' +
                        '<td>' + '<input type="text" class="form-control form-control-sm Price" id="Precio"  value="' + formatNumber(oDetalle["Precio"]) + '">' + '</td>' +
                        '<td>' + formatNumber(oDetalle["Importe"]) + '</td>' +
                        '<td class="text-center">' +
                        '<button type="submit" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>' +
                        '</td>' +
                        '</tr>'

                    );
                });
            }


        }
        $("#lstOperacion option[value='1']").attr("selected", true)

        CalcularTotales();

    });

    //ACtulizar cantidad
    $('#tbDetalle').on('change', '.Cant', function () {
        var row = $(this).closest('tr');
        var Id = $(this).closest('tr').attr('data-index');
        //var Cantidad = 0;
        var input = Number($(this).val());;
        //console.log($(this).val())
        //$('input', row).each(function () {
        //    input = 
        //});
        Lista.Vars.Detalle.map(function (data) {
            if (parseInt(data.Material.IdMaterial) == parseInt(Id)) {
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
                    '<td>' + oDetalle["Material"]["Codigo"] + '</td>' +
                    '<td>' + oDetalle["Material"]["Nombre"] + '</td>' +
                    '<td>' + oDetalle["Unidad"] + '</td>' +
                    '<td>' + oDetalle["DesOperacion"] + '</td>' +
                    //'<td>' + '<select id="lstOperacion" name="lstOperacion" class="form-control select"></select>' + '</td>' +
                    //'<td>' + '<input type="text" class="form-control por" id="descuentoPor" value="' + oDetalle["descuentopor"] + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm desc" id="descuento"   value="' + oDetalle["descuento"] + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Cant" id="Cantidad"  value="' + oDetalle["Cantidad"] + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Price" id="Precio"  value="' + oDetalle["Precio"] + '" disabled>' + '</td>' +
                    '<td>' + (oDetalle["Importe"] * 1).toFixed(2) + '</td>' +
                    '<td class="text-center">' +
                    '<button type="submit" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>' +
                    '</td>' +
                    '</tr>'
                );
            });
        }
    });

    //Actualizar Precio
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
        });

        CalcularTotales();

        Lista.CargarOperacion();

        var $tb = $('#tbDetalle');
        $tb.find('tbody').empty();
        if (Lista.Vars.Detalle.length == 0) {
            $tb.find('tbody').append('<tr><td colspan="15">No existen registros</td></tr>')
        } else {

            $.grep(Lista.Vars.Detalle, function (oDetalle) {
                $tb.find('tbody').append(
                    '<tr data-index=' + oDetalle["Material"]["IdMaterial"] + '>' +
                    '<td>' + oDetalle["Material"]["Codigo"] + '</td>' +
                    '<td>' + oDetalle["Material"]["Nombre"] + '</td>' +
                    '<td>' + oDetalle["Unidad"] + '</td>' +
                    '<td>' + oDetalle["DesOperacion"] + '</td>' +
                    //'<td>' + '<select id="lstOperacion" name="lstOperacion" class="form-control select"></select>' + '</td>' +
                    //'<td>' + '<input type="text" class="form-control por" id="descuentoPor" value="' + oDetalle["descuentopor"] + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm desc" id="descuento"   value="' + oDetalle["descuento"] + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Cant" id="Cantidad"  value="' + oDetalle["Cantidad"] + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Price" id="Precio"  value="' + oDetalle["Precio"] + '" disabled>' + '</td>' +
                    '<td>' + (oDetalle["Importe"] * 1).toFixed(2) + '</td>' +
                    '<td class="text-center">' +
                    '<button type="submit" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>' +
                    '</td>' +
                    '</tr>'

                );
            });
        }
    });
    //Actulizar descuento %
    //$('#tbDetalle').on('change', '.por', function () {
    //    var row = $(this).closest('tr');
    //    var Id = $(this).closest('tr').attr('data-index');
    //    ///
    //    var descuento = $("#txtDescuento").val();
    //    var descuentoPorcentaje = $("#txtDescuentoPor").val();
    //    var precio = $("#txtPrecio").val();
    //    var cantidad = $("#txtCantidad").val();
    //    var Total = $("#hdf_TotalImporte").val();

    //    var resta = 0;
    //    var diferenciaDescuento = 0;
    //    var desUnidad = 0;

    //    diferenciaDescuento = (precio * descuentoPorcentaje) / 100;

    //    desUnidad = precio - diferenciaDescuento
    //    Total = Total * 1
    //    Total = desUnidad * cantidad;
    //    descuento = (precio * cantidad) - Total;


    //    $("#txtDescuento").val(descuento.toFixed(2));
    //    $("#hdfprecio").val(desUnidad.toFixed(2));
    //    $("#txtTotal").val(Total.toFixed(2));


    //    ////

    //    var diferenciaDescuento = 0;
    //    var desUnidad = 0;




    //    var input = Number($(this).val());
    //    Lista.Vars.Detalle.map(function (data) {
    //        if (parseInt(data.Material.IdMaterial) == parseInt(Id)) {

    //            diferenciaDescuento = (data.Precio * input) / 100;
    //            desUnidad = data.Precio - diferenciaDescuento

    //            data.Importe = (desUnidad * data.Cantidad);
    //            data.descuento = (data.Precio * data.Cantidad) - data.Importe;
    //            data.descuentopor = input;

    //        }
    //    })
    //    Lista.CargarOperacion();
    //    CalcularTotales();
    //    var $tb = $('#tbDetalle');
    //    $tb.find('tbody').empty();
    //    if (Lista.Vars.Detalle.length == 0) {
    //        $tb.find('tbody').append('<tr><td colspan="15">No existen registros</td></tr>')
    //    } else {

    //        $.grep(Lista.Vars.Detalle, function (oDetalle) {
    //            $tb.find('tbody').append(
    //                '<tr data-index=' + oDetalle["Material"]["IdMaterial"] + '>' +
    //                '<td>' + oDetalle["Material"]["Codigo"] + '</td>' +
    //                '<td>' + oDetalle["Material"]["Nombre"] + '</td>' +
    //                '<td>' + oDetalle["Unidad"] + '</td>' +
    //                '<td>' + '<select id="lstOperacion" name="lstOperacion" class="form-control select"></select>' + '</td>' +
    //                //'<td>' + '<input type="text" class="form-control por" id="descuentoPor" value="' + oDetalle["descuentopor"] + '">' + '</td>' +
    //                '<td>' + '<input type="text" class="form-control desc" id="descuento"   value="' + oDetalle["descuento"] + '">' + '</td>' +
    //                '<td>' + '<input type="text" class="form-control Cant" id="Cantidad"  value="' + oDetalle["Cantidad"] + '">' + '</td>' +
    //                '<td>' + '<input type="text" class="form-control Price" id="Precio"  value="' + oDetalle["Precio"] + '">' + '</td>' +
    //                '<td>' + (oDetalle["Importe"] * 1).toFixed(2) + '</td>' +
    //                '<td class="text-center">' +
    //                '<button type="submit" class="btn btn-danger btn-xs"><i class="fa fa-trash"></i></button>' +
    //                '</td>' +
    //                '</tr>'

    //            );
    //        });
    //    }
    //});

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
        });

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
                    '<td>' + oDetalle["Material"]["Codigo"] + '</td>' +
                    '<td>' + oDetalle["Material"]["Nombre"] + '</td>' +
                    '<td>' + oDetalle["Unidad"] + '</td>' +
                    '<td>' + oDetalle["DesOperacion"] + '</td>' +
                    //'<td>' + '<select id="lstOperacion" name="lstOperacion" class="form-control select"></select>' + '</td>' +
                    //'<td>' + '<input type="text" class="form-control por" id="descuentoPor" value="' + oDetalle["descuentopor"] + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm desc" id="descuento"   value="' + oDetalle["descuento"] + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Cant" id="Cantidad"  value="' + oDetalle["Cantidad"] + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control form-control-sm Price" id="Precio"  value="' + oDetalle["Precio"] + '" disabled>' + '</td>' +
                    '<td>' + (oDetalle["Importe"] * 1).toFixed(2) + '</td>' +
                    '<td class="text-center">' +
                    '<button type="submit" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>' +
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
        console.log(input)
        Lista.Vars.Detalle.map(function (data) {
            if (parseInt(data.Material.IdMaterial) == parseInt(Id)) {
                data.operacion = input
            }
        })
        CalcularTotales();
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
                    '<td>' + oDetalle["Material"]["Codigo"] + '</td>' +
                    '<td>' + oDetalle["Material"]["Nombre"] + '</td>' +
                    '<td>' + oDetalle["Unidad"] + '</td>' +
                    '<td>' + oDetalle["DesOperacion"] + '</td>' +
                    //'<td>' + '<select id="lstOperacion" name="lstOperacion" class="form-control select"></select>' + '</td>' +
                    //'<td>' + '<input type="text" class="form-control por" id="descuentoPor" value="' + oDetalle["descuentopor"] + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control desc" id="descuento"   value="' + oDetalle["descuento"] + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control Cant" id="Cantidad"  value="' + oDetalle["Cantidad"] + '">' + '</td>' +
                    '<td>' + '<input type="text" class="form-control Price" id="Precio"  value="' + oDetalle["Precio"] + '" disabled>' + '</td>' +
                    '<td>' + (oDetalle["Importe"] * 1).toFixed(2) + '</td>' +
                    '<td class="text-center">' +
                    '<button type="submit" class="btn btn-danger btn-xs"><i class="fa fa-trash"></i></button>' +
                    '</td>' +
                    '</tr>'
                );
            })
        }
        Lista.CargarOperacion();
        CalcularTotales();

    });

    //guardar

    $("#btnProcesar").click(function () {

        numero = 0
       // $("#txtrecibida").val('0.00');
        $("#txtCambio").val('0.00');
        $("#txtCostoEnvio").val('0.00');

        var total = parseFloat($("#Totales").val());
        $("#txtPago").val(total.toFixed(2));
        $("#txtrecibida").val(total.toFixed(2));

        //$("#txtPago").val($("#Totales").val());

        $("#lstPago").val(1);


    });

    //datos del modal procesar venta
    var numero = 0;
    $("#btn10").click(function () {

        numero += 10;
        var total = $("#Totales").val();
        $("#txtrecibida").val(formatNumber(numero))
        var mostrarPago = numero - total
        $("#txtCambio").val(formatNumber(mostrarPago))

    })
    $("#btn20").click(function () {

        numero += 20;
        var total = $("#Totales").val();
        $("#txtrecibida").val(formatNumber(numero))
        var mostrarPago = numero - total
        $("#txtCambio").val(formatNumber(mostrarPago))
    })
    $("#btn50").click(function () {

        numero += 50;
        var total = $("#Totales").val();
        $("#txtrecibida").val(formatNumber(numero))
        var mostrarPago = numero - total
        $("#txtCambio").val(formatNumber(mostrarPago))
    })
    $("#btn100").click(function () {
        numero += 100;
        var total = $("#Totales").val();
        $("#txtrecibida").val(formatNumber(numero))
        var mostrarPago = numero - total
        $("#txtCambio").val(formatNumber(mostrarPago))
    })

    $("#btnLimpiar").click(function () {
        numero = 0
        $("#txtrecibida").val('0.00')
        $("#txtCambio").val('0.00')
    })

    $("#txtrecibida").change(function () {
        numero = parseFloat($("#txtrecibida").val())
        var total = $("#Totales").val();
        var mostrarPago = numero - total
        $("#txtCambio").val(mostrarPago)

    });
    // fin de los datos del modal procesar venta

    $("#btnGuardar").click(function () {
        if ($("#hdfId").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese el cliente');
        } else if (Lista.Vars.Detalle.length <= 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'No se ha ingresado el detalle');
        } else if ($("#lstDocumento").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'El tipo de documento no está seleccionado');
        } else if ($("#lstAlmacen").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'El almacén no está seleccionado');
        } else if ($("#lstTipoPago").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese la forma de pago');
        }
        else if ($("#lstMoneda").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'La moneda no está seleccionada');
        } else if ($("#txtrecibida").val() == '0.00') {
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese monto a cobrar');
        } else if ($("#lstVendedor").val() == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'selecione el Vendedor');
        }
        else {
            var oDatos = {
                cliente: { IdCliente: $("#hdfId").val() },
                Comprobante: { Id: $("#lstDocumento").val() },
                moneda: { IdMoneda: $("#lstMoneda").val() },
                Documento: { IdDocumento: $("#lstTipoPago").val() },
                fechaEmision: $("#dFechaAten").val(),
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


                montoRecibido: $("#txtrecibida").val(),
                vuelto: $("#txtCambio").val(),
                metodoPago: $("#lstPago").val(),
                NotaPago: $("#txtNotaPago").val(),
                CostoEnvio: $("#txtCostoEnvio").val(),
                IdVendedor: $("#lstVendedor").val(),
            }
            $.ajax({
                async: true,
                type: 'post',
                url: General.Utils.ContextPath('Venta/InstRegistrarVenta'),
                dataType: 'json',
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                data: { oDatos: oDatos, Detalle: Lista.Vars.Detalle },
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
                                var URL = General.Utils.ContextPath('Venta/imprimirFacBol?Id=' + response.Additionals[0] + "&Envio=" + 0 + "&venta=" + 1);
                                fileDownnload(URL);
                            }
                        })
                        LimpiarConteniedo();
                    }
                    else {
                        Swal.fire(
                            'Alerta!',
                            response.Message,
                            response.Id
                        )
                    }
                }
            });
        }

    });



    $("#limpiar").click(function () {

        LimpiarConteniedo();
    })
    $("#btnPrecio").click(function () {
        var Producto = $("#IdProducto").val();
        var Cliente = $("#hdfId").val(); 
        if (Producto == '') {
            General.Utils.ShowMessage(TypeMessage.Error, 'Seleccione Producto');
        } else {
            ListaPrecio(Producto, Cliente);
           
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
function CalcularTotales() {
    var TotalValorVta = ImpTotalVta = Igv = TotalSub = Cantidad = exonerada = inafecta = grabada = gratuita = 0;
    var totalfac = 0;
    var desc = 0;

    if ($("#CalIGV").prop('checked')) {
        $.grep(Lista.Vars.Detalle, function (oDetalle) {
            console.log(oDetalle)

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

            } else if (oDetalle["operacion"] === 4) {//gratuita
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

        $('#MontoSubtotal').html(TotalSub.toFixed(2)); // sub total
        $('#MontoCalulado').html(Total.toFixed(2));//esto! // Total
        $('#MontoIGV').html(Igv.toFixed(2)); // igv
        $('#mCant').html(Cantidad);

        $('#subtotales').val(TotalSub);
        $('#igv').val(Igv);
        $('#Totales').val(Total);
    }


}
function mostrarMensaje() {

    if ($("#CalIGV").prop('checked')) {
        General.Utils.ShowMessage(TypeMessage.Warning, 'Los precios incluyen IGV');

        CalcularTotales();

    } else {
        General.Utils.ShowMessage(TypeMessage.Warning, 'Los precios no incluyen IGV');

        CalcularTotales();
    }
}
var Obtener = function (Id) {
    var senData = {
        Id: Id
    }
    $.ajax({
        async: true,
        type: 'post',
        url: General.Utils.ContextPath('Mantenimiento/ListaEditMaterial'),
        dataType: 'json',
        data: senData,
        success: function (response) {
            //console.log(response);

            if (BuscarDetalleEnTabla(response.IdMaterial)) {
                General.Utils.ShowMessage(TypeMessage.Error, `El producto  ${response.Nombre} ya existe en la tabla`);
            } else {

                Lista.Vars.Detalle.push({

                    Material: {
                        IdMaterial: response.IdMaterial,
                        Codigo: response.Codigo,
                        Nombre: response.Nombre,
                    },
                    Categoria: response.Categoria.Nombre,
                    Marca: response.Marca.Nombre,
                    Unidad: response.Unidad.Nombre,
                    Cantidad: 1,
                    Precio: response.PrecioVenta,
                    Importe: (parseFloat(response.PrecioVenta) * 1) - (response.Descuento * 1),
                    descuentopor: 0,
                    descuento: response.Descuento,
                    //operacion: 1,
                    operacion: parseInt(response.TipoOperacion.IdTipoOperacion),
                    DesOperacion: response.TipoOperacion.Nombre,
                    Almacen: { IdAlmacen: $("#lstAlmacen").val() },
                    Sucursal: { IdSucursal: $("#lstSucursalCab").val() }
                });

                CalcularTotales();

                var $tb = $('#tbDetalle');
                $tb.find('tbody').empty();

                if (Lista.Vars.Detalle.length == 0) {
                    $tb.find('tbody').append('<tr><td colspan="15">No existen registros</td></tr>')
                } else {

                    $.grep(Lista.Vars.Detalle, function (oDetalle) {
                        $tb.find('tbody').append(
                            '<tr data-index=' + oDetalle["Material"]["IdMaterial"] + '>' +
                            '<td>' + oDetalle["Material"]["Codigo"] + '</td>' +
                            '<td>' + oDetalle["Material"]["Nombre"] + '</td>' +
                            '<td>' + oDetalle["Unidad"] + '</td>' +
                            '<td>' + oDetalle["DesOperacion"] + '</td>' +
                            //'<td>' + '<select id="lstOperacion" name="lstOperacion" class="form-control select"  value="' + oDetalle["operacion"] + '"></select>' + '</td>' +
                            //'<td>' + '<input type="text" class="form-control por" id="descuentoPor" value="' + oDetalle["descuentopor"] + '">' + '</td>' +
                            '<td>' + '<input type="text" class="form-control form-control-sm desc" id="descuento"   value="' + oDetalle["descuento"] + '">' + '</td>' +
                            '<td>' + '<input type="text" class="form-control form-control-sm Cant" id="Cantidad"  value="' + oDetalle["Cantidad"] + '">' + '</td>' +
                            '<td>' + '<input type="text" class="form-control form-control-sm Price" id="Precio"  value="' + oDetalle["Precio"] + '" disabled>' + '</td>' +
                            '<td>' + (oDetalle["Importe"] * 1).toFixed(2) + '</td>' +
                            '<td class="text-center">' +
                            '<button type="submit" class="btn btn-danger btn-xs"><i class="fa fa-trash"></i></button>' +
                            '</td>' +
                            '</tr>'

                        );
                    });
                }
            }
        }

    });

    $("[data-dismiss=modal]").trigger({ type: "click" });//cerrar modal.


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


function LimpiarConteniedo() {

    //Lista.CargarSerieDoc();
    $("#lstTipoDoc").val(0);
    $("#lstTipoDoc").trigger('change');

    $("#lstTipoPago").val(0)
    $("#lstTipoPago").trigger('change');

    $("#txtDocCli").val('');
    $("#hdfId").val(0);
    $("#txtNomCli").val('');
    $("#txtDireccionInicio").val('');
    $("#MontoSubtotal").html('0.00');
    $("#subtotales").val(0);
    $("#mCant").html('0.00');
    $("#dFechaAten").datepicker().datepicker("setDate", new Date());
    $("#MontoIGV").html('0.00');
    $("#MontoIGV").html('0.00');
    $("#igv").val(0);
    $("#Montodescuento").html('0.00');
    $("#descuento").val(0);
    $("#Totales").val(0);
    $("#MontoCalulado").html('0.00');
    $("#hMonedaComprobante").html('SOLES');
    $("#MontoIGV").val();
    Lista.Vars.Detalle = [];
    var $tb = $('#tbDetalle');
    $tb.find('tbody').empty();
    if (Lista.Vars.Detalle.length == 0) {
        $tb.find('tbody').append('<tr><td colspan="11">No existen registros</td></tr>')
    }
    changeDoc($("#lstDocumento").val());

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

//DATOS PARA REGISTRO CLIENTE MODAL
function ListaPrecio(Producto, Cliente) {
    $.ajax({
        async: false,
        type: 'post',
        url: General.Utils.ContextPath('venta/ListaPrecio'),
        beforeSend: General.Utils.StartLoading,
        complete: General.Utils.EndLoading,
        dataType: 'json',
        data: { comienzo: 0, iMedia: 20, producto: Producto, cliente: Cliente },
        success: function (response) {
            console.log(response);

            if (!response.hasOwnProperty('ErrorMessage')) {
                var $td = $("#tbPrecio")
                $td.find('tbody').empty();
                if (response.length == 0) {
                    $td.find('tbody').html('<tr><td colspan="10">No hay resultados para el filtro ingresado</td></tr>');
                    $('#pHelperProductos').html('');
                } else {
                    $.grep(response, function (item) {
                        $td.find('tbody').append(
                            '<tr data-id="' + item["DetalleVenta"]["material"]["IdMaterial"] + '">' +
                            '<td>' + formatNumber(item["DetalleVenta"]["precio"]) + '</td>' + 
                            '<td>' + formatNumber(item["DetalleVenta"]["cantidad"]) + '</td>' + 
                            '<td>' + item["fechaEmision"] + '</td>' +
                            '<td>' + item["moneda"]["Nombre"] + '</td>' +
                            '</tr>'
                        );
                    });
                    $('#pHelperProductos').html('Existe(n) ' + response["Total"] + ' resultado(s) para mostrar.' +
                        (response["Total"] > 0 ? ' Del&iacute;cese hacia abajo para visualizar m&aacute;s...' : ''));
                }

            }
        }
    });
}
$("#btnBuscar").click(function () {
    BuscarSunat();
});
$("#txtDocumento").change(function () {
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
                    $("[data-dismiss=modal]").trigger({ type: "click" });//cerrar modal
                    Limpiar();
                }
            });
        }
    }
});

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

function Limpiar() {
    //$("#hdfIdCliente").val(0);
    //$("#txtNombre").val('');
    //$("#txtSigla").val('');
    //$("#lstCategoria").val('0');

    $("#hdfId").val(0);

    $("#txtDocumento").val('');
    $("#txtRazon").val('');
    $("#txtTelefono").val('');
    $("#txtCelular").val('');
    $("#txtEmail").val('');
    $("#txtDirecion").val('');
    $("#lstDepartamento").val('0');
    $("#lstTipoDoc").val('0');
    //$("#lstTipoDoc").empty();

    $("#lstProvincia").empty();
    $("#lstProvincia").append($('<option>', { value: 0, text: 'Seleccione' }));
    $("#lstDistrito").empty();
    $("#lstDistrito").append($('<option>', { value: 0, text: 'Seleccione' }));

}