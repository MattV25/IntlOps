$(document).ready(function () {
    /*
    $('#regform').bootstrapValidator({
        container: '#messages',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        excluded: [':disabled'],
        fields: {
            Firstname: {
                validators: {
                    notEmpty: {
                        message: 'First name is required.'
                    }
                }
            },
            Lastname: {
                validators: {
                    notEmpty: {
                        message: 'Last name is required.'
                    }
                }
            },
            UserName: {
                validators: {
                    notEmpty: {
                        message: 'The username is required.'
                    }
                }
            },
            Password: {
                validators: {
                    notEmpty: {
                        message: 'The password is required.'
                    }
                }
            },
            ConfirmPassword: {
                validators: {
                    notEmpty: {
                        message: 'Confirm password is required.'
                    }
                }
            },
            Birthdate: {
                validators: {
                    notEmpty: {
                        message: 'Birthdate is required.'
                    }
                }
            },
            Gender: {
                validators: {
                    notEmpty: {
                        message: 'Gender is required.'
                    }
                }
            },
            MaritalStatus: {
                validators: {
                    notEmpty: {
                        message: 'Marital status is required.'
                    }
                }
            },
            JobTitle: {
                validators: {
                    notEmpty: {
                        message: 'The job title is required.'
                    }
                }
            },
            JobType: {
                validators: {
                    notEmpty: {
                        message: 'The job type is required.'
                    }
                }
            },
            Email: {
                validators: {
                    notEmpty: {
                        message: 'The email is required.'
                    }
                }
            },
            PhoneNumber: {
                validators: {
                    notEmpty: {
                        message: 'The phone number is required.'
                    }
                }
            },
            AccountName: {
                validators: {
                    notEmpty: {
                        message: 'The account name is required.'
                    }
                }
            },
            Street1: {
                validators: {
                    notEmpty: {
                        message: 'The first street is required.'
                    }
                }
            },
            Street2: {
                validators: {
                    notEmpty: {
                        message: 'The second street is required.'
                    }
                }
            },
            City: {
                validators: {
                    notEmpty: {
                        message: 'The city is required.'
                    }
                }
            },
            State: {
                validators: {
                    notEmpty: {
                        message: 'The state is required.'
                    }
                }
            },
            Zipcode: {
                validators: {
                    notEmpty: {
                        message: 'The zipcode is required.'
                    }
                }
            }
        }
    });
    */
    $('#loginform').bootstrapValidator({
        container: '#messages',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            UserName: {
                validators: {
                    notEmpty: {
                        message: 'Email is required.'
                    }
                }
            },
            Password: {
                validators: {
                    notEmpty: {
                        message: 'Password is required.'
                    }
                }
            }
        }
    });
});
