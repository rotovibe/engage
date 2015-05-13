var checkedCount = 0;

function CheckAll(me) {
    //get reference of GridView control
    var gridID = me.id.replace('_HeaderButton', '');
    var grid = document.getElementById(gridID);
    
    //variable to contain the cell of the grid
    var cell;

    if (grid.rows.length > 0) {
        //loop starts from 2. rows[0] points to the header because Paging is at both top and bottom
        for (i = 0; i < grid.rows.length; i++) {
            //get the reference of first column
            cell = grid.rows[i].cells[0];

            //loop according to the number of childNodes in the cell
            for (j = 0; j < cell.childNodes.length; j++) {
                if (cell.childNodes[j].tagName == "SPAN") {
                    for (k = 0; k < cell.childNodes[j].childNodes.length; k++) {
                        if (cell.childNodes[j].childNodes[k].type == "checkbox" && cell.childNodes[j].childNodes[k].className != "hide") {

                            if (cell.childNodes[j].childNodes[k].checked != document.getElementById(me.id).checked) {
                                cell.childNodes[j].childNodes[k].checked = !document.getElementById(me.id).checked;
                                cell.childNodes[j].childNodes[k].click();
                            }
                            break;
                        }
                        break;
                    }
                }
                else if (cell.childNodes[j].type == "checkbox" && cell.childNodes[j].className != "hide") {
                    if (cell.childNodes[j].checked != document.getElementById(me.id).checked) {
                        cell.childNodes[j].checked = !document.getElementById(me.id).checked;
                        cell.childNodes[j].click();
                    }
                    break;
                }
            }
        }
    }
}

function MaintainCheckedCount(control, individualCheck, chkLabel, chkLabelType) {
    if (control.checked)
        checkedCount = checkedCount + 1;
    else if (!control.checked && checkedCount > 0)
        checkedCount = checkedCount - 1;

    var checkedLabel = document.getElementById(chkLabel);
    if(checkedCount > 0)
        checkedLabel.value = checkedCount + " " + chkLabelType + " selected.";
    else
        checkedLabel.value = " "; //purposely set to a space....
}

function ResetCheckedCount(chkLabel) {
    checkedCount = 0;

    var checkedLabel = document.getElementById(chkLabel);
    checkedLabel.value = " "; //purposely set to a space....
}
//------------------------------------------------------------------------------
// IdeaSparx.CoolControls.Web
// Author: John Eric Sobrepena (2009)
// You can use these codes in whole or in parts without warranty.
// By using any part of this code, you agree 
// to keep this information about the author name intact.
// http://johnsobrepena.blogspot.com
//------------------------------------------------------------------------------

var $$$ = function (element) {

    //Check if the parameter is a DOM element or a string ID of the DOM element
    if (typeof element == 'String')
        element = document.getElementById(elementId);
    else
        element = element;

    if (typeof element != 'undefined'
        && element != null
        && typeof element.$elementExtenderID != 'undefined'
        && element.$elementExtenderID != null
        && element.$elementExtenderID == "jesCoolXoiopq98012iPOqw1908")
        return element;


    return $$$.Extend(element, {
        //Marker that element is extended
        $elementExtenderID: "jesCoolXoiopq98012iPOqw1908",

        //Attaches an event handler to the "element"
        AttachEvent: function (EventString, Handler, Context) {
            $$$.AttachEvent(element, EventString, Handler, Context);
        },

        GetWidth: function () {
            if (element.style.width != "")
                return element.style.width.replace('px', '');
            return element.offsetWidth;
        },

        GetHeight: function () {
            if (element.style.width != "")
                return element.style.height.replace('px', '');
            return element.offsetHeight;
        },

        GetAbsolutePosition: function () {
            var e = element;
            //--- Determine real "element" position relative to the main page.
            var ElementLeft = e.offsetLeft;
            var ElementTop = e.offsetTop;
            var TmpElem = e.offsetParent;
            while (TmpElem != null) {
                ElementLeft += TmpElem.offsetLeft;
                ElementTop += TmpElem.offsetTop;
                TmpElem = TmpElem.offsetParent;
            }

            var TmpElem = e.parentElement || e.parentNode;
            while (TmpElem != null && TmpElem != document.body) {
                ElementLeft -= TmpElem.scrollLeft;
                ElementTop -= TmpElem.scrollTop;
                TmpElem = TmpElem.parentElement || TmpElem.parentNode;
            }

            return { top: ElementTop, left: ElementLeft };
        },
        //Get children
        GetChildren: function () {
            if (element.children)
                return element.children;
            else
                return element.childNodes;
        },
        //Disable text selection in the page
        DisableSelection: function () {
            if (typeof element.style.MozUserSelect != "undefined") //Firefox route
                this.style.MozUserSelect = "none"
            else if (typeof element.style.webkitUserSelect != "undefied") //WebKit Chrome Safari branch
                this.style.webkitUserSelect = "none";
        },
        //Enable text selection in the page
        EnableSelection: function () {
            if (typeof element.style.MozUserSelect != "undefined") //Firefox route
                this.style.MozUserSelect = ""
            else if (typeof element.style.webkitUserSelect != "undefied") //WebKit Chrome Safari branch
                this.style.webkitUserSelect = "";
        },
        //Get the first child element with tagname specified
        FirstChildWithTagName: function (tagName) {
            var children = element.getElementsByTagName(tagName);
            if (children != null && children.length > 0)
                return children[1];
            return null;
        }
    });
}

