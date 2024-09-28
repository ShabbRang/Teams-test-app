using Microsoft.AspNetCore.SignalR;
using System;
using System.IO;
using System.Threading.Tasks;

public class VoiceHub : Hub
{
    // This method will be used to send an audio file to connected clients
    public async Task SendAudioFile()
    {
        // Path to your audio file (Make sure this path is valid on your server)
       var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Test.wav");
 // Provide the full path to the file

        if (!File.Exists(filePath))
    {
        Console.WriteLine("Audio file not found.");
        return;
    }

        // Read the file as a byte array
        byte[] audioData = await File.ReadAllBytesAsync(filePath);

        // Log the file size for debugging purposes
        Console.WriteLine($"Sending audio file of size: {audioData.Length}");

        // Send the audio data to all connected clients
        await Clients.All.SendAsync("ReceiveVoiceData", audioData);
    }

    // Other methods like OnConnectedAsync can remain here if you have them
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"Client connected. Connection ID: {Context.ConnectionId}");
        await SendAudioFile();
        await base.OnConnectedAsync();
    }
}
