﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.33440.
// 
namespace CdsCustomizationValidator.Infrastructure.DTO {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    public partial class AttributePrefixRule {
        
        private string schemaPrefixField;
        
        /// <remarks/>
        public string SchemaPrefix {
            get {
                return this.schemaPrefixField;
            }
            set {
                this.schemaPrefixField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EntityPrefixRule {
        
        private string schemaPrefixField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string schemaPrefix {
            get {
                return this.schemaPrefixField;
            }
            set {
                this.schemaPrefixField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DisallowSolutionToOwnManagedEntitiesRule {
        
        private bool allowField;
        
        public DisallowSolutionToOwnManagedEntitiesRule() {
            this.allowField = false;
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool allow {
            get {
                return this.allowField;
            }
            set {
                this.allowField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    public partial class RegexRule {
        
        private string patternField;
        
        private RuleScope scopeField;
        
        /// <remarks/>
        public string Pattern {
            get {
                return this.patternField;
            }
            set {
                this.patternField = value;
            }
        }
        
        /// <remarks/>
        public RuleScope Scope {
            get {
                return this.scopeField;
            }
            set {
                this.scopeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    public enum RuleScope {
        
        /// <remarks/>
        Entity,
        
        /// <remarks/>
        Attribute,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class CustomizationRule {
        
        private DisallowSolutionToOwnManagedEntitiesRule disallowSolutionToOwnManagedEntitiesRuleField;
        
        private EntityPrefixRule entityPrefixRuleField;
        
        /// <remarks/>
        public DisallowSolutionToOwnManagedEntitiesRule DisallowSolutionToOwnManagedEntitiesRule {
            get {
                return this.disallowSolutionToOwnManagedEntitiesRuleField;
            }
            set {
                this.disallowSolutionToOwnManagedEntitiesRuleField = value;
            }
        }
        
        /// <remarks/>
        public EntityPrefixRule EntityPrefixRule {
            get {
                return this.entityPrefixRuleField;
            }
            set {
                this.entityPrefixRuleField = value;
            }
        }
    }
}