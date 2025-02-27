// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    GetAllEmployees();
});

//Inicializa DataTable
function DataTables() {
    $('#tblEmployees').DataTable();
}   

//Destruye DataTable
function DataTableDestroy() {
    $('#tblEmployees').DataTable().destroy();
}

//Recarga DataTable
function DataTableReload() {
    DataTableDestroy();
    DataTables();
}

function GetAllEmployees() {
    $.ajax({
        type: "GET",
        url: "/Home/GetEmployees",
        success: function (response) {
            let tbody = $('#tblEmployees tbody');
            tbody.empty();

            $.each(response.employees, function (index, employee) {
                tbody.append(
                    `<tr>
                        <td>${employee.id}</td>
                        <td>${employee.nombre}</td>
                        <td>${employee.apellido}</td>
                        <td>${employee.email}</td>
                        <td>${employee.telefono}</td>
                        <td>${employee.salario}</td>
                        <td>${employee.fechaIngreso}</td>
                    </tr>`
                );
            }
            );
            DataTableReload();
        }
    });
}

function AddEmployee() {
    let employee = {
        nombre: $('#txtName').val(),
        apellido: $('#txtLastName').val(),
        email: $('#txtEmail').val(),
        telefono: $('#txtTelefono').val(),
        salario: $('#txtSalary').val(),
        fechaIngreso: $('#txtDate').val()
    };



    $.ajax({
        type: "POST",
        url: "/Home/CreateEmployee",
        data: employee,
        success: function (response) {
            CloseNewEmployeeDialog();
            ClearNewEmployeeDialog();
            GetAllEmployees();
        }
    });
}

function DeleteEmployee(id) {
    $.ajax({
        type: "DELETE",
        url: "/Home/DeleteEmployee/" + id,
        success: function (response) {
            console.log(response);
            GetAllEmployees();
        }
    });
}

function ShowNewEmployeeDialog() {
    $('#dlgNewEmployee').modal('show');
}

function CloseNewEmployeeDialog() {
    $('#dlgNewEmployee').modal('hide');
}

function ClearNewEmployeeDialog() {
    $('#txtName').val('');
    $('#txtLastName').val('');
    $('#txtEmail').val('');
    $('#txtTelefono').val('');
    $('#txtSalary').val('');
    $('#txtDate').val('');
}
