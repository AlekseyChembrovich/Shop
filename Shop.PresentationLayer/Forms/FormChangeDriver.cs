using System;
using System.Windows.Forms;
using Shop.DataAccessLayer.Models;
using Shop.DataAccessLayer.Repository;

namespace Shop.PresentationLayer.Forms
{
    public partial class FormChangeDriver : Form
    {
        private readonly FormMain _formMain;
        private readonly Driver _driver;
        private readonly IRepository<Driver> _repositoryDriver;

        public FormChangeDriver(FormMain formMain, Driver driver,
            IRepository<Driver> repositoryDriver)
        {
            InitializeComponent();
            _formMain = formMain;
            _driver = driver;
            _repositoryDriver = repositoryDriver;
        }

        private void FormChangeDriver_Load(object sender, EventArgs e)
        {
            textBox1.Text = _driver.Surname;
            textBox2.Text = _driver.Name;
            textBox3.Text = _driver.Patronymic;
            textBox4.Text = _driver.Experience.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var driver = new Driver
            {
                Id = _driver.Id,
                Surname = textBox1.Text,
                Name = textBox2.Text,
                Patronymic = textBox3.Text,
                Experience = Convert.ToInt32(textBox4.Text)
            };

            _repositoryDriver.Update(driver);
            _formMain.button2_Click(null, null);
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
    }
}
