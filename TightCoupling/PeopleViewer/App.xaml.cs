using PersonRepository.CSV;
using PersonRepository.Service;
using System.Windows;

namespace PeopleViewer
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ComposeObjects();
            Current.MainWindow.Show();
        }

        private static void ComposeObjects()
        {
            Current.MainWindow = new PeopleViewerWindow(
                new Presentation.PeopleViewerViewModel(
                    new CSVRepository()));
            Current.MainWindow.Title = "Loose Coupling App";
        }
    }
}
