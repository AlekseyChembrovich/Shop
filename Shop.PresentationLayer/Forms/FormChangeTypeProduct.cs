using System;
using System.Windows.Forms;
using Shop.DataAccessLayer.Models;
using Shop.DataAccessLayer.Repository;

namespace Shop.PresentationLayer.Forms
{
    public partial class FormChangeTypeProduct : Form
    {
        private readonly FormMain _formMain;
        private readonly TypeProduct _typeProduct;
        private readonly IRepository<TypeProduct> _repositoryTypeProduct;

        public FormChangeTypeProduct(FormMain formMain, TypeProduct typeProduct,
            IRepository<TypeProduct> repositoryTypeProduct)
        {
            InitializeComponent();
            _formMain = formMain;
            _typeProduct = typeProduct;
            _repositoryTypeProduct = repositoryTypeProduct;
        }

        private void FormChangeTypeProduct_Load(object sender, EventArgs e)
        {
            textBox1.Text = _typeProduct.Name;
            textBox2.Text = _typeProduct.ExtraCharge.ToString("F");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var typeProduct = new TypeProduct
            {
                Id = _typeProduct.Id,
                Name = textBox1.Text,
                ExtraCharge = Convert.ToSingle(textBox2.Text)
            };

            _repositoryTypeProduct.Update(typeProduct);
            _formMain.button9_Click(null, null);
            this.Close();
        }

        private void EnterOnlyLetter(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || e.KeyChar is (char)Keys.Back or (char)Keys.Delete)
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;
        }

        private void EnterFractionalNumber(object sender, KeyPressEventArgs e)
        {
            var textBox = sender as TextBox;
            var isContain = textBox?.Text.Contains(',') ?? false;
            if (isContain && e.KeyChar is ',')
            {
                e.Handled = true;
                return;
            }

            if (char.IsDigit(e.KeyChar) || e.KeyChar is (char)Keys.Back or (char)Keys.Delete or ',')
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;
        }
    }
}
