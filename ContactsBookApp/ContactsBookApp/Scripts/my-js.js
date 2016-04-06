var $dialog;
var currentPage;

$(document).ready(function () {
    currentPage = 1;

    LoadData();
    $("#newContact").click(function () {
        OpenPopUp("/contacts/save");
    });
    $(document).on("click", "a.popup", function (e) {
        e.preventDefault();
        var page = $(this).attr("href");
        OpenPopUp(page);
    });
    $(document).on("submit", "#saveForm", function (e) {
        e.preventDefault();
        SaveContact();
    });
    $(document).on("submit", "#deleteForm", function (e) {
        e.preventDefault();
        DeleteContact();
    });
    $('#next').click(function () {
        currentPage += 1;
        LoadData(currentPage);
    });
    $('#prev').click(function () {
        currentPage = currentPage == 1 ? 1 : currentPage -= 1;
        LoadData(currentPage);
    });
});

function LoadData(page) {
    var pg = page === undefined ? '' : '?page=' + page;
    $.ajax({
        url: '/contacts/GetAll' + pg,
        type: 'GET',
        async: true,
        dataType: 'json',
        success: function (data) {
            if (data.length > 0) {
                var $data = $('<b></b>');

                $.each(data, function (i, row) {
                    var $row = $('<tr/>');
                    $row.append($('<td/>').html(row.FirstName));
                    $row.append($('<td/>').html(row.LastName));
                    $row.append($('<td/>').html(row.PhoneNumber));
                    $row.append($('<td/>').html(row.Email));
                    $row.append($('<td/>').html(row.Address));
                    $row.append($('<td/>').html(row.City));
                    $row.append($('<td/>').html(row.Zip));
                    $row.append($('<td/>').html(row.IsFriend));
                    $row.append($('<td/>').html("<a href='/contacts/save/" + row.Id + "' class='popup'>Edit</a>&nbsp|&nbsp<a href='/contacts/delete/" + row.Id + "' class='popup'>Delete</a>"));
                    $data.append($row);
                });

                $('#tbody').html($data.get(0).innerHTML);
            } else {
                currentPage -= 1;
            }

            $('#currentPage').html(currentPage);
        }
    });

    $.get('/contacts/getcount/', '', function (response) {
        $('#total').html(response);
    }, 'json');
}

function OpenPopUp(page) {
    var $pageContent = $('<div/>');
    $pageContent.load(page);
    $dialog = $('<div class="popupWindow" style="overflow:hidden"></div>')
        .html($pageContent)
        .dialog({
            draggable: false,
            autoOpen: false,
            resizable: false,
            model: true,
            height: 600,
            width: 600,
            close: function () {
                $dialog.dialog('destroy').remove();
            }
        });
    $dialog.dialog('open');
}

function SaveContact() {
    if ($('#FirstName').val().trim() == '' ||
        $('#LastName').val().trim() == '' ||
        $('#PhoneNumber').val().trim() == '' ||
        $('#Email').val().trim() == '' ||
        $('#City').val().trim() == '' ||
        $('#Zip').val().trim() == '') {
        $('#msg').html('<div class="failed">All fields are required.</div>');
        return false;
    }
    
    var contact = {
        Id: $('#Id').val() == '' ? '0' : $('#Id').val(),
        FirstName: $('#FirstName').val().trim(),
        LastName: $('#LastName').val().trim(),
        PhoneNumber: $('#PhoneNumber').val().trim(),
        Email: $('#Email').val().trim(),
        City: $('#City').val().trim(),
        Zip: $('#Zip').val().trim(),
        IsFriend: $('#IsFriend').is(':checked')
    };
    
    contact.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();

    $.post('/contacts/save/', contact, function (response) {
        if (response.status) {
            alert(response.message);
            LoadData(currentPage);
            $dialog.dialog('close');
        } else {
            $('#msg').html('<div class="failed">' + response.message + '</div>');
            return false;
        }
    }, 'json');
}

function DeleteContact() {
    $.post('/contacts/delete/', { 'id': $('#Id').val(), '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val() }, function (response) {
        if (response.status) {
            alert(response.message);
            LoadData(currentPage);
            $dialog.dialog('close');
        } else {
            alert(respond.message);
            return false;
        }
    }, 'json');
}