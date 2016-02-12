using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics; 

namespace Stewart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SpeechSynthesizer _voice;
        WebClient _web;
        string _lastText; 

        public MainWindow()
        {
            InitializeComponent();

            this.KeyUp += MainWindow_KeyUp;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _web = new WebClient(); 
            _voice = new SpeechSynthesizer();
            Task.Run((Action)BackgroundLoop); 
        }

        private void BackgroundLoop()
        {
            for(;;)
            {
                Thread.Sleep(1000);

                Dispatcher.Invoke((Action)delegate ()
                {
                    try
                    {
                        string text = _web.DownloadString("http://www.ben-rush.net/hal/text.txt").ToLower().Replace("\n", "");

                        if (text != _lastText)
                        {
                            if (text == "die")
                            {
                                this.Close();
                            }
                            else if (text == "minimize")
                            {
                                WindowState = WindowState.Minimized;
                            }
                            else if(text == "maximize")
                            {
                                WindowState = WindowState.Maximized; 
                            }
                            else if(text == "playgame")
                            {
                                WindowState = WindowState.Minimized;
                                Process.Start(@"C:\Users\Ben\Desktop\jnes_1_1_1\Jnes.exe",
                                    @"C:\Users\Ben\Desktop\jnes_1_1_1\mod2.nes"); 
                            }
                            else if(text == "killgame")
                            {
                                Process[] procs = Process.GetProcessesByName("Jnes");
                                foreach (Process proc in procs)
                                {
                                    proc.Kill();
                                }
                            }
                            else
                            {
                                _voice.SpeakAsync(text);
                            }

                            _lastText = text;
                        }
                    }
                    catch (Exception ex)
                    {
                        _web.Dispose();
                        _voice.Dispose();

                        _web = new WebClient();
                        _voice = new SpeechSynthesizer();
                    }
                }); 
            }
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
