//CRUD Operations
$('.createBtn').on('click', function () {
    $('.modal-content').load('_Create.cshtml', function () {
        $('#createModal').modal({ show: true });
    });
});
$('.detailsBtn').on('click', function () {
    $('.modal-content').load('_Details.cshtml', function () {
        $('#detailsModal').modal({ show: true });
    });
});
$('.editBtn').on('click', function () {
    $('.modal-content').load('_Edit.cshtml', function () {
        $('#editModal').modal({ show: true });
    });
});
$('.deleteBtn').on('click', function () {
    $('.modal-content').load('_Delete.cshtml', function () {
        $('#deleteModal').modal({ show: true });
    });
});

//Role Management
$('.createRoleBtn').on('click', function () {
    $('.modal-content').load('CreateRole.cshtml', function () {
        $('#createRoleModal').modal({ show: true });
    });
});
$('.editRoleBtn').on('click', function () {
    $('.modal-content').load('EditRole.cshtml', function () {
        $('#editRoleModal').modal({ show: true });
    });
});
$('.deleteRoleBtn').on('click', function () {
    $('.modal-content').load('DeleteRole.cshtml', function () {
        $('#deleteRoleModal').modal({ show: true });
    });
});