$$$.Extend = function (target, varExtensions) {
    for (var name in varExtensions) {
        if (target[name] == 'undefined' || target[name] == null || target[name] != varExtensions[name])
            target[name] = varExtensions[name];
    }

    return target;
}

//Set the globally common functions $$$.fn
$$$.fn =
    {
        Event: {
            resizeState: {
                eventTarget: null,
                isResize: false,
                originX: 0,
                minX: 0,
                originalStyle: { cursor: '' },
                verticalLine: null,
                count: 0
            },
            isAjax: false,
            eventHandlers: []
        },
        //cross-browser attach event.
        //Element - is the object where eventhandler will be attached to
        //EventString    - is the event name to handle of Element
        //Handler is a function pointing to the event handler
        //Context is what context the Handler will be executed
        AttachEvent: function (Element, EventString, Handler, Context) {
            var isAttached = true;
            if (typeof Handler == 'undefined' || Handler == null) return;
            if (typeof (Context) != 'undefined' && Context != null) {
                if (Element.addEventListener) // W3C DOM
                    Element.addEventListener(EventString, Handler = $$$.ContextOf(Context, Handler), false);
                else if (Element.attachEvent) // IE DOM
                    Element.attachEvent('on' + EventString, Handler = $$$.ContextOf(Context, Handler));
                else {
                    isAttached = false;
                    alert('Unable to attach to event ' + EventString + ' of ' + Element.toString());
                }
            } else {
                if (Element.addEventListener) // W3C DOM
                    Element.addEventListener(EventString, Handler, false);
                else if (Element.attachEvent) // IE DOM
                    Element.attachEvent('on' + EventString, Handler = $$$.ContextOf(Element, Handler));
                else {
                    isAttached = false;
                    alert('Unable to attach to event ' + EventString + ' of ' + Element.toString());
                }
            }

            return Handler;
        },

        RegisterEventName: function (EventName) {
            if (!$$$.IsEventNameRegistered(EventName))
                $$$.Event.eventHandlers[$$$.Event.eventHandlers.length] = EventName;
        },

        IsEventNameRegistered: function (EventName) {
            for (var i = 0; i < $$$.Event.eventHandlers.length; i++) {
                if (EventName == $$$.Event.eventHandlers[i])
                    return true;
            }
            return false;
        },

        RemoveEvent: function (Element, EventString, Handler) {
            if (Element.removeEventListener) // W3C DOM
                Element.removeEventListener(EventString, Handler, false);
            else if (Element.detachEvent) // IE DOM
                Element.detachEvent('on' + EventString, Handler);
            else
                alert('Unable to remove event ' + EventString + ' of ' + Element.toString());
        },

        //Returns a function that executes [Method] in the context of [ThisObj]
        ContextOf: function (ThisObj, Method) {
            return function () { return Method.apply(ThisObj, arguments); };
        },

        GetMousePosition: function (evt) {
            var e = evt || window.event;
            var cursor = { x: 0, y: 0 };
            if (e.pageX || e.pageY) {
                cursor.x = e.pageX;
                cursor.y = e.pageY;
            }
            else {
                cursor.x = e.clientX +
                (document.documentElement.scrollLeft ||
                document.body.scrollLeft) -
                document.documentElement.clientLeft;
                cursor.y = e.clientY +
                (document.documentElement.scrollTop ||
                document.body.scrollTop) -
                document.documentElement.clientTop;
            }
            return cursor;
        },

        AjaxToolkitEndRequestHandler: function (sender, args) {
            var p = sender._updatePanelClientIDs;
            if (p != null) {
                for (var j = 0; j < p.length; j++) {
                    var scripts = $get(p[j]).getElementsByTagName("script");
                    $$$.Event.isAjax = true;
                    // .text is necessary for IE.
                    for (var i = 0; i < scripts.length; i++) {
                        try {
                            eval(scripts[i].innerHTML || scripts[i].text);
                        } catch (e2) { }
                    }
                    $$$.Event.isAjax = false;
                }
            }
        },

        _OnLoad: function (evt) {
            try {
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest($$$.AjaxToolkitEndRequestHandler);
            }
            catch (e) { }
        },

        IsDefined: function (obj) {
            return (typeof obj != 'undefined' && obj != null);
        },

        /*@cc_on
        @if (@_jscript_version <= 5.6)
        IsBrowserIE6: true
        @else @*/
            IsBrowserIE6 : false
        /*@end
        @*/
    }

