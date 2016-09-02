$(document).ready(function () {
    $("input[name=btnBuy]").on('click', null, function (event) {
        $("#errorSummary").hide();
        $("#errorSummary").empty();

        $.ajax(
        {
           dataType: 'JSON',
           url: '/Players/BuyHeroes/',
           type: 'POST',
           contentType: 'application/json',
           data: JSON.stringify({ idPlayer: $("#idPlayer").val(), idHero: $(this).attr('id') }),

           success: function (response) {
               if (response.error == "")
                   $("#errorSummary").append('<div class="alert alert-success"><button type="button" class="close" data-dismiss="alert">×</button><h3>' + response.message + '</h3></div>');

               if (response.error == "Exception")
                   MountModelStateErrorMessage(response.message);

               if (response.error == "ValidationResultError")
                   MountValildationResultError(response.message);

               $("#errorSummary").slideToggle('slow');
           }
       });
    });
});

function MountValildationResultError(message) {
    var div = '<div class="alert alert-danger"><button type="button" class="close" data-dismiss="alert">×</button><h3>Oops! something goes wrong:</h3><ul>';

    for (var i = 0; i < message.Erros.length; i++) {
        div += '<li>' + message.Erros[i].Message + '</li>';
    }

    div += '</ul></div>';
    $("#errorSummary").append(div);
}

function MountModelStateErrorMessage(message) {
    var div = '<div class="alert alert-danger"><button type="button" class="close" data-dismiss="alert">×</button><h3>Oops! something goes wrong:</h3><ul>';
    div += '<li>' + message + '</li>';
   
    div += '</ul></div>';
    $("#errorSummary").append(div);
}