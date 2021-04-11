Module ReadWrite

    '   EJESX = 8192 / Form1.Panel1.Width '12.8 'Divisores
    '   EJESY = 7168 / Form1.Panel1.Height '14.93 Divisores
    '   Xvalor = Form1.Panel1.Width / 2
    '   Yvalor = Form1.Panel1.Height / 2
    'Logos

    Public Function obj_logoWrite(ByVal offsetX As Integer, ByVal logo As PictureBox)
        offsetX -= 4
        Dim vOutx() As Byte = BitConverter.GetBytes(CInt((logo.Left * (8192 / Form1.Panel1.Width)) - 4096))
        Array.Copy(vOutx, 0, Form1.unzlibfile, offsetX + 4, 2)
        Dim vOuty() As Byte = BitConverter.GetBytes(CInt((logo.Top * (7168 / Form1.Panel1.Height)) - 3584))
        Array.Copy(vOuty, 0, Form1.unzlibfile, offsetX + 6, 2)
        Dim vOutw() As Byte = BitConverter.GetBytes(CInt(logo.Width * (8192 / Form1.Panel1.Width)))
        Array.Copy(vOutw, 0, Form1.unzlibfile, offsetX + 10, 2)
        Dim vOuth() As Byte = BitConverter.GetBytes(CInt(logo.Height * (7168 / Form1.Panel1.Height)))
        Array.Copy(vOuth, 0, Form1.unzlibfile, offsetX + 12, 2)
        Return False
    End Function
    Public Function obj_logoRead(ByVal offsetX As Integer, ByVal logo As PictureBox)
        offsetX -= 4
        logo.Left = (BitConverter.ToInt16(Form1.unzlibfile, offsetX + 4) + 4096) / (8192 / Form1.Panel1.Width)
        logo.Top = (BitConverter.ToInt16(Form1.unzlibfile, offsetX + 6) + 3584) / (7168 / Form1.Panel1.Height)
        logo.Width = BitConverter.ToInt16(Form1.unzlibfile, offsetX + 10) / (8192 / Form1.Panel1.Width)
        logo.Height = BitConverter.ToInt16(Form1.unzlibfile, offsetX + 12) / (7168 / Form1.Panel1.Height)
        Return False
    End Function

    'Logos Sin tamaño
    Public Function obj_logoSTWrite(ByVal offsetX As Integer, ByVal logo As PictureBox)
        offsetX -= 4
        Dim vOutx() As Byte = BitConverter.GetBytes(CInt((logo.Left * (8192 / Form1.Panel1.Width)) - 4096))
        Array.Copy(vOutx, 0, Form1.unzlibfile, offsetX + 4, 2)
        Dim vOuty() As Byte = BitConverter.GetBytes(CInt((logo.Top * (7168 / Form1.Panel1.Height)) - 3584))
        Array.Copy(vOuty, 0, Form1.unzlibfile, offsetX + 6, 2)
        Return False
    End Function
    Public Function obj_logoSTRead(ByVal offsetX As Integer, ByVal logo As PictureBox)
        offsetX -= 4
        logo.Left = (BitConverter.ToInt16(Form1.unzlibfile, offsetX + 4) + 4096) / (8192 / Form1.Panel1.Width)
        logo.Top = (BitConverter.ToInt16(Form1.unzlibfile, offsetX + 6) + 3584) / (7168 / Form1.Panel1.Height)
        Return False
    End Function
    'Texturas
    Public Function obj_Textura4Write(ByVal offsetX As Integer, ByVal textura As PictureBox, ByVal mapeo As PictureBox, ByVal invertir As CheckBox)
        offsetX -= 10
        Dim writeBytes(120) As Byte
        writeBytes(0) = 0
        'Escribir bytes en cero SCR
        Array.Copy(writeBytes, 0, Form1.unzlibfile, offsetX + 22, 26)

        Dim vOutx() As Byte = BitConverter.GetBytes(CInt((textura.Left * (8192 / Form1.Panel1.Width)) - 4096))
        Array.Copy(vOutx, 0, Form1.unzlibfile, offsetX + 10, 2)
        Dim vOuty() As Byte = BitConverter.GetBytes(CInt((textura.Top * (7168 / Form1.Panel1.Height)) - 3584))
        Array.Copy(vOuty, 0, Form1.unzlibfile, offsetX + 12, 2)
        Dim vOutw() As Byte = BitConverter.GetBytes(CInt(textura.Width * (8192 / Form1.Panel1.Width)))
        Array.Copy(vOutw, 0, Form1.unzlibfile, offsetX + 36, 2)
        Array.Copy(vOutw, 0, Form1.unzlibfile, offsetX + 42, 2)
        Dim vOuth() As Byte = BitConverter.GetBytes(CInt(textura.Height * (7168 / Form1.Panel1.Height)))
        Array.Copy(vOuth, 0, Form1.unzlibfile, offsetX + 26, 2)
        Array.Copy(vOuth, 0, Form1.unzlibfile, offsetX + 38, 2)


        If mapeo Is Nothing Then

        Else
            Dim UVx() As Byte = BitConverter.GetBytes(CInt(mapeo.Left * 8))
            Dim UVw() As Byte = BitConverter.GetBytes(CInt((mapeo.Width + mapeo.Left) * 8))
            Dim UVy() As Byte = BitConverter.GetBytes(CInt(mapeo.Top * 8))
            Dim UVh() As Byte = BitConverter.GetBytes(CInt((mapeo.Height + mapeo.Top) * 8))


            'Mapeo scr
            If invertir Is Nothing Then
                Array.Copy(UVx, 0, Form1.unzlibfile, offsetX + 48, 2)
                Array.Copy(UVx, 0, Form1.unzlibfile, offsetX + 52, 2)
                Array.Copy(UVw, 0, Form1.unzlibfile, offsetX + 56, 2)
                Array.Copy(UVw, 0, Form1.unzlibfile, offsetX + 60, 2)
            Else
                Select Case invertir.CheckState
                    Case CheckState.Checked
                        Array.Copy(UVx, 0, Form1.unzlibfile, offsetX + 56, 2)
                        Array.Copy(UVx, 0, Form1.unzlibfile, offsetX + 60, 2)
                        Array.Copy(UVw, 0, Form1.unzlibfile, offsetX + 48, 2)
                        Array.Copy(UVw, 0, Form1.unzlibfile, offsetX + 52, 2)
                    Case CheckState.Unchecked
                        Array.Copy(UVx, 0, Form1.unzlibfile, offsetX + 48, 2)
                        Array.Copy(UVx, 0, Form1.unzlibfile, offsetX + 52, 2)
                        Array.Copy(UVw, 0, Form1.unzlibfile, offsetX + 56, 2)
                        Array.Copy(UVw, 0, Form1.unzlibfile, offsetX + 60, 2)
                End Select
            End If
            Array.Copy(UVy, 0, Form1.unzlibfile, offsetX + 54, 2)
            Array.Copy(UVy, 0, Form1.unzlibfile, offsetX + 62, 2)
            Array.Copy(UVh, 0, Form1.unzlibfile, offsetX + 50, 2)
            Array.Copy(UVh, 0, Form1.unzlibfile, offsetX + 58, 2)
        End If
        Return False
    End Function
    Public Function obj_Textura4Read(ByVal offsetX As Integer, ByVal textura As PictureBox, ByVal mapeo As PictureBox, ByVal invertir As CheckBox)
        offsetX -= 10
        textura.Left = (BitConverter.ToInt16(Form1.unzlibfile, offsetX + 10) + 4096) / (8192 / Form1.Panel1.Width)
        textura.Top = (BitConverter.ToInt16(Form1.unzlibfile, offsetX + 12) + 3584) / (7168 / Form1.Panel1.Height)
        textura.Width = BitConverter.ToInt16(Form1.unzlibfile, offsetX + 36) / (8192 / Form1.Panel1.Width)
        textura.Height = BitConverter.ToInt16(Form1.unzlibfile, offsetX + 26) / (7168 / Form1.Panel1.Height)

        If mapeo Is Nothing Then
        Else
            'Mapeo Textura
            Dim MapeoX As Integer = BitConverter.ToInt16(Form1.unzlibfile, offsetX + 48) / 8
            Dim MapeoAN As Integer = BitConverter.ToInt16(Form1.unzlibfile, offsetX + 56) / 8

            mapeo.Top = BitConverter.ToInt16(Form1.unzlibfile, offsetX + 54) / 8
            mapeo.Height = BitConverter.ToInt16(Form1.unzlibfile, offsetX + 50) / 8 - mapeo.Top

            If MapeoX > MapeoAN Then 'Invertir
                mapeo.Left = MapeoAN
                mapeo.Width = MapeoX - MapeoAN
                If invertir Is Nothing Then
                Else
                    invertir.Checked = True
                End If
            Else
                mapeo.Left = MapeoX
                mapeo.Width = MapeoAN - MapeoX
                If invertir Is Nothing Then
                Else
                    invertir.Checked = False
                End If
            End If


        End If

        Return False
    End Function
    'Textos
    Public Function obj_TextoWrite(ByVal offsetX As Integer, ByVal logo As PictureBox, ByVal tamaño As ComboBox, ByVal color As ComboBox)
        offsetX -= 4
        Dim vOutx() As Byte = BitConverter.GetBytes(CInt((logo.Left * (8192 / Form1.Panel1.Width)) - 4096))
        Array.Copy(vOutx, 0, Form1.unzlibfile, offsetX + 4, 2)
        Dim vOuty() As Byte = BitConverter.GetBytes(CInt((logo.Top * (7168 / Form1.Panel1.Height)) - 3584))
        Array.Copy(vOuty, 0, Form1.unzlibfile, offsetX + 6, 2)
        Dim vOutw() As Byte = BitConverter.GetBytes(CInt(logo.Width * (8192 / Form1.Panel1.Width)))
        Array.Copy(vOutw, 0, Form1.unzlibfile, offsetX + 10, 2)
        Dim vOuth() As Byte = BitConverter.GetBytes(CInt(logo.Height * (7168 / Form1.Panel1.Height)))
        Array.Copy(vOuth, 0, Form1.unzlibfile, offsetX + 12, 2)

        If tamaño.SelectedIndex = 0 Then Form1.unzlibfile(offsetX + 1) = Byte.Parse(0)
        If tamaño.SelectedIndex = 1 Then Form1.unzlibfile(offsetX + 1) = Byte.Parse(1)

        If color.SelectedIndex = 0 Then Form1.unzlibfile(offsetX + 15) = Byte.Parse(0)
        If color.SelectedIndex = 1 Then Form1.unzlibfile(offsetX + 15) = Byte.Parse(1)
        Return False
    End Function
    Public Function obj_TextoRead(ByVal offsetX As Integer, ByVal Texto As PictureBox, ByVal tamaño As ComboBox, ByVal color As ComboBox)
        offsetX -= 4

        Dim value(4) As Byte

        Texto.Left = (BitConverter.ToInt16(Form1.unzlibfile, offsetX + 4) + 4096) / (8192 / Form1.Panel1.Width)
        Texto.Top = (BitConverter.ToInt16(Form1.unzlibfile, offsetX + 6) + 3584) / (7168 / Form1.Panel1.Height)
        Texto.Width = BitConverter.ToInt16(Form1.unzlibfile, offsetX + 10) / (8192 / Form1.Panel1.Width)
        Texto.Height = BitConverter.ToInt16(Form1.unzlibfile, offsetX + 12) / (7168 / Form1.Panel1.Height)


        value = {0, 0, 0, 0}
        Array.Copy(Form1.unzlibfile, offsetX + 1, value, 0, 1)
        If BitConverter.ToInt16(value, 0) = 0 Then tamaño.SelectedIndex = 0
        If BitConverter.ToInt16(value, 0) = 1 Then tamaño.SelectedIndex = 1

        value = {0, 0, 0, 0}
        Array.Copy(Form1.unzlibfile, offsetX + 15, value, 0, 1)
        If BitConverter.ToInt16(value, 0) Then color.SelectedIndex = 0
        If BitConverter.ToInt16(value, 0) Then color.SelectedIndex = 1

        Return False
    End Function

End Module
