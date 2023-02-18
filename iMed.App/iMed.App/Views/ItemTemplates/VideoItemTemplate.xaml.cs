namespace iMed.App.Views.ItemTemplates;


[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class VideoItemTemplate : ContentView
{
    public static readonly BindableProperty PlayCommandProperty =
        BindableProperty.Create(
            propertyName: nameof(PlayCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(VideoItemTemplate),
            defaultValue: null,
            defaultBindingMode: BindingMode.OneWay
        );

    public ICommand PlayCommand
    {
        get { return (ICommand)GetValue(PlayCommandProperty); }
        set { SetValue(PlayCommandProperty, value); }
    }

    public static readonly BindableProperty PurchaseCommandProperty =
        BindableProperty.Create(
            propertyName: nameof(PurchaseCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(VideoItemTemplate),
            defaultValue: null,
            defaultBindingMode: BindingMode.OneWay
        );

    public ICommand PurchaseCommand
    {
        get { return (ICommand)GetValue(PurchaseCommandProperty); }
        set { SetValue(PurchaseCommandProperty, value); }
    }

    public VideoItemTemplate()
    {
        InitializeComponent();
    }

    private void PlayTapGestureRecognizer_OnTapped(object sender, EventArgs e)
    {
        if (PlayCommand != null)
            PlayCommand.Execute(this.BindingContext);
    }

    private void PurchaseTapGestureRecognizer_OnTapped(object sender, EventArgs e)
    {
        if (PurchaseCommand != null)
            PurchaseCommand.Execute(null);
    }
}