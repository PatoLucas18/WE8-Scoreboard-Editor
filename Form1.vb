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
    Dim OFFSET, EJESX, EJESY, Identificador As Integer

    Public Background As Bitmap
    Dim Scr_CB, Tv_CB, EXTRA_CB, PlayerTEXTL_CB, PlayerTEXTV_CB As Bitmap

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
        Select Case My.Settings.ResolucionAlta
            Case 0
                ResBaja.CheckState = CheckState.Checked
                ResMedia.CheckState = CheckState.Unchecked
                ResAlta.CheckState = CheckState.Unchecked
                Resolucionbaja()
            Case 1
                ResBaja.CheckState = CheckState.Unchecked
                ResMedia.CheckState = CheckState.Checked
                ResAlta.CheckState = CheckState.Unchecked
                Resolucionmedia()
            Case 2
                ResBaja.CheckState = CheckState.Unchecked
                ResMedia.CheckState = CheckState.Unchecked
                ResAlta.CheckState = CheckState.Checked
                Resolucionalta()
        End Select


        'Cargar Idioma
        EnglishToolStripMenuItem.CheckState = My.Settings.EN
        If EnglishToolStripMenuItem.CheckState = CheckState.Checked Then
            EspañolToolStripMenuItem.CheckState = CheckState.Unchecked
            English()
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
        If ResBaja.CheckState = CheckState.Checked Then My.Settings.ResolucionAlta = 0
        If ResMedia.CheckState = CheckState.Checked Then My.Settings.ResolucionAlta = 1
        If ResAlta.CheckState = CheckState.Checked Then My.Settings.ResolucionAlta = 2


        My.Settings.EN = EnglishToolStripMenuItem.CheckState
        My.Settings.Save()

        Try
            My.Computer.FileSystem.DeleteFile("unzlib")
        Catch ex As Exception

        End Try
    End Sub

    
#Region "MAPEO"

    Private Function crop(ByVal textura As PictureBox, ByVal cropX As NumericUpDown, ByVal cropY As NumericUpDown, ByVal cropWidth As NumericUpDown, ByVal cropHeight As NumericUpDown) As Bitmap
        Try
            Dim rect As New Rectangle(textura.Left, textura.Height, textura.Width, textura.Height)
            Dim cropped As Bitmap = Mapeo.BackgroundImage.Clone(rect, Mapeo.BackgroundImage.PixelFormat)
            cropX.Value = textura.Left
            cropY.Value = textura.Top
            cropWidth.Value = textura.Width
            cropHeight.Value = textura.Height

            Return cropped
        Catch ex As Exception

        End Try
    End Function

    Public Function mapeoSize(ByVal textura As PictureBox, ByVal X As NumericUpDown, ByVal Y As NumericUpDown, ByVal W As NumericUpDown, ByVal H As NumericUpDown) As Bitmap
        Dim texturaBm As Bitmap
        textura.BringToFront()
        Try

            Cursor = Cursors.Default
            If textura.Width < 1 Then
                Exit Function
            End If
            Dim rect As Rectangle = New Rectangle(textura.Left, textura.Top, textura.Width, textura.Height)
            Dim bit As Bitmap = New Bitmap(Mapeo.BackgroundImage, Mapeo.Width, Mapeo.Height)
            cropBitmap = New Bitmap(textura.Width, textura.Height)
            Dim g As Graphics = Graphics.FromImage(cropBitmap)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)
            texturaBm = Nothing
            texturaBm = cropBitmap
        Catch exc As Exception

        End Try

        X.Value = textura.Left
        Y.Value = textura.Top
        W.Value = textura.Width
        H.Value = textura.Height
        Return texturaBm
    End Function


    Private Sub Rojo_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rojo.Resize, Rojo.Move
        Scr_CB = mapeoSize(Rojo, RojoX, RojoY, RojoAN, RojoAL)
        Exit Sub
    End Sub
    Private Sub Rojo_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RojoY.ValueChanged, RojoX.ValueChanged, RojoAN.ValueChanged, RojoAL.ValueChanged
        enlazar(Rojo, RojoX, RojoY, RojoAN, RojoAL)
    End Sub
    Private Sub Azul_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Azul.Resize, Azul.Move
        PlayerTEXTL_CB = mapeoSize(Azul, AzulX, AzulY, AzulAN, AzulAL)
        Exit Sub
    End Sub
    Private Sub Azul_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AzulY.ValueChanged, AzulX.ValueChanged, AzulAN.ValueChanged, AzulAL.ValueChanged
        enlazar(Azul, AzulX, AzulY, AzulAN, AzulAL)
    End Sub
    Private Sub Amarillo_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Amarillo.Resize, Amarillo.Move
        EXTRA_CB = mapeoSize(Amarillo, AmarilloX, AmarilloY, AmarilloAN, AmarilloAL)
        Exit Sub
    End Sub
    Private Sub Amarillo_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AmarilloY.ValueChanged, AmarilloX.ValueChanged, AmarilloAN.ValueChanged, AmarilloAL.ValueChanged
        enlazar(Amarillo, AmarilloX, AmarilloY, AmarilloAN, AmarilloAL)
    End Sub
    Private Sub Violeta_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Violeta.Resize, Violeta.Move
        Tv_CB = mapeoSize(Violeta, VioletaX, VioletaY, VioletaAN, VioletaAL)
        Exit Sub
    End Sub
    Private Sub Violeta_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VioletaY.ValueChanged, VioletaX.ValueChanged, VioletaAN.ValueChanged, VioletaAL.ValueChanged
        enlazar(Violeta, VioletaX, VioletaY, VioletaAN, VioletaAL)
    End Sub
    Private Sub Verde_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Verde.Resize, Verde.Move
        PlayerTEXTV_CB = mapeoSize(Verde, VerdeX, VerdeY, VerdeAN, VerdeAL)
        Exit Sub
    End Sub
    Private Sub Verde_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VerdeY.ValueChanged, VerdeX.ValueChanged, VerdeAN.ValueChanged, VerdeAL.ValueChanged
        enlazar(Verde, VerdeX, VerdeY, VerdeAN, VerdeAL)
    End Sub
