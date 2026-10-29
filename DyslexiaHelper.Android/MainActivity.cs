using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using AndroidX.Core.Content;
using Avalonia.Android;
using DyslexiaHelper.Services;

namespace DyslexiaHelper.Android;

[Activity(
    Label = "DyslexiaHelper.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity, ICameraService
{
    private const int CaptureRequestCode = 1001;
    private const string FileProviderAuthority = "com.CompanyName.DyslexiaHelper.fileprovider";

    private TaskCompletionSource<bool>? _captureCompletion;

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        base.OnActivityResult(requestCode, resultCode, data);

        if (requestCode == CaptureRequestCode)
            _captureCompletion?.TrySetResult(resultCode == Result.Ok);
    }

    public async Task<byte[]?> CapturePhotoAsync()
    {
        var photoFile = new Java.IO.File(CacheDir, "capture.jpg");
        var photoUri = FileProvider.GetUriForFile(this, FileProviderAuthority, photoFile);

        var intent = new Intent(MediaStore.ActionImageCapture);
        intent.PutExtra(MediaStore.ExtraOutput, photoUri);
        intent.AddFlags(ActivityFlags.GrantWriteUriPermission);

        _captureCompletion = new TaskCompletionSource<bool>();
        try
        {
            StartActivityForResult(intent, CaptureRequestCode);
        }
        catch (ActivityNotFoundException)
        {
            return null;
        }

        var photoTaken = await _captureCompletion.Task;
        if (!photoTaken)
            return null;

        return await System.IO.File.ReadAllBytesAsync(photoFile.AbsolutePath);
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        App.CameraService = this;
        base.OnCreate(savedInstanceState);
    }
}