using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace Elephant.Messages;

public sealed class DataFileChangedMessage : ValueChangedMessage<string>
{
    public DataFileChangedMessage(string value) : base(value) {}
}
