using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Shop.DataAccessLayer.Models;
using Shop.PresentationLayer.Tools;
using Shop.PresentationLayer.Forms;
using Shop.DataAccessLayer.Repository;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using Shop.DataAccessLayer.Repository.EntityFramework;

namespace Shop.PresentationLayer
{
    public partial class FormMain : Form
    {
        public CurrentTable CurrentTable = CurrentTable.Client;
        
        private readonly IRepository<Client> _repositoryClient;
        private readonly IRepository<Driver> _repositoryDriver;
        private readonly IRepository<Order> _repositoryOrder;
        private readonly IRepository<Product> _repositoryProduct;
        private readonly IRepository<ProvideProduct> _repositoryProvideProduct;
        private readonly IRepository<Provider> _repositoryProvider;
        private readonly IRepository<Storage> _repositoryStorage;
        private readonly IRepository<StorageDirector> _repositoryStorageDirector;
        private readonly IRepository<TypeProduct> _repositoryTypeProduct;

        private readonly List<Panel> _addPanels;
        private readonly List<Button> _tablesButtons;

        public FormMain()
        {
            InitializeComponent();
            var databaseContext = new DatabaseContext();
            #region Init repositoties

            _repositoryClient = new BaseRepository<Client>(databaseContext);
            _repositoryDriver = new BaseRepository<Driver>(databaseContext);
            _repositoryOrder = new RepositoryOrder(databaseContext);
            _repositoryProduct = new RepositoryProduct(databaseContext);
            _repositoryProvideProduct = new RepositoryProvideProduct(databaseContext);
            _repositoryProvider = new BaseRepository<Provider>(databaseContext);
            _repositoryStorage = new RepositoryStorage(databaseContext);
            _repositoryStorageDirector = new BaseRepository<StorageDirector>(databaseContext);
            _repositoryTypeProduct = new BaseRepository<TypeProduct>(databaseContext);

            #endregion
            _addPanels = new List<Panel> { panel4, panel5, panel6, panel7, panel8, panel9, panel10, panel11, panel12 };
            _tablesButtons = new List<Button> { button1, button2, button3, button4, button5, button6, button7, button8, button9 };
            menuStrip1.ForeColor = Color.FromArgb(243, 233, 109);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            UpdateCombobox();
            button1_Click(null, null);
        }

        private void PickOutButtonCurrentTable(CurrentTable currentTable)
        {
            var pickOut = Color.FromArgb(74, 35, 119);
            var normal = Color.FromArgb(97, 82, 159);
            var index = (int)currentTable;
            _tablesButtons[index].BackColor = pickOut;
            _tablesButtons.Except(new[] { _tablesButtons[index] }).ToList().ForEach(x =>
            {
                x.BackColor = normal;
            });
        }

        private void ToggleContextMenuItems(CurrentTable currentTable)
        {
            switch (currentTable)
            {
                case CurrentTable.Client:
                    отобратьЗаказыToolStripMenuItem.Visible = true;
                    отобратьПродуктыToolStripMenuItem.Visible = false;
                    break;
                case CurrentTable.Storage:
                    отобратьЗаказыToolStripMenuItem.Visible = false;
                    отобратьПродуктыToolStripMenuItem.Visible = true;
                    break;
                default:
                    отобратьЗаказыToolStripMenuItem.Visible = false;
                    отобратьПродуктыToolStripMenuItem.Visible = false;
                    break;
            }
        }

        private void UpdateCombobox()
        {
            this.FillCombobox(comboBox1, _repositoryClient.GetAll().Select(x => x.Name).ToArray());
            this.FillCombobox(comboBox2, _repositoryProvider.GetAll().Select(x => x.Name).ToArray());
            this.FillCombobox(comboBox3, _repositoryDriver.GetAll().Select(x => x.Surname).ToArray());
            this.FillCombobox(comboBox4, _repositoryStorage.GetAll().Select(x => x.Name).ToArray());
            this.FillCombobox(comboBox5, _repositoryTypeProduct.GetAll().Select(x => x.Name).ToArray());
            this.FillCombobox(comboBox6, _repositoryProduct.GetAll().Select(x => x.Name).ToArray());
            this.FillCombobox(comboBox7, _repositoryOrder.GetAll().Select(x => x.Number.ToString()).ToArray());
            this.FillCombobox(comboBox9, _repositoryStorageDirector.GetAll().Select(x => x.Surname).ToArray());
            this.FillCombobox(comboBox10, _repositoryClient.GetAll().Select(x => x.Name).ToArray());
        }

        private void ToggleFiltrationMenu(bool isShow)
        {
            panel14.Visible = isShow;
            panel14.Dock = isShow ? DockStyle.Top : DockStyle.None;
        }

        #region Print methods

