using System;
using System.Windows.Forms;
using Shop.DataAccessLayer.Models;
using Shop.DataAccessLayer.Repository;

namespace Shop.PresentationLayer.Forms
{
    public partial class FormChangeClient : Form
    {
        private readonly FormMain _formMain;
        private readonly Client _client;
        private readonly IRepository<Client> _repositoryClient;

        public FormChangeClient(FormMain formMain, Client client,
            IRepository<Client> repositoryClient)
        {
            InitializeComponent();
            _formMain = formMain;
            _client = client;
            _repositoryClient = repositoryClient;
        }

        private void FormChangeClient_Load(object sender, EventArgs e)
        {
            textBox1.Text = _client.Name;
            textBox2.Text = _client.Address;
            textBox3.Text = _client.TaxpayerIdentification;
            maskedTextBox1.Text = _client.Phone;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) || maskedTextBox1.Text.Trim(' ').Length < 18)
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var client = new Client
            {
                Id = _client.Id,
                Name = textBox1.Text,
                Address = textBox2.Text,
                TaxpayerIdentification = textBox3.Text,
                Phone = maskedTextBox1.Text
            };

            _repositoryClient.Update(client);
            _formMain.button1_Click(null, null);
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
