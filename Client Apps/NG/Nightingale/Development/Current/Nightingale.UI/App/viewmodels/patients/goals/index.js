define(['plugins/router', 'services/navigation', 'config.services', 'models/base', 'services/session', 'services/datacontext', 'viewmodels/patients/index', 'viewmodels/shell/shell'],
  function (router, navigation, servicesConfig, modelConfig, session, datacontext, patientsIndex, shell) {

    var subscriptionTokens= [];

    var selectedPatient = ko.computed(function () {
      return patientsIndex.selectedPatient();
    });

    var endDateSort = function (a,b) { var x = a.endDate(); var y = b.endDate(); if (x == y) { return 0; } if (isNaN(x) || x > y) { return -1; } if (isNaN(y) || x < y) { return 1; }}

    var navToken = navigation.currentRoute.subscribe(function () {
      if (selectedPatient()) {
        ko.utils.arrayForEach(selectedPatient().goals(), function (thisGoal) {
          // if it has an attribute called isExpanded,
          if(thisGoal.isExpanded) {
            thisGoal.isExpanded(false);
            thisGoal.isOpen(false);
          }
        });
      }
    });
    subscriptionTokens.push(navToken);

    var widgets = ko.observableArray();
    var activeGoal = ko.observable();
    var sliderOpen = ko.observable(true);
    var initialized = false;
    var isComposed = ko.observable(true);
    var isEditing = ko.observable(false);

    var newGoal = ko.observable();
    var goalModalShowing = ko.observable(true);

    var goalsEndPoint = ko.computed(function() {
      return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'goal', 'Goal');
    });

    var goalEndPoint = ko.computed(function () {
        return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Goal');
    });

    // var openWidget = ko.observable('Goals');

    function widget(data, column) {
      var self = this;
      self.name = ko.observable(data.name);
      self.path = ko.observable(data.path);
      self.column = column;
      self.isFullScreen = ko.observable(false);
      self.statusIds = data.statusIds;
      self.canAdd = ko.observable(data.canadd);
      self.emptyMesssage = data.emptymessage;
      self.sortOverride = data.sortoverride;
      self.isOpen = ko.observable(data.open);
    }

    function Column(name, open, widgets) {
      var self = this;
      self.name = ko.observable(name);
      self.widgets = ko.observableArray();
      self.isOpen = ko.observable(open).extend({ notify: 'always' });;
      $.each(widgets, function (index, item) {
        self.widgets.push(new widget(item, self))
      });
    }

    var columns = ko.observableArray([
      new Column('goals', true, [{ name: 'Active Goals', path: 'patients/widgets/goals.html', statusIds: ['1','3'], canadd: true, open: true }, { name: 'Goal History', path: 'patients/widgets/goals.html', statusIds: ['2','4'], canadd: false, open: false, emptymessage: 'No history available.', sortoverride: endDateSort }]),
    ]);

    var vm = {
      activate: activate,
      attached: attached,
      deactivate: deactivate,
      detached: detached,
      activeGoal: activeGoal,
      isEditing: isEditing,
      isComposed: isComposed,
      sliderOpen: sliderOpen,
      columns: columns,
      widgets: widgets,
      selectedPatient: selectedPatient,
      cancelChanges: cancelChanges,
      addGoal: addGoal,
      editGoal: editGoal,
      title: 'Goals',
      getGoalDetails: getGoalDetails,
      toggleWidgetOpen: toggleWidgetOpen
    };

    return vm;

    function editGoal(goal, msg) {
      var modalEntity = ko.observable(new ModalEntity(goal));
      var saveOverride = function () {        
		modalEntity().goal.checkAppend();		
        datacontext.saveGoal(modalEntity().goal);
      };
      var cancelOverride = function () {
          var goalCancel = modalEntity().goal;
          goalCancel.entityAspect.rejectChanges();
          getGoalDetails(goalCancel, true);
      };
      msg = msg ? msg : 'Edit Goal';
	  var modalSettings = {
			title: msg,
			showSelectedPatientInTitle: true,
			entity: modalEntity, 
			templatePath: 'viewmodels/templates/goal.edit', 
			showing: goalModalShowing, 
			saveOverride: saveOverride, 
			cancelOverride: cancelOverride, 
			deleteOverride: null, 
			classOverride: null
		}
      var modal = new modelConfig.modal(modalSettings);
      goalModalShowing(true);
      shell.currentModal(modal);
    }

    // Toggle whether the widget is open or not
    function toggleWidgetOpen(sender) {
        // Find how many widgets are open
        var openwidgets = ko.utils.arrayFilter(sender.column.widgets(), function (wid) {
            return wid.isOpen();
        });
        // If the widget is the only open widget
        if (openwidgets.length === 1 && openwidgets[0] === sender) {
            // Do nothing
        } else {
            sender.isOpen(!sender.isOpen());
        }
    }

    function addGoal() {
      //closeSlider();
      datacontext.initializeEntity(newGoal, 'Goal', selectedPatient().id()).then(goalReturned);
      //isEditing(true);

      function goalReturned(data) {
        var thisGoal = data.results[0];
        thisGoal.statusId(1);
        // In case we need to override the save or cancel methods
        thisGoal.startDate(new Date(moment().format('MM/DD/YYYY')));
        thisGoal.isNew(true);
        thisGoal.patientId(selectedPatient().id());
        editGoal(thisGoal, 'Add Goal');
      }
    };

    function ModalEntity(goal) {
        var self = this;
        self.goal = goal;
        // Object containing parameters to pass to the modal
        self.activationData = { goal: self.goal };
        // Create a computed property to subscribe to all of
        // the patients' observations and make sure they are
        // valid
        //self.canSave = ko.observable(true);
        self.canSave = ko.computed(function () {
            // The active goal needs a name and a status to save
            return (self.goal.name() && self.goal.sourceId());
        });
    }

    // Cancel changes to the active goal
    function cancelChanges() {
      // Set a local instance of the goal for performance
      var thisGoal = activeGoal();
      // Reject changes to the entity
      thisGoal.entityAspect.rejectChanges();
      // Reject changes to each task, barrier, and intervention
      ko.utils.arrayForEach(thisGoal.tasks(), function (task) {
        task.entityAspect.rejectChanges();
      });
      ko.utils.arrayForEach(thisGoal.barriers(), function (barrier) {
        barrier.entityAspect.rejectChanges();
      });
      ko.utils.arrayForEach(thisGoal.interventions(), function (intervention) {
        intervention.entityAspect.rejectChanges();
      });
    }

    // Stupid hack - remove when we update goals page
    function resizeSlider() {
      sliderOpen(false);
      setTimeout(function () { sliderOpen(false); }, 1500);      
    }

    function activate() {
      isComposed(false);
      // Set a local instance of selectedPatient equal to the injected patient
      var spToken = selectedPatient.subscribe(function (newValue) {
        // Clear out the active showing action column
        activeGoal(null);
        sliderOpen(false);
        isEditing(false);
      });
      subscriptionTokens.push(spToken);
    }

    function attached() {
      if (!initialized) {
        initialized = true;
      }
      isComposed(true);
    }

    function deactivate() {
      isComposed(false);
    }

    function detached() {
      // isComposed(false);
      // selectedPatient.dispose();
      // goalsEndPoint.dispose();
      // goalEndPoint.dispose();
      // ko.utils.arrayForEach(subscriptionTokens, function (token) {
      //     token.dispose();
      // });
    }

    function getGoalsForSelectedPatient() {
      datacontext.getEntityList(null, goalsEndPoint().EntityType, goalsEndPoint().ResourcePath, null, null, false);
    }

    function getGoalDetails (goal, forceReload) {
      var goalId = goal.id();
      var patientId = goal.patientId();
      // Only force remote if it hasn't been loaded yet
      var alreadyLoaded = forceReload ? true : !goal.isLoaded;
      datacontext.getEntityById(null, goalId, goalEndPoint().EntityType, goalEndPoint().ResourcePath + patientId + '/Goal/', alreadyLoaded).then(goalHasLoaded);

      function goalHasLoaded () {
        goal.isLoaded = true;
      }
    };

  });