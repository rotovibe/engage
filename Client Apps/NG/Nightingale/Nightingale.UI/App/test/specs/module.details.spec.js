define(['viewmodels/patients/tabs/module.details', 'services/datacontext'], function (moduleDetails, datacontext) {
    describe('List of details for a given module.', function(){

        it('has no errors loading', function () {
            expect(true).toBe(true);
        });
    	it("defaults to description closed, individual attributes open, module attributes closed, objectives closed", function () {
    	    expect(moduleDetails.descriptionSectionOpen().toBe(false));
            expect(moduleDetails.individualAttributesSectionOpen().toBe(true));
            expect(moduleDetails.attributesSectionOpen().toBe(false));
            expect(moduleDetails.objectivesSectionOpen().toBe(false));
		});
        it("shows no objectives set until objectives have been set", function () {
            var mockModule = createMockModule();
            moduleDetails.activeModule = mockModule;
            expect(moduleDetails.computedObjectives().length.toBe(0));
            expect(moduleDetails.computedObjectives().objectivesShowing().toBe(false));
        });


        function createMockModule() {
            return datacontext.createEntity('Module', { name: 'MockModule', description: 'MockModule', spawnElement: 2, enabled: true });
        }

    });
});