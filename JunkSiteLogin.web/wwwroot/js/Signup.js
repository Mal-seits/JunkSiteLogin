$(() => {

    function IsFormValid() {
        let name = $("#name").val();
        let email = $("#email").val();
        let password = $("#password").val();
        let isValid = name && email && password;
        $("#signup-btn").prop('disabled', !isValid);

    }

    $("#name").on('input', function () {
        IsFormValid();
    });

    $("#email").on('input', function () {
        IsFormValid();
    });
    $("#password").on('input', function () {
        IsFormValid();
    });
  
  
})