$(function () {
    $('#date').datepicker();
    $('#date').val('');
    $('#btn-fewer-options').hide();

    $('#print').on('click', function () {
        $(this).hide();
        $('#btn-fewer-options').hide();
        $('#btn-more-options').hide();
        $('#create-report').hide();
        $('#more-options').removeAttr('hidden');

        window.print();

        $(this).show();
        $('#btn-fewer-options').show();
        $('#create-report').show();
    })

    $('#btn-more-options').on('click', function () {
        $(this).hide();
        $('#more-options').removeAttr('hidden');
        $('#btn-fewer-options').show();
    })

    $('#btn-fewer-options').on('click', function () {
        $(this).hide();
        $('#more-options').attr('hidden', true);
        $('#btn-more-options').show();
    })
})