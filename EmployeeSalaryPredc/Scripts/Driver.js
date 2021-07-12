var PopUp, driverTable;
$(document).ready(function () {
    driverTable = $("#DriverTBL").DataTable({
        "ajax": {
            "type": "POST",
            "url": "/Driver/GetDriver",
            "datatype": "json"
        },
        "columns": [
            { "data": "Name", "autoWidth": true },
            { "data": "PhoneNo", "autoWidth": true },
            {
                "data": "DriverId", "render": function (data) {
                    return "<a class='btn btn-default btn-sm' onclick=PopupForm('/Driver/AddOrEdit/" + data + "')><i class='fa fa-pencil-square'></i> Edit</a> <a class='btn btn-danger btn-sm' onclick=RemoveDriv(" + data + ")><i class='fa fa-trash'></i> Delete</a>"
                },
                "searchable": false
            }
        ]
    })
})

function PopupForm(url) {
    var formDiv = $('<div />');
    $.get(url).done(function (response) {
        formDiv.html(response);

        PopUp = formDiv.dialog({
            autoOpen: true,
            resizable: false,
            title: "Plese fill the driver details",
            height: 500,
            width: 700,
            close: function () {
                PopUp.dialog('destroy').remove();
            }
        })
    })
}

function submitForm(form) {
    $.ajax({
        type: "POST",
        url: form.action,
        data: $(form).serialize(),
        success: function (data) {
            if (data.success) {
                PopUp.dialog('close');
                driverTable.ajax.reload();

                $.notify(data.message, {
                    globalPosition: "top center",
                    className: "success"
                })
            }
        }
    })
    return false;
}


function RemoveDriv(id) {
    if (confirm("Are you sure you want to delete")) {
        $.ajax({
            type: "POST",
            url: '@Url.Action("RemoveRecord","Driver")/' + id,
            success: function (data) {
                if (data.success) {
                    driverTable.ajax.reload();

                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success"
                    })
                }
            }
        })
    }
}