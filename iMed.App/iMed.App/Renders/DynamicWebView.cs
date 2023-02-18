namespace iMed.App.Renders;
public class DynamicWebView : WebView
{
    public event EventHandler<EventArgs> WebViewSized;
    public void WebViewSize() => WebViewSized?.Invoke(this, EventArgs.Empty);
}
