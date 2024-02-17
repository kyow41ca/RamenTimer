using System.Timers;
using Timer = System.Timers.Timer;

namespace RamenTimer;

public partial class MainPage : ContentPage
{
    private Timer _timer;
    private int _secondsRemaining;
    private bool _isTimerRunning = false; // タイマーが実行中かどうかを追跡

    public MainPage()
    {
        InitializeComponent();
        _timer = new Timer(1000); // 1秒ごとにタイマーイベントを発生
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true; // タイマーを自動リセット
    }

    private void OnStartButtonClicked(object sender, EventArgs e)
    {
        // タイマーが既に実行中の場合、何もしない
        if (_isTimerRunning) return;

        // タイマーが停止していて、_secondsRemainingが0なら新しい値を設定
        if (!_isTimerRunning && _secondsRemaining == 0 && int.TryParse(minutesEntry.Text, out int minutes))
        {
            _secondsRemaining = minutes * 60;
        }

        // タイマーが停止しているが、_secondsRemainingにまだ時間がある場合、そのままタイマーを再開
        if (!_isTimerRunning && _secondsRemaining > 0)
        {
            _timer.Start(); // タイマー開始
            _isTimerRunning = true; // タイマー実行中のフラグを設定
        }
    }

    private void OnStopButtonClicked(object sender, EventArgs e)
    {
        if (_isTimerRunning)
        {
            _timer.Stop(); // タイマー停止
            _isTimerRunning = false; // タイマー実行中のフラグを解除
        }
    }

    private void OnResetButtonClicked(object sender, EventArgs e)
    {
        _timer.Stop(); // タイマー停止
        _secondsRemaining = 0; // カウンターをリセット
        timerLabel.Text = "00:00";
        _isTimerRunning = false; // タイマー実行中のフラグを解除
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        _secondsRemaining--;

        if (_secondsRemaining <= 0)
        {
            _timer.Stop();
            _isTimerRunning = false; // タイマーが停止したのでフラグを解除
            MainThread.BeginInvokeOnMainThread(() =>
            {
                timerLabel.Text = "00:00";
                DisplayAlert("Time's Up!", "Ramen is ready!", "OK");
            });
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                timerLabel.Text = TimeSpan.FromSeconds(_secondsRemaining).ToString(@"mm\:ss");
            });
        }
    }
}
