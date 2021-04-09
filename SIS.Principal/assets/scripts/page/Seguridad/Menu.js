var Lista = {
    CargarMenu: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaCombo"),
            dataType: 'json',
            data: { Id: 15 },
            success: function (response) {
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 
                    $("#lstMenu").empty();
                    $("#lstMenu").append($('<option>', { value: 0, text: 'Seleccione' }));
                    $.grep(response, function (oDocumento) {
                        $('select[name="lstMenu"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));

                    });
                }
            }

        });
    }
}

$(function () {
    Lista.CargarMenu();
    var table = $("#tbMenu").DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ registros por p&aacute;gina",
            "zeroRecords": "No se encontraron datos.",
            "info": "Mostrando la p&aacute;gina _PAGE_ de _PAGES_",
            "infoEmpty": "No hay registros disponibles",
            "infoFiltered": "(filtrando _MAX_ total de registros)",
            "search": "Buscar:",
            "paginate": {
                "first": "Primero",
                "previous": "Anterior",
                "next": "Siguiente",
                "last": "&Uacute;timo"
            },
        },


        "processing": true,

        "order": [],
        "ajax": {
            "url": General.Utils.ContextPath('Seguridad/ListadoMenu'),
            "contentType": "application/json",
            "type": "POST",
            "dataType": "JSON",
            "data": function (d) {
                //console.log(d);
                return JSON.stringify(d);
            },

        },
        "columns": [
            { "data": "Id", "width": "1%" },
            { "data": "Text" },
            { "data": "Orden" },
            { "data": "Nombre" },
            { "data": "Vista" },
            { "data": "Controlador" },
            {
                "data": "Icono", "width": "10%",
                "render": function (data) {
                    //console.log(data)
                    var html = '<span class="badge badge-success"><i class="fa ' + data + '"></i></span>';

                    return html;
                }

            },
            {
                "data": "sEstado", "width": "10%",
                "render": function (data) {
                    var html = "";
                    switch (data) {
                        case "A":
                            html = '<span  class="badge badge-success"><i class="fa fa-thumbs-o-up"></i> Activo</span >';
                            break;
                        case "I":
                            html = '<span  class="badge badge-danger"><i class="fa fa-thumbs-o-down"></i> Inactivo</span >';
                            break;
                    }
                    return html;
                }
            },
            {
                "data": "Id", "render": function (data) {
                    return '<button class="btn btn-warning btn-xs evento" data-toggle="modal" data-target="#Nuevo" onclick="EditarUsu(' + data + ');"><i class="fa fa-edit"></i> </button>';
                },

                "orderable": false, "searchable": false, "width": "8%"
            }
        ],
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'pdf',
                text: "<i class='fa fa-file-pdf-o'> PDF</i>",
                titleAttr: "Exportar PDF",
                className: "btn btn-danger btn-xs",
            },
            {

                extend: 'excelHtml5',
                text: "<i class='fa fa-file-excel-o'> Excel</i>",
                titleAttr: "Exportar Excel",
                className: "btn btn-success btn-xs",
                customize: function (xlsx) {
                    var sheet = xlsx.xl.worksheets['sheet1.xml'];

                    $('row c[r^="C"]', sheet).attr('s', '2');
                }
            }
        ]
    });

    $("#btnGrabar").click(function () {



        var oCliente = {
            Id: $("#IdMenu").val(),
            IdPadre: $("#lstMenu").val(),

            Orden: $("#txtOrden").val(),
            Nombre: $("#txtMenu").val(),
            Vista: $("#txtVista").val(),
            Controlador: $("#txtControlador").val(),
            Icono: $("#txtIcono").val(),
            sEstado: ($("#Estado").is(':checked') === true ? 'A' : 'I')

        }

        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath('Seguridad/RegistrarMenu'),
            dataType: 'json',
            data: oCliente,
            success: function (response) {
                //console.log(response);
                if (response["Id"] == TypeMessage.Success) {

                    General.Utils.ShowMessageRequest(response);

                } else {

                    General.Utils.ShowMessageRequest(response);
                }
                $("#tbMenu").DataTable().ajax.reload();
                $("[data-dismiss=modal]").trigger({ type: "click" });//cerrar modal
                Limpiar();
            }
        });

    });

    $("#btnNuevo").click(function () {

        Limpiar();
    });

});

function EditarUsu(data) {

    Obtener(data);
}

var Obtener = function (Id) {
    var senData = {
        Id: Id
    }
    $.ajax({
        async: true,
        type: 'post',
        url: General.Utils.ContextPath('Seguridad/BuscarMenuId'),
        dataType: 'json',
        data: senData,
        success: function (response) {
            //console.log(response);
            $("#IdMenu").val(response.Id);
            $("#lstMenu").val(response.IdPadre);
            $("#txtOrden").val(response.Orden);
            $("#txtMenu").val(response.Nombre);
            $("#txtVista").val(response.Vista);
            $("#txtControlador").val(response.Controlador);
            $("#txtIcono").val(response.Icono);
            $("#Estado").prop('checked', response.sEstado === "A" ? true : false);

        }

    });
}
function Limpiar() {
    $("#IdMenu").val('0');
    Lista.CargarMenu();
    $("#txtOrden").val('');
    $("#txtMenu").val('');
    $("#txtVista").val('');
    $("#txtControlador").val('');
    $("#Estado").prop('checked');
}