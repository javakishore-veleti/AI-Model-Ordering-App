using System;
using System.Threading.Tasks;

public class ToastService
{
    public event Action<string, string, string>? OnShow;

    public void ShowSuccess(string message) =>
        OnShow?.Invoke("success", "Success", message);

    public void ShowError(string message) =>
        OnShow?.Invoke("danger", "Error", message);

    public void ShowInfo(string message) =>
        OnShow?.Invoke("info", "Info", message);

    public void ShowWarning(string message) =>
        OnShow?.Invoke("warning", "Warning", message);
}
