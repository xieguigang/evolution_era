Imports System.Runtime.CompilerServices

Public Class Creature

    Public ReadOnly Property BiologicalCharacters As BiologyCharacter()
        Get
            Return heredity.ToArray
        End Get
    End Property

    Dim index As Dictionary(Of BiologyCharacters, BiologyCharacter)
    ''' <summary>
    ''' fixed size
    ''' </summary>
    Dim heredity As BiologyCharacter()

    Sub New()
    End Sub

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

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetVector() As Double()
        Return BiologyCharacter.all_characters _
            .Select(Function(c)
                        If index.ContainsKey(c) Then
                            Return index(c).Level
                        Else
                            Return 0
                        End If
                    End Function) _
            .ToArray
    End Function

End Class
