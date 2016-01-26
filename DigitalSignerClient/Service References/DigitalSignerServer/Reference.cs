﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microarea.DigitalSignerClient.DigitalSignerServer {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.microarea.it/", ConfigurationName="DigitalSignerServer.DigitalSignerSoap")]
    public interface DigitalSignerSoap {
        
        // CODEGEN: Generating message contract since element name string1 from namespace http://www.microarea.it/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.microarea.it/SignBootstrapper", ReplyAction="*")]
        Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperResponse SignBootstrapper(Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.microarea.it/SignBootstrapper", ReplyAction="*")]
        System.Threading.Tasks.Task<Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperResponse> SignBootstrapperAsync(Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperRequest request);
        
        // CODEGEN: Generating message contract since element name string1 from namespace http://www.microarea.it/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.microarea.it/SignClickOnceManifest", ReplyAction="*")]
        Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestResponse SignClickOnceManifest(Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.microarea.it/SignClickOnceManifest", ReplyAction="*")]
        System.Threading.Tasks.Task<Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestResponse> SignClickOnceManifestAsync(Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SignBootstrapperRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SignBootstrapper", Namespace="http://www.microarea.it/", Order=0)]
        public Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperRequestBody Body;
        
        public SignBootstrapperRequest() {
        }
        
        public SignBootstrapperRequest(Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.microarea.it/")]
    public partial class SignBootstrapperRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string string1;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string string2;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string string3;
        
        public SignBootstrapperRequestBody() {
        }
        
        public SignBootstrapperRequestBody(string string1, string string2, string string3) {
            this.string1 = string1;
            this.string2 = string2;
            this.string3 = string3;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SignBootstrapperResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SignBootstrapperResponse", Namespace="http://www.microarea.it/", Order=0)]
        public Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperResponseBody Body;
        
        public SignBootstrapperResponse() {
        }
        
        public SignBootstrapperResponse(Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.microarea.it/")]
    public partial class SignBootstrapperResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string SignBootstrapperResult;
        
        public SignBootstrapperResponseBody() {
        }
        
        public SignBootstrapperResponseBody(string SignBootstrapperResult) {
            this.SignBootstrapperResult = SignBootstrapperResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SignClickOnceManifestRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SignClickOnceManifest", Namespace="http://www.microarea.it/", Order=0)]
        public Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestRequestBody Body;
        
        public SignClickOnceManifestRequest() {
        }
        
        public SignClickOnceManifestRequest(Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.microarea.it/")]
    public partial class SignClickOnceManifestRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string string1;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string string2;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string string3;
        
        public SignClickOnceManifestRequestBody() {
        }
        
        public SignClickOnceManifestRequestBody(string string1, string string2, string string3) {
            this.string1 = string1;
            this.string2 = string2;
            this.string3 = string3;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SignClickOnceManifestResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SignClickOnceManifestResponse", Namespace="http://www.microarea.it/", Order=0)]
        public Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestResponseBody Body;
        
        public SignClickOnceManifestResponse() {
        }
        
        public SignClickOnceManifestResponse(Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.microarea.it/")]
    public partial class SignClickOnceManifestResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string SignClickOnceManifestResult;
        
        public SignClickOnceManifestResponseBody() {
        }
        
        public SignClickOnceManifestResponseBody(string SignClickOnceManifestResult) {
            this.SignClickOnceManifestResult = SignClickOnceManifestResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface DigitalSignerSoapChannel : Microarea.DigitalSignerClient.DigitalSignerServer.DigitalSignerSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DigitalSignerSoapClient : System.ServiceModel.ClientBase<Microarea.DigitalSignerClient.DigitalSignerServer.DigitalSignerSoap>, Microarea.DigitalSignerClient.DigitalSignerServer.DigitalSignerSoap {
        
        public DigitalSignerSoapClient() {
        }
        
        public DigitalSignerSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DigitalSignerSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DigitalSignerSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DigitalSignerSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperResponse Microarea.DigitalSignerClient.DigitalSignerServer.DigitalSignerSoap.SignBootstrapper(Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperRequest request) {
            return base.Channel.SignBootstrapper(request);
        }
        
        public string SignBootstrapper(string string1, string string2, string string3) {
            Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperRequest inValue = new Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperRequest();
            inValue.Body = new Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperRequestBody();
            inValue.Body.string1 = string1;
            inValue.Body.string2 = string2;
            inValue.Body.string3 = string3;
            Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperResponse retVal = ((Microarea.DigitalSignerClient.DigitalSignerServer.DigitalSignerSoap)(this)).SignBootstrapper(inValue);
            return retVal.Body.SignBootstrapperResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperResponse> Microarea.DigitalSignerClient.DigitalSignerServer.DigitalSignerSoap.SignBootstrapperAsync(Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperRequest request) {
            return base.Channel.SignBootstrapperAsync(request);
        }
        
        public System.Threading.Tasks.Task<Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperResponse> SignBootstrapperAsync(string string1, string string2, string string3) {
            Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperRequest inValue = new Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperRequest();
            inValue.Body = new Microarea.DigitalSignerClient.DigitalSignerServer.SignBootstrapperRequestBody();
            inValue.Body.string1 = string1;
            inValue.Body.string2 = string2;
            inValue.Body.string3 = string3;
            return ((Microarea.DigitalSignerClient.DigitalSignerServer.DigitalSignerSoap)(this)).SignBootstrapperAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestResponse Microarea.DigitalSignerClient.DigitalSignerServer.DigitalSignerSoap.SignClickOnceManifest(Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestRequest request) {
            return base.Channel.SignClickOnceManifest(request);
        }
        
        public string SignClickOnceManifest(string string1, string string2, string string3) {
            Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestRequest inValue = new Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestRequest();
            inValue.Body = new Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestRequestBody();
            inValue.Body.string1 = string1;
            inValue.Body.string2 = string2;
            inValue.Body.string3 = string3;
            Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestResponse retVal = ((Microarea.DigitalSignerClient.DigitalSignerServer.DigitalSignerSoap)(this)).SignClickOnceManifest(inValue);
            return retVal.Body.SignClickOnceManifestResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestResponse> Microarea.DigitalSignerClient.DigitalSignerServer.DigitalSignerSoap.SignClickOnceManifestAsync(Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestRequest request) {
            return base.Channel.SignClickOnceManifestAsync(request);
        }
        
        public System.Threading.Tasks.Task<Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestResponse> SignClickOnceManifestAsync(string string1, string string2, string string3) {
            Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestRequest inValue = new Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestRequest();
            inValue.Body = new Microarea.DigitalSignerClient.DigitalSignerServer.SignClickOnceManifestRequestBody();
            inValue.Body.string1 = string1;
            inValue.Body.string2 = string2;
            inValue.Body.string3 = string3;
            return ((Microarea.DigitalSignerClient.DigitalSignerServer.DigitalSignerSoap)(this)).SignClickOnceManifestAsync(inValue);
        }
    }
}
