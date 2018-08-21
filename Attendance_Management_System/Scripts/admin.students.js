$(function () {
    var classes;
    var PersonClassIndex;
    $('#btn-show-all').hide();

    $.get('/admin/getclasses', function (allClasses) {
        classes = allClasses;
    });

    $('#filter-class').on('change', function () {
        var classId = $(this).val();

        if (classId == 0) {
            $('.table tr:gt(0)').show();
        } else {
            $(".table tr:gt(0)").each(function () {

                var row = $(this);
                var personsclasses = $(this).find("td").eq(2).find('input');
                var show = false;

                personsclasses.each(function (c) {
                    c = $(this).val();
                    if (c === classId) {
                        show = true;
                    }
                });

                if (show) {
                    row.show();
                } else {
                    row.hide();
                }
            });
        }
    })

    $('#btn-add-student').on('click', function () {
        PersonClassIndex = 0;

        $('#btn-add').attr('disabled', true);
        $('#classes-div-new').empty();
        $('#classes-div-new').append(FormClassesDropdown());

        $('#new-student-modal').modal();

        $('#first-name').val('');
        $('#last-name').val('');
        $('#home-phone').val('');
        $('#cell-phone').val('');
        $('#address1').val('');
        $('#address2').val('');
        $('#city').val('');
        $('#state').val('');
        $('#zip').val('');
        $('#yearStart').val('');
        $('#yearEnd').val('');
    });

    $('#btn-show-active').on('click', function () {

        $('#btn-show-all').show();
        $('#btn-show-active').hide();

        $(".table tr:gt(0)").each(function () {
            var row = $(this);
            var active = $(this).find("td").eq(3).find("input").val();

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
        var studentId = $(this).data('id');
        $('#student-id').val(studentId);

        $.get('/admin/getStudentClasses', { studentId: studentId }, function (sc) {
            PersonClassIndex = 0;
            var html;
            if (sc.StudentClasses.length === 0) {
                html = FormClassesDropdown();
            } else {
                sc.StudentClasses.forEach(function (studentClass) {
                    if (PersonClassIndex == 0) {
                        html = FormClassesDropdown(studentClass);
                    } else {
                        html += FormClassesDropdown(studentClass);
                    }
                })
            }
            $('#first-name-edit').val(sc.Student.FirstName);
            $('#last-name-edit').val(sc.Student.LastName);
            $('#cell-phone-edit').val(sc.Student.CellPhone);
            $('#home-phone-edit').val(sc.Student.HomePhone);
            $('#address1-edit').val(sc.Student.Address1);
            $('#address2-edit').val(sc.Student.Address2);
            $('#city-edit').val(sc.Student.City);
            $('#state-edit').val(sc.Student.State);
            $('#zip-edit').val(sc.Student.Zip);
            $('#isActive').val(sc.Student.IsActive)
            $('#yearStart-edit').val(sc.Student.YearStart)
            $('#yearEnd-edit').val(sc.Student.YearEnd)

            $('#classes-div-edit').empty();
            $('#classes-div-edit').append(html);
            $('#btn-save-changes').attr('disabled', true);
            $('#edit-modal').modal();
        })
    })

    $('.add-class-new').on('click', function () {
        $('#classes-div-new').append(FormClassesDropdown());
    });

    $('.add-class-edit').on('click', function () {
        $('#classes-div-edit').append(FormClassesDropdown());
        VerifyEditStudentModalInput();
    });

    $('.students-classes').on('click', '.remove-class', function () {
        var selectId = $(this).data('class-num');
        $(this).remove();
        $(`.classnum${selectId}`).remove();

        PersonClassIndex = 0;
        //relists subjects...
        $('.students-classes select').each(function (index) {
            $(this).attr('name', `classId[${index}]`);
            PersonClassIndex++;
        })

        VerifyEditStudentModalInput();
    })

    var addArray = ['#first-name', '#last-name', '#home-phone', '#cell-phone', '#address1', '#address2', '#city', '#state', '#zip','#yearStart','#yearEnd'];
    addArray.forEach(function (input) {
        $(input).on('keyup', function () {
            if ($('#first-name').val() && $('#last-name').val()) {
                $('#btn-add').removeAttr('disabled');
            } else {
                $('#btn-add').attr('disabled', true);
            }
        })
    })

    var editArray = ['#first-name-edit', '#last-name-edit', '#cell-phone-edit', '#home-phone-edit', '#address1-edit', '#address2-edit', '#city-edit', '#state-edit', '#zip-edit','#yearStart-edit','#yearEnd-edit'];
    editArray.forEach(function (input) {
        $(input).on('keyup', function () {
            VerifyEditStudentModalInput();
        })
    })

    $('.students-classes').on('change', '.class-dropdown', function () {
        VerifyEditStudentModalInput();
    })

    $('.students-classes').on('change', '.isActiveCheckbox', function () {
        VerifyEditStudentModalInput();
    })

    function VerifyEditStudentModalInput() {
        var firstName = $('#first-name-edit').val();
        var lastName = $('#last-name-edit').val();
        if (firstName && lastName) {
            $('#btn-save-changes').removeAttr('disabled');

            $('#first-name-edit-div').removeClass('has-error');
            $('#last-name-edit-div').removeClass('has-error');
        } else {
            if (!firstName) {
                $('#first-name-edit-div').addClass('has-error');
            }
            if (!lastName) {
                $('#last-name-edit-div').addClass('has-error');
            }
            $('#btn-save-changes').attr('disabled', true);
        }
    }

    function FormClassesDropdown(selectedClass) {

        var html = `<div class="form-group classnum${PersonClassIndex}"><div class="control-label col-md-3"><label>Class</label></div>` +
                    `<div class="col-md-7 input-group" style="margin-bottom: 5px;">` +
                    `<select name="classId[${PersonClassIndex}].BCClassId" class="form-control class-dropdown" >` +
                    `<option value="0">Select a Class..........</option>`;

        classes.forEach(function (classIndex) {
            html += `<option ${selectedClass && selectedClass.BCClassId === classIndex.BCClassId ? 'selected' : ''} value="${classIndex.BCClassId}">${classIndex.ClassName} - ${classIndex.Year}</option>`;
        })
        if (PersonClassIndex != 0 && !selectedClass) {
            html += `</select><span class="input-group-btn classnum${PersonClassIndex}"><button type="button" data-class-num="${PersonClassIndex}" class="btn remove-class">` +
                    `<span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button></span></div></div>`;
        } else {
            html += `</select></div></div>`;
        }

        if (selectedClass) {
            html += `<input type="checkbox" class="isActiveCheckbox" name="classId[${PersonClassIndex}].IsActive" value="true" ${selectedClass.IsActive ? 'checked' : ''} /> Active` +
                    `<input type="hidden" name="classId[${PersonClassIndex}].IsActive" value="false"/></div>` +
                    `<input type="hidden" name="classId[${PersonClassIndex}].BCStudentClassId" value="${selectedClass.BCStudentClassId}" />` +
                    `<input type="hidden" name="classId[${PersonClassIndex}].BCStudentId" value="${selectedClass.BCStudentId}" />`
        }

        PersonClassIndex++;
        return html;
    }
})