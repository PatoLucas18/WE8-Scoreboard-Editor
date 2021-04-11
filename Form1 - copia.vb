Imports System.IO
Imports System.Drawing.Drawing2D
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Math

Public Class Form1
    Dim tempCnt As Boolean         'check weather the roller is used or not
    Dim bm_dest As Bitmap
    Dim bm_source As Bitmap
    Dim i As Int16 = CShort(0.5)
    Dim rc As ResizeableControl
    Public Matriz(,) As System.Drawing.Color
    Dim OFFSET As Integer
    Dim EJESX As String
    Dim EJESY As String
    Dim Identificador As Integer

    Dim Background As Bitmap
    Dim Scr_CB As Bitmap
    Dim Tv_CB As Bitmap
    Dim EXTRA_CB As Bitmap
    Dim PlayerTEXTL_CB As Bitmap
    Dim PlayerTEXTV_CB As Bitmap

#Region "Image Resizing"
    Private Sub resizingTrackBar_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try

            Dim scale_factor As Integer
            Dim img1 As New PictureBox

            img1.Image = cropBitmap
            bm_source = New Bitmap(img1.Image)
            bm_dest = New Bitmap( _
                CInt(bm_source.Width * scale_factor), _
                CInt(bm_source.Height * scale_factor))

            Dim gr_dest As Graphics = Graphics.FromImage(bm_dest)

            gr_dest.DrawImage(bm_source, 0, 0, bm_dest.Width + i, bm_dest.Height + i)

            Scr.Image = bm_dest

            tempCnt = True
        Catch ex As Exception

        End Try
    End Sub
#End Region
#Region "Image Cropping"
    Dim cropX As Integer
    Dim cropY As Integer
    Dim cropWidth As Integer
    Dim cropHeight As Integer

    Dim oCropX As Integer
    Dim oCropY As Integer
    Dim cropBitmap As Bitmap

    Public cropPen As Pen
    Public cropPenSize As Integer = 1 '2
    Public cropDashStyle As Drawing2D.DashStyle = Drawing2D.DashStyle.Solid
    Public cropPenColor As Color = Color.Cyan


    Dim tmppoint As Point


