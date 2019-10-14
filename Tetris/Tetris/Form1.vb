
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
        Dim CurrentRot As CRot
        Public Sub InitRor()
            CurrentRot = CRot.ZERO
        End Sub

    End Structure
    ''draw items and functions
    Private Sub PictureBox1_Paint(sender As Object, e As PaintEventArgs) Handles PictureBox1.Paint
        Dim DimensionOfCubes As Single = PictureBox1.Width / 10
        Dim CubeForHeight As Integer = PictureBox1.Height / DimensionOfCubes
        Dim DrawInfo As Graphics = e.Graphics
        For ii = 0 To CubeForHeight - 1
            For jj = 0 To 10
                Dim Br As SolidBrush = New SolidBrush(Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255)))
                DrawInfo.FillRectangle(Br, jj * DimensionOfCubes, ii * DimensionOfCubes, DimensionOfCubes, DimensionOfCubes)
            Next
        Next
    End Sub
    '' Determine Tetro location
    Private Sub DrawTetrominos(XPlos As Single, YPos As Single, e As PaintEventArgs, TetroToDrw As TetroID)
        For ii = 0 To 3
            For jj = 0 To 3
                For Each item In TetroToDrw.TetroCodes

                Next
            Next
        Next
    End Sub
End Class
