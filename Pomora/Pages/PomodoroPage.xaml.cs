using System;
using System.Timers;
using Microsoft.Maui.Controls;



namespace Pomora.Pages;

public partial class PomodoroPage : ContentPage
{

    private System.Timers.Timer _timer;
    int minutes = 0;
    int seconds = 0;
    bool isPomodoroActivated = false;
    bool isBreaktime = false;
    int INITIAL_MINUTES = 0;
    int INITIAL_SECONDS = 0;

    public PomodoroPage()
	{
		InitializeComponent();
        //_timer = new System.Timers.Timer(1000);
        
    }

	private void HandleSecondsEntry(object sender, TextChangedEventArgs e)
	{
        // Verifica si el nuevo valor tiene más de 2 caracteres
        if (e.NewTextValue.Length > 2)
        {
            // Limita el texto a 2 caracteres
            SecondsEntry.Text = e.NewTextValue.Substring(0, 2);
        }
    }

    private void HandleMinutesEntry(object sender, TextChangedEventArgs e)
    {
        // Verifica si el nuevo valor tiene más de 2 caracteres
        if (e.NewTextValue.Length > 2)
        {
            // Limita el texto a 2 caracteres
            MinutesEntry.Text = e.NewTextValue.Substring(0, 2);
        }

        if (int.TryParse(e.NewTextValue, out int newValue))
        {
            //to don´t allows user put more than 60 minutes
            if (newValue > 60)
            {
                // Si el valor es mayor a 60, ajusta el texto
                MinutesEntry.Text = "60"; // O puedes dejarlo vacío o establecer otro valor
            }
            if (newValue < 0)
            {
                MinutesEntry.Text = "0";
            }
        }
    }

    private void StartPomodoro(object sender, EventArgs e) {
         minutes = int.Parse(MinutesEntry.Text);
         seconds = int.Parse(SecondsEntry.Text);
         INITIAL_MINUTES = minutes;
         INITIAL_SECONDS = seconds;
         isPomodoroActivated = true;
         StopButton.IsVisible = true;
         SkipButton.IsVisible = true;
         StartButton.IsVisible = false;
         ResetButton.IsVisible = true;

        if (isPomodoroActivated) {
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimedEvent;
            _timer.Enabled = true;
        }
    }

    private void StopPomodoro(object sender, EventArgs e) {
        isPomodoroActivated = false;
        StopButton.IsVisible = false;
        StartButton.IsVisible = true;
        

        base.OnDisappearing();
        _timer.Stop();
        _timer.Dispose();
    }

    private void SkipPomodoro(object sender, EventArgs e) {
        isBreaktime = !isBreaktime;

        if (isBreaktime) {
            PomodoroActionText.Text = "Break Time";
            minutes = 10;
            seconds = 0;
            activateVibration();
        }
        else
        {
            PomodoroActionText.Text = "Focus";
            minutes = INITIAL_MINUTES;
            seconds = INITIAL_SECONDS;
            activateVibration();
        }

        if (seconds == 0)
        {
            if (minutes > 0)
            {
                minutes--;
                seconds = 60;
            }
        }
        keepFormatTime();
    }

    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        if (isPomodoroActivated) {
            // Cancela el enfoque para que el teclado no se abra
            ((Entry)sender).Unfocus();
        }
    }

    private void ResetPomodoro(object sender, EventArgs e) {
        _timer.Stop();
        _timer.Dispose();
        ResetButton.IsVisible = false;
        StartButton.IsVisible= true;
        SkipButton.IsVisible = false;
        StopButton.IsVisible = false;
        isPomodoroActivated = false;
        PomodoroActionText.Text = "Focus";
        minutes = INITIAL_MINUTES;
        seconds = INITIAL_SECONDS;
        keepFormatTime();
    }

    private void OnTimedEvent(object sender, ElapsedEventArgs e)
    {
        // Esta acción se ejecutará cada segundo
        MainThread.BeginInvokeOnMainThread(() =>
        {
            //handle time
            if (seconds == 0) {
                
                //if there is minutes, seconds can change
                if (minutes > 0)
                {
                    minutes--;
                    seconds = 60;
                }

                //end of the time, the var seconds do not has more time
                if (seconds == 0)
                {
                    MinutesEntry.Text = "15";
                    SecondsEntry.Text = "00";
                    activateVibration();
                    isBreaktime = !isBreaktime;
                    if (isBreaktime)
                    {
                        PomodoroActionText.Text = "Break Time";
                        minutes = 10;
                        seconds = 0;

                        if (seconds == 0)
                        {
                            if (minutes > 0)
                            {
                                minutes--;
                                seconds = 60;
                            }
                        }
                    }
                }
            }
            seconds--;
            keepFormatTime();
        });
    }

    private void keepFormatTime() {
        //keep format for the minutes
        if (minutes < 10)
        {
            //show two digits format
            MinutesEntry.Text = "0" + minutes.ToString();
        }
        else
        {
            //show two digits format(default)
            MinutesEntry.Text = minutes.ToString();
        }

        //keep format for the seconds
        if (seconds < 10)
        {
            //show two digits format
            SecondsEntry.Text = "0" + seconds.ToString();
        }
        else
        {
            //show two digits format(default)
            SecondsEntry.Text = seconds.ToString();
        }
    }

    private async void activateVibration() {
        for (int i = 0; i < 5; i++) // Número de pulsos
        {
            Vibration.Vibrate(); // Inicia la vibración
            await Task.Delay(500); // Duración de la vibración (500 ms)
                                   // Espera un intervalo sin vibrar
            await Task.Delay(500); // Pausa entre pulsos (500 ms)
        }
    }
}