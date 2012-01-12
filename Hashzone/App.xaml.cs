using System.Windows;
using Hashzone.Infrastructure;

namespace Hashzone
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Messenger _messenger = new Messenger();
        
        public static Messenger Notification
        {
            get { return _messenger; }
        }
    }
}
