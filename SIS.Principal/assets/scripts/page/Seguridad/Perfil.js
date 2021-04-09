$(function () {

    var table = $("#tbPerfiles").DataTable({
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
            "url": General.Utils.ContextPath('Seguridad/ListaPerfil'),
            "contentType": "application/json",
            "type": "POST",
            "dataType": "JSON",
            "data": function (d) {
                console.log(d);
                return JSON.stringify(d);
            },

        },
        "columns": [
            { "data": "Nombre" },
            {
                "data": "Id", "render": function (data) {
                    return '<button class="btn btn-warning btn-xs evento" data-toggle="modal" data-target="#ModalNuevo" onclick="Obtener(' + data + ');"><i class="fa fa-edit"></i> </button>' +
                        "&nbsp; " +
                        '<a class="btn btn-info btn-xs" href="' + General.Utils.ContextPath('Seguridad/AccesosPorPerfil/' + data) + '"><i class="fa fa-pencil"></i> Editar Accesos</a>' +
                        "&nbsp; " + '<button class="btn btn-danger Eliminar" data-toggle="modal" data-target="#Eliminar"  "><i class="fa fa-trash"></i> </button>';

                },

                "orderable": false, "searchable": false, "width": "12%"
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

        console.log("entro")
        var $form = $("#ModalNuevo");
        var oDatos = General.Utils.SerializeForm($form);

        if (General.Utils.ValidateForm($form)) {
            var oPais = {
                Id: $("#hdfPerfil").val(),

                NombrePerfil: $("#txtPerfil").val(),

            }

            $.ajax({
                async: true,
                type: 'post',
                url: General.Utils.ContextPath('Seguridad/Insertar_Perfil'),
                dataType: 'json',
                data: oPais,
                success: function (response) {
                    console.log(response);
                    if (response["Id"] == TypeMessage.Success) {

                        General.Utils.ShowMessageRequest(response);

                    } else {

                        General.Utils.ShowMessageRequest(response);
                    }
                    $("#tbPerfiles").DataTable().ajax.reload();
                    $("[data-dismiss=modal]").trigger({ type: "click" });//cerrar modal
                    Limpiar();
                }
            });



        }
    });
    $("#tbPerfiles").on('click', 'tbody .Eliminar', function () {
        var data = table.row($(this).parents("tr")).data();

        $("#hdfPerfil").val(data.Id);
        var Nombre = $(this).parents("tr").find("td").eq(0).text();
        $("#Nombre").text(Nombre);

        Swal.fire({
            title: 'Eliminar',
            text: '¿Desea eliminar el registro ' + Nombre + ' ?',
            icon: 'warning',
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
                    url: General.Utils.ContextPath('Mantenimiento/Eliminar'),
                    dataType: 'json',
                    data: { Id: $("#hdfPerfil").val(), IdFlag: 11 },
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
                        $("#tbPerfiles").DataTable().ajax.reload();

                    }
                }); 
            }
        })

    });

    $("#btnNuevo").click(function () {
        Limpiar();
    });
});

var Obtener = function (Id) {
    var senData = {
        Id: Id
    }
    $.ajax({
        async: true,
        type: 'post',
        url: General.Utils.ContextPath('Seguridad/ListaEditPerfil'),
        dataType: 'json',
        data: senData,
        success: function (response) {
            console.log(response);
            $("#hdfPerfil").val(response.Id);
            $("#txtPerfil").val(response.Nombre);


        }

    });
}

function Limpiar() {
    $("#hdfPerfil").val(0); 
    $("#txtPerfil").val('');
}