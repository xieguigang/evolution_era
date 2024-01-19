Public Class BiologyCharacter

    Public ReadOnly Property Character As BiologyCharacters
    Public Property Level As Double

    Friend Shared ReadOnly all_characters As BiologyCharacters() = Enums(Of BiologyCharacters)()


End Class

Public Enum BiologyCharacters
    None
    ''' <summary>
    ''' body size(weight)
    ''' </summary>
    BodySize
    Wing
    Claw
    Tooth
    OuterShell
    Toxin
    Antitoxin
    ElectricalShock
    FishFin
    FishGill
End Enum
