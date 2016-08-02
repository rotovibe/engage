//Contains tile size and scene layout details
var sceneLayoutService = function () {
    var width,
        get = function () {
            width = $('#contentHolder').width();
            //scene1
            var pad = 5,
            r1H = 210,
            //small
            s1Sh = 175,
            s1Sh2 = 323,
            s1Sw = 365,
            //medium
            s1Mh = 175,
            s1Mw = 237,
            s1Mw2 = 365,
            //large
            s1Lh = 300,
            s1Lw = 485,

            items = { tiles:
                    [
                    { name: 'Condition Compliance',
                        tileId: 'Template3',
                        scenes: [
                            { height: s1Lh, width: s1Lw, top: 0, left: 0, opacity: 1, size: 2, z: 0 }
                        ]
                    },
                    { name: 'Diabetes',
                        tileId: 'Template2',
                        scenes: [
                            { height: s1Lh, width: s1Lw, top: 0, left: s1Lw + (pad * 2), opacity: 1, size: 2, z: 0 }
                        ]
                    },

                    { name: 'Hypertension',
                        tileId: 'Template4',
                        scenes: [
                            { height: s1Mh, width: s1Mw, top: s1Lh + (pad * 2), left: 0, opacity: 1, size: 1, z: 0 }
                        ]
                    },

                    { name: 'Hypercholesterolemia',
                        tileId: 'Template1',
                        scenes: [
                            { height: s1Mh, width: s1Mw, top: s1Lh + (pad * 2), left: s1Mw + (pad * 2), opacity: 1, size: 1, z: 0 }
                        ]
                    },

                    { name: 'Obesity',
                        tileId: 'Template5',
                        scenes: [
                            { height: s1Mh, width: s1Mw, top: s1Lh + (pad * 2), left: (s1Mw * 2) + (pad * 4), opacity: 1, size: 1, z: 0 }
                        ]
                    },

                    { name: 'Preventive',
                        tileId: 'Template6',
                        scenes: [
                            { height: s1Mh, width: s1Mw, top: s1Lh + (pad * 2), left: (s1Mw * 3) + (pad * 6), opacity: 1, size: 1, z: 0 }
                        ]
                    }              
                ]
            };

            return items;
        };

    return {
        get: get
    };

} ();