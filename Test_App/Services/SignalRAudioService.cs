using Microsoft.AspNetCore.SignalR.Client;
using NAudio.Wave;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Test_App.Services  // Make sure the namespace matches your project
{
    public class SignalRAudioService
    {
        private HubConnection _connection;

        public async Task StartConnectionAsync()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5032/voiceHub")  // Ensure the URL points to your SignalR hub
                .Build();

            await _connection.StartAsync();
            Console.WriteLine($"Connected to SignalR hub. Connection ID: {_connection.ConnectionId}");
        }

        public async Task PlayAudioAsync(Action onAudioStarted, Action onAudioEnded)
        {
            // Subscribe to the SignalR event
            _connection.On<byte[]>("ReceiveVoiceData", async audioData =>
            {
                onAudioStarted();  // Call the onAudioStarted action

                using (var ms = new MemoryStream(audioData))
                using (var waveOut = new WaveOutEvent())
                using (var reader = new WaveFileReader(ms))
                {
                    waveOut.Init(reader);
                    waveOut.Play();

                    // Use a TaskCompletionSource to await the playback completion
                    var tcs = new TaskCompletionSource<bool>();
                    waveOut.PlaybackStopped += (sender, e) => tcs.SetResult(true);

                    await tcs.Task; // Await until the audio playback finishes
                }

                onAudioEnded();  // Call the onAudioEnded action
            });
        }
    }
}
