var PopUp, VechTable;
$(document).ready(function () {
    VechTable = $("#VechileTBL").DataTable({
        "ajax": {
            "type": "POST",
            "url": "/Vechile/GetVech",
            "datatype": "json"
        },
        "columns": [
            { "data": "EmpName" },
            { "data": "DrivName" },
            { "data": "VechName" },
            { "data": "Number" },
            { "data": "Route" },
            { "data": "Capacity" },
            {
                "data": "VechId", "render": function (data) {
                    return "<a class='btn btn-default btn-sm' onclick=PopupForm('/Vechile/AddOrEdit/" + data + "')><i class='fa fa-pencil-square'></i> Edit</a> <a class='btn btn-danger btn-sm' onclick=RemoveVech(" + data + ")><i class='fa fa-trash'></i> Delete</a>"
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
                VechTable.ajax.reload();

                $.notify(data.message, {
                    globalPosition: "top center",
                    className: "success"
                })
            }
        }
    })
    return false;
}

function RemoveVech(id) {
    if (confirm("You want to conformly delete the record")) {
        $.ajax({
            type: "POST",
            url: "/Vechile/RemoveVech/" + id,
            success: function (data) {
                if (data.success) {
                    SalTable.ajax.reload();

                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success"
                    })
                }
            }
        })
    }
}