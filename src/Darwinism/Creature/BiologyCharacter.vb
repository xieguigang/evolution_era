Public Class BiologyCharacter

    Public ReadOnly Property Character As BiologyCharacters
    Public Property Level As Double

    Public Shared ReadOnly all_characters As IReadOnlyCollection(Of BiologyCharacters) = Enums(Of BiologyCharacters)()

    ''' <summary>
    ''' make a new copy
    ''' </summary>
    ''' <param name="copy"></param>
    Sub New(copy As BiologyCharacter)
        Character = copy.Character
        Level = copy.Level
    End Sub

    ''' <summary>
    ''' create a new empty slot
    ''' </summary>
    Sub New()
        Character = BiologyCharacters.None
        Level = 0
    End Sub

    Sub New(c As BiologyCharacters, level As Double)
        Me._Level = level
        Me._Character = c
    End Sub

    Public Sub SetCharacter(c As BiologyCharacters, level As Double)
        Me._Level = level
        Me._Character = c
    End Sub

    Public Overrides Function ToString() As String
        Return $"{Character.Description} ~ {Level}"
    End Function

End Class


