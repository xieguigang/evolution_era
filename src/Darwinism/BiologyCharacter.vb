Public Class BiologyCharacter

    Public ReadOnly Property Character As BiologyCharacters
    Public Property Level As Double

    Friend Shared ReadOnly all_characters As BiologyCharacters() = Enums(Of BiologyCharacters)()


End Class

Public Enum BiologyCharacters As Long
    ''' <summary>
    ''' this slot is empty
    ''' </summary>
    None
    ''' <summary>
    ''' body size(weight)
    ''' </summary>
    BodySize
    Wing
    Claw
    Foot
    Tooth
    OuterShell
    Toxin
    Antitoxin
    ElectricalShock
    FishFin
    FishGill

    ''' <summary>
    ''' 
    ''' </summary>
    Cannibalism

    ''' <summary>
    ''' could be crossover and mutation, otherwise when SexualReproduction
    ''' is zero, only mutation will happends when do reproduction
    ''' </summary>
    SexualReproduction
End Enum