#End Region


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim TE As Bitmap
            TE = My.Resources.game01
            cropX = 1
            cropY = 124
            cropWidth = 136
            cropHeight = 7
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(TE, TE.Width, TE.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            AtaqueL.Image = cropBitmap
            AtaqueV.Image = cropBitmap
        Catch ex As Exception

        End Try
        
        'Cargar Resolucion
        ResAlta.CheckState = My.Settings.ResolucionAlta
        If ResAlta.CheckState = CheckState.Checked Then
            ResBaja.CheckState = CheckState.Unchecked
            Resolucionalta(True)
        Else
            ResBaja.CheckState = CheckState.Checked
            ResAlta.CheckState = CheckState.Unchecked
        End If

        'Cargar Idioma
        EnglishToolStripMenuItem.CheckState = My.Settings.EN
        If EnglishToolStripMenuItem.CheckState = CheckState.Checked Then
            EspañolToolStripMenuItem.CheckState = CheckState.Unchecked
            English(True)
        Else
            EnglishToolStripMenuItem.CheckState = CheckState.Unchecked
            EspañolToolStripMenuItem.CheckState = CheckState.Checked
        End If

        'Recorrer los paneles para agragar ResizeableControl
        For Each c As Control In Me.Panel1.Controls
            If TypeOf c Is PictureBox Then
                rc = New ResizeableControl(c)
            End If
        Next
        For Each c As Control In Me.Mapeo.Controls
            If TypeOf c Is PictureBox Then
                rc = New ResizeableControl(c)
            End If
        Next


        'Tooltip de 1 control
        Me.ToolTip1.SetToolTip(Me.Rojo, "Textura Marcador")
        Me.ToolTip1.SetToolTip(Me.Azul, "Textura Placa Nombre Local")
        Me.ToolTip1.SetToolTip(Me.Verde, "Textura Placa Nombre Visitante")
        Me.ToolTip1.SetToolTip(Me.Amarillo, "Textura Extra")
        Me.ToolTip1.SetToolTip(Me.Violeta, "Textura TV Logo")

    End Sub
    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        My.Settings.ResolucionAlta = ResAlta.CheckState
        My.Settings.EN = EnglishToolStripMenuItem.CheckState
        My.Settings.Save()
        Try
            My.Computer.FileSystem.DeleteFile(dlgAbrir.FileName & "_unzlib")
        Catch ex As Exception

        End Try
    End Sub

    Public Class ResizeableControl

        Private WithEvents mControl As Control
        Private mMouseDown As Boolean = False
        Private mEdge As EdgeEnum = EdgeEnum.None
        Private mWidth As Integer = 4
        Private mOutlineDrawn As Boolean = False

        Private Enum EdgeEnum
            None
            Right
            Left
            Top
            Bottom
            TopLeft
        End Enum

        Public Sub New(ByVal Control As Control)
            mControl = Control
        End Sub

        Private Sub mControl_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mControl.MouseDown
            If e.Button = Windows.Forms.MouseButtons.Left Then
                mMouseDown = True
            End If
        End Sub

        Private Sub mControl_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mControl.MouseUp
            mMouseDown = False
        End Sub

        Private Sub mControl_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mControl.MouseMove
            Dim c As Control = CType(sender, Control)
            Dim g As Graphics = c.CreateGraphics
            c.BackgroundImage = My.Resources.Sombra
            Select Case mEdge
                Case EdgeEnum.TopLeft
                    g.FillRectangle(Brushes.Fuchsia, 0, 0, mWidth * 4, mWidth * 4)
                    mOutlineDrawn = True
                Case EdgeEnum.Left
                    g.FillRectangle(Brushes.Fuchsia, 0, 0, mWidth, c.Height)
                    mOutlineDrawn = True
                Case EdgeEnum.Right
                    g.FillRectangle(Brushes.Fuchsia, c.Width - mWidth, 0, c.Width, c.Height)
                    mOutlineDrawn = True
                Case EdgeEnum.Top
                    g.FillRectangle(Brushes.Fuchsia, 0, 0, c.Width, mWidth)
                    mOutlineDrawn = True
                Case EdgeEnum.Bottom
                    g.FillRectangle(Brushes.Fuchsia, 0, c.Height - mWidth, c.Width, mWidth)
                    mOutlineDrawn = True
                Case EdgeEnum.None
                    If mOutlineDrawn Then
                        c.Refresh()
                        mOutlineDrawn = False
                    End If
            End Select

            If mMouseDown And mEdge <> EdgeEnum.None Then
                c.SuspendLayout()
                Select Case mEdge
                    Case EdgeEnum.TopLeft
                        c.SetBounds(c.Left + e.X, c.Top + e.Y, c.Width, c.Height)
                    Case EdgeEnum.Left
                        c.SetBounds(c.Left + e.X, c.Top, c.Width - e.X, c.Height)
                    Case EdgeEnum.Right
                        c.SetBounds(c.Left, c.Top, c.Width - (c.Width - e.X), c.Height)
                    Case EdgeEnum.Top
                        c.SetBounds(c.Left, c.Top + e.Y, c.Width, c.Height - e.Y)
                    Case EdgeEnum.Bottom
                        c.SetBounds(c.Left, c.Top, c.Width, c.Height - (c.Height - e.Y))
                End Select
                c.ResumeLayout()
            Else
                Select Case True
                    Case e.X <= (mWidth * 4) And e.Y <= (mWidth * 4) 'top left corner
                        c.Cursor = Cursors.SizeAll
                        mEdge = EdgeEnum.TopLeft
                    Case e.X <= mWidth 'left edge
                        c.Cursor = Cursors.VSplit
                        mEdge = EdgeEnum.Left
                    Case e.X > c.Width - (mWidth + 1) 'right edge
                        c.Cursor = Cursors.VSplit
                        mEdge = EdgeEnum.Right
                    Case e.Y <= mWidth 'top edge
                        c.Cursor = Cursors.HSplit
                        mEdge = EdgeEnum.Top
                    Case e.Y > c.Height - (mWidth + 1) 'bottom edge
                        c.Cursor = Cursors.HSplit
                        mEdge = EdgeEnum.Bottom
                    Case Else 'no edge
                        c.Cursor = Cursors.Default
                        mEdge = EdgeEnum.None
                End Select
            End If
        End Sub

        Private Sub mControl_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles mControl.MouseLeave
            Dim c As Control = CType(sender, Control)
            mEdge = EdgeEnum.None
            c.BackgroundImage = Nothing
            c.Refresh()
        End Sub

    End Class
    Class Zlibtool
        <DllImport("zlib1.dll", EntryPoint:="compress", SetLastError:=True, CharSet:=CharSet.Unicode, ExactSpelling:=True, CallingConvention:=CallingConvention.Cdecl)> _
        Friend Shared Function CompressByteArray(ByVal dest As Byte(), ByRef destLen As Integer, ByVal src As Byte(), ByVal srcLen As Integer) As Integer
        End Function

        <DllImport("zlib1.dll", EntryPoint:="uncompress", CallingConvention:=CallingConvention.Cdecl)> _
        Friend Shared Function UncompressByteArray(ByVal dest As Byte(), ByRef destLen As Integer, ByVal src As Byte(), ByVal srcLen As Integer) As Integer
        End Function
    End Class
#Region "MAPEO"
    Private Sub Rojo_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rojo.Resize
        Rojo.BringToFront()
        RojoAL.Value = Rojo.Size.Height.ToString()
        RojoAN.Value = Rojo.Size.Width.ToString()
        'Textura marcador
        Try
            cropX = Rojo.Left
            cropY = Rojo.Top
            cropWidth = Rojo.Width
            cropHeight = Rojo.Height
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(Mapeo.BackgroundImage, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            Scr_CB = Nothing
            Scr_CB = cropBitmap
        Catch exc As Exception
        End Try
    End Sub
    Private Sub Rojo_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rojo.Move
        Rojo.BringToFront()
        RojoX.Value = Rojo.Left
        RojoY.Value = Rojo.Top
        'Textura marcador
        Try
            cropX = Rojo.Left
            cropY = Rojo.Top
            cropWidth = Rojo.Width
            cropHeight = Rojo.Height
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(Mapeo.BackgroundImage, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            Scr_CB = Nothing
            Scr_CB = cropBitmap
        Catch exc As Exception
        End Try
    End Sub
    Private Sub RojoX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RojoY.ValueChanged, RojoX.ValueChanged
        Rojo.Left = CInt(RojoX.Value)
        Rojo.Top = CInt(RojoY.Value)
    End Sub
    Private Sub RojoAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RojoAN.ValueChanged, RojoAL.ValueChanged
        Rojo.Width = CInt(RojoAN.Value)
        Rojo.Height = CInt(RojoAL.Value)
    End Sub
    Private Sub Azul_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Azul.Resize
        Azul.BringToFront()
        AzulAL.Value = Azul.Size.Height.ToString()
        AzulAN.Value = Azul.Size.Width.ToString()
        'Textura marcador
        Try
            cropX = Azul.Left
            cropY = Azul.Top
            cropWidth = Azul.Width
            cropHeight = Azul.Height
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(Mapeo.BackgroundImage, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            PlayerTEXTL_CB = Nothing
            Select Case CheckBox1.CheckState
                Case CheckState.Checked
                    cropBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX) 'Flip the image 
                    PlayerTEXTL_CB = cropBitmap
                Case CheckState.Unchecked
                    PlayerTEXTL_CB = cropBitmap
            End Select
        Catch exc As Exception
        End Try
    End Sub
    Private Sub Azul_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Azul.Move
        Azul.BringToFront()
        AzulX.Value = Azul.Left
        AzulY.Value = Azul.Top
        'Textura marcador
        Try
            cropX = Azul.Left
            cropY = Azul.Top
            cropWidth = Azul.Width
            cropHeight = Azul.Height
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(Mapeo.BackgroundImage, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            PlayerTEXTL_CB = Nothing
            Select Case CheckBox1.CheckState
                Case CheckState.Checked
                    cropBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX) 'Flip the image 
                    PlayerTEXTL_CB = cropBitmap
                Case CheckState.Unchecked
                    PlayerTEXTL_CB = cropBitmap
            End Select
        Catch exc As Exception
        End Try
    End Sub
    Private Sub AzulX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AzulY.ValueChanged, AzulX.ValueChanged
        Azul.Left = CInt(AzulX.Value)
        Azul.Top = CInt(AzulY.Value)
    End Sub
    Private Sub AzulAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AzulAN.ValueChanged, AzulAL.ValueChanged
        Azul.Width = CInt(AzulAN.Value)
        Azul.Height = CInt(AzulAL.Value)
    End Sub
    Private Sub Amarillo_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Amarillo.Resize
        Amarillo.BringToFront()
        AmarilloAL.Value = Amarillo.Size.Height.ToString()
        AmarilloAN.Value = Amarillo.Size.Width.ToString()
        'Textura marcador
        Try
            cropX = Amarillo.Left
            cropY = Amarillo.Top
            cropWidth = Amarillo.Width
            cropHeight = Amarillo.Height
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(Mapeo.BackgroundImage, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            EXTRA_CB = Nothing
            EXTRA_CB = cropBitmap
        Catch exc As Exception
        End Try
    End Sub
    Private Sub Amarillo_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Amarillo.Move
        Amarillo.BringToFront()
        AmarilloX.Value = Amarillo.Left
        AmarilloY.Value = Amarillo.Top
        'Textura marcador
        Try
            cropX = Amarillo.Left
            cropY = Amarillo.Top
            cropWidth = Amarillo.Width
            cropHeight = Amarillo.Height
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(Mapeo.BackgroundImage, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            EXTRA_CB = Nothing
            EXTRA_CB = cropBitmap
        Catch exc As Exception
        End Try
    End Sub
    Private Sub AmarilloX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AmarilloY.ValueChanged, AmarilloX.ValueChanged
        Amarillo.Left = CInt(AmarilloX.Value)
        Amarillo.Top = CInt(AmarilloY.Value)
    End Sub
    Private Sub AmarilloAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AmarilloAN.ValueChanged, AmarilloAL.ValueChanged
        Amarillo.Width = CInt(AmarilloAN.Value)
        Amarillo.Height = CInt(AmarilloAL.Value)
    End Sub
    Private Sub Violeta_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Violeta.Resize
        Violeta.BringToFront()
        VioletaAL.Value = Violeta.Size.Height.ToString()
        VioletaAN.Value = Violeta.Size.Width.ToString()
        'Textura marcador
        Try
            cropX = Violeta.Left
            cropY = Violeta.Top
            cropWidth = Violeta.Width
            cropHeight = Violeta.Height
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(Mapeo.BackgroundImage, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            Tv_CB = Nothing
            Tv_CB = cropBitmap
        Catch exc As Exception
        End Try
    End Sub
    Private Sub Violeta_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Violeta.Move
        Violeta.BringToFront()
        VioletaX.Value = Violeta.Left
        VioletaY.Value = Violeta.Top
        'Textura marcador
        Try
            cropX = Violeta.Left
            cropY = Violeta.Top
            cropWidth = Violeta.Width
            cropHeight = Violeta.Height
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(Mapeo.BackgroundImage, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            Tv_CB = Nothing
            Tv_CB = cropBitmap
        Catch exc As Exception
        End Try
    End Sub

    Private Sub VioletaX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VioletaY.ValueChanged, VioletaX.ValueChanged
        Violeta.Left = CInt(VioletaX.Value)
        Violeta.Top = CInt(VioletaY.Value)
    End Sub
    Private Sub VioletaAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VioletaAN.ValueChanged, VioletaAL.ValueChanged
        Violeta.Width = CInt(VioletaAN.Value)
        Violeta.Height = CInt(VioletaAL.Value)
    End Sub
    Private Sub VerdeX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VerdeY.ValueChanged, VerdeX.ValueChanged
        Verde.Left = CInt(VerdeX.Value)
        Verde.Top = CInt(VerdeY.Value)
    End Sub
    Private Sub VerdeAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VerdeAN.ValueChanged, VerdeAL.ValueChanged
        Verde.Width = CInt(VerdeAN.Value)
        Verde.Height = CInt(VerdeAL.Value)
    End Sub
    Private Sub Verde_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Verde.Resize
        Verde.BringToFront()
        VerdeAL.Value = Verde.Size.Height.ToString()
        VerdeAN.Value = Verde.Size.Width.ToString()
        'Textura marcador
        Try
            cropX = Verde.Left
            cropY = Verde.Top
            cropWidth = Verde.Width
            cropHeight = Verde.Height
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(Mapeo.BackgroundImage, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            PlayerTEXTV_CB = Nothing
            Select Case CheckBox2.CheckState
                Case CheckState.Checked
                    cropBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX) 'Flip the image 
                    PlayerTEXTV_CB = cropBitmap
                Case CheckState.Unchecked
                    PlayerTEXTV_CB = cropBitmap
            End Select
        Catch exc As Exception
        End Try
    End Sub
    Private Sub Verde_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Verde.Move
        Verde.BringToFront()
        VerdeX.Value = Verde.Left
        VerdeY.Value = Verde.Top
        'Textura marcador
        Try
            cropX = Verde.Left
            cropY = Verde.Top
            cropWidth = Verde.Width
            cropHeight = Verde.Height
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(Mapeo.BackgroundImage, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            PlayerTEXTV_CB = Nothing
            Select Case CheckBox2.CheckState
                Case CheckState.Checked
                    cropBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX) 'Flip the image 
                    PlayerTEXTV_CB = cropBitmap
                Case CheckState.Unchecked
                    PlayerTEXTV_CB = cropBitmap
            End Select
        Catch exc As Exception
        End Try
    End Sub

#End Region
#Region "Marcador-final"
    'Escudo Local

    Private Sub ScoreLX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreLX.ValueChanged
        ScoreL.Left = CInt(ScoreLX.Value)
    End Sub
    Private Sub ScoreLY_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreLY.ValueChanged
        ScoreL.Top = CInt(ScoreLY.Value)
    End Sub
    Private Sub ScoreLAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreLAN.ValueChanged
        ScoreL.Width = CInt(ScoreLAN.Value)
    End Sub
    Private Sub ScoreLAL_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreLAL.ValueChanged
        ScoreL.Height = CInt(ScoreLAL.Value)
    End Sub
    Private Sub ScoreVX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreVX.ValueChanged
        ScoreV.Left = CInt(ScoreVX.Value)
    End Sub
    Private Sub ScoreVY_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreVY.ValueChanged
        ScoreV.Top = CInt(ScoreVY.Value)
    End Sub
    Private Sub ScoreVAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreVAN.ValueChanged
        ScoreV.Width = CInt(ScoreVAN.Value)
    End Sub
    Private Sub ScoreVAL_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreVAL.ValueChanged
        ScoreV.Height = CInt(ScoreVAL.Value)
    End Sub
    Private Sub ScrX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScrY.ValueChanged, ScrX.ValueChanged
        Scr.Left = CInt(ScrX.Value)
        Scr.Top = CInt(ScrY.Value)
    End Sub
    Private Sub ScrAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScrAN.ValueChanged, ScrAL.ValueChanged
        Scr.Width = CInt(ScrAN.Value)
        Scr.Height = CInt(ScrAL.Value)
    End Sub
    
    Private Sub BanderaLX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaLY.ValueChanged, BanderaLX.ValueChanged
        BanderaL.Left = CInt(BanderaLX.Value)
        BanderaL.Top = CInt(BanderaLY.Value)
    End Sub
    Private Sub BanderaLAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaLAN.ValueChanged, BanderaLAL.ValueChanged
        BanderaL.Width = CInt(BanderaLAN.Value)
        BanderaL.Height = CInt(BanderaLAL.Value)
    End Sub
    Private Sub BanderaVX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaVY.ValueChanged, BanderaVX.ValueChanged
        BanderaV.Left = CInt(BanderaVX.Value)
        BanderaV.Top = CInt(BanderaVY.Value)
    End Sub
    Private Sub BanderaVAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaVAN.ValueChanged, BanderaVAL.ValueChanged
        BanderaV.Width = CInt(BanderaVAN.Value)
        BanderaV.Height = CInt(BanderaVAL.Value)
    End Sub
    Private Sub MinutosX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MinutosY.ValueChanged, MinutosX.ValueChanged
        Minutos.Left = CInt(MinutosX.Value)
        Minutos.Top = CInt(MinutosY.Value)
    End Sub
    Private Sub MinutosAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MinutosAN.ValueChanged, MinutosAL.ValueChanged
        Minutos.Width = CInt(MinutosAN.Value)
        Minutos.Height = CInt(MinutosAL.Value)
    End Sub
    Private Sub SegundosX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SegundosY.ValueChanged, SegundosX.ValueChanged
        Segundos.Left = CInt(SegundosX.Value)
        Segundos.Top = CInt(SegundosY.Value)
    End Sub
    Private Sub SegundosAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SegundosAN.ValueChanged, SegundosAL.ValueChanged
        Segundos.Width = CInt(SegundosAN.Value)
        Segundos.Height = CInt(SegundosAL.Value)
    End Sub
    Private Sub HalfX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HalfY.ValueChanged, HalfX.ValueChanged
        half.Left = CInt(HalfX.Value)
        half.Top = CInt(HalfY.Value)
    End Sub
    Private Sub HalfAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HalfAN.ValueChanged, HalfAL.ValueChanged
        half.Width = CInt(HalfAN.Value)
        half.Height = CInt(HalfAL.Value)
    End Sub

    Private Sub ScoreL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreL.Move
        ScoreLX.Value = ScoreL.Left
        ScoreLY.Value = ScoreL.Top
    End Sub
    Private Sub ScoreL_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreL.Resize
        ScoreLAL.Value = ScoreL.Size.Height.ToString()
        ScoreLAN.Value = ScoreL.Size.Width.ToString()
    End Sub
    Private Sub ScoreV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreV.Move
        ScoreVX.Value = ScoreV.Left
        ScoreVY.Value = ScoreV.Top
    End Sub
    Private Sub ScoreV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreV.Resize
        ScoreVAL.Value = ScoreV.Size.Height.ToString()
        ScoreVAN.Value = ScoreV.Size.Width.ToString()
    End Sub
    Private Sub Src_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Scr.Move
        ScrX.Value = Scr.Left
        ScrY.Value = Scr.Top
    End Sub
    Private Sub Src_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Scr.Resize
        ScrAL.Value = Scr.Size.Height.ToString()
        ScrAN.Value = Scr.Size.Width.ToString()
    End Sub
    
    Private Sub BanderaL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaL.Move
        BanderaLX.Value = BanderaL.Left
        BanderaLY.Value = BanderaL.Top
    End Sub
    Private Sub BanderaL_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaL.Resize
        BanderaLAL.Value = BanderaL.Size.Height.ToString()
        BanderaLAN.Value = BanderaL.Size.Width.ToString()
    End Sub
    Private Sub BanderaV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaV.Move
        BanderaVX.Value = BanderaV.Left
        BanderaVY.Value = BanderaV.Top
    End Sub
    Private Sub BanderaV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaV.Resize
        BanderaVAL.Value = BanderaV.Size.Height.ToString()
        BanderaVAN.Value = BanderaV.Size.Width.ToString()
    End Sub
    Private Sub Minutos_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Minutos.Move
        MinutosX.Value = Minutos.Left
        MinutosY.Value = Minutos.Top
    End Sub
    Private Sub Minutos_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Minutos.Resize
        MinutosAL.Value = Minutos.Size.Height.ToString()
        MinutosAN.Value = Minutos.Size.Width.ToString()
    End Sub
    Private Sub Segundos_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Segundos.Move
        SegundosX.Value = Segundos.Left
        SegundosY.Value = Segundos.Top
    End Sub
    Private Sub Segundos_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Segundos.Resize
        SegundosAL.Value = Segundos.Size.Height.ToString()
        SegundosAN.Value = Segundos.Size.Width.ToString()
    End Sub
    Private Sub Half_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles half.Move
        HalfX.Value = half.Left
        HalfY.Value = half.Top
    End Sub
    Private Sub Half_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles half.Resize
        HalfAL.Value = half.Size.Height.ToString()
        HalfAN.Value = half.Size.Width.ToString()
    End Sub
    Private Sub TIEMPCUMPLIDO_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TIEMPCUMPLIDO.Move
        TIEMPCUMPLIDOX.Value = TIEMPCUMPLIDO.Left
        TIEMPCUMPLIDOY.Value = TIEMPCUMPLIDO.Top
    End Sub
    Private Sub TIEMPCUMPLIDO_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TIEMPCUMPLIDO.Resize
        TIEMPCUMPLIDOAL.Value = TIEMPCUMPLIDO.Size.Height.ToString()
        TIEMPCUMPLIDOAN.Value = TIEMPCUMPLIDO.Size.Width.ToString()
    End Sub
    Private Sub TIEMPCUMPLIDOX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TIEMPCUMPLIDOY.ValueChanged, TIEMPCUMPLIDOX.ValueChanged
        TIEMPCUMPLIDO.Left = CInt(TIEMPCUMPLIDOX.Value)
        TIEMPCUMPLIDO.Top = CInt(TIEMPCUMPLIDOY.Value)
    End Sub
    Private Sub TIEMPCUMPLIDOAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TIEMPCUMPLIDOAN.ValueChanged, TIEMPCUMPLIDOAL.ValueChanged
        TIEMPCUMPLIDO.Width = CInt(TIEMPCUMPLIDOAN.Value)
        TIEMPCUMPLIDO.Height = CInt(TIEMPCUMPLIDOAL.Value)
    End Sub

#End Region
#Region "otros"
    Private Sub EscalaX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EscalaY.ValueChanged, EscalaX.ValueChanged
        Escala.Left = EscalaX.Value
        Escala.Top = EscalaY.Value
    End Sub
    Private Sub EscalaAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EscalaAN.ValueChanged, EscalaAL.ValueChanged
        Escala.Width = CInt(EscalaAN.Value)
        Escala.Height = CInt(EscalaAL.Value)
    End Sub
    Private Sub Escala_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Escala.Move
        EscalaX.Value = Escala.Left
        EscalaY.Value = Escala.Top
    End Sub
    Private Sub Escala_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Escala.Resize
        EscalaAL.Value = Escala.Size.Height.ToString()
        EscalaAN.Value = Escala.Size.Width.ToString()
    End Sub
    Private Sub PlayerLX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerLY.ValueChanged, PlayerLX.ValueChanged
        PlayerL.Left = CInt(PlayerLX.Value)
        PlayerL.Top = CInt(PlayerLY.Value)
    End Sub
    Private Sub PlayerLAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerLAN.ValueChanged, PlayerLAL.ValueChanged
        PlayerL.Width = CInt(PlayerLAN.Value)
        PlayerL.Height = CInt(PlayerLAL.Value)
    End Sub
    Private Sub PlayerVX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerVY.ValueChanged, PlayerVX.ValueChanged
        PlayerV.Left = CInt(PlayerVX.Value)
        PlayerV.Top = CInt(PlayerVY.Value)
    End Sub
    Private Sub PlayerVAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerVAN.ValueChanged, PlayerVAL.ValueChanged
        PlayerV.Width = CInt(PlayerVAN.Value)
        PlayerV.Height = CInt(PlayerVAL.Value)
    End Sub
    Private Sub PosLX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PosLY.ValueChanged, PosLX.ValueChanged
        PosL.Left = CInt(PosLX.Value)
        PosL.Top = CInt(PosLY.Value)
    End Sub
    Private Sub PosVX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PosVY.ValueChanged, PosVX.ValueChanged
        PosV.Left = CInt(PosVX.Value)
        PosV.Top = CInt(PosVY.Value)
    End Sub
    Private Sub ResistenciaLX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResistenciaLY.ValueChanged, ResistenciaLX.ValueChanged
        ResistenciaL.Left = CInt(ResistenciaLX.Value)
        ResistenciaL.Top = CInt(ResistenciaLY.Value)
    End Sub
    Private Sub ResistenciaLAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResistenciaLAN.ValueChanged, ResistenciaLAL.ValueChanged
        ResistenciaL.Width = CInt(ResistenciaLAN.Value)
        ResistenciaL.Height = CInt(ResistenciaLAL.Value)
    End Sub
    Private Sub ResistenciaVX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResistenciaVY.ValueChanged, ResistenciaVX.ValueChanged
        ResistenciaV.Left = CInt(ResistenciaVX.Value)
        ResistenciaV.Top = CInt(ResistenciaVY.Value)
    End Sub
    Private Sub ResistenciaVAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResistenciaVAN.ValueChanged, ResistenciaVAL.ValueChanged
        ResistenciaV.Width = CInt(ResistenciaVAN.Value)
        ResistenciaV.Height = CInt(ResistenciaVAL.Value)
    End Sub
    Private Sub PlayerTEXTLX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerTEXTLY.ValueChanged, PlayerTEXTLX.ValueChanged
        PlayerTEXTL.Left = CInt(PlayerTEXTLX.Value)
        PlayerTEXTL.Top = CInt(PlayerTEXTLY.Value)
    End Sub
    Private Sub PlayerTEXTLAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerTEXTLAN.ValueChanged, PlayerTEXTLAL.ValueChanged
        PlayerTEXTL.Width = CInt(PlayerTEXTLAN.Value)
        PlayerTEXTL.Height = CInt(PlayerTEXTLAL.Value)
    End Sub
    Private Sub PlayerTEXTVX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerTEXTVY.ValueChanged, PlayerTEXTVX.ValueChanged
        PlayerTEXTV.Left = CInt(PlayerTEXTVX.Value)
        PlayerTEXTV.Top = CInt(PlayerTEXTVY.Value)
    End Sub
    Private Sub PlayerTEXTVAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerTEXTVAN.ValueChanged, PlayerTEXTVAL.ValueChanged
        PlayerTEXTV.Width = CInt(PlayerTEXTVAN.Value)
        PlayerTEXTV.Height = CInt(PlayerTEXTVAL.Value)
    End Sub
    Private Sub PlayerL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerL.Move
        PlayerLX.Value = PlayerL.Left
        PlayerLY.Value = PlayerL.Top
    End Sub
    Private Sub PlayerL_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerL.Resize
        PlayerLAL.Value = PlayerL.Size.Height.ToString()
        PlayerLAN.Value = PlayerL.Size.Width.ToString()
    End Sub
    Private Sub PlayerV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerV.Move
        PlayerVX.Value = PlayerV.Left
        PlayerVY.Value = PlayerV.Top
    End Sub
    Private Sub PlayerV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerV.Resize
        PlayerVAL.Value = PlayerV.Size.Height.ToString()
        PlayerVAN.Value = PlayerV.Size.Width.ToString()
    End Sub
    Private Sub PosL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PosL.Move
        PosLX.Value = PosL.Left
        PosLY.Value = PosL.Top
    End Sub
    Private Sub PosL_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PosL.Resize
        PosLAL.Value = PosL.Size.Height.ToString()
        PosLAN.Value = PosL.Size.Width.ToString()
    End Sub
    Private Sub PosV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PosV.Move
        PosVX.Value = PosV.Left
        PosVY.Value = PosV.Top
    End Sub
    Private Sub PosV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PosV.Resize
        PosVAL.Value = PosV.Size.Height.ToString()
        PosVAN.Value = PosV.Size.Width.ToString()
    End Sub
    Private Sub ResistenciaL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResistenciaL.Move
        ResistenciaLX.Value = ResistenciaL.Left
        ResistenciaLY.Value = ResistenciaL.Top
    End Sub
    Private Sub ResistenciaL_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResistenciaL.Resize
        ResistenciaLAL.Value = ResistenciaL.Size.Height.ToString()
        ResistenciaLAN.Value = ResistenciaL.Size.Width.ToString()
    End Sub
    Private Sub ResistenciaV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResistenciaV.Move
        ResistenciaVX.Value = ResistenciaV.Left
        ResistenciaVY.Value = ResistenciaV.Top
    End Sub
    Private Sub ResistenciaV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResistenciaV.Resize
        ResistenciaVAL.Value = ResistenciaV.Size.Height.ToString()
        ResistenciaVAN.Value = ResistenciaV.Size.Width.ToString()
    End Sub
    Private Sub PlayerTEXTL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerTEXTL.Move
        PlayerTEXTLX.Value = PlayerTEXTL.Left
        PlayerTEXTLY.Value = PlayerTEXTL.Top
    End Sub
    Private Sub PlayerTEXTL_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerTEXTL.Resize
        PlayerTEXTLAL.Value = PlayerTEXTL.Size.Height.ToString()
        PlayerTEXTLAN.Value = PlayerTEXTL.Size.Width.ToString()
    End Sub
    Private Sub PlayerTEXTV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerTEXTV.Move
        PlayerTEXTVX.Value = PlayerTEXTV.Left
        PlayerTEXTVY.Value = PlayerTEXTV.Top
    End Sub
    Private Sub PlayerTEXTV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerTEXTV.Resize
        PlayerTEXTVAL.Value = PlayerTEXTV.Size.Height.ToString()
        PlayerTEXTVAN.Value = PlayerTEXTV.Size.Width.ToString()
    End Sub
    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        AboutBox1.ShowDialog()
    End Sub
    Private Sub TV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TV.Move
        TVX.Value = TV.Left
        TVY.Value = TV.Top
    End Sub
    Private Sub TV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TV.Resize
        TVAL.Value = TV.Size.Height.ToString()
        TVAN.Value = TV.Size.Width.ToString()
    End Sub
    Private Sub TVX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TVY.ValueChanged, TVX.ValueChanged
        TV.Left = CInt(TVX.Value)
        TV.Top = CInt(TVY.Value)
    End Sub
    Private Sub TVAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TVAN.ValueChanged, TVAL.ValueChanged
        TV.Width = CInt(TVAN.Value)
        TV.Height = CInt(TVAL.Value)
    End Sub
    Private Sub EXTRA_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EXTRA.Move
        EXTRAX.Value = EXTRA.Left
        EXTRAY.Value = EXTRA.Top
    End Sub
    Private Sub EXTRA_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EXTRA.Resize
        EXTRAAL.Value = EXTRA.Size.Height.ToString()
        EXTRAAN.Value = EXTRA.Size.Width.ToString()
    End Sub
    Private Sub EXTRAX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EXTRAY.ValueChanged, EXTRAX.ValueChanged
        EXTRA.Left = CInt(EXTRAX.Value)
        EXTRA.Top = CInt(EXTRAY.Value)
    End Sub
    Private Sub EXTRAAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EXTRAAN.ValueChanged, EXTRAAL.ValueChanged
        EXTRA.Width = CInt(EXTRAAN.Value)
        EXTRA.Height = CInt(EXTRAAL.Value)
    End Sub
    Private Sub AtaqueLX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AtaqueLY.ValueChanged, AtaqueLX.ValueChanged
        AtaqueL.Left = CInt(AtaqueLX.Value)
        AtaqueL.Top = CInt(AtaqueLY.Value)

    End Sub
    Private Sub AtaqueLAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AtaqueLAN.ValueChanged, AtaqueLAL.ValueChanged
        AtaqueL.Width = CInt(AtaqueLAN.Value)
        AtaqueL.Height = CInt(AtaqueLAL.Value)
    End Sub
    Private Sub AtaqueVX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AtaqueVY.ValueChanged, AtaqueVX.ValueChanged
        AtaqueV.Left = CInt(AtaqueVX.Value)
        AtaqueV.Top = CInt(AtaqueVY.Value)

    End Sub
    Private Sub AtaqueVAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AtaqueVAN.ValueChanged, AtaqueVAL.ValueChanged
        AtaqueV.Width = CInt(AtaqueVAN.Value)
        AtaqueV.Height = CInt(AtaqueVAL.Value)
    End Sub
    Private Sub AtaqueL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AtaqueL.Move
        AtaqueLX.Value = AtaqueL.Left
        AtaqueLY.Value = AtaqueL.Top
    End Sub
    Private Sub AtaqueL_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AtaqueL.Resize
        AtaqueLAL.Value = AtaqueL.Size.Height.ToString()
        AtaqueLAN.Value = AtaqueL.Size.Width.ToString()
    End Sub
    Private Sub AtaqueV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AtaqueV.Move
        AtaqueVX.Value = AtaqueV.Left
        AtaqueVY.Value = AtaqueV.Top
    End Sub
    Private Sub AtaqueV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AtaqueV.Resize
        AtaqueVAL.Value = AtaqueV.Size.Height.ToString()
        AtaqueVAN.Value = AtaqueV.Size.Width.ToString()
    End Sub

    Private Sub PotenciaL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PotenciaL.Move
        PotenciaLX.Value = PotenciaL.Left
        PotenciaLY.Value = PotenciaL.Top
    End Sub
    Private Sub PotenciaL_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PotenciaL.Resize
        PotenciaLAL.Value = PotenciaL.Size.Height.ToString()
        PotenciaLAN.Value = PotenciaL.Size.Width.ToString()
    End Sub
    Private Sub PotenciaLX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PotenciaLY.ValueChanged, PotenciaLX.ValueChanged
        PotenciaL.Left = CInt(PotenciaLX.Value)
        PotenciaL.Top = CInt(PotenciaLY.Value)
    End Sub
    Private Sub PotenciaLAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PotenciaLAN.ValueChanged, PotenciaLAL.ValueChanged
        PotenciaL.Width = CInt(PotenciaLAN.Value)
        PotenciaL.Height = CInt(PotenciaLAL.Value)
    End Sub
    Private Sub PotenciaV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PotenciaV.Move
        PotenciaVX.Value = PotenciaV.Left
        PotenciaVY.Value = PotenciaV.Top
    End Sub
    Private Sub PotenciaV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PotenciaV.Resize
        PotenciaVAL.Value = PotenciaV.Size.Height.ToString()
        PotenciaVAN.Value = PotenciaV.Size.Width.ToString()
    End Sub
    Private Sub PotenciaVX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PotenciaVY.ValueChanged, PotenciaVX.ValueChanged
        PotenciaV.Left = CInt(PotenciaVX.Value)
        PotenciaV.Top = CInt(PotenciaVY.Value)
    End Sub
    Private Sub PotenciaVAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PotenciaVAN.ValueChanged, PotenciaVAL.ValueChanged
        PotenciaV.Width = CInt(PotenciaVAN.Value)
        PotenciaV.Height = CInt(PotenciaVAL.Value)
    End Sub


    Private Sub AAL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AAL.Move
        AALX.Value = AAL.Left
        AALY.Value = AAL.Top
    End Sub
    Private Sub AALX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AALY.ValueChanged, AALX.ValueChanged
        AAL.Left = CInt(AALX.Value)
        AAL.Top = CInt(AALY.Value)
    End Sub
    Private Sub ABL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ABL.Move
        ABLX.Value = ABL.Left
        ABLY.Value = ABL.Top
    End Sub
    Private Sub ABLX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ABLY.ValueChanged, ABLX.ValueChanged
        ABL.Left = CInt(ABLX.Value)
        ABL.Top = CInt(ABLY.Value)
    End Sub
    Private Sub ACL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ACL.Move
        ACLX.Value = ACL.Left
        ACLY.Value = ACL.Top
    End Sub
    Private Sub ACLX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ACLY.ValueChanged, ACLX.ValueChanged
        ACL.Left = CInt(ACLX.Value)
        ACL.Top = CInt(ACLY.Value)
    End Sub
    Private Sub ADL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ADL.Move
        ADLX.Value = ADL.Left
        ADLY.Value = ADL.Top
    End Sub
    Private Sub ADLX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ADLY.ValueChanged, ADLX.ValueChanged
        ADL.Left = CInt(ADLX.Value)
        ADL.Top = CInt(ADLY.Value)
    End Sub
    Private Sub AEL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AEL.Move
        AELX.Value = AEL.Left
        AELY.Value = AEL.Top
    End Sub
    Private Sub AELX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AELY.ValueChanged, AELX.ValueChanged
        AEL.Left = CInt(AELX.Value)
        AEL.Top = CInt(AELY.Value)
    End Sub
    Private Sub AAV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AAV.Move
        AAVX.Value = AAV.Left
        AAVY.Value = AAV.Top
    End Sub
    Private Sub AAVX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AAVY.ValueChanged, AAVX.ValueChanged
        AAV.Left = CInt(AAVX.Value)
        AAV.Top = CInt(AAVY.Value)
    End Sub
    Private Sub ABV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ABV.Move
        ABVX.Value = ABV.Left
        ABVY.Value = ABV.Top
    End Sub
    Private Sub ABVX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ABVY.ValueChanged, ABVX.ValueChanged
        ABV.Left = CInt(ABVX.Value)
        ABV.Top = CInt(ABVY.Value)
    End Sub
    Private Sub ACV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ACV.Move
        ACVX.Value = ACV.Left
        ACVY.Value = ACV.Top
    End Sub
    Private Sub ACVX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ACVY.ValueChanged, ACVX.ValueChanged
        ACV.Left = CInt(ACVX.Value)
        ACV.Top = CInt(ACVY.Value)
    End Sub
    Private Sub ADV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ADV.Move
        ADVX.Value = ADV.Left
        ADVY.Value = ADV.Top
    End Sub
    Private Sub ADVX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ADVY.ValueChanged, ADVX.ValueChanged
        ADV.Left = CInt(ADVX.Value)
        ADV.Top = CInt(ADVY.Value)
    End Sub
    Private Sub AEV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AEV.Move
        AEVX.Value = AEV.Left
        AEVY.Value = AEV.Top
    End Sub
    Private Sub AEVX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AEVY.ValueChanged, AEVX.ValueChanged
        AEV.Left = CInt(AEVX.Value)
        AEV.Top = CInt(AEVY.Value)
    End Sub

#End Region
#Region "Funciones extras"
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        VerdeX.Value = AzulX.Value
        VerdeY.Value = AzulY.Value
        VerdeAN.Value = AzulAN.Value
        VerdeAL.Value = AzulAL.Value
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        AzulX.Value = VerdeX.Value
        AzulY.Value = VerdeY.Value
        AzulAN.Value = VerdeAN.Value
        AzulAL.Value = VerdeAL.Value
    End Sub
    Private Sub CheckBox1_CheckStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckStateChanged
        Try
            Select Case CheckBox1.CheckState
                Case CheckState.Checked
                    PlayerTEXTL_CB.RotateFlip(RotateFlipType.RotateNoneFlipX) 'Flip the image 
                    PlayerTEXTL.Refresh()
                Case CheckState.Unchecked
                    PlayerTEXTL_CB.RotateFlip(RotateFlipType.RotateNoneFlipX) 'Flip the image 
                    PlayerTEXTL.Refresh()
            End Select
        Catch ex As Exception

        End Try
    End Sub
    Private Sub CheckBox2_CheckStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckStateChanged
        Try
            Select Case CheckBox2.CheckState
                Case CheckState.Checked
                    PlayerTEXTV_CB.RotateFlip(RotateFlipType.RotateNoneFlipX) 'Flip the image 
                    PlayerTEXTV.Refresh()
                Case CheckState.Unchecked
                    PlayerTEXTV_CB.RotateFlip(RotateFlipType.RotateNoneFlipX) 'Flip the image 
                    PlayerTEXTV.Refresh()
            End Select
        Catch ex As Exception

        End Try
    End Sub
    Private Sub about_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles about.Click
        AboutBox1.ShowDialog()
    End Sub
    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start("https://www.facebook.com/PatoL18")
    End Sub
    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        System.Diagnostics.Process.Start("http://foro.pesretro.net")
    End Sub
    Private Sub RestaurarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RestaurarToolStripMenuItem.Click
        Select Case ResAlta.CheckState
            Case CheckState.Checked
                Panel1.BackgroundImage = My.Resources._1024BG
            Case CheckState.Unchecked
                Panel1.BackgroundImage = My.Resources._640BG
        End Select
    End Sub
    Private Sub CargarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CargarToolStripMenuItem.Click

        Dim BG As New System.Windows.Forms.OpenFileDialog
        BG.Filter = "Archivos de Imagen |*.png; *.jpg; *.bmp" + "|All Files|*.*"
        If BG.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        If Not BG.FileName Is Nothing Then
            Background = System.Drawing.Bitmap.FromFile(BG.FileName)
            Panel1.BackgroundImage = Background
        End If

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.ColorDialog1.ShowDialog()
        Me.Mapeo.BackColor = Me.ColorDialog1.Color
    End Sub


    Private Sub EscudoL_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EscudoL.DoubleClick
        Dim openDlg As New System.Windows.Forms.OpenFileDialog
        openDlg.Filter = "Archivos de Imagen |*.png; *.bmp" + "|All Files|*.*"
        If openDlg.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        If Not openDlg.FileName Is Nothing Then
            Dim img As New FileStream(openDlg.FileName, FileMode.Open)
            EscudoL.Image = System.Drawing.Image.FromStream(img)
            img.Close()
        End If
    End Sub
    Private Sub EscudoV_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EscudoV.DoubleClick
        Dim openDlg As New System.Windows.Forms.OpenFileDialog
        openDlg.Filter = "Archivos de Imagen |*.png; *.bmp" + "|All Files|*.*"
        If openDlg.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        If Not openDlg.FileName Is Nothing Then
            Dim img As New FileStream(openDlg.FileName, FileMode.Open)
            EscudoV.Image = System.Drawing.Image.FromStream(img)
            img.Close()
        End If
    End Sub
    Private Sub BanderaL_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaL.DoubleClick
        Dim openDlg As New System.Windows.Forms.OpenFileDialog
        openDlg.Filter = "Archivos de Imagen |*.png; *.bmp" + "|All Files|*.*"
        If openDlg.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        If Not openDlg.FileName Is Nothing Then
            Dim img As New FileStream(openDlg.FileName, FileMode.Open)
            BanderaL.Image = System.Drawing.Image.FromStream(img)
            img.Close()
        End If
    End Sub
    Private Sub BanderaV_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaV.DoubleClick
        Dim openDlg As New System.Windows.Forms.OpenFileDialog
        openDlg.Filter = "Archivos de Imagen |*.png; *.bmp" + "|All Files|*.*"
        If openDlg.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        If Not openDlg.FileName Is Nothing Then
            Dim img As New FileStream(openDlg.FileName, FileMode.Open)
            BanderaV.Image = System.Drawing.Image.FromStream(img)
            img.Close()
        End If
    End Sub



#End Region

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint
        '' crear un objeto bitmap

        Try
            Dim Scr_Graphics As Graphics = e.Graphics
            Scr_Graphics.DrawImage(Scr_CB, Me.ScrX.Value, Me.ScrY.Value, Me.ScrAN.Value, Me.ScrAL.Value)
            Dim TV_Graphics As Graphics = e.Graphics
            TV_Graphics.DrawImage(Tv_CB, Me.TVX.Value, Me.TVY.Value, Me.TVAN.Value, Me.TVAL.Value)
            Dim EXTRA_Graphics As Graphics = e.Graphics
            EXTRA_Graphics.DrawImage(EXTRA_CB, Me.EXTRAX.Value, Me.EXTRAY.Value, Me.EXTRAAN.Value, Me.EXTRAAL.Value)
            Dim PlayerTEXTL_Graphics As Graphics = e.Graphics
            PlayerTEXTL_Graphics.DrawImage(PlayerTEXTL_CB, Me.PlayerTEXTLX.Value, Me.PlayerTEXTLY.Value, Me.PlayerTEXTLAN.Value, Me.PlayerTEXTLAL.Value)
            Dim PlayerTEXTV_Graphics As Graphics = e.Graphics
            PlayerTEXTV_Graphics.DrawImage(PlayerTEXTV_CB, Me.PlayerTEXTVX.Value, Me.PlayerTEXTVY.Value, Me.PlayerTEXTVAN.Value, Me.PlayerTEXTVAL.Value)

            Dim AtaqueL_Graphics As Graphics = e.Graphics
            AtaqueL_Graphics.DrawImage(AtaqueL.Image, Me.AtaqueLX.Value, Me.AtaqueLY.Value, Me.AtaqueLAN.Value, Me.AtaqueLAL.Value)
            Dim AtaqueV_Graphics As Graphics = e.Graphics
            AtaqueV_Graphics.DrawImage(AtaqueV.Image, Me.AtaqueVX.Value, Me.AtaqueVY.Value, Me.AtaqueVAN.Value, Me.AtaqueVAL.Value)

        Catch ex As Exception

        End Try

    End Sub

    Dim zliblong As Integer
    Dim unzliblong As Integer
    Dim zlibfile() As Byte

    Dim Xvalor As String
    Dim Yvalor As String
    Dim XvalorNP As String
    Private Sub Importarwe8(ByVal we8 As Boolean)
        Try
            My.Computer.FileSystem.DeleteFile(dlgAbrir.FileName & "_unzlib")
        Catch ex As Exception

        End Try
        'Descomprimir
        Dim zlibfs As New FileStream(dlgAbrir.FileName, FileMode.Open)
        Dim zlibbr As New BinaryReader(zlibfs)

        zlibfs.Position = 4
        zliblong = zlibbr.ReadInt32
        zlibfs.Position = 8
        unzliblong = zlibbr.ReadInt32


        zlibfs.Seek(32, SeekOrigin.Begin)
        zlibfile = zlibbr.ReadBytes(zliblong)

        Dim unzlibfile(unzliblong - 1) As Byte
        Zlibtool.UncompressByteArray(unzlibfile, unzliblong, zlibfile, zliblong)
        System.IO.File.WriteAllBytes(dlgAbrir.FileName & "_unzlib", unzlibfile)
        zlibfs.Close()
        zlibbr.Close()


        Dim fs As New FileStream(dlgAbrir.FileName & "_unzlib", FileMode.Open)
        Dim br As New BinaryReader(fs)
        'Modalidad textura
        fs.Seek(40, SeekOrigin.Begin)
        Dim Bytes() As Byte = br.ReadBytes(7)
        Dim SCR_slot As String = Encoding.UTF8.GetString(Bytes) 'Convierte los bytes a string(texto)
        modalidad.Text = SCR_slot

        Exportar.Enabled = True


        Select Case ResAlta.CheckState
            Case CheckState.Checked
                EJESX = 8 'Divisores
                EJESY = 11.94 'Divisores
                Xvalor = 512
                Yvalor = 300
                XvalorNP = 1058
            Case CheckState.Unchecked
                EJESX = 12.8 'Divisores
                EJESY = 14.93 'Divisores
                Xvalor = 320
                Yvalor = 240
                XvalorNP = 661
        End Select

        Dim value As Integer 'offset  para CB
        
        'ScoreL
        OFFSET = 1232 'offset Fuente
        fs.Position = OFFSET
        ScoreLX.Text = (br.ReadInt16() / EJESX) + Xvalor
        fs.Position = OFFSET + 2
        ScoreLY.Text = (br.ReadInt16() / EJESY) + Yvalor
        fs.Position = OFFSET + 6
        ScoreLAN.Text = (br.ReadInt16() / EJESX)
        fs.Position = OFFSET + 8
        ScoreLAL.Text = (br.ReadInt16() / EJESY)
        fs.Position = OFFSET - 3
        value = br.ReadByte.ToString
        If value = 0 Then ScoreLT.Text = "Pequeño"
        If value = 1 Then ScoreLT.Text = "Grande"
        fs.Position = OFFSET + 11
        value = br.ReadByte.ToString
        If value = 0 Then ScoreLC.Text = "Blanco"
        If value = 1 Then ScoreLC.Text = "Negro"
        'ScoreV
        OFFSET = 1256 'offset Fuente
        fs.Position = OFFSET
        ScoreVX.Text = br.ReadInt16()
        ScoreVX.Text = (ScoreVX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2
        ScoreVY.Text = br.ReadInt16()
        ScoreVY.Text = (ScoreVY.Text / EJESY) + Yvalor
        fs.Position = OFFSET + 6
        ScoreVAN.Text = br.ReadInt16()
        ScoreVAN.Text = (ScoreVAN.Text / EJESX)
        fs.Position = OFFSET + 8
        ScoreVAL.Text = br.ReadInt16()
        ScoreVAL.Text = (ScoreVAL.Text / EJESY)
        fs.Position = OFFSET - 3
        value = br.ReadByte.ToString
        If value = 0 Then ScoreVT.Text = "Pequeño"
        If value = 1 Then ScoreVT.Text = "Grande"
        fs.Position = OFFSET + 11
        value = br.ReadByte.ToString
        If value = 0 Then ScoreVC.Text = "Blanco"
        If value = 1 Then ScoreVC.Text = "Negro"
        'half
        OFFSET = 1280 'offset Fuente
        fs.Position = OFFSET
        HalfX.Text = br.ReadInt16()
        HalfX.Text = (HalfX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2
        HalfY.Text = br.ReadInt16()
        HalfY.Text = (HalfY.Text / EJESY) + Yvalor
        fs.Position = OFFSET + 6
        HalfAN.Text = br.ReadInt16()
        HalfAN.Text = (HalfAN.Text / EJESX)
        fs.Position = OFFSET + 8
        HalfAL.Text = br.ReadInt16()
        HalfAL.Text = (HalfAL.Text / EJESY)
        fs.Position = OFFSET - 3
        value = br.ReadByte.ToString
        If value = 0 Then HalfT.Text = "Pequeño"
        If value = 1 Then HalfT.Text = "Grande"
        fs.Position = OFFSET + 11
        value = br.ReadByte.ToString
        If value = 0 Then HalfC.Text = "Blanco"
        If value = 1 Then HalfC.Text = "Negro"
        'Minutos
        OFFSET = 1304 'offset Fuente
        fs.Position = OFFSET
        MinutosX.Text = br.ReadInt16()
        MinutosX.Text = (MinutosX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2
        MinutosY.Text = br.ReadInt16()
        MinutosY.Text = (MinutosY.Text / EJESY) + Yvalor
        fs.Position = OFFSET + 6
        MinutosAN.Text = br.ReadInt16()
        MinutosAN.Text = (MinutosAN.Text / EJESX)
        fs.Position = OFFSET + 8
        MinutosAL.Text = br.ReadInt16()
        MinutosAL.Text = (MinutosAL.Text / EJESY)
        fs.Position = OFFSET - 3
        value = br.ReadByte.ToString
        If value = 0 Then MinutosT.Text = "Pequeño"
        If value = 1 Then MinutosT.Text = "Grande"
        fs.Position = OFFSET + 11
        value = br.ReadByte.ToString
        If value = 0 Then MinutosC.Text = "Blanco"
        If value = 1 Then MinutosC.Text = "Negro"
        'Segundos
        OFFSET = 1328 'offset Fuente
        fs.Position = OFFSET
        SegundosX.Text = br.ReadInt16()
        SegundosX.Text = (SegundosX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2
        SegundosY.Text = br.ReadInt16()
        SegundosY.Text = (SegundosY.Text / EJESY) + Yvalor
        fs.Position = OFFSET + 6
        SegundosAN.Text = br.ReadInt16()
        SegundosAN.Text = (SegundosAN.Text / EJESX)
        fs.Position = OFFSET + 8
        SegundosAL.Text = br.ReadInt16()
        SegundosAL.Text = (SegundosAL.Text / EJESY)
        fs.Position = OFFSET - 3
        value = br.ReadByte.ToString
        If value = 0 Then SegundosT.Text = "Pequeño"
        If value = 1 Then SegundosT.Text = "Grande"
        fs.Position = OFFSET + 11
        value = br.ReadByte.ToString
        If value = 0 Then SegundosC.Text = "Blanco"
        If value = 1 Then SegundosC.Text = "Negro"
        'TIEMPCUMPLIDO
        OFFSET = 1352 'offset Fuente
        fs.Position = OFFSET
        TIEMPCUMPLIDOX.Text = br.ReadInt16()
        TIEMPCUMPLIDOX.Text = (TIEMPCUMPLIDOX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2
        TIEMPCUMPLIDOY.Text = br.ReadInt16()
        TIEMPCUMPLIDOY.Text = (TIEMPCUMPLIDOY.Text / EJESY) + Yvalor
        fs.Position = OFFSET + 6
        TIEMPCUMPLIDOAN.Text = br.ReadInt16()
        TIEMPCUMPLIDOAN.Text = (TIEMPCUMPLIDOAN.Text / EJESX)
        fs.Position = OFFSET + 8
        TIEMPCUMPLIDOAL.Text = br.ReadInt16()
        TIEMPCUMPLIDOAL.Text = (TIEMPCUMPLIDOAL.Text / EJESY)
        fs.Position = OFFSET - 3
        value = br.ReadByte.ToString
        If value = 0 Then TIEMPCUMPLIDOT.Text = "Pequeño"
        If value = 1 Then TIEMPCUMPLIDOT.Text = "Grande"
        fs.Position = OFFSET + 11
        value = br.ReadByte.ToString
        If value = 0 Then TIEMPCUMPLIDOC.Text = "Blanco"
        If value = 1 Then TIEMPCUMPLIDOC.Text = "Negro"

        ''Nombres Placa Local
        'PotenciaL
        OFFSET = 2340 'offset Fuente
        fs.Position = OFFSET
        PotenciaLX.Text = br.ReadInt16()
        PotenciaLX.Text = (PotenciaLX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2
        PotenciaLY.Text = br.ReadInt16()
        PotenciaLY.Text = (PotenciaLY.Text / EJESY) + Yvalor
        fs.Position = OFFSET + 6
        PotenciaLAN.Text = br.ReadInt16()
        PotenciaLAN.Text = (PotenciaLAN.Text / EJESX)
        fs.Position = OFFSET + 8
        PotenciaLAL.Text = br.ReadInt16()
        PotenciaLAL.Text = (PotenciaLAL.Text / EJESY)
        'PlayerL
        OFFSET = 2354 'offset Fuente
        fs.Position = OFFSET
        PlayerLX.Text = br.ReadInt16()
        PlayerLX.Text = (PlayerLX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2
        PlayerLY.Text = br.ReadInt16()
        PlayerLY.Text = (PlayerLY.Text / EJESY) + Yvalor
        fs.Position = OFFSET + 6
        PlayerLAN.Text = br.ReadInt16()
        PlayerLAN.Text = (PlayerLAN.Text / EJESX)
        fs.Position = OFFSET + 8
        PlayerLAL.Text = br.ReadInt16()
        PlayerLAL.Text = (PlayerLAL.Text / EJESY)
        'PosL
        OFFSET = 2368 'offset Fuente
        fs.Position = OFFSET
        PosLX.Text = br.ReadInt16()
        PosLX.Text = (PosLX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2
        PosLY.Text = br.ReadInt16()
        PosLY.Text = (PosLY.Text / EJESY) + Yvalor

        PosL.Visible = True

        'PLACA ATAQUE LOCAL, Identificar caso 
        'Ataque Placa Local
        OFFSET = 2282
        AtaqueL.Visible = True
        fs.Position = OFFSET 'offset
        AtaqueLX.Text = br.ReadInt16()
        AtaqueLX.Text = (AtaqueLX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2 'offset
        AtaqueLY.Text = br.ReadInt16()
        AtaqueLY.Text = (AtaqueLY.Text / EJESY) + Yvalor
        fs.Position = OFFSET + 26 'offset
        AtaqueLAN.Text = br.ReadInt16()
        AtaqueLAN.Text = (AtaqueLAN.Text / EJESX)
        fs.Position = OFFSET + 28 'offset
        AtaqueLAL.Text = br.ReadInt16()
        AtaqueLAL.Text = (AtaqueLAL.Text / EJESY)
        'Ataque Placa Local
        OFFSET = 2382
        'Ataque AAL
        fs.Position = OFFSET 'offset
        AALX.Text = br.ReadInt16()
        AALX.Text = (AALX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2 'offset
        AALY.Text = br.ReadInt16()
        AALY.Text = (AALY.Text / EJESY) + Yvalor
        'Ataque ABL
        fs.Position = OFFSET + 14 'offset
        ABLX.Text = br.ReadInt16()
        ABLX.Text = (ABLX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 16 'offset
        ABLY.Text = br.ReadInt16()
        ABLY.Text = (ABLY.Text / EJESY) + Yvalor
        'Ataque ACL
        fs.Position = OFFSET + 28 'offset
        ACLX.Text = br.ReadInt16()
        ACLX.Text = (ACLX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 30 'offset
        ACLY.Text = br.ReadInt16()
        ACLY.Text = (ACLY.Text / EJESY) + Yvalor
        'Ataque ADL
        fs.Position = OFFSET + 42 'offset
        ADLX.Text = br.ReadInt16()
        ADLX.Text = (ADLX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 44 'offset
        ADLY.Text = br.ReadInt16()
        ADLY.Text = (ADLY.Text / EJESY) + Yvalor
        'Ataque AEL
        fs.Position = OFFSET + 56 'offset
        AELX.Text = br.ReadInt16()
        AELX.Text = (AELX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 58 'offset
        AELY.Text = br.ReadInt16()
        AELY.Text = (AELY.Text / EJESY) + Yvalor


        ''Nombres Placa Visitante
        'PotenciaV
        OFFSET = 3156 'offset 3156
        fs.Position = OFFSET
        PotenciaVX.Text = br.ReadInt16()
        PotenciaVX.Text = (PotenciaVX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2
        PotenciaVY.Text = br.ReadInt16()
        PotenciaVY.Text = (PotenciaVY.Text / EJESY) + Yvalor
        fs.Position = OFFSET + 6
        PotenciaVAN.Text = br.ReadInt16()
        PotenciaVAN.Text = (PotenciaVAN.Text / EJESX)
        fs.Position = OFFSET + 8
        PotenciaVAL.Text = br.ReadInt16()
        PotenciaVAL.Text = (PotenciaVAL.Text / EJESY)
        'PlayerV
        OFFSET = 3170 'offset Fuente
        fs.Position = OFFSET
        PlayerVX.Text = br.ReadInt16()
        PlayerVX.Text = (PlayerVX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2
        PlayerVY.Text = br.ReadInt16()
        PlayerVY.Text = (PlayerVY.Text / EJESY) + Yvalor
        fs.Position = OFFSET + 6
        PlayerVAN.Text = br.ReadInt16()
        PlayerVAN.Text = (PlayerVAN.Text / EJESX)
        fs.Position = OFFSET + 8
        PlayerVAL.Text = br.ReadInt16()
        PlayerVAL.Text = (PlayerVAL.Text / EJESY)
        'PosV
        OFFSET = 3184 'offset Fuente
        fs.Position = OFFSET
        PosVX.Text = br.ReadInt16()
        PosVX.Text = (PosVX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2
        PosVY.Text = br.ReadInt16()
        PosVY.Text = (PosVY.Text / EJESY) + Yvalor
        PosV.Visible = True

        'PLACA ATAQUE vISITA, Identificar caso
        'Ataque Placa Visita
        OFFSET = 3098
        AtaqueV.Visible = True
        fs.Position = OFFSET 'offset
        AtaqueVX.Text = br.ReadInt16()
        AtaqueVX.Text = (AtaqueVX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2 'offset
        AtaqueVY.Text = br.ReadInt16()
        AtaqueVY.Text = (AtaqueVY.Text / EJESY) + Yvalor
        fs.Position = OFFSET + 26 'offset
        AtaqueVAN.Text = br.ReadInt16()
        AtaqueVAN.Text = (AtaqueVAN.Text / EJESX)
        fs.Position = OFFSET + 28 'offset
        AtaqueVAL.Text = br.ReadInt16()
        AtaqueVAL.Text = (AtaqueVAL.Text / EJESY)

        'Ataque Placa Visita
        OFFSET = 3198
        'Ataque AAV
        fs.Position = OFFSET 'offset
        AAVX.Text = br.ReadInt16()
        AAVX.Text = (AAVX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2 'offset
        AAVY.Text = br.ReadInt16()
        AAVY.Text = (AAVY.Text / EJESY) + Yvalor
        'Ataque ABV
        fs.Position = OFFSET + 14 'offset
        ABVX.Text = br.ReadInt16()
        ABVX.Text = (ABVX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 16 'offset
        ABVY.Text = br.ReadInt16()
        ABVY.Text = (ABVY.Text / EJESY) + Yvalor
        'Ataque ACV
        fs.Position = OFFSET + 28 'offset
        ACVX.Text = br.ReadInt16()
        ACVX.Text = (ACVX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 30 'offset
        ACVY.Text = br.ReadInt16()
        ACVY.Text = (ACVY.Text / EJESY) + Yvalor
        'Ataque ADV
        fs.Position = OFFSET + 42 'offset
        ADVX.Text = br.ReadInt16()
        ADVX.Text = (ADVX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 44 'offset
        ADVY.Text = br.ReadInt16()
        ADVY.Text = (ADVY.Text / EJESY) + Yvalor
        'Ataque AEV
        fs.Position = OFFSET + 56 'offset
        AEVX.Text = br.ReadInt16()
        AEVX.Text = (AEVX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 58 'offset
        AEVY.Text = br.ReadInt16()
        AEVY.Text = (AEVY.Text / EJESY) + Yvalor


        
        'TEXTURA TV
        OFFSET = 598 'offset Rango X
        fs.Position = OFFSET 'offset X (3A/58)
        TVX.Text = br.ReadInt16()
        TVX.Text = (TVX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2 'offset
        TVY.Text = br.ReadInt16()
        TVY.Text = (TVY.Text / EJESY) + Yvalor
        fs.Position = OFFSET + 26 'offset
        TVAN.Text = br.ReadInt16()
        TVAN.Text = (TVAN.Text / EJESX)
        fs.Position = OFFSET + 16 'offset
        TVAL.Text = br.ReadInt16()
        TVAL.Text = (TVAL.Text / EJESY)
        'Mapeo Textura1
        Dim TVa As Integer
        Dim TVb As Integer
        Dim TVc As Integer
        Dim TVd As Integer
        fs.Position = OFFSET + 38 'offset x
        TVa = br.ReadInt16()
        fs.Position = OFFSET + 46 'offset an
        TVb = br.ReadInt16()
        fs.Position = OFFSET + 44 'offset y, al
        TVc = br.ReadInt16()
        fs.Position = OFFSET + 40 'offset 
        TVd = br.ReadInt16()
        VioletaX.Text = TVa
        VioletaX.Text = (VioletaX.Text / 8)
        VioletaAN.Text = TVb
        VioletaAN.Text = (VioletaAN.Text / 8) - VioletaX.Text
        VioletaY.Text = TVc
        VioletaY.Text = (VioletaY.Text / 8)
        VioletaAL.Text = TVd
        VioletaAL.Text = (VioletaAL.Text / 8) - VioletaY.Text
        'TEXTURA EXTRA
        OFFSET = 662 'offset Rango X
        fs.Position = OFFSET 'offset X (3A/58)
        EXTRAX.Text = br.ReadInt16()
        EXTRAX.Text = (EXTRAX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2 'offset
        EXTRAY.Text = br.ReadInt16()
        EXTRAY.Text = (EXTRAY.Text / EJESY) + Yvalor
        fs.Position = OFFSET + 26 'offset
        EXTRAAN.Text = br.ReadInt16()
        EXTRAAN.Text = (EXTRAAN.Text / EJESX)
        fs.Position = OFFSET + 16 'offset
        EXTRAAL.Text = br.ReadInt16()
        EXTRAAL.Text = (EXTRAAL.Text / EJESY)
        'Mapeo Textura1
        Dim EXTRAa As Integer
        Dim EXTRAb As Integer
        Dim EXTRAc As Integer
        Dim EXTRAd As Integer
        fs.Position = OFFSET + 38 'offset x
        EXTRAa = br.ReadInt16()
        fs.Position = OFFSET + 46 'offset an
        EXTRAb = br.ReadInt16()
        fs.Position = OFFSET + 44 'offset y, al
        EXTRAc = br.ReadInt16()
        fs.Position = OFFSET + 40 'offset 
        EXTRAd = br.ReadInt16()
        AmarilloX.Text = EXTRAa
        AmarilloX.Text = (AmarilloX.Text / 8)
        AmarilloAN.Text = EXTRAb
        AmarilloAN.Text = (AmarilloAN.Text / 8) - AmarilloX.Text
        AmarilloY.Text = EXTRAc
        AmarilloY.Text = (AmarilloY.Text / 8)
        AmarilloAL.Text = EXTRAd
        AmarilloAL.Text = (AmarilloAL.Text / 8) - AmarilloY.Text


        'TEXTURA PlayerTEXTL
        OFFSET = 1706 'offset Rango X
        fs.Position = OFFSET 'offset X (3A/58)
        PlayerTEXTLX.Text = br.ReadInt16()
        PlayerTEXTLX.Text = (PlayerTEXTLX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2 'offset
        PlayerTEXTLY.Text = br.ReadInt16()
        PlayerTEXTLY.Text = (PlayerTEXTLY.Text / EJESY) + Yvalor
        fs.Position = OFFSET + 26 'offset
        PlayerTEXTLAN.Text = br.ReadInt16()
        PlayerTEXTLAN.Text = (PlayerTEXTLAN.Text / EJESX)
        fs.Position = OFFSET + 16 'offset
        PlayerTEXTLAL.Text = br.ReadInt16()
        PlayerTEXTLAL.Text = (PlayerTEXTLAL.Text / EJESY)
        'Mapeo Textura1
        Dim PlayerTEXTLa As Integer
        Dim PlayerTEXTLb As Integer
        Dim PlayerTEXTLc As Integer
        Dim PlayerTEXTLd As Integer
        fs.Position = OFFSET + 38 'offset x
        PlayerTEXTLa = br.ReadInt16()
        fs.Position = OFFSET + 46 'offset an
        PlayerTEXTLb = br.ReadInt16()
        fs.Position = OFFSET + 44 'offset y, al
        PlayerTEXTLc = br.ReadInt16()
        fs.Position = OFFSET + 40 'offset 
        PlayerTEXTLd = br.ReadInt16()
        If PlayerTEXTLa <= PlayerTEXTLb Then 'Normal
            AzulX.Text = PlayerTEXTLa
            AzulX.Text = (AzulX.Text / 8)
            AzulAN.Text = PlayerTEXTLb
            AzulAN.Text = (AzulAN.Text / 8) - AzulX.Text
            CheckBox1.Checked = False
        ElseIf PlayerTEXTLa > PlayerTEXTLb Then 'Invertir
            AzulX.Text = PlayerTEXTLb
            AzulX.Text = (AzulX.Text / 8)
            AzulAN.Text = PlayerTEXTLa
            AzulAN.Text = (AzulAN.Text / 8) - AzulX.Text
            CheckBox1.Checked = True
        End If
        AzulY.Text = PlayerTEXTLc
        AzulY.Text = (AzulY.Text / 8)
        AzulAL.Text = PlayerTEXTLd
        AzulAL.Text = (AzulAL.Text / 8) - AzulY.Text

        'TEXTURA PlayerTEXTV
        OFFSET = 2522 'offset Rango X
        fs.Position = OFFSET 'offset X (3A/58)
        PlayerTEXTVX.Text = br.ReadInt16()
        PlayerTEXTVX.Text = (PlayerTEXTVX.Text / EJESX) + Xvalor
        fs.Position = OFFSET + 2 'offset
        PlayerTEXTVY.Text = br.ReadInt16()
        PlayerTEXTVY.Text = (PlayerTEXTVY.Text / EJESY) + Yvalor
        fs.Position = OFFSET + 26 'offset
        PlayerTEXTVAN.Text = br.ReadInt16()
        PlayerTEXTVAN.Text = (PlayerTEXTVAN.Text / EJESX)
        fs.Position = OFFSET + 16 'offset
        PlayerTEXTVAL.Text = br.ReadInt16()
        PlayerTEXTVAL.Text = (PlayerTEXTVAL.Text / EJESY)
        'Mapeo Textura1
        Dim PlayerTEXTVa As Integer
        Dim PlayerTEXTVb As Integer
        Dim PlayerTEXTVc As Integer
        Dim PlayerTEXTVd As Integer
        fs.Position = OFFSET + 38 'offset x
        PlayerTEXTVa = br.ReadInt16()
        fs.Position = OFFSET + 46 'offset an
        PlayerTEXTVb = br.ReadInt16()
        fs.Position = OFFSET + 44 'offset y, al
        PlayerTEXTVc = br.ReadInt16()
        fs.Position = OFFSET + 40 'offset 
        PlayerTEXTVd = br.ReadInt16()
        If PlayerTEXTVa <= PlayerTEXTVb Then 'Normal
            VerdeX.Text = PlayerTEXTVa
            VerdeX.Text = (VerdeX.Text / 8)
            VerdeAN.Text = PlayerTEXTVb
            VerdeAN.Text = (VerdeAN.Text / 8) - VerdeX.Text
            CheckBox2.Checked = False
        ElseIf PlayerTEXTVa > PlayerTEXTVb Then 'Invertir
            VerdeX.Text = PlayerTEXTVb
            VerdeX.Text = (VerdeX.Text / 8)
            VerdeAN.Text = PlayerTEXTVa
            VerdeAN.Text = (VerdeAN.Text / 8) - VerdeX.Text
            CheckBox2.Checked = True
        End If
        VerdeY.Text = PlayerTEXTVc
        VerdeY.Text = (VerdeY.Text / 8)
        VerdeAL.Text = PlayerTEXTVd
        VerdeAL.Text = (VerdeAL.Text / 8) - VerdeY.Text
        br.Close()
        fs.Close()

        obj_logoRead(1376, BanderaL) 'Bandera Local
        obj_logoRead(1390, BanderaV) 'Bandera Visita
        obj_Textura4Read(534, Scr, Rojo)

        MsgBox("Marcador WE8 Cargado")
        Exportar.Enabled = True

    End Sub
    Private Sub exportarwe8(ByVal we8 As Boolean)
        'Guardar
        Dim fs As New FileStream(dlgAbrir.FileName & "_unzlib", FileMode.Open)
        Dim bw As New BinaryWriter(fs)

        'Escribir bytes en cero SCR
        Dim writemodalidad(7) As Byte
        writemodalidad(0) = 0
        fs.Position = 40
        fs.Write(writemodalidad, 0, 8)

        'Guardar Modalidad textura
        If modalidad.Text.Length <= 7 Then
            Dim Cadena As String = modalidad.Text
            Dim bytes2() As Byte = Encoding.ASCII.GetBytes(Cadena)
            bw.Seek(40, SeekOrigin.Begin)
            bw.Write(bytes2, 0, modalidad.Text.Length)
        Else
            MsgBox("Modalidad No debe contener más de 7 Carácteres")
            Exit Sub
        End If


        Select Case ResAlta.CheckState
            Case CheckState.Checked
                EJESX = 8 'Divisores
                EJESY = 11.94 'Divisores
                Xvalor = 512
                Yvalor = 300
                XvalorNP = 1058
            Case CheckState.Unchecked
                EJESX = 12.8 'Divisores
                EJESY = 14.93 'Divisores
                Xvalor = 320
                Yvalor = 240
                XvalorNP = 661
        End Select


        'Borrar Bytes Innecesarios
        ''parchar Marcador
        fs.Position = 726   'Ocultar placa
        bw.Write(Int16.Parse("8192"))
        fs.Position = 790   'Ocultar placa
        bw.Write(Int16.Parse("8192"))
        fs.Position = 854   'Ocultar placa
        bw.Write(Int16.Parse("8192"))
        fs.Position = 918   'Ocultar placa
        bw.Write(Int16.Parse("8192"))
        fs.Position = 982   'Ocultar placa
        bw.Write(Int16.Parse("8192"))
        fs.Position = 1046   'Ocultar placa
        bw.Write(Int16.Parse("8192"))
        fs.Position = 1110   'Ocultar placa
        bw.Write(Int16.Parse("8192"))
        fs.Position = 1174   'Ocultar placa
        bw.Write(Int16.Parse("8192"))


        'Escribir bytes en cero SCR
        Dim writeBytes(41) As Byte
        writeBytes(0) = 0
        fs.Position = 546
        fs.Write(writeBytes, 0, writeBytes.Count)
        'Escribir bytes en cero SCR
        fs.Position = 610
        fs.Write(writeBytes, 0, writeBytes.Count)
        'Escribir bytes en cero SCR
        fs.Position = 674
        fs.Write(writeBytes, 0, writeBytes.Count)
        'Escribir bytes en cero SCR
        fs.Position = 1718
        fs.Write(writeBytes, 0, writeBytes.Count)
        'Escribir bytes en cero SCR
        fs.Position = 2534
        fs.Write(writeBytes, 0, writeBytes.Count)

        

        'TEXTURA TV
        OFFSET = 598 'offset Rango X
        fs.Position = OFFSET 'offset
        temp.Text = (TVX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (TVY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 26 'offset
        temp.Text = (TVAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 32 'offset
        temp.Text = (TVAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 16 'offset
        temp.Text = (TVAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 28 'offset
        temp.Text = (TVAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'Mapeo TV
        fs.Position = OFFSET + 38 'offset x
        bw.Write(Int16.Parse(CDbl(Violeta.Left) * 8))
        fs.Position = OFFSET + 42 'offset
        bw.Write(Int16.Parse(CDbl(Violeta.Left) * 8))
        fs.Position = OFFSET + 46 'offset
        bw.Write(Int16.Parse(CDbl(Violeta.Width + Violeta.Left) * 8))
        fs.Position = OFFSET + 50 'offset
        bw.Write(Int16.Parse(CDbl(Violeta.Width + Violeta.Left) * 8))
        fs.Position = OFFSET + 44 'offset
        bw.Write(Int16.Parse(CDbl(Violeta.Top) * 8))
        fs.Position = OFFSET + 52 'offset
        bw.Write(Int16.Parse(CDbl(Violeta.Top) * 8))
        fs.Position = OFFSET + 40 'offset
        bw.Write(Int16.Parse(CDbl(Violeta.Height + Violeta.Top) * 8))
        fs.Position = OFFSET + 48 'offset
        bw.Write(Int16.Parse(CDbl(Violeta.Height + Violeta.Top) * 8))

        'TEXTURA EXTRA
        OFFSET = 662 'offset Rango X
        fs.Position = OFFSET 'offset
        temp.Text = (EXTRAX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (EXTRAY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 26 'offset
        temp.Text = (EXTRAAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 32 'offset
        temp.Text = (EXTRAAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 16 'offset
        temp.Text = (EXTRAAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 28 'offset
        temp.Text = (EXTRAAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'Mapeo EXTRA
        fs.Position = OFFSET + 38 'offset x
        bw.Write(Int16.Parse(CDbl(Amarillo.Left) * 8))
        fs.Position = OFFSET + 42 'offset
        bw.Write(Int16.Parse(CDbl(Amarillo.Left) * 8))
        fs.Position = OFFSET + 46 'offset
        bw.Write(Int16.Parse(CDbl(Amarillo.Width + Amarillo.Left) * 8))
        fs.Position = OFFSET + 50 'offset
        bw.Write(Int16.Parse(CDbl(Amarillo.Width + Amarillo.Left) * 8))
        fs.Position = OFFSET + 44 'offset
        bw.Write(Int16.Parse(CDbl(Amarillo.Top) * 8))
        fs.Position = OFFSET + 52 'offset
        bw.Write(Int16.Parse(CDbl(Amarillo.Top) * 8))
        fs.Position = OFFSET + 40 'offset
        bw.Write(Int16.Parse(CDbl(Amarillo.Height + Amarillo.Top) * 8))
        fs.Position = OFFSET + 48 'offset
        bw.Write(Int16.Parse(CDbl(Amarillo.Height + Amarillo.Top) * 8))

        'TEXTURA PlayerTEXTL
        OFFSET = 1706 'offset Rango X
        fs.Position = OFFSET 'offset
        temp.Text = (PlayerTEXTLX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (PlayerTEXTLY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 26 'offset
        temp.Text = (PlayerTEXTLAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 32 'offset
        temp.Text = (PlayerTEXTLAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 16 'offset
        temp.Text = (PlayerTEXTLAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 28 'offset
        temp.Text = (PlayerTEXTLAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'Mapeo PlayerTEXTL
        Select Case CheckBox1.CheckState
            Case CheckState.Checked
                fs.Position = OFFSET + 46 'offset x
                bw.Write(Int16.Parse(CDbl(Azul.Left) * 8))
                fs.Position = OFFSET + 50 'offset
                bw.Write(Int16.Parse(CDbl(Azul.Left) * 8))
                fs.Position = OFFSET + 38 'offset
                bw.Write(Int16.Parse(CDbl(Azul.Width + Azul.Left) * 8))
                fs.Position = OFFSET + 42 'offset
                bw.Write(Int16.Parse(CDbl(Azul.Width + Azul.Left) * 8))
            Case CheckState.Unchecked
                fs.Position = OFFSET + 38 'offset x
                bw.Write(Int16.Parse(CDbl(Azul.Left) * 8))
                fs.Position = OFFSET + 42 'offset
                bw.Write(Int16.Parse(CDbl(Azul.Left) * 8))
                fs.Position = OFFSET + 46 'offset
                bw.Write(Int16.Parse(CDbl(Azul.Width + Azul.Left) * 8))
                fs.Position = OFFSET + 50 'offset
                bw.Write(Int16.Parse(CDbl(Azul.Width + Azul.Left) * 8))
        End Select
        fs.Position = OFFSET + 44 'offset
        bw.Write(Int16.Parse(CDbl(Azul.Top) * 8))
        fs.Position = OFFSET + 52 'offset
        bw.Write(Int16.Parse(CDbl(Azul.Top) * 8))
        fs.Position = OFFSET + 40 'offset
        bw.Write(Int16.Parse(CDbl(Azul.Height + Azul.Top) * 8))
        fs.Position = OFFSET + 48 'offset
        bw.Write(Int16.Parse(CDbl(Azul.Height + Azul.Top) * 8))

        'TEXTURA PlayerTEXTV
        OFFSET = 2522 'offset Rango X
        fs.Position = OFFSET 'offset
        temp.Text = (PlayerTEXTVX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (PlayerTEXTVY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 26 'offset
        temp.Text = (PlayerTEXTVAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 32 'offset
        temp.Text = (PlayerTEXTVAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 16 'offset
        temp.Text = (PlayerTEXTVAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 28 'offset
        temp.Text = (PlayerTEXTVAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'Mapeo PlayerTEXTV
        Select Case CheckBox2.CheckState
            Case CheckState.Checked
                fs.Position = OFFSET + 46 'offset x
                bw.Write(Int16.Parse(CDbl(Verde.Left) * 8))
                fs.Position = OFFSET + 50 'offset
                bw.Write(Int16.Parse(CDbl(Verde.Left) * 8))
                fs.Position = OFFSET + 38 'offset
                bw.Write(Int16.Parse(CDbl(Verde.Width + Verde.Left) * 8))
                fs.Position = OFFSET + 42 'offset
                bw.Write(Int16.Parse(CDbl(Verde.Width + Verde.Left) * 8))
            Case CheckState.Unchecked
                fs.Position = OFFSET + 38 'offset x
                bw.Write(Int16.Parse(CDbl(Verde.Left) * 8))
                fs.Position = OFFSET + 42 'offset
                bw.Write(Int16.Parse(CDbl(Verde.Left) * 8))
                fs.Position = OFFSET + 46 'offset
                bw.Write(Int16.Parse(CDbl(Verde.Width + Verde.Left) * 8))
                fs.Position = OFFSET + 50 'offset
                bw.Write(Int16.Parse(CDbl(Verde.Width + Verde.Left) * 8))
        End Select
        fs.Position = OFFSET + 44 'offset
        bw.Write(Int16.Parse(CDbl(Verde.Top) * 8))
        fs.Position = OFFSET + 52 'offset
        bw.Write(Int16.Parse(CDbl(Verde.Top) * 8))
        fs.Position = OFFSET + 40 'offset
        bw.Write(Int16.Parse(CDbl(Verde.Height + Verde.Top) * 8))
        fs.Position = OFFSET + 48 'offset
        bw.Write(Int16.Parse(CDbl(Verde.Height + Verde.Top) * 8))

        ''parchar Name plate local
        'fs.Position = 2590 'offset
        'bw.Write(Int16.Parse("0"))
        'fs.Position = 2318   'Ocultar placa
        'bw.Write(Int16.Parse("8192"))
        'fs.Position = 2458   'Ocultar placa
        'bw.Write(Int16.Parse("8192"))
        'fs.Position = 2738   'Ocultar placa
        'bw.Write(Int16.Parse("8192"))
        ''Local
        'Dim writeBytes2(118) As Byte
        'writeBytes2(0) = 0
        'fs.Position = 2610
        'fs.Write(writeBytes2, 0, writeBytes2.Count)

        'BanderaL
        OFFSET = 1376
        fs.Position = OFFSET 'offset
        temp.Text = (BanderaLX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (BanderaLY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 6 'offset
        temp.Text = (BanderaLAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 8 'offset
        temp.Text = (BanderaLAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))

        ''BanderaV
        'OFFSET = 1390
        'fs.Position = OFFSET 'offset
        'temp.Text = (BanderaVX.Text - Xvalor) * EJESX
        'bw.Write(Int16.Parse(temp.Text))
        'fs.Position = OFFSET + 2 'offset
        'temp.Text = (BanderaVY.Text - Yvalor) * EJESY
        'bw.Write(Int16.Parse(temp.Text))
        'fs.Position = OFFSET + 6 'offset
        'temp.Text = (BanderaVAN.Text) * EJESX
        'bw.Write(Int16.Parse(temp.Text))
        'fs.Position = OFFSET + 8 'offset
        'temp.Text = (BanderaVAL.Text) * EJESY
        'bw.Write(Int16.Parse(temp.Text))

        'ScoreL
        OFFSET = 1232  'offset NOMBRE X
        fs.Position = OFFSET 'offset
        temp.Text = (ScoreLX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (ScoreLY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 6 'offset
        temp.Text = (ScoreLAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 8 'offset
        temp.Text = (ScoreLAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'Fuente Local
        fs.Position = OFFSET - 3 'offset TAMAÑO
        If ScoreLT.SelectedIndex = 0 Then bw.Write(Byte.Parse("0"))
        If ScoreLT.SelectedIndex = 1 Then bw.Write(Byte.Parse("1"))
        fs.Position = OFFSET + 11 'offset COLOR Fuente
        If ScoreLC.SelectedIndex = 0 Then bw.Write(Byte.Parse("0"))
        If ScoreLC.SelectedIndex = 1 Then bw.Write(Byte.Parse("1"))
        'ScoreV
        OFFSET = 1256  'offset NOMBRE X
        fs.Position = OFFSET 'offset
        temp.Text = (ScoreVX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (ScoreVY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 6 'offset
        temp.Text = (ScoreVAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 8 'offset
        temp.Text = (ScoreVAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'Fuente Local
        fs.Position = OFFSET - 3 'offset TAMAÑO
        If ScoreVT.SelectedIndex = 0 Then bw.Write(Byte.Parse("0"))
        If ScoreVT.SelectedIndex = 1 Then bw.Write(Byte.Parse("1"))
        fs.Position = OFFSET + 11 'offset COLOR Fuente
        If ScoreVC.SelectedIndex = 0 Then bw.Write(Byte.Parse("0"))
        If ScoreVC.SelectedIndex = 1 Then bw.Write(Byte.Parse("1"))
        'Half
        OFFSET = 1280  'offset NOMBRE X
        fs.Position = OFFSET 'offset
        temp.Text = (HalfX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (HalfY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 6 'offset
        temp.Text = (HalfAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 8 'offset
        temp.Text = (HalfAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'Fuente Local
        fs.Position = OFFSET - 3 'offset TAMAÑO
        If HalfT.SelectedIndex = 0 Then bw.Write(Byte.Parse("0"))
        If HalfT.SelectedIndex = 1 Then bw.Write(Byte.Parse("1"))
        fs.Position = OFFSET + 11 'offset COLOR Fuente
        If HalfC.SelectedIndex = 0 Then bw.Write(Byte.Parse("0"))
        If HalfC.SelectedIndex = 1 Then bw.Write(Byte.Parse("1"))
        'Minutos
        OFFSET = 1304  'offset NOMBRE X
        fs.Position = OFFSET 'offset
        temp.Text = (MinutosX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (MinutosY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 6 'offset
        temp.Text = (MinutosAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 8 'offset
        temp.Text = (MinutosAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'Fuente Local
        fs.Position = OFFSET - 3 'offset TAMAÑO
        If MinutosT.SelectedIndex = 0 Then bw.Write(Byte.Parse("0"))
        If MinutosT.SelectedIndex = 1 Then bw.Write(Byte.Parse("1"))
        fs.Position = OFFSET + 11 'offset COLOR Fuente
        If MinutosC.SelectedIndex = 0 Then bw.Write(Byte.Parse("0"))
        If MinutosC.SelectedIndex = 1 Then bw.Write(Byte.Parse("1"))
        'Segundos
        OFFSET = 1328  'offset NOMBRE X
        fs.Position = OFFSET 'offset
        temp.Text = (SegundosX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (SegundosY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 6 'offset
        temp.Text = (SegundosAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 8 'offset
        temp.Text = (SegundosAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'Fuente Local
        fs.Position = OFFSET - 3 'offset TAMAÑO
        If SegundosT.SelectedIndex = 0 Then bw.Write(Byte.Parse("0"))
        If SegundosT.SelectedIndex = 1 Then bw.Write(Byte.Parse("1"))
        fs.Position = OFFSET + 11 'offset COLOR Fuente
        If SegundosC.SelectedIndex = 0 Then bw.Write(Byte.Parse("0"))
        If SegundosC.SelectedIndex = 1 Then bw.Write(Byte.Parse("1"))
        'TIEMPCUMPLIDO
        OFFSET = 1352  'offset NOMBRE X
        fs.Position = OFFSET 'offset
        temp.Text = (TIEMPCUMPLIDOX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (TIEMPCUMPLIDOY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 6 'offset
        temp.Text = (TIEMPCUMPLIDOAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 8 'offset
        temp.Text = (TIEMPCUMPLIDOAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'Fuente Local
        fs.Position = OFFSET - 3 'offset TAMAÑO
        If TIEMPCUMPLIDOT.SelectedIndex = 0 Then bw.Write(Byte.Parse("0"))
        If TIEMPCUMPLIDOT.SelectedIndex = 1 Then bw.Write(Byte.Parse("1"))
        fs.Position = OFFSET + 11 'offset COLOR Fuente
        If TIEMPCUMPLIDOC.SelectedIndex = 0 Then bw.Write(Byte.Parse("0"))
        If TIEMPCUMPLIDOC.SelectedIndex = 1 Then bw.Write(Byte.Parse("1"))

        ''PotenciaL
        'OFFSET = 2218
        'fs.Position = OFFSET  'offset
        'temp.Text = (PotenciaLX.Text - Xvalor) * EJESX
        'bw.Write(Int16.Parse(temp.Text))
        'fs.Position = OFFSET + 2  'offset
        'temp.Text = (PotenciaLY.Text - Yvalor) * EJESY
        'bw.Write(Int16.Parse(temp.Text))
        'fs.Position = 16068 + 6 'offset
        'temp.Text = (PotenciaLAN.Text) * EJESX
        'bw.Write(Int16.Parse(temp.Text))
        'fs.Position = 16070 + 8 'offset
        'temp.Text = (PotenciaLAL.Text) * EJESY
        'bw.Write(Int16.Parse(temp.Text))


        'PotenciaL
        OFFSET = 2340
        fs.Position = OFFSET  'offset
        temp.Text = (PotenciaLX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (PotenciaLY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 6 'offset
        temp.Text = (PotenciaLAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 8 'offset
        temp.Text = (PotenciaLAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'PotenciaL
        OFFSET = 35680
        fs.Position = OFFSET  'offset
        temp.Text = (PotenciaLX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (PotenciaLY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 6 'offset
        temp.Text = (PotenciaLAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 8 'offset
        temp.Text = (PotenciaLAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'PotenciaL
        OFFSET = 31364
        fs.Position = OFFSET  'offset
        temp.Text = (PotenciaLX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (PotenciaLY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 6 'offset
        temp.Text = (PotenciaLAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 8 'offset
        temp.Text = (PotenciaLAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'Escala (Local)
        OFFSET = 13458
        fs.Position = OFFSET  'offset X
        temp.Text = (PotenciaLX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset y
        temp.Text = (PotenciaLY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        ''ResistenciaL
        'OFFSET = 2340  'offset NOMBRE X
        'fs.Position = OFFSET 'offset
        'temp.Text = (ResistenciaLX.Text - Xvalor) * EJESX
        'bw.Write(Int16.Parse(temp.Text))
        'fs.Position = OFFSET + 2 'offset
        'temp.Text = (ResistenciaLY.Text - Yvalor) * EJESY
        'bw.Write(Int16.Parse(temp.Text))
        'fs.Position = OFFSET + 6 'offset
        'temp.Text = (ResistenciaLAN.Text) * EJESX
        'bw.Write(Int16.Parse(temp.Text))
        'fs.Position = OFFSET + 8 'offset
        'temp.Text = (ResistenciaLAL.Text) * EJESY
        'bw.Write(Int16.Parse(temp.Text))
        'PlayerL
        OFFSET = 2354  'offset NOMBRE X
        fs.Position = OFFSET 'offset
        temp.Text = (PlayerLX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (PlayerLY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 6 'offset
        temp.Text = (PlayerLAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 8 'offset
        temp.Text = (PlayerLAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'PosL
        OFFSET = 2368  'offset NOMBRE X
        fs.Position = OFFSET 'offset
        temp.Text = (PosLX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (PosLY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))

        'ATAQUE LOCAL
        OFFSET = 2282
        'ATAQUE LOCAL PLACA
        fs.Position = OFFSET 'offset
        temp.Value = (AtaqueLX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 2 'offset
        temp.Value = (AtaqueLY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 26 'offset
        temp.Value = (AtaqueLAN.Value) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 32 'offset
        temp.Value = (AtaqueLAN.Value) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 16 'offset
        temp.Value = (AtaqueLAL.Value) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 28 'offset
        temp.Value = (AtaqueLAL.Value) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        OFFSET = 2382
        'AAL
        fs.Position = OFFSET  'offset
        temp.Value = (AALX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 2 'offset
        temp.Value = (AALY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ABL
        fs.Position = OFFSET + 14 'offset
        temp.Value = (ABLX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 16 'offset
        temp.Value = (ABLY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ACL
        fs.Position = OFFSET + 28 'offset
        temp.Value = (ACLX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 30 'offset
        temp.Value = (ACLY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ADL
        fs.Position = OFFSET + 42 'offset
        temp.Value = (ADLX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 44 'offset
        temp.Value = (ADLY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'AEL
        fs.Position = OFFSET + 56 'offset
        temp.Value = (AELX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 58 'offset
        temp.Value = (AELY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))

        OFFSET = 31406
        'AAL
        fs.Position = OFFSET  'offset
        temp.Value = (AALX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 2 'offset
        temp.Value = (AALY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ABL
        fs.Position = OFFSET + 14 'offset
        temp.Value = (ABLX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 16 'offset
        temp.Value = (ABLY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ACL
        fs.Position = OFFSET + 28 'offset
        temp.Value = (ACLX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 30 'offset
        temp.Value = (ACLY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ADL
        fs.Position = OFFSET + 42 'offset
        temp.Value = (ADLX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 44 'offset
        temp.Value = (ADLY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'AEL
        fs.Position = OFFSET + 56 'offset
        temp.Value = (AELX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 58 'offset
        temp.Value = (AELY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))

        OFFSET = 27686
        'AAL
        fs.Position = OFFSET  'offset
        temp.Value = (AALX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 2 'offset
        temp.Value = (AALY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ABL
        fs.Position = OFFSET + 14 'offset
        temp.Value = (ABLX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 16 'offset
        temp.Value = (ABLY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ACL
        fs.Position = OFFSET + 28 'offset
        temp.Value = (ACLX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 30 'offset
        temp.Value = (ACLY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ADL
        fs.Position = OFFSET + 42 'offset
        temp.Value = (ADLX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 44 'offset
        temp.Value = (ADLY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'AEL
        fs.Position = OFFSET + 56 'offset
        temp.Value = (AELX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 58 'offset
        temp.Value = (AELY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))

        'PotenciaV
        OFFSET = 3156
        fs.Position = OFFSET  'offset
        temp.Text = (PotenciaVX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (PotenciaVY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 6 'offset
        temp.Text = (PotenciaVAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 8 'offset
        temp.Text = (PotenciaVAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'PotenciaV
        OFFSET = 36176
        fs.Position = OFFSET  'offset
        temp.Text = (PotenciaVX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (PotenciaVY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 6 'offset
        temp.Text = (PotenciaVAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 8 'offset
        temp.Text = (PotenciaVAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'PotenciaV
        OFFSET = 31604
        fs.Position = OFFSET  'offset
        temp.Text = (PotenciaVX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (PotenciaVY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 6 'offset
        temp.Text = (PotenciaVAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 8 'offset
        temp.Text = (PotenciaVAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'Escala (Visita)
        OFFSET = 13698
        fs.Position = OFFSET  'offset X
        temp.Text = (PotenciaVX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset y
        temp.Text = (PotenciaVY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))

        'PlayerV
        OFFSET = 3170  'offset NOMBRE X
        fs.Position = OFFSET 'offset
        temp.Text = (PlayerVX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (PlayerVY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 6 'offset
        temp.Text = (PlayerVAN.Text) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 8 'offset
        temp.Text = (PlayerVAL.Text) * EJESY
        bw.Write(Int16.Parse(temp.Text))
        'PosV
        OFFSET = 3184  'offset NOMBRE X
        fs.Position = OFFSET 'offset
        temp.Text = (PosVX.Text - Xvalor) * EJESX
        bw.Write(Int16.Parse(temp.Text))
        fs.Position = OFFSET + 2 'offset
        temp.Text = (PosVY.Text - Yvalor) * EJESY
        bw.Write(Int16.Parse(temp.Text))

        'ATAQUE VISITA
        OFFSET = 3098
        fs.Position = OFFSET 'offset
        temp.Value = (AtaqueVX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 2 'offset
        temp.Value = (AtaqueVY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 26 'offset
        temp.Value = (AtaqueVAN.Value) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 32 'offset
        temp.Value = (AtaqueVAN.Value) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 16 'offset
        temp.Value = (AtaqueVAL.Value) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 28 'offset
        temp.Value = (AtaqueVAL.Value) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))

        'ATAQUE VISITA
        OFFSET = 3198
        'AAL
        fs.Position = OFFSET  'offset
        temp.Value = (AAVX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 2 'offset
        temp.Value = (AAVY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ABL
        fs.Position = OFFSET + 14 'offset
        temp.Value = (ABVX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 16 'offset
        temp.Value = (ABVY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ACL
        fs.Position = OFFSET + 28 'offset
        temp.Value = (ACVX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 30 'offset
        temp.Value = (ACVY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ADL
        fs.Position = OFFSET + 42 'offset
        temp.Value = (ADVX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 44 'offset
        temp.Value = (ADVY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'AEL
        fs.Position = OFFSET + 56 'offset
        temp.Value = (AEVX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 58 'offset
        temp.Value = (AEVY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))

        OFFSET = 28182
        'AAL
        fs.Position = OFFSET  'offset
        temp.Value = (AAVX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 2 'offset
        temp.Value = (AAVY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ABL
        fs.Position = OFFSET + 14 'offset
        temp.Value = (ABVX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 16 'offset
        temp.Value = (ABVY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ACL
        fs.Position = OFFSET + 28 'offset
        temp.Value = (ACVX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 30 'offset
        temp.Value = (ACVY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ADL
        fs.Position = OFFSET + 42 'offset
        temp.Value = (ADVX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 44 'offset
        temp.Value = (ADVY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'AEL
        fs.Position = OFFSET + 56 'offset
        temp.Value = (AEVX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 58 'offset
        temp.Value = (AEVY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))

        OFFSET = 31646
        'AAL
        fs.Position = OFFSET  'offset
        temp.Value = (AAVX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 2 'offset
        temp.Value = (AAVY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ABL
        fs.Position = OFFSET + 14 'offset
        temp.Value = (ABVX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 16 'offset
        temp.Value = (ABVY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ACL
        fs.Position = OFFSET + 28 'offset
        temp.Value = (ACVX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 30 'offset
        temp.Value = (ACVY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'ADL
        fs.Position = OFFSET + 42 'offset
        temp.Value = (ADVX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 44 'offset
        temp.Value = (ADVY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        'AEL
        fs.Position = OFFSET + 56 'offset
        temp.Value = (AEVX.Value - Xvalor) * EJESX
        bw.Write(Int16.Parse(CInt(temp.Value)))
        fs.Position = OFFSET + 58 'offset
        temp.Value = (AEVY.Value - Yvalor) * EJESY
        bw.Write(Int16.Parse(CInt(temp.Value)))
        bw.Close()
        fs.Close()

        'Bandera Visitante
        obj_logoWrite(1390, BanderaV)
        obj_Textura4Write(534, Scr, Rojo)
        'Leer
        Dim unzlibfs As New FileStream(dlgAbrir.FileName & "_unzlib", FileMode.Open)
        Dim unzlibbr As New BinaryReader(unzlibfs)
        Dim unzlibfile() As Byte
        unzlibfs.Seek(0, SeekOrigin.Begin)
        unzlibfile = unzlibbr.ReadBytes(unzlibfs.Length)
        unzliblong = unzlibfs.Length
        Dim zlibfilenew(unzlibfs.Length) As Byte
        Zlibtool.CompressByteArray(zlibfilenew, unzlibfs.Length, unzlibfile, unzlibfs.Length)
        'System.IO.File.WriteAllBytes(archivo & "_2", bytes)


        Dim zlibfs As New FileStream(dlgAbrir.FileName, FileMode.Open)
        Dim zlibbw As New BinaryWriter(zlibfs)

        'Buscar bytes seguidos en cero
        Dim find As Byte = 0
        Dim index As Integer = 0
        Dim index2 As Integer = 0
        Dim index3 As Integer = 0
        Dim index4 As Integer = 0

        Do
            index = System.Array.IndexOf(Of Byte)(zlibfilenew, find, index + 1)
            index2 = System.Array.IndexOf(Of Byte)(zlibfilenew, find, index + 2)
            index3 = System.Array.IndexOf(Of Byte)(zlibfilenew, find, index2 + 3)
            index4 = System.Array.IndexOf(Of Byte)(zlibfilenew, find, index3 + 4)
            If index4 - index = 9 Then
                'MsgBox(index & "=" & index4)
                Exit Do
            End If
        Loop



        Dim opdbyte() As Byte = {&H0, &H4, &H1, &H0}
        zlibfs.Position = 0
        zlibfs.Write(opdbyte, 0, 4)
        zlibfs.Position = 4
        zlibbw.Write(Int32.Parse(index))
        zlibfs.Position = 8
        zlibbw.Write(Int32.Parse(unzliblong))

        zlibfs.Position = 32
        zlibfs.Write(zlibfilenew, 0, index + 16)
        zlibfs.Position = 33
        zlibbw.Write(Byte.Parse(CInt("&HDA")))

        zlibfs.Close()
        zlibbw.Close()


        unzlibfs.Close()
        unzlibbr.Close()



        MsgBox("Marcador Guardado", MsgBoxStyle.Information)

    End Sub
#Region "Objetos Leer-Escribir"
    'Logos
    Private Sub obj_logoWrite(ByVal offsetX As Integer, ByVal logo As PictureBox)
        'Guardar
        Dim fs As New FileStream(dlgAbrir.FileName & "_unzlib", FileMode.Open)
        Dim bw As New BinaryWriter(fs)
        fs.Position = offsetX     'offset X
        bw.Write(Int16.Parse(Round((logo.Left - Xvalor) * EJESX)))
        fs.Position = offsetX + 2 'offset Y
        bw.Write(Int16.Parse(Round((logo.Top - Yvalor) * EJESY)))
        fs.Position = offsetX + 6 'offset ancho
        bw.Write(Int16.Parse(Round(logo.Width * EJESX)))
        fs.Position = offsetX + 8 'offset alto
        bw.Write(Int16.Parse(Round(logo.Height * EJESY)))
        fs.Close()
        bw.Close()
    End Sub
    Private Sub obj_logoRead(ByVal offsetX As Integer, ByVal logo As PictureBox)
        'leer
        Dim fs As New FileStream(dlgAbrir.FileName & "_unzlib", FileMode.Open)
        Dim br As New BinaryReader(fs)
        'Bandera Visita
        fs.Position = offsetX     'offset
        logo.Left = (br.ReadInt16() / EJESX) + Xvalor
        fs.Position = offsetX + 2 'offset
        logo.Top = (br.ReadInt16() / EJESY) + Yvalor
        fs.Position = offsetX + 6 'offset
        logo.Width = (br.ReadInt16() / EJESX)
        fs.Position = offsetX + 8 'offset
        logo.Height = (br.ReadInt16() / EJESY)
        fs.Close()
        br.Close()
    End Sub

    'Texturas
    Private Sub obj_Textura4Write(ByVal offsetX As Integer, ByVal textura As PictureBox, ByVal mapeo As PictureBox)
        'Guardar
        Dim fs As New FileStream(dlgAbrir.FileName & "_unzlib", FileMode.Open)
        Dim bw As New BinaryWriter(fs)
        

        'TEXTURA scr
        OFFSET = 534 'offset Rango X
        fs.Position = offsetX 'offset
        bw.Write(Int16.Parse(Round((textura.Top - Xvalor) * EJESX)))
        fs.Position = offsetX + 2 'offset Y
        bw.Write(Int16.Parse(Round((textura.Left - Yvalor) * EJESY)))

        fs.Position = offsetX + 26 'offset Ancho
        bw.Write(Int16.Parse(Round(textura.Width * EJESX)))
        fs.Position = offsetX + 32 'offset Ancho
        bw.Write(Int16.Parse(Round(textura.Width * EJESX)))
        fs.Position = offsetX + 16 'offset Alto
        bw.Write(Int16.Parse(Round(textura.Height * EJESY)))
        fs.Position = offsetX + 28 'offset Alto
        bw.Write(Int16.Parse(Round(textura.Height * EJESY)))

        'Mapeo scr
        fs.Position = offsetX + 38 'offset x
        bw.Write(Int16.Parse(mapeo.Left * 8))
        fs.Position = offsetX + 42 'offset
        bw.Write(Int16.Parse(mapeo.Left * 8))
        fs.Position = offsetX + 46 'offset
        bw.Write(Int16.Parse((mapeo.Width + mapeo.Left) * 8))
        fs.Position = offsetX + 50 'offset
        bw.Write(Int16.Parse((mapeo.Width + mapeo.Left) * 8))
        fs.Position = offsetX + 44 'offset 
        bw.Write(Int16.Parse(mapeo.Top * 8))
        fs.Position = offsetX + 52 'offset
        bw.Write(Int16.Parse(mapeo.Top * 8))
        fs.Position = offsetX + 40 'offset
        bw.Write(Int16.Parse((mapeo.Height + mapeo.Top) * 8))
        fs.Position = offsetX + 48 'offset
        bw.Write(Int16.Parse((mapeo.Height + mapeo.Top) * 8))
        fs.Close()
        bw.Close()
    End Sub
    Private Sub obj_Textura4Read(ByVal offsetX As Integer, ByVal textura As PictureBox, ByVal mapeo As PictureBox)
        'leer
        Dim fs As New FileStream(dlgAbrir.FileName & "_unzlib", FileMode.Open)
        Dim br As New BinaryReader(fs)
        'TEXTURA
        fs.Position = offsetX 'offset X (3A/58)
        textura.Left = (br.ReadInt16() / EJESX) + Xvalor
        fs.Position = offsetX + 2 'offset
        textura.Top = (br.ReadInt16() / EJESY) + Yvalor
        fs.Position = offsetX + 26 'offset
        textura.Width = (br.ReadInt16() / EJESX)
        fs.Position = offsetX + 16 'offset
        textura.Height = (br.ReadInt16() / EJESY)
        'Mapeo Textura
        fs.Position = offsetX + 38 'offset x
        mapeo.Left = (br.ReadInt16() / 8)
        fs.Position = offsetX + 46 'offset an
        mapeo.Width = (br.ReadInt16() / 8) - mapeo.Left
        fs.Position = offsetX + 44 'offset y, al
        mapeo.Top = (br.ReadInt16() / 8)
        fs.Position = offsetX + 40 'offset
        mapeo.Height = (br.ReadInt16() / 8) - mapeo.Top
        fs.Close()
        br.Close()

    End Sub


#End Region
   
#Region "funciones"

    Private Sub CargarTexturaA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim openDlg As New System.Windows.Forms.OpenFileDialog
        openDlg.Filter = "Archivos de Imagen |*.png; *.bmp" + "|All Files|*.*"
        If openDlg.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        If Not openDlg.FileName Is Nothing Then
            Dim img As New FileStream(openDlg.FileName, FileMode.Open)
            Mapeo.BackgroundImage = System.Drawing.Image.FromStream(img)
            img.Close()
        End If

        'Mover para actualizar mapeo XD(aumenta 1 y resta 1)
        Rojo.Left = Rojo.Left + 1
        Rojo.Left = Rojo.Left - 1
        Azul.Left = Azul.Left + 1
        Azul.Left = Azul.Left - 1
        Violeta.Left = Violeta.Left + 1
        Violeta.Left = Violeta.Left - 1
        Amarillo.Left = Amarillo.Left + 1
        Amarillo.Left = Amarillo.Left - 1
        Verde.Left = Verde.Left + 1
        Verde.Left = Verde.Left - 1


    End Sub
    'Importar
    Private Sub importar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles importar.Click
        'Cargar Archivo
        ' configurar el cuadro de diálogo por código 
        Me.dlgAbrir.Title = "Seleccionar archivo"
        Me.dlgAbrir.Filter = "Scoreboard file |*.opd; *.bin" + "|All Files|*.*"
        '' abrir el diálogo 
        If dlgAbrir.ShowDialog = Windows.Forms.DialogResult.OK Then
            Importarwe8(True)

        ElseIf MsgBox("No se ha seleccionado un archivo") Then
        End If
    End Sub
    'Exportar
    Private Sub Exportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Exportar.Click
        exportarwe8(True)

    End Sub


    'Carga de Textura para marcadores,tv,etc
    Private Sub ScrTvExtraEtcToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScrTvExtraEtcToolStripMenuItem.Click

        Panel1.Refresh()
        Dim openDlg As New System.Windows.Forms.OpenFileDialog
        openDlg.Filter = "Archivos de Imagen |*.png; *.bmp" + "|All Files|*.*"
        If openDlg.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        If Not openDlg.FileName Is Nothing Then
            Dim img As New FileStream(openDlg.FileName, FileMode.Open)
            Mapeo.BackgroundImage = System.Drawing.Image.FromStream(img)

            img.Close()
        End If

        'Mover para actualizar mapeo XD(aumenta 1 y resta 1)
        Rojo.Left = Rojo.Left + 1
        Rojo.Left = Rojo.Left - 1
        Azul.Left = Azul.Left + 1
        Azul.Left = Azul.Left - 1
        Violeta.Left = Violeta.Left + 1
        Violeta.Left = Violeta.Left - 1
        Amarillo.Left = Amarillo.Left + 1
        Amarillo.Left = Amarillo.Left - 1
        Verde.Left = Verde.Left + 1
        Verde.Left = Verde.Left - 1

        Try
            cropX = 1
            cropY = 249
            cropWidth = 272
            cropHeight = 14
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(Mapeo.BackgroundImage, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            AtaqueL.Image = cropBitmap
            AtaqueV.Image = cropBitmap
        Catch exc As Exception
        End Try
        Panel1.Refresh()
    End Sub
    'Seleccion de Textura game00
    Private Sub Game00ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Game00ToolStripMenuItem.Click
        Dim BG As New System.Windows.Forms.OpenFileDialog
        BG.Filter = "Archivos de Imagen |*.png; *.jpg; *.bmp" + "|All Files|*.*"
        If BG.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        If Not BG.FileName Is Nothing Then
            'TIEMPCUMPLIDO.Image = System.Drawing.Bitmap.FromFile(BG.FileName)
        End If
        Dim TE As Bitmap
        TE = System.Drawing.Bitmap.FromFile(BG.FileName)
        Try
            cropX = 9
            cropY = 376
            cropWidth = 64
            cropHeight = 20
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(TE, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            AAL.Image = cropBitmap
        Catch exc As Exception
        End Try
        Try
            cropX = 9
            cropY = 349
            cropWidth = 64
            cropHeight = 20
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(TE, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            ABL.Image = cropBitmap
        Catch exc As Exception
        End Try
        Try
            cropX = 8
            cropY = 320
            cropWidth = 64
            cropHeight = 20
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(TE, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            ACL.Image = cropBitmap
        Catch exc As Exception
        End Try
        Try
            cropX = 8
            cropY = 291
            cropWidth = 64
            cropHeight = 20
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(TE, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            ADL.Image = cropBitmap
        Catch exc As Exception
        End Try
        Try
            cropX = 8
            cropY = 263
            cropWidth = 64
            cropHeight = 20
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(TE, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            AEL.Image = cropBitmap
        Catch exc As Exception
        End Try
        Try
            cropX = 9
            cropY = 376
            cropWidth = 64
            cropHeight = 20
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(TE, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            AAV.Image = cropBitmap
        Catch exc As Exception
        End Try
        Try
            cropX = 9
            cropY = 349
            cropWidth = 64
            cropHeight = 20
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(TE, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            ABV.Image = cropBitmap
        Catch exc As Exception
        End Try
        Try
            cropX = 8
            cropY = 320
            cropWidth = 64
            cropHeight = 20
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(TE, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            ACV.Image = cropBitmap
        Catch exc As Exception
        End Try
        Try
            cropX = 8
            cropY = 291
            cropWidth = 64
            cropHeight = 20
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(TE, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            ADV.Image = cropBitmap
        Catch exc As Exception
        End Try
        Try
            cropX = 8
            cropY = 263
            cropWidth = 64
            cropHeight = 20
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(TE, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            AEV.Image = cropBitmap
        Catch exc As Exception
        End Try
    End Sub
    'Seleccion de Textura game01
    Private Sub Game01Textura2ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Game01Textura2ToolStripMenuItem.Click
        Dim BG As New System.Windows.Forms.OpenFileDialog
        BG.Filter = "Archivos de Imagen |*.png; *.jpg; *.bmp" + "|All Files|*.*"
        If BG.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        If Not BG.FileName Is Nothing Then
            'TIEMPCUMPLIDO.Image = System.Drawing.Bitmap.FromFile(BG.FileName)
        End If
        Dim TE As Bitmap
        TE = System.Drawing.Bitmap.FromFile(BG.FileName)
        Try
            cropX = 32
            cropY = 311
            cropWidth = 203
            cropHeight = 38
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(TE, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            TIEMPCUMPLIDO.Image = cropBitmap
        Catch exc As Exception
        End Try
        Try
            cropX = 173
            cropY = 354
            cropWidth = 21
            cropHeight = 28
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(TE, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            ScoreL.Image = cropBitmap
            ScoreV.Image = cropBitmap
        Catch exc As Exception
        End Try
        Try
            cropX = 173
            cropY = 354
            cropWidth = 40
            cropHeight = 27
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(TE, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            Minutos.Image = cropBitmap
            Segundos.Image = cropBitmap
        Catch exc As Exception
        End Try
        Try
            cropX = 238
            cropY = 272
            cropWidth = 82
            cropHeight = 38
            Cursor = Cursors.Default
            If cropWidth < 1 Then
                Exit Sub
            End If
            Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim bit As Bitmap = New Bitmap(TE, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(cropWidth, cropHeight)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            half.Image = cropBitmap
        Catch exc As Exception
        End Try

    End Sub


#End Region
#Region "Opciones"

    'Idiomas
    Private Sub EspañolToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EspañolToolStripMenuItem.Click
        EspañolToolStripMenuItem.CheckState = CheckState.Checked
        EnglishToolStripMenuItem.CheckState = CheckState.Unchecked
        Me.OpcionesToolStripMenuItem.Text = "Opciones"
        Me.IdiomaToolStripMenuItem1.Text = "Idioma"
        Me.ResoluciónToolStripMenuItem.Text = "Resolución"
        Me.TabPage8.Text = "Marcador"
        Me.GroupBox3.Text = "Goles"
        Me.ScoreVT.Text = "Tamaño"
        Me.ScoreVC.Text = "Color"
        Me.ScoreLT.Text = "Tamaño"
        Me.Label20.Text = "Visitante:"
        Me.ScoreLC.Text = "Color"
        Me.Label29.Text = "Local:"
        Me.Label30.Text = "Alto:"
        Me.Label31.Text = "X:"
        Me.Label32.Text = "Ancho:"
        Me.GroupBox4.Text = "Textura"
        Me.Label42.Text = "Extra:"
        Me.Label41.Text = "TV Logo:"
        Me.Label40.Text = "Marcador:"
        Me.Label16.Text = "Alto:"
        Me.Label27.Text = "Ancho:"
        Me.GroupBox2.Text = "Tiempo"
        Me.TIEMPCUMPLIDOT.Text = "Tamaño"
        Me.TIEMPCUMPLIDOC.Text = "Color"
        Me.Label25.Text = "T. Extra:"
        Me.HalfT.Text = "Tamaño"
        Me.HalfC.Text = "Color"
        Me.Label38.Text = "1er, 2do:"
        Me.SegundosT.Text = "Tamaño"
        Me.SegundosC.Text = "Color"
        Me.MinutosT.Text = "Tamaño"
        Me.Label4.Text = "Segundos:"
        Me.MinutosC.Text = "Color"
        Me.Label6.Text = "Minutos:"
        Me.Label9.Text = "Alto:"
        Me.Label11.Text = "Ancho:"
        Me.GroupBox1.Text = "Escudos y Banderas"
        Me.Label17.Text = "Local:"
        Me.Label18.Text = "Visita:"
        Me.Label14.Text = "Ancho:"
        Me.Label8.Text = "Alto:"
        Me.TabPage2.Text = "Marcador"
        Me.TabPage1.Text = "Nombre Placa"
        Me.GroupBox5.Text = "Local"
        Me.Label1.Text = "Potencia:"
        Me.Label2.Text = "Ataque:"
        Me.CheckBox2.Text = "Invertir"
        Me.Label3.Text = "Textura:"
        Me.Label5.Text = "Resistencia:"
        Me.Label22.Text = "Posición:"
        Me.Label24.Text = "Nombre:"
        Me.Label35.Text = "Alto:"
        Me.Label44.Text = "Ancho:"
        Me.GroupBox8.Text = "Local"
        Me.Label43.Text = "Potencia:"
        Me.Label37.Text = "Ataque:"
        Me.CheckBox1.Text = "Invertir"
        Me.Label34.Text = "Textura:"
        Me.Label23.Text = "Resistencia:"
        Me.Label19.Text = "Posición:"
        Me.Label57.Text = "Nombre:"
        Me.Label51.Text = "Alto:"
        Me.Label53.Text = "Ancho:"
        Me.TabPage26.Text = "Mapeo de Textura:"
        Me.GroupBox7.Text = "Barra de Potencia WE9 y PES5"
        Me.GroupBox6.Text = "Mapeo de Textura"
        Me.Label36.Text = "Intercambiar Mapeo"
        Me.Label140.Text = "Nombre Visita"
        Me.Label224.Text = "TV Logo"
        Me.Label225.Text = "Extra"
        Me.Label226.Text = "Nombre Local"
        Me.Label233.Text = "Ancho:"
        Me.Label230.Text = "Marcador"
        Me.Label231.Text = "Alto:"
        Me.Button1.Text = "Color"
        Me.archivo.Text = "Archivo"
        Me.ToolStripMenuItem1.Text = "Archivo"
        Me.importar.Text = "Abrir"
        Me.Exportar.Text = "Guardar"
        
        Me.TexturasToolStripMenuItem.Text = "Texturas (.png)"
        Me.ScrTvExtraEtcToolStripMenuItem.Text = "Scr, Tv, Extra, Nombre Placa"
        Me.Game00ToolStripMenuItem.Text = "game_00 (Text. 1 - game_2d_x) 512x512"
        Me.Game01Textura2ToolStripMenuItem.Text = "game_01 (Text. 2 - game_2d_x) 512x512"
        Me.about.Text = "Acerca De..."
        Me.FondoToolStripMenuItem.Text = "Fondo"
        Me.CargarToolStripMenuItem.Text = "Cargar"
        Me.RestaurarToolStripMenuItem.Text = "Restaurar"
        Me.modalidad.Text = "Modalidad Textura"
        Me.Label46.Text = "Mapeo"
    End Sub
    Private Sub EnglishToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnglishToolStripMenuItem.Click
        EspañolToolStripMenuItem.CheckState = CheckState.Unchecked
        EnglishToolStripMenuItem.CheckState = CheckState.Checked
        Me.OpcionesToolStripMenuItem.Text = "Options"
        Me.IdiomaToolStripMenuItem1.Text = "Language"
        Me.ResoluciónToolStripMenuItem.Text = "Resolution"
        Me.TabPage8.Text = "Scoreboard"
        Me.GroupBox3.Text = "Goals"
        Me.ScoreVT.Text = "Size"
        Me.ScoreVC.Text = "Colors"
        Me.ScoreLT.Text = "Size"
        Me.Label20.Text = "Away:"
        Me.ScoreLC.Text = "Colors"
        Me.Label29.Text = "Home:"
        Me.Label30.Text = "Height:"
        Me.Label32.Text = "Width:"
        Me.GroupBox4.Text = "Texture"
        Me.Label42.Text = "Extra:"
        Me.Label41.Text = "TV Logo:"
        Me.Label40.Text = "Scoreboard:"
        Me.Label16.Text = "Height:"
        Me.Label27.Text = "Width:"
        Me.GroupBox2.Text = "Time"
        Me.TIEMPCUMPLIDOT.Text = "Size"
        Me.TIEMPCUMPLIDOC.Text = "Colors"
        Me.Label25.Text = "Extra Time:"
        Me.HalfT.Text = "Size"
        Me.HalfC.Text = "Colors"
        Me.Label38.Text = "1st, 2nd:"
        Me.SegundosT.Text = "Size"
        Me.SegundosC.Text = "Colors"
        Me.MinutosT.Text = "Size"
        Me.Label4.Text = "Seconds:"
        Me.MinutosC.Text = "Colors"
        Me.Label6.Text = "Minutes:"
        Me.Label9.Text = "Height:"
        Me.Label11.Text = "Width:"
        Me.GroupBox1.Text = "Flag: Club and National Team"
        Me.Label17.Text = "Home:"
        Me.Label18.Text = "Away:"
        Me.Label14.Text = "Width:"
        Me.Label8.Text = "Height:"
        Me.TabPage1.Text = "Name Plate"
        Me.TabPage2.Text = "Scoreboard"
        Me.GroupBox5.Text = "Home"
        Me.Label1.Text = "Shoot Bar:"
        Me.Label2.Text = "Attack:"
        Me.CheckBox2.Text = "Invert"
        Me.Label3.Text = "Texture:"
        Me.Label5.Text = "Stamina:"
        Me.Label22.Text = "Position:"
        Me.Label24.Text = "Name:"
        Me.Label35.Text = "Height:"
        Me.Label44.Text = "Width:"
        Me.GroupBox8.Text = "Home"
        Me.Label43.Text = "Shoot Bar:"
        Me.Label37.Text = "Attack:"
        Me.CheckBox1.Text = "Invert"
        Me.Label34.Text = "Texture:"
        Me.Label23.Text = "Stamina:"
        Me.Label19.Text = "Position:"
        Me.Label57.Text = "Name:"
        Me.Label51.Text = "Height:"
        Me.Label53.Text = "Width:"
        Me.TabPage26.Text = "Map Texture:"
        Me.GroupBox7.Text = "Shoot Bar WE9 y PES5"
        Me.GroupBox6.Text = "Map Texture"
        Me.Label36.Text = "Switch Map"
        Me.Label140.Text = "Name Away"
        Me.Label224.Text = "TV Logo"
        Me.Label225.Text = "Extra"
        Me.Label226.Text = "Name Home"
        Me.Label233.Text = "Width:"
        Me.Label230.Text = "Scoreboard"
        Me.Label231.Text = "Height:"
        Me.Button1.Text = "Colors"
        Me.archivo.Text = "File"
        Me.ToolStripMenuItem1.Text = "File"
        Me.importar.Text = "Open"
        Me.Exportar.Text = "Save"
        Me.TexturasToolStripMenuItem.Text = "Textures (.png)"
        Me.ScrTvExtraEtcToolStripMenuItem.Text = "Scr, Tv, Extra, Name Plate"
        Me.Game00ToolStripMenuItem.Text = "game_00 (Text. 1 - game_2d_x) 512x512"
        Me.Game01Textura2ToolStripMenuItem.Text = "game_01 (Text. 1 - game_2d_x) 512x512"
        Me.about.Text = "About..."
        Me.FondoToolStripMenuItem.Text = "Background"
        Me.CargarToolStripMenuItem.Text = "Load"
        Me.RestaurarToolStripMenuItem.Text = "Restore"
        Me.modalidad.Text = "Slot Texture"
        Me.Label46.Text = "Map"
    End Sub
    Private Sub English(ByVal English As Boolean)
        Me.TabPage8.Text = "Scoreboard"
        Me.GroupBox3.Text = "Goals"
        Me.ScoreVT.Text = "Size"
        Me.ScoreVC.Text = "Colors"
        Me.ScoreLT.Text = "Size"
        Me.Label20.Text = "Away:"
        Me.ScoreLC.Text = "Colors"
        Me.Label29.Text = "Home:"
        Me.Label30.Text = "Height:"
        Me.Label31.Text = "X:"
        Me.Label32.Text = "Width:"
        Me.GroupBox4.Text = "Texture"
        Me.Label42.Text = "Extra:"
        Me.Label41.Text = "TV Logo:"
        Me.Label40.Text = "Scoreboard:"
        Me.Label16.Text = "Height:"
        Me.Label27.Text = "Width:"
        Me.GroupBox2.Text = "Time"
        Me.TIEMPCUMPLIDOT.Text = "Size"
        Me.TIEMPCUMPLIDOC.Text = "Colors"
        Me.Label25.Text = "Extra Time:"
        Me.HalfT.Text = "Size"
        Me.HalfC.Text = "Colors"
        Me.Label38.Text = "1st, 2nd:"
        Me.SegundosT.Text = "Size"
        Me.SegundosC.Text = "Colors"
        Me.MinutosT.Text = "Size"
        Me.Label4.Text = "Seconds:"
        Me.MinutosC.Text = "Colors"
        Me.Label6.Text = "Minutes:"
        Me.Label9.Text = "Height:"
        Me.Label11.Text = "Width:"
        Me.GroupBox1.Text = "Flag: Club and National Team"
        Me.Label17.Text = "Home:"
        Me.Label18.Text = "Away:"
        Me.Label14.Text = "Width:"
        Me.Label8.Text = "Height:"
        Me.TabPage1.Text = "Name Plate"
        Me.GroupBox5.Text = "Home"
        Me.Label1.Text = "Shoot Bar:"
        Me.Label2.Text = "Attack:"
        Me.CheckBox2.Text = "Invert"
        Me.Label3.Text = "Texture:"
        Me.Label5.Text = "Stamina:"
        Me.Label22.Text = "Position:"
        Me.Label24.Text = "Name:"
        Me.Label35.Text = "Height:"
        Me.Label44.Text = "Width:"
        Me.GroupBox8.Text = "Home"
        Me.Label43.Text = "Shoot Bar:"
        Me.Label37.Text = "Attack:"
        Me.CheckBox1.Text = "Invert"
        Me.Label34.Text = "Texture:"
        Me.Label23.Text = "Stamina:"
        Me.Label19.Text = "Position:"
        Me.Label57.Text = "Name:"
        Me.Label51.Text = "Height:"
        Me.Label53.Text = "Width:"
        Me.TabPage26.Text = "Map Texture:"
        Me.GroupBox7.Text = "Shoot Bar WE9 y PES5"
        Me.GroupBox6.Text = "Map Texture"
        Me.Label36.Text = "Switch Map"
        Me.Label140.Text = "Name Away"
        Me.Label224.Text = "TV Logo"
        Me.Label225.Text = "Extra"
        Me.Label226.Text = "Name Home"
        Me.Label233.Text = "Width:"
        Me.Label230.Text = "Scoreboard"
        Me.Label231.Text = "Height:"
        Me.Button1.Text = "Colors"
        Me.archivo.Text = "File"
        Me.ToolStripMenuItem1.Text = "File"
        Me.importar.Text = "Open"
        Me.Exportar.Text = "Save"
        Me.TexturasToolStripMenuItem.Text = "Textures (.png)"
        Me.ScrTvExtraEtcToolStripMenuItem.Text = "Scr, Tv, Extra, Name Plate"
        Me.Game00ToolStripMenuItem.Text = "game_00 (Texture 1 -151) 512x512"
        Me.Game01Textura2ToolStripMenuItem.Text = "game_01 (Texture 2 -151) 512x512"
        Me.about.Text = "About..."
        Me.FondoToolStripMenuItem.Text = "Background"
        Me.CargarToolStripMenuItem.Text = "Load"
        Me.RestaurarToolStripMenuItem.Text = "Restore"
        Me.modalidad.Text = "Slot Texture"
        Me.Label46.Text = "Map"
    End Sub
    Private Sub Español(ByVal Español As Boolean)
        Me.TabPage8.Text = "Marcador"
        Me.GroupBox3.Text = "Goles"
        Me.ScoreVT.Text = "Tamaño"
        Me.ScoreVC.Text = "Color"
        Me.ScoreLT.Text = "Tamaño"
        Me.Label20.Text = "Visitante:"
        Me.ScoreLC.Text = "Color"
        Me.Label29.Text = "Local:"
        Me.Label30.Text = "Alto:"
        Me.Label31.Text = "X:"
        Me.Label32.Text = "Ancho:"
        Me.GroupBox4.Text = "Textura"
        Me.Label42.Text = "Extra:"
        Me.Label41.Text = "TV Logo:"
        Me.Label40.Text = "Marcador:"
        Me.Label16.Text = "Alto:"
        Me.Label27.Text = "Ancho:"
        Me.GroupBox2.Text = "Tiempo"
        Me.TIEMPCUMPLIDOT.Text = "Tamaño"
        Me.TIEMPCUMPLIDOC.Text = "Color"
        Me.Label25.Text = "T. Extra:"
        Me.HalfT.Text = "Tamaño"
        Me.HalfC.Text = "Color"
        Me.Label38.Text = "1er, 2do:"
        Me.SegundosT.Text = "Tamaño"
        Me.SegundosC.Text = "Color"
        Me.MinutosT.Text = "Tamaño"
        Me.Label4.Text = "Segundos:"
        Me.MinutosC.Text = "Color"
        Me.Label6.Text = "Minutos:"
        Me.Label9.Text = "Alto:"
        Me.Label11.Text = "Ancho:"
        Me.GroupBox1.Text = "Escudos y Banderas"
        Me.Label17.Text = "Local:"
        Me.Label18.Text = "Visita:"
        Me.Label14.Text = "Ancho:"
        Me.Label8.Text = "Alto:"
        Me.TabPage1.Text = "Nombre Placa"
        Me.GroupBox5.Text = "Local"
        Me.Label1.Text = "Potencia:"
        Me.Label2.Text = "Ataque:"
        Me.CheckBox2.Text = "Invertir"
        Me.Label3.Text = "Textura:"
        Me.Label5.Text = "Resistencia:"
        Me.Label22.Text = "Posición:"
        Me.Label24.Text = "Nombre:"
        Me.Label35.Text = "Alto:"
        Me.Label44.Text = "Ancho:"
        Me.GroupBox8.Text = "Local"
        Me.Label43.Text = "Potencia:"
        Me.Label37.Text = "Ataque:"
        Me.CheckBox1.Text = "Invertir"
        Me.Label34.Text = "Textura:"
        Me.Label23.Text = "Resistencia:"
        Me.Label19.Text = "Posición:"
        Me.Label57.Text = "Nombre:"
        Me.Label51.Text = "Alto:"
        Me.Label53.Text = "Ancho:"
        Me.TabPage26.Text = "Mapeo de Textura:"
        Me.GroupBox7.Text = "Barra de Potencia WE9 y PES5"
        Me.GroupBox6.Text = "Mapeo de Textura"
        Me.Label36.Text = "Intercambiar Mapeo"
        Me.Label140.Text = "Nombre Visita"
        Me.Label224.Text = "TV Logo"
        Me.Label225.Text = "Extra"
        Me.Label226.Text = "Nombre Local"
        Me.Label233.Text = "Ancho:"
        Me.Label230.Text = "Marcador"
        Me.Label231.Text = "Alto:"
        Me.Button1.Text = "Color"
        Me.archivo.Text = "Archivo"
        Me.ToolStripMenuItem1.Text = "Archivo"
        Me.importar.Text = "Abrir"
        Me.Exportar.Text = "Guardar"
        Me.TexturasToolStripMenuItem.Text = "Texturas (.png)"
        Me.ScrTvExtraEtcToolStripMenuItem.Text = "Scr, Tv, Extra, Nombre Placa"
        Me.Game00ToolStripMenuItem.Text = "game_00 (Textura 1 -151) 512x512"
        Me.Game01Textura2ToolStripMenuItem.Text = "game_01 (Textura 2 -151) 512x512"
        Me.about.Text = "Acerca De..."
        Me.FondoToolStripMenuItem.Text = "Fondo"
        Me.CargarToolStripMenuItem.Text = "Cargar"
        Me.RestaurarToolStripMenuItem.Text = "Restaurar"
        Me.modalidad.Text = "Modalidad Textura"
        Me.Label46.Text = "Mapeo"
    End Sub

    'Resolucion
    Private Sub Resolucionbaja(ByVal Resolucionbaja As Boolean)
        If Background Is Nothing Then Panel1.BackgroundImage = My.Resources._640BG
        Me.AAL.MaximumSize = New Size(34, 6)
        Me.AAL.MinimumSize = New Size(34, 6)
        Me.ABL.MaximumSize = New Size(34, 6)
        Me.ABL.MinimumSize = New Size(34, 6)
        Me.ACL.MaximumSize = New Size(34, 6)
        Me.ACL.MinimumSize = New Size(34, 6)
        Me.ADL.MaximumSize = New Size(34, 6)
        Me.ADL.MinimumSize = New Size(34, 6)
        Me.AEL.MaximumSize = New Size(34, 6)
        Me.AEL.MinimumSize = New Size(34, 6)
        Me.AAV.MaximumSize = New Size(34, 6)
        Me.AAV.MinimumSize = New Size(34, 6)
        Me.ABV.MaximumSize = New Size(34, 6)
        Me.ABV.MinimumSize = New Size(34, 6)
        Me.ACV.MaximumSize = New Size(34, 6)
        Me.ACV.MinimumSize = New Size(34, 6)
        Me.ADV.MaximumSize = New Size(34, 6)
        Me.ADV.MinimumSize = New Size(34, 6)
        Me.AEV.MaximumSize = New Size(34, 6)
        Me.AEV.MinimumSize = New Size(34, 6)
        Panel1.Height = "480"
        Panel1.Width = "640"
        TabControl1.Left = "646"
        TabControl3.Width = "990"
        TabControl3.Height = "538"
        LinkLabel1.Top = 491
        LinkLabel2.Top = 491
        LinkLabel1.Left = 64
        LinkLabel2.Left = 404
        For Each c As Control In Me.Panel1.Controls
            'si el control es de tipo Panel modifico su visibilidad segun el parametro
            If TypeOf c Is PictureBox Then
                c.Top = c.Top / 1.25
                c.Left = c.Left / 1.6
                c.Height = c.Height / 1.25
                c.Width = c.Width / 1.6
            End If
        Next
    End Sub
    Private Sub Resolucionalta(ByVal Resolucionalta As Boolean)
        If Background Is Nothing Then Panel1.BackgroundImage = My.Resources._1024BG
        Me.AAL.MaximumSize = New Size(54, 6)
        Me.AAL.MinimumSize = New Size(54, 6)
        Me.ABL.MaximumSize = New Size(54, 6)
        Me.ABL.MinimumSize = New Size(54, 6)
        Me.ACL.MaximumSize = New Size(54, 6)
        Me.ACL.MinimumSize = New Size(54, 6)
        Me.ADL.MaximumSize = New Size(54, 6)
        Me.ADL.MinimumSize = New Size(54, 6)
        Me.AEL.MaximumSize = New Size(54, 6)
        Me.AEL.MinimumSize = New Size(54, 6)
        Me.AAV.MaximumSize = New Size(54, 6)
        Me.AAV.MinimumSize = New Size(54, 6)
        Me.ABV.MaximumSize = New Size(54, 6)
        Me.ABV.MinimumSize = New Size(54, 6)
        Me.ACV.MaximumSize = New Size(54, 6)
        Me.ACV.MinimumSize = New Size(54, 6)
        Me.ADV.MaximumSize = New Size(54, 6)
        Me.ADV.MinimumSize = New Size(54, 6)
        Me.AEV.MaximumSize = New Size(54, 6)
        Me.AEV.MinimumSize = New Size(54, 6)
        Panel1.Height = "600"
        Panel1.Width = "1024"
        TabControl1.Left = "1030"
        TabControl3.Width = "1370"
        TabControl3.Height = "658"
        LinkLabel1.Top = 610
        LinkLabel2.Top = 610
        LinkLabel1.Left = 64
        LinkLabel2.Left = 804
        For Each c As Control In Me.Panel1.Controls
            'si el control es de tipo Panel modifico su visibilidad segun el parametro
            If TypeOf c Is PictureBox Then
                c.Top = 1.25 * c.Top
                c.Left = 1.6 * c.Left
                c.Height = 1.25 * c.Height
                c.Width = 1.6 * c.Width
            End If
        Next
    End Sub

    Private Sub ResBaja_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResBaja.Click
        Select Case ResBaja.CheckState
            Case CheckState.Checked
                MsgBox("Ya esta Seleccionada esta opción")
            Case CheckState.Unchecked
                ResBaja.CheckState = CheckState.Checked
                ResAlta.CheckState = CheckState.Unchecked
                Resolucionbaja(True)
        End Select




    End Sub
    Private Sub ResAlta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResAlta.Click
        Select Case ResAlta.CheckState
            Case CheckState.Checked
                MsgBox("Ya esta Seleccionada esta opción")
            Case CheckState.Unchecked
                ResBaja.CheckState = CheckState.Unchecked
                ResAlta.CheckState = CheckState.Checked
                Resolucionalta(True)
        End Select
    End Sub



#End Region

End Class