define(['config.models', 'services/datacontext', 'viewmodels/shell/shell'],
    function (modelConfig, datacontext, shell) {
        
        var ctor = function () {

        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.contactCard = settings.contactCard;
            self.communicationModalShowing = ko.observable(false);
            // Primary communication modes for this patient
            self.primaryCommunications = ko.computed(function () {
                // Get the primary communication types and return them
                var communications = [];
                var contactcard = self.contactCard();
                if (contactcard) {
                    var prefPhone = ko.utils.arrayFirst(contactcard.phones(), function (phone) {
                        return phone.phonePreferred();
                    });
                    if (prefPhone) {
                        prefPhone.template = 'templates/phone.html';
                        communications.push(prefPhone);
                    }
                    var prefText = ko.utils.arrayFirst(contactcard.phones(), function (phone) {
                        return phone.textPreferred();
                    });
                    if (prefText && prefText !== prefPhone) {
                        prefText.template = 'templates/phone.html';
                        communications.push(prefText);
                    }
                    var prefEmail = ko.utils.arrayFirst(contactcard.emails(), function (email) {
                        return email.preferred();
                    });
                    if (prefEmail) {
                        prefEmail.template = 'templates/email.html';
                        communications.push(prefEmail);
                    }
                    var prefAddress = ko.utils.arrayFirst(contactcard.addresses(), function (address) {
                        return address.preferred();
                    });
                    if (prefAddress) {
                        prefAddress.template = 'templates/address.html';
                        communications.push(prefAddress);
                    }
                    //var thisContactLanguage = ko.utils.arrayFirst(contactcard.languages(), function (language) {
                    //    return language.preferred();
                    //});
                    //if (thisContactLanguage) {
                    //    var prefLanguage =  ko.utils.arrayFirst(datacontext.enums.languages(), function (enumLanguage) {
                    //        return enumLanguage.id() === thisContactLanguage.lookUpLanguageId();
                    //    });
                    //    prefLanguage.template = 'templates/language.html';
                    //    communications.push(prefLanguage);
                    //}
                }
                // Return the list of preferred communications
                return communications;
            });
            // Secondary communication modes for this patient
            self.secondaryCommunications = ko.computed(function () {
                // Get the secondary communication types and return them
                var communications = [];
                var contactcard = self.contactCard();
                if (contactcard) {
                    // Add each phone,
                    if (!contactcard.phoneOptedOut()) {
                        ko.utils.arrayForEach(contactcard.phones(), function (phone) {
                            // As long as it isn't primary already
                            if (!phone.phonePreferred() && !phone.textPreferred() && !phone.optOut()) {
                                phone.template = 'templates/phone.html';
                                communications.push(phone);
                            }
                        });
                    }
                    if (contactcard.phoneOptedOut() && !contactcard.textOptedOut()) {
                        ko.utils.arrayForEach(contactcard.phones(), function (phone) {
                            // As long as it isn't primary already
                            if (!phone.textPreferred() && phone.isText() && !phone.optOut) {
                                phone.template = 'templates/phone.html';
                                communications.push(phone);
                            }
                        });
                    }
                    if (!contactcard.emailOptedOut()) {
                        ko.utils.arrayForEach(contactcard.emails(), function (email) {
                            if (!email.preferred() && !email.optOut()) {
                                email.template = 'templates/email.html';
                                communications.push(email);
                            }
                        });
                    }
                    if (!contactcard.addressOptedOut()) {
                        ko.utils.arrayForEach(contactcard.addresses(), function (address) {
                            if (!address.preferred() && !address.optOut()) {
                                address.template = 'templates/address.html';
                                communications.push(address);
                            }
                        });
                    }
                }
                // Return the list of preferred communications
                return communications;
            });
            // The modal to display for editing
            self.modal = new modelConfig.modal('Edit Communication Preferences', self.contactCard, 'templates/contactcard.html', self.communicationModalShowing);
            self.isOpen = ko.observable(true);
            // Why do we need this on the contactCard and in this view model?
            // TODO: I deleted it from the model but I think we can reduce either this one
            // or the communicationModalShowing one.
            self.isEditing = ko.observable(false);
            self.toggleEditing = function () {
                if (self.isEditing()) {
                    self.communicationModalShowing(false);
                }
                else {
                    shell.currentModal(self.modal);
                    self.communicationModalShowing(true);
                    var editingToken = self.communicationModalShowing.subscribe(function () {
                        self.isEditing(false);
                        editingToken.dispose();
                    });
                }
                self.isEditing(!self.isEditing());
                self.isOpen(true);
            };
        };
                
        ctor.prototype.attached = function () {

        };

        return ctor;
    });