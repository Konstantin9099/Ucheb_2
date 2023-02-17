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
    public partial class ChangeOrders : Form
    {
        public int ID = 0;
        public ChangeOrders(int id_user)
        {
            InitializeComponent();
            dateTimePicker1.Value = DateTime.Now;
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ID = id_user;
        }

        // Изменение заказа.
        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем, чтобы были заполнены поля ввода/вывода данных.
            if (textBox1.Text == null || textBox1.Text == "")
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
                DialogResult res = MessageBox.Show("Изменить заказ?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                    int id = Convert.ToInt32(Ord.Change);
                    string query = "UPDATE orders SET order_date='" + date + "', order_comment='" + textBox1.Text + "' WHERE  order_id='" + id + "'; ";
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
                    MessageBox.Show("Заказ изменен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Перезод к форме Orders
        private void button2_Click(object sender, EventArgs e)
        {
            Orders ord = new Orders(ID); // Обращение к форме "Orders", на которую будет совершаться переход.
            ord.Owner = this;
            this.Hide();
            ord.Show(); // Запуск окна "Orders".
        }

        private void ChangeOrders_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
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

        private void ChangeOrders_Load(object sender, EventArgs e)
        {
            textBox1.Text = Ord.Zakaz;
        }
    }
}
