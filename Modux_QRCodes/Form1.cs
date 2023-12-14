using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;

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
            bool[][] arr = {
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
            //byte[] data = QRMethods.V1GetData(arr);
            /*
            bool[] values = new bool[256];
            for (byte i = 0; i <= (byte)0xff; i++)
            {
                values[i] = true;
                bool[] GF = new bool[256];
                GF[0] = true;
                byte temp = 1;
                for (int j = 0; j < 256; j++)
                {
                    GF[temp] = true;
                    temp = (byte)(temp * i);
                }
                foreach (bool value in GF)
                {
                    if (!value)
                    {
                        values[i] = false;
                        break;
                    }
                }
            }
            for (int j = 0; j < 256; j++)
            {
                if (values[j])
                {
                    Debug.WriteLine(j);
                }
            }
            Debug.WriteLine((byte)((byte)0b00101000 * (byte)0b00001001));
            */
            if (imageDisplay.Image == null)
            {
                label1.Text = "No Image Selected";
            }
            else
            {
                bool[][] QRCode = ImageProcessing.ImageToQR(imageDisplay.Image);
                byte[] data = QRMethods.V1GetData(QRCode);
                decodeOutput.Text = System.Text.Encoding.ASCII.GetString(data);
            }
        }
    }
}
