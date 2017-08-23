$(function () {
    var subjectrow = 1;
    var editSubjectRow = 1

    $('#btn-show-all').hide();

    $('#btn-add-teacher').on('click', function () {
        subjectrow = 1;
        $('.subjects').append(AddSubject());
        subjectrow++;
        $('#btn-add').attr('disabled', true);
        $('#new-teacher-modal').modal();
        $('#title').val('');
        $('#first-name').val('');
        $('last-name').val('');
        $('#subject').val('');
    })

    $('#btn-show-all').hide();

    $('#btn-show-active').on('click', function () {
        $('#btn-show-all').show();
        $('#btn-show-active').hide();

        $(".table tr:gt(0)").each(function () {
            var row = $(this);
            var active = $(this).find("td").eq(4).find("input").val();

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

    $('.add-subject-new').on('click', function () {
        $('.subjects').append(AddSubject());
        subjectrow++;
    })

    $('.subjects').on('click', '.remove-subject', function () {
        var rowId = $(this).data('row');
        $(this).remove();
        $(`.subjectnum${rowId}`).remove();

        subjectrow = 1;
        //relists subjects...
        $('.subjects input').each(function (index) {
            console.log('hi')
            $(this).attr('name', `teachersubjects[${index}].Subject`);
            subjectrow++;
        })

        VerifyAddTeacherModalInput();
    })

    //$('form').submit(function () {
    //    event.preventDefault();
    //    var subjectsToRemove = [];
    //    $('.subject:hidden').each(function() {
    //        subjectsToRemove.push($(this).data('subjectId'));
    //    })
    //    $.post("/Students/Update", {Student, subjectsToRemove: subjectsToRemove}, function() {
    //        //close modal
    //        //window.location = '/students
    //    });
    //})

    $('.btn-edit').on('click', function () {

        editSubjectRow = 1;

        $('#title-edit').val();
        $('#first-name-edit').val();
        $('#last-name-edit').val();
        $('#cell-phone-edit').val();
        $('#home-phone-edit').val();
        $('.subjects-edit').empty();

        var teacherId = $(this).data('teacher-id')
        $('#teacher-id').val(teacherId);

        $.get('/admin/getTeacher', { teacherId: teacherId }, function (teacher) {

            $('#title-edit').val(teacher.Teacher.Title);
            $('#first-name-edit').val(teacher.Teacher.FirstName);
            $('#last-name-edit').val(teacher.Teacher.LastName);
            $('#cell-phone-edit').val(teacher.Teacher.CellPhone);
            $('#home-phone-edit').val(teacher.Teacher.HomePhone);
            $('#isActive-edit').val(teacher.Teacher.IsActive === 1 ? true : false);

            if (teacher.Subjects.length > 0) {
                teacher.Subjects.forEach(function (subject) {
                    
                    $('.subjects-edit').append(AddSubjectEdit(subject));
                    editSubjectRow++;
                })
            } else {
                $('.subjects-edit').append(AddSubjectEdit());
                editSubjectRow++;
            }

            $('#btn-save-changes').attr('disabled', true);
            $('#edit-modal').modal();
        })
    });

    $('.add-subject-edit').on('click', function () {
        $('.subjects-edit').append(AddSubjectEdit());
        editSubjectRow++;
    })

    $('.subjects-edit').on('click', '.remove-subject', function () {
        var rowId = $(this).data('row');
        $(this).remove();
        $(`.subjectnum${rowId}`).remove();

        //relists subjects...
        $('.subjects-edit input').each(function (index) {
            $(this).attr('name', `subjects[${index}]`);
        })

        VerifyEditTeacherModalInput();
    })

    $('.subjects-edit').on('change', '#isActiveCheckbox', function () {
        VerifyEditTeacherModalInput();
    })

    var addarr = ['#title', '#last-name', '#subject', '#cell-phone', '#home-phone', '.subject'];
    addarr.forEach(function (input) {
        $(input).on('keyup', function () {
            VerifyAddTeacherModalInput();
        });
    });
    
    var editarr = ['#title-edit', '#first-name-edit', '#last-name-edit', '#cell-phone-edit', '#home-phone-edit'];
    editarr.forEach(function (input) {
        $(input).on('keyup', function () {
            VerifyEditTeacherModalInput();
        })
    })

    $('.subjects-edit').on('keyup', '.subject-edit', function () {
        VerifyEditTeacherModalInput();
    })

    function VerifyAddTeacherModalInput() {
        var title = $('#title').val();
        var lastName = $('#last-name').val();

        if (title && lastName) {
            $('#btn-add').removeAttr('disabled');
        } else {
            $('#btn-add').attr('disabled', true);
        }
    }

    function VerifyEditTeacherModalInput() {

        var title = $('#title-edit').val();
        var lastName = $('#last-name-edit').val();

        if (title && lastName) {
            $('#btn-save-changes').removeAttr('disabled');
        } else {
            $('#btn-save-changes').attr('disabled', true);
        }
    }

    function AddSubjectEdit(subject) {
        var html = `<div class="form-group subjectnum${editSubjectRow}"><div class="col-md-3 control-label"><label>Subject</label></div>` +
                    `<div class="col-md-7 input-group" style="margin-bottom: 5px;">` +
                    `<input type="text" name="subjects[${editSubjectRow - 1}].Subject" ${subject != null ? `value='${subject.Subject}'` : ''} class="form-control subject-edit" />` +
                    `<span class="input-group-btn"><button type="button" data-row="${editSubjectRow}" class="btn remove-subject"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button></span></div>`;
        if (subject != null) {
            html += `Active <input type="checkbox" id="isActiveCheckbox" name="subjects[${editSubjectRow - 1}].IsActive" value="true" ${subject.IsActive ? 'checked' : ''} />` +
                `<input type="hidden" name="subjects[${editSubjectRow - 1}].IsActive" value="false"/>` +
                `<input type="hidden" name="subjects[${editSubjectRow - 1}].TeacherSubjectId" value="${subject.TeacherSubjectId}" />` +
                `<input type="hidden" name="subjects[${editSubjectRow - 1}].TeacherId" value="${subject.TeacherId}" />`
        }
        html += `</div>`

        return html;
    }

    function AddSubject() {
        return `<div class="form-group subjectnum${subjectrow}">` +
                    `<div class="control-label col-md-3"><label>Subject</label></div>` +
                    `<div class="col-md-7 input-group" style="margin-bottom: 5px;">` +
                    `<input type="text" name="teachersubjects[${subjectrow - 1}].Subject" class="form-control subject"/>` +
                    `<span class="input-group-btn"><button type="button" data-row="${subjectrow}" class="btn remove-subject">` +
                    `<span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button></span></div></div>`;
    }
})