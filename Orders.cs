using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Ucheb_2
{
    public partial class Orders : Form
    {
        public int ID = 0;
        public Orders(int id_user)
        {
            InitializeComponent();
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ID = id_user;
            Get_Info(id_user);
        }

        public void Get_Info(int ID)
        {
            string query = "SELECT order_id as 'Код заказа', order_date 'Дата заказа', client_fio as 'ФИО работника', client_phone as 'Номер телефона', client_birth as 'Дата рождения', order_comment as 'Заказ' FROM orders, clients, auth where orders.order_client=clients.client_id and orders.order_response=auth.auth_id and order_response= '" + ID + "'; ";
            MySqlConnection conn = DBUtils.GetDBConnection();
            MySqlDataAdapter sda = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                dataGridView1.DataSource = dt;
                dataGridView1.ClearSelection();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.ClearSelection();
                this.dataGridView1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[0].Width = 70;
                this.dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[1].Width = 90;
                this.dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[2].Width = 150;
                this.dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[3].Width = 100;
                this.dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[4].Width = 100;
                this.dataGridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[5].Width = 250;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла непредвиденая ошибка!" + Environment.NewLine + ex.Message);
            }
        }

        // Вывод данных в текстовые поля формы из строки. выбранной в таблице dataGridView.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox3.Text = ((DateTime)dataGridView1.CurrentRow.Cells[4].Value).ToString("yyyy-MM-dd");
            textBox4.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
        }

        //Функция, позволяющая отправить команду на сервер БД для оптимизации кода.
        public void Action(string query)
        {
            MySqlConnection conn = DBUtils.GetDBConnection();
            MySqlCommand cmDB = new MySqlCommand(query, conn);
            try
            {
                conn.Open();
                cmDB.ExecuteReader();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
            }
        }

        private void Orders_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // Переход к форме AddOrders
        private void button1_Click(object sender, EventArgs e)
        {
            Ord.Client = textBox1.Text;
            Ord.Phone = textBox2.Text;
            Ord.Birth = textBox3.Text;
            AddOrders add = new AddOrders(ID); // Обращение к форме "AddOrders", на которую будет совершаться переход.
            add.Owner = this;
            this.Hide();
            add.Show(); // Запуск окна "AddOrders".
        }

        // Переход к форме ChangeOrders
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox4.Text == "" || textBox4.Text == null)
            {
                MessageBox.Show("Для изменения данных выберете строку в таблице заказов.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                int id = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString()); // Определяем id заказа.
                string idString = id.ToString();
                Ord.Change = idString;
                Ord.Zakaz = textBox4.Text;
                ChangeOrders chang = new ChangeOrders(ID); // Обращение к форме "ChangeOrders", на которую будет совершаться переход.
                chang.Owner = this;
                this.Hide();
                chang.Show(); // Запуск окна "ChangeOrders".
            }
        }

        // Переход в профиль работника
        private void button6_Click(object sender, EventArgs e)
        {
            Ord.Client = textBox1.Text;
            Ord.Phone = textBox2.Text;
            Ord.Birth = textBox3.Text;
            Clients clients = new Clients(ID); // Обращение к форме "ChangeOrders", на которую будет совершаться переход.
            clients.Owner = this;
            this.Hide();
            clients.Show(); // Запуск окна "ChangeOrders".
        }
         
        // Выводим на форму данные авторизировавшегося работника.
        private void Orders_Load(object sender, EventArgs e)
        {
            try
            {
                string query = "select DATE_FORMAT(client_birth,'%Y-%m-%d') as 'data', client_fio, client_phone  FROM clients where client_fio='" + Client.Fio + "'; ";
                MySqlConnection conn = DBUtils.GetDBConnection();
                MySqlCommand cmDB = new MySqlCommand(query, conn);

                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    textBox1.Text = reader["client_fio"].ToString();
                    textBox2.Text = reader["client_phone"].ToString();
                    textBox3.Text = reader["data"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Ord.Client = textBox1.Text;
            Ord.Phone = textBox2.Text;
            Ord.Birth = textBox3.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Проверяем, чтобы были заполнены все поля.
            if (textBox4.Text == null || textBox4.Text == "")
            {
                MessageBox.Show(
                    "Выберете строку в таблице заказов, подлежащую удалению.",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            else
            {
                DialogResult res = MessageBox.Show("Удалить данные о заказе?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    string valueCell = dataGridView1.CurrentCell.Value != null ? dataGridView1.CurrentCell.Value.ToString() : "";
                    string del = "DELETE FROM orders WHERE order_id = " + valueCell + ";";
                    Action(del);
                    Get_Info(ID);
                    textBox4.Clear();
                }
                else
                {
                    MessageBox.Show("Удаление записи отменено!");
                }
            }
        }

        //Строка поиска.
        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Selected = false;
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                        if (dataGridView1.Rows[i].Cells[j].Value.ToString().Contains(textBox5.Text))
                        {
                            dataGridView1.Rows[i].Selected = true;
                            break;
                        }
            }
        }

        // Функция отбора по периоду.
        public void OtborPeriod(int ID)
        {
            string date1 = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string date2 = dateTimePicker3.Value.ToString("yyyy-MM-dd");
            string query = "SELECT order_id as 'Код заказа', order_date 'Дата заказа', client_fio as 'ФИО работника', client_phone as 'Номер телефона', client_birth as 'Дата рождения', order_comment as 'Заказ' FROM orders, clients, auth where orders.order_client=clients.client_id and orders.order_response=auth.auth_id and order_response= '" + ID + "'and order_date >= '" + date1 + "' and order_date <= '" + date2 + "'; ";
            MySqlConnection conn = DBUtils.GetDBConnection();
            MySqlDataAdapter sda = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.ClearSelection();
                this.dataGridView1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[0].Width = 70;
                this.dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[1].Width = 90;
                this.dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[2].Width = 150;
                this.dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[3].Width = 100;
                this.dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[4].Width = 100;
                this.dataGridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[5].Width = 250;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла непредвиденая ошибка!" + Environment.NewLine + ex.Message);
            }
        }

        // Функция отбора по дате.
        public void OtborData(int ID)
        {
            string date = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string query = "SELECT order_id as 'Код заказа', order_date 'Дата заказа', client_fio as 'ФИО работника', client_phone as 'Номер телефона', client_birth as 'Дата рождения', order_comment as 'Заказ' FROM orders, clients, auth where orders.order_client=clients.client_id and orders.order_response=auth.auth_id and order_response= '" + ID + "'and order_date = '" + date + "'; ";
            MySqlConnection conn = DBUtils.GetDBConnection();
            MySqlDataAdapter sda = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.ClearSelection();
                this.dataGridView1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[0].Width = 70;
                this.dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[1].Width = 90;
                this.dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[2].Width = 150;
                this.dataGridView1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[3].Width = 100;
                this.dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[4].Width = 100;
                this.dataGridView1.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[5].Width = 250;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла непредвиденая ошибка!" + Environment.NewLine + ex.Message);
            }
        }

        // Фильтрация заказов.
        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "за период")
            {
                OtborPeriod(ID);
            }
            else if (comboBox1.Text == "за дату")
            {
                OtborData(ID);
            }
            else if (comboBox1.Text == "сброс настроек")
            {
                Get_Info(ID);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Ascending);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Descending);
        }
    }

    static class Ord
    {
        public static string Change { get; set; }
        public static string Client { get; set; }
        public static string Phone { get; set; }
        public static string Birth { get; set; }
        public static string Zakaz { get; set; }
    }
}
