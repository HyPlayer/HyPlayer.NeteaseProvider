using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models
{
    public class NeteaseImageResourceQualityTag : ImageResourceQualityTag
    {
        public NeteaseImageResourceQualityTag(int pixelX, int pixelY) : base(pixelX, pixelY)
        {
            //There is nothing, I just need to override the "ToString Method."
        }

        public override string ToString()
        {
            return $"param={PixelX}y{PixelY}";
        }
    }
}
