using System;
using Microsoft.Maui.Dispatching;

namespace Pomora.Pages;

public partial class ChronometerPage : ContentPage
{
    private bool isRunning = false;
    private TimeSpan elapsedTime = TimeSpan.Zero;
    private IDispatcherTimer timer;

    public ChronometerPage()
    {
        InitializeComponent();
        ConfigureTimer();
    }

    private void ConfigureTimer()
    {
        timer = Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += OnTimerTick;
    }

    private void OnTimerTick(object sender, EventArgs e)
    {
        if (isRunning)
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
            TimerLabel.Text = elapsedTime.ToString(@"hh\:mm\:ss");
        }
    }

    private void OnStartClicked(object sender, EventArgs e)
    {
        isRunning = true;
        timer.Start();

        StartButton.IsVisible = false;
        PauseButton.IsVisible = true;
        ResetButton.IsVisible = true;
    }

    private void OnPauseClicked(object sender, EventArgs e)
    {
        isRunning = false;
        timer.Stop();

        StartButton.IsVisible = true;
        PauseButton.IsVisible = false;
    }

    private void OnResetClicked(object sender, EventArgs e)
    {
        isRunning = false;
        elapsedTime = TimeSpan.Zero;
        TimerLabel.Text = "00:00:00";
        timer.Stop();

        StartButton.IsVisible = true;
        PauseButton.IsVisible = false;
        ResetButton.IsVisible = false;
    }
}