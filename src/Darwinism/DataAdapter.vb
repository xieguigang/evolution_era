Public Class DataAdapter

    ReadOnly world As GeographyPlate

    Sub New(world As GeographyPlate)
        Me.world = world
    End Sub

    Public Iterator Function GetCreatures() As IEnumerable(Of Creature)
        For Each position As Position In world.GetPositions
            If Not position.Creature Is Nothing Then
                Yield position.Creature
            End If
        Next
    End Function
End Class
