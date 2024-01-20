Imports System.Drawing
Imports System.IO
Imports evolution_era
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization

<Package("Era")>
Public Module Era

    ''' <summary>
    ''' create a new world
    ''' </summary>
    ''' <param name="size"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("world")>
    <RApiReturn(GetType(GeographyPlate))>
    Public Function world(<RRawVectorArgument(TypeCodes.integer)>
                          Optional size As Object = "10,10,3",
                          Optional reproductive_isolation As Double = 0.9,
                          Optional reproduce_rate As Double = 0.5,
                          Optional dna_size As Integer = 5,
                          Optional natural_death As Integer = 100,
                          Optional env As Environment = Nothing) As Object

        Dim args As New WorldParameters With {
            .reproduce_rate = reproduce_rate,
            .reproductive_isolation = reproductive_isolation,
            .dna_capacity = dna_size,
            .natural_death = natural_death
        }
        Dim dims As Integer() = CLRVector.asInteger(size)

        If dims.IsNullOrEmpty Then
            Return Internal.debug.stop("the dimension size of the new world could not be nothing!", env)
        ElseIf dims.Length = 2 Then
            dims = {dims(0), dims(1), 3}
        ElseIf dims.Length = 1 Then
            dims = {dims(0), dims(0), dims(0)}
        End If

        Return New GeographyPlate(args, New Size(dims(0), dims(1)), height:=dims(2))
    End Function

    ''' <summary>
    ''' run the simulation
    ''' </summary>
    ''' <param name="world"></param>
    ''' <param name="file">
    ''' the result save file
    ''' </param>
    ''' <param name="time"></param>
    ''' <returns></returns>
    <ExportAPI("evolve")>
    Public Function evolve(world As GeographyPlate, file As Object,
                           Optional time As Integer = 1000,
                           Optional env As Environment = Nothing) As Object

        Dim buf = SMRUCC.Rsharp.GetFileStream(file, IO.FileAccess.Write, env)

        If buf Like GetType(Message) Then
            Return buf.TryCast(Of Message)
        End If

        Dim save As New DataWriter(buf.TryCast(Of Stream), world.Arguments)
        Dim reader As New DataAdapter(world)

        Call world.Init()
        Call save.Record(0, reader.GetCreatures)

        For Each i As Integer In Tqdm.Wrap(Enumerable.Range(1, time + 1).ToArray, useColor:=True)
            Call world.TimeElapsed()
            Call save.Record(i, reader.GetCreatures)
        Next

        Call save.Dispose()

        Return True
    End Function

End Module
