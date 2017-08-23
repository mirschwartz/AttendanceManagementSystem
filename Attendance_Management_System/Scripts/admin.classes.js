$(function () {
    $('#btn-show-all').hide();
    
    $('#btn-add-class').on('click', function () {
        $('#btn-add').attr('disabled', true);
        $('#add-modal').modal();
        $('#name').val('');
        $('#year').val('');
    })

    $('#btn-show-active').on('click', function () {
        $('#btn-show-all').show();
        $('#btn-show-active').hide();

        $(".table tr:gt(0)").each(function () {
            var row = $(this);
            var active = $(this).find("td").eq(2).find("input").val();

            var show = true;
            if (active == "false") {
                show = false;
            }

            if (show) {
                row.show();
            } else {
                row.hide();
            }
        });
    })

    $('#btn-show-all').on('click', function () {
        $('#btn-show-active').show();
        $('#btn-show-all').hide();

        $('.table tr:gt(0)').show();
    })

    $('.btn-edit').on('click', function () {
        $('#id').val($(this).data('id'));
        $('#active').val($(this).data('active'));
        $('#name-edit').val($(this).data('name'));
        $('#year-edit').val($(this).data('year'));
        $('#btn-save-changes').attr('disabled', true);
        $('#edit-modal').modal();
    })

    var addArray = ['#name', '#year'];
    addArray.forEach(function (input) {
        $(input).on('keyup', function () {
            if ($('#name').val() && $('#year').val()) {
                $('#btn-add').removeAttr('disabled');
            } else {
                $('#btn-add').attr('disabled', true);
            }
        })
    })

    var editArray = ['#name-edit', '#year-edit'];
    editArray.forEach(function (input) {
        $(input).on('keyup', function () {
            if ($('#name-edit').val() && $('#year-edit').val()) {
                $('#btn-save-changes').removeAttr('disabled');
            } else {
                $('#btn-save-changes').attr('disabled', true);
            }
        })
    })
})