Public Class Creature

    Public Property BiologicalCharacters As BiologyCharacter()

    Public Function Similarity(another As Creature) As Double
        If another Is Me Then
            Return 1
        End If


    End Function

End Class
