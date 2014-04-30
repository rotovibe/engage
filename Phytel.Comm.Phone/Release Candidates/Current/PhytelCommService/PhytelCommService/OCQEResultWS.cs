
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.SoapTypeAttribute(Namespace="http://phytel.com/OCQEResultWS")]
public partial class reqObject_Array {
    
    private reqObject[] gfRequestField;
    
    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
    public reqObject[] gfRequest {
        get {
            return this.gfRequestField;
        }
        set {
            this.gfRequestField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.SoapTypeAttribute(Namespace="http://phytel.com/OCQEResultWS")]
public partial class reqObject {
    
    private int sendIDField;
    
    private int contractIDField;
    
    private int facilityIDField;
    
    private int providerIDField;
    
    private int activityIDField;
    
    private System.Nullable<int> scheduleIDField;
    
    private int commEventIDField;
    
    private string callIDField;
    
    private System.DateTime callDistributorTimeOfCallField;
    
    private long callDurationField;
    
    private string resultCodeField;
    
    private string resultStatusField;
    
    private string hangupLocaleField;
    
    private bool recordingField;
    
    private string fileLocationField;
    
    private string fileNameField;
    
    /// <remarks/>
    public int SendID {
        get {
            return this.sendIDField;
        }
        set {
            this.sendIDField = value;
        }
    }
    
    /// <remarks/>
    public int ContractID {
        get {
            return this.contractIDField;
        }
        set {
            this.contractIDField = value;
        }
    }
    
    /// <remarks/>
    public int FacilityID {
        get {
            return this.facilityIDField;
        }
        set {
            this.facilityIDField = value;
        }
    }
    
    /// <remarks/>
    public int ProviderID {
        get {
            return this.providerIDField;
        }
        set {
            this.providerIDField = value;
        }
    }
    
    /// <remarks/>
    public int ActivityID {
        get {
            return this.activityIDField;
        }
        set {
            this.activityIDField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
    public System.Nullable<int> ScheduleID {
        get {
            return this.scheduleIDField;
        }
        set {
            this.scheduleIDField = value;
        }
    }
    
    /// <remarks/>
    public int CommEventID {
        get {
            return this.commEventIDField;
        }
        set {
            this.commEventIDField = value;
        }
    }
    
    /// <remarks/>
    public string CallID {
        get {
            return this.callIDField;
        }
        set {
            this.callIDField = value;
        }
    }
    
    /// <remarks/>
    public System.DateTime CallDistributorTimeOfCall {
        get {
            return this.callDistributorTimeOfCallField;
        }
        set {
            this.callDistributorTimeOfCallField = value;
        }
    }
    
    /// <remarks/>
    public long CallDuration {
        get {
            return this.callDurationField;
        }
        set {
            this.callDurationField = value;
        }
    }
    
    /// <remarks/>
    public string ResultCode {
        get {
            return this.resultCodeField;
        }
        set {
            this.resultCodeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
    public string ResultStatus {
        get {
            return this.resultStatusField;
        }
        set {
            this.resultStatusField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
    public string HangupLocale {
        get {
            return this.hangupLocaleField;
        }
        set {
            this.hangupLocaleField = value;
        }
    }
    
    /// <remarks/>
    public bool Recording {
        get {
            return this.recordingField;
        }
        set {
            this.recordingField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
    public string FileLocation {
        get {
            return this.fileLocationField;
        }
        set {
            this.fileLocationField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
    public string FileName {
        get {
            return this.fileNameField;
        }
        set {
            this.fileNameField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.SoapTypeAttribute(Namespace="http://phytel.com/OCQEResultWS")]
public partial class respObject {
    
    private string callIDField;
    
    private string returnStatusField;
    
    private string returnStringField;
    
    /// <remarks/>
    public string CallID {
        get {
            return this.callIDField;
        }
        set {
            this.callIDField = value;
        }
    }
    
    /// <remarks/>
    public string returnStatus {
        get {
            return this.returnStatusField;
        }
        set {
            this.returnStatusField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
    public string returnString {
        get {
            return this.returnStringField;
        }
        set {
            this.returnStringField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.SoapTypeAttribute(Namespace="http://phytel.com/OCQEResultWS")]
public partial class respObject_Array {
    
    private respObject[] gfResponseField;
    
    /// <remarks/>
    [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
    public respObject[] gfResponse {
        get {
            return this.gfResponseField;
        }
        set {
            this.gfResponseField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
public delegate void sendOCQEResultsCompletedEventHandler(object sender, sendOCQEResultsCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class sendOCQEResultsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal sendOCQEResultsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public respObject_Array Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((respObject_Array)(this.results[0]));
        }
    }
}
