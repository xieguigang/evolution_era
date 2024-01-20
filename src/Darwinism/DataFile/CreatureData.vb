Imports System.Runtime.CompilerServices

Public Class CreatureData

    Public Property Guid As Integer
    Public Property X As Integer
    Public Property Y As Integer
    Public Property Z As Integer
    Public Property Parent As Integer()
    Public Property Era As Integer
    Public Property Age As Integer
    Public Property LifeSpan As Integer
    Public Property Energy As Double
    Public Property Heredity As BiologyCharacter()

    Sub New(guid As Integer, xyz As Integer(), parent As Integer(),
            era As Integer, age As Integer, lifespan As Integer, energy As Double,
            heredity As BiologyCharacter())

        Me.Guid = guid
        Me.X = xyz(0)
        Me.Y = xyz(1)
        Me.Z = xyz(2)
        Me.Parent = parent
        Me.Era = era
        Me.Age = age
        Me.LifeSpan = lifespan
        Me.Energy = energy
        Me.Heredity = heredity
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetTopCharacter() As BiologyCharacters
        Return Heredity.OrderByDescending(Function(c) c.Level).First.Character
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Widening Operator CType(data As (guid As Integer, xyz As Integer(), parent As Integer(),
            era As Integer, age As Integer, lifespan As Integer, energy As Double,
            heredity As BiologyCharacter())) As CreatureData

        Return New CreatureData(data.guid, data.xyz, data.parent, data.era, data.age, data.lifespan, data.energy, data.heredity)
    End Operator

End Class
