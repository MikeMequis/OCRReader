using System;
using System.Windows.Forms;

namespace OCRReader2
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string path;
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png";

            if (file.ShowDialog() == DialogResult.OK)
            {
                path = file.FileName;

                string extension = System.IO.Path.GetExtension(path).ToLower();

                if (extension != ".jpg" && extension != ".png")
                {
                    throw new Exception("Apenas arquivos de imagem (.jpg ou .png) são permitidos.");
                }

                txtPath.Text = path;
            }
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            try
            {
                string[] ocrTexts = ImageProcessor.ResizeAndPerformOCR(txtPath.Text);

                txtResult.Clear();

                foreach (string text in ocrTexts)
                {
                    txtResult.Text += text + Environment.NewLine;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPath.Clear();
            txtResult.Clear();
        }
    }
}