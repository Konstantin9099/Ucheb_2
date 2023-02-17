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
    public partial class Registracia : Form
    {
        public Registracia()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
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
         
        // Добавление пользователя
        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем, чтобы были заполнены все поля.
            if (textBox1.Text == null || textBox1.Text == "" || textBox2.Text == null || textBox2.Text == "" || textBox3.Text == null || textBox3.Text == "")
            {
                MessageBox.Show(
                    "Заполните все поля ввода данных.",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            else
            {
                DialogResult res = MessageBox.Show("Выполнить регистрацию в системе?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    try
                    {
                        string query = "INSERT INTO auth (auth_log, auth_pwd, auth_fio) VALUES ('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "'); ";
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
                        button1.Visible = false;
                        string name = textBox3.Text;
                        label5.Text = $"{name},\n Вы успешно зарегистрировались!";
                    }
                    catch (NullReferenceException ex)
                    {
                        MessageBox.Show("Произошла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
                    }
                }
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                button1.Visible = false;
            }

        }

        // Возврат на форму авторизации.
        private void button2_Click(object sender, EventArgs e)
        {
            Auth aut = new Auth(); // Обращение к форме "Auth", на которую будет совершаться переход.
            aut.Owner = this;
            this.Hide();
            aut.Show(); // Запуск окна "Auth".
        }

        private void Registracia_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
