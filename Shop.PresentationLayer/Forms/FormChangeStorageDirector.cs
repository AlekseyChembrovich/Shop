using System;
using System.Windows.Forms;
using Shop.DataAccessLayer.Models;
using Shop.DataAccessLayer.Repository;

namespace Shop.PresentationLayer.Forms
{
    public partial class FormChangeStorageDirector : Form
    {
        private readonly FormMain _formMain;
        private readonly StorageDirector _storageDirector;
        private readonly IRepository<StorageDirector> _repositoryStorageDirector;

        public FormChangeStorageDirector(FormMain formMain, StorageDirector storageDirector,
            IRepository<StorageDirector> repositoryStorageDirector)
        {
            InitializeComponent();
            _formMain = formMain;
            _storageDirector = storageDirector;
            _repositoryStorageDirector = repositoryStorageDirector;
        }

        private void FormChangeStorageDirector_Load(object sender, EventArgs e)
        {
            textBox1.Text = _storageDirector.Surname;
            textBox2.Text = _storageDirector.Name;
            textBox3.Text = _storageDirector.Patronymic;
            dateTimePicker1.Value = _storageDirector.Birthday.Date;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var storageDirector = new StorageDirector
            {
                Id = _storageDirector.Id,
                Surname = textBox1.Text,
                Name = textBox2.Text,
                Patronymic = textBox3.Text,
                Birthday = dateTimePicker1.Value.Date
            };

            _repositoryStorageDirector.Update(storageDirector);
            _formMain.button8_Click(null, null);
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
    }
}
