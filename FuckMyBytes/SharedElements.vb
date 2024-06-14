Imports System.Reflection
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Namespace FuckMyBytes
Public Class SharedElements
    public shared ReadOnly Settingspath as string = GetStartupPath() + "settings.json"
    public shared ReadOnly Loginpath as string = GetStartupPath() + "authorization.json"
    
    Public shared Function GetStartupPath() As String
        dim path as string = IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
        if RuntimeInformation.IsOSPlatform(OSPlatform.Windows) Then
            path = path + "\"
        Else
            path = path + "/"
        End if
        Return path
    End Function
    
    Public shared Function GetWindowsMachineId() As String
        If RuntimeInformation.IsOSPlatform(OSPlatform.Windows) Then
            Using key = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Cryptography")
                If key IsNot Nothing Then
                    Return key.GetValue("MachineGuid").ToString()
                Else
                    Throw New Exception("none")
                End If
            End Using
        Else
            Throw New PlatformNotSupportedException("This function is only supported on Windows.")
        End If
    End Function
    
    public shared function RunCommand(command as String) as String
        Dim processStartInfo As New ProcessStartInfo() With {
                .FileName = "cmd.exe",
                .Arguments = "/C " + command,
                .RedirectStandardOutput = True,
                .UseShellExecute = False,
                .CreateNoWindow = True
                }

        Dim process As New System.Diagnostics.Process() With {.StartInfo = processStartInfo}
        process.Start()

        Dim output As String = process.StandardOutput.ReadToEnd()
        process.WaitForExit()

        Return output.Trim()
    End function
    
    Public shared Function RunBashCommand(command As String) As String
        Dim processStartInfo As New ProcessStartInfo() With {
                .FileName = "/bin/bash",
                .Arguments = "-c """ + command + """",
                .RedirectStandardOutput = True,
                .UseShellExecute = False,
                .CreateNoWindow = True
                }

        Dim process As New Diagnostics.Process() With {.StartInfo = processStartInfo}
        process.Start()

        Dim output As String = process.StandardOutput.ReadToEnd()
        process.WaitForExit()
        if output.Length = 0 Then
            output = "none"
        End If
        
        Return output.Trim()
    End Function
End Class
End Namespace