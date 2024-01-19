Imports System.IO
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.DataStorage.HDSPack
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Microsoft.VisualBasic.Text

Public Class DataWriter : Implements IDisposable

    ReadOnly bin As StreamPack

    Dim populationSize As New List(Of Integer)
    Dim disposedValue As Boolean
    Dim args As WorldParameters

    Sub New(file As Stream, args As WorldParameters)
        Me.bin = New StreamPack(file, meta_size:=1024 * 1024 * 32)
        Me.args = args
    End Sub

    Public Sub Record(time As Integer, creatures As IEnumerable(Of Creature))
        Dim all = creatures.ToArray
        Dim path As String = $"/data/{time}.dat"

        populationSize.Add(all.Length)

        Dim s As Stream = bin.OpenFile(path, FileMode.Create, FileAccess.Write)
        Dim wd As New BinaryDataWriter(s, Encodings.ASCII) With {
            .ByteOrder = ByteOrder.BigEndian
        }

        Call wd.Write(all.Length)

        For Each creature As Creature In all
            Call wd.Write(creature.guid)
            Call wd.Write(creature.parent.TryCount)
            Call wd.Write(creature.parent.SafeQuery.ToArray)

            For Each character As BiologyCharacter In creature.heredity
                Call wd.Write(character.Character)
                Call wd.Write(character.Level)
            Next
        Next
    End Sub

    Private Sub SaveMetaData()
        Call bin.WriteText({populationSize.ToArray.GetJson}, fileName:="/metadata/population_size.json")
        Call bin.WriteText({args.GetObject.GetJson}, fileName:="/metadata/world_arguments.json")
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)
                Call SaveMetaData()
                Call bin.Dispose()
            End If

            ' TODO: 释放未托管的资源(未托管的对象)并重写终结器
            ' TODO: 将大型字段设置为 null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: 仅当“Dispose(disposing As Boolean)”拥有用于释放未托管资源的代码时才替代终结器
    ' Protected Overrides Sub Finalize()
    '     ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
