
    $(document).ready(function () {

        $('#assignTeam').hide();
    $('#assignManager').hide();
    $('#unassignManager').hide();
    $('#assignClient').hide();
    $('#unassignClient').hide();

    $('#assignEmployeesButton').click(function () {
            // Check form validity
            if ($('#assignEmployeesForm')[0].checkValidity()) {
        // Show loading spinner or any indicator
        $('#assignTeam').show();

    var selectedEmployees = $('#employees').val();
    var projectId = '@Model.ProjectId';

    $.ajax({
        url: '@Url.Action("AssignEmployees", "Admin")',
    type: 'POST',
    data: {
        projectId: projectId,
    employeeIds: selectedEmployees
                    },
    success: function (response) {
        $('#assignTeam').hide();// hide spinner
    swal("Thank You!", "Team Assigned Successfully!", "success", {
        button: "Ok!",
                        });
                    },
    error: function (xhr, status, error) {
        swal({
            title: "Error!",
            text: "An unexpected error occurred",
            icon: "warning",
            button: "Cancel!",
        });
                    }
                });
            } else {
        // If the form is invalid, add Bootstrap validation class
        $('#assignEmployeesForm').addClass('was-validated');
            }
        });


    $('#assignManagerButton').click(function () {
            var form = $('#assignManagerForm')[0];

    if (form.checkValidity()) {
        $('#assignManager').show();
    var selectedManager = $('#manager').val();
    var projectId = '@Model.ProjectId';

    $.ajax({
        url: '@Url.Action("AssignProjectManager", "Admin")',
    type: 'POST',
    data: {
        projectId: projectId,
    projectManagerId: selectedManager
                    },
    success: function (response) {
        // $('#managerMessage').html('<div class="alert alert-success">Project manager assigned successfully!</div>');
        swal("Thank You!", "Project Manager Assigned Successfully!", "success", {
            button: "Ok!",
        });
    $('#assignManager').hide();
                            setTimeout(() => {
        location.reload(); // Reload the page to reflect changes
                            }, 2000);
                    },
    error: function (xhr, status, error) {
        // $('#managerMessage').html('<div class="alert alert-danger">An error occurred: ' + error + '</div
        swal({
            title: "Error!",
            text: "An unexpected error occurred",
            icon: "warning",
            button: "Cancel!",
        });
    $('#assignManager').hide();
                    }
                });
            } else {
        form.classList.add('was-validated');
            }
        });

    //unassign project manager
    $('#unassignManagerButton').click(function () {
        $('#unassignManager').show();
    var projectId = '@Model.ProjectId';

    $.ajax({
        url: '@Url.Action("UnassignProjectManager", "Admin")', // Ensure this is correct
    type: 'POST',
    data: {
        projectId: projectId
                },
    success: function (response) {
        swal("Thank You!", "Project Manager Unassigned Successfully!", "success", {
            button: "Ok!",
        });
    $('#unassignManager').hide();
                        setTimeout(() => {
        location.reload(); // Reload the page to reflect changes
                        }, 2000);
                },
    error: function (xhr, status, error) {
        swal({
            title: "Error!",
            text: "An unexpected error occurred",
            icon: "warning",
            button: "Cancel!",
        });
    $('#unassignManager').hide();
                }
            });
        });

    //assign project client

    $('#assignClientButton').click(function () {
                var form = $('#assignClientForm')[0];

    if (form.checkValidity()) {
        $('#assignClient').show();
    var selectedClient = $('#client').val();
    var projectId = '@Model.ProjectId';

    $.ajax({
        url: '@Url.Action("AssignProjectClient", "Admin")',
    type: 'POST',
    data: {
        projectId: projectId,
    projectClientId: selectedClient
                    },
    success: function (response) {
        // $('#managerMessage').html('<div class="alert alert-success">Project manager assigned successfully!</div>');
        swal("Thank You!", "Project Client Assigned Successfully!", "success", {
            button: "Ok!",
        });
    $('#assignClient').hide();
                            setTimeout(() => {
        location.reload(); // Reload the page to reflect changes
                            }, 2000);
                    },
    error: function (xhr, status, error) {
        // $('#managerMessage').html('<div class="alert alert-danger">An error occurred: ' + error + '</div
        swal({
            title: "Error!",
            text: "An unexpected error occurred",
            icon: "warning",
            button: "Cancel!",
        });
    $('#assignClient').hide();
                    }
                });
            } else {
        form.classList.add('was-validated');
            }
        });

    //unassign project client
    $('#unassignClientButton').click(function () {
        $('#unassignClient').show();
    var projectId = '@Model.ProjectId';

    $.ajax({
        url: '@Url.Action("UnassignProjectClient", "Admin")', // Ensure this is correct
    type: 'POST',
    data: {
        projectId: projectId
                    },
    success: function (response) {
        swal("Thank You!", "Project Manager Unassigned Successfully!", "success", {
            button: "Ok!",
        });
    $('#unassignClient').hide();
                        setTimeout(() => {
        location.reload(); // Reload the page to reflect changes
                        }, 2000);
                    },
    error: function (xhr, status, error) {
        swal({
            title: "Error!",
            text: "An unexpected error occurred",
            icon: "warning",
            button: "Cancel!",
        });
    $('#unassignClient').hide();
                    }
                });
            });

    });



