using System;
using System.Windows.Forms;
using Shop.DataAccessLayer.Models;
using Shop.DataAccessLayer.Repository;

namespace Shop.PresentationLayer.Forms
{
    public partial class FormChangeProvider : Form
    {
        private readonly FormMain _formMain;
        private readonly Provider _provider;
        private readonly IRepository<Provider> _repositoryProvider;

        public FormChangeProvider(FormMain formMain, Provider provider,
            IRepository<Provider> repositoryProvider)
        {
            InitializeComponent();
            _formMain = formMain;
            _provider = provider;
            _repositoryProvider = repositoryProvider;
        }

        private void FormChangeProvider_Load(object sender, EventArgs e)
        {
            textBox1.Text = _provider.Name;
            textBox2.Text = _provider.Address;
            textBox3.Text = _provider.TaxpayerIdentification;
            maskedTextBox1.Text = _provider.Phone;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) || maskedTextBox1.Text.Trim(' ').Length < 18)
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var provider = new Provider
            {
                Id = _provider.Id,
                Name = textBox1.Text,
                Address = textBox2.Text,
                TaxpayerIdentification = textBox3.Text,
                Phone = maskedTextBox1.Text
            };

            _repositoryProvider.Update(provider);
            _formMain.button6_Click(null, null);
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

        private void EnterAddress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetterOrDigit(e.KeyChar) || e.KeyChar is '.' or ',' or (char)Keys.Back or (char)Keys.Delete)
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;
        }
    }
}
