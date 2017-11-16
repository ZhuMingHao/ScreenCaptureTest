using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScreenCaptureTest;
using ScreenCaptureTest.UWP;
using Xamarin.Forms;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Display;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using System.IO;
using Microsoft.Graphics.Canvas;
using Windows.UI;
using Windows.Storage;

[assembly: Dependency(typeof(ScreenshotService))]

namespace ScreenCaptureTest.UWP
{
    public class ScreenshotService : IScreenshotServicecs
    {
        public async Task<byte[]> CaptureAsync(Stream Tem)
        {
            var rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(Window.Current.Content);

            var pixelBuffer = await rtb.GetPixelsAsync();
            var pixels = pixelBuffer.ToArray();

            var displayInformation = DisplayInformation.GetForCurrentView();
            var stream = new InMemoryRandomAccessStream();
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
            encoder.SetPixelData(BitmapPixelFormat.Bgra8,
            BitmapAlphaMode.Premultiplied,
            (uint)rtb.PixelWidth,
            (uint)rtb.PixelHeight,
            displayInformation.RawDpiX,
            displayInformation.RawDpiY,
            pixels);
            await encoder.FlushAsync();
            stream.Seek(0);
            var readStram = stream.AsStreamForRead();


            var pagebitmap = await GetSoftwareBitmap(readStram);
            var softwareBitmap = await GetSoftwareBitmap(Tem);


            CanvasDevice device = CanvasDevice.GetSharedDevice();
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(device, rtb.PixelWidth, rtb.PixelHeight, 96);


            using (var ds = renderTarget.CreateDrawingSession())
            {
                ds.Clear(Colors.White);
                var page = CanvasBitmap.CreateFromSoftwareBitmap(device, pagebitmap);
                var image = CanvasBitmap.CreateFromSoftwareBitmap(device, softwareBitmap);
                ds.DrawImage(page);
                ds.DrawImage(image);
            }

            return renderTarget.GetPixelBytes();

            //StorageFolder storageFolder = KnownFolders.PicturesLibrary;
            //var file = await storageFolder.CreateFileAsync("output.jpg", CreationCollisionOption.ReplaceExisting);
            //using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            //{
            //    await renderTarget.SaveAsync(fileStream, CanvasBitmapFileFormat.Jpeg, 1f);
            //}

        }
        private async Task<SoftwareBitmap> GetSoftwareBitmap(Stream data)
        {
            BitmapDecoder pagedecoder = await BitmapDecoder.CreateAsync(data.AsRandomAccessStream());
            return await pagedecoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
        }
    }
}