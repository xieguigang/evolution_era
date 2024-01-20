Public Enum BiologyCharacters As Long
    ''' <summary>
    ''' this slot is empty
    ''' </summary>
    None
    ''' <summary>
    ''' body size(weight)
    ''' </summary>
    BodySize
    ''' <summary>
    ''' ability to enable move on <see cref="GeographyType.Air"/>
    ''' </summary>
    Wing
    ''' <summary>
    ''' ability to enable move on <see cref="GeographyType.Land"/>
    ''' </summary>
    Foot
    Tooth
    OuterShell
    Toxin
    Antitoxin
    ElectricalShock
    AntiElectricalShock
    ''' <summary>
    ''' ability to enable move on <see cref="GeographyType.Water"/>
    ''' </summary>
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