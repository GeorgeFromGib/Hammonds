﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class releases {
    
    private releasesReleaseNumber[] releaseNumberField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("releaseNumber")]
    public releasesReleaseNumber[] releaseNumber {
        get {
            return this.releaseNumberField;
        }
        set {
            this.releaseNumberField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class releasesReleaseNumber {
    
    private string[] entryField;
    
    private string releaseField;
    private string dateField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("entry")]
    public string[] entry {
        get {
            return this.entryField;
        }
        set {
            this.entryField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string release {
        get {
            return this.releaseField;
        }
        set {
            this.releaseField = value;
        }
    }

    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string date
    {
        get
        {
            return this.dateField;
        }
        set
        {
            this.dateField = value;
        }
    }
}
