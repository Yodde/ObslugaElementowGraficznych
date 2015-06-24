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
namespace ObslugaElementowGraficznych {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        int wynik;
        double start = 0.0;
        int sleep = 100;
        int licz = 0;
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
           new Action(() => silnia.Text = wynik.ToString() + " " + licz));
                    timer.Stop();
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
                start += x;
                st.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() => st.ScaleX = start));
                st.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() => LoadBar.LayoutTransform = st));
                licz++;
                Thread.Sleep(sleep);
            }
            koniec = true;
        }
        private void Start_Click(object sender, RoutedEventArgs e) {
            licz = 0;
            koniec = false;
            time = TimeSpan.Zero;
            start = 0.0;
            wynik = 0;
            st.ScaleX = start;
            LoadBar.LayoutTransform = st;
            int n = Convert.ToInt32(silnia.Text);
            Task t2 = Task.Run(() => LiczSilnie(n));

            while(wynik == 0) ;
            double s = (double)wynik / 10.0;
            time = new TimeSpan(0, 0, (int)Math.Ceiling(s));
            timer.Start();
            Task t1 = Task.Run(() => {
                double x = 1.0 / wynik;
                addToLoadBar(x);
            });
        }
        void timer_Tick(object sender, EventArgs e) {
            time -= addTime;
            Pozostalo.Text = "Do konca pozostalo " + time.TotalSeconds + "s";
        }
    }
}
