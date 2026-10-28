using Android.App;
using Android.Runtime;
using Avalonia;
using Avalonia.Android;

namespace DyslexiaHelper.Android
{
    [Application]
    public class Application : AvaloniaAndroidApplication<App>
    {
        protected Application(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            
        }

        public override void OnCreate()
        {
            App.ModelDirectory = ExtractModels();
            base.OnCreate();
        }

        private string ExtractModels()
        {
            var targetDirectory = System.IO.Path.Combine(FilesDir!.AbsolutePath, "models", "v5");
            System.IO.Directory.CreateDirectory(targetDirectory);

            foreach (var fileName in Assets!.List("models/v5")!)
            {
                var targetPath = System.IO.Path.Combine(targetDirectory, fileName);
                if (System.IO.File.Exists(targetPath))
                    continue;

                using var assetStream = Assets.Open($"models/v5/{fileName}");
                using var fileStream = System.IO.File.Create(targetPath);
                assetStream.CopyTo(fileStream);
            }

            return targetDirectory;
        }

        protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
        {
            return base.CustomizeAppBuilder(builder).WithInterFont();
        }
    }
}