#End Region
#Region "Marcador"
    'Escudo Local

    Private Sub ScoreLX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreLX.ValueChanged
        ScoreL.Left = ScoreLX.Value
    End Sub
    Private Sub ScoreLY_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreLY.ValueChanged
        ScoreL.Top = ScoreLY.Value
    End Sub
    Private Sub ScoreLAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreLAN.ValueChanged
        ScoreL.Width = ScoreLAN.Value
    End Sub
    Private Sub ScoreLAL_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreLAL.ValueChanged
        ScoreL.Height = ScoreLAL.Value
    End Sub
    Private Sub ScoreVX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreVX.ValueChanged
        ScoreV.Left = ScoreVX.Value
    End Sub
    Private Sub ScoreVY_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreVY.ValueChanged
        ScoreV.Top = ScoreVY.Value
    End Sub
    Private Sub ScoreVAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreVAN.ValueChanged
        ScoreV.Width = ScoreVAN.Value
    End Sub
    Private Sub ScoreVAL_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreVAL.ValueChanged
        ScoreV.Height = ScoreVAL.Value
    End Sub
    Private Sub ScrX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScrY.ValueChanged, ScrX.ValueChanged
        Scr.Left = ScrX.Value
        Scr.Top = ScrY.Value
    End Sub
    Private Sub ScrAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScrAN.ValueChanged, ScrAL.ValueChanged
        Scr.Width = ScrAN.Value
        Scr.Height = ScrAL.Value
    End Sub

    Private Sub BanderaLX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaLY.ValueChanged, BanderaLX.ValueChanged
        BanderaL.Left = BanderaLX.Value
        BanderaL.Top = BanderaLY.Value
    End Sub
    Private Sub BanderaLAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaLAN.ValueChanged, BanderaLAL.ValueChanged
        BanderaL.Width = BanderaLAN.Value
        BanderaL.Height = BanderaLAL.Value
    End Sub
    Private Sub BanderaVX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaVY.ValueChanged, BanderaVX.ValueChanged
        BanderaV.Left = BanderaVX.Value
        BanderaV.Top = BanderaVY.Value
    End Sub
    Private Sub BanderaVAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaVAN.ValueChanged, BanderaVAL.ValueChanged
        BanderaV.Width = BanderaVAN.Value
        BanderaV.Height = BanderaVAL.Value
    End Sub
    Private Sub MinutosX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MinutosY.ValueChanged, MinutosX.ValueChanged
        Minutos.Left = MinutosX.Value
        Minutos.Top = MinutosY.Value
    End Sub
    Private Sub MinutosAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MinutosAN.ValueChanged, MinutosAL.ValueChanged
        Minutos.Width = MinutosAN.Value
        Minutos.Height = MinutosAL.Value
    End Sub
    Private Sub SegundosX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SegundosY.ValueChanged, SegundosX.ValueChanged
        Segundos.Left = SegundosX.Value
        Segundos.Top = SegundosY.Value
    End Sub
    Private Sub SegundosAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SegundosAN.ValueChanged, SegundosAL.ValueChanged
        Segundos.Width = SegundosAN.Value
        Segundos.Height = SegundosAL.Value
    End Sub
    Private Sub HalfX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HalfY.ValueChanged, HalfX.ValueChanged
        half.Left = HalfX.Value
        half.Top = HalfY.Value
    End Sub
    Private Sub HalfAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HalfAN.ValueChanged, HalfAL.ValueChanged
        half.Width = HalfAN.Value
        half.Height = HalfAL.Value
    End Sub

    Private Sub ScoreL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreL.Move
        ScoreLX.Value = ScoreL.Left
        ScoreLY.Value = ScoreL.Top
    End Sub
    Private Sub ScoreL_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreL.Resize
        ScoreLAL.Value = ScoreL.Height
        ScoreLAN.Value = ScoreL.Width
    End Sub
    Private Sub ScoreV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreV.Move
        ScoreVX.Value = ScoreV.Left
        ScoreVY.Value = ScoreV.Top
    End Sub
    Private Sub ScoreV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScoreV.Resize
        ScoreVAL.Value = ScoreV.Height
        ScoreVAN.Value = ScoreV.Width
    End Sub
    Private Sub Src_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Scr.Move
        ScrX.Value = Scr.Left
        ScrY.Value = Scr.Top
    End Sub
    Private Sub Src_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Scr.Resize
        ScrAL.Value = Scr.Height
        ScrAN.Value = Scr.Width
    End Sub

    Private Sub BanderaL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaL.Move
        BanderaLX.Value = BanderaL.Left
        BanderaLY.Value = BanderaL.Top
    End Sub
    Private Sub BanderaL_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaL.Resize
        BanderaLAL.Value = BanderaL.Height
        BanderaLAN.Value = BanderaL.Width
    End Sub
    Private Sub BanderaV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaV.Move
        BanderaVX.Value = BanderaV.Left
        BanderaVY.Value = BanderaV.Top
    End Sub
    Private Sub BanderaV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BanderaV.Resize
        BanderaVAL.Value = BanderaV.Height
        BanderaVAN.Value = BanderaV.Width
    End Sub
    Private Sub Minutos_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Minutos.Move
        MinutosX.Value = Minutos.Left
        MinutosY.Value = Minutos.Top
    End Sub
    Private Sub Minutos_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Minutos.Resize
        MinutosAL.Value = Minutos.Height
        MinutosAN.Value = Minutos.Width
    End Sub
    Private Sub Segundos_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Segundos.Move
        SegundosX.Value = Segundos.Left
        SegundosY.Value = Segundos.Top
    End Sub
    Private Sub Segundos_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Segundos.Resize
        SegundosAL.Value = Segundos.Height
        SegundosAN.Value = Segundos.Width
    End Sub
    Private Sub Half_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles half.Move
        HalfX.Value = half.Left
        HalfY.Value = half.Top
    End Sub
    Private Sub Half_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles half.Resize
        HalfAL.Value = half.Height
        HalfAN.Value = half.Width
    End Sub
    Private Sub TIEMPCUMPLIDO_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TIEMPCUMPLIDO.Move
        TIEMPCUMPLIDOX.Value = TIEMPCUMPLIDO.Left
        TIEMPCUMPLIDOY.Value = TIEMPCUMPLIDO.Top
    End Sub
    Private Sub TIEMPCUMPLIDO_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TIEMPCUMPLIDO.Resize
        TIEMPCUMPLIDOAL.Value = TIEMPCUMPLIDO.Height
        TIEMPCUMPLIDOAN.Value = TIEMPCUMPLIDO.Width
    End Sub
    Private Sub TIEMPCUMPLIDOX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TIEMPCUMPLIDOY.ValueChanged, TIEMPCUMPLIDOX.ValueChanged
        TIEMPCUMPLIDO.Left = TIEMPCUMPLIDOX.Value
        TIEMPCUMPLIDO.Top = TIEMPCUMPLIDOY.Value
    End Sub
    Private Sub TIEMPCUMPLIDOAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TIEMPCUMPLIDOAN.ValueChanged, TIEMPCUMPLIDOAL.ValueChanged
        TIEMPCUMPLIDO.Width = TIEMPCUMPLIDOAN.Value
        TIEMPCUMPLIDO.Height = TIEMPCUMPLIDOAL.Value
    End Sub

