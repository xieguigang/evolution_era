Imports System.Runtime.CompilerServices
Imports rand = Microsoft.VisualBasic.Math.RandomExtensions

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

    Sub New(characters As BiologyCharacter())
        heredity = characters
        index = characters _
            .Where(Function(c) c.Character <> BiologyCharacters.None) _
            .ToDictionary(Function(c) c.Character)
    End Sub

    Sub New(capacity As Integer)
        heredity = Empty(capacity).ToArray
    End Sub

    Public Shared Iterator Function Empty(capacity As Integer) As IEnumerable(Of BiologyCharacter)
        For i As Integer = 0 To capacity - 1
            Yield New BiologyCharacter()
        Next
    End Function

    ''' <summary>
    ''' evaluate the similarity for calculate the reproductive isolation, the less 
    ''' similar, the more reproductive isolation chance
    ''' </summary>
    ''' <param name="another"></param>
    ''' <returns></returns>
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

    Public Function GetCharacter(character As BiologyCharacters) As Double
        If index.ContainsKey(character) Then
            Return index(character).Level
        Else
            Return 0
        End If
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

    Public Function Reproduce(another As Creature, isolation As Double) As Creature
        Dim sexualReproduction As Double = Me.GetCharacter(BiologyCharacters.SexualReproduction)

        If sexualReproduction = 0.0 Then
            ' asexual
            ' only mutation happends
            Return Mutation(0.1)
        Else
            ' test reproductive isolation
            If another Is Nothing OrElse another.GetCharacter(BiologyCharacters.SexualReproduction) <= 0 Then
                ' can not combine and create new
                Return Nothing
            End If

            Dim similarity As Double = Me.Similarity(another)

            If similarity > isolation Then
                ' could be combine and create new one: mutation and crossover
                Return Crossover(newOne:=Mutation(sexualReproduction), another)
            Else
                ' reproductive isolation
                Return Nothing
            End If
        End If
    End Function

    Private Shared Function Crossover(ByRef newOne As Creature, another As Creature) As Creature
        Dim inheritance = another.index.Values.ToArray.Random

        If newOne.index.ContainsKey(inheritance.Character) Then
            Dim character As BiologyCharacter = newOne.index(inheritance.Character)
            Dim newLevel As Double = (character.Level + inheritance.Level) / 2

            character.Level = newLevel
        Else
            ' add new one
            For i As Integer = 0 To newOne.heredity.Length - 1
                If newOne.heredity(i).Character = BiologyCharacters.None Then
                    newOne.heredity(i).SetCharacter(inheritance.Character, inheritance.Level)
                    newOne.index.Add(inheritance.Character, newOne.heredity(i))
                    Exit For
                End If
            Next
        End If

        Return newOne
    End Function

    Public Function Mutation(mutation_rate As Double) As Creature
        Dim newOne As BiologyCharacter() = Me.heredity _
            .Select(Function(c) New BiologyCharacter(c)) _
            .ToArray
        Dim pick As Integer = rand.NextInteger(newOne.Length)
        Dim character As BiologyCharacter = newOne(pick)

        If character.Character = BiologyCharacters.None Then
            ' obtain a new character
            Dim newCharacter = BiologyCharacter.all_characters.Random
            Dim index = newOne.Where(Function(c) c.Character <> BiologyCharacters.None).ToDictionary(Function(c) c.Character)

            If index.ContainsKey(newCharacter) Then
                ' increase character level
                index(newCharacter).Level += mutation_rate
            Else
                ' add a new character
                character.SetCharacter(newCharacter, rand.NextDouble * mutation_rate)
            End If
        Else
            ' adjust character levels
            Dim sign As Double = If(rand.NextDouble > 0.5, 1, -1)
            Dim d As Double = rand.NextDouble * mutation_rate

            character.Level += (sign * d)

            If character.Level < 0 Then
                ' loss a character
                character.SetCharacter(BiologyCharacters.None, 0)
            End If
        End If

        Return New Creature(newOne)
    End Function

End Class
