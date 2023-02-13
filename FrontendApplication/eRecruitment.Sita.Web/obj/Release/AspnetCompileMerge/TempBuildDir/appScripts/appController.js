"use strict";
angular.module('mainApp', []).controller('mainCtrl', ['$scope', function ($scope) {

    //Education Variables
    $scope.EducationListData = [];
    $scope.EducationDataForEdit = [];
    $scope.QualificationTypeList = [];
    $scope.EducationCRUDMode = "";
    //End Education Variables

    $scope.provinceID = "";
    $scope.VacancyID = "";

    //Reference Variables
    $scope.ReferenceListData = []; //Gets all record per profile id
    $scope.ReferenceDataForEdit = []; //Gets record for edit by reference id
    $scope.ReferenceCRUDMode = ""; //controlling variable for add, edit and load
    //End Reference Variables

    //Skills Variables
    $scope.SkillsListData = [];
    $scope.SkillsTypeList = [];
    $scope.SkillProficiencyList = [];
    $scope.SkillsCRUDMode = "";
    //End Skills Variables

    //Attachment Variables
    $scope.AttachmentListData = [];
    $scope.AttachmentCRUDMode = "";
    //End Attachment Variables

    //Candidate Screening Virables
    $scope.GetCandidateScreeningVacancy = [];
    $scope.ScreenedCandidateData = [];
    $scope.CandidateListData = [];
    $scope.GenderList = [];
    $scope.RaceList = [];
    //End Candidate Screening Virables

    //Date Variables
    $scope.DaysListData = [];
    $scope.MonthListData = [];
    $scope.YearListData = [];
    //End Date Variables

    //Languages Variables
    $scope.LanguagesListData = [];
    $scope.LanguagesTypeList = [];
    $scope.LanguagesProficiencyList = [];
    $scope.LanguagesCRUDMode = "";
    //End Languages Variables

    //WorkHistory Variables
    $scope.WorkHistoryListData = [];
    $scope.WorkHistoryDataForEdit = [];
    $scope.WorkHistoryCRUDMode = "";
    //End WorkHistory Variables


    $scope.SystemErr = "";
    $scope.EmploymentType = [];
    $scope.StatusData = [];
    $scope.ProvinceData = [];

    $scope.VacancyQuestionBank = [];
    $scope.hasCurrentWork;

    $scope.GetStatusTest = function () {
        alert("I am here");
    };

    $scope.AddWorkHistory = function () {
        $scope.WorkHistoryCRUDMode = "Add";
        $scope.$apply();
    }

    $scope.AddLanguages = function () {
        $scope.LanguagesCRUDMode = "Add";
        $scope.$apply();
    }

    $scope.AddReference = function () {
        $scope.ReferenceCRUDMode = "Add";
        $scope.$apply();
    };

    $scope.AddSkills = function () {
        $scope.SkillsCRUDMode = "Add";
        $scope.$apply();
    }

    $scope.AddEducation = function () {
        $scope.EducationCRUDMode = "Add";
        $scope.$apply();
    };

    $scope.AddAttachment = function () {
        $scope.AttachmentCRUDMode = "Add";
        $scope.$apply();
    };

    $scope.GetVacancyQuestionBank = function () {
        var Info = {"VID": $('#VacancyID').val()}

        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            data: { list: JSON.stringify(Info) },
            url: 'GetVacancyQuestionBank',
            success: function (data) {
                if (data.error) {
                    $scope.VacancyQuestionBank = "";
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                }
                else {
                    $scope.VacancyQuestionBank = data;
                    $scope.$apply();

                }
            }

        });
    }

    $scope.GetScreeningVacancyList = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetCandidateScreeningVacancy',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                }
                else {
                    $scope.GetCandidateScreeningVacancy = data;
                    $scope.$apply();

                }
            }

        });
    }

    $scope.GetLanguagesList = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetLanguagesList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                }
                else {
                    $scope.LanguagesTypeList = data;
                    $scope.$apply();

                }
            }

        });
        /*var xhr = new XMLHttpRequest();
        xhr.open('GET', 'GetLanguagesList');
        xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
        xhr.onload = function() {
            if (xhr.status === 200) {
                $scope.LanguagesTypeList = JSON.parse(this.responseText);
                $scope.$apply();
            }
            else {
                console.log('Error: ', JSON.parse(this.responseText));
                $scope.SystemErr = JSON.parse(this.responseText.error);
                $scope.$apply()
            }
        };
        xhr.send();*/
    }

    $scope.GetCandidateLanguagesData = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetCandidateLanguagesList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                }
                else {
                    $scope.LanguagesListData = data;
                    $scope.LanguagesCRUDMode = "Load";
                    $scope.$apply();

                }
            }
        

        });
       /* var xhr = new XMLHttpRequest();
        xhr.open('GET', 'GetCandidateLanguagesList');
        xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
        xhr.onload = function () {
            if (xhr.status === 200) {
                $scope.LanguagesTypeList = JSON.parse(this.responseText);
                $scope.$apply();
            }
            else {
                console.log('Error: ', JSON.parse(this.responseText));
                $scope.SystemErr = JSON.parse(this.responseText.error);
                $scope.$apply()
            }
        };
        xhr.send();*/
    }

    $scope.GetLanguageProficiencyList = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetLanguageProficiencyList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                }
                else {
                    $scope.LanguagesProficiencyList = data;
                    $scope.$apply();

                }
            }

        });
    } 

    $scope.AddLanguagesInfo = function () {
        var LanguageID = $('#LanguageID').val();
        var LanguageProficiencyID = $('#LanguageProficiencyID').val();
        var mymodal = $('#myModal');

        if (LanguageID == null || LanguageID == '') {
            mymodal.find('.modal-body').text('Please select language Type!');
            mymodal.modal('show');
            return;
        }

        if (LanguageProficiencyID == null || LanguageProficiencyID == '') {
            mymodal.find('.modal-body').text('Please select language Proficiency Type!');
            mymodal.modal('show');
            return;
        }

        var LanguageIDInfo = {
            "LanguageID": LanguageID,
            "LanguageProficiencyID": LanguageProficiencyID
        }

        $.ajax({
            url: "AddLanguage",
            type: "POST",
            data: { list: JSON.stringify(LanguageIDInfo) },
            datatype: "json",
            success: function (data) {
                if (data == "Warning") {
                    mymodal.find('.modal-body').text('Record already Exist');
                    mymodal.modal('show');
                    //return;
                }
                $scope.GetCandidateLanguagesData();
                $scope.LanguagesCRUDMode = "Load";
                $scope.$apply();
                console.log(data);
            }
        });
    }

    $scope.AddSkillsInfo = function () {

        var skillID = $('#skillID').val();
        var SkillProficiencyID = $('#SkillProficiencyID').val();
        var mymodal = $('#myModal');

        if (skillID == null || skillID == '') {
            mymodal.find('.modal-body').text('Please select skill Type!');
            mymodal.modal('show');
            return;
        }

        if (SkillProficiencyID == null || SkillProficiencyID == '') {
            mymodal.find('.modal-body').text('Please select Skill Proficiency Type!');
            mymodal.modal('show');
            return;
        }

        var SkillsInfo = {
            "skillID": skillID,
            "SkillProficiencyID": SkillProficiencyID

        }

        $.ajax({
            url: "AddSkills",
            type: "POST",
            data: { list: JSON.stringify(SkillsInfo) },
            datatype: "json",
            success: function (data) {
                if (data == "Warning") {
                    mymodal.find('.modal-body').text('Record already Exist');
                    mymodal.modal('show');
                    //return;
                }
                $scope.GetCandidateSkillsData();
                $scope.SkillsCRUDMode = "Load";
                $scope.$apply();
                console.log(data);
            }
        });
    }

    $scope.GetSkillProficiencyList = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'SkillProficiencyList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                    //$("#divLoader").hide();
                }
                else {
                    //alert("back with data");
                    $scope.SkillProficiencyList = data;
                    $scope.$apply();

                }
            }

        });
    }

    $scope.GetSkillsTypeList = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'SkillsTypeList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                    //$("#divLoader").hide();
                }
                else {
                    //alert("back with data");
                    $scope.SkillsTypeList = data;
                    $scope.$apply();

                }
            }

        });
    }

    $scope.GetStatusList = function () {
      
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetStatusList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply();
                    //$("#divLoader").hide();
                }
                else {
                    //alert("back with data");
                    $scope.StatusData = data;
                    $scope.$apply();

                }
            }

        });
    };

    $scope.GetCandidateReferenceData = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetCandidateReferenceList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply();
                    //$("#divLoader").hide();
                }
                else {
                    $scope.ReferenceListData = data;
                    $scope.ReferenceCRUDMode = "Load";
                    $scope.$apply();

                }
            }

        });
    };

    $scope.ExportCandidateDataToExcel = function () {
        alert($('#VacancyID').val());
        alert("Hello");
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'ExportToExcel' + '?id=' + $('#VacancyID').val(),
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply();
                    //$("#divLoader").hide();
                }
                else {
                    $scope.CandidateListData = data;
                    $scope.$apply();

                }
            }

        });
    };

    $scope.GetCandidateEducationData = function () {
        //alert("Here");
         var url = '@Url.Action("GetCandidateEducationList")';
        //var url = '../Candidate/GetCandidateEducationList';
        $.ajax({
            type: 'GET',
            url: 'GetCandidateEducationList',
            contentType: 'application/json; charset=utf-8',
            dataType:"json",
            success: function (data) {
                //alert("back with data");
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply();
                    //$("#divLoader").hide();
                }
                else {
                    $scope.EducationListData = data;
                    $scope.EducationCRUDMode = "Load";
                    $scope.$apply();


                }
            }

        });
    };

    $scope.GetCandidateSkillsData = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetCandidateSkillsList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                    //$("#divLoader").hide();
                }
                else {
                    //alert("load");
                    $scope.SkillsListData = data;
                    $scope.SkillsCRUDMode = "Load";
                    $scope.$apply();
                }
            }

        });
    }

    $scope.GetCandidateAttachmentData = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetCandidateAttachmentList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                    //$("#divLoader").hide();
                }
                else {
                    $scope.AttachmentListData = data;
                    $scope.AttachmentCRUDMode = "Load";
                    $scope.$apply();

                }
            }

        });
    }

    $scope.GetScreenedCandidateList = function () {
        var vacancyID = $('#VacancyID').val();
        var provinceID = $('#ProvinceID').val();
        var ageRange = $('#AgeRange').val();
        var genderID = $('#GenderID').val();
        var raceID = $('#RaceID').val();
        var disability = $('#chkWithDisabilities').val();
        var cvAttached = $('#chkWithAttachedCV').val();
        var idAttached = $('#chkWithAttachedID').val();

        alert(disability);
        var mymodal = $('#myModal');

        if (vacancyID == null || vacancyID == '')
        {
            mymodal.find('.modal-body').text('Please select Vacancy Name First!');
            mymodal.modal('show');
            return;
        }
        if (provinceID == null || provinceID == '') {
            mymodal.find('.modal-body').text('Please select Province Name First!');
            mymodal.modal('show');
            return;
        }
        var Info = {
            "VID": vacancyID
            , "PID": provinceID
            , "ARange": ageRange
            , "GID": genderID
            , "RID": raceID
            , "Disability": disability
            , "CVAttached": cvAttached
            , "IDAttached": idAttached
        }

        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'ScreenedCandidateList',
            data: { list: JSON.stringify(Info) },
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                    //$("#divLoader").hide();
                }
                else {
                    $scope.GetCandidateScreeningVacancy = data;
                    $scope.GetProvinceList();
                    $scope.GetScreeningVacancyList();
                    console.log(data);
                    $scope.provinceID = data[0].provinceID;
                    $scope.VacancyID = data[0].VacancyID;
                    console.log(data[0].VacancyID);
                    //$('#startDate').val(data[0].startDate);
                    //var sd = data[0].startDate;
                    //$scope.startDate = new Date(sd);
                    //$scope.endDate = new Date(data[0].endDate);
                    //$scope.EducationCRUDMode = "Edit";
                    $scope.$apply();

                }
            }

        });
    };

    $scope.GetCandidateEducationForEdit = function (d) {
        //alert(d);
        var QualIDInfo = { "QID": d }

        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetCandidateEducationForEdit',
            data: { list: JSON.stringify(QualIDInfo) },
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                    //$("#divLoader").hide();
                }
                else {
                    $scope.EducationDataForEdit = data;
                    $('#startDate').val(data[0].startDate);
                    var convertedSd = moment(data[0].startDate).utcOffset(0, true).format('MM/DD/YYYY');
                    var convertedEd = moment(data[0].endDate).utcOffset(0, true).format('MM/DD/YYYY');
                    var sd = data[0].startDate;
                    $scope.startDate = convertedSd;
                    $scope.endDate = convertedEd;
                    $scope.EducationCRUDMode = "Edit";
                    $scope.$apply();

                }
            }

        });
    };

    $scope.GetCandidateReferenceForEdit = function (d) {

        var RefIDInfo = { "RID": d }

        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetCandidateReferenceForEdit',
            data: { list: JSON.stringify(RefIDInfo) },
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                    //$("#divLoader").hide();
                }
                else {
                    $scope.ReferenceDataForEdit = data;
                    $scope.ReferenceCRUDMode = "Edit";
                    $scope.$apply();

                }
            }

        });
    };

    $scope.GetEmploymentTypeList = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'EmploymentType',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                    //$("#divLoader").hide();
                }
                else {
                    //alert("back with data");
                    $scope.EmploymentType = data;
                    $scope.$apply();

                }
            }

        });
    };

    $scope.AddWorkHistoryInfo = function () {
        var companyName = $('#companyName').val();
        var jobTitle = $('#jobTitle').val();
        var positionHeld = $('#positionHeld').val();
        var departmentName = $('#department').val();
        var startDate = new Date($('#startDateQual').val());
        var endDate = new Date($('#endDateQual').val());
        var reasonForLeaving = $('#reasonForLeaving').val();
        var previouslyEmployedPS = $('#previouslyEmployedPS').val();
        var reEmployment = $('#reEmployment').val();
        var previouslyEmployedDepartment = $('#previouslyEmployedDepartment').val();

        var today = new Date();
        var mymodal = $('#myModal');



        if (companyName == null || companyName == '') {
            mymodal.find('.modal-body').text('Company Name cannot be null!');
            mymodal.modal('show');
            return;
        }

        if (jobTitle == null || jobTitle == '') {
            mymodal.find('.modal-body').text('Job Title cannot be null!');
            mymodal.modal('show');
            return;
        }

      

        if (startDate === undefined || startDate == "Invalid Date") {
            mymodal.find('.modal-body').text('Please select valid start date!');
            mymodal.modal('show');
            return;
        }

        if (startDate === null || startDate === '') {
            mymodal.find('.modal-body').text('Start Date cannot be empty!');
            mymodal.modal('show');
            return;
        }

        if (startDate > today) {
            mymodal.find('.modal-body').text('Start Date cannot be in the future!');
            mymodal.modal('show');
            return;
        }

        if (endDate == null || endDate == '') {
            mymodal.find('.modal-body').text('End Date cannot be empty!');
            mymodal.modal('show');
            return;
        }


        if (startDate > endDate) {
            mymodal.find('.modal-body').text('Start Date cannot be after End Date!');
            mymodal.modal('show');
            return;
        }

        if (endDate > today) {
            mymodal.find('.modal-body').text('End Date cannot be in the future!');
            mymodal.modal('show');
            return;
        }

        //if (reasonForLeaving == null || reasonForLeaving == '') {
        //    mymodal.find('.modal-body').text('Reason For Leaving cannot be empty!');
        //    mymodal.modal('show');
        //    return;
        //}

        //if (previouslyEmployedPS == null || previouslyEmployedPS == '') {
        //    mymodal.find('.modal-body').text('Previously Employed By Public Service cannot be empty!');
        //    mymodal.modal('show');
        //    return;
        //}


        var WorkInfo = {
            "companyName": companyName,
            "jobTitle": jobTitle,
            "positionHeld": positionHeld,
            "department": departmentName,
            "startdate": moment(startDate).add(1, 'days'),
            "enddate": Date.parse(endDate) ? moment(endDate).add(1, 'days') : null,
            "reasonForLeaving": reasonForLeaving,
            "previouslyEmployedPS": previouslyEmployedPS,
            "reEmployment": reEmployment,
            "previouslyEmployedDepartment": previouslyEmployedDepartment
        }


        $.ajax({
            url: "AddWorkHistory",
            type: "POST",
            data: { list: JSON.stringify(WorkInfo) },
            datatype: "json",
            success: function (data) {
                $scope.GetCandidateWorkHistoryData();
                $scope.WorkHistoryCRUDMode = "Load";
                $scope.$apply();
                console.log(data);
                alert(data)
            }
        });
    }

    $scope.AddEducationInfo = function (d) {
        var instName = $('#institutionName').val();
        var qualName = $('#qualificationName').val();
        var OtherName = $('#OtherName').val();
        var qTypeID = $('#QualificationTypeName').val();
        var certNumber = $('#certificateNumber').val();
        var startDate = new Date($('#startDate').val());
        var endDate = new Date($('#endDate').val());

        //alert(endDate);

        var mymodal = $('#myModal');
        var today = new Date();

        if (instName == null || instName == '') {
            mymodal.find('.modal-body').text('Institution Name cannot be null!');
            mymodal.modal('show');
            return;
        }

        if (qualName == null || qualName == '') {
            mymodal.find('.modal-body').text('Qualification Name cannot be null!');
            mymodal.modal('show');
            return;
        }

        if (qTypeID == null || qTypeID == '') {
            mymodal.find('.modal-body').text('Please select Qualification Type!');
            mymodal.modal('show');
            return;
        }


        if (startDate === undefined || startDate == "Invalid Date") {
            mymodal.find('.modal-body').text('Please select valid start date!');
            mymodal.modal('show');
            return;
        }



        if (startDate == null || startDate == '') {
            mymodal.find('.modal-body').text('Qualification Start Date cannot be empty!');
            mymodal.modal('show');
            return;
        }

        if (startDate > today) {
            mymodal.find('.modal-body').text('Qualification Start Date cannot be in the future!');
            mymodal.modal('show');
            return;
        }

        if (endDate == null || endDate == '') {
            mymodal.find('.modal-body').text('Qualification End Date cannot be empty!');
            mymodal.modal('show');
            return;
        }

        //if (endDate > today) {
        //    mymodal.find('.modal-body').text('Qualification End Date cannot be in the future!');
        //    mymodal.modal('show');
        //    return;
        //}

        if (startDate > endDate) {
            mymodal.find('.modal-body').text('Qualification Start Date cannot be after End Date!');
            mymodal.modal('show');
            return;
        }

        var EduInfo = {
            "IName": instName,
            "QName": qualName,
            "OName": OtherName,
            "QTypeID": qTypeID,
            "CNumber": certNumber,
            "startdate": moment(startDate).add(1, 'days'),
            "enddate": moment(endDate).add(1, 'days')
        }

        $.ajax({
            //url: "../Candidate/AddEducation",
            type: "POST",
            url: 'AddEducation',
            data: { list: JSON.stringify(EduInfo) },
            datatype: "json",
            success: function (data) {
                $scope.GetCandidateEducationData();
                $scope.EducationCRUDMode = "Load";
                $scope.$apply();
                console.log(data);
                alert(data)
            }
        });
    };

    $scope.AddReferenceInfo = function () {
        var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
        var refName = $('#txtrefName').val();
        var companyName = $('#txtcompanyName').val();
        var positionHeld = $('#txtpositionHeld').val();
        var telNo = $('#txttelNo').val();
        var emailAddress = $('#txtemailAddress').val();
        var mymodal = $('#myModal');

        if (refName === null || refName === '') {
            mymodal.find('.modal-body').text('Full name cannot be null!');
            mymodal.modal('show');
            return;
        }

        if (companyName === null || companyName === '') {
            mymodal.find('.modal-body').text('Company Name cannot be null!');
            mymodal.modal('show');
            return;
        }

        //if (positionHeld === null || positionHeld === '') {
        //    mymodal.find('.modal-body').text('Position Held cannot be null!');
        //    mymodal.modal('show');
        //    return;
        //}

        if (isNaN(telNo)) {
            mymodal.find('.modal-body').text('Please enter numeric Telephone number!');
            mymodal.modal('show');
            return;
        }

        if (telNo === null || telNo === '') {
            mymodal.find('.modal-body').text('Telephone number cannot be null!');
            mymodal.modal('show');
            return;
        }

        if (emailAddress !== null && emailAddress !== '')
        {
            if (!emailAddress.match(mailformat)) {
                mymodal.find('.modal-body').text('Please enter a valid email address!');
                mymodal.modal('show');
                return;
            }
        }
 
        /*if (emailAddress === null || emailAddress === '') {
            mymodal.find('.modal-body').text('Email Address cannot be empty!');
            mymodal.modal('show');
            return;
        }*/

        var RefInfo = {
            "refName": refName,
            "companyName": companyName,
            "positionHeld": positionHeld,
            "telNo": telNo,
            "emailAddress": emailAddress
        }

        $.ajax({
            url: "AddReferenceInfo",
            type: "POST",
            data: { list: JSON.stringify(RefInfo) },
            datatype: "json",
            success: function (data) {
                $scope.GetCandidateReferenceData();
                $scope.ReferenceCRUDMode = "Load";
                $scope.$apply();
                console.log(data);
                alert(data)
            }
        });
    };

    $scope.UpdateWorkHistoryInfo = function (d) {

        var CompanyName = $('#companyNameEdit').val();
        var JobTitle = $('#jobTitleEdit').val();
        var PositionHeld = $('#positionHeldEdit').val();
        var Department = $('#departmentEdit').val();
        var StartDate = new Date($('#startDateEdit').val());
        var EndDate = new Date($('#endDateEdit').val());
        var ReasonForLeaving = $('#reasonForLeavingEdit').val();
        var PreviouslyEmployedPS = $('#previouslyEmployedPSEdit').val();
        var ReEmployment = $('#reEmploymentEdit').val();
        var PreviouslyEmployedDepartment = $('#previouslyEmployedDepartmentEdit').val();

        var today = new Date();
        var mymodal = $('#myModal');

        if (CompanyName == null || CompanyName == '') {
            mymodal.find('.modal-body').text('Company Name cannot be null!');
            mymodal.modal('show');
            return;
        }

        if (JobTitle == null || JobTitle == '') {
            mymodal.find('.modal-body').text('Job Title cannot be null!');
            mymodal.modal('show');
            return;
        }

     

        if (StartDate == null || StartDate == '') {
            mymodal.find('.modal-body').text('Start Date cannot be empty!');
            mymodal.modal('show');
            return;
        }

        if (StartDate > today) {
            mymodal.find('.modal-body').text('Start Date cannot be in the future!');
            mymodal.modal('show');
            return;
        }

        if (EndDate == null || EndDate == '') {
            mymodal.find('.modal-body').text('End Date cannot be empty!');
            mymodal.modal('show');
            return;
        }

        if (EndDate > today) {
            mymodal.find('.modal-body').text('End Date cannot be in the future!');
            mymodal.modal('show');
            return;
        }

        if (StartDate > EndDate) {
            mymodal.find('.modal-body').text('Start Date cannot be after End Date!');
            mymodal.modal('show');
            return;
        }
        //if (ReasonForLeaving == null || ReasonForLeaving == '') {
        //    mymodal.find('.modal-body').text('Reason For Leaving cannot be empty!');
        //    mymodal.modal('show');
        //    return;
        //}

        //if (PreviouslyEmployedPS == null || PreviouslyEmployedPS == '') {
        //    mymodal.find('.modal-body').text('Previously Employed By Public Service cannot be empty!');
        //    mymodal.modal('show');
        //    return;
        //}
        
        var WorkInfo = {
            "WID": d,
            "companyName": CompanyName,
            "jobTitle": JobTitle,
            "positionHeld": PositionHeld,
            "department": Department,
            "startdate": StartDate,
            "enddate": Date.parse(EndDate) ? EndDate : null,
            "reasonForLeaving": ReasonForLeaving,
            "previouslyEmployedPS": PreviouslyEmployedPS,
            "reEmployment": ReEmployment,
            "previouslyEmployedDepartment": PreviouslyEmployedDepartment
        }
        //alert('I am here');
       

        $.ajax({
            url: "UpdateWorkHistoryInfo",
            type: "POST",
            data: { list: JSON.stringify(WorkInfo) },
            datatype: "json",
            success: function (data) {
               /* alert('I am back with data');*/
                alert(data);
                console.log(data);
                $scope.GetCandidateWorkHistoryData();
                $scope.WorkHistoryCRUDMode = "Load";
                $scope.$apply();
            


            }
        });
    }

    $scope.GetCandidateWorkHistoryForEdit = function (d) {
        $scope.current = false;
        var WorkIDInfo = { "WID": d }
        $scope.showCurrent = false;
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetCandidateWorkHistoryForEdit',
            data: { list: JSON.stringify(WorkIDInfo) },
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                }
                else {
                    $scope.WorkHistoryDataForEdit = data;
                    var sd = data[0].startDate;
                    var convertedSt = moment(sd).utcOffset(0, true).format('MM/DD/YYYY');
                    var convertedED = ''; // moment(data[0].endDate).utcOffset(0, true).format('MM/DD/YYYY');

                    // yyyy/mm/dd = localDB and yyyy-mm-dd = citDB
                    if (data[0].endDate == '0001/01/01' || data[0].endDate == '0001-01-01' ) {
                        $scope.current = true;
                        convertedED = null;
                        $scope.showCurrent = true;
                    } else {
                        convertedED = moment(data[0].endDate).utcOffset(0, true).format('MM/DD/YYYY');
                    }
                    $scope.startDate = convertedSt;
                    $scope.endDate = convertedED;
                    $scope.WorkHistoryCRUDMode = "Edit";
                    $scope.WorkHistoryDataForEdit[0].previouslyEmployedPS = data[0].previouslyEmployedPS;   
                    $scope.$apply();

                }
            }

        });
    }

    $scope.GetCandidateWorkHistoryData = function () {
        $scope.hasCurrentWork = false;
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetCandidateWorkHistoryList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                }
                else {
                    
                    $scope.WorkHistoryListData = data;
                    for (var i = 0; i < data.length; i++) {
                        if (data[i] && (data[i]['endDate'] == '0001/01/01' || data[i]['endDate'] == '0001-01-01' || data[i]['endDate'] == '1/1/0001' || data[i]['endDate'] == '01/01/0001')) {
                            $scope.hasCurrentWork = true;
                        }
                    }
                    $scope.WorkHistoryCRUDMode = "Load";
                    $scope.$apply();

                }
            }

        });
    }

    $scope.GetQualificationTypeList = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetQualificationTypeList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                    //$("#divLoader").hide();
                }
                else {
                    //alert("back with data");
                    $scope.QualificationTypeList = data;
                    $scope.$apply();

                }
            }

        });
    };

    $scope.GetDayListData = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetDaysList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                    //$("#divLoader").hide();
                }
                else {
                    $scope.DaysListData = data;
                    $scope.$apply();

                }
            }

        });
    };

    $scope.GetMonthListData = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetMonthList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                    //$("#divLoader").hide();
                }
                else {
                    $scope.MonthListData = data;
                    $scope.$apply();

                }
            }

        });
    };

    $scope.GetYearListData = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetYearList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                    //$("#divLoader").hide();
                }
                else {
                    $scope.YearListData = data;
                    $scope.$apply();

                }
            }

        });
    };

    $scope.UpdateEducation = function (d) {
        //alert("In");
        var instName = $('#institutionNameEdit').val();
        var qualName = $('#qualificationNameEdit').val();
        var OtherName = $('#OtherQualificationNameEdit').val();
        var qTypeID = $('#QualificationTypeNameEdit').val();
        var certNumber = $('#certificateNumberEdit').val();
        var startDate = new Date($('#startDateEdit').val());
        var endDate = new Date($('#endDateEdit').val());

        var mymodal = $('#myModal');
        var today = new Date();

        if (instName == null || instName == '') {
            mymodal.find('.modal-body').text('Institution Name cannot be null!');
            mymodal.modal('show');
            return;
        }

        if (qualName == null || qualName == '') {
            mymodal.find('.modal-body').text('Qualification Name cannot be null!');
            mymodal.modal('show');
            return;
        }

        if (qTypeID == null || qTypeID == '') {
            mymodal.find('.modal-body').text('Please select Qualification Type!');
            mymodal.modal('show');
            return;
        }

       

        if (startDate == null || startDate == '') {
            mymodal.find('.modal-body').text('Qualification Start Date cannot be empty!');
            mymodal.modal('show');
            return;
        }

        if (startDate > today) {
            mymodal.find('.modal-body').text('Qualification Start Date cannot be in the future!');
            mymodal.modal('show');
            return;
        }

        if (endDate == null || endDate == '') {
            mymodal.find('.modal-body').text('Qualification End Date cannot be empty!');
            mymodal.modal('show');
            return;
        }

       

        if (startDate > endDate) {
            mymodal.find('.modal-body').text('Qualification Start Date cannot be after End Date!');
            mymodal.modal('show');
            return;
        }

        var EduInfo = {
            "QID": d,
            "IName": instName,
            "QName": qualName,
            "OName": OtherName,
            "QTypeID": qTypeID,
            "CNumber": certNumber,
            "startdate": startDate,
            "enddate": endDate
        }

        $.ajax({
            url: 'UpdateEducation',
            type: "POST",
            data: { list: JSON.stringify(EduInfo) },
            datatype: "json",
            success: function (data) {
                alert(data);
                $scope.GetCandidateEducationData();
                $scope.EducationCRUDMode = "Load";
                $scope.$apply();
            }
        });
    };

    $scope.DownloadAttachment = function (d) {
        alert("here");
        alert(d);
        var AInfo = {
            "AID": d
        }

        $.ajax({
            url: "DownloadAttachment",
            type: "POST",
            data: { list: JSON.stringify(AInfo) },
            datatype: "json",
            success: function (data) {
                //$scope.GetCandidateEducationData();
                //$scope.EducationCRUDMode = "Load";
                //$scope.$apply();
            }
        });
    };

    $scope.UpdateReferenceInfo = function (d) {
        var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
        var refName = $('#txtrefName').val();
        var companyName = $('#txtcompanyName').val();
        var positionHeld = $('#txtpositionHeld').val();
        var telNo = $('#txttelNo').val();
        var emailAddress = $('#txtemailAddress').val();
        var mymodal = $('#myModal');

        if (refName === null || refName === '') {
            mymodal.find('.modal-body').text('Full name cannot be null!');
            mymodal.modal('show');
            return;
        }

        if (companyName === null || companyName === '') {
            mymodal.find('.modal-body').text('Company Name cannot be null!');
            mymodal.modal('show');
            return;
        }

        //if (positionHeld === null || positionHeld === '') {
        //    mymodal.find('.modal-body').text('Position Held cannot be null!');
        //    mymodal.modal('show');
        //    return;
        //}

        if (isNaN(telNo)) {
            mymodal.find('.modal-body').text('Please enter numeric Telephone number!');
            mymodal.modal('show');
            return;
        }

        if (telNo === null || telNo === '') {
            mymodal.find('.modal-body').text('Telephone number cannot be null!');
            mymodal.modal('show');
            return;
        }

        if (emailAddress !== null && emailAddress !== '')
        {
            if (!emailAddress.match(mailformat)) {
                mymodal.find('.modal-body').text('Please enter a valid email address!');
                mymodal.modal('show');
                return;
            }
        }


        //if (emailAddress === null || emailAddress === '') {
        //    mymodal.find('.modal-body').text('Email Address cannot be empty!');
        //    mymodal.modal('show');
        //    return;
        //}

        var RefInfo = {
            "RID": d,
            "refName": refName,
            "companyName": companyName,
            "positionHeld": positionHeld,
            "telNo": telNo,
            "emailAddress": emailAddress
        }

        $.ajax({
            url: "UpdateReferenceInfo",
            type: "POST",
            data: { list: JSON.stringify(RefInfo) },
            datatype: "json",
            success: function (data) {
                $scope.GetCandidateReferenceData();
                $scope.ReferenceCRUDMode = "Load";
                $scope.$apply();
            }
        });
    };

    $scope.DeleteCandidateReference = function (d) {

        if (d === null || d === '') {
            var mymodal = $('#myModal');
            mymodal.find('.modal-body').text('No Reference No specified!');
            mymodal.modal('show');
            return;
        }

        var RefInfo = { "RID": d }

        var r = confirm("Are you sure you want to delete the selected record?");
        if (r === true) {
            $.ajax({
                url: "DeleteReferenceInfo",
                type: "POST",
                data: { list: JSON.stringify(RefInfo) },
                datatype: "json",
                success: function (data) {
                    $scope.GetCandidateReferenceData();
                    $scope.ReferenceCRUDMode = "Load";
                    $scope.$apply();
                }
            });
        } else {
            //txt = "You pressed Cancel!";
        }


    };

    $scope.DeleteCandidateEducation = function (d) {
        var mymodal = $('#myModal');
        if (d === null || d === '') {
            mymodal.find('.modal-body').text('No Qualification ID specified!');
            mymodal.modal('show');
            return;
        }

        var RefInfo = { "QID": d }

        var r = confirm("Are you sure you want to delete the selected record?");
        if (r === true) {
            $.ajax({
                url: "DeleteEducationInfo",
                type: "POST",
                data: { list: JSON.stringify(RefInfo) },
                datatype: "json",
                success: function (data) {
                    $scope.GetCandidateEducationData();
                    $scope.EducationCRUDMode = "Load";
                    $scope.$apply();
                }
            });
        } else {
            //txt = "You pressed Cancel!";
        }


    };

    $scope.DeleteCandidateWorkHistory = function (d) {

        var mymodal = $('#myModal');

        if (d === null || d === '') {
            mymodal.find('.modal-body').text('No Qualification ID specified!');
            mymodal.modal('show');
            return;
        }

        var RefInfo = { "QID": d }

        var r = confirm("Are you sure you want to delete the selected record?");

        if (r === true) {
            $.ajax({
                url: "DeleteCandidateWorkHistory",
                type: "POST",
                data: { list: JSON.stringify(RefInfo) },
                datatype: "json",
                success: function (data) {
                    
                    $scope.WorkHistoryListData();
                    $scope.WorkHistoryCRUDMode = "Load";
                    $scope.$apply();
                }
            });
        } else {
            //txt = "You pressed Cancel!";
        }


    };

    $scope.DeleteSkillsByID = function (d) {

        var mymodal = $('#myModal');

        if (d === null || d === '') {
            mymodal.find('.modal-body').text('No Skills ID specified!');
            mymodal.modal('show');
            return;
        }

        var RefInfo = { "QID": d }

        var r = confirm("Are you sure you want to delete the selected record?");

        if (r === true) {
            $.ajax({
                url: "DeleteCandidateSkillsByID",
                type: "POST",
                data: { list: JSON.stringify(RefInfo) },
                datatype: "json",
                success: function (data) {
                    $scope.GetCandidateSkillsData();
                    $scope.SkillsCRUDMode = "Load";
                    $scope.$apply();
                }
            });
        } else {
            //txt = "You pressed Cancel!";
        }


    };

    $scope.DeleteLanguageByID = function (d) {

        var mymodal = $('#myModal');

        if (d === null || d === '') {
            mymodal.find('.modal-body').text('No Language ID specified!');
            mymodal.modal('show');
            return;
        }

        var RefInfo = { "QID": d }

        var r = confirm("Are you sure you want to delete the selected record?");

        if (r === true) {
            $.ajax({
                url: "DeleteCandidateLanguageByID",
                type: "POST",
                data: { list: JSON.stringify(RefInfo) },
                datatype: "json",
                success: function (data) {
                    $scope.GetCandidateLanguagesData();
                    $scope.LanguagesCRUDMode = "Load";
                    $scope.$apply();
                }
            });
        } else {
            //txt = "You pressed Cancel!";
        }


    };
    
    $scope.GetProvinceList = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            //url: '/Home/GetMenuList' + '?mid=' + $('#LanguageID').val(),
            url: 'GetProvinceList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                }
                else {
                    $scope.ProvinceData = data;
                    $scope.$apply();
                }

            }

        });
    };

    $scope.GetHomeContentData = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            //url: '/Home/GetMenuList' + '?mid=' + $('#LanguageID').val(),
            url: 'GetHomeContent' + '?ProvCD=' + $('#LanguageID').val(),
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                }
                else {
                    $scope.HomeContentData = data;
                    $scope.$apply();
                }

            }

        });
    };

    $scope.GetChildHeadedHouseHold = function () {
        //alert($('#ProvinceID').val());
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetChildHeadedHouseHold' + '?ProvCD=' + $('#ProvinceID').val(),
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                }
                else {
                    $scope.ChildHeadedTotals = data;
                    $scope.$apply();
                }

            }

        });
    };

    $scope.GetMunicipalityByProvince = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetMunicipalityByProvince' + '?ProvCD=' + $('#ProvinceID').val(),
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                }
                else {
                    $scope.MunicData = data;
                    $scope.$apply();
                    $("#MunicipalityID").val($scope.MunicData[0].MunicipalityID);
                    $scope.TownData = "";
                }

            }

        });
    };

    $scope.GetTownByMunicipality = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetTownByMunicipality' + '?TownID=' + $('#MunicipalityID').val(),
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                }
                else {
                    $scope.TownData = data;
                    $scope.$apply();
                    $("#TownID").val($scope.TownData[0].TownId);
                }

            }

        });
    };

    function displayMessage(elem, message, timeout) {
        $(elem).show().html('<div class="alert"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button><span>' + message + '</span></div>');
        if (timeout || timeout === 0) {
            setTimeout(function () {
                $(elem).alert('close');
            }, timeout);
        }
    };

    $scope.GetGenderList = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetGenderList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                }
                else {
                    $scope.GenderList = data;
                    $scope.$apply();

                }
            }

        });
    }

    $scope.GetRaceList = function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: 'GetRaceList',
            success: function (data) {
                if (data.error) {
                    console.log('Error: ', data);
                    $scope.SystemErr = data.error;
                    $scope.$apply()
                }
                else {
                    $scope.RaceList = data;
                    $scope.$apply();

                }
            }

        });
    }

    $('.uploadMyProfileForm').submit(_ => {
        alert("submiting");
    })

}]);