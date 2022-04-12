using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MessageBox_wpf
{
    public partial class CustomMessageBox : Window
    {
        private CustomMessageBox()
        {
            InitializeComponent();
        }

        static CustomMessageBox? _messageBox;
        static MessageBoxResult _result = MessageBoxResult.No;
        static MessageBoxResult Show(string windowTitle, string message, MessageBoxType boxType)
        {
            return boxType switch
            {
                MessageBoxType.ConfirmationWithYesNo => Show(windowTitle, message, MessageBoxButton.YesNo, null),
                MessageBoxType.ConfirmationWithYesNoCancel => Show(windowTitle, message, MessageBoxButton.YesNoCancel, null),
                MessageBoxType.Information => Show(windowTitle, message, MessageBoxButton.OK, null),
                MessageBoxType.Error => Show(windowTitle, message, MessageBoxButton.OK, null),
                MessageBoxType.Warning => Show(windowTitle, message, MessageBoxButton.OK, null),
                _ => MessageBoxResult.No,
            };
        }

        public static MessageBoxResult Show(string message)
        {
            return Show(string.Empty, message, MessageBoxButton.OK, null);
        }

        public static MessageBoxResult Show(string message, MessageBoxType boxType)
        {
            return Show(string.Empty, message, boxType);
        }


        public static MessageBoxResult Show(string message, MessageBoxButton button, List<FileImportStatus> messageBoxDetails)
        {
            return Show(string.Empty, message, button, messageBoxDetails);
        }

        public static MessageBoxResult Show(string caption, string text)
        {
            return Show(caption, text, MessageBoxButton.OK, null);
        }

        public static MessageBoxResult Show(string caption, string text, MessageBoxButton button)
        {
            return Show(caption, text, button, null);
        }

        public static MessageBoxResult Show(
            string caption,
            string text,
            MessageBoxButton button,
            List<FileImportStatus>? importStatus)
        {
            _messageBox = new CustomMessageBox
            {
                txtMsg = { Text = text },
                Title = caption,
            };

            if (importStatus?.Count > 0)
            {
                _messageBox.ImportStatus.Visibility = Visibility.Visible;
                _messageBox.ImportStatus.ItemsSource = importStatus.OrderBy(x => x.Status).ToList();
            }

            SetVisibilityOfButtons(button);
            _messageBox.ShowDialog();
            return _result;
        }

        private static void SetVisibilityOfButtons(MessageBoxButton button)
        {
            if (_messageBox == null)
            {
                return;
            }
            switch (button)
            {
                case MessageBoxButton.OK:
                    _messageBox.btnCancel.Visibility = Visibility.Collapsed;
                    _messageBox.btnNo.Visibility = Visibility.Collapsed;
                    _messageBox.btnYes.Visibility = Visibility.Collapsed;
                    _messageBox.btnOk.Focus();
                    break;
                case MessageBoxButton.OKCancel:
                    _messageBox.btnNo.Visibility = Visibility.Collapsed;
                    _messageBox.btnYes.Visibility = Visibility.Collapsed;
                    _messageBox.btnCancel.Focus();
                    break;
                case MessageBoxButton.YesNo:
                    _messageBox.btnOk.Visibility = Visibility.Collapsed;
                    _messageBox.btnCancel.Visibility = Visibility.Collapsed;
                    _messageBox.btnNo.Focus();
                    break;
                case MessageBoxButton.YesNoCancel:
                    _messageBox.btnOk.Visibility = Visibility.Collapsed;
                    _messageBox.btnCancel.Focus();
                    break;
                default:
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_messageBox is null) return;

            if (sender == btnOk)
            {
                _result = MessageBoxResult.OK;
            }
            else if (sender == btnYes)
            {
                _result = MessageBoxResult.Yes;
            }
            else if (sender == btnNo)
            {
                _result = MessageBoxResult.No;
            }
            else if (sender == btnCancel)
            {
                _result = MessageBoxResult.Cancel;
            }
            else
            {
                _result = MessageBoxResult.None;
            }
            _messageBox.Close();
            _messageBox = null;
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }

    public enum MessageBoxType
    {
        ConfirmationWithYesNo = 0,
        ConfirmationWithYesNoCancel,
        Information,
        Error,
        Warning
    }
}
