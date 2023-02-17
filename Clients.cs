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
    public partial class Clients : Form
    {
        public int ID = 0;
        public Clients(int id_user)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ID = id_user;
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
        // Добавить.
        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем, чтобы были заполнены все поля.
            if (comboBox1.Text.Equals(""))
            {
                MessageBox.Show(
                    "Введите полные данные.",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            else
            {
                DialogResult res = MessageBox.Show("Добавить данные о работнике?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    try
                    {
                        string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                        string query = "INSERT INTO clients (client_fio, client_phone, client_birth) VALUES ('" + comboBox1.Text + "', '" + maskedTextBox1.Text + "', '" + date + "'); ";
                        MySqlConnection conn = DBUtils.GetDBConnection();
                        MySqlCommand cmDB = new MySqlCommand(query, conn);
                        try
                        {
                            conn.Open();
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Произошла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
                        }
                        Action(query);
                        MessageBox.Show("Данные работника успешно добавлены.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (NullReferenceException ex)
                    {
                        MessageBox.Show("Произошла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
                    }
                }
            }
        }

        // Изменить.
        private void button2_Click(object sender, EventArgs e)
        {
            // Проверяем, чтобы были заполнены поля ввода/вывода данных.
            if (comboBox1.Text.Equals(""))
            {
                MessageBox.Show(
                    "Внесите необходимые изменения.",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            else
            {
                DialogResult res = MessageBox.Show("Изменить данные работника?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                    string query = "UPDATE clients SET client_fio='" + comboBox1.Text + "', client_phone='" +  maskedTextBox1.Text + "', client_birth='" + date + "' WHERE  client_id='" + ID + "'; ";
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    MySqlCommand cmDB = new MySqlCommand(query, conn);
                    try
                    {
                        conn.Open();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
                    }
                    Action(query);
                    MessageBox.Show("Данные работника успешно изменены.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void Clients_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // Переход на форму Заказы.
        private void button3_Click(object sender, EventArgs e)
        {
            Orders ord = new Orders(ID); // Обращение к форме "Orders", на которую будет совершаться переход.
            ord.Owner = this;
            this.Hide();
            ord.Show(); // Запуск окна "Orders".
        }

        private void Clients_Load(object sender, EventArgs e)
        {
            try
            {
                string query = "select auth.auth_fio from auth order by auth.auth_fio;";
                MySqlConnection conn = DBUtils.GetDBConnection();
                MySqlCommand cmDB = new MySqlCommand(query, conn);

                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.DropDownHeight = 150;
                    comboBox1.Items.Add(reader.GetString(0));
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            comboBox1.Text = Ord.Client;
            maskedTextBox1.Text = Ord.Phone;
            dateTimePicker1.Text = Ord.Birth;
        }
    }
}
