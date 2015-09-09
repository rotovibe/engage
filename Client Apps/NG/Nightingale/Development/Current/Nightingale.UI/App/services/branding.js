define(['services/session'],
  function (session) {

    var currentBrand = ko.computed(function() {
      var returnValue;
      var brandName = 'Phytel';
      var logoLink = '/NightingaleUI/Content/images/phytel_logo.png';
      var classOverride = 'phytel-logo';
      if (session.currentUser()) {
        if (session.currentUser().contracts()[0].number() === 'Demo001') {
          brandName = 'Watson Health';
          logoLink = '/NightingaleUI/Content/images/watson_cm_logo.png';
          classOverride = 'watson-logo';
        }
      }
      return new brand(brandName, logoLink, classOverride);
    });

    var branding = {
      currentBrand: currentBrand
    };
    return branding;

    function brand(name, logoPath, override) {
      var self = this;
      self.brandName = ko.observable(name);
      self.imageSource = ko.observable(logoPath);
      self.classOverride = ko.observable(override)
    }
});
