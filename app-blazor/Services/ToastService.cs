using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace app_blazor.Shared;

public class ToastService
{
    public event Action<ToastMessage>? OnShow;
    public event Action<ToastMessage>? OnHide;

    public void ShowToast(string message, string title, ToastType type = ToastType.Info, int duration = 4000)
    {
        var toast = new ToastMessage
        {
            Message = message,
            Title = title,
            Type = type,
            Duration = duration
        };

        OnShow?.Invoke(toast);

        // Auto dismiss
        Task.Delay(duration).ContinueWith(_ => OnHide?.Invoke(toast));
    }

    public void ShowSuccess(string msg, string title = "Success") =>
        ShowToast(msg, title, ToastType.Success);

    public void ShowError(string msg, string title = "Error") =>
        ShowToast(msg, title, ToastType.Error);

    public void ShowWarning(string msg, string title = "Warning") =>
        ShowToast(msg, title, ToastType.Warning);

    public void ShowInfo(string msg, string title = "Info") =>
        ShowToast(msg, title, ToastType.Info);
}
