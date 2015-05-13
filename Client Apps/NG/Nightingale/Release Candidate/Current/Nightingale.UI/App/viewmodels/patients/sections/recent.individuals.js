define(['services/session', 'config.services', 'services/datacontext', 'viewmodels/patients/index'], function (session, servicesConfig, datacontext, patientsIndex) {
	
	var choosePatient = patientsIndex.choosePatient;
	var selectedPatient = ko.computed(patientsIndex.selectedPatient);
	var recentIndividualsList = ko.observableArray();

    function activate (settings) {
    }

    var recentIndividuals = {
        activate: activate,
        choosePatient: choosePatient,
        recentIndividualsList: recentIndividualsList,
        selectedPatient: selectedPatient,
        currentUser: session.currentUser
    };
    return recentIndividuals;

});