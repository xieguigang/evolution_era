Imports System.Drawing
Imports System.IO
Imports evolution_era
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("Era")>
Public Module Era

    ''' <summary>
    ''' create a new world
    ''' </summary>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("world")>
    <RApiReturn(GetType(GeographyPlate))>
    Public Function world(map As Image,
                          Optional height As Integer = 3,
                          Optional reproductive_isolation As Double = 0.9,
                          Optional reproduce_rate As Double = 0.5,
                          Optional dna_size As Integer = 6,
                          Optional natural_death As Integer = 50,
                          Optional sexual_maturity As Integer = 3,
                          Optional predation_diff As Integer = 2,
                          Optional water_color As String = "#0026ff",
                          Optional env As Environment = Nothing) As Object

        Dim args As New WorldParameters With {
            .reproduce_rate = reproduce_rate,
            .reproductive_isolation = reproductive_isolation,
            .dna_capacity = dna_size,
            .natural_death = natural_death,
            .sexual_maturity = sexual_maturity,
            .predation_diff = predation_diff
        }

        Return New GeographyPlate(args, map, height:=height, water:=water_color)
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
        Dim bar As Tqdm.ProgressBar = Nothing
        Dim pop_size As Integer = 0

        Call world.Init(era:=0)
        Call save.Record(0, reader.GetCreatures)

        For Each i As Integer In Tqdm.Wrap(Enumerable.Range(1, time + 1).ToArray, bar:=bar, width:=60, useColor:=True)
            Call world.TimeElapsed(era:=i)
            Call save.Record(i, reader.GetCreatures, getSize:=pop_size)
            Call bar.SetLabel($"population size: {pop_size}")

            If pop_size <= 0 Then
                Call world.Init(era:=i)
            End If
        Next

        Call save.Dispose()

        Return True
    End Function

End Module
