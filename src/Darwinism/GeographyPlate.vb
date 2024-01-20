Imports System.Drawing
Imports Microsoft.VisualBasic.Data.GraphTheory.GridGraph
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports rand = Microsoft.VisualBasic.Math.RandomExtensions

Public Class GeographyPlate

    Dim spatial As Spatial3D(Of Position)
    Dim world As WorldParameters
    Dim size As Size
    Dim height As Integer

    Public ReadOnly Property Arguments As WorldParameters
        Get
            Return world
        End Get
    End Property

    Sub New(args As WorldParameters, map As Image, Optional height As Integer = 3, Optional water As String = "#0026ff")
        Dim points As New List(Of Position)
        Dim c As Position
        Dim terrain As BitmapBuffer = BitmapBuffer.FromImage(map)
        Dim water_color As Color = water.TranslateColor

        For x As Integer = 0 To size.Width
            For y As Integer = 0 To size.Height
                For z As Integer = 0 To height
                    c = New Position(x, y, z)
                    points.Add(c)

                    If z = 0 Then
                        ' is water or land
                        If terrain.GetPixel(x, y).Equals(water_color, tolerance:=15) Then
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

    Public Sub Init()
        Dim x As Integer = rand.NextInteger(size.Width)
        Dim y As Integer = rand.NextInteger(size.Height)
        Dim position As Position = spatial.GetData(x, y, z:=0)
        Dim characters = Creature.Empty(capacity:=world.dna_capacity).ToArray

        If position.Geography = GeographyType.Land Then
            characters(0).SetCharacter(BiologyCharacters.Foot, 1)
        Else
            characters(0).SetCharacter(BiologyCharacters.FishFin, 1)
        End If

        position.Creature = New Creature(characters).SetLifeSpan(world.natural_death, era:=0)
    End Sub

    ''' <summary>
    ''' run a loop
    ''' </summary>
    Public Sub TimeElapsed(era As Integer)
        For Each layer As Grid(Of Position) In spatial.ZLayers
            For Each point As Position In layer.EnumerateData
                If point.Creature Is Nothing Then
                    Continue For
                End If
                If point.Creature.TimeElapsed Then
                    ' natural_death: age increased to life span
                    point.Creature = Nothing
                    Continue For
                End If

                Dim nearby As Position() = spatial _
                    .Query(point.X, point.Y, point.Z) _
                    .Where(Function(a) a IsNot point) _
                    .ToArray
                Dim tryOne As Position = nearby.Random

                Call point.TryMoveTo(another:=tryOne, nearby, era, world)
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
    Dim dna_capacity As Integer
    Dim natural_death As Integer

    Public Function GetObject() As Dictionary(Of String, String)
        Return New Dictionary(Of String, String) From {
            {"reproductive_isolation", reproductive_isolation},
            {"reproduce_rate", reproduce_rate},
            {"dna_capacity", dna_capacity},
            {"natural death", natural_death}
        }
    End Function

End Structure