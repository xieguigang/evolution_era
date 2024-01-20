
Imports System.Drawing
Imports System.IO
Imports evolution_era
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("Analysis")>
Public Module DataFile

    ''' <summary>
    ''' open the simulation result file
    ''' </summary>
    ''' <param name="file"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("open")>
    <RApiReturn(GetType(DataReader))>
    Public Function open(<RRawVectorArgument> file As Object, Optional env As Environment = Nothing) As Object
        Dim buf = SMRUCC.Rsharp.GetFileStream(file, IO.FileAccess.Read, env)

        If buf Like GetType(Message) Then
            Return buf.TryCast(Of Message)
        End If

        Return New DataReader(buf.TryCast(Of Stream))
    End Function

    <ExportAPI("population_size")>
    Public Function population_size(file As DataReader) As Object
        Return file.GetPopulateSize
    End Function

    <ExportAPI("biology_abundance")>
    Public Function biologyCharacterAbundance(file As DataReader) As Object
        Dim df As New dataframe With {.columns = New Dictionary(Of String, Array)}
        Dim levels = file.BiologyCharacterAbundance.ToArray
        Dim i As Integer = 0

        For Each type As BiologyCharacters In BiologyCharacter.all_characters
            df.add(type.Description, levels.Select(Function(t) t(i)))
            i += 1
        Next

        Return df
    End Function

    ''' <summary>
    ''' get all creatures from the simulation result
    ''' </summary>
    ''' <param name="file"></param>
    ''' <returns></returns>
    <ExportAPI("creatures")>
    Public Function creatures(file As DataReader) As Object
        Dim mat = file.CreatureMatrix
        Dim df As New dataframe With {
            .columns = New Dictionary(Of String, Array),
            .rownames = mat.rownames
        }

        For Each name As String In mat.featureNames
            Call df.add(name, mat(name).vector)
        Next

        Return df
    End Function

    Private Iterator Function CharacterColors() As IEnumerable(Of (BiologyCharacters, Brush))
        Dim characters = BiologyCharacter.all_characters.ToArray
        Dim colors = Designer.GetColors("paper", characters.Length)

        For i As Integer = 0 To characters.Length - 1
            Yield (characters(i), New SolidBrush(colors(i)))
        Next
    End Function

    <ExportAPI("distributionMap")>
    Public Function distributionMap(file As DataReader, era As Integer) As Image
        Dim map As Image = file.GetWorldMap
        Dim g As Graphics = Graphics.FromImage(map)

        Static colors As Dictionary(Of BiologyCharacters, Brush) = CharacterColors() _
            .ToDictionary(Function(c) c.Item1,
                          Function(c)
                              Return c.Item2
                          End Function)

        For Each c As CreatureData In file.GetEraCreatures(era)
            Call g.DrawCircle(New PointF(c.X, c.Y), 2, colors(c.GetTopCharacter))
        Next

        Call g.Flush()

        Return map
    End Function

End Module
