﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System.Data

Namespace daWS
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute(ConfigurationName:="daWS.DataAccessWSSoap")>  _
    Public Interface DataAccessWSSoap
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/HelloWorld", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function HelloWorld() As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/CheckConnectionWS", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function CheckConnectionWS(ByVal strConnection As String) As Boolean
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/SelectCommandWS", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(MarshalByRefObject)),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(SqlParameter()))>  _
        Function SelectCommandWS(ByVal strConnection As String, ByVal strCommand As String, ByVal sqlParameters() As daWS.SqlParameter) As System.Data.DataSet
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/InsertCommandWS", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(MarshalByRefObject)),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(SqlParameter()))>  _
        Function InsertCommandWS(ByVal strConnection As String, ByVal strCommand As String, ByVal sqlParameters() As daWS.SqlParameter) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/UpdateCommandWS", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(MarshalByRefObject)),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(SqlParameter()))>  _
        Function UpdateCommandWS(ByVal strConnection As String, ByVal strCommand As String, ByVal sqlParameters() As daWS.SqlParameter) As Boolean
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/SaveCommandWS", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(MarshalByRefObject)),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(SqlParameter()))>  _
        Function SaveCommandWS(ByVal strConnection As String, ByVal strCommand As String, ByVal sqlParameters() As daWS.SqlParameter) As Integer
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/DeleteCommandWS", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(MarshalByRefObject)),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(SqlParameter()))>  _
        Function DeleteCommandWS(ByVal strConnection As String, ByVal strCommand As String, ByVal sqlParameters() As daWS.SqlParameter) As Boolean
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/WebSyncTableDataUpload", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(MarshalByRefObject)),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(SqlParameter()))>  _
        Sub WebSyncTableDataUpload(ByVal intWebSyncLogID As Integer, ByVal strSourceSQLServer As String, ByVal strSourceDatabase As String, ByVal strMasterConnection As String, ByVal strTableName As String, ByVal dsNewOrChanged As System.Data.DataSet)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/GetFileList", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(MarshalByRefObject)),  _
         System.ServiceModel.ServiceKnownTypeAttribute(GetType(SqlParameter()))>  _
        Function GetFileList(ByVal strConnection As String, ByVal strDir As String) As String
    End Interface
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/")>  _
    Partial Public Class SqlParameter
        Inherits DbParameter
        
        Private compareInfoField As SqlCompareOptions
        
        Private xmlSchemaCollectionDatabaseField As String
        
        Private xmlSchemaCollectionOwningSchemaField As String
        
        Private xmlSchemaCollectionNameField As String
        
        Private forceColumnEncryptionField As Boolean
        
        Private localeIdField As Integer
        
        Private sqlDbTypeField As SqlDbType
        
        Private sqlValueField As Object
        
        Private udtTypeNameField As String
        
        Private typeNameField As String
        
        Private offsetField As Integer
        
        Public Sub New()
            MyBase.New
            Me.forceColumnEncryptionField = false
        End Sub
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>  _
        Public Property CompareInfo() As SqlCompareOptions
            Get
                Return Me.compareInfoField
            End Get
            Set
                Me.compareInfoField = value
                Me.RaisePropertyChanged("CompareInfo")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>  _
        Public Property XmlSchemaCollectionDatabase() As String
            Get
                Return Me.xmlSchemaCollectionDatabaseField
            End Get
            Set
                Me.xmlSchemaCollectionDatabaseField = value
                Me.RaisePropertyChanged("XmlSchemaCollectionDatabase")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>  _
        Public Property XmlSchemaCollectionOwningSchema() As String
            Get
                Return Me.xmlSchemaCollectionOwningSchemaField
            End Get
            Set
                Me.xmlSchemaCollectionOwningSchemaField = value
                Me.RaisePropertyChanged("XmlSchemaCollectionOwningSchema")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=3)>  _
        Public Property XmlSchemaCollectionName() As String
            Get
                Return Me.xmlSchemaCollectionNameField
            End Get
            Set
                Me.xmlSchemaCollectionNameField = value
                Me.RaisePropertyChanged("XmlSchemaCollectionName")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=4),  _
         System.ComponentModel.DefaultValueAttribute(false)>  _
        Public Property ForceColumnEncryption() As Boolean
            Get
                Return Me.forceColumnEncryptionField
            End Get
            Set
                Me.forceColumnEncryptionField = value
                Me.RaisePropertyChanged("ForceColumnEncryption")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=5)>  _
        Public Property LocaleId() As Integer
            Get
                Return Me.localeIdField
            End Get
            Set
                Me.localeIdField = value
                Me.RaisePropertyChanged("LocaleId")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=6)>  _
        Public Property SqlDbType() As SqlDbType
            Get
                Return Me.sqlDbTypeField
            End Get
            Set
                Me.sqlDbTypeField = value
                Me.RaisePropertyChanged("SqlDbType")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=7)>  _
        Public Property SqlValue() As Object
            Get
                Return Me.sqlValueField
            End Get
            Set
                Me.sqlValueField = value
                Me.RaisePropertyChanged("SqlValue")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=8)>  _
        Public Property UdtTypeName() As String
            Get
                Return Me.udtTypeNameField
            End Get
            Set
                Me.udtTypeNameField = value
                Me.RaisePropertyChanged("UdtTypeName")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=9)>  _
        Public Property TypeName() As String
            Get
                Return Me.typeNameField
            End Get
            Set
                Me.typeNameField = value
                Me.RaisePropertyChanged("TypeName")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=10)>  _
        Public Property Offset() As Integer
            Get
                Return Me.offsetField
            End Get
            Set
                Me.offsetField = value
                Me.RaisePropertyChanged("Offset")
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.FlagsAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0"),  _
     System.SerializableAttribute(),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/")>  _
    Public Enum SqlCompareOptions
        
        '''<remarks/>
        None = 1
        
        '''<remarks/>
        IgnoreCase = 2
        
        '''<remarks/>
        IgnoreNonSpace = 4
        
        '''<remarks/>
        IgnoreKanaType = 8
        
        '''<remarks/>
        IgnoreWidth = 16
        
        '''<remarks/>
        BinarySort = 32
        
        '''<remarks/>
        BinarySort2 = 64
    End Enum
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0"),  _
     System.SerializableAttribute(),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/")>  _
    Public Enum SqlDbType
        
        '''<remarks/>
        BigInt
        
        '''<remarks/>
        Binary
        
        '''<remarks/>
        Bit
        
        '''<remarks/>
        [Char]
        
        '''<remarks/>
        DateTime
        
        '''<remarks/>
        [Decimal]
        
        '''<remarks/>
        Float
        
        '''<remarks/>
        Image
        
        '''<remarks/>
        Int
        
        '''<remarks/>
        Money
        
        '''<remarks/>
        NChar
        
        '''<remarks/>
        NText
        
        '''<remarks/>
        NVarChar
        
        '''<remarks/>
        Real
        
        '''<remarks/>
        UniqueIdentifier
        
        '''<remarks/>
        SmallDateTime
        
        '''<remarks/>
        SmallInt
        
        '''<remarks/>
        SmallMoney
        
        '''<remarks/>
        Text
        
        '''<remarks/>
        Timestamp
        
        '''<remarks/>
        TinyInt
        
        '''<remarks/>
        VarBinary
        
        '''<remarks/>
        VarChar
        
        '''<remarks/>
        [Variant]
        
        '''<remarks/>
        Xml
        
        '''<remarks/>
        Udt
        
        '''<remarks/>
        Structured
        
        '''<remarks/>
        [Date]
        
        '''<remarks/>
        Time
        
        '''<remarks/>
        DateTime2
        
        '''<remarks/>
        DateTimeOffset
    End Enum
    
    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(DbParameter)),  _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(SqlParameter)),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/")>  _
    Partial Public MustInherit Class MarshalByRefObject
        Inherits Object
        Implements System.ComponentModel.INotifyPropertyChanged
        
        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        
        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class
    
    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(SqlParameter)),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/")>  _
    Partial Public MustInherit Class DbParameter
        Inherits MarshalByRefObject
        
        Private dbTypeField As DbType
        
        Private directionField As ParameterDirection
        
        Private isNullableField As Boolean
        
        Private parameterNameField As String
        
        Private precisionField As Byte
        
        Private scaleField As Byte
        
        Private sizeField As Integer
        
        Private sourceColumnField As String
        
        Private sourceColumnNullMappingField As Boolean
        
        Private sourceVersionField As DataRowVersion
        
        Private valueField As Object
        
        Public Sub New()
            MyBase.New
            Me.directionField = ParameterDirection.Input
            Me.parameterNameField = ""
            Me.sourceColumnField = ""
            Me.sourceColumnNullMappingField = false
            Me.sourceVersionField = DataRowVersion.Current
        End Sub
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>  _
        Public Property DbType() As DbType
            Get
                Return Me.dbTypeField
            End Get
            Set
                Me.dbTypeField = value
                Me.RaisePropertyChanged("DbType")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=1),  _
         System.ComponentModel.DefaultValueAttribute(ParameterDirection.Input)>  _
        Public Property Direction() As ParameterDirection
            Get
                Return Me.directionField
            End Get
            Set
                Me.directionField = value
                Me.RaisePropertyChanged("Direction")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>  _
        Public Property IsNullable() As Boolean
            Get
                Return Me.isNullableField
            End Get
            Set
                Me.isNullableField = value
                Me.RaisePropertyChanged("IsNullable")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=3),  _
         System.ComponentModel.DefaultValueAttribute("")>  _
        Public Property ParameterName() As String
            Get
                Return Me.parameterNameField
            End Get
            Set
                Me.parameterNameField = value
                Me.RaisePropertyChanged("ParameterName")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=4)>  _
        Public Property Precision() As Byte
            Get
                Return Me.precisionField
            End Get
            Set
                Me.precisionField = value
                Me.RaisePropertyChanged("Precision")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=5)>  _
        Public Property Scale() As Byte
            Get
                Return Me.scaleField
            End Get
            Set
                Me.scaleField = value
                Me.RaisePropertyChanged("Scale")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=6)>  _
        Public Property Size() As Integer
            Get
                Return Me.sizeField
            End Get
            Set
                Me.sizeField = value
                Me.RaisePropertyChanged("Size")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=7),  _
         System.ComponentModel.DefaultValueAttribute("")>  _
        Public Property SourceColumn() As String
            Get
                Return Me.sourceColumnField
            End Get
            Set
                Me.sourceColumnField = value
                Me.RaisePropertyChanged("SourceColumn")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=8),  _
         System.ComponentModel.DefaultValueAttribute(false)>  _
        Public Property SourceColumnNullMapping() As Boolean
            Get
                Return Me.sourceColumnNullMappingField
            End Get
            Set
                Me.sourceColumnNullMappingField = value
                Me.RaisePropertyChanged("SourceColumnNullMapping")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=9),  _
         System.ComponentModel.DefaultValueAttribute(DataRowVersion.Current)>  _
        Public Property SourceVersion() As DataRowVersion
            Get
                Return Me.sourceVersionField
            End Get
            Set
                Me.sourceVersionField = value
                Me.RaisePropertyChanged("SourceVersion")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Order:=10)>  _
        Public Property Value() As Object
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = value
                Me.RaisePropertyChanged("Value")
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0"),  _
     System.SerializableAttribute(),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/")>  _
    Public Enum DbType
        
        '''<remarks/>
        AnsiString
        
        '''<remarks/>
        Binary
        
        '''<remarks/>
        [Byte]
        
        '''<remarks/>
        [Boolean]
        
        '''<remarks/>
        Currency
        
        '''<remarks/>
        [Date]
        
        '''<remarks/>
        DateTime
        
        '''<remarks/>
        [Decimal]
        
        '''<remarks/>
        [Double]
        
        '''<remarks/>
        Guid
        
        '''<remarks/>
        Int16
        
        '''<remarks/>
        Int32
        
        '''<remarks/>
        Int64
        
        '''<remarks/>
        [Object]
        
        '''<remarks/>
        [SByte]
        
        '''<remarks/>
        [Single]
        
        '''<remarks/>
        [String]
        
        '''<remarks/>
        Time
        
        '''<remarks/>
        UInt16
        
        '''<remarks/>
        UInt32
        
        '''<remarks/>
        UInt64
        
        '''<remarks/>
        VarNumeric
        
        '''<remarks/>
        AnsiStringFixedLength
        
        '''<remarks/>
        StringFixedLength
        
        '''<remarks/>
        Xml
        
        '''<remarks/>
        DateTime2
        
        '''<remarks/>
        DateTimeOffset
    End Enum
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0"),  _
     System.SerializableAttribute(),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/")>  _
    Public Enum ParameterDirection
        
        '''<remarks/>
        Input
        
        '''<remarks/>
        Output
        
        '''<remarks/>
        InputOutput
        
        '''<remarks/>
        ReturnValue
    End Enum
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0"),  _
     System.SerializableAttribute(),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/")>  _
    Public Enum DataRowVersion
        
        '''<remarks/>
        Original
        
        '''<remarks/>
        Current
        
        '''<remarks/>
        Proposed
        
        '''<remarks/>
        [Default]
    End Enum
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface DataAccessWSSoapChannel
        Inherits daWS.DataAccessWSSoap, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class DataAccessWSSoapClient
        Inherits System.ServiceModel.ClientBase(Of daWS.DataAccessWSSoap)
        Implements daWS.DataAccessWSSoap
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String)
            MyBase.New(endpointConfigurationName)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As String)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal binding As System.ServiceModel.Channels.Binding, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(binding, remoteAddress)
        End Sub
        
        Public Function HelloWorld() As String Implements daWS.DataAccessWSSoap.HelloWorld
            Return MyBase.Channel.HelloWorld
        End Function
        
        Public Function CheckConnectionWS(ByVal strConnection As String) As Boolean Implements daWS.DataAccessWSSoap.CheckConnectionWS
            Return MyBase.Channel.CheckConnectionWS(strConnection)
        End Function
        
        Public Function SelectCommandWS(ByVal strConnection As String, ByVal strCommand As String, ByVal sqlParameters() As daWS.SqlParameter) As System.Data.DataSet Implements daWS.DataAccessWSSoap.SelectCommandWS
            Return MyBase.Channel.SelectCommandWS(strConnection, strCommand, sqlParameters)
        End Function
        
        Public Function InsertCommandWS(ByVal strConnection As String, ByVal strCommand As String, ByVal sqlParameters() As daWS.SqlParameter) As String Implements daWS.DataAccessWSSoap.InsertCommandWS
            Return MyBase.Channel.InsertCommandWS(strConnection, strCommand, sqlParameters)
        End Function
        
        Public Function UpdateCommandWS(ByVal strConnection As String, ByVal strCommand As String, ByVal sqlParameters() As daWS.SqlParameter) As Boolean Implements daWS.DataAccessWSSoap.UpdateCommandWS
            Return MyBase.Channel.UpdateCommandWS(strConnection, strCommand, sqlParameters)
        End Function
        
        Public Function SaveCommandWS(ByVal strConnection As String, ByVal strCommand As String, ByVal sqlParameters() As daWS.SqlParameter) As Integer Implements daWS.DataAccessWSSoap.SaveCommandWS
            Return MyBase.Channel.SaveCommandWS(strConnection, strCommand, sqlParameters)
        End Function
        
        Public Function DeleteCommandWS(ByVal strConnection As String, ByVal strCommand As String, ByVal sqlParameters() As daWS.SqlParameter) As Boolean Implements daWS.DataAccessWSSoap.DeleteCommandWS
            Return MyBase.Channel.DeleteCommandWS(strConnection, strCommand, sqlParameters)
        End Function
        
        Public Sub WebSyncTableDataUpload(ByVal intWebSyncLogID As Integer, ByVal strSourceSQLServer As String, ByVal strSourceDatabase As String, ByVal strMasterConnection As String, ByVal strTableName As String, ByVal dsNewOrChanged As System.Data.DataSet) Implements daWS.DataAccessWSSoap.WebSyncTableDataUpload
            MyBase.Channel.WebSyncTableDataUpload(intWebSyncLogID, strSourceSQLServer, strSourceDatabase, strMasterConnection, strTableName, dsNewOrChanged)
        End Sub
        
        Public Function GetFileList(ByVal strConnection As String, ByVal strDir As String) As String Implements daWS.DataAccessWSSoap.GetFileList
            Return MyBase.Channel.GetFileList(strConnection, strDir)
        End Function
    End Class
End Namespace
