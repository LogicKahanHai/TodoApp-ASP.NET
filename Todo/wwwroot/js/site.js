function deleteTodo(i) {
  $.ajax({
    url: "/Home/Delete",
    type: "POST",
    data: { id: i },
    success: function () {
      window.location.reload();
    },
  });
}

function populateForm(i) {
  $.ajax({
    url: "/Home/PopulateForm",
    type: "GET",
    data: { id: i },
    success: function (data) {
      $("#Todo_Id").val(data.id);
      $("#Todo_Name").val(data.name);
      $("#form-button").val("Update Todo");
      $("#form-action").attr("action", "/Home/Update");
    },
  });
}
