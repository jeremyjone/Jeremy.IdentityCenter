var roleId = 0;

$("td>button.btn.btn-danger").on("click",
    function () {
        roleId = $(this).data("id");
    });

$("#deleteConfirmBtn").on("click",
    function () {
        if (roleId) {
            $.post("/role/delete?id=" + roleId,
                null,
                function () {
                    history.go(0);
                });
        } else {
            $("#DeleteRoleDialog").modal("hide");
        }
    });
