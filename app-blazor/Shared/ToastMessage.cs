namespace app_blazor.Shared;

public class ToastMessage
{
    public string Message { get; set; } = "";
    public string Title { get; set; } = "";
    public ToastType Type { get; set; } = ToastType.Info;
    public int Duration { get; set; } = 4000; // milliseconds
}

public enum ToastType
{
    Success,
    Error,
    Warning,
    Info
}
