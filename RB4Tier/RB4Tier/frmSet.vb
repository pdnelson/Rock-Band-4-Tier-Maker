Imports System.IO
Imports System.Reflection
Imports System.Drawing.Text
Imports System.Runtime.InteropServices
Imports System.Drawing.Printing

Public Class frmRB4Tier

    'GLOBAL VARIABLES
    Dim bass(6) As PictureBox
    Dim guitar(6) As PictureBox
    Dim vocal(6) As PictureBox
    Dim drum(6) As PictureBox
    Dim albumArt As Bitmap
    Dim privateFonts As New PrivateFontCollection()
    Dim privateFontsB As New PrivateFontCollection()

    'FORM LOAD
    Private Sub frmRB4Tier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        Dim show As String
        show = My.Computer.FileSystem.ReadAllText("Sys\Settings\view.txt")
        If (show.Equals("hide" + vbCrLf)) Then
            Me.Size = New Size(589, 329)
            mnuHide.Checked = True
            mnuShow.Checked = False
            lblBorderTop.Visible = False
        Else
            mnuShow.Checked = True
        End If

        'sets image to a default value
        albumArt = My.Resources.nopic

        'opens the font files from the system folder
        privateFonts.AddFontFile("Sys\Fonts\realfont.ttf")
        privateFontsB.AddFontFile("Sys\Fonts\realfontbold.ttf")
        Dim fnt As New System.Drawing.Font(privateFonts.Families(0), 32)
        Dim fntB As New System.Drawing.Font(privateFontsB.Families(0), 32, FontStyle.Bold)

        'sets the labels to the font
        lblTitle.Font = fntB
        lblAlbum.Font = fnt
        lblGenreT.Font = fnt
        lblYear.Font = fnt

        'dispose temporary fonts so they don't hog memory
        fntB.Dispose()
        fnt.Dispose()

    End Sub

    '----difficulty modifiers----

    'GUITAR TRACKBAR DRAG
    Private Sub trkGuitar_Scroll(sender As Object, e As EventArgs) Handles trkGuitar.Scroll
        changeDiff(trkGuitar, guitar)
    End Sub

    'GUITAR TRACKBAR CLICK
    Private Sub trkGuitar_MouseDown(sender As Object, e As MouseEventArgs) Handles trkGuitar.MouseDown

        'jumps to the clicked location on the trackbar
        Dim dblValue As Double = (Convert.ToDouble(e.X) / Convert.ToDouble(trkGuitar.Width)) * (trkGuitar.Maximum - trkGuitar.Minimum)
        trkGuitar.Value = Convert.ToInt32(dblValue)

        'updates the preview
        changeDiff(trkGuitar, guitar)
    End Sub

    'DRUMS TRACKBAR DRAG
    Private Sub trkDrums_Scroll(sender As Object, e As EventArgs) Handles trkDrums.Scroll
        changeDiff(trkDrums, drum)
    End Sub

    'DRUMS TRACKBAR CLICK
    Private Sub trkDrums_MouseDown(sender As Object, e As MouseEventArgs) Handles trkDrums.MouseDown

        'jumps to the clicked location on the trackbar
        Dim dblValue As Double = (Convert.ToDouble(e.X) / Convert.ToDouble(trkDrums.Width)) * (trkDrums.Maximum - trkDrums.Minimum)
        trkDrums.Value = Convert.ToInt32(dblValue)

        'updates the preview
        changeDiff(trkDrums, drum)
    End Sub

    'VOCAL TRACKBAR DRAG
    Private Sub trkVocals_Scroll(sender As Object, e As EventArgs) Handles trkVocals.Scroll
        changeDiff(trkVocals, vocal)
    End Sub

    'VOCAL TRACKBAR CLICK
    Private Sub trkVocals_MouseDown(sender As Object, e As MouseEventArgs) Handles trkVocals.MouseDown

        'jumps to the clicked location
        Dim dblValue As Double = (Convert.ToDouble(e.X) / Convert.ToDouble(trkVocals.Width)) * (trkVocals.Maximum - trkVocals.Minimum)
        trkVocals.Value = Convert.ToInt32(dblValue)

        'updates the preview
        changeDiff(trkVocals, vocal)
    End Sub

    'BASS TRACKBAR DRAG
    Private Sub trkBass_Scroll(sender As Object, e As EventArgs) Handles trkBass.Scroll
        changeDiff(trkBass, bass)
    End Sub

    'BASS TRACKBAR CLICK
    Private Sub trkBass_MouseDown(sender As Object, e As MouseEventArgs) Handles trkBass.MouseDown

        'jumps to the clicked location on the trackbar
        Dim dblValue As Double = (Convert.ToDouble(e.X) / Convert.ToDouble(trkBass.Width)) * (trkBass.Maximum - trkBass.Minimum)
        trkBass.Value = Convert.ToInt32(dblValue)

        'updates the preview
        changeDiff(trkBass, bass)
    End Sub


    '----song info modifiers----

    'SONG TITLE TEXTBOX
    Private Sub txtTitle_TextChanged(sender As Object, e As EventArgs) Handles txtTitle.TextChanged

        'if the textbox is empty, it will return to the default text
        If (txtTitle.Text.Equals("")) Then
            lblTitle.Text = "[Title]"

            'else, update the label
        Else
            lblTitle.Text = txtTitle.Text
        End If
    End Sub

    'ARTIST TEXTBOX
    Private Sub txtArtist_TextChanged(sender As Object, e As EventArgs) Handles txtArtist.TextChanged

        'if the textbox is empty, it will return to the default text
        If (txtArtist.Text.Equals("")) Then
            lblAlbum.Text = "[Artist]"

            'else, update the label
        Else
            lblAlbum.Text = txtArtist.Text
        End If
    End Sub

    'GENRE DROP-DOWN
    Private Sub drpGenre_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpGenre.SelectedIndexChanged

        'if the textbox is empty, it will use this unused text
        If (drpGenre.Text.Equals("")) Then
            lblGenreT.Text = "[Genre]"

            'else, update the label
        Else
            lblGenreT.Text = drpGenre.Text
        End If
    End Sub

    'YEAR TEXTBOX
    Private Sub txtYear_TextChanged(sender As Object, e As EventArgs) Handles txtYear.TextChanged

        'if the textbox is empty, it will return to the default text
        If (txtYear.Text.Equals("")) Then
            lblYear.Text = "[Year]"

            'else, update the label
        Else
            lblYear.Text = txtYear.Text
        End If
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
            Dim saveImage As New Bitmap(generateImage())
            strFileName = fd.FileName
            saveImage.Save(strFileName, Imaging.ImageFormat.Png)

            'dispose of temporary images so they don't hog memory
            SaveImage.Dispose()
        End If

    End Sub

    'COPY TO CLIPBOARD
    Private Sub CopyToClipboardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles mnuCopy.Click

        'temporary image variable
        Dim saveImage As New Bitmap(generateImage())

        'sets clipboard contents to the generateImage() output
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

        'set guitar back to "no instrument"
        picNoGuitar.Visible = True
        picGuitarDevil.Visible = False
        picGuitar5.Visible = False
        picGuitar4.Visible = False
        picGuitar3.Visible = False
        picGuitar2.Visible = False
        picGuitar1.Visible = False

        'set drums back to "no instrument"
        picNoDrums.Visible = True
        picDrumDevil.Visible = False
        picDrum5.Visible = False
        picDrum4.Visible = False
        picDrum3.Visible = False
        picDrum2.Visible = False
        picDrum1.Visible = False

        'set vocals back to "no instrument"
        picNoVocals.Visible = True
        picVocDevil.Visible = False
        picVoc5.Visible = False
        picVoc4.Visible = False
        picVoc3.Visible = False
        picVoc2.Visible = False
        picVoc1.Visible = False

        'set bass back to "no instrument"
        picNoBass.Visible = True
        picBassDevil.Visible = False
        picBass5.Visible = False
        picBass4.Visible = False
        picBass3.Visible = False
        picBass2.Visible = False
        picBass1.Visible = False

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
        Me.Size = New Size(589, 329)

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
        MessageBox.Show("Rock Band 4 Tier Maker is a free, open source program designed in 2019 by Patrick Nelson (BetaMaster64)." + vbCrLf + vbCrLf + "Version 1.0" + vbCrLf + vbCrLf + "Found a bug? Shoot me an email at supermariokart98@gmail.com!", "About")
    End Sub


    '----other functions----

    'SAVE SETTINGS
    'takes in parameters "path" and "condition"
    'path will be the directory/file name you are saving to
    'condition will be the setting that you are saving
    Private Sub saveToFile(path As String, condition As String)

        'opens new StreamWriter, setting append to False
        Dim file As StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(path, False)

        'write file
        file.WriteLine(condition)
        file.Close()
    End Sub

    'CHANGE DIFFICULTY PREVIEW UPDATE
    'takes in parameters "TrackBar" t and "PictureBox()" p (array)
    'TrackBar t is the trackbar that is being used
    'PictureBox() p is an array of preview images corresponding to the passed trackbar
    Private Sub changeDiff(t As TrackBar, p As PictureBox())

        'no instrument
        If t.Value = 0 Then
            p(0).Visible = True
            For i As Integer = 1 To 6
                p(i).Visible = False
            Next

            'easy peasy
        ElseIf t.Value = 1 Then
            For i As Integer = 0 To 6
                p(i).Visible = False
            Next

            '1 dot
        ElseIf t.Value = 2 Then
            p(6).Visible = True
            For i As Integer = 0 To 5
                p(i).Visible = False
            Next

            '2 dots
        ElseIf t.Value = 3 Then
                p(0).Visible = False
                p(1).Visible = False
                p(2).Visible = False
                p(3).Visible = False
                p(4).Visible = False
                p(5).Visible = True
                p(6).Visible = True

            '3 dots
        ElseIf t.Value = 4 Then
                p(0).Visible = False
                p(1).Visible = False
                p(2).Visible = False
                p(3).Visible = False
                p(4).Visible = True
                p(5).Visible = True
                p(6).Visible = True

            '4 dots
        ElseIf t.Value = 5 Then
                p(0).Visible = False
                p(1).Visible = False
                p(2).Visible = False
                p(3).Visible = True
                p(4).Visible = True
                p(5).Visible = True
                p(6).Visible = True

            '5 dots
        ElseIf t.Value = 6 Then
                p(0).Visible = False
                p(1).Visible = False
                p(2).Visible = True
                p(3).Visible = True
                p(4).Visible = True
                p(5).Visible = True
                p(6).Visible = True

            'devils
        ElseIf t.Value = 7 Then
                p(0).Visible = False
                p(1).Visible = True
                p(2).Visible = False
                p(3).Visible = False
                p(4).Visible = False
                p(5).Visible = False
                p(6).Visible = False
        End If
    End Sub

    'GENERATE IMAGE
    'returns a Bitmap based on textbox contents and trackbar positions
    Function generateImage() As Bitmap

        Dim final As Bitmap = My.Resources.blank

        'adds the text
        Using finalEdit As Graphics = Graphics.FromImage(final)

            'declaring new fonts to use for this method
            Dim fontB = New System.Drawing.Font(privateFontsB.Families(0), 32, FontStyle.Bold)
            Dim font = New System.Drawing.Font(privateFonts.Families(0), 32)

            'draws the song info
            finalEdit.DrawString(lblTitle.Text, fontB, Brushes.White, 320, 17)
            finalEdit.DrawString(lblAlbum.Text, font, Brushes.White, 320, 69)
            finalEdit.DrawString(lblGenreT.Text, font, Brushes.White, 320, 121)
            finalEdit.DrawString(lblYear.Text, font, Brushes.White, 320, 173)

            'dispose fonts
            font.Dispose()
            fontB.Dispose()
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

End Class
