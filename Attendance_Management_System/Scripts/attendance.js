$(function () {
    var classIndex = 0;
    var teacherIndex = 1;
    var date;
    var classId = 0;
    var students;
    var hideUpdate = true;
    $('#printable-form').hide()

    $('#print').on('click', function () {
        
        var className = $('#sort-class option:selected').text();
        var date = $('#datepicker').val();

        $(this).hide();
        $('.pager-btn').hide();
        $('#add-column').hide();
        $('#delete-column').hide();
        $('#class-info').hide();
        $('#submit').hide();
        $('#update').hide();
        $('#instructions').hide();
        $('#printable-form').append(`<h2>Bnos Chaim Attendance</h2><h3>${date}</h3><h6>Class ${className}</h6>`)
        $('#printable-form').show();

        $('.teachers').each(function (teacher) {
            var text = $(this).find('select option:selected').text();
            $(this).find('select').hide();
            $(this).append(`<div style="font-size: 9pt;">${text}</div>`)
        })

        window.print();

        $('#printable-form').hide()
        $('#printable-form').empty();

        $('.teachers').each(function (teacher) {
            $(this).find('div').remove();
            $(this).find('select').show();
        })

        $(this).show();
        $('.pager-btn').show();
        $('#add-column').show();
        $('#delete-column').show();
        $('#class-info').show();
        $('#instructions').show();
        if (hideUpdate) {
            $('#submit').show();
        } else {
            $('#update').show();
        }
    })

    $('#datepicker').datepicker();

    $('#update').hide();
    $('#submit').attr('disabled', true);
    $('.pager-btn').attr('disabled', true);

    $("#sort-class").on('change', function () {
        classId = $(this).val();
        date = $('#datepicker').val();

        if (Date.parse(date) && classId > 0) {
            GetClassList();
            $('#label').hide();
        } else {
            $('#label').show();
        }
    })

    $('#datepicker').datepicker().on('changeDate', function () {
        date = $('#datepicker').val();

        if (Date.parse(date) && classId > 0) {
            GetClassList();
            $('#label').hide();
        } else {
            $('#label').show();
        }
    })

    $("#add-column").on('click', function () {
        GetTeachers();

        var studentIndex = 0;
        students.forEach(function (student) {
            $(`.table-attendance tr:eq(${studentIndex + 1})`).append(
                `<td><input type="hidden" name="class[${classIndex}][${studentIndex}].BCStudentClassId" value="${student.BCStudentClassId}" />` +
                `<input type="checkbox" name="class[${classIndex}][${studentIndex}].Status" value="2" />` +
                `<input type="hidden" name="class[${classIndex}][${studentIndex}].Period" value="${classIndex}" />` +
                `<input type="hidden" name="class[${classIndex}][${studentIndex}].Status" value="1" /></td>`); //checked = unexcusedAbsence
            studentIndex++;
        })
        classIndex++;
    })

    $('#delete-column').on('click', function () {
        //ensures that user can't remove first 3 columns
        if ($('.table-attendance td:last-child').index() > 2) {
            $('.table-attendance td:last-child').remove();
            $('.table-attendance th:last-child').remove();
        }
    })

    $('.pager-btn').on('click', function () {
        var button = $(this).data('btn');

        $.get('/attendance/getClassListByDate', { CurrentDate: date, btn: button, classId: classId }, function (Date) {
            date = Date;
            $('#datepicker').datepicker('setDate', date);
        })
    })

    function GetClassList() {
        classIndex = 0;

        $('#submit').show();
        $('#update').hide();
        hideUpdate = true;

        $('#form').attr('action', '/attendance/MarkAttendance');

        //clears extra columns
        $('.table-attendance tr:gt(0)').remove();
        $('.table-attendance td:gt(1)').remove();
        $('.table-attendance th:gt(1)').remove();

        if (classId != 0) {
            $('#add-column').removeAttr('disabled');
            $('#delete-column').removeAttr('disabled');
            $('#submit').removeAttr('disabled');
            $('.pager-btn').removeAttr('disabled');
        }
        else {
            $('#add-column').attr('disabled', true);
            $('#delete-column').attr('disabled', true);
            $('#submit').attr('disabled', true);
            $('.pager-btn').attr('disabled', true);
        }

        $.get('/attendance/getPastAttendance', { Date: date, ClassId: classId }, function (classes) {

            $.get('/attendance/getClassList', { ClassId: classId }, function (s) {
                students = s;

                if (classes.length > 0) {
                    FillAttendanceSheet(classes);
                } else {
                    teacherIndex = 0;
                    var studentIndex = 0;

                    GetTeachers();

                    students.forEach(function (s) {
                        $('.table-attendance').append(`<tr><td style="font-size: 9pt;">${s.FirstName}</td><td style="font-size: 9pt;">${s.LastName}</td><td>` +
                            `<input type="hidden" name="class[${classIndex}][${studentIndex}].BCStudentClassId" value="${s.BCStudentClassId}" />` +
                            `<input type="hidden" name="class[${classIndex}][${studentIndex}].Period" value="${classIndex}" />` +
                            `<input type="checkbox" name="class[${classIndex}][${studentIndex}].Status" value="2" />` +
                            `<input type="hidden" name="class[${classIndex}][${studentIndex}].Status" value="1" /></td></tr>`); //checked = unexcused absence
                        studentIndex++;
                    })
                    classIndex++
                }
            })
        })
    }

    function FillAttendanceSheet(classes) {
        var studentIndex = 0;
        teacherIndex = 0;

        $('.table-attendance tr:gt(0)').remove();
        $('.table-attendance td:gt(1)').remove();
        $('.table-attendance th:gt(1)').remove();

        $('#submit').hide();
        $('#update').show();
        hideUpdate = false;

        $('#form').attr('action', '/attendance/updateAttendance');

        var studentWithMostPeriods = classes.sort((a, b) => { return a.Period - b.period }).reverse()[0];
        var count = classes.filter(c => { return c.StudentId === studentWithMostPeriods.StudentId }).length;

        students.forEach(function (s) {
            var tracker = 1
            classIndex = 0;

            var student = classes.filter(function (c) { return c.StudentId === s.BCStudentClassId }).sort((a, b) => { return a.Period - b.Period });
            $('.table-attendance').append(`<tr><td>${s.FirstName}</td><td>${s.LastName}</td></tr>`);

            student.forEach(function (period, index, array) {

                if (studentWithMostPeriods.StudentId == s.BCStudentClassId) {
                    GetTeachers(period);
                }

                $(".table-attendance tr:eq(" + (studentIndex + 1) + ")").append(
                    `<td><input type="hidden" name="class[${classIndex}][${studentIndex}].BCStudentClassId" value="${s.BCStudentClassId}" />` +
                    `<input type="checkbox" ${period.Status === 2 || period.Status === 3 || period.Status === 4 || period.Status === 5 ? 'checked' : ''} name="class[${classIndex}][${studentIndex}].Status" value="${period.Status !== 1 ? period.Status : 2}" />` +
                    `<input type="hidden" name="class[${classIndex}][${studentIndex}].Notes" value="${period.notes}"/>` + // checked = excused/unexcused absence/lateness
                    `<input type="hidden" name="class[${classIndex}][${studentIndex}].Period" value="${classIndex}" />` +
                    `<input type="hidden" name="class[${classIndex}][${studentIndex}].Status" value="1" /></td>`);
                classIndex++;
                tracker++;
            })
             

            //Adds rows for students that were added since this transaction...
            while (tracker <= count) {
                $(".table-attendance tr:eq(" + (studentIndex + 1) + ")").append(
                    `<td><input type="hidden" name="class[${classIndex}][${studentIndex}].BCStudentClassId" value="${s.BCStudentClassId}" />` +
                    `<input type="checkbox" name="class[${classIndex}][${studentIndex}].Status" value="2" />` +
                    `<input type="hidden" name="class[${classIndex}][${studentIndex}].Period" value="${classIndex}" />` + //checked = unexcused absence
                    `<input type="hidden" name="class[${classIndex}][${studentIndex}].Status" value="1" /></td>`);
                classIndex++;
                tracker++;
            }
            
            studentIndex++;
        });
    }

    function GetTeachers(previousClass) {
        //teacher dropdown:
        $.get('/attendance/GetTeachers', function (result) {

            var select = (`<th class="teachers"><select style="padding-left:1px; padding-right:1px; font-size: 8pt;" name="Teacher[${teacherIndex}].BCTeacherSubjectId" class="form-control">` +
                `<option value="0">Select a Teacher...</option>`);

            result.forEach(function (teacher) {
                if (previousClass && previousClass.TeacherSubjectId === teacher.BCTeacherSubjectId) {
                    select += (`<option selected value="${teacher.BCTeacherSubjectId}">${teacher.Subject} - ${teacher.Title} ${teacher.LastName}</option>`);
                } else {
                    select += (`<option value="${teacher.BCTeacherSubjectId}">${teacher.Subject} - ${teacher.Title} ${teacher.LastName}</option>`);
                }
            })

            select += (`</select></th>`);

            $('.table-attendance tr:first').append(select);
            teacherIndex++;
        })
    }
})