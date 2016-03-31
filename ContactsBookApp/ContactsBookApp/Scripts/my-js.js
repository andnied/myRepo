var $dialog;

$(document).ready(function () {
    LoadData();
    $(document).on("click", "a.popup", function (e) {
        e.preventDefault();
        var page = $(this).attr("href");
        OpenPopUp(page);
    });
    $(document).on("submit", "#saveForm", function (e) {
        e.preventDefault();
        SaveContact();
    });
});

function LoadData() {
    $.ajax({
        url: '/contacts/GetAll',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var len = data.length;
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
                $row.append($('<td/>').html("<a href='/contacts/save/" + row.Id + "' class='popup'>Edit</a>&nbsp|&nbsp<a href='/contacts/save/" + row.Id + "' class='popup'>Delete</a>"));
                $data.append($row);
            });

            $('#tbody').html($data.get(0).innerHTML);
        }
    });
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
            LoadData();
            $dialog.dialog('close');
        } else {
            $('#msg').html('<div class="failed">' + response.message + '</div>');
            return false;
        }
    }, 'json');
}