#End Region
#Region "otros"
    Private Sub EscalaX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EscalaY.ValueChanged, EscalaX.ValueChanged
        Escala.Left = EscalaX.Value
        Escala.Top = EscalaY.Value
    End Sub
    Private Sub EscalaAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EscalaAN.ValueChanged, EscalaAL.ValueChanged
        Escala.Width = EscalaAN.Value
        Escala.Height = EscalaAL.Value
    End Sub
    Private Sub Escala_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Escala.Move
        EscalaX.Value = Escala.Left
        EscalaY.Value = Escala.Top
    End Sub
    Private Sub Escala_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Escala.Resize
        EscalaAL.Value = Escala.Height
        EscalaAN.Value = Escala.Width
    End Sub
    Private Sub PlayerLX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerLY.ValueChanged, PlayerLX.ValueChanged
        PlayerL.Left = PlayerLX.Value
        PlayerL.Top = PlayerLY.Value
    End Sub
    Private Sub PlayerLAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerLAN.ValueChanged, PlayerLAL.ValueChanged
        PlayerL.Width = PlayerLAN.Value
        PlayerL.Height = PlayerLAL.Value
    End Sub
    Private Sub PlayerVX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerVY.ValueChanged, PlayerVX.ValueChanged
        PlayerV.Left = PlayerVX.Value
        PlayerV.Top = PlayerVY.Value
    End Sub
    Private Sub PlayerVAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerVAN.ValueChanged, PlayerVAL.ValueChanged
        PlayerV.Width = PlayerVAN.Value
        PlayerV.Height = PlayerVAL.Value
    End Sub
    Private Sub PosLX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PosLY.ValueChanged, PosLX.ValueChanged
        PosL.Left = PosLX.Value
        PosL.Top = PosLY.Value
    End Sub
    Private Sub PosVX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PosVY.ValueChanged, PosVX.ValueChanged
        PosV.Left = PosVX.Value
        PosV.Top = PosVY.Value
    End Sub
    Private Sub ResistenciaLX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResistenciaLY.ValueChanged, ResistenciaLX.ValueChanged
        ResistenciaL.Left = ResistenciaLX.Value
        ResistenciaL.Top = ResistenciaLY.Value
    End Sub
    Private Sub ResistenciaLAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResistenciaLAN.ValueChanged, ResistenciaLAL.ValueChanged
        ResistenciaL.Width = ResistenciaLAN.Value
        ResistenciaL.Height = ResistenciaLAL.Value
    End Sub
    Private Sub ResistenciaVX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResistenciaVY.ValueChanged, ResistenciaVX.ValueChanged
        ResistenciaV.Left = ResistenciaVX.Value
        ResistenciaV.Top = ResistenciaVY.Value
    End Sub
    Private Sub ResistenciaVAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResistenciaVAN.ValueChanged, ResistenciaVAL.ValueChanged
        ResistenciaV.Width = ResistenciaVAN.Value
        ResistenciaV.Height = ResistenciaVAL.Value
    End Sub
    Private Sub PlayerTEXTLX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerTEXTLY.ValueChanged, PlayerTEXTLX.ValueChanged
        PlayerTEXTL.Left = PlayerTEXTLX.Value
        PlayerTEXTL.Top = PlayerTEXTLY.Value
    End Sub
    Private Sub PlayerTEXTLAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerTEXTLAN.ValueChanged, PlayerTEXTLAL.ValueChanged
        PlayerTEXTL.Width = PlayerTEXTLAN.Value
        PlayerTEXTL.Height = PlayerTEXTLAL.Value
    End Sub
    Private Sub PlayerTEXTVX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerTEXTVY.ValueChanged, PlayerTEXTVX.ValueChanged
        PlayerTEXTV.Left = PlayerTEXTVX.Value
        PlayerTEXTV.Top = PlayerTEXTVY.Value
    End Sub
    Private Sub PlayerTEXTVAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerTEXTVAN.ValueChanged, PlayerTEXTVAL.ValueChanged
        PlayerTEXTV.Width = PlayerTEXTVAN.Value
        PlayerTEXTV.Height = PlayerTEXTVAL.Value
    End Sub
    Private Sub PlayerL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerL.Move
        PlayerLX.Value = PlayerL.Left
        PlayerLY.Value = PlayerL.Top
    End Sub
    Private Sub PlayerL_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerL.Resize
        PlayerLAL.Value = PlayerL.Height
        PlayerLAN.Value = PlayerL.Width
    End Sub
    Private Sub PlayerV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerV.Move
        PlayerVX.Value = PlayerV.Left
        PlayerVY.Value = PlayerV.Top
    End Sub
    Private Sub PlayerV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerV.Resize
        PlayerVAL.Value = PlayerV.Height
        PlayerVAN.Value = PlayerV.Width
    End Sub
    Private Sub PosL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PosL.Move
        PosLX.Value = PosL.Left
        PosLY.Value = PosL.Top
    End Sub
    Private Sub PosL_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PosL.Resize
        PosLAL.Value = PosL.Height
        PosLAN.Value = PosL.Width
    End Sub
    Private Sub PosV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PosV.Move
        PosVX.Value = PosV.Left
        PosVY.Value = PosV.Top
    End Sub
    Private Sub PosV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PosV.Resize
        PosVAL.Value = PosV.Height
        PosVAN.Value = PosV.Width
    End Sub
    Private Sub ResistenciaL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResistenciaL.Move
        ResistenciaLX.Value = ResistenciaL.Left
        ResistenciaLY.Value = ResistenciaL.Top
    End Sub
    Private Sub ResistenciaL_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResistenciaL.Resize
        ResistenciaLAL.Value = ResistenciaL.Height
        ResistenciaLAN.Value = ResistenciaL.Width
    End Sub
    Private Sub ResistenciaV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResistenciaV.Move
        ResistenciaVX.Value = ResistenciaV.Left
        ResistenciaVY.Value = ResistenciaV.Top
    End Sub
    Private Sub ResistenciaV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResistenciaV.Resize
        ResistenciaVAL.Value = ResistenciaV.Height
        ResistenciaVAN.Value = ResistenciaV.Width
    End Sub
    Private Sub PlayerTEXTL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerTEXTL.Move
        PlayerTEXTLX.Value = PlayerTEXTL.Left
        PlayerTEXTLY.Value = PlayerTEXTL.Top
    End Sub
    Private Sub PlayerTEXTL_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerTEXTL.Resize
        PlayerTEXTLAL.Value = PlayerTEXTL.Height
        PlayerTEXTLAN.Value = PlayerTEXTL.Width
    End Sub
    Private Sub PlayerTEXTV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerTEXTV.Move
        PlayerTEXTVX.Value = PlayerTEXTV.Left
        PlayerTEXTVY.Value = PlayerTEXTV.Top
    End Sub
    Private Sub PlayerTEXTV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayerTEXTV.Resize
        PlayerTEXTVAL.Value = PlayerTEXTV.Height
        PlayerTEXTVAN.Value = PlayerTEXTV.Width
    End Sub
    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        AboutBox1.ShowDialog()
    End Sub
    Private Sub TV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TV.Move
        TVX.Value = TV.Left
        TVY.Value = TV.Top
    End Sub
    Private Sub TV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TV.Resize
        TVAL.Value = TV.Height
        TVAN.Value = TV.Width
    End Sub
    Private Sub TVX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TVY.ValueChanged, TVX.ValueChanged
        TV.Left = TVX.Value
        TV.Top = TVY.Value
    End Sub
    Private Sub TVAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TVAN.ValueChanged, TVAL.ValueChanged
        TV.Width = TVAN.Value
        TV.Height = TVAL.Value
    End Sub
    Private Sub EXTRA_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EXTRA.Move
        EXTRAX.Value = EXTRA.Left
        EXTRAY.Value = EXTRA.Top
    End Sub
    Private Sub EXTRA_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EXTRA.Resize
        EXTRAAL.Value = EXTRA.Height
        EXTRAAN.Value = EXTRA.Width
    End Sub
    Private Sub EXTRAX_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EXTRAY.ValueChanged, EXTRAX.ValueChanged
        EXTRA.Left = EXTRAX.Value
        EXTRA.Top = EXTRAY.Value
    End Sub
    Private Sub EXTRAAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EXTRAAN.ValueChanged, EXTRAAL.ValueChanged
        EXTRA.Width = EXTRAAN.Value
        EXTRA.Height = EXTRAAL.Value
    End Sub
    Private Sub AtaqueLX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AtaqueLY.ValueChanged, AtaqueLX.ValueChanged
        AtaqueL.Left = AtaqueLX.Value
        AtaqueL.Top = AtaqueLY.Value

    End Sub
    Private Sub AtaqueLAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AtaqueLAN.ValueChanged, AtaqueLAL.ValueChanged
        AtaqueL.Width = AtaqueLAN.Value
        AtaqueL.Height = AtaqueLAL.Value
    End Sub
    Private Sub AtaqueVX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AtaqueVY.ValueChanged, AtaqueVX.ValueChanged
        AtaqueV.Left = AtaqueVX.Value
        AtaqueV.Top = AtaqueVY.Value

    End Sub
    Private Sub AtaqueVAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AtaqueVAN.ValueChanged, AtaqueVAL.ValueChanged
        AtaqueV.Width = AtaqueVAN.Value
        AtaqueV.Height = AtaqueVAL.Value
    End Sub
    Private Sub AtaqueL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AtaqueL.Move
        AtaqueLX.Value = AtaqueL.Left
        AtaqueLY.Value = AtaqueL.Top
    End Sub
    Private Sub AtaqueL_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AtaqueL.Resize
        AtaqueLAL.Value = AtaqueL.Height
        AtaqueLAN.Value = AtaqueL.Width
    End Sub
    Private Sub AtaqueV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AtaqueV.Move
        AtaqueVX.Value = AtaqueV.Left
        AtaqueVY.Value = AtaqueV.Top
    End Sub
    Private Sub AtaqueV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AtaqueV.Resize
        AtaqueVAL.Value = AtaqueV.Height
        AtaqueVAN.Value = AtaqueV.Width
    End Sub

    Private Sub PotenciaL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PotenciaL.Move
        PotenciaLX.Value = PotenciaL.Left
        PotenciaLY.Value = PotenciaL.Top
    End Sub
    Private Sub PotenciaL_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PotenciaL.Resize
        PotenciaLAL.Value = PotenciaL.Height
        PotenciaLAN.Value = PotenciaL.Width
    End Sub
    Private Sub PotenciaLX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PotenciaLY.ValueChanged, PotenciaLX.ValueChanged
        PotenciaL.Left = PotenciaLX.Value
        PotenciaL.Top = PotenciaLY.Value
    End Sub
    Private Sub PotenciaLAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PotenciaLAN.ValueChanged, PotenciaLAL.ValueChanged
        PotenciaL.Width = PotenciaLAN.Value
        PotenciaL.Height = PotenciaLAL.Value
    End Sub
    Private Sub PotenciaV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PotenciaV.Move
        PotenciaVX.Value = PotenciaV.Left
        PotenciaVY.Value = PotenciaV.Top
    End Sub
    Private Sub PotenciaV_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PotenciaV.Resize
        PotenciaVAL.Value = PotenciaV.Height
        PotenciaVAN.Value = PotenciaV.Width
    End Sub
    Private Sub PotenciaVX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PotenciaVY.ValueChanged, PotenciaVX.ValueChanged
        PotenciaV.Left = PotenciaVX.Value
        PotenciaV.Top = PotenciaVY.Value
    End Sub
    Private Sub PotenciaVAN_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PotenciaVAN.ValueChanged, PotenciaVAL.ValueChanged
        PotenciaV.Width = PotenciaVAN.Value
        PotenciaV.Height = PotenciaVAL.Value
    End Sub


    Private Sub AAL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AAL.Move
        AALX.Value = AAL.Left
        AALY.Value = AAL.Top
    End Sub
    Private Sub AALX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AALY.ValueChanged, AALX.ValueChanged
        AAL.Left = AALX.Value
        AAL.Top = AALY.Value
    End Sub
    Private Sub ABL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ABL.Move
        ABLX.Value = ABL.Left
        ABLY.Value = ABL.Top
    End Sub
    Private Sub ABLX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ABLY.ValueChanged, ABLX.ValueChanged
        ABL.Left = ABLX.Value
        ABL.Top = ABLY.Value
    End Sub
    Private Sub ACL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ACL.Move
        ACLX.Value = ACL.Left
        ACLY.Value = ACL.Top
    End Sub
    Private Sub ACLX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ACLY.ValueChanged, ACLX.ValueChanged
        ACL.Left = ACLX.Value
        ACL.Top = ACLY.Value
    End Sub
    Private Sub ADL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ADL.Move
        ADLX.Value = ADL.Left
        ADLY.Value = ADL.Top
    End Sub
    Private Sub ADLX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ADLY.ValueChanged, ADLX.ValueChanged
        ADL.Left = ADLX.Value
        ADL.Top = ADLY.Value
    End Sub
    Private Sub AEL_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AEL.Move
        AELX.Value = AEL.Left
        AELY.Value = AEL.Top
    End Sub
    Private Sub AELX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AELY.ValueChanged, AELX.ValueChanged
        AEL.Left = AELX.Value
        AEL.Top = AELY.Value
    End Sub
    Private Sub AAV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AAV.Move
        AAVX.Value = AAV.Left
        AAVY.Value = AAV.Top
    End Sub
    Private Sub AAVX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AAVY.ValueChanged, AAVX.ValueChanged
        AAV.Left = AAVX.Value
        AAV.Top = AAVY.Value
    End Sub
    Private Sub ABV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ABV.Move
        ABVX.Value = ABV.Left
        ABVY.Value = ABV.Top
    End Sub
    Private Sub ABVX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ABVY.ValueChanged, ABVX.ValueChanged
        ABV.Left = ABVX.Value
        ABV.Top = ABVY.Value
    End Sub
    Private Sub ACV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ACV.Move
        ACVX.Value = ACV.Left
        ACVY.Value = ACV.Top
    End Sub
    Private Sub ACVX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ACVY.ValueChanged, ACVX.ValueChanged
        ACV.Left = ACVX.Value
        ACV.Top = ACVY.Value
    End Sub
    Private Sub ADV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ADV.Move
        ADVX.Value = ADV.Left
        ADVY.Value = ADV.Top
    End Sub
    Private Sub ADVX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ADVY.ValueChanged, ADVX.ValueChanged
        ADV.Left = ADVX.Value
        ADV.Top = ADVY.Value
    End Sub
    Private Sub AEV_Move(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AEV.Move
        AEVX.Value = AEV.Left
        AEVY.Value = AEV.Top
    End Sub
    Private Sub AEVX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AEVY.ValueChanged, AEVX.ValueChanged
        AEV.Left = AEVX.Value
        AEV.Top = AEVY.Value
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
        Select Case My.Settings.ResolucionAlta
            Case 2
                Panel1.BackgroundImage = My.Resources._1024BG
            Case 1
                Panel1.BackgroundImage = My.Resources._640BG
            Case 0
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
    'Dibujar las texturas
    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint
        If Mapeo.BackgroundImage Is Nothing Then Exit Sub
       
        If TabControl3.SelectedIndex = 0 Then

            '' crear un objeto bitmap

            Try
                Dim Scr_Graphics As Graphics = e.Graphics
                Scr_Graphics.DrawImage(Scr_CB, Me.ScrX.Value, Me.ScrY.Value, Me.ScrAN.Value, Me.ScrAL.Value)
            Catch ex As Exception
            End Try

            Try
                Dim TV_Graphics As Graphics = e.Graphics
                TV_Graphics.DrawImage(Tv_CB, Me.TVX.Value, Me.TVY.Value, Me.TVAN.Value, Me.TVAL.Value)
            Catch ex As Exception

            End Try
            Try
                Dim EXTRA_Graphics As Graphics = e.Graphics
                EXTRA_Graphics.DrawImage(EXTRA_CB, Me.EXTRAX.Value, Me.EXTRAY.Value, Me.EXTRAAN.Value, Me.EXTRAAL.Value)
            Catch ex As Exception

            End Try
            Try
                Dim PlayerTEXTL_Graphics As Graphics = e.Graphics
                PlayerTEXTL_Graphics.DrawImage(PlayerTEXTL_CB, Me.PlayerTEXTLX.Value, Me.PlayerTEXTLY.Value, Me.PlayerTEXTLAN.Value, Me.PlayerTEXTLAL.Value)
            Catch ex As Exception

            End Try
            Try
                Dim PlayerTEXTV_Graphics As Graphics = e.Graphics
                PlayerTEXTV_Graphics.DrawImage(PlayerTEXTV_CB, Me.PlayerTEXTVX.Value, Me.PlayerTEXTVY.Value, Me.PlayerTEXTVAN.Value, Me.PlayerTEXTVAL.Value)
            Catch ex As Exception

            End Try
            Try
                Dim AtaqueL_Graphics As Graphics = e.Graphics
                AtaqueL_Graphics.DrawImage(AtaqueL.Image, Me.AtaqueLX.Value, Me.AtaqueLY.Value, Me.AtaqueLAN.Value, Me.AtaqueLAL.Value)
            Catch ex As Exception

            End Try
            Try
                Dim AtaqueV_Graphics As Graphics = e.Graphics
                AtaqueV_Graphics.DrawImage(AtaqueV.Image, Me.AtaqueVX.Value, Me.AtaqueVY.Value, Me.AtaqueVAN.Value, Me.AtaqueVAL.Value)

            Catch ex As Exception

            End Try

        End If
    End Sub


    Dim zliblong, unzliblong, Xvalor, Yvalor, XvalorNP As Integer
    Public zlibfile(), unzlibfile() As Byte


    Private Sub Importarwe8(ByVal we8 As Boolean)
        unzlibfile = unzlib(dlgAbrir.FileName)
        'Modalidad textura
        Dim Bytes(7) As Byte
        Array.Copy(unzlibfile, 40, Bytes, 0, 7)
        Dim SCR_slot As String = Encoding.UTF8.GetString(Bytes) 'Convierte los bytes a string(texto)
        modalidad.Text = SCR_slot

        Exportar.Enabled = True

        PosL.Visible = True
        PosV.Visible = True
        AtaqueV.Visible = True
        AtaqueL.Visible = True
        Importarwe8load_Obj()
    End Sub
    Private Sub Importarwe8load_Obj()
        obj_TextoRead(1232, ScoreL, ScoreLT, ScoreLC) 'Score local
        obj_TextoRead(1256, ScoreV, ScoreVT, ScoreVC) 'Score Visita
        obj_TextoRead(1280, half, HalfT, HalfC) 'Half
        obj_TextoRead(1304, Minutos, MinutosT, MinutosC) 'Minutos
        obj_TextoRead(1328, Segundos, SegundosT, SegundosC) 'Segundos
        obj_TextoRead(1352, TIEMPCUMPLIDO, TIEMPCUMPLIDOT, TIEMPCUMPLIDOC) 'TIEMPCUMPLIDO

        obj_logoRead(1376, BanderaL) 'Bandera Local
        obj_logoRead(1390, BanderaV) 'Bandera Visita
        obj_logoRead(2340, PotenciaL) 'Potencia Local
        obj_logoRead(3156, PotenciaV) 'Potencia Visita
        obj_logoRead(2354, PlayerL) 'Player Local
        obj_logoRead(3170, PlayerV) 'Player Visita
        obj_logoSTRead(2368, PosL) 'Pos Local
        obj_logoSTRead(3184, PosV) 'Pos Visita
        obj_Textura4Read(2282, AtaqueL, Nothing, Nothing) 'Ataque Local
        obj_Textura4Read(3098, AtaqueV, Nothing, Nothing) 'Ataque Visita
        obj_logoSTRead(2382, AAL) 'AA Local
        obj_logoSTRead(3198, AAV) 'AA Visita
        obj_logoSTRead(2396, ABL) 'AB Local
        obj_logoSTRead(3212, ABV) 'AB Visita
        obj_logoSTRead(2410, ACL) 'AC Local
        obj_logoSTRead(3226, ACV) 'AC Visita
        obj_logoSTRead(2424, ADL) 'AD Local
        obj_logoSTRead(3240, ADV) 'AD Visita
        obj_logoSTRead(2438, AEL) 'AE Local
        obj_logoSTRead(3254, AEV) 'AE Visita
        obj_Textura4Read(534, Scr, Rojo, Nothing) 'Textura Scr
        obj_Textura4Read(598, TV, Violeta, Nothing) 'Textura tv
        obj_Textura4Read(662, EXTRA, Amarillo, Nothing) 'Textura extra
        obj_Textura4Read(1706, PlayerTEXTL, Azul, CheckBox1) 'Textura Nameplate Local
        obj_Textura4Read(2522, PlayerTEXTV, Verde, CheckBox2) 'Textura Nameplate Local

        If EspañolToolStripMenuItem.CheckState = CheckState.Checked Then
            MsgBox("Marcador Cargado", MsgBoxStyle.Information)
        Else
            MsgBox("Loaded Scoreboard", MsgBoxStyle.Information)
        End If
        Exportar.Enabled = True

    End Sub
    Private Sub exportarwe8(ByVal we8 As Boolean)
       'Escribir bytes en cero SCR
        Dim writemodalidad(7) As Byte
        writemodalidad(0) = 0
        Array.Copy(writemodalidad, 0, unzlibfile, 40, 8)

        'fs.Position = 40
        'fs.Write(writemodalidad, 0, 8)

        'Guardar Modalidad textura
        If modalidad.Text.Length <= 7 Then
            Dim Cadena As String = modalidad.Text
            Dim bytes2() As Byte = Encoding.ASCII.GetBytes(Cadena)

            Array.Copy(bytes2, 0, unzlibfile, 40, bytes2.Length)

            'bw.Seek(40, SeekOrigin.Begin)
            'bw.Write(bytes2, 0, modalidad.Text.Length)
        Else
            MsgBox("Modalidad No debe contener más de 7 Carácteres")
            Exit Sub
        End If

        'parchar Marcador
        unzlibfile(727) = Byte.Parse(32)
        unzlibfile(791) = Byte.Parse(32)
        unzlibfile(855) = Byte.Parse(32)
        unzlibfile(919) = Byte.Parse(32)
        unzlibfile(983) = Byte.Parse(32)
        unzlibfile(1047) = Byte.Parse(32)
        unzlibfile(1111) = Byte.Parse(32)
        unzlibfile(1175) = Byte.Parse(32)

        obj_logoSTWrite(13458, PotenciaL)
        obj_logoSTWrite(13698, PotenciaV)


        AtaqueV.Visible = True
        AtaqueL.Visible = True
        obj_TextoWrite(1232, ScoreL, ScoreLT, ScoreLC) 'Score local
        obj_TextoWrite(1256, ScoreV, ScoreVT, ScoreVC) 'Score Visita
        obj_TextoWrite(1280, half, HalfT, HalfC) 'Half
        obj_TextoWrite(1304, Minutos, MinutosT, MinutosC) 'Minutos
        obj_TextoWrite(1328, Segundos, SegundosT, SegundosC) 'Segundos
        obj_TextoWrite(1352, TIEMPCUMPLIDO, TIEMPCUMPLIDOT, TIEMPCUMPLIDOC) 'TIEMPCUMPLIDO

        obj_logoWrite(1376, BanderaL) 'Bandera Local
        obj_logoWrite(1390, BanderaV) 'Bandera Visita
        obj_logoWrite(2340, PotenciaL) 'Potencia Local
        obj_logoWrite(35680, PotenciaL) 'Potencia Local
        obj_logoWrite(31364, PotenciaL) 'Potencia Local
        obj_logoWrite(3156, PotenciaV) 'Potencia Visita
        obj_logoWrite(36176, PotenciaV) 'Potencia Visita
        obj_logoWrite(31604, PotenciaV) 'Potencia Visita
        obj_logoWrite(2354, PlayerL) 'Player Local
        obj_logoWrite(3170, PlayerV) 'Player Visita
        obj_logoSTWrite(2368, PosL) 'Pos Local
        obj_logoSTWrite(3184, PosV) 'Pos Visita
        obj_Textura4Write(2282, AtaqueL, Nothing, Nothing) 'Ataque Local
        obj_Textura4Write(3098, AtaqueV, Nothing, Nothing) 'Ataque Visita
        obj_logoSTWrite(2382, AAL) 'AA Local
        obj_logoSTWrite(3198, AAV) 'AA Visita
        obj_logoSTWrite(2396, ABL) 'AB Local
        obj_logoSTWrite(3212, ABV) 'AB Visita
        obj_logoSTWrite(2410, ACL) 'AC Local
        obj_logoSTWrite(3226, ACV) 'AC Visita
        obj_logoSTWrite(2424, ADL) 'AD Local
        obj_logoSTWrite(3240, ADV) 'AD Visita
        obj_logoSTWrite(2438, AEL) 'AE Local
        obj_logoSTWrite(3254, AEV) 'AE Visita

        obj_logoSTWrite(31406, AAL) 'AA Local
        obj_logoSTWrite(31646, AAV) 'AA Visita
        obj_logoSTWrite(31420, ABL) 'AB Local
        obj_logoSTWrite(31660, ABV) 'AB Visita
        obj_logoSTWrite(31434, ACL) 'AC Local
        obj_logoSTWrite(31674, ACV) 'AC Visita
        obj_logoSTWrite(31448, ADL) 'AD Local
        obj_logoSTWrite(31688, ADV) 'AD Visita
        obj_logoSTWrite(31462, AEL) 'AE Local
        obj_logoSTWrite(31702, AEV) 'AE Visita


        obj_logoSTWrite(27686, AAL) 'AA Local
        obj_logoSTWrite(28182, AAV) 'AA Visita
        obj_logoSTWrite(27700, ABL) 'AB Local
        obj_logoSTWrite(28196, ABV) 'AB Visita
        obj_logoSTWrite(27714, ACL) 'AC Local
        obj_logoSTWrite(28210, ACV) 'AC Visita
        obj_logoSTWrite(27728, ADL) 'AD Local
        obj_logoSTWrite(28224, ADV) 'AD Visita
        obj_logoSTWrite(27742, AEL) 'AE Local
        obj_logoSTWrite(28238, AEV) 'AE Visita

        obj_Textura4Write(534, Scr, Rojo, Nothing) 'Textura Scr
        obj_Textura4Write(598, TV, Violeta, Nothing) 'Textura tv
        obj_Textura4Write(662, EXTRA, Amarillo, Nothing) 'Textura extra
        obj_Textura4Write(1706, PlayerTEXTL, Azul, CheckBox1) 'Textura Nameplate Local
        obj_Textura4Write(2522, PlayerTEXTV, Verde, CheckBox2) 'Textura Nameplate Local

        zlib(dlgAbrir.FileName, unzlibfile)
        

        If EspañolToolStripMenuItem.CheckState = CheckState.Checked Then
            MsgBox("Marcador Guardado", MsgBoxStyle.Information)
        Else
            MsgBox("Saved Scoreboard", MsgBoxStyle.Information)
        End If


    End Sub


   
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
        Rojo.Left += 1
        Rojo.Left -= 1
        Azul.Left += 1
        Azul.Left -= 1
        Violeta.Left += 1
        Violeta.Left -= 1
        Amarillo.Left += 1
        Amarillo.Left -= 1
        Verde.Left += 1
        Verde.Left -= 1


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
        Español()
    End Sub
    Private Sub EnglishToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnglishToolStripMenuItem.Click
        EspañolToolStripMenuItem.CheckState = CheckState.Unchecked
        EnglishToolStripMenuItem.CheckState = CheckState.Checked
        English()
    End Sub
   

    
    Private Sub ResBaja_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResBaja.Click
        Select Case ResBaja.CheckState
            Case CheckState.Checked
                MsgBox("Ya esta Seleccionada esta opción")
            Case CheckState.Unchecked
                ResBaja.CheckState = CheckState.Checked
                ResMedia.CheckState = CheckState.Unchecked
                ResAlta.CheckState = CheckState.Unchecked
                Resolucionbaja()
                Try
                    Importarwe8load_Obj()
                Catch ex As Exception

                End Try
        End Select
    End Sub
    Private Sub ResAlta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResAlta.Click
        Select Case ResAlta.CheckState
            Case CheckState.Checked
                MsgBox("Ya esta Seleccionada esta opción")
            Case CheckState.Unchecked
                ResBaja.CheckState = CheckState.Unchecked
                ResMedia.CheckState = CheckState.Unchecked
                ResAlta.CheckState = CheckState.Checked

                Resolucionalta()
                Try
                    Importarwe8load_Obj()
                Catch ex As Exception

                End Try
        End Select
    End Sub

    Private Sub ResMedia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResMedia.Click
        Select Case ResMedia.CheckState
            Case CheckState.Checked
                MsgBox("Ya esta Seleccionada esta opción")
            Case CheckState.Unchecked
                ResBaja.CheckState = CheckState.Unchecked
                ResMedia.CheckState = CheckState.Checked
                ResAlta.CheckState = CheckState.Unchecked
                Resolucionmedia()
                Try
                    Importarwe8load_Obj()
                Catch ex As Exception

                End Try

        End Select
    End Sub


#End Region

End Class