//Extend the $$$ from $$$.fn
$$$ = $$$.Extend($$$, $$$.fn);
$$$(window).AttachEvent('load', $$$._OnLoad);

//------------------------------------------------------------------------------
// IdeaSparx.CoolControls.Web
// Author: John Eric Sobrepena (2009)
// You can use these codes in whole or in parts without warranty.
// By using any part of this code, you agree 
// to keep this information about the author name intact.
// http://johnsobrepena.blogspot.com
//------------------------------------------------------------------------------


//CoolProperties = {GridID : "", GridContainerID : "", HeaderContainerID : "", TableContainerID : "", FooterContainerID : "", PagerContainerID : "", HiddenFieldDataID : "", FormID : "", AllowResizeColumn : ""}
function CoolGridView(CoolProperties) {
    this.HeaderContainer = null;
    this.TableContainer = null;
    this.GridContainer = null
    this.FooterContainer = null;
    this.Grid = null;
    this.PagerContainerTop = null;
    this.PagerContainerBottom = null;
    this.HiddenFieldData = null;
    this.AllowResizeColumn = CoolProperties.AllowResizeColumn;
    this.HeaderCells = new Array();
    this._CoolProperties = CoolProperties;
    this._ControlState = { ColumnWidths: [], ScrollPosition: { x: 0, y: 0} };

    //always false for now. Initialize() has to run inline as well as on page onload 
    //required for working with UpdatePanel. Need to optimize this.
    this._isPageLoad = false;

    this.OnPageLoadHandler = function () {
        this._isPageLoad = true;
        this.Initialize();
        this._isPageLoad = false;
    }

    //Add 100px to the DIV that contains the header and footer as preparation for scrolling
    this.CorrectHeaderFooterEndSpacing = function () {
        var objs = [this.HeaderContainer, this.FooterContainer];
        for (var i = 0; i < objs.length; i++) {
            if (objs[i] == null) continue;
            var d = objs[i].getElementsByTagName("DIV");
            if (d != null && d.length > 0) {
                //var t = d[0].getElementsByTagName("TABLE");
                //if (t != null && t.length > 0) {
                //d[0].style.width = (t[0].offsetWidth + 100) + 'px';
                d[0].style.width = (this.TableContainer.scrollWidth + 100) + 'px';
                //}
            }
        }
    }

    this.Initialize = function () {
        //If not yet initialized or is being excuted after ajax call then continue, else return.
        if (!this._isPageLoad && !$$$.Event.isAjax)
            return;
        //Initialize References
        this.Grid = document.getElementById(this._CoolProperties.GridID);
        this.HeaderContainer = document.getElementById(this._CoolProperties.HeaderContainerID);
        this.TableContainer = document.getElementById(this._CoolProperties.TableContainerID);
        this.GridContainer = document.getElementById(this._CoolProperties.GridContainerID);
        this.FooterContainer = document.getElementById(this._CoolProperties.FooterContainerID);
        this.PagerContainerTop = document.getElementById(this._CoolProperties.PagerContainerID + "Top");
        this.PagerContainerBottom = document.getElementById(this._CoolProperties.PagerContainerID + "Bottom");
        this.HiddenFieldData = document.getElementById(this._CoolProperties.HiddenFieldDataID);
        this.Form = document.getElementById(this._CoolProperties.FormID);
        $$$.Event.resizeState.verticalLine = document.getElementById("lLKAopspo28lOANcaju9182ia92u");
        this.CorrectHeaderFooterEndSpacing();
        this.LoadControlStateData();

        //Register scroll handler
        if (this.TableContainer != null)
            $$$(this.TableContainer).AttachEvent('scroll', this.TableContainerScrollHandler, this);
        //$$$.AttachEvent(this.TableContainer, 'scroll', this.TableContainerScrollHandler, this);
        //Register TableContainer resize handler
        if (this.TableContainer != null)
            $$$(this.TableContainer).AttachEvent('resize', this.TableContainerResizeHandler, this);
        //Register GridContainer resize handler
        if (this.GridContainer != null)
            $$$(this.GridContainer).AttachEvent('resize', this.GridContainerResizeHandler, this);

        //Register the OnFormSubmit event handler
        $$$(this.Form).AttachEvent("submit", this.OnFormSubmit, this);

        //Check if user-resizing column is enabled
        if (this.AllowResizeColumn) {
            //Initialize columns for Resizing:
            //Initialize column resizing only if there is a header present
            var tableWidth = 0;
            var cells = this.GetCellsOfFirstRow(this.HeaderContainer);
            var tCells = this.GetCellsOfFirstRow(this.TableContainer);
            var fCells = this.GetCellsOfFirstRow(this.FooterContainer);
            if (cells != null && cells.length > 0) {
                this.HeaderCells = new Array();
                for (var i = 0; i < cells.length; i++) {
                    this.HeaderCells[i] = cells[i];
                    this.HeaderCells[i].TableColumn = tCells && tCells[i] || null;
                    this.HeaderCells[i].FooterColumn = fCells && fCells[i] || null;
                    this.HeaderCells[i].CoolGrid = this;
                    tableWidth += (this.HeaderCells[i].jesWidth = this.GetColWidth(cells[i]) || $$$(this.HeaderCells[i]).GetWidth());
                    $$$(cells[i]).AttachEvent("mousemove", this.OnCellMouseMoveHandler);
                    $$$(cells[i]).AttachEvent("mousedown", this.OnCellMouseDownHandler);
                }
            }
        }

        $$$.Event.resizeState.originalStyle.cursor = document.body.style.cursor;

        //Register these events one time only
        if (!$$$.IsEventNameRegistered("98jkiopIOPULKDkjas9123")) {
            $$$(document.body).AttachEvent("mousemove", this.OnDocMouseMoveHandler);
            $$$(document.body).AttachEvent("mouseup", this.OnDocMouseUpHandler);
            $$$(document.body).AttachEvent("selectstart", this.OnDocSelectStartHandler);
            $$$.RegisterEventName("98jkiopIOPULKDkjas9123")
        }

        //TODO:For AjaxControlToolkit compatibility
        try {
            Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest($$$.ContextOf(this, this.AjaxToolkitBeginRequestHandler));
        } catch (ex1) { }


        this.GridContainerResizeHandler();
        this.TableContainer.scrollLeft = this._ControlState.ScrollPosition.x;
        this.TableContainer.scrollTop = this._ControlState.ScrollPosition.y;
        this.TableContainerScrollHandler();
    }

    //Get an array [] of cells of the first row in a table inside a DIV or SPAN
    this.GetCellsOfFirstRow = function (container) {
        if (typeof container == 'undefined' || container == null) return null;

        var cells = null;
        var _tables = container.getElementsByTagName("TABLE");
        if (_tables != null && _tables.length > 0) {
            var _control = _tables[0];
            var columns = _control.getElementsByTagName("COL");
            var rows = _control.getElementsByTagName("TR");

            if (rows != null && rows.length > 0) {
                var cells = rows[0].getElementsByTagName("TH");
                if (cells == null || cells.length == 0)
                    cells = rows[0].getElementsByTagName("TD");

            }

            if (cells != null) {
                for (var i = 0; i < cells.length; i++) {
                    cells[i].ColObject = columns[i] || null;
                }
            }
        }
        return cells;
    }

    //Resizing Columns Handlers
    this.OnCellMouseMoveHandler = function (e) {
        if ($$$.Event.resizeState.isResize) return false;

        var cursor = $$$.GetMousePosition(e);
        var elem = $$$(this);
        var position = elem.GetAbsolutePosition();
        position.width = elem.offsetWidth;
        position.height = elem.offsetHeight;

        if (cursor.x >= position.left + position.width - 5) {
            this.style.cursor = 'w-resize';
        }
        else
            this.style.cursor = '';
    }

    //Resizing Columns Handlers
    this.OnCellMouseDownHandler = function (e) {
        var cursor = $$$.GetMousePosition(e);
        var elem = $$$(this);
        var position = elem.GetAbsolutePosition();
        position.width = elem.offsetWidth;
        position.height = elem.offsetHeight;
        $$$.Event.resizeState.originalStyle.cursor = document.body.style.cursor;

        //Check if resizing should start
        if (cursor.x >= position.left + position.width - 5) {
            document.body.style.cursor = 'w-resize';
            $$$.Event.resizeState.eventTarget = elem;
            $$$.Event.resizeState.originX = cursor.x;
            $$$.Event.resizeState.minX = position.left;
            $$$(document.body).DisableSelection();
            $$$.Event.resizeState.isResize = true;

            var vLine = $$$.Event.resizeState.verticalLine;
            if (vLine != null) {
                vLine.style.width = '3px';
                vLine.style.height = (elem.CoolGrid.GridContainer.offsetHeight - 2) + 'px';
                vLine.style.left = (cursor.x - 1) + 'px';
                vLine.style.top = $$$(elem.CoolGrid.GridContainer).GetAbsolutePosition().top + 'px';
                vLine.style.display = '';
            }
        }

        return false;
    }

    //Resizing Columns Handlers
    this.OnDocMouseMoveHandler = function (e) {
        //if not resizing mode then do  nothing
        if (!$$$.Event.resizeState.isResize || $$$.Event.resizeState.eventTarget == null) return;

        var cursor = $$$.GetMousePosition(e);

        var vLine = $$$.Event.resizeState.verticalLine;
        if (vLine != null) {
            vLine.style.left = (cursor.x - 1) + 'px';
        }

        return false;
    }

    //Resizing Columns Handlers
    this.OnDocMouseUpHandler = function (e) {
        //if not resizing mode then do  nothing
        if (!$$$.Event.resizeState.isResize || $$$.Event.resizeState.eventTarget == null) return;

        $$$(document.body).EnableSelection();
        document.body.style.cursor = ''; //$$$.Event.resizeState.originalStyle.cursor;

        var cursor = $$$.GetMousePosition(e);
        var totalWidth = 0;
        var width = cursor.x - $$$.Event.resizeState.minX;

        if (width <= 10)
            width = 10;

        $$$.Event.resizeState.eventTarget.jesWidth = width;

        var vLine = $$$.Event.resizeState.verticalLine;
        if (vLine != null)
            vLine.style.display = 'none';

        var coolgrid = $$$.Event.resizeState.eventTarget.CoolGrid;

        for (var i = 0; i < $$$.Event.resizeState.eventTarget.CoolGrid.HeaderCells.length; i++) {

            var cell = $$$.Event.resizeState.eventTarget.CoolGrid.HeaderCells[i];
            if (cell.TableColumn) coolgrid.SetColWidth(cell.TableColumn, cell.jesWidth);
            if (cell.FooterColumn) coolgrid.SetColWidth(cell.FooterColumn, cell.jesWidth);
            coolgrid.SetColWidth(cell, cell.jesWidth);
        }

        $$$.Event.resizeState.eventTarget.CoolGrid.CorrectHeaderFooterEndSpacing();
        $$$.Event.resizeState.eventTarget.CoolGrid.GridContainerResizeHandler();
        $$$.Event.resizeState.eventTarget.CoolGrid.TableContainerScrollHandler();
        $$$.Event.resizeState.isResize = false;
    }

    //Resizing Columns Handlers
    this.OnDocMouseDownHandler = function (e) {
        return false;
    }

    //Resizing Columns Handlers
    this.OnDocSelectStartHandler = function (e) {
        return !$$$.Event.resizeState.isResize;
    }

    this.SetColWidth = function (THorTDObject, value) {
        if (typeof THorTDObject != 'undefined' && THorTDObject != null) {
            if (typeof THorTDObject.ColObject != 'undefined' && THorTDObject.ColObject != null) {
                THorTDObject.ColObject.width = value;
            } else {
                THorTDObject.style.width = value + 'px';
            }
        }
    }

    this.GetColWidth = function (THorTDObject) {
        if (typeof THorTDObject != 'undefined' && THorTDObject != null) {
            if (typeof THorTDObject.ColObject != 'undefined' && THorTDObject.ColObject != null) {
                return THorTDObject.ColObject.width;
            }

            return $$$(THorTDObject).GetWidth();
        }
        return 0;
    }

    this.GetAjaxToolkitEndRequestHandler = function () {
        // return the function to run this.AjaxToolkitEndRequestHandler to run in "this" context
        return $$$.ContextOf(this, this.AjaxToolkitEndRequestHandler);
    }

    //Compatibility with AjaxControlToolkit
    this.AjaxToolkitBeginRequestHandler = function (sender, args) {
        try {
            //TODO: This is a work-around with Ajax Control Toolkit that modify the request body to inject the new value of the hidden field.
            //TODO: Blog about how AjaxControlToolkit ignore assigned values in hidden field if assignment is done in initializeRequest and beginRequest event. This hack works.
            var $body = args.get_request().get_body();
            this.OnFormSubmit();
            var $p = '&' + this.HiddenFieldData.id + '=' + escape(this.HiddenFieldData.value);
            var $pat = '&' + this.HiddenFieldData.id + '=[^&]*';
            $body = $body.replace(new RegExp($pat), $p);
            args.get_request().set_body($body);
        } catch (ex1) { }
    }

    this.AjaxToolkitEndRequestHandler = function (sender, args) {
        this.Initialize();
    }

    //ScrollHandler to synchronize grid content's and header content's scrolling
    this.TableContainerScrollHandler = function () {
        if (this.HeaderContainer != null) {
            if ($$$.IsBrowserIE6)
                this.HeaderContainer.children[0].style.marginLeft = '-' + this.TableContainer.scrollLeft + 'px';
            else
                this.HeaderContainer.scrollLeft = this.TableContainer.scrollLeft;
        }
        if (this.FooterContainer != null) {
            if ($$$.IsBrowserIE6)
                this.FooterContainer.children[0].style.marginLeft = '-' + this.TableContainer.scrollLeft + 'px';
            else
                this.FooterContainer.scrollLeft = this.TableContainer.scrollLeft;
        }
    }

    //Handle the TableContainer resize event
    this.TableContainerResizeHandler = function () {
        this.TableContainerScrollHandler();
    }

    this.GridContainerResizeHandler = function () {
        if (this.GridContainer == null || this.GridContainer.style.height == '')
            return;

        var _height = this.GridContainer.clientHeight;

        //compatibility with IE6 and IE7
        if (_height + this.GridContainer.offsetTop <= this.GridContainer.clientHeight)
            _height += this.GridContainer.offsetTop;

        if (this.HeaderContainer != null)
            _height -= this.HeaderContainer.offsetHeight;
        if (this.FooterContainer != null)
            _height -= this.FooterContainer.offsetHeight;
        if (this.PagerContainerTop != null)
            _height -= this.PagerContainerTop.offsetHeight;
        if (this.PagerContainerBottom != null)
            _height -= this.PagerContainerBottom.offsetHeight;
        if (_height > 0)
            this.TableContainer.style.height = _height.toString() + 'px';
    }

    this.UpdateControlStateData = function () {
        var json = '{ColumnWidths : ';
        var s1 = '';
        for (var i = 0; i < this.HeaderCells.length; i++) {
            var h = this.HeaderCells[i];
            s1 += (s1.length > 0 ? ',' : '') + ((h.ColObject && h.ColObject.width) || h.jesWidth || h.offsetWidth);
        }
        json += '[' + s1 + ']';
        json += ', ScrollPosition : { x: ' + this.TableContainer.scrollLeft + ', y: ' + this.TableContainer.scrollTop + '}';
        json += '}';

        this.HiddenFieldData.value = json;
    }

    this.LoadControlStateData = function () {
        try {
            var state = eval("[" + this.HiddenFieldData.value + "]");
            if (typeof state != 'undefined' && state != null && state.length > 0)
                this._ControlState = state[0];
        } catch (e) { }
    }

    //Event handler for form submission
    this.OnFormSubmit = function () {
        this._ControlState.ScrollPosition.x = this.TableContainer.scrollLeft;
        this._ControlState.ScrollPosition.y = this.TableContainer.scrollTop;
        //update control state data of the column changes
        this.UpdateControlStateData();
    }

    //Attach an eventhandler for on load
    $$$.AttachEvent(window, 'load', this.OnPageLoadHandler, this);
}