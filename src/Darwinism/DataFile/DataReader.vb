Imports System.IO
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.DataStorage.HDSPack
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.DataFrame
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
        For Each i As Integer In Tqdm.Wrap(Enumerable.Range(0, time).ToArray, useColor:=True)
            Dim counts As New Dictionary(Of BiologyCharacters, Counter)

            For Each type As BiologyCharacters In BiologyCharacter.all_characters
                Call counts.Add(type, New Counter)
            Next

            For Each c As CreatureData In GetEraCreatures(era:=i)
                For Each chr As BiologyCharacter In c.Heredity
                    counts(chr.Character).total += chr.Level
                    counts(chr.Character).n += 1
                Next
            Next

            Yield BiologyCharacter.all_characters _
                .Select(Function(a) counts(a).average) _
                .ToArray
        Next
    End Function

    Private Iterator Function GetCreatures(rd As BinaryDataReader) As IEnumerable(Of CreatureData)
        Dim pop_size As Integer = rd.ReadInt32

        For i As Integer = 0 To pop_size - 1
            Yield ParseOne(rd)
        Next
    End Function

    Private Function ParseOne(rd As BinaryDataReader) As CreatureData
        Dim biology As BiologyCharacters
        Dim level As Double
        Dim guid = rd.ReadInt32() ' guid
        Dim xyz As Integer() = rd.ReadInt32s(3) ' position
        Dim parent = rd.ReadInt32s(rd.ReadInt32) ' parent lineage
        Dim era = rd.ReadInt32() ' era
        Dim age = rd.ReadInt32() ' age
        Dim lifespan = rd.ReadInt32() ' lifespan
        Dim energy As Double = rd.ReadDouble  ' energy
        Dim heredity As BiologyCharacter() = New BiologyCharacter(dna_size - 1) {}

        For j As Integer = 0 To dna_size - 1
            biology = rd.ReadInt64
            level = rd.ReadDouble
            heredity(j) = New BiologyCharacter(biology, level)
        Next

        Return (guid, xyz, parent, era, age, lifespan, energy, heredity)
    End Function

    Public Function GetEraCreatures(era As Integer) As IEnumerable(Of CreatureData)
        Dim path As String = $"/data/{era}.dat"
        Dim s As Stream = bin.OpenFile(path, FileMode.Open, FileAccess.Read)
        Dim rd As New BinaryDataReader(s, Encodings.ASCII) With {
            .ByteOrder = ByteOrder.BigEndian
        }

        Return GetCreatures(rd)
    End Function

    Public Function CreatureMatrix() As DataFrame
        Dim characters As New Dictionary(Of BiologyCharacters, List(Of Double))
        Dim era As New List(Of Integer)
        Dim creature_id As New List(Of String)
        Dim obj_index As New Index(Of String)

        For Each character As BiologyCharacters In BiologyCharacter.all_characters
            Call characters.Add(character, New List(Of Double))
        Next

        For Each i As Integer In Tqdm.Wrap(Enumerable.Range(0, time).ToArray, useColor:=True)
            For Each c As CreatureData In GetEraCreatures(era:=i)
                Dim obj_id As String = $"{c.Era} - {c.Guid}"

                ' has already been added
                If obj_id Like obj_index Then
                    Continue For
                Else
                    obj_index.Add(obj_id)
                End If

                era.Add(c.Era)
                creature_id.Add(c.Guid)

                Dim character_groups = c.Heredity _
                    .GroupBy(Function(ci) ci.Character) _
                    .ToDictionary(Function(ci) ci.Key,
                                  Function(ci)
                                      Return ci.ToArray
                                  End Function)

                For Each character As BiologyCharacters In BiologyCharacter.all_characters
                    If character_groups.ContainsKey(character) Then
                        Call characters(character).Add(Aggregate ci As BiologyCharacter
                                                       In character_groups(character)
                                                       Into Sum(ci.Level))
                    Else
                        Call characters(character).Add(0)
                    End If
                Next
            Next
        Next

        Dim df As New DataFrame With {
            .rownames = creature_id.ToArray,
            .features = New Dictionary(Of String, FeatureVector)
        }

        Call df.add("Era", era)

        For Each character As BiologyCharacters In characters.Keys
            Call df.add(character.Description, characters(character))
        Next

        Return df
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

        Public Overrides Function ToString() As String
            Return $"{total}/{n} = {average}"
        End Function

    End Class

End Class
