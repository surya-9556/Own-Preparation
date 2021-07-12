var PopUp,dataTable;
$(document).ready(function () {
    dataTable = $("#EmployeeTBL").DataTable({
        "ajax": {
            "type": "GET",
            "url": "/Employee/GetEmp",
            "datatype":"json"
        },
        "columns": [
            { "data": "EmployeeName", "autoWidth": true },
            {
                "data": "BirthDate", 'render': function (jsonDate) {
                    var date = new Date(parseInt(jsonDate.substr(6)));
                    var month = ("0" + (date.getMonth() + 1)).slice(-2);
                    return ("0" + date.getDate()).slice(-2) + '-' + month + '-' + date.getFullYear();
                },
                "autoWidth": true
            },
            {
                "data": "HireDate", 'render': function (jsonDate) {
                    var date = new Date(parseInt(jsonDate.substr(6)));
                    var month = ("0" + (date.getMonth() + 1)).slice(-2);
                    return ("0" + date.getDate()).slice(-2) + '-' + month + '-' + date.getFullYear();
                },
                "autoWidth": true
            },
            { "data": "Gender", "autoWidth": true },
            { "data": "MaritalStatus", "autoWidth": true },
            { "data": "Email", "autoWidth": true },
            { "data": "PhoneNumber", "autoWidth": true },
            {
                "data": "EmpId", "render": function (data) {
                    return "<a class='btn btn-default btn-sm' onclick=PopupForm('/Employee/AddOrEdit/" + data + "')><i class='fa fa-pencil-square'></i> Edit</a> <a class='btn btn-danger btn-sm' onclick=Delete(" + data + ")><i class='fa fa-trash'></i> Delete</a>"
                },
                "searchable": false
            }
        ],
        "dom": "Bfrtip",
        "buttons": [
            {
                extend: "copy",
                className: "copyButton",
                text: '<i class = "fa fa-clone"></i> Copy'
            },
            {
                extend: "excel",
                text: '<i class = "fa fa-file-excel"></i> Excel'
            },
            {
                extend: "csv",
                text: '<i class = "fa fa-file-excel"></i> CSV'
            },
            {
                extend: "pdf",
                text: '<i class = "fa fa-file-pdf"></i> PDF'
            },
            {
                extend: "print",
                text: '<i class = "fa fa-print"></i> Print'
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
                dataTable.ajax.reload();

                $.notify(data.message, {
                    globalPosition: "top center",
                    class: "success"
                })
            }
        }
    })
    return false;
}


function Delete(id) {
    if (confirm("You want to conformly delete the record")) {
        $.ajax({
            type: "POST",
            url: "/Employee/RemoveEmp/" + id,
            success: function (data) {
                if (data.success) {
                    dataTable.ajax.reload();

                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success"
                    })
                }
            }
        })
    }
}