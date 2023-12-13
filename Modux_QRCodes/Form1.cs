using System.Collections;

namespace Modux_QRCodes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void fileBtn_Click(object sender, EventArgs e)
        {
            // Source: https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.openfiledialog?view=windowsdesktop-8.0
            using (OpenFileDialog keywordsOpen = new OpenFileDialog())
            {
                keywordsOpen.Filter = "Image files (*.png, *.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
                keywordsOpen.FilterIndex = 1;
                if (keywordsOpen.ShowDialog() == DialogResult.OK)
                {
                    imageDisplay.ImageLocation = keywordsOpen.FileName;
                }
            }
        }

        private void decodeBtn_Click(object sender, EventArgs e)
        {
            bool[][] boolArr = {
                [true,  true,  true,  true,  true,  true,  true,  false, true,  true,  false, false, true,  false, true,  true,  true,  true,  true,  true,  true ],
                [true,  false, false, false, false, false, true,  false, false, true,  false, false, true,  false, true,  false, false, false, false, false, true ],
                [true,  false, true,  true,  true,  false, true,  false, true,  false, true,  false, true,  false, true,  false, true,  true,  true,  false, true ],
                [true,  false, true,  true,  true,  false, true,  false, true,  false, false, true,  false, false, true,  false, true,  true,  true,  false, true ],
                [true,  false, true,  true,  true,  false, true,  false, true,  true,  true,  false, false, false, true,  false, true,  true,  true,  false, true ],
                [true,  false, false, false, false, false, true,  false, false, false, false, false, false, false, true,  false, false, false, false, false, true ],
                [true,  true,  true,  true,  true,  true,  true,  false, true,  false, true,  false, true,  false, true,  true,  true,  true,  true,  true,  true ],
                [false, false, false, false, false, false, false, false, false, true,  true,  false, false, false, false, false, false, false, false, false, false],
                [true,  true,  true,  true,  false, false, true,  false, true,  false, true,  false, false, true,  false, false, true,  true,  true,  false, true ],
                [true,  false, false, true,  true,  true,  false, true,  false, false, true,  false, true,  false, false, true,  false, true,  true,  false, true ],
                [true,  false, true,  false, true,  true,  true,  false, false, false, true,  false, true,  true,  true,  true,  true,  false, false, true,  true ],
                [true,  false, true,  true,  false, false, false, true,  true,  false, true,  false, true,  true,  false, false, false, true,  false, true,  false],
                [false, true,  true,  false, true,  false, true,  false, false, true,  true,  true,  false, true,  false, false, true,  true,  false, true,  false],
                [false, false, false, false, false, false, false, false, true,  false, true,  false, false, true,  false, true,  false, true,  false, true,  false],
                [true,  true,  true,  true,  true,  true,  true,  false, false, false, true,  false, false, true,  true,  false, true,  true,  true,  false, false],
                [true,  false, false, false, false, false, true,  false, false, true,  false, true,  true,  true,  true,  false, false, true,  true,  false, false],
                [true,  false, true,  true,  true,  false, true,  false, false, true,  true,  true,  false, false, true,  false, false, false, true,  true,  true ],
                [true,  false, true,  true,  true,  false, true,  false, true,  true,  false, false, false, true,  false, false, true,  true,  false, true,  false],
                [true,  false, true,  true,  true,  false, true,  false, true,  false, true,  true,  false, true,  false, false, true,  false, true,  false, false],
                [true,  false, false, false, false, false, true,  false, true,  true,  true,  true,  true,  false, true,  false, true,  true,  false, false, true ],
                [true,  true,  true,  true,  true,  true,  true,  false, true,  true,  false, true,  true,  false, false, true,  false, false, false, false, false]
            };
            BitArray[] arr = boolArr.Select(x => new BitArray(x)).ToArray();
            (int EC, int mask) = QRMethods.GetFormatInfo(arr);
            decodeOutput.Text = EC.ToString() + " " + mask.ToString();
        }
    }
}
