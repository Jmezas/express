var Lista = {
    CargarPerfil: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaCombo"),
            dataType: 'json',
            data: { Id: 8 },
            success: function (response) {
                $("#lstPerfil").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 

                    $.grep(response, function (oDocumento) {
                        $('select[name="lstPerfil"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));

                    });
                }
            }

        });
    },
 

    CargarDocumento: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaCombo"),
            dataType: 'json',
            data: { Id: 6 },
            success: function (response) {
                $("#lstDocumento").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error 

                    $.grep(response, function (oDocumento) {
                        $('select[name="lstDocumento"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));

                    });
                }
            }

        });
    },
    CargarSucursal: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaCombo"),
            dataType: 'json',
            data: { Id: 7 },
            success: function (response) {
                //$("#lstsucursal").append($('<option>', { value: 0, text: 'Seleccione' }));
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error

                    $.grep(response, function (oDocumento) {
                        $('select[name="lstsucursal"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));

                    });
                }
            }
        });
    }
    
   
}


$(function () {
    Lista.CargarPerfil();
    Lista.CargarDocumento();
    Lista.CargarSucursal();
    $("#lstsucursal").select2({
        placeholder: "selecione sucursales",
      
    });

    var table = $("#tbUsuario").DataTable({
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
            "url": General.Utils.ContextPath('Seguridad/ListaUsuario'),
            "contentType": "application/json",
            "type": "POST",
            "dataType": "JSON",
            "data": function (d) {
                console.log(d);
                return JSON.stringify(d);
            },

        },
        "columns": [
            { "data": "Perfil.Nombre" },
            { "data": "NroDocumento" },
            { "data": "Nombre" },
            { "data": "Usuario" },
            { "data": "CorreoElectronico" },
            { "data": "Direccion" },
            { "data": "Telefono" },

            {
                "data": "Id", "render": function (data) {
                    return '<button class="btn btn-warning btn-sm btn-xs evento" data-toggle="modal" data-target="#ModalNuevo" onclick="Obtener(' + data + ');"><i class="fa fa-edit"></i> </button>' +
                        "&nbsp; " + '<button class="btn btn-danger btn-sm Eliminar" data-toggle="modal" data-target="#Eliminar"  "><i class="fa fa-trash"></i> </button>';

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

        var data = new FormData($('#frm-adjuntar')[0]);//el fromData
        var $form = $("#ModalNuevo");
        var oDatos = General.Utils.SerializeForm($form);
        if ($("#lstPerfil").val() == 0 || $("#lstDocumento").val() == 0 || $("#lstsucursal").val()==0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'debe de seleccionar los campos obligatorio');

        } else {
            if (General.Utils.ValidateForm($form)) {
                var Apli = $("#lstsucursal").val();
                var oDatos = {
       
                    Id: $("#hdfId").val(),
                    Perfil: {
                        IdPerfil: $("#lstPerfil").val(),
                    },

                    TipoDocumento: { Id: $("#lstDocumento").val(), },

                    Nombre: $("#txtNombre").val(),
                    ApellidoPaterno: $("#txtPaterno").val(),
                    ApellidoMaterno: $("#txtMaterno").val(),
                    NroDocumento: $("#txtDocumento").val(),
                    Usuario: $("#txtUsuario").val(),
                    Password: $("#txtContrasenia").val(),
                    imagen: "",
                    Direccion: $("#txtDireccion").val(),
                    Telefono: $("#txtTelefono").val(),

                    CorreoElectronico: $("#txtEmail").val(),
                    Sucursal: { codigo: Apli.toString()}

                }
                for (var prop in oDatos) {
                    var value = oDatos[prop];
                    if (typeof value === 'object') {
                        for (var valueProp in value) {
                            data.append('oDatos.' + prop + '.' + valueProp, value[valueProp]);
                        }
                    } else {
                        data.append('oDatos.' + prop, value);
                    }
                }
                console.log(oDatos)
                $.ajax({
                    async: true,
                    type: 'post',
                    url: General.Utils.ContextPath('Seguridad/InstUsuario'),
                    dataType: 'json',
                    data: data,
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        console.log(response);
                        if (response["Id"] == TypeMessage.Success) {
                            General.Utils.ShowMessageRequest(response);
                        } else {

                            General.Utils.ShowMessageRequest(response);
                        }
                        $("#tbUsuario").DataTable().ajax.reload();
                        $("[data-dismiss=modal]").trigger({ type: "click" });//cerrar modal
                        Limpiar();
                    }
                });
            }
        }
    });
    $("#tbUsuario").on('click', 'tbody .Eliminar', function () {
        var data = table.row($(this).parents("tr")).data();

        $("#hdfId").val(data.Id);           
        var Nombre = $(this).parents("tr").find("td").eq(2).text();
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
                    data: { Id: $("#hdfId").val(), IdFlag: 10 },
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
                        $("#tbUsuario").DataTable().ajax.reload(); 

                    }
                });

            }
        })

    });
 

    $("#btnNuevo").click(function () {
        $('#imgSalida').attr("src", "/Imagenes/Usuario/avatar.jpg");
        Limpiar();
    });

    $('#documento').change(function (e) {
        addImage(e);
    });

    $('#imgSalida').attr("src", "/Imagenes/Usuario/avatar.jpg");

    //visulizart
    $("#show_hide_password button").on('click', function (event) {
        event.preventDefault();
        if ($('#show_hide_password input').attr("type") == "text") {
            $('#show_hide_password input').attr('type', 'password');
            $('#show_hide_password i').addClass("fa-eye-slash");
            $('#show_hide_password i').removeClass("fa-eye");
        } else if ($('#show_hide_password input').attr("type") == "password") {
            $('#show_hide_password input').attr('type', 'text');
            $('#show_hide_password i').removeClass("fa-eye-slash");
            $('#show_hide_password i').addClass("fa-eye");
        }
    });
});

