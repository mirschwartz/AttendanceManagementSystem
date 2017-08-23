$(function () {
    $('.date').datepicker();

    $('#print').on('click', function () {
        $(this).hide();
        $('#back-to-students').hide();
        window.print();
        $(this).show();
        $('#back-to-students').show();
    })

    $(".date").on("change", function () {
        var from = stringToDate($("#date-from").val());
        var to = stringToDate($("#date-to").val());
        var count = 1;

        $(".table tr:gt(0)").each(function () {
            var row = $(this);
            var date = stringToDate(row.find("td").eq(3).text());

            var show = true;

            if (from && date < from) {
                show = false;
            }

            if (to && date > to) {
                show = false;
            }

            if (show) {
                row.show();
                row.find('td:eq(0)').text(count);
                count++;
            } else {
                row.hide();
            }
        });
    });

    //parse entered date. return NaN if invalid
    function stringToDate(s) {
        var ret = NaN;
        var parts = s.split("/");
        date = new Date(parts[2], parts[0], parts[1]);
        if (!isNaN(date.getTime())) {
            ret = date;
        }
        return ret;
    }
})