        public void button1_Click(object sender, EventArgs e)
        {
            CurrentTable = CurrentTable.Client;
            ToggleContextMenuItems(CurrentTable);
            PickOutButtonCurrentTable(CurrentTable);
            ToggleFiltrationMenu(false);
            this.OpenAddPanel(panel4, _addPanels);
            this.GenerateColumns(
                listView1,
                120,
                "Код", "Имя", "Адрес", "Идентификатор налогоплательщика", "Телефон"
            );
            foreach (var item in _repositoryClient.GetAll())
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.Name,
                    item.Address,
                    item.TaxpayerIdentification,
                    item.Phone
                });
                listView1.Items.Add(newItem);
            }

            // Code

            UpdateCombobox();
        }

        public void button2_Click(object sender, EventArgs e)
        {
            CurrentTable = CurrentTable.Driver;
            ToggleContextMenuItems(CurrentTable);
            PickOutButtonCurrentTable(CurrentTable);
            ToggleFiltrationMenu(false);
            this.OpenAddPanel(panel5, _addPanels);
            this.GenerateColumns(
                listView1,
                120,
                "Код", "Фамилия", "Имя", "Отчество", "Опыт"
            );
            foreach (var item in _repositoryDriver.GetAll())
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.Surname,
                    item.Name,
                    item.Patronymic,
                    item.Experience.ToString()
                });
                listView1.Items.Add(newItem);
            }

            // Code

            UpdateCombobox();
        }

        private void PrintOrders(IEnumerable<Order> orders)
        {
            CurrentTable = CurrentTable.Order;
            ToggleContextMenuItems(CurrentTable);
            PickOutButtonCurrentTable(CurrentTable);
            ToggleFiltrationMenu(true);
            this.OpenAddPanel(panel6, _addPanels);
            this.GenerateColumns(
                listView1,
                120,
                "Код", "Номер", "Дата", "Водитель", "Клиент", "Поставщик"
            );
            foreach (var item in orders)
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.Number.ToString(),
                    item.Date.ToShortDateString(),
                    item.Driver.Surname,
                    item.Client.Name,
                    item.Provider.Name
                });
                listView1.Items.Add(newItem);
            }

            UpdateCombobox();
        }

        public void button3_Click(object sender, EventArgs e)
        {
            if (_repositoryOrder is RepositoryOrder repositoryOrder)
            {
                PrintOrders(repositoryOrder.GetAllIncludeForeignKey());
            }
        }

        private void PrintProducts(IEnumerable<Product> products)
        {
            foreach (var item in products)
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.Name,
                    item.Balance.ToString(),
                    item.Price.ToString("F"),
                    item.MinValue.ToString(),
                    item.TypeProduct.Name,
                    item.Storage.Name
                });
                listView1.Items.Add(newItem);
            }
        }

        public void button4_Click(object sender, EventArgs e)
        {
            CurrentTable = CurrentTable.Product;
            ToggleContextMenuItems(CurrentTable);
            PickOutButtonCurrentTable(CurrentTable);
            ToggleFiltrationMenu(false);
            this.OpenAddPanel(panel7, _addPanels);
            this.GenerateColumns(
                listView1,
                120,
                "Код", "Имя", "Остаток", "Цена", "Минимальное значение", "Тип продукта", "Хранилище"
            );
            if (_repositoryProduct is RepositoryProduct repositoryProduct)
            {
                PrintProducts(repositoryProduct.GetAllIncludeForeignKey());
            }

            // Code

            UpdateCombobox();
        }

        public void button5_Click(object sender, EventArgs e)
        {
            CurrentTable = CurrentTable.ProvideProduct;
            ToggleContextMenuItems(CurrentTable);
            PickOutButtonCurrentTable(CurrentTable);
            ToggleFiltrationMenu(false);
            this.OpenAddPanel(panel8, _addPanels);
            this.GenerateColumns(
                listView1,
                120,
                "Код", "Количество", "Цена", "Тип поставки", "Номер заказа", "Имя продукта"
            );
            foreach (var item in (_repositoryProvideProduct as RepositoryProvideProduct)?.GetAllIncludeForeignKey())
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.Count.ToString(),
                    item.Price.ToString("F"),
                    item.TypeProvide,
                    item.Order.Number.ToString(),
                    item.Product.Name
                });
                listView1.Items.Add(newItem);
            }

            // Code

            UpdateCombobox();
        }

        public void button6_Click(object sender, EventArgs e)
        {
            CurrentTable = CurrentTable.Provider;
            ToggleContextMenuItems(CurrentTable);
            PickOutButtonCurrentTable(CurrentTable);
            ToggleFiltrationMenu(false);
            this.OpenAddPanel(panel9, _addPanels);
            this.GenerateColumns(
                listView1,
                120,
                "Код", "Имя", "Адрес", "Идентификатор налогоплательщика", "Телефон"
            );
            foreach (var item in _repositoryProvider.GetAll())
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.Name,
                    item.Address,
                    item.TaxpayerIdentification,
                    item.Phone
                });
                listView1.Items.Add(newItem);
            }

            // Code

            UpdateCombobox();
        }

        public void button7_Click(object sender, EventArgs e)
        {
            CurrentTable = CurrentTable.Storage;
            ToggleContextMenuItems(CurrentTable);
            PickOutButtonCurrentTable(CurrentTable);
            ToggleFiltrationMenu(false);
            this.OpenAddPanel(panel10, _addPanels);
            this.GenerateColumns(
                listView1,
                120,
                "Код", "Имя", "Адрес", "Директор"
            );
            foreach (var item in (_repositoryStorage as RepositoryStorage)?.GetAllIncludeForeignKey())
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.Name,
                    item.Address,
                    item.StorageDirector.Surname
                });
                listView1.Items.Add(newItem);
            }

            // Code

            UpdateCombobox();
        }

        public void button8_Click(object sender, EventArgs e)
        {
            CurrentTable = CurrentTable.StorageDirector;
            ToggleContextMenuItems(CurrentTable);
            PickOutButtonCurrentTable(CurrentTable);
            ToggleFiltrationMenu(false);
            this.OpenAddPanel(panel11, _addPanels);
            this.GenerateColumns(
                listView1,
                120,
                "Код", "Фамилия", "Имя", "Отчество", "Дата рождения"
            );
            foreach (var item in _repositoryStorageDirector.GetAll())
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.Surname,
                    item.Name,
                    item.Patronymic,
                    item.Birthday.ToShortDateString()
                });
                listView1.Items.Add(newItem);
            }

            // Code

            UpdateCombobox();
        }

        public void button9_Click(object sender, EventArgs e)
        {
            CurrentTable = CurrentTable.TypeProduct;
            ToggleContextMenuItems(CurrentTable);
            PickOutButtonCurrentTable(CurrentTable);
            ToggleFiltrationMenu(false);
            this.OpenAddPanel(panel12, _addPanels);
            this.GenerateColumns(
                listView1,
                120,
                "Код", "Имя", "Размер НДС"
            );
            foreach (var item in _repositoryTypeProduct.GetAll())
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.Name,
                    item.ExtraCharge.ToString("F")
                });
                listView1.Items.Add(newItem);
            }

            // Code

            UpdateCombobox();
        }

        #endregion

        #region Add methods

        // Add

        private void button10_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) || maskedTextBox1.Text.Trim(' ').Length < 18)
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var client = new Client
            {
                Name = textBox1.Text,
                Address = textBox2.Text,
                TaxpayerIdentification = textBox3.Text,
                Phone = maskedTextBox1.Text
            };

            _repositoryClient.Insert(client);
            button1_Click(null, null);
            button11_Click(null, null);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox8.Text) || string.IsNullOrWhiteSpace(textBox7.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text) || string.IsNullOrWhiteSpace(textBox24.Text))
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var driver = new Driver
            {
                Surname = textBox8.Text,
                Name = textBox7.Text,
                Patronymic = textBox6.Text,
                Experience = Convert.ToInt32(textBox24.Text)
            };

            _repositoryDriver.Insert(driver);
            button2_Click(null, null);
            button12_Click(null, null);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox10.Text) || comboBox1.SelectedIndex == -1 ||
                comboBox2.SelectedIndex == -1 || comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var client = _repositoryClient.GetModelByProperty(comboBox1.SelectedItem.ToString(), "Name");
            var provider = _repositoryProvider.GetModelByProperty(comboBox2.SelectedItem.ToString(), "Name");
            var driver = _repositoryDriver.GetModelByProperty(comboBox3.SelectedItem.ToString(), "Surname");
            var order = new Order
            {
                Number = Convert.ToInt32(textBox10.Text),
                Date = dateTimePicker2.Value.Date,
                ClientId = client.Id,
                ProviderId = provider.Id,
                DriverId = driver.Id
            };

            _repositoryOrder.Insert(order);
            button3_Click(null, null);
            button14_Click(null, null);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox9.Text) ||
                string.IsNullOrWhiteSpace(textBox11.Text) || string.IsNullOrWhiteSpace(textBox12.Text) ||
                comboBox5.SelectedIndex == -1 || comboBox4.SelectedIndex == -1)
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var typeProduct = _repositoryTypeProduct.GetModelByProperty(comboBox5.SelectedItem.ToString(), "Name");
            var storage = _repositoryStorage.GetModelByProperty(comboBox4.SelectedItem.ToString(), "Name");
            var product = new Product
            {
                Name = textBox5.Text,
                Balance = Convert.ToInt32(textBox9.Text),
                Price = Convert.ToDecimal(textBox11.Text),
                MinValue = Convert.ToInt32(textBox12.Text),
                TypeProductId = typeProduct.Id,
                StorageId = storage.Id
            };

            _repositoryProduct.Insert(product);
            button4_Click(null, null);
            button16_Click(null, null);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox16.Text) || string.IsNullOrWhiteSpace(textBox15.Text) ||
                comboBox8.SelectedIndex == -1 || comboBox7.SelectedIndex == -1 || comboBox6.SelectedIndex == -1)
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var order = _repositoryOrder.GetModelByProperty(comboBox7.SelectedItem.ToString(), "Number");
            var product = _repositoryStorage.GetModelByProperty(comboBox6.SelectedItem.ToString(), "Name");
            var provideProduct = new ProvideProduct
            {
                Count = Convert.ToInt32(textBox16.Text),
                Price = Convert.ToDecimal(textBox15.Text),
                TypeProvide = comboBox8.SelectedItem.ToString(),
                OrderId = order.Id,
                ProductId = product.Id
            };

            _repositoryProvideProduct.Insert(provideProduct);
            button5_Click(null, null);
            button18_Click(null, null);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox18.Text) || string.IsNullOrWhiteSpace(textBox17.Text) ||
                string.IsNullOrWhiteSpace(textBox14.Text) || maskedTextBox2.Text.Trim(' ').Length < 18) // +375-(33)-111-11-11 (19) 
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var provider = new Provider
            {
                Name = textBox18.Text,
                Address = textBox17.Text,
                TaxpayerIdentification = textBox14.Text,
                Phone = maskedTextBox2.Text
            };

            _repositoryProvider.Insert(provider);
            button6_Click(null, null);
            button20_Click(null, null);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox21.Text) || string.IsNullOrWhiteSpace(textBox22.Text) ||
                comboBox9.SelectedIndex == -1)
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var storageDirector =
                _repositoryStorageDirector.GetModelByProperty(comboBox9.SelectedItem.ToString(), "Surname");
            var storage = new Storage
            {
                Name = textBox21.Text,
                Address = textBox22.Text,
                StorageDirectorId = storageDirector.Id
            };

            _repositoryStorage.Insert(storage);
            button7_Click(null, null);
            button22_Click(null, null);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox23.Text) || string.IsNullOrWhiteSpace(textBox20.Text) ||
                string.IsNullOrWhiteSpace(textBox19.Text))
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var storageDirector = new StorageDirector
            {
                Surname = textBox23.Text,
                Name = textBox20.Text,
                Patronymic = textBox19.Text,
                Birthday = dateTimePicker1.Value.Date
            };

            _repositoryStorageDirector.Insert(storageDirector);
            button8_Click(null, null);
            button24_Click(null, null);
        }

        private void button27_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox28.Text) || string.IsNullOrWhiteSpace(textBox27.Text))
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var typeProduct = new TypeProduct
            {
                Name = textBox28.Text,
                ExtraCharge = Convert.ToSingle(textBox27.Text)
            };

            _repositoryTypeProduct.Insert(typeProduct);
            button9_Click(null, null);
            button26_Click(null, null);
        }

        // Clear

        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            maskedTextBox1.Text = "";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox8.Text = "";
            textBox7.Text = "";
            textBox6.Text = "";
            textBox24.Text = "";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            textBox10.Text = "";
            dateTimePicker2.Value = DateTime.Now.Date;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            textBox9.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            comboBox5.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            textBox16.Text = "";
            textBox15.Text = "";
            comboBox8.SelectedIndex = -1;
            comboBox7.SelectedIndex = -1;
            comboBox6.SelectedIndex = -1;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            textBox18.Text = "";
            textBox17.Text = "";
            textBox14.Text = "";
            maskedTextBox2.Text = "";
        }

        private void button22_Click(object sender, EventArgs e)
        {
            textBox21.Text = "";
            textBox22.Text = "";
            comboBox9.SelectedIndex = -1;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            textBox23.Text = "";
            textBox20.Text = "";
            textBox19.Text = "";
            dateTimePicker1.Value = DateTime.Now;
        }

        private void button26_Click(object sender, EventArgs e)
        {
            textBox28.Text = "";
            textBox27.Text = "";
        }

        #endregion

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Выберите строку таблицы!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var id = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            switch (CurrentTable)
            {
                case CurrentTable.Client:
                    var client = _repositoryClient.GetById(id);
                    _repositoryClient.Delete(client);
                    button1_Click(this, null);
                    break;
                case CurrentTable.Driver:
                    var driver = _repositoryDriver.GetById(id);
                    _repositoryDriver.Delete(driver);
                    button2_Click(this, null);
                    break;
                case CurrentTable.Order:
                    var order = _repositoryOrder.GetById(id);
                    _repositoryOrder.Delete(order);
                    button3_Click(this, null);
                    break;
                case CurrentTable.Product:
                    var product = _repositoryProduct.GetById(id);
                    _repositoryProduct.Delete(product);
                    button4_Click(this, null);
                    break;
                case CurrentTable.ProvideProduct:
                    var provideProduct = _repositoryProvideProduct.GetById(id);
                    _repositoryProvideProduct.Delete(provideProduct);
                    button5_Click(this, null);
                    break;
                case CurrentTable.Provider:
                    var provider = _repositoryProvider.GetById(id);
                    _repositoryProvider.Delete(provider);
                    button6_Click(this, null);
                    break;
                case CurrentTable.Storage:
                    var storage = _repositoryStorage.GetById(id);
                    _repositoryStorage.Delete(storage);
                    button7_Click(this, null);
                    break;
                case CurrentTable.StorageDirector:
                    var storageDirector = _repositoryStorageDirector.GetById(id);
                    _repositoryStorageDirector.Delete(storageDirector);
                    button8_Click(this, null);
                    break;
                case CurrentTable.TypeProduct:
                    var typeProduct = _repositoryTypeProduct.GetById(id);
                    _repositoryTypeProduct.Delete(typeProduct);
                    button9_Click(this, null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Выберите строку таблицы!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var id = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            switch (CurrentTable)
            {
                case CurrentTable.Client:
                    var client = _repositoryClient.GetById(id);
                    var formChangeClient = new FormChangeClient(this, client, _repositoryClient);
                    formChangeClient.ShowDialog();
                    break;
                case CurrentTable.Driver:
                    var driver = _repositoryDriver.GetById(id);
                    var formChangeDriver = new FormChangeDriver(this, driver, _repositoryDriver);
                    formChangeDriver.ShowDialog();
                    break;
                case CurrentTable.Order:
                    var order = _repositoryOrder.GetById(id);
                    var formChangeOrder = new FormChangeOrder(this, order, _repositoryOrder, 
                        _repositoryClient, _repositoryDriver, _repositoryProvider);
                    formChangeOrder.ShowDialog();
                    break;
                case CurrentTable.Product:
                    var product = _repositoryProduct.GetById(id);
                    var formChangeProduct = new FormChangeProduct(this, product, _repositoryProduct,
                        _repositoryTypeProduct, _repositoryStorage);
                    formChangeProduct.ShowDialog();
                    break;
                case CurrentTable.ProvideProduct:
                    var provideProduct = _repositoryProvideProduct.GetById(id);
                    var formChangeProvideProduct = new FormChangeProvideProduct(this, provideProduct, _repositoryProvideProduct,
                        _repositoryOrder, _repositoryProduct);
                    formChangeProvideProduct.ShowDialog();
                    break;
                case CurrentTable.Provider:
                    var provider = _repositoryProvider.GetById(id);
                    var formChangeProvider = new FormChangeProvider(this, provider, _repositoryProvider);
                    formChangeProvider.ShowDialog();
                    break;
                case CurrentTable.Storage:
                    var storage = _repositoryStorage.GetById(id);
                    var formChangeStorage = new FormChangeStorage(this, storage, _repositoryStorage, _repositoryStorageDirector);
                    formChangeStorage.ShowDialog();
                    break;
                case CurrentTable.StorageDirector:
                    var storageDirector = _repositoryStorageDirector.GetById(id);
                    var formChangeStorageDirector =
                        new FormChangeStorageDirector(this, storageDirector, _repositoryStorageDirector);
                    formChangeStorageDirector.ShowDialog();
                    break;
                case CurrentTable.TypeProduct:
                    var typeProduct = _repositoryTypeProduct.GetById(id);
                    var formChangeTypeProduct = new FormChangeTypeProduct(this, typeProduct, _repositoryTypeProduct);
                    formChangeTypeProduct.ShowDialog();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region Excel

        private void PrintIntoExcel(CurrentTable currentTable, params string[] namesColumns)
        {
            var application = new Excel.Application();
            var worksheet = (Excel.Worksheet)application.Workbooks.Add(Type.Missing).ActiveSheet;
            const int indexFirstLetter = 65;
            var nextLetter = Convert.ToChar(indexFirstLetter + namesColumns.Length - 1);
            var excelCells = worksheet.get_Range("A1", $"{nextLetter}1").Cells;
            excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
            excelCells.Interior.Color = Color.Gold;
            excelCells.Merge(Type.Missing);
            var nameTable = currentTable switch
            {
                CurrentTable.Client => "Клиенты",
                CurrentTable.Driver => "Водители",
                CurrentTable.Order => "Заказы",
                CurrentTable.Product => "Продукты",
                CurrentTable.ProvideProduct => "Поставки продуктов",
                CurrentTable.Provider => "Поставщики",
                CurrentTable.Storage => "Хранилища",
                CurrentTable.StorageDirector => "Директора",
                CurrentTable.TypeProduct => "Типы продуктов",
                _ => throw new ArgumentOutOfRangeException()
            };

            worksheet.Cells[1, 1] = $"Табличны данные \"{nameTable}\"";
            for (var i = 0; i < namesColumns.Length; i++)
            {
                worksheet.Cells[2, i + 1] = namesColumns[i];
                worksheet.Cells[2, i + 1].HorizontalAlignment = Excel.Constants.xlCenter;
                worksheet.Columns[i + 1].ColumnWidth = 35;
            }

            switch (currentTable)
            {
                case CurrentTable.Client:
                    var clients = _repositoryClient.GetAll();
                    var listClients = clients.ToList();
                    for (var i = 0; i < listClients.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listClients[i].Name;
                        application.Cells[i + 3, 2] = listClients[i].Address;
                        application.Cells[i + 3, 3] = listClients[i].TaxpayerIdentification.ToString();
                        application.Cells[i + 3, 4] = listClients[i].Phone;
                    }
                    break;
                case CurrentTable.Driver:
                    var drivers = _repositoryDriver.GetAll();
                    var listDrivers = drivers.ToList();
                    for (var i = 0; i < listDrivers.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listDrivers[i].Surname;
                        application.Cells[i + 3, 2] = listDrivers[i].Name;
                        application.Cells[i + 3, 3] = listDrivers[i].Patronymic;
                        application.Cells[i + 3, 4] = listDrivers[i].Experience.ToString();
                    }
                    break;
                case CurrentTable.Order:
                    var orders = _repositoryOrder.GetAll();
                    var listOrders = orders.ToList();
                    for (var i = 0; i < listOrders.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listOrders[i].Number;
                        application.Cells[i + 3, 2] = listOrders[i].Date.ToShortDateString();
                        var client = _repositoryClient.GetById(listOrders[i].ClientId);
                        var driver = _repositoryDriver.GetById(listOrders[i].DriverId);
                        var provider = _repositoryProvider.GetById(listOrders[i].ProviderId);
                        application.Cells[i + 3, 3] = client.Name;
                        application.Cells[i + 3, 4] = driver.Surname;
                        application.Cells[i + 3, 5] = provider.Name;
                    }
                    break;
                case CurrentTable.Product:
                    var products = _repositoryProduct.GetAll();
                    var listProducts = products.ToList();
                    for (var i = 0; i < listProducts.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listProducts[i].Name;
                        application.Cells[i + 3, 2] = listProducts[i].Balance.ToString();
                        application.Cells[i + 3, 3] = listProducts[i].Price.ToString("F");
                        application.Cells[i + 3, 4] = listProducts[i].MinValue.ToString();
                        var typeProduct = _repositoryTypeProduct.GetById(listProducts[i].TypeProductId);
                        var storage = _repositoryStorage.GetById(listProducts[i].StorageId);
                        application.Cells[i + 3, 5] = typeProduct.Name;
                        application.Cells[i + 3, 6] = storage.Name;
                    }
                    break;
                case CurrentTable.ProvideProduct:
                    var providesProduct = _repositoryProvideProduct.GetAll();
                    var listProvidesProduct = providesProduct.ToList();
                    for (var i = 0; i < listProvidesProduct.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listProvidesProduct[i].Count.ToString();
                        application.Cells[i + 3, 2] = listProvidesProduct[i].Price.ToString("F");
                        application.Cells[i + 3, 3] = listProvidesProduct[i].TypeProvide;
                        var order = _repositoryOrder.GetById(listProvidesProduct[i].OrderId);
                        var product = _repositoryProduct.GetById(listProvidesProduct[i].ProductId);
                        application.Cells[i + 3, 4] = order.Number;
                        application.Cells[i + 3, 5] = product.Name;
                    }
                    break;
                case CurrentTable.Provider:
                    var providers = _repositoryProvider.GetAll();
                    var listProviders = providers.ToList();
                    for (var i = 0; i < listProviders.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listProviders[i].Name;
                        application.Cells[i + 3, 2] = listProviders[i].Address;
                        application.Cells[i + 3, 3] = listProviders[i].TaxpayerIdentification.ToString();
                        application.Cells[i + 3, 4] = listProviders[i].Phone;
                    }
                    break;
                case CurrentTable.Storage:
                    var storages = _repositoryStorage.GetAll();
                    var listStorages = storages.ToList();
                    for (var i = 0; i < listStorages.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listStorages[i].Name;
                        application.Cells[i + 3, 2] = listStorages[i].Address;
                        var director = _repositoryStorageDirector.GetById(listStorages[i].StorageDirectorId);
                        application.Cells[i + 3, 3] = director.Surname;
                    }
                    break;
                case CurrentTable.StorageDirector:
                    var directors = _repositoryStorageDirector.GetAll();
                    var listDirectors = directors.ToList();
                    for (var i = 0; i < listDirectors.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listDirectors[i].Surname;
                        application.Cells[i + 3, 2] = listDirectors[i].Name;
                        application.Cells[i + 3, 3] = listDirectors[i].Patronymic;
                        application.Cells[i + 3, 4] = listDirectors[i].Birthday.ToShortDateString();
                    }
                    break;
                case CurrentTable.TypeProduct:
                    var typesProduct = _repositoryTypeProduct.GetAll();
                    var listTypesProduct = typesProduct.ToList();
                    for (var i = 0; i < listTypesProduct.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listTypesProduct[i].Name;
                        application.Cells[i + 3, 2] = listTypesProduct[i].ExtraCharge.ToString("F");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            application.Visible = true;
        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.Client, "Название", "Адрес", "ИНН", "Телефон");
        }

        private void водителиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.Driver, "Фамилия", "Имя", "Отчество", "Опыт");
        }

        private void заказыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.Order, "Номер", "Дата", "Клиент", "Водитель", "Поставщик");
        }

        private void продуктыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.Product, "Название", "Остаток", "Цена", 
                "Минимальное значение", "Тип продукта", "Хранилище");
        }

        private void поставкиПродуктовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.ProvideProduct, "Количество", "Цена", "Тип поставки",
                "Минимальное значение", "Заказ", "Продукт");
        }

        private void поставщикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.Provider, "Название", "Адрес", "ИНН", "Телефон");
        }

        private void хранилищаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.Storage, "Название", "Адрес", "Директор");
        }

        private void директораToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.StorageDirector, "Фамилия", "Имя", "Отчество", "Дата рождения");
        }

        private void типыПродуктовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.TypeProduct, "Название", "НДС (%)");
        }

        private void накладнаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTable != CurrentTable.Order)
            {
                MessageBox.Show("Выберите таблицу заказов!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Выберите строку таблицы!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var orderId = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            var order = _repositoryOrder.GetById(orderId);
            var providesProduct = _repositoryProvideProduct.GetAll().Where(x => x.OrderId == order.Id).ToList();

            var namesColumns = new[] { "Название товара", "Цена товара", "Количество", "Тип продукта", "Склад продукта" };

            var application = new Excel.Application();
            var worksheet = (Excel.Worksheet)application.Workbooks.Add(Type.Missing).ActiveSheet;
            const int indexFirstLetter = 65;
            var nextLetter = Convert.ToChar(indexFirstLetter + namesColumns.Length - 1);
            var excelCells = worksheet.get_Range("A1", $"{nextLetter}1").Cells;
            excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
            excelCells.Interior.Color = Color.Gold;
            excelCells.Merge(Type.Missing);

            worksheet.Cells[1, 1] = $"Накладная на заказ №{order.Number} от {order.Date.ToShortDateString()}";

            for (var i = 0; i < namesColumns.Length; i++)
            {
                worksheet.Cells[2, i + 1] = namesColumns[i];
                worksheet.Cells[2, i + 1].HorizontalAlignment = Excel.Constants.xlCenter;
                worksheet.Columns[i + 1].ColumnWidth = 35;
            }

            for (var i = 0; i < providesProduct.Count; i++)
            {
                var product = _repositoryProduct.GetById(providesProduct[i].OrderId);
                var typeProduct = _repositoryTypeProduct.GetById(product.TypeProductId);
                var storage = _repositoryStorage.GetById(product.StorageId);

                application.Cells[i + 3, 1] = product.Name;
                application.Cells[i + 3, 2] = providesProduct[i].Price;
                application.Cells[i + 3, 3] = providesProduct[i].Count;
                application.Cells[i + 3, 4] = typeProduct.Name;
                application.Cells[i + 3, 5] = storage.Name;
            }

            var client = _repositoryClient.GetById(order.ClientId);
            application.Cells[providesProduct.Count + 3, 1] = "Клиент:";
            application.Cells[providesProduct.Count + 3, 2] = client.Name;
            application.Cells[providesProduct.Count + 3, 1].HorizontalAlignment = Excel.Constants.xlCenter;
            application.Cells[providesProduct.Count + 3, 1].Interior.Color = Color.Gold;

            var driver = _repositoryDriver.GetById(order.DriverId);
            application.Cells[providesProduct.Count + 4, 1] = "Водитель:";
            application.Cells[providesProduct.Count + 4, 2] = driver.Surname + ' ' + driver.Name + ' ' + driver.Patronymic;
            application.Cells[providesProduct.Count + 4, 1].HorizontalAlignment = Excel.Constants.xlCenter;
            application.Cells[providesProduct.Count + 4, 1].Interior.Color = Color.Gold;

            var provider = _repositoryProvider.GetById(order.ClientId);
            application.Cells[providesProduct.Count + 5, 1] = "Поставщик:";
            application.Cells[providesProduct.Count + 5, 2] = provider.Name;
            application.Cells[providesProduct.Count + 5, 1].HorizontalAlignment = Excel.Constants.xlCenter;
            application.Cells[providesProduct.Count + 5, 1].Interior.Color = Color.Gold;

            application.Visible = true;
        }

        private void отчётПоСкладуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTable != CurrentTable.Storage)
            {
                MessageBox.Show("Выберите таблицу хранилищ!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Выберите строку таблицы!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var storageId = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            var storage = _repositoryStorage.GetById(storageId);
            var director = _repositoryStorageDirector.GetById(storage.StorageDirectorId);
            var products = _repositoryProduct.GetAll().Where(x => x.StorageId == storageId);
            var application = new Word.Application
            {
                Visible = false
            };

            var path = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Documents\\Storage.docx");
            var document = application.Documents.Open(path);
            ReplaceData("{date}", DateTime.Now.ToShortDateString(), document);
            ReplaceData("{name}", storage.Name, document);
            ReplaceData("{address}", storage.Address, document);
            ReplaceData("{director}", director.Surname + ' ' + director.Name + ' ' + director.Patronymic, document);
            var enumerableProducts = products.ToList();
            ReplaceData("{countProduct}", enumerableProducts
                .Select(x => x.Balance).Sum().ToString(), document);
            ReplaceData("{price}", enumerableProducts
                .Select(x => x.Price).Sum().ToString("F"), document);
            ReplaceData("{countTypes}", enumerableProducts.Count().ToString(), document);
            ReplaceData("{birthday}", director.Birthday.ToShortDateString(), document);
            document.SaveAs(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Documents\\Result.docx"));
            application.Visible = true;
            void ReplaceData(string target, string data, Word.Document documentMy)
            {
                var content = documentMy.Content;
                content.Find.ClearFormatting();
                content.Find.Execute(FindText: target, ReplaceWith: data);
            }
        }

        #endregion

        #region Validation enter

        private void EnterOnlyLetter(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || e.KeyChar is (char)Keys.Back or (char)Keys.Delete)
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;
        }

        private void EnterLetterAndDigit(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetterOrDigit(e.KeyChar) || e.KeyChar is (char)Keys.Back or (char)Keys.Delete)
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

        private void EnterAddress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetterOrDigit(e.KeyChar) || e.KeyChar is '.' or ',' or (char)Keys.Back or (char)Keys.Delete)
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;
        }

        #endregion

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            var itemsListView = listView1.Items;
            var textBox = sender as TextBox;
            if (textBox?.Text == string.Empty)
            {
                foreach (ListViewItem item in itemsListView)
                {
                    item.Selected = false;
                }
                return;
            }

            foreach (ListViewItem item in itemsListView)
            {
                for (var i = 0; i < item.SubItems.Count; i++)
                {
                    if (item.SubItems[i].Text.ToLower().Contains(textBox?.Text.ToLower()))
                    {
                        item.Selected = true;
                        break;
                    }

                    item.Selected = false;
                }
            }
        }

        private void отобратьЗаказыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Выберите строку таблицы!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var clientId = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            var orders = _repositoryOrder.GetAll().ToList();
            var listOrders = orders.Where(x => x.ClientId == clientId).ToList();
            var enumerableOrders = listOrders.Select(x =>
            {
                x.Client = _repositoryClient.GetById(x.ClientId);
                x.Driver = _repositoryDriver.GetById(x.DriverId);
                x.Provider = _repositoryProvider.GetById(x.ProviderId);
                return x;
            });

            PrintOrders(enumerableOrders);
        }

        private void отобратьПродуктыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Выберите строку таблицы!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var storageId = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            var products = _repositoryProduct.GetAll().ToList();
            var listProducts = products.Where(x => x.StorageId == storageId).ToList();
            var enumerableProducts = listProducts.Select(x =>
            {
                x.Storage = _repositoryStorage.GetById(x.StorageId);
                x.TypeProduct = _repositoryTypeProduct.GetById(x.TypeProductId);
                return x;
            });

            PrintProducts(enumerableProducts);
        }

        #region Filtration

        private void button28_Click(object sender, EventArgs e)
        {
            button3_Click(null, null);
            dateTimePicker3.Value = DateTime.Now;
            comboBox10.SelectedIndex = -1;
        }

        private void button29_Click(object sender, EventArgs e)
        {
            var listOrders = _repositoryOrder.GetAll().ToList();
            listOrders = listOrders.Where(x => x.Date.Date == dateTimePicker3.Value.Date).ToList();
            if (comboBox10.SelectedIndex != -1)
            {
                var client = _repositoryClient
                    .GetModelByProperty(comboBox10.SelectedItem.ToString(), "Name");
                listOrders = listOrders.Where(x => x.ClientId == client.Id).ToList();
            }

            listOrders = listOrders.Select(x =>
            {
                x.Client = _repositoryClient.GetById(x.ClientId);
                x.Driver = _repositoryDriver.GetById(x.DriverId);
                x.Provider = _repositoryProvider.GetById(x.ProviderId);
                return x;
            }).ToList();

            PrintOrders(listOrders);
        }

        #endregion
    }
}
