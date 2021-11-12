using System;
using System.Linq;
using System.Windows.Forms;
using Shop.DataAccessLayer.Models;
using Shop.PresentationLayer.Tools;
using Shop.DataAccessLayer.Repository;

namespace Shop.PresentationLayer.Forms
{
    public partial class FormChangeProduct : Form
    {
        private readonly FormMain _formMain;
        private readonly Product _product;
        private readonly IRepository<Product> _repositoryProduct;
        private readonly IRepository<TypeProduct> _repositoryTypeProduct;
        private readonly IRepository<Storage> _repositoryStorage;

        public FormChangeProduct(FormMain formMain, Product product, IRepository<Product> repositoryProduct,
            IRepository<TypeProduct> repositoryTypeProduct, IRepository<Storage> repositoryStorage)
        {
            InitializeComponent();
            _formMain = formMain;
            _product = product;
            _repositoryProduct = repositoryProduct;
            _repositoryTypeProduct = repositoryTypeProduct;
            _repositoryStorage = repositoryStorage;
        }

        private void FormChangeProduct_Load(object sender, EventArgs e)
        {
            this.FillCombobox(comboBox1, _repositoryTypeProduct.GetAll().Select(x => x.Name).ToArray());
            this.FillCombobox(comboBox2, _repositoryStorage.GetAll().Select(x => x.Name).ToArray());
            textBox1.Text = _product.Name;
            textBox2.Text = _product.Balance.ToString();
            textBox3.Text = _product.Price.ToString("F");
            textBox4.Text = _product.MinValue.ToString();
            var typeProduct = _repositoryTypeProduct.GetById(_product.TypeProductId);
            var storage = _repositoryStorage.GetById(_product.StorageId);
            comboBox1.SelectedItem = typeProduct.Name;
            comboBox2.SelectedItem = storage.Name;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text) ||
                comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var typeProduct = _repositoryTypeProduct.GetModelByProperty(comboBox1.SelectedItem.ToString(), "Name");
            var storage = _repositoryStorage.GetModelByProperty(comboBox2.SelectedItem.ToString(), "Name");
            var product = new Product
            {
                Id = _product.Id,
                Name = textBox1.Text,
                Balance = Convert.ToInt32(textBox2.Text),
                Price = Convert.ToDecimal(textBox3.Text),
                MinValue = Convert.ToInt32(textBox4.Text),
                TypeProductId = typeProduct.Id,
                StorageId = storage.Id
            };

            _repositoryProduct.Update(product);
            _formMain.button4_Click(null, null);
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

        private void EnterOnlyDigit(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar is (char)Keys.Back or (char)Keys.Delete)
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
