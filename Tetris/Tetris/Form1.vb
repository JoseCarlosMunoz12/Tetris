Public Class Form1
    Dim rnd As New Random
    Public Enum Collisions
        GROUND
        BORDER_LEFT
        BORDER_RIGHT
        TETRO_STACKS
    End Enum
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
    Structure CubeInfo
        Dim Xpos As Single
        Dim Ypos As Single
        Dim TetroColor As SolidBrush
    End Structure
    Structure ColInfo
        Dim Colisions As Boolean
        Dim CubesLoc() As CubeInfo
    End Structure
    Dim TempTetro As TetroID
    Dim AllTetros As New List(Of Integer())
    Dim AllStacks As New List(Of CubeInfo)
    Dim AllTetColo As Color() = {Color.Blue, Color.Green, Color.Red, Color.Purple, Color.Yellow, Color.Pink, Color.Brown}
    Dim TimerCountFall As Integer = 0
    Dim Count As Integer = 1
    Dim PrevTetro As Integer = 0
    Dim DimensionOfCubes As Single
    Dim CubeForHeight As Integer
    Dim Rand As New Random
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DimensionOfCubes = (PictureBox1.Width - 1) / 10
        CubeForHeight = PictureBox1.Height / DimensionOfCubes
        ''All tetronimos
        AllTetros.Add({1, 5, 9, 13})  ''Long Item
        AllTetros.Add({5, 6, 8, 9})   ''Step to left
        AllTetros.Add({5, 6, 10, 11}) ''Step to Right
        AllTetros.Add({5, 6, 9, 10})  ''Square
        AllTetros.Add({1, 5, 9, 10})  ''L to right
        AllTetros.Add({2, 6, 9, 10})  ''L to Left
        AllTetros.Add({5, 8, 9, 10})  ''T Item

        Dim RandNum As Integer = Rand.Next(0, 1500)
        Dim Val As Integer = 0
        Do Until Not Val = PrevTetro
            RandNum = Rand.Next(0, 1500)
            Val = RandNum Mod 7
        Loop
        PrevTetro = Val
        TempTetro = New TetroID With {
                  .TetroCodes = AllTetros(Val),
                  .XPos = 3,
                  .YPos = -3}
        TempTetro.InitRor(AllTetColo(Val))
        Timer1.Enabled = True
    End Sub
    ''draw items and functions
    Private Sub PictureBox1_Paint(sender As Object, e As PaintEventArgs) Handles PictureBox1.Paint
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
        For Each Cube In AllStacks
            DrawInfo.FillRectangle(Cube.TetroColor,
                                   Cube.Xpos * DimensionOfCubes,
                                   Cube.Ypos * DimensionOfCubes,
                                   DimensionOfCubes, DimensionOfCubes)
        Next
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
                        Exit For
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
    Private Function CollsionCheck(Tetroinfo As TetroID) As Boolean()
        Dim AllCols(3) As Boolean
        For ii = 0 To 3
            For jj = 0 To 3
                For Each item In Tetroinfo.TetroCodes
                    If DrawItem(jj, ii, item, Tetroinfo.CurrentRot) Then
                        AllCols(0) = jj + Tetroinfo.XPos < 0
                        AllCols(1) = jj + Tetroinfo.XPos > 9
                        AllCols(2) = CubeForHeight = (ii + TempTetro.YPos + 1)
                        If AllStacks.Count = 0 Then
                            AllCols(3) = False
                        Else
                            For Each Cube In AllStacks
                                AllCols(3) = Cube.Xpos = jj + TempTetro.XPos And Cube.Ypos = ii + TempTetro.YPos + 1
                                If AllCols(3) Then
                                    Return AllCols
                                End If
                            Next
                        End If
                        For Each pos In AllCols
                            If pos Then
                                Return AllCols
                            End If
                        Next
                    End If
                Next
            Next
        Next
        Return AllCols
    End Function
    Private Sub ResetTetro()
        Dim RandNum As Integer = Rand.Next(0, 1500)
        Dim Val As Integer = Rand.Next(0, 1500) Mod 7
        Do Until Not Val = PrevTetro
            RandNum = Rand.Next(0, 1500)
            Val = RandNum Mod 7
        Loop
        PrevTetro = Val
        TempTetro = New TetroID With {
              .TetroCodes = AllTetros(Val),
              .XPos = 3,
              .YPos = -3}
        TempTetro.InitRor(AllTetColo(Val))
    End Sub
    Private Sub AddToList(TetroInfo As TetroID)
        Dim Count As Integer = 0
        For ii = 0 To 3
            For jj = 0 To 3
                For Each item In TetroInfo.TetroCodes
                    If DrawItem(jj, ii, item, TetroInfo.CurrentRot) Then
                        Dim Temp As New CubeInfo With {
                            .Xpos = jj + TetroInfo.XPos,
                            .Ypos = ii + TetroInfo.YPos,
                            .TetroColor = TetroInfo.TetroColor}
                        AllStacks.Add(Temp)
                        Count += 1
                    End If
                    If Count = 4 Then
                        Exit Sub
                    End If
                Next
            Next

        Next
    End Sub
    ''event functions and keys
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        TempTetro.YPos += Count
        Dim ColTest() As Boolean = CollsionCheck(TempTetro)

        If ColTest(0) Then
            ResetTetro()
        End If
        If ColTest(1) Then
            ResetTetro()
        Else
        End If
        If ColTest(2) Then
            AddToList(TempTetro)
            ResetTetro()
        End If
        If ColTest(3) Then
            AddToList(TempTetro)
            ResetTetro()
        End If
        If Not AllStacks.Count = 0 Then
            If FindCeiling() Then
                Timer1.Enabled = False
            End If
        End If
        PictureBox1.Refresh()
    End Sub
    Private Function FindCeiling() As Boolean
        For Each item In AllStacks
            If item.Ypos = 0 Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.D
                TempTetro.XPos += 1
            Case Keys.A
                TempTetro.XPos -= 1
            Case Keys.W
                TempTetro.CurrentRot += 1
                If TempTetro.CurrentRot > CRot.THREEFOURTHS Then
                    TempTetro.CurrentRot = CRot.ZERO
                End If
            Case Keys.S
                Count = 2
        End Select
    End Sub
    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        Count = 1
    End Sub
End Class
