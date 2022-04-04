using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace Elephant.Messages;

public sealed class ViewModelChangedMessage : ValueChangedMessage<string>
{
    public ViewModelChangedMessage(string value) : base(value) {}
}
