var PopUp, AddrTable;
$(document).ready(function () {
    AddrTable = $("#AddressTBL").DataTable({
        "ajax": {
            "type": "POST",
            "url": "/Address/GetEmpAddr",
            "datatype": "json"
        },
        "columns": [
            { "data": "Name" },
            { "data": "Address" },
            { "data": "City" },
            { "data": "State" },
            { "data": "PinCode" },
            {
                "data": "ADDID", "render": function (data) {
                    return "<a class='btn btn-default btn-sm' onclick=PopupForm('/Address/AddOrEdit/" + data + "')><i class='fa fa-pencil-square'></i> Edit</a> <a class='btn btn-danger btn-sm' onclick=RemoveAddr(" + data + ")><i class='fa fa-trash'></i> Delete</a>"
                },
                "searchable": false
            }
        ]
    })
})

function PopupForm(url) {
    var formDiv = $('<div/>');
    $.get(url).done(function (response) {
        formDiv.html(response)

        PopUp = formDiv.dialog({
            autoOpen: true,
            resizable: false,
            title: "Please fill the Details",
            height: 500,
            width: 700,
            close: function () {
                PopUp.dialog('destroy').remove()
            }
        })
    })
}

function submitForm(from) {
    $.ajax({
        type: "POST",
        url: from.action,
        data: $(from).serialize(),
        success: function (data) {
            if (data.success) {
                PopUp.dialog('close');
                AddrTable.ajax.reload();

                $.notify(data.message, {
                    globalPosition: "top center",
                    className: "success"
                })
            }
        }
    })
    return false;
}

function RemoveAddr(id) {
    if (confirm("You want to conformly delete the record")) {
        $.ajax({
            type: "POST",
            url: "/Address/RemAddr/" + id,
            success: function (data) {
                if (data.success) {
                    AddrTable.ajax.reload();

                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success"
                    })
                }
            }
        })
    }
}