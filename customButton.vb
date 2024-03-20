Imports System.Drawing.Drawing2D

''' <summary>
''' Forked from a public repository
''' </summary>
''' <see cref="https://github.com/kdash1/DarkControls/blob/main/customButton.vb"/>
Public Class customButton
    Inherits Button

#Region "Members"

    Private borderSz As Integer = 0
    Private borderRad As Integer = 0
    Private borderClr As Color = Color.White

#End Region

#Region "Property"

    Public Property BorderSize As Integer
        Get
            Return borderSz
        End Get
        Set(ByVal value As Integer)
            borderSz = value
            Me.Invalidate()
        End Set
    End Property

    Public Property BorderRadius As Integer
        Get
            Return borderRad
        End Get
        Set(ByVal value As Integer)
            borderRad = value
            Me.Invalidate()
        End Set
    End Property

    Public Property BorderColor As Color
        Get
            Return borderClr
        End Get
        Set(ByVal value As Color)
            borderClr = value
            Me.Invalidate()
        End Set
    End Property

    'Public Property BorderBackgroundColor As Color
    '    Get
    '        Return Me.BackColor
    '    End Get
    '    Set(ByVal value As Color)
    '        Me.BackColor = value
    '    End Set
    'End Property

    Public Property TextColor As Color
        Get
            Return Me.ForeColor
        End Get
        Set(value As Color)
            Me.ForeColor = value
        End Set
    End Property

#End Region

#Region "Constructor"
    Public Sub New()
        Me.FlatStyle = FlatStyle.Flat
        Me.FlatAppearance.BorderSize = 1
        Me.Size = New Size(100, 25)
        Me.BackColor = Color.Indigo
        Me.ForeColor = Color.White
        AddHandler Me.Resize, New EventHandler(AddressOf Button_Resize)
    End Sub

    Private Function GetFigurePath(ByVal rect As Rectangle, ByVal radius As Integer) As GraphicsPath
        Dim path As GraphicsPath = New GraphicsPath()
        Dim curveSize As Single = radius * 2.0F
        path.StartFigure()
        path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90)
        path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90)
        path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90)
        path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90)
        path.CloseFigure()
        Return path
    End Function

    Protected Overrides Sub OnPaint(ByVal pevent As PaintEventArgs)
        MyBase.OnPaint(pevent)
        Dim rectSurface As Rectangle = Me.ClientRectangle
        Dim rectBorder As Rectangle = Rectangle.Inflate(rectSurface, -borderSz, -borderSz)
        Dim smoothSize As Integer = 2
        If borderSz > 0 Then smoothSize = borderSz

        If borderRad > 2 Then

            Using pathSurface As GraphicsPath = GetFigurePath(rectSurface, borderRad)

                Using pathBorder As GraphicsPath = GetFigurePath(rectBorder, borderRad - borderSz)

                    Using penSurface As Pen = New Pen(Me.Parent.BackColor, smoothSize)

                        Using penBorder As Pen = New Pen(borderClr, borderSz)
                            pevent.Graphics.SmoothingMode = SmoothingMode.HighQuality
                            Me.Region = New Region(pathSurface)
                            pevent.Graphics.DrawPath(penSurface, pathSurface)
                            If borderSz >= 1 Then pevent.Graphics.DrawPath(penBorder, pathBorder)
                        End Using
                    End Using
                End Using
            End Using
        Else
            pevent.Graphics.SmoothingMode = SmoothingMode.HighQuality
            Me.Region = New Region(rectSurface)

            If BorderSize >= 1 Then

                Using penBorder As Pen = New Pen(borderClr, borderSz)
                    penBorder.Alignment = PenAlignment.Inset
                    pevent.Graphics.SmoothingMode = SmoothingMode.HighQuality
                    pevent.Graphics.DrawRectangle(penBorder, 0, 0, Me.Width - 1, Me.Height - 1)
                End Using
            End If
        End If
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        AddHandler Me.Parent.BackColorChanged, New EventHandler(AddressOf Container_BGChanged)
    End Sub
    Private Sub Container_BGChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.Invalidate()
    End Sub
    Private Sub Button_Resize(ByVal sender As Object, ByVal e As EventArgs)
        If borderRad > Me.Height Then borderRad = Me.Height
    End Sub

#End Region

End Class
