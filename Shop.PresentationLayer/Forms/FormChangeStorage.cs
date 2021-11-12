using System;
using System.Linq;
using System.Windows.Forms;
using Shop.DataAccessLayer.Models;
using Shop.PresentationLayer.Tools;
using Shop.DataAccessLayer.Repository;

namespace Shop.PresentationLayer.Forms
{
    public partial class FormChangeStorage : Form
    {
        private readonly FormMain _formMain;
        private readonly Storage _storage;
        private readonly IRepository<Storage> _repositoryStorage;
        private readonly IRepository<StorageDirector> _repositoryStorageDirector;

        public FormChangeStorage(FormMain formMain, Storage storage,
            IRepository<Storage> repositoryStorage, IRepository<StorageDirector> repositoryStorageDirector)
        {
            InitializeComponent();
            _formMain = formMain;
            _storage = storage;
            _repositoryStorage = repositoryStorage;
            _repositoryStorageDirector = repositoryStorageDirector;
        }

        private void FormChangeStorage_Load(object sender, EventArgs e)
        {
            this.FillCombobox(comboBox1, _repositoryStorageDirector.GetAll().Select(x => x.Surname).ToArray());
            textBox1.Text = _storage.Name;
            textBox2.Text = _storage.Address;
            var storageDirector = _repositoryStorageDirector.GetById(_storage.StorageDirectorId);
            comboBox1.SelectedItem = storageDirector.Surname;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var storageDirector = _repositoryStorageDirector
                .GetModelByProperty(comboBox1.SelectedItem.ToString(), "Surname");
            var storage = new Storage
            {
                Id = _storage.Id,
                Name = textBox1.Text,
                Address = textBox2.Text,
                StorageDirectorId = storageDirector.Id
            };

            _repositoryStorage.Update(storage);
            _formMain.button7_Click(null, null);
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
