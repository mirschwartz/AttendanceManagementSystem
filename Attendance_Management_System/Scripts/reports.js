$(function () {
    $('#date').datepicker({
        orientation: "bottom auto",
        autoclose: true,
        todayHighlight: true
    });

    $('#btn-fewer-options').hide();
    $('#more-options').hide();

    $('#print').on('click', function () {
        $(this).hide();
        $('#btn-fewer-options').hide();
        $('#btn-more-options').hide();
        $('#create-report').hide();
        $('#options').hide();
        $('#more-options').hide();

        var teacher = $('#sort-teacher option:selected').text();
        var teacherVal = $('#sort-teacher option:selected').val();
        var classes = $('#sort-class option:selected').text();
        var classesVal = $('#sort-class option:selected').val();
        var student = $('#sort-student option:selected').text();
        var studentVal = $('#sort-student option:selected').val();
        var status = $('#status option:selected').text();
        var statusVal = $('#status option:selected').val();
        var date = $('#date').val();

        $('#options-printable').append(`<h5 style="padding-left: 15px;">${teacherVal > 0 ? 'Teacher: ' + teacher + ', ' : ''}` +
                                        `${classesVal > 0 ? 'Class: ' + classes + ', ' : ''}` +
                                        `${studentVal > 0 ? 'Student: ' + student + ', ' : ''}` +
                                        `${statusVal != 0 ? 'Status: ' + status + ', ' : ''}` +
                                        `${date ? 'Date: ' + date : ''}</h5>`)

        window.print();
    })

    $('#btn-more-options').on('click', function () {
        $(this).hide();
        $('#more-options').show();
        $('#btn-fewer-options').show();
    })

    $('#btn-fewer-options').on('click', function () {
        $(this).hide();
        $('#more-options').hide();
        $('#btn-more-options').show();
    })
})