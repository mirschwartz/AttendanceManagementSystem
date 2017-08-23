$(function () {
    var classIndex = 0;
    var teacherIndex = 1;
    var date;
    var classId = 0;
    var students;

    $('#datepicker').datepicker();
    $('#update').hide();
    $('#submit').attr('disabled', true);
    $('.pager-btn').attr('disabled', true);

    $("#sort-class").on('change', function () {
        classId = $(this).val();
        date = $('#datepicker').val();
        if (Date.parse(date) && classId > 0) {
            GetClassList();
        }
    })
    $('#datepicker').datepicker().on('changeDate', function () {
        date = $('#datepicker').val();
        if (Date.parse(date) && classId > 0) {
            GetClassList();
        }
    })

    $("#add-column").on('click', function () {
        GetTeachers();

        var studentIndex = 0;
        students.forEach(function (student) {
            $(".table-attendance tr:eq(" + (studentIndex + 1) + ")").append(`<td><input type="hidden" name="class[${classIndex}][${studentIndex}].StudentClassId" value="${student.StudentClassId}" />` +
            `<input type="checkbox" name="class[${classIndex}][${studentIndex}].Status" value="1" />` +
            `<input type="hidden" name="class[${classIndex}][${studentIndex}].Period" value="${classIndex}" />` +
            `<input type="hidden" name="class[${classIndex}][${studentIndex}].Status" value="2" /></td>`);
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
        $('.pager-btn').show();
        $.get('/attendance/getClassListByDate', { CurrentDate: date, btn: button, classId: classId }, function (Date) {
            date = Date;
            $('#datepicker').datepicker('setDate', date);
        })
    })

    function GetClassList() {
        classIndex = 0;

        $('#submit').show();
        $('#update').hide();
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
                    GetTeachers();
                    var studentIndex = 0;
                    students.forEach(function (s) {
                        $('.table-attendance').append(`<tr><td>${s.FirstName}</td><td>${s.LastName}</td><td>` +
                            `<input type="hidden" name="class[${classIndex}][${studentIndex}].StudentClassId" value="${s.StudentClassId}" />` +
                            `<input type="hidden" name="class[${classIndex}][${studentIndex}].Period" value="${classIndex}" />` +
                            `<input type="checkbox" name="class[${classIndex}][${studentIndex}].Status" value="1" />` +
                            `<input type="hidden" name="class[${classIndex}][${studentIndex}].Status" value="2" /></td></tr>`);
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
        var first = true;
        $('.table-attendance tr:gt(0)').remove();
        $('.table-attendance td:gt(1)').remove();
        $('.table-attendance th:gt(1)').remove();

        $('#submit').hide();
        $('#update').show();
        $('#form').attr('action', '/attendance/updateAttendance');

        var studentWithMostPeriods = classes.sort((a, b) => { return a.Period - b.period }).reverse()[0];
        var count = classes.filter(c => { return c.StudentId === studentWithMostPeriods.StudentId }).length;

        students.forEach(function (s) {
            var tracker = 1
            classIndex = 0;
            var student = classes.filter(function (c) { return c.StudentId === s.StudentClassId }).sort((a, b) => { return a.Period - b.Period });

            $('.table-attendance').append(`<tr><td>${s.FirstName}</td><td>${s.LastName}</td></tr>`);

            student.forEach(function (period, index, array) {

                if (studentWithMostPeriods.StudentId == s.StudentClassId) {
                    GetTeachers(period);
                }

                $(".table-attendance tr:eq(" + (studentIndex + 1) + ")").append(`<td><input type="hidden" name="class[${classIndex}][${studentIndex}].StudentClassId" value="${s.StudentClassId}" />` +
                `<input type="checkbox" ${period.Status === 1 ? 'checked' : ''} name="class[${classIndex}][${studentIndex}].Status" value="1" />` +
                `<input type="hidden" name="class[${classIndex}][${studentIndex}].Notes" value="${period.notes}"/>` +
                `<input type="hidden" name="class[${classIndex}][${studentIndex}].Period" value="${classIndex}" />` +
                `<input type="hidden" name="class[${classIndex}][${studentIndex}].Status" value="${period.Status === 3 ? 3 : 2}" /></td>`);
                classIndex++;
                tracker++;
            })

            //Adds rows for students that were added since this transaction...
            while (tracker <= count) {
                $(".table-attendance tr:eq(" + (studentIndex + 1) + ")").append(`<td><input type="hidden" name="class[${classIndex}][${studentIndex}].StudentClassId" value="${s.StudentClassId}" />` +
                `<input type="checkbox" name="class[${classIndex}][${studentIndex}].Status" value="1" />` +
                `<input type="hidden" name="class[${classIndex}][${studentIndex}].Period" value="${classIndex}" />` +
                `<input type="hidden" name="class[${classIndex}][${studentIndex}].Status" value="2" /></td>`);
                classIndex++;
                tracker++;
            }
            
            studentIndex++;
            first = false;
        });
    }

    function GetTeachers(previousClass) {
        //teacher dropdown:
        $.get('/attendance/GetTeachers', function (result) {
            var select = (`<th><select style="padding-left:1px; padding-right:1px; font-size: 8pt;" name="Teacher[${teacherIndex}].TeacherSubjectId" class="form-control">` +
                `<option value="0">Select a Teacher...</option>`);
            result.forEach(function (teacher) {
                if (previousClass && previousClass.TeacherSubjectId === teacher.TeacherSubjectId) {
                    select += (`<option selected value="${teacher.TeacherSubjectId}">${teacher.Title} ${teacher.LastName} - ${teacher.Subject}</option>`);
                } else {
                    select += (`<option value="${teacher.TeacherSubjectId}">${teacher.Title} ${teacher.LastName} - ${teacher.Subject}</option>`);
                }
            })
            select += (`</select></th>`);
            $('.table-attendance tr:first').append(select);
            teacherIndex++;
        })
    }
})