var Obtener = function (Id) {
    var senData = {
        Id: Id
    }
    $.ajax({
        async: true,
        type: 'post',
        url: General.Utils.ContextPath('Seguridad/ListaEditUsuario'),
        dataType: 'json',
        data: senData,
        success: function (response) {
            console.log(response);
            $("#hdfId").val(response.Id);
            $("#lstPerfil").val(response.Perfil.Id); 
            $("#lstDocumento").val(response.TipoDocumento.Id); 
            $("#txtUsuario").val(response.Usuario);
            $("#txtContrasenia").val(response.Password);
            $("#txtNombre").val(response.Nombre);
            $("#txtPaterno").val(response.ApellidoPaterno);
            $("#txtMaterno").val(response.ApellidoMaterno);
            $("#txtDocumento").val(response.NroDocumento);
            $("#txtDireccion").val(response.Direccion);
            $("#txtEmail").val(response.CorreoElectronico);
            $("#txtTelefono").val(response.Telefono); 
            //$("#lstsucursal").val(response.Sucursal.IdSucursal); 
            $("#imgSalida").attr("src", response.Imagen); 
            let string = response.Sucursal.codigo;
            let arr = string.split(','); 
            $("#lstsucursal").val(arr);
            $('#lstsucursal').trigger('change'); 
            var Img = response.Imagen.split("/");
            $("#hdfImg").val(Img[2]);   
        }

    });
}

function Limpiar() {
    $("#hdfId").val(0);
    $("#lstPerfil").val(0);
    $("#lstDocumento").val(0); 
    $("#txtUsuario").val('');
    $("#txtContrasenia").val('');
    $("#txtNombre").val('');
    $("#txtPaterno").val('');
    $("#txtMaterno").val('');
    $("#txtDocumento").val('');
    $("#txtDireccion").val('');
    $("#txtEmail").val('');
    $("#txtTelefono").val('');
  

}

function addImage(e) {
    var file = e.target.files[0],
        imageType = /image.*/;

    if (!file.type.match(imageType))
        return;

    var reader = new FileReader();
    reader.onload = fileOnload;
    reader.readAsDataURL(file);

    var f = e.target.files,
        len = f.length;
    for (var i = 0; i < len; i++) {
        console.log(f[i].name);
        $("#hdfImagen").val(f[i].name);
    }
}
function fileOnload(e) {
    var result = e.target.result;
    $('#imgSalida').attr("src", result);

}