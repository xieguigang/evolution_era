Public Class DataAdapter

    ReadOnly world As GeographyPlate

    Sub New(world As GeographyPlate)
        Me.world = world
    End Sub

    ''' <summary>
    ''' get a collection of the position which has a creature on it
    ''' </summary>
    ''' <returns></returns>
    Public Iterator Function GetCreatures() As IEnumerable(Of Position)
        For Each position As Position In world.GetPositions
            If Not position.Creature Is Nothing Then
                Yield position
            End If
        Next
    End Function
End Class
