Imports System.IO
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.DataStorage.HDSPack
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Microsoft.VisualBasic.Text

Public Class DataReader

    ReadOnly bin As StreamPack
    ReadOnly time As Integer
    ReadOnly dna_size As Integer

    Sub New(s As Stream)
        bin = New StreamPack(s, [readonly]:=True)
        time = GetPopulateSize.Length

        Dim args = bin.ReadText("/metadata/world_arguments.json") _
            .DoCall(AddressOf Strings.Trim) _
            .LoadJSON(Of Dictionary(Of String, String))

        dna_size = Integer.Parse(args!dna_capacity)
    End Sub

    Public Function GetPopulateSize() As Integer()
        Return Strings.Trim(bin.ReadText("/metadata/population_size.json")).LoadJSON(Of Integer())
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns>a vector keeps the element order with <see cref="BiologyCharacter.all_characters"/></returns>
    Public Iterator Function BiologyCharacterAbundance() As IEnumerable(Of Double())
        For i As Integer = 0 To time - 1
            Dim path As String = $"/data/{time}.dat"
            Dim s As Stream = bin.OpenFile(path, FileMode.Open, FileAccess.Read)
            Dim rd As New BinaryDataReader(s, Encodings.ASCII) With {
                .ByteOrder = ByteOrder.BigEndian
            }
            Dim biology As BiologyCharacters
            Dim level As Double
            Dim counts As New Dictionary(Of BiologyCharacters, Counter)

            For Each type As BiologyCharacters In BiologyCharacter.all_characters
                Call counts.Add(type, New Counter)
            Next

            Do While Not rd.EndOfStream
                rd.ReadInt32() ' guid
                rd.ReadInt32s(rd.ReadInt32) ' parent lineage

                For j As Integer = 1 To dna_size
                    biology = rd.ReadInt64
                    level = rd.ReadDouble
                    counts(biology).total += level
                    counts(biology).n += 1
                Next
            Loop

            Yield BiologyCharacter.all_characters _
                .Select(Function(a) counts(a).average) _
                .ToArray
        Next
    End Function

    Private Class Counter

        Public n As Integer = 0
        Public total As Double = 0

        Public ReadOnly Property average As Double
            Get
                If n = 0 Then
                    Return 0
                Else
                    Return total / n
                End If
            End Get
        End Property

    End Class

End Class
