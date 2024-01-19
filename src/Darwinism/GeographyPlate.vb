Imports System.Drawing
Imports Microsoft.VisualBasic.Data.GraphTheory.GridGraph
Imports Microsoft.VisualBasic.Imaging
Imports rand = Microsoft.VisualBasic.Math.RandomExtensions

Public Class GeographyPlate

    Dim spatial As Spatial3D(Of Position)
    Dim world As WorldParameters

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

        Me.world = args
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

End Class

Public Class Position : Implements IPoint3D

    Public Property Creature As Creature
    Public Property Z As Integer Implements IPoint3D.Z
    Public Property X As Integer Implements RasterPixel.X
    Public Property Y As Integer Implements RasterPixel.Y
    Public Property Geography As GeographyType

    Sub New(x As Integer, y As Integer, z As Integer)
        Me.X = x
        Me.Y = y
        Me.Z = z

        If z > 0 Then
            Geography = GeographyType.Air
        End If
    End Sub

    Public Sub TryMoveTo(another As Position, nearby As Position(), world As WorldParameters)
        If TestMove(another, Creature) Then
            Call DoMove(another, nearby, world)
        End If
    End Sub

    Public Shared Function TestMove(another As Position, creature As Creature) As Boolean
        If another.Z > 0 OrElse another.Geography = GeographyType.Air Then
            ' needs wing
            Dim wing As Double = creature.GetCharacter(BiologyCharacters.Wing)

            If wing > 0 Then
                Return True
            End If
        ElseIf another.Geography = GeographyType.Water Then
            Dim fin As Double = creature.GetCharacter(BiologyCharacters.FishFin)

            If fin > 0 Then
                Return True
            End If
        Else
            ' is land
            Dim foot As Double = creature.GetCharacter(BiologyCharacters.Foot)

            If foot > 0 Then
                Return True
            End If
        End If

        Return False
    End Function

    Private Sub DoMove(another As Position, nearby As Position(), world As WorldParameters)
        If another.Creature Is Nothing Then
            If rand.NextDouble < world.reproduce_rate Then
                ' try to reproduce new one
                another.Creature = Creature.Reproduce(another:=Nothing, world.reproductive_isolation)
            Else
                ' move to another position
                another.Creature = Creature
                Creature = Nothing
            End If
        Else
            ' another position already has a creature
            ' needs test this creature can predation another creature
            Dim same_species As Boolean = Creature.Similarity(another.Creature) > world.reproductive_isolation

            If same_species Then
                If rand.NextDouble < world.reproduce_rate Then
                    ' do reproduce
                    Dim newOne As Creature = Creature.Reproduce(another.Creature, world.reproductive_isolation)

                    ' check for empty slot
                    For i As Integer = 0 To nearby.Length - 1
                        If nearby(i) Is Me OrElse nearby(i) Is another Then
                            Continue For
                        End If

                        If nearby(i).Creature Is Nothing AndAlso TestMove(nearby(i), creature:=newOne) Then
                            nearby(i).Creature = newOne
                        End If
                    Next
                Else
                    If Creature.GetCharacter(BiologyCharacters.Cannibalism) > 0 Then
                        ' eat another
                        another.Creature = Creature
                        Creature = Nothing
                    Else
                        ' do nothing
                    End If
                End If
            Else
                If Predation(another.Creature) Then
                    another.Creature = Creature
                    Creature = Nothing
                End If
            End If
        End If
    End Sub

    Private Function Predation(another As Creature) As Boolean

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