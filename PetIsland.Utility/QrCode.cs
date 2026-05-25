using ZXing.SkiaSharp;
using SkiaSharp;

namespace PetIsland.Utility
{
    public class QrCode
    {
        public static string? ReadQrCode(Stream imageStream)
        {
            using var bitmap = SKBitmap.Decode(imageStream);
            if (bitmap == null) return null;

            var reader = new BarcodeReader();
            var result = reader.Decode(bitmap);

            return result?.Text;
        }
    }
}
