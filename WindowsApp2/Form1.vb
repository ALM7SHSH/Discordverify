Imports System.Collections.Specialized
Imports System.Net
Imports System.Security
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic.CompilerServices

Public Class Form1
    Dim Expaire_OFF As Boolean
    Public randomString As String
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub
    Shared random As New Random()
    Public Function Randomiz()
        Dim validchars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
        Dim sb As New StringBuilder()
        Dim rand As New Random()
        For i As Integer = 1 To 10
            Dim idx As Integer = rand.Next(0, validchars.Length)
            Dim randomChar As Char = validchars(idx)
            sb.Append(randomChar)
        Next i
        randomString = sb.ToString()
    End Function
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox2.Text = TextBox1.Text + "-" + randomString Then
            MsgBox("Welcome")
        Else
            MsgBox("Error")
        End If
    End Sub
    Public Function Convert_To_MD5(ByVal input As String) As String
        Dim M22 As New Cryptography.MD5CryptoServiceProvider
        Dim Data As Byte()
        Dim Result As Byte()
        Dim Res As String = ""
        Dim Tmp As String = ""
        Data = Encoding.ASCII.GetBytes(input)
        Result = M22.ComputeHash(Data)
        For i As Integer = 0 To Result.Length - 1
            Tmp = Hex(Result(i))
            If Len(Tmp) = 1 Then Tmp = "0" & Tmp
            Res += Tmp
        Next
        Return Res
    End Function
    Sub Code_Expaire()
        For i As Integer = 0 To 60
            Thread.Sleep(1000)
            Invoke(Sub() Label2.Text += 1)
            If Expaire_OFF = True Then Exit Sub
        Next
        Invoke(Sub() Label2.Text = 0)
        Randomiz()
        MsgBox("Code Expaire",, "Verify System")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Randomiz()
        Dim dcWeb As dWebHook = New dWebHook
        dcWeb.WebHook = "Add Your WebHook Here"
        dcWeb.SendMessage(TextBox1.Text + "-" + randomString)
        Task.Factory.StartNew(Sub() Code_Expaire())
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ProjectData.EndApp()
    End Sub
End Class
Public Class dWebHook
    Implements IDisposable
    Private ReadOnly client As WebClient
    Private Shared discordValues As NameValueCollection = New NameValueCollection()
    Public Property WebHook As String
    Public Property UserName As String
    Public Property ProfilePicture As String
    Public Sub New()
        client = New WebClient()
    End Sub
    Public Sub SendMessage(ByVal msgSend As String)
        If msgSend = "" Or WebHook = "" Then
            MsgBox("The webhook link And message are required!", vbCritical + vbOKOnly)
            Return
        End If
        discordValues.Add("content", msgSend)
        Try
            client.UploadValues(WebHook, discordValues)
        Catch
            MsgBox("Unable To send message!" & vbNewLine & vbNewLine & "This issue can be caused by one Or more Of the following:" & vbNewLine & "- The webhook link is incorrect." & vbNewLine & "- There is no connection to the Internet." & vbNewLine & "- Another program or firewall is blocking this application's access to the Internet." & vbNewLine & "- Discord's servers are down." & vbNewLine & vbNewLine & "If you believe everything is in working order and this problem persists, please submit an issue on this program's Github page.", vbCritical + vbOKOnly, "Discord Webhook Announcer")
        End Try
        discordValues.Remove("username")
        discordValues.Remove("content")
    End Sub
    Public Sub Dispose()
        client.Dispose()
    End Sub

    Private Sub IDisposable_Dispose() Implements IDisposable.Dispose
        DirectCast(client, IDisposable).Dispose()
    End Sub
End Class