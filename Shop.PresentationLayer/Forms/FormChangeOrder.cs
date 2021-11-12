using System;
using System.Linq;
using System.Windows.Forms;
using Shop.DataAccessLayer.Models;
using Shop.PresentationLayer.Tools;
using Shop.DataAccessLayer.Repository;

namespace Shop.PresentationLayer.Forms
{
    public partial class FormChangeOrder : Form
    {
        private readonly FormMain _formMain;
        private readonly Order _order;
        private readonly IRepository<Order> _repositoryOrder;
        private readonly IRepository<Client> _repositoryClient;
        private readonly IRepository<Driver> _repositoryDriver;
        private readonly IRepository<Provider> _repositoryProvider;

        public FormChangeOrder(FormMain formMain, Order order, IRepository<Order> repositoryOrder, 
            IRepository<Client> repositoryClient, IRepository<Driver> repositoryDriver, 
            IRepository<Provider> repositoryProvider)
        {
            InitializeComponent();
            _formMain = formMain;
            _order = order;
            _repositoryOrder = repositoryOrder;
            _repositoryClient = repositoryClient;
            _repositoryDriver = repositoryDriver;
            _repositoryProvider = repositoryProvider;
        }

        private void FormChangeOrder_Load(object sender, EventArgs e)
        {
            this.FillCombobox(comboBox1, _repositoryClient.GetAll().Select(x => x.Name).ToArray());
            this.FillCombobox(comboBox2, _repositoryProvider.GetAll().Select(x => x.Name).ToArray());
            this.FillCombobox(comboBox3, _repositoryDriver.GetAll().Select(x => x.Surname).ToArray());
            textBox1.Text = _order.Number.ToString();
            dateTimePicker1.Value = _order.Date.Date;
            var client = _repositoryClient.GetById(_order.ClientId);
            var provider = _repositoryProvider.GetById(_order.ProviderId);
            var driver = _repositoryDriver.GetById(_order.DriverId);
            comboBox1.SelectedItem = client.Name;
            comboBox2.SelectedItem = provider.Name;
            comboBox3.SelectedItem = driver.Surname;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || comboBox1.SelectedIndex == -1 ||
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
                Id = _order.Id,
                Number = Convert.ToInt32(textBox1.Text),
                Date = dateTimePicker1.Value.Date,
                ClientId = client.Id,
                ProviderId = provider.Id,
                DriverId = driver.Id
            };

            _repositoryOrder.Update(order);
            _formMain.button3_Click(null, null);
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
    }
}
