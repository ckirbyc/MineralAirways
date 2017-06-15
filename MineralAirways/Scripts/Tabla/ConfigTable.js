$(document).ready(function () {
    $('.tablaMantenedores').DataTable({
        "language": {
            "processing": "<font color='#cc0000'>Procesando...</font>",
            "lengthMenu": "<font color='#cc0000'>Mostrar _MENU_" + "&nbsp;&nbsp;&nbsp;</font>",
            "zeroRecords": "<font color='#cc0000'><B>No se encontraron resultados</B></font>",
            "info": "<font color='#cc0000'>Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros</font>",
            "infoEmpty": "<font color='#cc0000'>Mostrando registros del 0 al 0 de un total de 0 registros</font>",
            "infoFiltered": "<font color='#cc0000'>(filtrado de un total de _MAX_ registros)</font>",
            "infoPostFix": "",
            "search": "<font color='#cc0000'>Buscar:</font>",
            "url": "",
            "infoThousands": ",",
            "loadingRecords": "Cargando...",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            },
            "aria": {
                "sortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        }
    });
});