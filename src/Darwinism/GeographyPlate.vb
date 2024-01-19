Imports System.Drawing
Imports Microsoft.VisualBasic.Data.GraphTheory.GridGraph
Imports rand = Microsoft.VisualBasic.Math.RandomExtensions

Public Class GeographyPlate

    Dim spatial As Spatial3D(Of Position)
    Dim world As WorldParameters
    Dim size As Size
    Dim height As Integer

    Sub New(args As WorldParameters, size As Size, Optional height As Integer = 3)
        Dim points As New List(Of Position)
        Dim c As Position

        For x As Integer = 0 To size.Width
            For y As Integer = 0 To size.Height
                For z As Integer = 0 To height
                    c = New Position(x, y, z)
                    points.Add(c)

                    If z = 0 Then
                        ' is water or land
                        If rand.NextDouble > 0.4 Then
                            c.Geography = GeographyType.Water
                        Else
                            c.Geography = GeographyType.Land
                        End If
                    End If
                Next
            Next
        Next

        Me.size = size
        Me.height = height
        Me.world = args
        Me.spatial = Spatial3D(Of Position).CreateSpatial3D(Of Position)(points)
    End Sub

    Public Sub Init(capacity As Integer)
        Dim x As Integer = rand.NextInteger(size.Width)
        Dim y As Integer = rand.NextInteger(size.Height)
        Dim position As Position = spatial.GetData(x, y, z:=0)
        Dim characters = Creature.Empty(capacity).ToArray

        If position.Geography = GeographyType.Land Then
            characters(0).SetCharacter(BiologyCharacters.Foot, 1)
        Else
            characters(0).SetCharacter(BiologyCharacters.FishFin, 1)
        End If

        position.Creature = New Creature(characters)
    End Sub

    ''' <summary>
    ''' run a loop
    ''' </summary>
    Public Sub TimeElapsed()
        For Each layer As Grid(Of Position) In spatial.ZLayers
            For Each point As Position In layer.EnumerateData
                If point.Creature Is Nothing Then
                    Continue For
                End If

                Dim nearby As Position() = spatial.Query(point.X, point.Y, point.Z).ToArray
                Dim tryOne As Position = nearby.Random

                Call point.TryMoveTo(another:=tryOne, nearby, world)
            Next
        Next
    End Sub

    Public Iterator Function GetPositions() As IEnumerable(Of Position)
        For Each layer As Grid(Of Position) In spatial.ZLayers
            For Each point As Position In layer.EnumerateData
                Yield point
            Next
        Next
    End Function

End Class

Public Enum GeographyType
    Water
    Land
    Air
End Enum

Public Structure WorldParameters
    Dim reproductive_isolation As Double
    Dim reproduce_rate As Double
End Structure