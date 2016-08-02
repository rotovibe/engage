define([],
    function () {

        var createMockStepAndQuestions = function (manager) {
            var thisStepMap = manager.createEntity('Step', { name: 'Eligibility' });
            var thisStepMap = manager.createEntity('QuestionTypeId', { name: 'True/False' });
            manager.createEntity('Question', { question: 'Eligibility', stepMap: thisStepMap, typeId: 1 });
            manager.createEntity('Question', { question: 'Eligibility', stepMap: thisStepMap, typeId: 1 });
            manager.createEntity('Question', { question: 'Eligibility', stepMap: thisStepMap, typeId: 1 });
        };

        var getStepById = function (manager, entityObservable, id) {
            var p = getLocalList(manager, 'Step', entityType);
            if (p.length > 0) {
                entityObservable(p);
                return Q.resolve();
            }
        };

        var getStepMap = function (manager) {
            var p = getLocalList(manager, 'Step', entityType);
            if (p.length > 0) {
                entityObservable(p);
                return Q.resolve();
            }
        };

        var stepservice = {
            createMockStepAndQuestions: createMockStepAndQuestions,
            getStepById: getStepById,
            getMockStepMap: getMockStepMap
        };
        return stepservice;

        function getLocalList(manager, resource, entityType, parentPropertyName, parentPropertyId, orderby) {
            var query = breeze.EntityQuery.from(resource)
                .toType(entityType);

            if (orderby) {
                query = query.orderBy(orderby);
            }

            if (parentPropertyName && parentPropertyId) {
                query = query.where(parentPropertyName, '==', parentPropertyId);
            }
            return manager.executeQueryLocally(query);
        }
    });