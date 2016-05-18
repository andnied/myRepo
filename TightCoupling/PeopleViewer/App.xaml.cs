using PeopleViewer.Presentation;
using PersonRepository.Caching;
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
            var repo = new CachingRepository(new CSVRepository());
            var peopleVM = new PeopleViewerViewModel(repo);

            Current.MainWindow = new PeopleViewerWindow(peopleVM);
            Current.MainWindow.Title = "Loose Coupling App";
        }
    }
}
