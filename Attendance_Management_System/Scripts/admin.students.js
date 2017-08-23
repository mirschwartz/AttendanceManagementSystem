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
                sc.StudentClasses.forEach(function (id) {
                    if (PersonClassIndex == 0) {
                        html = FormClassesDropdown(id);
                    } else {
                        html += FormClassesDropdown(id);
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

    var addArray = ['#first-name', '#last-name', '#home-phone', '#cell-phone', '#address1', '#address2', '#city', '#state', '#zip'];
    addArray.forEach(function (input) {
        $(input).on('keyup', function () {
            if ($('#first-name').val() && $('#last-name').val()) {
                $('#btn-add').removeAttr('disabled');
            } else {
                $('#btn-add').attr('disabled', true);
            }
        })
    })

    var editArray = ['#first-name-edit', '#last-name-edit', '#cell-phone-edit', '#home-phone-edit', '#address1-edit', '#address2-edit', '#city-edit', '#state-edit', '#zip-edit'];
    editArray.forEach(function (input) {
        $(input).on('keyup', function () {
            VerifyEditStudentModalInput();
        })
    })

    $('.students-classes').on('change', '.class-dropdown', function () {
        VerifyEditStudentModalInput();
    })

    function VerifyEditStudentModalInput() {
        var firstName = $('#first-name-edit').val();
        var lastName = $('#last-name-edit').val();
        if (firstName && lastName) {
            $('#btn-save-changes').removeAttr('disabled');
        } else {
            $('#btn-save-changes').attr('disabled', true);
        }
    }

    function FormClassesDropdown(selectedClass) {
        var html = `<div class="form-group classnum${PersonClassIndex}"><div class="control-label col-md-3"><label>Class</label></div><div class="col-md-7 input-group" style="margin-bottom: 5px;">` +
                    `<select name="classId[${PersonClassIndex}]" class="form-control class-dropdown" >` +
                    `<option value="0">Select a Class..........</option>`;
        classes.forEach(function (classIndex) {
            html += `<option ${selectedClass === classIndex.ClassId ? 'selected' : ''} value="${classIndex.ClassId}">${classIndex.ClassName} - ${classIndex.Year}</option>`;
        })
        html += `</select>`;
        if (PersonClassIndex != 0) {
            html += `<span class="input-group-btn classnum${PersonClassIndex}"><button type="button" data-class-num="${PersonClassIndex}" class="btn remove-class"><span aria-hidden="true" class="glyphicon glyphicon-remove"></span></button></span></div></div>`;
        } else {
            html += `</div></div>`;
        }
        PersonClassIndex++;
        return html;
    }
})