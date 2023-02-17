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
    public partial class Auth : Form
    {
        public int ID = 0;
        public Auth()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        }

        // Вход.
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && textBox2.Text == "")
            {
                MessageBox.Show(
                    "Не введены логин и/или пароль!",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                // Запрос к таблице Auth.
                string query = "SELECT auth_id FROM auth WHERE auth_log ='" + textBox1.Text + "' and auth_pwd = '" + textBox2.Text + "' and auth_fio = '" + comboBox1.Text + "';";
                MySqlConnection conn = DBUtils.GetDBConnection();
                // Объект для выполнения SQL-запроса.
                MySqlCommand cmDB = new MySqlCommand(query, conn);
                try
                {
                    // Устанавливаем соединение с БД.
                    conn.Open();
                    int result = 0;
                    result = Convert.ToInt32(cmDB.ExecuteScalar());
                    if (result > 0)
                    {
                        Client.Fio = comboBox1.Text;
                        Orders ord = new Orders(result); // Переход на форму Orders.
                        ord.Owner = this;
                        this.Hide();
                        ord.Show(); // Запуск окна Orders.
                        textBox1.Clear(); // Очистка поля - логин.
                        textBox2.Clear(); // Очистка поля - пароль.
                        comboBox1.ResetText();
                        comboBox1.SelectedIndex = -1; // Очистка поля - ФИО.
                    }
                    else
                        MessageBox.Show("Возникла ошибка авторизации!");
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Возникла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
                }
            }
        }

        // Выход.
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Переход к регистрации.
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registracia reg = new Registracia(); // Обращение к форме "Registracia", на которую будет совершаться переход.
            reg.Owner = this;
            this.Hide();
            reg.Show(); // Запуск окна "Registracia".
        }

        private void Auth_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Auth_Load(object sender, EventArgs e)
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
        }
    }

    static class Client
    {
        public static string Fio { get; set; }
    }
}
