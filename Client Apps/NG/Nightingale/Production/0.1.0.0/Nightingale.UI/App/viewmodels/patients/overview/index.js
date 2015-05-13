define(['plugins/router'],
    function (router) {
        var vm = {
            activate: activate,
            goBack: goBack,
            title: 'index'
        };

        return vm;

        function activate() {
            console.log('Overview activated');
        }

        function goBack(complete) {
            router.navigateBack();
        }
    });