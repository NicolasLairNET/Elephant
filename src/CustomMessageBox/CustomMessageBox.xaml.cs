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

        static CustomMessageBox? messageBox;
        static MessageBoxResult result = MessageBoxResult.No;
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
            messageBox = new CustomMessageBox
            {
                txtMsg = { Text = text },
                Title = caption,
            };

            if (importStatus?.Count > 0)
            {
                messageBox.ImportStatus.Visibility = Visibility.Visible;
                messageBox.ImportStatus.ItemsSource = importStatus.OrderBy(x => x.Status).ToList();
            }

            SetVisibilityOfButtons(button);
            messageBox.ShowDialog();
            return result;
        }

        private static void SetVisibilityOfButtons(MessageBoxButton button)
        {
            if (messageBox == null)
            {
                return;
            }
            switch (button)
            {
                case MessageBoxButton.OK:
                    messageBox.btnCancel.Visibility = Visibility.Collapsed;
                    messageBox.btnNo.Visibility = Visibility.Collapsed;
                    messageBox.btnYes.Visibility = Visibility.Collapsed;
                    messageBox.btnOk.Focus();
                    break;
                case MessageBoxButton.OKCancel:
                    messageBox.btnNo.Visibility = Visibility.Collapsed;
                    messageBox.btnYes.Visibility = Visibility.Collapsed;
                    messageBox.btnCancel.Focus();
                    break;
                case MessageBoxButton.YesNo:
                    messageBox.btnOk.Visibility = Visibility.Collapsed;
                    messageBox.btnCancel.Visibility = Visibility.Collapsed;
                    messageBox.btnNo.Focus();
                    break;
                case MessageBoxButton.YesNoCancel:
                    messageBox.btnOk.Visibility = Visibility.Collapsed;
                    messageBox.btnCancel.Focus();
                    break;
                default:
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (messageBox is null) return;

            if (sender == btnOk)
            {
                result = MessageBoxResult.OK;
            }
            else if (sender == btnYes)
            {
                result = MessageBoxResult.Yes;
            }
            else if (sender == btnNo)
            {
                result = MessageBoxResult.No;
            }
            else if (sender == btnCancel)
            {
                result = MessageBoxResult.Cancel;
            }
            else
            {
                result = MessageBoxResult.None;
            }
            messageBox.Close();
            messageBox = null;
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
