using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using static System.Environment;

namespace Tasks.Example13
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string ThreadDetails
        {
            get
            {
                var thread = Thread.CurrentThread;
                return
                    $"[Thread : #{thread.ManagedThreadId}," +
                    $"ThreadPool:{thread.IsThreadPoolThread}," +
                    $"Background:{thread.IsBackground}]";
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ConfigureAwaitBtn1_OnClick(object sender, RoutedEventArgs e)
        {
            OutputBox.AppendText($"Program : A1. Delay in background {ThreadDetails}{NewLine}");
            OutputBox.AppendText($"Program : A2. Window isn't frozen during delay {ThreadDetails}{NewLine}");

            await Task.Delay(5_000).ConfigureAwait(true);
            
            OutputBox.AppendText($"Program : A3. After delay {ThreadDetails}{NewLine}");
        }

        private async void ConfigureAwaitBtn2_OnClick(object sender, RoutedEventArgs e)
        {
            OutputBox.AppendText($"Program : B1. Delay in background {ThreadDetails}{NewLine}");
            OutputBox.AppendText($"Program : B2. Exception after delay; details in Debug console {ThreadDetails}{NewLine}");

            await Task.Delay(5_000).ConfigureAwait(false);
            
            Debug.WriteLine($"Program : B3. After delay {ThreadDetails}");
            OutputBox.AppendText($"Program : B3. After delay {ThreadDetails}{NewLine}");
        }

        private void SyncCtxPostBtn_OnClick(object sender, RoutedEventArgs e)
        {
            OutputBox.AppendText($"Program : C1. Ask sync context to enqueue callback {ThreadDetails}{NewLine}");

            SynchronizationContext.Current.Post(state =>
            {
                OutputBox.AppendText($"Sync context : C3. Callback was processed {ThreadDetails}{NewLine}");
            }, null);

            OutputBox.AppendText($"Program : C2. Waiting for callback {ThreadDetails}{NewLine}");
        }

        private void DeadlockBtn_OnClick(object sender, RoutedEventArgs e)
        {
            OutputBox.AppendText($"Program : D1. Deadlock because of ConfigureAwait(true) in library {ThreadDetails}{NewLine}");
            
            WorkloadAsync(true).GetAwaiter().GetResult();
            
            OutputBox.AppendText($"Program : D2. You can't see this text {ThreadDetails}{NewLine}");
        }
        private void FreezeBtn_OnClick(object sender, RoutedEventArgs e)
        {
            OutputBox.AppendText($"Program : F1. Window freeze because of sync call  {ThreadDetails}{NewLine}");
            
            WorkloadAsync(false).GetAwaiter().GetResult();
            
            OutputBox.AppendText($"Program : F2. After freeze {ThreadDetails}{NewLine}");
        }

        private async Task WorkloadAsync(bool captureContext)
        {
            await Task.Delay(5_000).ConfigureAwait(captureContext);
        }
    }
}
