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
    public partial class AddOrders : Form
    {
        public int ID = 0;
        public AddOrders(int id_user)
        {
            InitializeComponent();
            dateTimePicker1.Value = DateTime.Now;
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ID = id_user;
        }

        void Action(string query)
        {
            MySqlConnection conn = DBUtils.GetDBConnection();
            MySqlCommand cmDB = new MySqlCommand(query, conn);
            try
            {
                conn.Open();
                MySqlDataReader rd = cmDB.ExecuteReader();
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла непредвиденная ошибка!");
            }
        }

        // Добавить заказ.
        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем, чтобы были заполнены все поля.
            if (textBox1.Text == null || textBox1.Text == "")
            {
                MessageBox.Show(
                    "Введите заказ.",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            else
            {
                DialogResult res = MessageBox.Show("Добавить данные о заказе?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    try
                    {
                        string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                        string query = "INSERT INTO orders (order_date, order_client, order_comment, order_response) VALUES ('" + date + "', (select client_id from clients where client_fio='" + Ord.Client + "' and client_phone='" + Ord.Phone + "' and client_birth='" + Ord.Birth + "'),  '" + textBox1.Text + "', " + ID + "); ";
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
                        MessageBox.Show("Заказ добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (NullReferenceException ex)
                    {
                        MessageBox.Show("Произошла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
                    }
                }
            }
        }

        // Переход на форму Заказы.
        private void button2_Click(object sender, EventArgs e)
        {
            Orders ord = new Orders(ID); // Обращение к форме "Orders", на которую будет совершаться переход.
            ord.Owner = this;
            this.Hide();
            ord.Show(); // Запуск окна "Orders".
        }

        private void AddOrders_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
