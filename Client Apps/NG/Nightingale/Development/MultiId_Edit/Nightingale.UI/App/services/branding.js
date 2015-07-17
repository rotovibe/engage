define([],
    function () {

        var currentBrand = ko.observable(new brand('Phytel', '/NightingaleUI/Content/images/phytel_logo.png'));

        var branding = {
            currentBrand: currentBrand
        };
        return branding;

        function brand(name, logoPath) {
            var self = this;
            self.brandName = ko.observable(name);
            self.imageSource = ko.observable(logoPath);
        }
    });