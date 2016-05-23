﻿/// <autosync enabled="true" />
/// <reference path="../weyland-config.js" />
/// <reference path="../app/config.app.js" />
/// <reference path="../app/config.routes.js" />
/// <reference path="../app/config.services.js" />
/// <reference path="../sandbox/index.js" />
/// <reference path="../app/main-built.js" />
/// <reference path="accordion.js" />
/// <reference path="almond-custom.js" />
/// <reference path="breeze.ajaxpost.js" />
/// <reference path="breeze.ajaxpost2.js" />
/// <reference path="breeze.ajaxpost3.js" />
/// <reference path="breeze.js" />
/// <reference path="es5-sham.min.js" />
/// <reference path="es5-shim.min.js" />
/// <reference path="fullcalendar.js" />
/// <reference path="knockout-3.2.0.js" />
/// <reference path="modernzr.js" />
/// <reference path="moment-with-langs.js" />
/// <reference path="moment.js" />
/// <reference path="q.js" />
/// <reference path="r.js" />
/// <reference path="require.js" />
/// <reference path="text.js" />
/// <reference path="typeahead-backup.js" />
/// <reference path="typeahead.js" />
/// <reference path="../app/models/allergies.js" />
/// <reference path="../app/models/base.js" />
/// <reference path="../app/models/contacts.js" />
/// <reference path="../app/models/controls.js" />
/// <reference path="../app/models/goals.js" />
/// <reference path="../app/models/lookups.js" />
/// <reference path="../app/models/medications.js" />
/// <reference path="../app/models/notes.js" />
/// <reference path="../app/models/observations.js" />
/// <reference path="../app/models/programs.js" />
/// <reference path="../app/models/users.js" />
/// <reference path="../app/viewmodels/authenticate.js" />
/// <reference path="chosen/chosen.jquery.min.js" />
/// <reference path="chosen/chosen.jquerybackup.js" />
/// <reference path="durandal/activator.js" />
/// <reference path="durandal/app.js" />
/// <reference path="durandal/binder.js" />
/// <reference path="durandal/composition.js" />
/// <reference path="durandal/events.js" />
/// <reference path="durandal/system.js" />
/// <reference path="durandal/viewengine.js" />
/// <reference path="durandal/viewlocator.js" />
/// <reference path="highcharts/highcharts-more.js" />
/// <reference path="highcharts/highcharts.js" />
/// <reference path="jquery/jquery-1.10.2.js" />
/// <reference path="jqueryui/jquery-ui.js" />
/// <reference path="../app/viewmodels/admin/index.js" />
/// <reference path="../app/viewmodels/home/index.js" />
/// <reference path="../app/viewmodels/insight/index.js" />
/// <reference path="../app/viewmodels/patients/index.js" />
/// <reference path="../app/viewmodels/populations/index.js" />
/// <reference path="../app/viewmodels/programdesigner/index.js" />
/// <reference path="../app/viewmodels/shell/shell.js" />
/// <reference path="../app/viewmodels/shell/sidebar.js" />
/// <reference path="../app/viewmodels/templates/action.edit.js" />
/// <reference path="../app/viewmodels/templates/allergies.panel.js" />
/// <reference path="../app/viewmodels/templates/allergy.edit.js" />
/// <reference path="../app/viewmodels/templates/barrier.edit.js" />
/// <reference path="../app/viewmodels/templates/care.team.edit.js" />
/// <reference path="../app/viewmodels/templates/clinical.dataentry.js" />
/// <reference path="../app/viewmodels/templates/event.details.js" />
/// <reference path="../app/viewmodels/templates/focusproblems.js" />
/// <reference path="../app/viewmodels/templates/goal.edit.js" />
/// <reference path="../app/viewmodels/templates/intervention.edit.js" />
/// <reference path="../app/viewmodels/templates/intervention.panel.js" />
/// <reference path="../app/viewmodels/templates/medications.panel.js" />
/// <reference path="../app/viewmodels/templates/module.edit.js" />
/// <reference path="../app/viewmodels/templates/observation.add.js" />
/// <reference path="../app/viewmodels/templates/observation.bloodpressure.add.js" />
/// <reference path="../app/viewmodels/templates/patient.systems.js" />
/// <reference path="../app/viewmodels/templates/program.edit.js" />
/// <reference path="../app/viewmodels/templates/program.remove.js" />
/// <reference path="../app/viewmodels/templates/task.edit.js" />
/// <reference path="../app/viewmodels/templates/task.panel.js" />
/// <reference path="../app/viewmodels/templates/todo.edit.js" />
/// <reference path="../app/viewmodels/templates/todo.panel.js" />
/// <reference path="../app/viewmodels/venn/patient.venn.js" />
/// <reference path="../app/widgets/chsnmultiple/viewmodel.js" />
/// <reference path="../app/widgets/chsnsingle/viewmodel.js" />
/// <reference path="../app/widgets/chsnsingledark/viewmodel.js" />
/// <reference path="../app/widgets/multiselect/viewmodel.js" />
/// <reference path="../app/widgets/singleselect/viewmodel.js" />
/// <reference path="durandal/plugins/dialog.js" />
/// <reference path="durandal/plugins/history.js" />
/// <reference path="durandal/plugins/http.js" />
/// <reference path="durandal/plugins/observable.js" />
/// <reference path="durandal/plugins/router.js" />
/// <reference path="durandal/plugins/serializer.js" />
/// <reference path="durandal/plugins/widget.js" />
/// <reference path="durandal/transitions/entrance.js" />
/// <reference path="jquery/olderversion/jquery-1.9.1.js" />
/// <reference path="jqueryui/olderversion/jquery-ui-1.10.3.js" />
/// <reference path="../app/viewmodels/admin/concierge/index.js" />
/// <reference path="../app/viewmodels/admin/widgets/delete.individuals.list.js" />
/// <reference path="../app/viewmodels/home/myhome/myhome.js" />
/// <reference path="../app/viewmodels/home/population/index.js" />
/// <reference path="../app/viewmodels/home/todos/index.js" />
/// <reference path="../app/viewmodels/patients/careplan/index.js" />
/// <reference path="../app/viewmodels/patients/data/index.js" />
/// <reference path="../app/viewmodels/patients/goals/index.js" />
/// <reference path="../app/viewmodels/patients/history/index.js" />
/// <reference path="../app/viewmodels/patients/medications/index.js" />
/// <reference path="../app/viewmodels/patients/overview/index.js" />
/// <reference path="../app/viewmodels/patients/sections/additional.observations.js" />
/// <reference path="../app/viewmodels/patients/sections/additional.problem.observations.js" />
/// <reference path="../app/viewmodels/patients/sections/allergies.edit.js" />
/// <reference path="../app/viewmodels/patients/sections/allergies.search.js" />
/// <reference path="../app/viewmodels/patients/sections/allergy.details.js" />
/// <reference path="../app/viewmodels/patients/sections/background.js" />
/// <reference path="../app/viewmodels/patients/sections/barrier.details.js" />
/// <reference path="../app/viewmodels/patients/sections/basic.observations.js" />
/// <reference path="../app/viewmodels/patients/sections/care.team.js" />
/// <reference path="../app/viewmodels/patients/sections/communications.js" />
/// <reference path="../app/viewmodels/patients/sections/data.list.js" />
/// <reference path="../app/viewmodels/patients/sections/demographics.js" />
/// <reference path="../app/viewmodels/patients/sections/focus.areas.js" />
/// <reference path="../app/viewmodels/patients/sections/goal.details.js" />
/// <reference path="../app/viewmodels/patients/sections/goals.js" />
/// <reference path="../app/viewmodels/patients/sections/intervention.details.js" />
/// <reference path="../app/viewmodels/patients/sections/medication.details.js" />
/// <reference path="../app/viewmodels/patients/sections/medication.edit.js" />
/// <reference path="../app/viewmodels/patients/sections/medications.js" />
/// <reference path="../app/viewmodels/patients/sections/medications.search.js" />
/// <reference path="../app/viewmodels/patients/sections/notes.js" />
/// <reference path="../app/viewmodels/patients/sections/problems.list.js" />
/// <reference path="../app/viewmodels/patients/sections/programs.js" />
/// <reference path="../app/viewmodels/patients/sections/recent.individuals.js" />
/// <reference path="../app/viewmodels/patients/sections/system.ids.js" />
/// <reference path="../app/viewmodels/patients/sections/task.details.js" />
/// <reference path="../app/viewmodels/patients/tabs/action.details.js" />
/// <reference path="../app/viewmodels/patients/tabs/action.steps.js" />
/// <reference path="../app/viewmodels/patients/tabs/module.details.js" />
/// <reference path="../app/viewmodels/patients/tabs/module.objectives.js" />
/// <reference path="../app/viewmodels/patients/tabs/program.details.js" />
/// <reference path="../app/viewmodels/patients/tabs/program.goals.js" />
/// <reference path="../app/viewmodels/patients/tabs/program.objectives.js" />
/// <reference path="../app/viewmodels/patients/widgets/action.details.js" />
/// <reference path="../app/viewmodels/patients/widgets/allergies.js" />
/// <reference path="../app/viewmodels/patients/widgets/medications.js" />
/// <reference path="../app/viewmodels/patients/widgets/module.details.js" />
/// <reference path="../app/viewmodels/patients/widgets/program.details.js" />
/// <reference path="../app/viewmodels/shell/quickadd/assign.js" />
/// <reference path="../app/viewmodels/shell/quickadd/notes.js" />
/// <reference path="../app/viewmodels/shell/quickadd/quickaddpopover.js" />
/// <reference path="../app/viewmodels/shell/quickadd/todo.js" />
/// <reference path="chosen/abstract-chosen.js" />
/// <reference path="../app/services/dataservices/allergiesservice.js" />
/// <reference path="../app/services/dataservices/calendarservice.js" />
/// <reference path="../app/services/dataservices/caremembersservice.js" />
/// <reference path="../app/services/dataservices/contactservice.js" />
/// <reference path="../app/services/dataservices/getentityservice.js" />
/// <reference path="../app/services/dataservices/goalsservice.js" />
/// <reference path="../app/services/dataservices/lookupsservice.js" />
/// <reference path="../app/services/dataservices/medicationsservice.js" />
/// <reference path="../app/services/dataservices/notesservice.js" />
/// <reference path="../app/services/dataservices/observationsservice.js" />
/// <reference path="../app/services/dataservices/patientsservice.js" />
/// <reference path="../app/services/dataservices/programsservice.js" />
/// <reference path="../app/services/dataservices/stepservice.js" />
/// <reference path="../app/services/dataservices/usersservice.js" />
/// <reference path="../app/services/analytics.js" />
/// <reference path="../app/services/bindings.js" />
/// <reference path="../app/services/branding.js" />
/// <reference path="../app/services/customvalidators.js" />
/// <reference path="../app/services/datacontext.js" />
/// <reference path="../app/services/entityfinder.js" />
/// <reference path="../app/services/entityserializer.js" />
/// <reference path="../app/services/jsonresultsadapter.js" />
/// <reference path="../app/services/local.collections.js" />
/// <reference path="../app/services/logger.js" />
/// <reference path="../app/services/navigation.js" />
/// <reference path="../app/services/report.context.js" />
/// <reference path="../app/services/session.js" />
/// <reference path="../app/services/usercontext.js" />
/// <reference path="../app/services/validatorfactory.js" />
/// <reference path="../app/main.js" />
/// <reference path="../App/viewmodels/patients/sections/dropdown.w.addnew.js" />
/// <reference path="jquery.timepicker.min.js" />
/// <reference path="../app/services/datehelper.js" />
/// <reference path="../app/services/formatter.js" />
/// <reference path="../app/viewmodels/patients/sections/clinicalbackground.js" />
/// <reference path="../app/viewmodels/templates/datetimepicker.js" />
/// <reference path="../app/viewmodels/templates/note.general.edit.js" />
/// <reference path="../app/viewmodels/templates/note.touchpoint.edit.js" />
/// <reference path="../app/viewmodels/templates/patient.status.js" />
/// <reference path="../app/viewmodels/patients/sections/status.js" />
/// <reference path="../app/viewmodels/templates/note.utilization.edit.js" />
/// <reference path="../app/viewmodels/patients/notes/index.js" />
/// <reference path="../copyright.js" />
/// <reference path="../gulpfile.js" />
/// <reference path="../app/viewmodels/home/contacts/index.js" />
/// <reference path="../app/viewmodels/templates/contact.edit.js" />
/// <reference path="../app/viewmodels/home/contacts/contact.details.js" />
/// <reference path="../app/viewmodels/patients/team/caremember.details.js" />
/// <reference path="../app/viewmodels/patients/team/index.js" />
/// <reference path="../app/viewmodels/patients/widgets/caremembers.js" />
/// <reference path="../app/viewmodels/templates/caremembers.panel.js" />
/// <reference path="../app/viewmodels/templates/caremember.edit.js" />

 
 / / /   < r e f e r e n c e   p a t h = " b r e e z e . d e b u g . j s "   / > 
 
 