Public Enum GeographyType
    Water
    Land
    Air
End Enum

Public Structure WorldParameters

    Dim reproductive_isolation As Double
    Dim reproduce_rate As Double
    ''' <summary>
    ''' the max dna size.(max number of the <see cref="BiologyCharacters"/> a creature that it has)
    ''' </summary>
    Dim dna_capacity As Integer
    ''' <summary>
    ''' max age for reach the natural_death
    ''' </summary>
    Dim natural_death As Integer
    ''' <summary>
    ''' age for sexual maturity
    ''' </summary>
    Dim sexual_maturity As Integer
    Dim predation_diff As Integer

    Public Function GetObject() As Dictionary(Of String, String)
        Return New Dictionary(Of String, String) From {
            {"reproductive_isolation", reproductive_isolation},
            {"reproduce_rate", reproduce_rate},
            {"dna_capacity", dna_capacity},
            {"natural death", natural_death},
            {"sexual_maturity", sexual_maturity},
            {"predation_diff", predation_diff}
        }
    End Function

End Structure