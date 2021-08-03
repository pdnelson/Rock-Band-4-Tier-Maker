Imports System.IO
Imports System.Reflection
Imports System.Drawing.Text
Imports System.Runtime.InteropServices
Imports System.Drawing.Printing

Public Class frmRB4Tier

    Dim bass(6) As PictureBox
    Dim guitar(6) As PictureBox
    Dim vocal(6) As PictureBox
    Dim drum(6) As PictureBox
    Dim albumArt As Bitmap
    Dim fontSize(3) As Integer
    Dim CanSaveSettings As Boolean
    Dim FontsArePresent As Boolean
    Dim DefaultFontSize As Integer

    Private Sub frmRB4Tier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Check for presence of settings file/directory
        CanSaveSettings = CheckForSettingsFile()
        FontsArePresent = CheckForFonts()

        'initialize the arrays of images
        bass = {picNoBass, picBassDevil, picBass5, picBass4, picBass3, picBass2, picBass1}
        guitar = {picNoGuitar, picGuitarDevil, picGuitar5, picGuitar4, picGuitar3, picGuitar2, picGuitar1}
        vocal = {picNoVocals, picVocDevil, picVoc5, picVoc4, picVoc3, picVoc2, picVoc1}
        drum = {picNoDrums, picDrumDevil, picDrum5, picDrum4, picDrum3, picDrum2, picDrum1}

        'defaults genre drop-down to "other"
        drpGenre.SelectedIndex = -1

        'this disables the preview if the resolution is too small
        If (Screen.PrimaryScreen.Bounds.Width < 1439) Then
            mnuShow.Enabled = False
            saveToFile("Sys\Settings\view.txt", "hide")
        End If

        'checking if the user last had hidden checked or not
        Dim show As String = ""

        If CanSaveSettings Then
            show = My.Computer.FileSystem.ReadAllText("Sys\Settings\view.txt")
        End If

        If (show.Contains("hide")) Then
            Me.Size = New Size(789, 329)
            mnuHide.Checked = True
            mnuShow.Checked = False
            lblBorderTop.Visible = False
        Else
            mnuShow.Checked = True
        End If

        'sets image to a default value
        albumArt = My.Resources.nopic

        Dim font As Font
        Dim fontBold As Font

        If FontsArePresent Then
            DefaultFontSize = 32

            Dim privateFonts As New PrivateFontCollection()
            Dim privateFontsB As New PrivateFontCollection()

            privateFonts.AddFontFile("Sys\Fonts\realfont.ttf")
            privateFontsB.AddFontFile("Sys\Fonts\realfontbold.ttf")

            font = New Font(privateFonts.Families(0), DefaultFontSize)
            fontBold = New Font(privateFontsB.Families(0), DefaultFontSize, FontStyle.Bold)
        Else
            DefaultFontSize = 28

            font = New Font(lblAlbum.Font.FontFamily, DefaultFontSize)
            fontBold = New Font(lblTitle.Font.FontFamily, DefaultFontSize, FontStyle.Bold)
        End If

        lblTitle.Font = fontBold
        lblAlbum.Font = font
        lblGenreT.Font = font
        lblYear.Font = font

        fontBold.Dispose()
        font.Dispose()

    End Sub

    '----difficulty modifiers----

    'GUITAR TRACKBAR DRAG
    Private Sub trkGuitar_Scroll(sender As Object, e As EventArgs) Handles trkGuitar.Scroll
        ChangeDifficulty(trkGuitar.Value, guitar)
    End Sub

    'GUITAR TRACKBAR CLICK
    Private Sub trkGuitar_MouseDown(sender As Object, e As MouseEventArgs) Handles trkGuitar.MouseDown

        'jumps to the clicked location on the trackbar
        Dim dblValue As Double = (Convert.ToDouble(e.X) / Convert.ToDouble(trkGuitar.Width)) * (trkGuitar.Maximum - trkGuitar.Minimum)
        trkGuitar.Value = Convert.ToInt32(dblValue)

        'updates the preview
        ChangeDifficulty(trkGuitar.Value, guitar)
    End Sub

    'DRUMS TRACKBAR DRAG
    Private Sub trkDrums_Scroll(sender As Object, e As EventArgs) Handles trkDrums.Scroll
        ChangeDifficulty(trkDrums.Value, drum)
    End Sub

    'DRUMS TRACKBAR CLICK
    Private Sub trkDrums_MouseDown(sender As Object, e As MouseEventArgs) Handles trkDrums.MouseDown

        'jumps to the clicked location on the trackbar
        Dim dblValue As Double = (Convert.ToDouble(e.X) / Convert.ToDouble(trkDrums.Width)) * (trkDrums.Maximum - trkDrums.Minimum)
        trkDrums.Value = Convert.ToInt32(dblValue)

        'updates the preview
        ChangeDifficulty(trkDrums.Value, drum)
    End Sub

    'VOCAL TRACKBAR DRAG
    Private Sub trkVocals_Scroll(sender As Object, e As EventArgs) Handles trkVocals.Scroll
        ChangeDifficulty(trkVocals.Value, vocal)
    End Sub

    'VOCAL TRACKBAR CLICK
    Private Sub trkVocals_MouseDown(sender As Object, e As MouseEventArgs) Handles trkVocals.MouseDown

        'jumps to the clicked location
        Dim dblValue As Double = (Convert.ToDouble(e.X) / Convert.ToDouble(trkVocals.Width)) * (trkVocals.Maximum - trkVocals.Minimum)
        trkVocals.Value = Convert.ToInt32(dblValue)

        'updates the preview
        ChangeDifficulty(trkVocals.Value, vocal)
    End Sub

    'BASS TRACKBAR DRAG
    Private Sub trkBass_Scroll(sender As Object, e As EventArgs) Handles trkBass.Scroll
        ChangeDifficulty(trkBass.Value, bass)
    End Sub

    'BASS TRACKBAR CLICK
    Private Sub trkBass_MouseDown(sender As Object, e As MouseEventArgs) Handles trkBass.MouseDown

        'jumps to the clicked location on the trackbar
        Dim dblValue As Double = (Convert.ToDouble(e.X) / Convert.ToDouble(trkBass.Width)) * (trkBass.Maximum - trkBass.Minimum)
        trkBass.Value = Convert.ToInt32(dblValue)

        'updates the preview
        ChangeDifficulty(trkBass.Value, bass)
    End Sub


    '----song info modifiers----

    'SONG TITLE TEXTBOX
    Private Sub txtTitle_TextChanged(sender As Object, e As EventArgs) Handles txtTitle.TextChanged
        ChangeText(txtTitle.Text, lblTitle, "[Title]")
    End Sub

    'ARTIST TEXTBOX
    Private Sub txtArtist_TextChanged(sender As Object, e As EventArgs) Handles txtArtist.TextChanged
        ChangeText(txtArtist.Text, lblAlbum, "[Artist]")
    End Sub

    'GENRE DROP-DOWN
    Private Sub drpGenre_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpGenre.SelectedIndexChanged
        ChangeText(drpGenre.Text, lblGenreT, "[Genre]")
    End Sub

    'YEAR TEXTBOX
    Private Sub txtYear_TextChanged(sender As Object, e As EventArgs) Handles txtYear.TextChanged
        ChangeText(txtYear.Text, lblYear, "[Year]")
    End Sub

    'SEARCH FOR ALBUM ART
    Private Sub btnArt_Click(sender As Object, e As EventArgs) Handles btnArt.Click


        'creates a dialog box for browsing for album cover art
        Dim fd As OpenFileDialog = New OpenFileDialog()
        Dim strFileName As String
        fd.Title = "Browse for Album Cover Art"
        fd.InitialDirectory = "Libraries\Pictures"
        fd.Filter = "Image Files|*.png;*.jpg;*.bmp;*.gif|PNG|*.png|JPG|*.jpg|BMP|*.bmp|GIF|*.gif"
        fd.FilterIndex = 1
        fd.RestoreDirectory = True

        'adds the album cover art using the directory pulled from the dialog box
        If fd.ShowDialog() = DialogResult.OK Then
            strFileName = fd.FileName
            Dim cover As Image = Image.FromFile(strFileName)

            'dispose of old variables before assigning new
            albumArt.Dispose()

            'adds the image to the large preview
            picCover.Image = New Bitmap(cover, New Size(283, 283))
            picCover.Visible = True

            'adds the image to the small preview
            picPreview.Image = New Bitmap(cover, New Size(94, 94))


            'adds the image to the global variable
            albumArt = New Bitmap(cover, New Size(283, 283))

            'dispose of any temporary images
            cover.Dispose()
        End If


    End Sub


    '----menu bar options----

    'SAVE TO FILE
    Private Sub ExportToImageFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles mnuExport.Click

        'opens a dialog box to browse for a location to save the image file
        Dim fd As SaveFileDialog = New SaveFileDialog()
        Dim strFileName As String
        fd.Title = "Save Tier"
        fd.Filter = "PNG|*.png|JPG|*.jpg|BMP|*.bmp|GIF|*.gif"
        fd.FilterIndex = 1
        fd.FileName = lblAlbum.Text + " - " + lblTitle.Text + " Tier"
        fd.RestoreDirectory = True

        'saves the image
        If fd.ShowDialog() = DialogResult.OK Then
            Dim saveImage As New Bitmap(GenerateImage())
            strFileName = fd.FileName
            saveImage.Save(strFileName, Imaging.ImageFormat.Png)

            'dispose of temporary images so they don't hog memory
            SaveImage.Dispose()
        End If

    End Sub

    'COPY TO CLIPBOARD
    Private Sub CopyToClipboardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles mnuCopy.Click

        'temporary image variable
        Dim saveImage As New Bitmap(GenerateImage())

        'sets clipboard contents to the GenerateImage() output
        My.Computer.Clipboard.SetImage(saveImage)

        'dispose the image so it doesn't hog memory
        saveImage.Dispose()

    End Sub

    'CLEAR ALL
    Private Sub ClearToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles mnuClear.Click

        'clear the fields
        txtArtist.Text = ""
        txtTitle.Text = ""
        txtYear.Text = ""
        drpGenre.SelectedIndex = -1
        trkGuitar.Value = 0
        trkDrums.Value = 0
        trkVocals.Value = 0
        trkBass.Value = 0

        'Set all instrument displays back to "no instrument"
        For i As Integer = 0 To 6 Step 1
            If (i = 0) Then
                bass(i).Visible = True
                drum(i).Visible = True
                vocal(i).Visible = True
                guitar(i).Visible = True
            Else
                bass(i).Visible = False
                drum(i).Visible = False
                vocal(i).Visible = False
                guitar(i).Visible = False
            End If
        Next

        'clear images
        picCover.Visible = False
        picPreview.Image = My.Resources.artpreview
        albumArt = My.Resources.nopic

        'sets cursor back to the beginning
        txtTitle.Select()
    End Sub

    'HIDE PREVIEW
    Private Sub mnuHide_Click(sender As Object, e As EventArgs) Handles mnuHide.Click

        'sets checkboxes, then size
        lblBorderTop.Visible = False
        mnuHide.Checked = True
        mnuShow.Checked = False
        Me.Size = New Size(789, 329)

        'saves the user's setting
        saveToFile("Sys\Settings\view.txt", "hide")
    End Sub

    'SHOW PREVIEW
    Private Sub mnuShow_Click(sender As Object, e As EventArgs) Handles mnuShow.Click

        'sets checkboxes, then size
        lblBorderTop.Visible = True
        mnuHide.Checked = False
        mnuShow.Checked = True
        Me.Size = New Size(1439, 661)

        'saves the user's setting
        saveToFile("Sys\Settings\view.txt", "show")
    End Sub

    'ABOUT
    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles mnuAbout.Click
        MessageBox.Show("Rock Band 4 Tier Maker is a free, open source program designed in 2019 by Patrick Nelson (BetaMaster64)." + vbCrLf + vbCrLf + "Version 1.3" + vbCrLf + vbCrLf + "Found a bug? Shoot me an email at supermariokart98@gmail.com!", "About")
    End Sub


    '----other functions----

    ''' <summary>
    ''' Saves data to a file. This is used for saving the user's settings.
    ''' </summary>
    ''' <param name="path">The path to save to.</param>
    ''' <param name="data">The data being saved to the file.</param>
    Private Sub SaveToFile(path As String, data As String)

        If CanSaveSettings Then
            'opens new StreamWriter, setting append to False
            Dim file As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(path, False)

            'write file
            file.WriteLine(data)
            file.Close()
        End If
    End Sub

    ''' <summary>
    ''' Changes the difficulty trackbars.
    ''' </summary>
    ''' <param name="difficulty">Difficulty value from 0 to 7.</param>
    ''' <param name="p">Images corresponding to the trackbar.</param>
    Private Sub ChangeDifficulty(difficulty As Integer, p As PictureBox())

        'no instrument
        If difficulty = 0 Then
            p(0).Visible = True
            For i As Integer = 1 To 6
                p(i).Visible = False
            Next

            'easy peasy
        ElseIf difficulty = 1 Then
            For i As Integer = 0 To 6
                p(i).Visible = False
            Next

            '1 dot
        ElseIf difficulty = 2 Then
            p(6).Visible = True
            For i As Integer = 0 To 5
                p(i).Visible = False
            Next

            '2 dots
        ElseIf difficulty = 3 Then
            p(0).Visible = False
            p(1).Visible = False
            p(2).Visible = False
            p(3).Visible = False
            p(4).Visible = False
            p(5).Visible = True
            p(6).Visible = True

            '3 dots
        ElseIf difficulty = 4 Then
            p(0).Visible = False
            p(1).Visible = False
            p(2).Visible = False
            p(3).Visible = False
            p(4).Visible = True
            p(5).Visible = True
            p(6).Visible = True

            '4 dots
        ElseIf difficulty = 5 Then
            p(0).Visible = False
            p(1).Visible = False
            p(2).Visible = False
            p(3).Visible = True
            p(4).Visible = True
            p(5).Visible = True
            p(6).Visible = True

            '5 dots
        ElseIf difficulty = 6 Then
            p(0).Visible = False
            p(1).Visible = False
            p(2).Visible = True
            p(3).Visible = True
            p(4).Visible = True
            p(5).Visible = True
            p(6).Visible = True

            'devils
        ElseIf difficulty = 7 Then
            p(0).Visible = False
            p(1).Visible = True
            p(2).Visible = False
            p(3).Visible = False
            p(4).Visible = False
            p(5).Visible = False
            p(6).Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Compares specified text against a specified label's text, and updates the label
    ''' accordingly.
    ''' </summary>
    ''' <param name="newText">The new text that the user just typed.</param>
    ''' <param name="label">The label that should be modified.</param>
    ''' <param name="defaultText"></param>
    Private Sub ChangeText(newText As String, label As Label, defaultText As String)

        'determines if text is being removed or added
        Dim textAdded As Boolean = False
        If (Text.Length() < label.Text.Length()) Then
            textAdded = False
        Else
            textAdded = True
        End If

        'if the textbox is empty, it will return to the default text
        If (newText.Equals("")) Then
            label.Text = defaultText

            'sets the font size back to what it should be
            Using font As Font = New Font(label.Font.FontFamily, DefaultFontSize, label.Font.Style)
                label.Font = font
            End Using

            'else, update the label
        Else
            label.Text = newText

            'if there is too much text, make the font smaller so that it fits
            While (label.Width() > (My.Resources.blank.Width - 330) And textAdded = True)
                Using font As Font = New Font(label.Font.FontFamily, (label.Font.Size - 1), label.Font.Style)
                    label.Font = font
                End Using
            End While

            'while the user is backspacing, makes the font larger to fit the maximum width
            While (label.Width() < (My.Resources.blank.Width - 330) And label.Font.Size < DefaultFontSize And textAdded = False)
                Using font As Font = New Font(label.Font.FontFamily, (label.Font.Size + 1), label.Font.Style)
                    label.Font = font
                End Using
            End While

            'final buffer for backspacing
            While label.Width() > (My.Resources.blank.Width - 330) And textAdded = False
                Using font As Font = New Font(label.Font.FontFamily, label.Font.Size - 1, label.Font.Style)
                    label.Font = font
                End Using
            End While

        End If

        lblAlbum.Location = New Point(lblAlbum.Location.X, lblTitle.Location.Y + lblTitle.Height + 6)
        lblGenreT.Location = New Point(lblGenreT.Location.X, lblAlbum.Location.Y + lblAlbum.Height + 9)
        lblYear.Location = New Point(lblYear.Location.X, lblGenreT.Location.Y + lblGenreT.Height + 11)
    End Sub

    ''' <summary>
    ''' Generates an image that the user will be able to copy or save.
    ''' </summary>
    ''' <returns>Bitmap image.</returns>
    Function GenerateImage() As Bitmap

        Dim final As Bitmap = My.Resources.blank

        'adds the text
        Using finalEdit As Graphics = Graphics.FromImage(final)

            'draws each of the song info lines with the font size used in the preview
            Using title As Font = New Font(lblTitle.Font.FontFamily, lblTitle.Font.Size, FontStyle.Bold)
                finalEdit.DrawString(lblTitle.Text, title, Brushes.White, 320, 17)
            End Using

            Using artist As Font = New Font(lblAlbum.Font.FontFamily, lblAlbum.Font.Size, FontStyle.Regular)
                finalEdit.DrawString(lblAlbum.Text, artist, Brushes.White, 320, (17 + lblTitle.Height + 6))
            End Using

            Using genre As Font = New Font(lblGenreT.Font.FontFamily, lblGenreT.Font.Size, FontStyle.Regular)
                finalEdit.DrawString(lblGenreT.Text, genre, Brushes.White, 320, (17 + lblTitle.Height + 6 + lblAlbum.Height + 9))
            End Using

            Using year As Font = New Font(lblYear.Font.FontFamily, lblYear.Font.Size, FontStyle.Regular)
                finalEdit.DrawString(lblYear.Text, year, Brushes.White, 320, (17 + lblTitle.Height + 6 + lblAlbum.Height + 9 + lblGenreT.Height + 11))
            End Using
        End Using

        'adds the images
        Using finalEdit = Graphics.FromImage(final)

            'first lays down the template
            finalEdit.DrawImage(albumArt, 16, 15)

            '--places guitar difficulty based on trackbar value
            'no instrument
            If (trkGuitar.Value = 0) Then
                finalEdit.DrawImage(My.Resources.no_instrument1, 380, 234)

                'devils
            ElseIf (trkGuitar.Value = 7) Then
                finalEdit.DrawImage(My.Resources.devils1, 383, 238)

                'dots
            ElseIf (trkGuitar.Value > 1) Then
                For i As Integer = 383 To (383 + (trkGuitar.Value - 2) * 40) Step 40
                    finalEdit.DrawImage(My.Resources.dot2, i, 239)
                Next
            End If

            '--places drums difficulty based on trackbar value
            'no instrument
            If (trkDrums.Value = 0) Then
                finalEdit.DrawImage(My.Resources.no_instrument1, 662, 234)

                'devils
            ElseIf (trkDrums.Value = 7) Then
                finalEdit.DrawImage(My.Resources.devils1, 665, 238)

                'dots
            ElseIf (trkDrums.Value > 1) Then
                For i As Integer = 667 To (667 + (trkDrums.Value - 2) * 40) Step 40
                    finalEdit.DrawImage(My.Resources.dot2, i, 239)
                Next
            End If


            '--places vocals difficulty based on trackbar value
            'no instrument
            If (trkVocals.Value = 0) Then
                finalEdit.DrawImage(My.Resources.no_instrument1, 938, 234)

                'devils
            ElseIf (trkVocals.Value = 7) Then
                finalEdit.DrawImage(My.Resources.devils1, 941, 238)

                'dots
            ElseIf (trkVocals.Value > 1) Then
                For i As Integer = 943 To (943 + (trkVocals.Value - 2) * 40) Step 40
                    finalEdit.DrawImage(My.Resources.dot2, i, 239)
                Next
            End If

            '--places bass difficulty based on trackbar value
            'no instrument
            If (trkBass.Value = 0) Then
                finalEdit.DrawImage(My.Resources.no_instrument1, 1220, 234)

                'devils
            ElseIf (trkBass.Value = 7) Then
                finalEdit.DrawImage(My.Resources.devils1, 1223, 238)

                'dots
            ElseIf (trkBass.Value > 1) Then
                For i As Integer = 1227 To (1227 + (trkBass.Value - 2) * 40) Step 40
                    finalEdit.DrawImage(My.Resources.dot2, i, 239)
                Next
            End If
        End Using

        Return final
    End Function

    ''' <summary>
    ''' Checks to see if the settings file is present.
    ''' </summary>
    ''' <returns>True if settings can be saved, False otherwise.</returns>
    Function CheckForSettingsFile() As Boolean
        Try
            My.Computer.FileSystem.ReadAllBytes("Sys\Settings\view.txt")
        Catch e As Exception
            MessageBox.Show("The settings file could not be found! Please verify that you are running the program in the same directory as the Sys folder, and that all of that folder's contents are present. You will still be able to use the program, but your view settings will not be saved.", "Error Finding Settings File")
            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' Checks to see if both font files are present (bold and regular).
    ''' </summary>
    ''' <returns>True if the fonts were found, False otherwise.</returns>
    Function CheckForFonts() As Boolean
        Try
            My.Computer.FileSystem.ReadAllBytes("Sys\Fonts\realfont.ttf")
            My.Computer.FileSystem.ReadAllBytes("Sys\Fonts\realfontbold.ttf")
        Catch e As Exception
            MessageBox.Show("One or both of the font files could not be found! Please verify that you are running the program in the same directory as the Sys folder, and that all of that folder's contents are present. You will still be able to use the program, but some things may not appear correctly.", "Error Finding Font Files")
            Return False
        End Try
        Return False
    End Function

End Class
