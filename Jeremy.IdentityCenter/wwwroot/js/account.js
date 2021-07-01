var userId = 0;

$("td>button.btn.btn-danger").on("click",
    function() {
        userId = $(this).data("id");
    });

$("#deleteConfirmBtn").on("click",
    function() {
        if (userId > 0) {
            $.post("/account/delete?id=" + userId,
                null,
                function() {
                    history.go(0);
                });
        } else {
            $("#DeleteUserDialog").modal("hide");
        }
    });
