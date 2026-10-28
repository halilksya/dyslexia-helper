using RapidOcrNet;
using SkiaSharp;

namespace DyslexiaHelper.Core.Recognition;

public sealed class TextRecognizer : IDisposable
{
    private readonly RapidOcr _ocrEngine;

    public TextRecognizer()
    {
        var modelDirectory = Path.Combine(AppContext.BaseDirectory, "models", "v5");

        _ocrEngine = new RapidOcr();
        _ocrEngine.InitModels(
            Path.Combine(modelDirectory, RapidOcr.DefaultDetModelPath),
            Path.Combine(modelDirectory, RapidOcr.DefaultClsModelPath),
            Path.Combine(modelDirectory, RapidOcr.DefaultRecModelPath),
            Path.Combine(modelDirectory, RapidOcr.DefaultKeysFilePath));
    }

    public Task<string> RecognizeAsync(byte[] imageBytes)
    {
        return Task.Run(() =>
        {
            using var bitmap = SKBitmap.Decode(imageBytes);
            if(bitmap is null)
                throw new ArgumentException("Image could not be decoded", nameof(imageBytes));
            
            var result = _ocrEngine.Detect(bitmap, RapidOcrOptions.Default);
            return result.StrRes;
        });
    }

    public void Dispose()
    {
        _ocrEngine.Dispose();
    }
}