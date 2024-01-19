Public Class Creature

    Public Property BiologicalCharacters As BiologyCharacter()
        Get

        End Get
        Set(value As BiologyCharacter())

        End Set
    End Property

    Dim index As Dictionary(Of BiologyCharacters, BiologyCharacter)

    Public Function Similarity(another As Creature) As Double
        If another Is Me Then
            Return 1
        ElseIf another Is Nothing Then
            Return 0
        End If

        Dim x As Double() = GetVector()
        Dim y As Double() = another.GetVector

        Return Microsoft.VisualBasic.Math.SSM_SIMD(x, y)
    End Function

    Public Function GetVector() As Double()

    End Function

End Class
