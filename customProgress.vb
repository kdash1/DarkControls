Imports System.ComponentModel
Public Class customProgress
    
#Region "Members"
    Dim _percent, _dia, _staticSize, _dynamicSize As Integer
    Dim _staticColor, _dynamicColor As Color
#End Region
    
#Region "Properties"
    <DefaultValue(GetType(Integer), "45")>
    <Description("Percentage")>
    Public Property Percent As Integer
        Set(value As Integer)
            _percent = value
        End Set
        Get
            Return _percent
        End Get
    End Property
    <DefaultValue(GetType(Integer), "100")>
    <Description("Diameter of the Circle")>
    Public Property Diameter As Integer
        Set(value As Integer)
            _dia = value
        End Set
        Get
            Return _dia
        End Get
    End Property
    Public Property StaticCircleSize As Integer
        Get
            Return _staticSize
        End Get
        Set(value As Integer)
            _staticSize = value
        End Set
    End Property
    Public Property DynamicCircleSize As Integer
        Get
            Return _dynamicSize
        End Get
        Set(value As Integer)
            _dynamicSize = value
        End Set
    End Property

    <DefaultValue("45,45,45")>
    Public Property DynamicCircleColor As Color
        Get
            Return _dynamicColor
        End Get
        Set(value As Color)
            _dynamicColor = value
        End Set
    End Property
    <DefaultValue("45,45,45")>
    Public Property StaticCircleColor As Color
        Get
            Return _staticColor
        End Get
        Set(value As Color)
            _staticColor = value
        End Set
    End Property
#End Region
    
#Region "Constructors"
    Private Sub DrawProgress(g As Graphics, rect As Rectangle, percentage As Single, staticCircleSize As Integer, dynamicCircleSize As Integer)
        Dim centerX As Integer = Me.Width \ 2
        Dim centerY As Integer = Me.Height \ 2

        Dim progressRect As New Rectangle(centerX - (rect.Width \ 2), centerY - (rect.Height \ 2), rect.Width, rect.Height)

        Dim textSize As SizeF = g.MeasureString(percentage.ToString() + "%", Me.Font)
        Dim textX As Integer = CInt(centerX - (textSize.Width / 2))
        Dim textY As Integer = CInt(centerY - (textSize.Height / 2))

        Dim progressAngle As Single = CSng(360 / 100 * percentage)
        Dim remainderAngle As Single = 360 - progressAngle
        Using progressPen As New Pen(_dynamicColor, dynamicCircleSize),
          remainderPen As New Pen(_staticColor, staticCircleSize)
            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            g.DrawArc(progressPen, progressRect, -90, progressAngle)
            g.DrawArc(remainderPen, progressRect, progressAngle - 90, remainderAngle)
        End Using

        Using fnt As New Font(Me.Font, Me.Font.Style)
            g.DrawString(percentage.ToString() + "%", fnt, Brushes.Black, textX, textY)
        End Using
    End Sub
    Private Sub UserControl1_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        If Diameter = 0 Then
            Diameter = 70
        End If
        If StaticCircleSize = 0 Then
            StaticCircleSize = 2
        End If
        If DynamicCircleSize = 0 Then
            DynamicCircleSize = 5
        End If
        If DynamicCircleColor.IsEmpty Then
            DynamicCircleColor = Color.Green
        End If
        If StaticCircleColor.IsEmpty Then
            StaticCircleColor = Color.Black
        End If
        DrawProgress(e.Graphics, New Rectangle(5, 5, Diameter, Diameter), _percent, StaticCircleSize, DynamicCircleSize)
    End Sub
    ''' <summary>
    ''' To update the progress percent and circle
    ''' </summary>
    ''' <param name="percent"> Percentage to be updated </param>
    ''' <returns> Current Percentage </returns>
    Public Function updatePercent(percent As Integer)
        Dim initPercent As Integer = Me.Percent
        For x% = initPercent To percent - 1
            System.Threading.Thread.Sleep(5)
            Me.Percent += 1
            Me.Refresh()
        Next
        Return Me.Percent
    End Function
#End Region
    
End Class
