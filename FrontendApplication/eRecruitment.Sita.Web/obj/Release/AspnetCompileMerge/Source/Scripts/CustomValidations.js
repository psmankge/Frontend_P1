function generalCheckBoxValidation(checkbox) {
    checkbox = "." + checkbox;
    var check = false;
    $(checkbox).each(function () {
        check = $(this).find('.checkbox > label > input:checkbox').is(":checked");

    });

    return check;
}

function customValidation(event) {

    let today = new Date();
    let mymodal = $('#myModal');

    var idQual = $("#idQual").val();

    
    var controllerAction = window.location.pathname.split('/');

    //========================================Peter - new code, specific field================================================20221017
    if ($("#idQual").val() === "displayQualifError") {
        event.preventDefault();//prevent the form submit
        mymodal.find('.modal-body').text('Qualification should match the profile..');
        mymodal.modal('show');
        $("#BPSVacancyNo").focus();
        /*var accesslevel = '<%= Session("accesslevel").ToString().ToLower() %>*/
    }
    //========================================Peter - new code, specific field================================================20221017

    //========================================Peter - new code, specific field================================================20221017
    //else if ($("#DepartmentID").val() === "") {
    //    event.preventDefault();//prevent the form submit
    //    mymodal.find('.modal-body').text('Department cannot be empty, please make sure all required fields that are marked with red asterik are not empty');
    //    mymodal.modal('show');
    //    $("#DepartmentID").focus();
    //}
    //========================================================================================================================
}

function convertToCommar(rand) {
    var strRand = String(rand);
    return strRand.replace('.', ',');
}

// convert , -> .

function convertToDot(rand) {
    var strRand = String(rand);
    return strRand.replace(',', '.');
}

