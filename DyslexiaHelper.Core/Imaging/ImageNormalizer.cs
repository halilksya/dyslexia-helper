using SkiaSharp;

namespace DyslexiaHelper.Core.Imaging;

public static class ImageNormalizer
{
    private const int MaxSideLength = 2000;

    public static byte[] Normalize(byte[] imageBytes)
    {
        using var data = SKData.CreateCopy(imageBytes);
        using var codec = SKCodec.Create(data) ?? throw new ArgumentException("Image could not be decoded", nameof(imageBytes));
        
        using var decoded = SKBitmap.Decode(codec);
        using var upright = ApplyOrientation(decoded, codec.EncodedOrigin);
        using var resized = LimitSize(upright);

        using var encoded = resized.Encode(SKEncodedImageFormat.Jpeg, 95);
        return encoded.ToArray();
    }
    
    private static SKBitmap ApplyOrientation(SKBitmap source, SKEncodedOrigin origin)
    {
        var(degress, swapSides) = origin switch
        {
            SKEncodedOrigin.RightTop => (90, true),
            SKEncodedOrigin.BottomRight => (180, false),
            SKEncodedOrigin.LeftBottom => (270, true),
            _ => (0, false)
        };

        if(degress == 0)
            return source.Copy();

        var width = swapSides ? source.Height : source.Width;
        var height = swapSides ? source.Width : source.Height;
        var rotated = new SKBitmap(width, height);

        using var canvas = new SKCanvas(rotated);
        canvas.Translate(width / 2f, height / 2f);
        canvas.RotateDegrees(degress);
        canvas.Translate(-source.Width / 2f, -source.Height / 2f);
        canvas.DrawBitmap(source, 0, 0);

        return rotated;
    }

    private static SKBitmap LimitSize(SKBitmap source)
    {
        var longestSide = Math.Max(source.Width, source.Height);
        if(longestSide <= MaxSideLength)
            return source.Copy();

        var scale = (float)MaxSideLength / longestSide;
        var targetInfo = new SKImageInfo((int)(source.Width * scale), (int)(source.Height * scale));

        return source.Resize(targetInfo, new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
    }
}