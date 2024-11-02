using System.Timers;
using Timer = System.Timers.Timer; // Alias para la clase Timer

namespace Pomora.Pages
{
    public partial class CronometerPage : ContentPage
    {
        private Timer _timer;
        private int _remainingTimeInSeconds;
        private bool _isTimerRunning = false;

        public CronometerPage()
        {
            InitializeComponent();

            // Asociar eventos para detectar cambios en las entradas
            HoursEntry.TextChanged += OnTimeEntryTextChanged;
            MinutesEntry.TextChanged += OnTimeEntryTextChanged;
            SecondsEntry.TextChanged += OnTimeEntryTextChanged;
        }

        private void OnStartButtonClicked(object sender, EventArgs e)
        {
            // Leer y convertir los valores de las entradas
            int hours = int.TryParse(HoursEntry.Text, out int h) ? h : 0;
            int minutes = int.TryParse(MinutesEntry.Text, out int m) ? m : 0;
            int seconds = int.TryParse(SecondsEntry.Text, out int s) ? s : 0;

            // Calcular el tiempo total en segundos
            _remainingTimeInSeconds = (hours * 3600) + (minutes * 60) + seconds;

            // Verificar si el temporizador ya est� en ejecuci�n
            if (!_isTimerRunning)
            {
                // Configurar el temporizador
                _timer = new Timer(1000); // 1000 ms = 1 segundo
                _timer.Elapsed += OnTimerElapsed;
                _timer.AutoReset = true;
                _timer.Start();

                _isTimerRunning = true; // Marcar que el temporizador est� en ejecuci�n
            }
        }

        private void OnStopTimer(object sender, EventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null; // Limpiar la referencia al temporizador
                _isTimerRunning = false; // Reiniciar el estado del temporizador
            }
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Disminuir el tiempo restante
            if (_remainingTimeInSeconds > 0)
            {
                _remainingTimeInSeconds--;
                int hours = _remainingTimeInSeconds / 3600;
                int minutes = (_remainingTimeInSeconds % 3600) / 60;
                int seconds = _remainingTimeInSeconds % 60;

                // Actualizar el UI (necesitamos hacerlo en el hilo principal)
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    HoursEntry.Text = hours.ToString("D2");
                    MinutesEntry.Text = minutes.ToString("D2");
                    SecondsEntry.Text = seconds.ToString("D2");
                });
            }
            else
            {
                // Detener el temporizador cuando llegue a 0
                OnStopTimer(sender, e); // Usar el m�todo de parada para limpiar todo
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Tiempo Completo", "La cuenta regresiva ha terminado.", "OK");
                });
            }
        }

        private void OnTimeEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            if (int.TryParse(entry.Text, out int value))
            {
                if (entry == HoursEntry && (value < 0 || value > 23))
                    entry.TextColor = Colors.Red;
                else if ((entry == MinutesEntry || entry == SecondsEntry) && (value < 0 || value > 59))
                    entry.TextColor = Colors.Red;
                else
                    entry.TextColor = Colors.White;
            }
            else
            {
                entry.TextColor = Colors.Red;
            }
        }
    }
}