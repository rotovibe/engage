(function () {
    window.Visualization = (function () {

        // Need to pass in the filters observable

        function Visualization(selector, patients, rad_prop) {
            var _this = this;
            this.selector = selector;
            this.patients = patients;
            this.rad_prop = rad_prop;
            this.svg = d3.select(this.selector);
            this.color_scale = d3.scale.category10();
            this.dynamic_radius = true;
            this.filters = $('#filters :checkbox');
            this.height = d3.min([$(this.svg.node()).parents('.body').width(), $(this.svg.node()).parents('.body').height()]);
            this.padding = 15;
            this.show_backgrounds = true;
            this.width = this.height;
            this.force = d3.layout.force().charge(-70).gravity(.05).size([this.width, this.height]).on('tick', function (e) {
                return _this.tick(e);
            });
            this.filters.change(function () {
                return _this.data_grouping();
            });
            this.filters.each(function (index, filter) {
                return $(filter).parents('td').next('td').find('.indicator').css('background', _this.color_scale(index));
            });
            this.chart_setup();
        }

        Visualization.prototype.chart_setup = function () {
            var _this = this;
            this.patients.forEach(function (patient) {
                return patient.radius = patient[_this.rad_prop] + 5;
            });
            this.svg.attr('height', this.height).attr('width', this.width);
            this.svg.append('g').attr('class', 'placeholders');
            this.svg.append('g').attr('class', 'nodes');
            this.nodes = this.svg.select('g.nodes').selectAll('circle.node').data(this.patients).enter().append('svg:circle').attr('class', 'node').attr('r', function (d) {
                return d.radius;
            }).attr('cx', function (d) {
                return d.x;
            }).attr('cy', function (d) {
                return d.y;
            }).style('fill', function (d) {
                return _this.color_scale(0);
            }).style('opacity', '1');
            return this.data_grouping();
        };

        Visualization.prototype.data_grouping = function () {
            var active_filters, all_conditions, overlap_exists,
              _this = this;
            active_filters = $('#filters :checkbox:checked');
            this.filters.not(':checked').prop('disabled', active_filters.length > 2);
            active_filters.prop('disabled', active_filters.length < 2);
            $.uniform.update(this.filters);
            this.overlaps = [];
            overlap_exists = function (overlap) {
                var duplicate;
                duplicate = false;
                _.pluck(_this.overlaps, 'sets').forEach(function (set) {
                    if (d3.max(set) === d3.max(overlap) && d3.min(set) === d3.min(overlap)) {
                        return duplicate = true;
                    }
                });
                return duplicate;
            };
            all_conditions = function (patient) {
                var count;
                count = 0;
                _this.sets.forEach(function (set) {
                    if (set.conditional(patient)) {
                        return count++;
                    }
                });
                return count === _this.sets.length;
            };
            this.sets = $.map(active_filters, function (group) {
                var conditional, obj, patients;
                group = $(group);
                conditional = (function () {
                    switch (group.data('comparator')) {
                        case 'contains':
                            return function (patient) {
                                return patient[group.data('key')].indexOf(parseInt(group.val())) !== -1;
                            };
                        case 'equality':
                            return function (patient) {
                                return patient[group.data('key')];
                            };
                        case 'threshold':
                            return function (patient) {
                                return patient[group.data('key')] > group.val();
                            };
                    }
                })();
                patients = _this.patients.filter(function (patient) {
                    return conditional(patient);
                });
                return obj = {
                    id: group.val(),
                    conditional: conditional,
                    patients: patients,
                    size: patients.length,
                    color: group.parents('td').next('td').find('.indicator').css('background-color')
                };
            });
            this.sets.forEach(function (group, group_index) {
                return _this.sets.filter(function (d, i) {
                    return i !== group_index;
                }).forEach(function (set) {
                    var patients, sets;
                    patients = group.patients.filter(function (patient) {
                        return set.conditional(patient);
                    });
                    if (_this.sets.length > 2) {
                        _this.central_patients = _this.patients.filter(function (patient) {
                            return all_conditions(patient);
                        });
                        patients = _.union(patients, _this.central_patients);
                    }
                    group.patients = _.difference(group.patients, patients);
                    set.patients = _.difference(set.patients, patients);
                    sets = [group_index, _this.sets.indexOf(set)].sort();
                    if (!overlap_exists(sets)) {
                        return _this.overlaps.push({
                            patients: patients,
                            sets: sets,
                            size: patients.length,
                            color: _this.color_mixer([group.color, set.color])
                        });
                    }
                });
            });
            if (_.isEmpty(this.overlaps)) {
                this.sets[0].x = this.width / 2;
                this.sets[0].y = this.height / 2;
                this.sets[0].radius = d3.min([this.width, this.height]) / 2;
            } else {
                this.sets = venn.venn(this.sets, this.overlaps, {
                    layoutFunction: venn.classicMDSLayout
                });
                this.sets = venn.scaleSolution(this.sets, this.width, this.height, this.padding);
            }
            return this.patient_attractions();
        };

        Visualization.prototype.patient_attractions = function () {
            var middle,
              _this = this;
            this.sets.forEach(function (set, index) {
                var coords;
                coords = {
                    x: set.x,
                    y: set.y
                };
                return _.intersection(_this.patients, set.patients).forEach(function (patient) {
                    patient.bounds = [set];
                    patient.cluster = set;
                    return patient.color = set.color;
                });
            });
            this.overlaps.forEach(function (overlap, index) {
                overlap.groups = [_this.sets[overlap.sets[0]], _this.sets[overlap.sets[1]]];
                _.extend(overlap, _this.lens_center(overlap.groups[0], overlap.groups[1]));
                return _.intersection(_this.patients, overlap.patients).forEach(function (patient) {
                    patient.bounds = overlap.groups;
                    patient.cluster = overlap;
                    return patient.color = overlap.color;
                });
            });
            this.unscoped_patients = _.difference(this.patients, _.flatten(_.pluck(this.sets, 'patients')), _.flatten(_.pluck(this.overlaps, 'patients')));
            this.unscoped_patients.forEach(function (patient) {
                return patient.bounds = void 0;
            });
            _.where(this.patients, {
                bounds: void 0
            }).forEach(function (patient) {
                return patient.cluster = {
                    x: -1000,
                    y: -1000
                };
            });
            if (this.overlaps.length > 2) {
                middle = this.circumcenter();
                this.central_patients.forEach(function (patient) {
                    patient.bounds = _this.sets;
                    patient.cluster = middle;
                    return patient.color = _this.color_mixer(_.pluck(_this.sets, 'color'));
                });
            }
            return this.determine_radius();
        };

        Visualization.prototype.determine_radius = function () {
            var area, gaps;
            area = d3.sum(this.sets, function (d) {
                return venn.circleArea(d.radius, d.radius);
            });
            gaps = d3.sum(this.patients, function (d) {
                return d[this.rad_prop];
            });
            this.radius = Math.sqrt(area / gaps / Math.PI);
            return this.node_animations();
        };

        Visualization.prototype.node_animations = function (sets, overlaps) {
            var _this = this;
            if (this.show_backgrounds) {
                this.placeholders();
            }
            this.nodes.data(this.patients);
            this.force.nodes(this.patients).start();
            this.filter_patients();
            return this.svg.on('click', function () {
                var cluster, datum, duration, unscoped;
                duration = 500;
                datum = d3.select(d3.event.target).datum();
                if (_this.nodes.filter(function (d) {
                  return d === datum;
                }).size() > 0) {
                    cluster = _this.nodes.filter(function (d) {
                        return d.cluster === datum.cluster;
                    });
                    cluster.transition().duration(duration).style('opacity', '1');
                    unscoped = _this.nodes.filter(function (d) {
                        return d.cluster !== datum.cluster;
                    });
                    unscoped.transition().duration(duration).style('opacity', '0.25');
                    return _this.filter_patients(unscoped.data());
                } else {
                    _this.nodes.transition().duration(duration).style('opacity', '1');
                    return _this.filter_patients();
                }
            });
        };

        Visualization.prototype.tick = function (e) {
            var k, q,
              _this = this;
            q = d3.geom.quadtree(this.patients);
            k = .1 * e.alpha;
            this.patients.forEach(function (o, i) {
                o.radius = _this.dynamic_radius ? 2 * o[_this.rad_prop] + 4 : _this.radius;
                o.x += (o.cluster.x - o.x) * k;
                o.y += (o.cluster.y - o.y) * k;
                return q.visit(_this.contain(o));
            });
            return this.nodes.attr('cx', function (d) {
                return d.x;
            }).attr('cy', function (d) {
                return d.y;
            }).attr('r', function (d) {
                return d.radius;
            }).style('fill', function (d) {
                return d.color;
            });
        };

        Visualization.prototype.filter_patients = function (hide) {
            var hidden_rows, patient_ids, table_rows;
            if (hide == null) {
                hide = this.unscoped_patients;
            }
            patient_ids = _.pluck(hide, 'id');
            table_rows = $(this.svg.node()).parents('.column').next('.column').find('tbody tr');
            hidden_rows = table_rows.filter(function (i) {
                return _.contains(patient_ids, $(this).data('id'));
            });
            hidden_rows.css('display', 'none');
            return table_rows.not(hidden_rows).css('display', '');
        };

        Visualization.prototype.placeholders = function () {
            var placeholders;
            placeholders = this.svg.select('g.placeholders').selectAll('circle.layout').data(this.sets);
            placeholders.enter().append('svg:circle').attr('class', 'layout');
            placeholders.attr('r', function (d) {
                return d.radius;
            }).attr('cx', function (d) {
                return d.x;
            }).attr('cy', function (d) {
                return d.y;
            }).attr('fill', function (d) {
                return d.color;
            });
            return placeholders.exit().remove();
        };

        /*
            # FUNCTIONS
        */
        
        Visualization.prototype.cartesian_to_polar = function (point, center) {
            return {
                angle: Math.atan2(point.y - center.y, point.x - center.x),
                magnitude: venn.distance(point, center)
            };
        };

        Visualization.prototype.circumcenter = function () {
            var functions, intx,
              _this = this;
            functions = this.overlaps.map(function (overlap) {
                overlap.points = venn.circleCircleIntersection(overlap.groups[0], overlap.groups[1]);
                overlap.slope = _this.slope(overlap.points[0], overlap.points[1]);
                overlap.midpoint = _this.midpoint(overlap.points[0], overlap.points[1]);
                return overlap;
            });
            functions = functions.filter(function (overlap) {
                return overlap.slope !== void 0;
            });
            functions = functions.map(function (overlap) {
                overlap.yint = overlap.points[0].y - overlap.slope * overlap.points[0].x;
                return overlap;
            });
            intx = ((-functions[1].slope * functions[1].points[0].x) + functions[1].points[0].y + (functions[0].slope * functions[0].points[0].x) - functions[0].points[0].y) / (functions[0].slope - functions[1].slope);
            return {
                x: intx,
                y: functions[0].slope * intx + functions[0].yint
            };
        };

        Visualization.prototype.collide = function (node) {
            var nx1, nx2, ny1, ny2, r;
            r = node.radius;
            nx1 = node.x - r;
            nx2 = node.x + r;
            ny1 = node.y - r;
            ny2 = node.y + r;
            return function (quad, x1, y1, x2, y2) {
                var l, x, y;
                if (quad.point && quad.point !== node) {
                    x = node.x - quad.point.x;
                    y = node.y - quad.point.y;
                    l = Math.sqrt(Math.pow(x, 2) + Math.pow(y, 2));
                    r = 1.05 * (node.radius + quad.point.radius);
                    if (l < r) {
                        l = (l - r) / l * .5;
                        node.x -= x *= l;
                        node.y -= y *= l;
                        quad.point.x += x;
                        quad.point.y += y;
                    }
                }
                return x1 > nx2 || x2 < nx1 || y1 > ny2 || y2 < ny1;
            };
        };

        Visualization.prototype.color_mixer = function (colors) {
            var color,
              _this = this;
            color = Color(colors.pop());
            colors.forEach(function (string) {
                return color.mix(Color(string));
            });
            if (colors.length > 1) {
                color.darken(0.35);
            }
            return color.rgbString();
        };

        Visualization.prototype.contain = function (patient) {
            var point,
              _this = this;
            point = {
                x: patient.x,
                y: patient.y
            };
            if (typeof patient.bounds !== 'undefined') {
                point = this.exclude(patient, point);
                patient.bounds.forEach(function (bound) {
                    var coord;
                    coord = _this.cartesian_to_polar(point, bound);
                    return point = _this.polar_to_cartesian(coord.angle, Math.min(coord.magnitude, bound.radius - _this.padding), bound);
                });
            }
            patient.x = point.x;
            patient.y = point.y;
            return this.collide(patient);
        };

        Visualization.prototype.exclude = function (patient, point) {
            var inverse,
              _this = this;
            inverse = _.difference(this.sets, patient.bounds);
            inverse.forEach(function (condition) {
                var coord;
                coord = _this.cartesian_to_polar(point, condition);
                return point = _this.polar_to_cartesian(coord.angle, Math.max(coord.magnitude, condition.radius + _this.padding), condition);
            });
            return point;
        };

        Visualization.prototype.lens_center = function (c1, c2) {
            var points;
            points = venn.circleCircleIntersection(c1, c2);
            return this.midpoint(points[0], points[1]);
        };

        Visualization.prototype.midpoint = function (p1, p2) {
            return {
                x: (p1.x + p2.x) / 2,
                y: (p1.y + p2.y) / 2
            };
        };

        Visualization.prototype.slope = function (p1, p2) {
            [p1, p2].forEach(function (p) {
                p.x = Math.floor(p.x);
                return p.y = Math.floor(p.y);
            });
            if (p2.x === p1.x) {
                return void 0;
            } else {
                return (p2.y - p1.y) / (p2.x - p1.x);
            }
        };

        Visualization.prototype.polar_to_cartesian = function (theta, radius, offset) {
            if (offset == null) {
                offset = {
                    x: 0,
                    y: 0
                };
            }
            return {
                x: (Math.cos(theta) * radius) + offset.x,
                y: (Math.sin(theta) * radius) + offset.y
            };
        };

        return Visualization;

    })();

}).call(this);
