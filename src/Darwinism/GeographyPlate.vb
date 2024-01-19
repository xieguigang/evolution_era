Imports Microsoft.VisualBasic.Data.GraphTheory.GridGraph
Imports Microsoft.VisualBasic.Imaging

Public Class GeographyPlate

    Dim spatial As Spatial3D(Of Position)

    ''' <summary>
    ''' 
    ''' </summary>
    Dim reproductive_isolation As Double = 0.9

    Sub New()

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

                Dim nearby = spatial.Query(point.X, point.Y, point.Z).ToArray
                Dim tryOne As Position = nearby.Random

                Call point.TryMoveTo(another:=tryOne)
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

    Public Sub TryMoveTo(another As Position)
        If another.Z > 0 OrElse another.Geography = GeographyType.Air Then
            ' needs wing
            Dim wing As Double = Creature.GetCharacter(BiologyCharacters.Wing)

            If wing > 0 Then
                Call DoMove(another)
            End If
        ElseIf another.Geography = GeographyType.Water Then
            Dim fin As Double = Creature.GetCharacter(BiologyCharacters.FishFin)

            If fin > 0 Then
                Call DoMove(another)
            End If
        Else
            ' is land
            Dim foot As Double = Creature.GetCharacter(BiologyCharacters.Foot)

            If foot > 0 Then
                Call DoMove(another)
            End If
        End If
    End Sub

    Private Sub DoMove(another As Position)
        If another.Creature Is Nothing Then
            another.Creature = Creature
            Creature = Nothing
        Else
            ' another position already has a creature
            ' needs test this creature can predation another creature
        End If
    End Sub

End Class

Public Enum GeographyType
    Water
    Land
    Air
End Enum