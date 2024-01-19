Imports System.IO
Imports Microsoft.VisualBasic.DataStorage.HDSPack
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.Serialization.JSON

Public Class DataReader

    ReadOnly bin As StreamPack

    Sub New(s As Stream)
        bin = New StreamPack(s, [readonly]:=True)
    End Sub

    Public Function GetPopulateSize() As Integer()
        Return Strings.Trim(bin.ReadText("/metadata/population_size.json")).LoadJSON(Of Integer())
    End Function

End Class
