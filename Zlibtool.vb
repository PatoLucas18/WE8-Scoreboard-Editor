Imports System.Runtime.InteropServices

Public Class Zlibtool
    
    <DllImport("zlib1.dll", EntryPoint:="compress", SetLastError:=True, CharSet:=CharSet.Unicode, ExactSpelling:=True, CallingConvention:=CallingConvention.Cdecl)> _
    Friend Shared Function CompressByteArray(ByVal dest As Byte(), ByRef destLen As Integer, ByVal src As Byte(), ByVal srcLen As Integer) As Integer
    End Function

    <DllImport("zlib1.dll", EntryPoint:="uncompress", CallingConvention:=CallingConvention.Cdecl)> _
    Friend Shared Function UncompressByteArray(ByVal dest As Byte(), ByRef destLen As Integer, ByVal src As Byte(), ByVal srcLen As Integer) As Integer
    End Function
End Class
