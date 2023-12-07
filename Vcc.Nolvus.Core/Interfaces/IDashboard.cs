using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Frames;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IDashboard : INolvusService
    {
        Task<T> LoadFrameAsync<T>(FrameParameters Parameters = null) where T : DashboardFrame;
        T LoadFrame<T>(FrameParameters Parameters = null) where T : DashboardFrame;
        Task Error(string Title, string Message, string Trace = null, bool Retry = false);        

        void ShutDown();
        
        bool IsOlder(string Version);
        string Version { get; }      
       
        event OnFrameLoadedHandler OnFrameLoaded;
        event OnFrameLoadedHandler OnFrameLoadedAsync;

        double ScalingFactor { get; }

        void Status(string Value);
        void NoStatus();
        void Progress(int Value);
        void ProgressCompleted();
        void Info(string Value);
        void AdditionalInfo(string Value);
        void AdditionalSecondaryInfo(string Value);
        void AdditionalTertiaryInfo(string Value);
        void TitleInfo(string Value);
        void Title(string Value);
        void NexusAccount(string Value);
        void AccountType(string Value);
        void LoadAccountImage(string Url);
        void LoadAccountImage(System.Drawing.Image Image);
        void ClearInfo();
    }
}
