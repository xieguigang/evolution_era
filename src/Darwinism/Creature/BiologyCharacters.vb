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
    Foot
    Tooth
    OuterShell
    Toxin
    Antitoxin
    ElectricalShock
    AntiElectricalShock
    FishFin

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