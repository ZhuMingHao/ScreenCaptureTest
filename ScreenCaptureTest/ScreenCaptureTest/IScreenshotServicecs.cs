using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCaptureTest
{
    public interface IScreenshotServicecs
    {
        Task<Stream> CaptureAsync(Stream stream);
    }
}
