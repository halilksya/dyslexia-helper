using System.Threading.Tasks;

namespace DyslexiaHelper.Services;

public interface ICameraService
{
    Task<byte[]> CapturePhotoAsync();
}


