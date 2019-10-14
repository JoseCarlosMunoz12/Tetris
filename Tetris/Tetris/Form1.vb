Public Class Form1
    Dim rnd As New Random
    Public Enum CRot
        ZERO
        QUARTER
        HALF
        THREEFOURTHS
    End Enum
    Structure TetroID
        Dim TetroCodes() As Integer
        Dim XPos As Single
        Dim YPos As Single
        Dim TetroColor As SolidBrush
        Dim CurrentRot As CRot
        Public Sub InitRor(ChosenColor As Color)
            CurrentRot = CRot.ZERO
            TetroColor = New SolidBrush(ChosenColor)
        End Sub
    End Structure
    Dim TempTetro As TetroID
    Dim AllTetros As New List(Of Integer())
    Dim TimerCountFall As Integer = 0
    Dim Count As Integer = 0

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ''All tetronimos
        AllTetros.Add({1, 5, 9, 13})  ''Long Item
        AllTetros.Add({8, 9, 5, 6})   ''Cross to left
        AllTetros.Add({5, 6, 10, 11}) ''Cross to  Right
        AllTetros.Add({5, 6, 9, 10})  ''Square
        AllTetros.Add({1, 5, 9, 10})  ''Left to right
        AllTetros.Add({2, 6, 10, 9})  ''Left to Left
        AllTetros.Add({5, 8, 9, 10})  ''T Item

        TempTetro = New TetroID With {
                  .TetroCodes = AllTetros(6),
                  .XPos = 3,
                  .YPos = 0}
        TempTetro.InitRor(Color.Yellow)
        Timer1.Enabled = True
    End Sub
    ''draw items and functions
    Private Sub PictureBox1_Paint(sender As Object, e As PaintEventArgs) Handles PictureBox1.Paint
        Dim DimensionOfCubes As Single = (PictureBox1.Width - 1) / 10
        Dim CubeForHeight As Integer = PictureBox1.Height / DimensionOfCubes
        Dim DrawInfo As Graphics = e.Graphics

        For ii = 0 To CubeForHeight - 1
            For jj = 0 To 10
                Dim Br As SolidBrush = New SolidBrush(Color.Gray)
                Dim Outline As Pen = New Pen(Br)
                DrawInfo.DrawRectangle(Outline, jj * DimensionOfCubes, ii * DimensionOfCubes, DimensionOfCubes, DimensionOfCubes)
            Next
        Next
        DrawTetrominos(TempTetro.YPos * DimensionOfCubes,
                       TempTetro.XPos * DimensionOfCubes,
                       DrawInfo, TempTetro, DimensionOfCubes)
    End Sub
    '' Determine Tetro location
    Private Sub DrawTetrominos(YPos As Single, XPos As Single, e As Graphics, TetroToDrw As TetroID, Dimension As Single)
        For ii = 0 To 3
            For jj = 0 To 3
                For Each item In TetroToDrw.TetroCodes
                    If DrawItem(jj, ii, item, TetroToDrw.CurrentRot) Then
                        Dim Br As SolidBrush = New SolidBrush(Color.Gray)
                        Dim Outline As Pen = New Pen(Br)
                        e.FillRectangle(TetroToDrw.TetroColor, XPos + jj * Dimension, YPos + ii * Dimension, Dimension, Dimension)
                        e.DrawRectangle(Outline, XPos + jj * Dimension, YPos + ii * Dimension, Dimension, Dimension)
                    End If
                Next
            Next
        Next
    End Sub
    Private Function DrawItem(Xid As Integer, Yid As Integer, TetroCode As Integer, CurrRot As CRot) As Boolean
        Select Case CurrRot
            Case CRot.ZERO
                Return TetroCode = (4 * Yid + Xid)
            Case CRot.QUARTER
                Return TetroCode = (12 + Yid - 4 * Xid)
            Case CRot.HALF
                Return TetroCode = (15 - 4 * Yid - Xid)
            Case CRot.THREEFOURTHS
                Return TetroCode = (3 - Yid + 4 * Xid)
        End Select
        Return False
    End Function

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Count += 1
        If Count > 10 Then
            TempTetro.YPos += 1
            Count = 0
        End If
        'TempTetro.CurrentRot += 1
        'If TempTetro.CurrentRot > CRot.THREEFOURTHS Then
        '    TempTetro.CurrentRot = CRot.ZERO
        'End If
        PictureBox1.Refresh()
    End Sub
End Class
