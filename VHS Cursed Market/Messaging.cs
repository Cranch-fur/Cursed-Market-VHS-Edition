using System.Windows.Forms;

namespace VHS_Cursed_Market
{
    public static class Messaging
    {
        public static void ShowMessage(string message,
            MessageBoxButtons messageButtons = MessageBoxButtons.OK,
                MessageBoxIcon messageType = MessageBoxIcon.None,
                    MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
        {
            MessageBox.Show(message, Globals.SelfExecutableFriendlyName, messageButtons, messageType, defaultButton);
        }

        public static DialogResult ShowDialog(string message,
            MessageBoxButtons messageButtons = MessageBoxButtons.OK,
                MessageBoxIcon messageType = MessageBoxIcon.None,
                    MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
        {
            return MessageBox.Show(message, Globals.SelfExecutableFriendlyName, messageButtons, messageType, defaultButton);
        }
    }
}
