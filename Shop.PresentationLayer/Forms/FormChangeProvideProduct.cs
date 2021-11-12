using System;
using System.Linq;
using System.Windows.Forms;
using Shop.DataAccessLayer.Models;
using Shop.PresentationLayer.Tools;
using Shop.DataAccessLayer.Repository;

namespace Shop.PresentationLayer.Forms
{
    public partial class FormChangeProvideProduct : Form
    {
        private readonly FormMain _formMain;
        private readonly ProvideProduct _provideProduct;
        private readonly IRepository<ProvideProduct> _repositoryProvideProduct;
        private readonly IRepository<Order> _repositoryOrder;
        private readonly IRepository<Product> _repositoryProduct;

        public FormChangeProvideProduct(FormMain formMain, ProvideProduct provideProduct, 
            IRepository<ProvideProduct> repositoryProvideProduct, IRepository<Order> repositoryOrder, 
            IRepository<Product> repositoryProduct)
        {
            InitializeComponent();
            _formMain = formMain;
            _provideProduct = provideProduct;
            _repositoryProvideProduct = repositoryProvideProduct;
            _repositoryOrder = repositoryOrder;
            _repositoryProduct = repositoryProduct;
        }

        private void FormChangeProvideProduct_Load(object sender, EventArgs e)
        {
            this.FillCombobox(comboBox1, _repositoryOrder.GetAll().Select(x => x.Number.ToString()).ToArray());
            this.FillCombobox(comboBox2, _repositoryProduct.GetAll().Select(x => x.Name).ToArray());
            textBox1.Text = _provideProduct.Count.ToString();
            textBox2.Text = _provideProduct.Price.ToString("F");
            var order = _repositoryOrder.GetById(_provideProduct.OrderId);
            var product = _repositoryProduct.GetById(_provideProduct.ProductId);
            comboBox1.SelectedItem = order.Number.ToString();
            comboBox2.SelectedItem = product.Name;
            comboBox3.SelectedItem = _provideProduct.TypeProvide;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var order = _repositoryOrder.GetModelByProperty(comboBox1.SelectedItem.ToString(), "Number");
            var product = _repositoryProduct.GetModelByProperty(comboBox2.SelectedItem.ToString(), "Name");
            var provideProduct = new ProvideProduct
            {
                Id = _provideProduct.Id,
                Count = Convert.ToInt32(textBox1.Text),
                Price = Convert.ToDecimal(textBox2.Text),
                TypeProvide = comboBox3.SelectedItem.ToString(),
                OrderId = order.Id,
                ProductId = product.Id,
            };

            _repositoryProvideProduct.Update(provideProduct);
            _formMain.button5_Click(null, null);
            this.Close();
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
