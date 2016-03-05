﻿Imports System.Data.Linq.Mapping
Imports System.Linq
Imports Microsoft.VisualBasic.Serialization

Namespace Services.ScriptBuilder

    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=False, Inherited:=True)>
    Public Class Parameter : Inherits Attribute

        Public ReadOnly Property [Optional] As Boolean
        Public ReadOnly Property Name As String
        Public ReadOnly Property Type As ValueTypes

        ''' <summary>
        ''' API会自动根据类型来修正路径之中的分隔符的
        ''' </summary>
        ''' <param name="name"></param>
        ''' <param name="opt">Is this parameter optional?</param>
        Sub New(name As String, Optional type As ValueTypes = ValueTypes.String, Optional opt As Boolean = False)
            Me.Name = name
            Me.[Optional] = opt
            Me.Type = type
        End Sub

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class

    Public Enum ValueTypes
        [String] = 0
        ''' <summary>
        ''' 这个是一个字符串类型的文件路径
        ''' </summary>
        [Path]
    End Enum

    <AttributeUsage(AttributeTargets.Class Or AttributeTargets.Struct, AllowMultiple:=False, Inherited:=True)>
    Public Class RFunc : Inherits Attribute

        Public ReadOnly Property Name As String

        ''' <summary>
        ''' Declaring a R function
        ''' </summary>
        ''' <param name="name">R function name</param>
        Sub New(name As String)
            Me.Name = name
        End Sub

        Public Overrides Function ToString() As String
            Return Name
        End Function

        Public Shared Narrowing Operator CType(rfunc As RFunc) As String
            Return rfunc.Name
        End Operator
    End Class
End Namespace