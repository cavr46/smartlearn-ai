using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using SmartLearn.Domain.Interfaces;

namespace SmartLearn.Infrastructure.Services;

public class TranscriptionService : ITranscriptionService
{
    private readonly string _speechKey;
    private readonly string _speechRegion;

    public TranscriptionService(IConfiguration configuration)
    {
        _speechKey = configuration["AzureCognitiveServices:SpeechKey"] ?? throw new InvalidOperationException("Speech key not configured");
        _speechRegion = configuration["AzureCognitiveServices:SpeechRegion"] ?? throw new InvalidOperationException("Speech region not configured");
    }

    public async Task<string> TranscribeAudioAsync(string audioUrl, CancellationToken cancellationToken = default)
    {
        try
        {
            var config = SpeechConfig.FromSubscription(_speechKey, _speechRegion);
            config.SpeechRecognitionLanguage = "en-US";

            using var audioConfig = AudioConfig.FromWavFileInput(audioUrl);
            using var recognizer = new SpeechRecognizer(config, audioConfig);

            var result = await recognizer.RecognizeOnceAsync();

            return result.Reason switch
            {
                ResultReason.RecognizedSpeech => result.Text,
                ResultReason.NoMatch => "No speech could be recognized.",
                ResultReason.Canceled => HandleCancellation(result),
                _ => "Unknown error occurred during transcription."
            };
        }
        catch (Exception ex)
        {
            return $"Error during transcription: {ex.Message}";
        }
    }

    public async Task<string> TranscribeVideoAsync(string videoUrl, CancellationToken cancellationToken = default)
    {
        // For video transcription, we would typically:
        // 1. Extract audio from video using FFmpeg or similar
        // 2. Upload audio to Azure Speech Service
        // 3. Use batch transcription API for longer content
        
        // For now, we'll simulate the process
        await Task.Delay(1000, cancellationToken);
        
        return "Video transcription functionality requires audio extraction from video. This is a placeholder implementation.";
    }

    public async Task<string> TranscribeFromBlobAsync(string blobUrl, CancellationToken cancellationToken = default)
    {
        try
        {
            // Download audio from blob storage
            using var httpClient = new HttpClient();
            var audioData = await httpClient.GetByteArrayAsync(blobUrl, cancellationToken);
            
            // Create temporary file
            var tempFile = Path.GetTempFileName();
            await File.WriteAllBytesAsync(tempFile, audioData, cancellationToken);
            
            try
            {
                var config = SpeechConfig.FromSubscription(_speechKey, _speechRegion);
                config.SpeechRecognitionLanguage = "en-US";

                using var audioConfig = AudioConfig.FromWavFileInput(tempFile);
                using var recognizer = new SpeechRecognizer(config, audioConfig);

                var result = await recognizer.RecognizeOnceAsync();

                return result.Reason switch
                {
                    ResultReason.RecognizedSpeech => result.Text,
                    ResultReason.NoMatch => "No speech could be recognized.",
                    ResultReason.Canceled => HandleCancellation(result),
                    _ => "Unknown error occurred during transcription."
                };
            }
            finally
            {
                // Clean up temporary file
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }
        catch (Exception ex)
        {
            return $"Error during blob transcription: {ex.Message}";
        }
    }

    private string HandleCancellation(SpeechRecognitionResult result)
    {
        var cancellation = CancellationDetails.FromResult(result);
        
        if (cancellation.Reason == CancellationReason.Error)
        {
            return $"Speech recognition was cancelled due to an error. Error code: {cancellation.ErrorCode}, Details: {cancellation.ErrorDetails}";
        }
        
        return "Speech recognition was cancelled.";
    }
}