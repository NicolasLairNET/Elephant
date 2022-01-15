using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elephant.Messages
{
    public sealed class ViewModelChangedMessage : ValueChangedMessage<string>
    {
        public ViewModelChangedMessage(string value) : base(value)
        {

        }
    }
}
