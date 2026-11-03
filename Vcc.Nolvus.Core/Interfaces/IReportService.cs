using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Syncfusion.Pdf;
using Vcc.Nolvus.Core.Misc;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IReportService : INolvusService
    {        
        Task<PdfDocument> GenerateReportToPdf(ModObjectList ModObjects, Image Image, Action<string, int> Progress);
    }
}
