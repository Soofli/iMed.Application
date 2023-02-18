using System.ComponentModel;
using System.Runtime.CompilerServices;
using iMed.App.Annotations;

namespace iMed.App.Models.Config;

public class UiItemModel<T> : INotifyPropertyChanged
{
    public string ErrorMessage { get; set; }
    public bool HasError { get; set; } = false;
    public T Value { get; set; }


    public event PropertyChangedEventHandler PropertyChanged;
    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}