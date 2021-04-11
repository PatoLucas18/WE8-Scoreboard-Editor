Imports System.IO

Module Others

    Public Function unzlib(ByVal archivo As String) As Byte()
        Dim zliblong, unzliblong As Integer
        unzliblong = 0
        zliblong = 0
        Dim zlibfile() As Byte = IO.File.ReadAllBytes(archivo)
        zliblong = BitConverter.ToInt32(zlibfile, 4)
        unzliblong = BitConverter.ToInt32(zlibfile, 8)
        Dim bytes2 As Byte() = IO.File.ReadAllBytes(archivo)
        Array.Copy(zlibfile, 32, bytes2, 0, zliblong)
        Dim unzlibfiletemp(unzliblong - 1) As Byte
        Zlibtool.UncompressByteArray(unzlibfiletemp, unzliblong, bytes2, zliblong)

        Return unzlibfiletemp

    End Function
    Public Function zlib(ByVal archivo As String, ByVal unzlib() As Byte)
        Dim zlibfile(unzlib.Length) As Byte
        Zlibtool.CompressByteArray(zlibfile, unzlib.Length, unzlib, unzlib.Length)

        Dim longZlib As Integer = zlibfile.Length
        Dim longUnzlib As Integer = unzlib.Length
        'Buscar bytes seguidos en cero
        Dim find As Byte = 0
        Dim index As Integer = longZlib
        Do
            index -= 8
            If BitConverter.ToInt64(zlibfile, index) = 0 Then
            Else
                index = System.Array.IndexOf(Of Byte)(zlibfile, find, index)
                Exit Do
            End If
        Loop

        Dim opd(index + 31) As Byte
        Dim opdbyte() As Byte = {&H0, &H4, &H1, &H0}
        Array.Copy(opdbyte, 0, opd, 0, 4)
        Array.Copy(zlibfile, 0, opd, 32, index)
        Dim vOut1() As Byte = BitConverter.GetBytes(CInt(index))
        Array.Copy(vOut1, 0, opd, 4, 4)
        Dim vOut2() As Byte = BitConverter.GetBytes(longUnzlib)
        Array.Copy(vOut2, 0, opd, 8, 4)
        opd(33) = Byte.Parse(&HDA)
        File.WriteAllBytes(archivo, opd)
        Return False
    End Function

    Public Function enlazar(ByVal textura As PictureBox, ByVal X As NumericUpDown, ByVal Y As NumericUpDown, ByVal W As NumericUpDown, ByVal H As NumericUpDown)
        textura.Left = X.Value
        textura.Top = Y.Value
        textura.Width = W.Value
        textura.Height = H.Value
        Return False
    End Function

    Public Function English()
        Form1.TabPage8.Text = "Scoreboard"
        Form1.GroupBox3.Text = "Goals"
        Form1.ScoreVT.Text = "Size"
        Form1.ScoreVC.Text = "Colors"
        Form1.ScoreLT.Text = "Size"
        Form1.Label20.Text = "Away:"
        Form1.ScoreLC.Text = "Colors"
        Form1.Label29.Text = "Home:"
        'Form1.Label30.Text = "Height:"
        'Form1.Label31.Text = "X:"
        'Form1.Label32.Text = "Width:"
        Form1.GroupBox4.Text = "Texture"
        Form1.Label42.Text = "Extra:"
        Form1.Label41.Text = "TV Logo:"
        Form1.Label40.Text = "Scoreboard:"
        'Form1.Label16.Text = "Height:"
        'Form1.Label27.Text = "Width:"
        Form1.GroupBox2.Text = "Time"
        Form1.TIEMPCUMPLIDOT.Text = "Size"
        Form1.TIEMPCUMPLIDOC.Text = "Colors"
        Form1.Label25.Text = "Extra Time:"
        Form1.HalfT.Text = "Size"
        Form1.HalfC.Text = "Colors"
        Form1.Label38.Text = "1st, 2nd:"
        Form1.SegundosT.Text = "Size"
        Form1.SegundosC.Text = "Colors"
        Form1.MinutosT.Text = "Size"
        Form1.Label4.Text = "Seconds:"
        Form1.MinutosC.Text = "Colors"
        Form1.Label6.Text = "Minutes:"
        'Form1.Label9.Text = "Height:"
        'Form1.Label11.Text = "Width:"
        Form1.GroupBox1.Text = "Flag: Club and National Team"
        Form1.Label17.Text = "Home:"
        Form1.Label18.Text = "Away:"
        'Form1.Label14.Text = "Width:"
        'Form1.Label8.Text = "Height:"
        Form1.TabPage1.Text = "Name Plate"
        Form1.GroupBox5.Text = "Home"
        Form1.Label1.Text = "Shoot Bar:"
        Form1.Label2.Text = "Attack:"
        Form1.CheckBox2.Text = "Invert"
        Form1.Label3.Text = "Texture:"
        Form1.Label5.Text = "Stamina:"
        Form1.Label22.Text = "Position:"
        Form1.Label24.Text = "Name:"
        'Form1.Label35.Text = "Height:"
        'Form1.Label44.Text = "Width:"
        Form1.GroupBox8.Text = "Home"
        Form1.Label43.Text = "Shoot Bar:"
        Form1.Label37.Text = "Attack:"
        Form1.CheckBox1.Text = "Invert"
        Form1.Label34.Text = "Texture:"
        Form1.Label23.Text = "Stamina:"
        Form1.Label19.Text = "Position:"
        Form1.Label57.Text = "Name:"
        'Form1.Label51.Text = "Height:"
        'Form1.Label53.Text = "Width:"
        Form1.TabPage26.Text = "Map Texture:"
        Form1.GroupBox7.Text = "Shoot Bar WE9 y PES5"
        Form1.GroupBox6.Text = "Map Texture"
        Form1.Label36.Text = "Switch Map"
        Form1.Label140.Text = "Name Away"
        Form1.Label224.Text = "TV Logo"
        Form1.Label225.Text = "Extra"
        Form1.Label226.Text = "Name Home"
        'Form1.Label233.Text = "Width:"
        Form1.Label230.Text = "Scoreboard"
        'Form1.Label231.Text = "Height:"
        Form1.Button1.Text = "Colors"
        Form1.archivo.Text = "File"
        Form1.ToolStripMenuItem1.Text = "File"
        Form1.importar.Text = "Open"
        Form1.Exportar.Text = "Save"
        Form1.TexturasToolStripMenuItem.Text = "Textures (.png)"
        Form1.ScrTvExtraEtcToolStripMenuItem.Text = "Scr, Tv, Extra, Name Plate"
        Form1.Game00ToolStripMenuItem.Text = "game_00 (Texture 1 -151) 512x512"
        Form1.Game01Textura2ToolStripMenuItem.Text = "game_01 (Texture 2 -151) 512x512"
        Form1.about.Text = "About..."
        Form1.FondoToolStripMenuItem.Text = "Background"
        Form1.CargarToolStripMenuItem.Text = "Load"
        Form1.RestaurarToolStripMenuItem.Text = "Restore"
        Form1.modalidad.Text = "Slot Texture"
        Form1.Label46.Text = "Map"
        Return False
    End Function
    Public Function Español()
        Form1.TabPage8.Text = "Marcador"
        Form1.GroupBox3.Text = "Goles"
        Form1.ScoreVT.Text = "Tamaño"
        Form1.ScoreVC.Text = "Color"
        Form1.ScoreLT.Text = "Tamaño"
        Form1.Label20.Text = "Visitante:"
        Form1.ScoreLC.Text = "Color"
        Form1.Label29.Text = "Local:"
        'form1.Label30.Text = "Alto:"
        'form1.Label31.Text = "X:"
        'form1.Label32.Text = "Ancho:"
        Form1.GroupBox4.Text = "Textura"
        Form1.Label42.Text = "Extra:"
        Form1.Label41.Text = "TV Logo:"
        Form1.Label40.Text = "Marcador:"
        'form1.Label16.Text = "Alto:"
        'form1.Label27.Text = "Ancho:"
        Form1.GroupBox2.Text = "Tiempo"
        Form1.TIEMPCUMPLIDOT.Text = "Tamaño"
        Form1.TIEMPCUMPLIDOC.Text = "Color"
        Form1.Label25.Text = "T. Extra:"
        Form1.HalfT.Text = "Tamaño"
        Form1.HalfC.Text = "Color"
        Form1.Label38.Text = "1er, 2do:"
        Form1.SegundosT.Text = "Tamaño"
        Form1.SegundosC.Text = "Color"
        Form1.MinutosT.Text = "Tamaño"
        Form1.Label4.Text = "Segundos:"
        Form1.MinutosC.Text = "Color"
        Form1.Label6.Text = "Minutos:"
        'form1.Label9.Text = "Alto:"
        'form1.Label11.Text = "Ancho:"
        Form1.GroupBox1.Text = "Escudos y Banderas"
        Form1.Label17.Text = "Local:"
        Form1.Label18.Text = "Visita:"
        'form1.Label14.Text = "Ancho:"
        'form1.Label8.Text = "Alto:"
        Form1.TabPage1.Text = "Nombre Placa"
        Form1.GroupBox5.Text = "Local"
        Form1.Label1.Text = "Potencia:"
        Form1.Label2.Text = "Ataque:"
        Form1.CheckBox2.Text = "Invertir"
        Form1.Label3.Text = "Textura:"
        Form1.Label5.Text = "Resistencia:"
        Form1.Label22.Text = "Posición:"
        Form1.Label24.Text = "Nombre:"
        'form1.Label35.Text = "Alto:"
        'form1.Label44.Text = "Ancho:"
        Form1.GroupBox8.Text = "Local"
        Form1.Label43.Text = "Potencia:"
        Form1.Label37.Text = "Ataque:"
        Form1.CheckBox1.Text = "Invertir"
        Form1.Label34.Text = "Textura:"
        Form1.Label23.Text = "Resistencia:"
        Form1.Label19.Text = "Posición:"
        Form1.Label57.Text = "Nombre:"
        'form1.Label51.Text = "Alto:"
        'form1.Label53.Text = "Ancho:"
        Form1.TabPage26.Text = "Mapeo de Textura:"
        Form1.GroupBox7.Text = "Barra de Potencia WE9 y PES5"
        Form1.GroupBox6.Text = "Mapeo de Textura"
        Form1.Label36.Text = "Intercambiar Mapeo"
        Form1.Label140.Text = "Nombre Visita"
        Form1.Label224.Text = "TV Logo"
        Form1.Label225.Text = "Extra"
        Form1.Label226.Text = "Nombre Local"
        'form1.Label233.Text = "Ancho:"
        Form1.Label230.Text = "Marcador"
        'form1.Label231.Text = "Alto:"
        Form1.Button1.Text = "Color"
        Form1.archivo.Text = "Archivo"
        Form1.ToolStripMenuItem1.Text = "Archivo"
        Form1.importar.Text = "Abrir"
        Form1.Exportar.Text = "Guardar"
        Form1.TexturasToolStripMenuItem.Text = "Texturas (.png)"
        Form1.ScrTvExtraEtcToolStripMenuItem.Text = "Scr, Tv, Extra, Nombre Placa"
        Form1.Game00ToolStripMenuItem.Text = "game_00 (Textura 1 -151) 512x512"
        Form1.Game01Textura2ToolStripMenuItem.Text = "game_01 (Textura 2 -151) 512x512"
        Form1.about.Text = "Acerca De..."
        Form1.FondoToolStripMenuItem.Text = "Fondo"
        Form1.CargarToolStripMenuItem.Text = "Cargar"
        Form1.RestaurarToolStripMenuItem.Text = "Restaurar"
        Form1.modalidad.Text = "Modalidad Textura"
        Form1.Label46.Text = "Mapeo"
        Return False
    End Function


    'Resolucion

    Dim ResAntiguoAncho As Double
    Dim ResAntiguoAlto As Double
    Dim ResNuevoAncho As Double
    Dim ResNuevoAlto As Double

    Public Function Resolucionbaja()

        Form1.Panel1.Size = New Size(640, 480)
        Form1.Width = 640 + 370
        Form1.Height = 480 + 123
        'If Background Is Nothing Then form1.Panel1.BackgroundImage = My.Resources._640BG
        Form1.AAL.MaximumSize = New Size(34, 6)
        Form1.AAL.MinimumSize = New Size(34, 6)
        Form1.ABL.MaximumSize = New Size(34, 6)
        Form1.ABL.MinimumSize = New Size(34, 6)
        Form1.ACL.MaximumSize = New Size(34, 6)
        Form1.ACL.MinimumSize = New Size(34, 6)
        Form1.ADL.MaximumSize = New Size(34, 6)
        Form1.ADL.MinimumSize = New Size(34, 6)
        Form1.AEL.MaximumSize = New Size(34, 6)
        Form1.AEL.MinimumSize = New Size(34, 6)
        Form1.AAV.MaximumSize = New Size(34, 6)
        Form1.AAV.MinimumSize = New Size(34, 6)
        Form1.ABV.MaximumSize = New Size(34, 6)
        Form1.ABV.MinimumSize = New Size(34, 6)
        Form1.ACV.MaximumSize = New Size(34, 6)
        Form1.ACV.MinimumSize = New Size(34, 6)
        Form1.ADV.MaximumSize = New Size(34, 6)
        Form1.ADV.MinimumSize = New Size(34, 6)
        Form1.AEV.MaximumSize = New Size(34, 6)
        Form1.AEV.MinimumSize = New Size(34, 6)

        Return False
    End Function
    Public Function Resolucionalta()
        Form1.Panel1.Size = New Size(1024, 600)
        Form1.Width = 1024 + 370
        Form1.Height = 600 + 123
        Form1.AAL.MaximumSize = New Size(54, 7)
        Form1.AAL.MinimumSize = New Size(54, 7)
        Form1.ABL.MaximumSize = New Size(54, 7)
        Form1.ABL.MinimumSize = New Size(54, 7)
        Form1.ACL.MaximumSize = New Size(54, 7)
        Form1.ACL.MinimumSize = New Size(54, 7)
        Form1.ADL.MaximumSize = New Size(54, 7)
        Form1.ADL.MinimumSize = New Size(54, 7)
        Form1.AEL.MaximumSize = New Size(54, 7)
        Form1.AEL.MinimumSize = New Size(54, 7)
        Form1.AAV.MaximumSize = New Size(54, 7)
        Form1.AAV.MinimumSize = New Size(54, 7)
        Form1.ABV.MaximumSize = New Size(54, 7)
        Form1.ABV.MinimumSize = New Size(54, 7)
        Form1.ACV.MaximumSize = New Size(54, 7)
        Form1.ACV.MinimumSize = New Size(54, 7)
        Form1.ADV.MaximumSize = New Size(54, 7)
        Form1.ADV.MinimumSize = New Size(54, 7)
        Form1.AEV.MaximumSize = New Size(54, 7)
        Form1.AEV.MinimumSize = New Size(54, 7)

        Return False
    End Function
    Public Function Resolucionmedia()
        Form1.Panel1.Size = New Size(800, 600)
        Form1.Width = 800 + 370
        Form1.Height = 600 + 123
        Form1.AAL.MaximumSize = New Size(43, 7)
        Form1.AAL.MinimumSize = New Size(43, 7)
        Form1.ABL.MaximumSize = New Size(43, 7)
        Form1.ABL.MinimumSize = New Size(43, 7)
        Form1.ACL.MaximumSize = New Size(43, 7)
        Form1.ACL.MinimumSize = New Size(43, 7)
        Form1.ADL.MaximumSize = New Size(43, 7)
        Form1.ADL.MinimumSize = New Size(43, 7)
        Form1.AEL.MaximumSize = New Size(43, 7)
        Form1.AEL.MinimumSize = New Size(43, 7)
        Form1.AAV.MaximumSize = New Size(43, 7)
        Form1.AAV.MinimumSize = New Size(43, 7)
        Form1.ABV.MaximumSize = New Size(43, 7)
        Form1.ABV.MinimumSize = New Size(43, 7)
        Form1.ACV.MaximumSize = New Size(43, 7)
        Form1.ACV.MinimumSize = New Size(43, 7)
        Form1.ADV.MaximumSize = New Size(43, 7)
        Form1.ADV.MinimumSize = New Size(43, 7)
        Form1.AEV.MaximumSize = New Size(43, 7)
        Form1.AEV.MinimumSize = New Size(43, 7)
        Return False
    End Function

End Module
