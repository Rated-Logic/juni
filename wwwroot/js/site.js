function deleteTodo(i){


    $.ajax({
        url: 'Home/Delete',
        type:'POST',
        data:{
            id : i
        },
        success:function () {
            window.location.reload();
        }
    });
}

function populateform(i){
    $.ajax({
        url:"Home/Populate",
        type:'GET',
        data:{
            id:i
        },
        datatype: 'json',
        success: function(response){
            $("#Todo_Name").val(response.name);
            $("#Todo_Id").val(response.id);
            $("#form-button").val("Update Todo");
            $("#form-action").attr("action","/Home/Update");
        }
    });
}