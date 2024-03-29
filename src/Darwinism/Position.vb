﻿Imports Microsoft.VisualBasic.Imaging
Imports rand = Microsoft.VisualBasic.Math.RandomExtensions
Imports std = System.Math

Public Class Position : Implements IPoint3D

    Public Property Creature As Creature
    Public Property Z As Integer Implements IPoint3D.Z
    Public Property X As Integer Implements RasterPixel.X
    Public Property Y As Integer Implements RasterPixel.Y
    Public Property Geography As GeographyType
    Public Property energy As Double = 0

    Sub New(x As Integer, y As Integer, z As Integer)
        Me.X = x
        Me.Y = y
        Me.Z = z

        If z > 0 Then
            Geography = GeographyType.Air
        End If
    End Sub

    Public Sub TimeElapsed(era As Integer)
        If Not Creature Is Nothing AndAlso energy > 0 Then
            Creature.energy += 1
            energy -= 1
        End If

        ' energy only existsed for land and water, not for air
        If energy < 100 AndAlso Geography <> GeographyType.Air Then
            If Geography = GeographyType.Land Then
                energy += 2
            Else
                energy += 1
            End If
        End If
    End Sub

    Public Overrides Function ToString() As String
        Dim label As String = If(Creature Is Nothing, "", Creature.ToString)
        Return $"{label} - {Geography.Description} ({X},{Y},{Z})"
    End Function

    Public Sub TryMoveTo(another As Position, nearby As Position(), era As Integer, world As WorldParameters)
        If TestMove(another, Creature) Then
            Call DoMove(another, nearby, era, world)
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

    Private Sub DoMove(another As Position, nearby As Position(), era As Integer, world As WorldParameters)
        If another.Creature Is Nothing Then
            If rand.NextDouble < world.reproduce_rate Then
                ' try to reproduce new one
                another.Creature = Creature.Reproduce(another:=Nothing, era, world)
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
                    Dim newOne As Creature = Creature.Reproduce(another.Creature, era, world)

                    If Not newOne Is Nothing Then
                        ' check for empty slot
                        For i As Integer = 0 To nearby.Length - 1
                            If nearby(i) Is Me OrElse nearby(i) Is another Then
                                Continue For
                            End If

                            If nearby(i).Creature Is Nothing AndAlso TestMove(nearby(i), creature:=newOne) Then
                                nearby(i).Creature = newOne
                            End If
                        Next
                    End If
                Else
                    If Creature.GetCharacter(BiologyCharacters.Cannibalism) > 0 Then
                        ' eat another
                        Creature.Eat(another.Creature)
                        another.Creature = Creature
                        Creature = Nothing
                    Else
                        ' do nothing
                    End If
                End If
            Else
                If Predation(another.Creature, world.predation_diff) Then
                    Creature.Eat(another.Creature)
                    another.Creature = Creature
                    Creature = Nothing
                End If
            End If
        End If
    End Sub

    Private Shared Function BodySizeDifference(a As Creature, b As Creature) As Double
        Return std.Abs(a.GetCharacter(BiologyCharacters.BodySize) - b.GetCharacter(BiologyCharacters.BodySize))
    End Function

    Private Function Predation(another As Creature, differ As Integer) As Boolean
        Dim electricalShock = Creature.GetCharacter(BiologyCharacters.ElectricalShock)

        If electricalShock > another.GetCharacter(BiologyCharacters.AntiElectricalShock) Then
            Return BodySizeDifference(Creature, another) < differ
        End If

        Dim toxin = Creature.GetCharacter(BiologyCharacters.Toxin)

        If toxin > another.GetCharacter(BiologyCharacters.Antitoxin) Then
            Return BodySizeDifference(Creature, another) < differ
        End If

        Dim tooth = Creature.GetCharacter(BiologyCharacters.Tooth)

        If tooth > another.GetCharacter(BiologyCharacters.OuterShell) Then
            Return BodySizeDifference(Creature, another) < differ
        End If

        Return BodySizeDifference(Creature, another) < differ
    End Function

End Class
