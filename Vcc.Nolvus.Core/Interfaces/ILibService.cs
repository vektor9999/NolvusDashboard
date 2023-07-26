using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface ILibService : INolvusService
    {
        Image ResizeKeepAspectRatio(Image source, int width, int height);
        Image SetImageOpacity(Image Source, float Opacity);
        Image SetImageGradient(Image InputImage);
        string EncryptString(string value);
        string DecryptString(string value);
        Image GetImageFromWebStream(string ImageUrl);
    }
}
