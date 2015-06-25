using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace ObslugaElementowGraficznych {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        int wynik;
        bool powt;
        double start = 0.0;
        int sleep = 1000;
        bool koniec = false;
        DispatcherTimer timer;
        TimeSpan time, addTime;
        ScaleTransform st = new ScaleTransform(0.0, 1);

        public MainWindow() {
            InitializeComponent();
            timer = new DispatcherTimer();
            time = new TimeSpan(0, 0, 0);
            addTime = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(TimeSpan.TicksPerSecond);
            // LoadBar.RenderTransform = st;
            LoadBar.LayoutTransform = st;
        }
        public void LiczSilnie(int n) {
            wynik = 1;
            if(n == 1 || n == 0) {
                wynik = 1;
            }
            else {
                for(int i = n; i >= 1; i--) {
                    wynik *= i;
                }
            }
            while(true) {
                if(koniec) {
                    silnia.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
           new Action(() => silnia.Text = wynik.ToString()));
                    break;
                }
                else {
                    Thread.Sleep(sleep);
                }
            }
            // silnia.Text = wynik.ToString();
        }
        public void addToLoadBar(double x) {
            while(start < 1) {
                if(powt) {
                    start += x;
                    st.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() => st.ScaleX = start));
                    st.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() => LoadBar.LayoutTransform = st));
                }
                else
                    powt = true;
                Thread.Sleep(sleep);
            }
            koniec = true;
            timer.Stop();
        }
        private void Start_Click(object sender, RoutedEventArgs e) {
            int n;
            string nfromTextBox = silnia.Text;
            if(int.TryParse(nfromTextBox, out n)) {
                koniec = false;
                time = TimeSpan.Zero;
                start = 0.0;
                powt = false;
                wynik = 0;
                st.ScaleX = start;
                LoadBar.LayoutTransform = st;
                //  int n = Convert.ToInt32(silnia.Text);
                int s;
                Task t2 = Task.Run(() => LiczSilnie(n));
                if(n == 0)
                    s = 1;
                else
                    s = n * n;
                time = new TimeSpan(0, 0, s+1);
                timer.Start();
                Task t1 = Task.Run(() => {
                    addToLoadBar(1.0/(double)s);
                });
            }
        }
        void timer_Tick(object sender, EventArgs e) {
            time -= addTime;
            Pozostalo.Text = "Do konca pozostalo " + time.TotalSeconds + "s";
        }
    }
}
