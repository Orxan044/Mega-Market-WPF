using Mega_Market_App.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace Mega_Market_App.Views;


public partial class SplashView : Window
{
    public SplashView()
    {
        InitializeComponent();
    }

    private void Window_ContentRendered(object sender, EventArgs e)
    {
        BackgroundWorker worker = new BackgroundWorker();
        worker.WorkerReportsProgress = true;
        worker.DoWork += worker_DoWork;
        worker.ProgressChanged += worker_ProgressChanged;
        worker.RunWorkerAsync();
    }
    private void worker_DoWork(object? sender, DoWorkEventArgs e)
    {
        for (int i = 0; i < 8; i++)
        {
            (sender as BackgroundWorker)!.ReportProgress(i);
            Thread.Sleep(800);
        }

    }

    private void worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
    {
        progressBar.Value += e.ProgressPercentage;
        progressBar.Value += 10;
        if (progressBar.Value == 100)
        {
            Close();

            Application.Current.MainWindow = new MainView();
            Application.Current.MainWindow.DataContext = new MainViewModel();
            Application.Current.MainWindow.Show();

        }

    }

}
