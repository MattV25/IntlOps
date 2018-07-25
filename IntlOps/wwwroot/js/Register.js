var currentTab = 0;
showTab(currentTab); 

function showTab(n) {
    var x = document.getElementsByClassName("tab");
    x[n].style.display = "block";
    if (n == 0) {
        document.getElementById("prevBtn").style.display = "none";
    } else {
        document.getElementById("prevBtn").style.display = "inline";
    }
    if (n == (x.length - 1)) {
        document.getElementById("nextBtn").innerHTML = "Submit";
    } else {
        document.getElementById("nextBtn").innerHTML = "Next";
    }
    fixStepIndicator(n)
}
function nextPrev(n) {
    var x = document.getElementsByClassName("tab");
    if (n == 1 && !validateForm()) return false;
    x[currentTab].style.display = "none";
    currentTab = currentTab + n;
    if (currentTab >= x.length) {
        //document.getElementById("regform").submit();
        return false;
    }
    showTab(currentTab);
}
function validateForm() {
    var x, y, i, valid = true;
    x = document.getElementsByClassName("tab");
    y = x[currentTab].getElementsByTagName("input");
    for (i = 0; i < y.length; i++) {
        if (y[i].value == "") {
            y[i].className += " invalid";
            valid = false;
        }
    }
    if (valid) {
        document.getElementsByClassName("step")[currentTab].className += " finish";
    }
    return valid; 
}
function fixStepIndicator(n) {
    var i, x = document.getElementsByClassName("step");
    for (i = 0; i < x.length; i++) {
        x[i].className = x[i].className.replace(" active", "");
    }
    x[n].className += " active";
}
//-------------------------------Set the Datepicker options--------------------------------//
$(function () {
        $(".birthdate").datepicker({
            showButtonPanel: true,
            changeMonth: true,
            changeYear: true,
            showOtherMonths: true,
            selectOtherMonths: true,
            yearRange: "1930:2020"
        });
});
//--------------------------------------End Datepicker-------------------------------------//

$('select').on('change', function () {
    $("#regform").validate().element('select');
});
    var $validator = $("#regform").validate({
        errorPlacement: function errorPlacement(error, element) {
            element.before(error);
            error.addClass("help-block");
            element.parents(".col-sm-5").addClass("has-feedback");
            if (element.prop("type") === "checkbox") {
                error.insertAfter(element.parent("label"));
            } else {
                error.insertAfter(element);
            }
            if (!element.next("span")[0]) {
                $("<span class='glyphicon glyphicon-remove form-control-feedback'></span>").insertAfter(element);
            }
        },
        rules: {
            Firstname: 'required',
            Lastname: 'required',
            UserName: 'required',
            Password: 'required',
            ConfirmPassword: 'required',
            Birthdate: 'required',
            Gender: 'required',
            MaritalStatus: 'required',
            JobTitle: 'required',
            JobType: 'required',
            Email: 'required',
            PhoneNumber: 'required',
            AccountName: 'required',
            Street1: 'required',
            Street2: 'required',
            City: 'required',
            State: 'required',
            Zipcode: 'required'
        },
        messages: {
            Firstname: "First name is required.",
            Lastname: "Last name is required.",
            UserName: 'User name is required.',
            Password: 'Password is required.',
            ConfirmPassword: 'Confirm password is required.',
            Birthdate: 'Birthdate is required.',
            Gender: 'Gender is required.',
            MaritalStatus: 'Marital status is required.',
            JobTitle: 'Job title is required.',
            JobType: 'Job type is required.',
            Email: 'Email is required.',
            PhoneNumber: 'Phone number is required.',
            AccountName: 'Company name is required.',
            Street1: 'Street Line 1 is required.',
            Street2: 'Street Line 2 is required.',
            City: 'City is required.',
            State: 'State is required.',
            Zipcode: 'Zipcode is required.'
        },
        success: function (label, element) {
            if (!$(element).next("span")[0]) {
                $("<span class='glyphicon glyphicon-ok form-control-feedback'></span>").insertAfter($(element));
            }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).parents(".col-sm-5").addClass("has-error").removeClass("has-success");
            $(element).next("span").addClass("glyphicon-remove").removeClass("glyphicon-ok");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).parents(".col-sm-5").addClass("has-success").removeClass("has-error");
            $(element).next("span").addClass("glyphicon-ok").removeClass("glyphicon-remove");
        }
    });
    $("#regform").bootstrapWizard({
        tabClass: 'arrows',
        onNext: function (tab, navigation, index) {
            if (index == 1) {
                var $valid = $("#regform").valid();
                if (!$valid) {
                    $validator.focusInvalid();
                    return false;
                }
            }
            return false;
        },
    });