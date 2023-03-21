using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CRMSystem
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DispatcherTimer dispatcher;
        Regex regex = new Regex($"^[0-9a-zA-Z`~!@#$%^&*()_\\-+={{}}\\[\\]\\|:;\"'<>,.?\\/]{{8}}$"); // Регулярное выражение для проверки корректности сгенерированого кода

        public static string code = "";
        private int counter = 10;
        public MainWindow()
        {
            InitializeComponent();
            DB.tbe = new Entities();
            dispatcher = new DispatcherTimer();
            dispatcher.Interval = new TimeSpan(0, 0, 1);
            dispatcher.Tick += new EventHandler(TimerEnd);
            btnEntry.Visibility = Visibility.Collapsed;
            stackPassword.Visibility = Visibility.Collapsed;


        }
        /// <summary>
        ///  Метод таймера, считающий отсчет 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerEnd(object sender, EventArgs e)
        {
            try
            {
                if (counter != 0)
                {
                    if (counter % 2 == 0)
                    {
                        tbNewCode.Foreground = Brushes.Red;

                    }
                    else
                    {
                        tbNewCode.Foreground = Brushes.Black;
                    }
                    tbNewCode.Visibility = Visibility.Visible;
                    tbNewCode.Text = "Оставшееся время \n" + string.Format("00:0{0}:{1} ", counter / 60, counter % 60) + " секунд ";


                }
                else
                {
                    tbNewCode.Visibility = Visibility.Visible;
                    stackCode.Visibility = Visibility.Visible;
                    imgUpdate.Visibility = Visibility.Visible;
                    dispatcher.Stop();
                    code = "";
                    tbNewCode.Text = "Код не действителен";


                }
                counter--;

            }
            catch
            {
                MessageBox.Show("Дваайте еще раз попробуем");
            }



        }
        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {


        }

        private void btnEntry_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                List<Employees> list1 = DB.tbe.Employees.Where(x => x.Nomer == tbNumber.Text).ToList();

                if (!string.IsNullOrEmpty(tbNumber.Text))
                {
                    if (list1.Count == 1)
                    {
                        MessageBox.Show("Номер в базе данных существует");
                        btnEntry.Visibility = Visibility.Visible;
                        stackPassword.Visibility = Visibility.Visible;
                        tbPassword.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Номер отсутствует в базе данных");
                    }
                }
                else
                {
                    MessageBox.Show("Поле номера не заполнено!");
                }

            }
        }


        void generateCode()
        {
            List<Employees> list1 = DB.tbe.Employees.Where(x => x.Nomer == tbNumber.Text && x.Password == tbPassword.Text).ToList();

            if (list1.Count == 1)
            {
                while (true)
                {
                    Random random = new Random();

                    for (int i = 0; i < 8; i++)
                    {
                        int j = random.Next(4);
                        if (j == 0)
                        {
                            code += random.Next(9).ToString();
                        }
                        else if (j == 1 || j == 2)
                        {
                            int l = random.Next(2);
                            if (l == 0)
                            {
                                code += (char)random.Next('A', 'Z' + 1);
                            }
                            else
                            {
                                code += (char)random.Next('a', 'z' + 1);
                            }
                        }
                        else
                        {
                            int l = random.Next(4);
                            if (l == 0)
                            {
                                code += (char)random.Next(33, 48);
                            }
                            else if (l == 1)
                            {
                                code += (char)random.Next(58, 65);
                            }
                            else if (l == 2)
                            {
                                code += (char)random.Next(91, 97);
                            }
                            else if (l == 3)
                            {
                                code += (char)random.Next(123, 127);
                            }
                        }
                    }

                    if (regex.IsMatch(code))
                    {
                        break;
                    }
                }
                MessageBox.Show("Код для доступа " + code + "\nКод действителен в течении 10 секунд.");



                stackCode.Visibility = Visibility.Visible;
                counter = 10;
                dispatcher.Start();

            }
            else
            {
                dispatcher.Stop();
                tbNewCode.Text = "";
                tbNewCode.IsEnabled = false;
                tbNewCode.Text = "";
                MessageBox.Show("Сотрудник с таким номером и паролем не найден!");

            }


        }
        private void tbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                if (!string.IsNullOrEmpty(tbPassword.Text))
                {
                    stackCode.Visibility = Visibility.Visible;
                    imgUpdate.Visibility = Visibility.Visible;
                    tbCode.Visibility = Visibility.Visible;
                    tbCode.Focus();
                    tbCode.Text = "";
                    generateCode();
                }
            }
        }

        private void imgUpdate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            tbCode.Text = "";
            code = "";
            generateCode();
        }
        void log()
        {
            if (code != "")

            {
                if (tbCode.Text == code)
                {
                    dispatcher.Stop();
                    tbNewCode.Text = "";
                    code = "";
                    Employees employee = DB.tbe.Employees.FirstOrDefault(x => x.Nomer == tbNumber.Text && x.Password == tbPassword.Text);
                    if (employee != null)
                    {
                        MessageBox.Show("Вы успешно авторизовались с ролью <<" + employee.Roles.Role + ">>");
                        tbNumber.Text = "";
                        tbPassword.Text = "";
                        tbCode.Text = "";
                        stackCode.Visibility = Visibility.Collapsed;
                        stackPassword.Visibility = Visibility.Collapsed;
                        btnEntry.Visibility = Visibility.Collapsed;
                        tbPassword.Visibility = Visibility.Collapsed;


                    }
                    else
                    {
                        MessageBox.Show("Отсутствует сотрудник в соответствии с веденными данными.");
                    }
                }
                else
                {
                    MessageBox.Show("Неверный код");
                }

            }
            else
            {
                MessageBox.Show("Поле код пустое. Возможно вы ничего не ввели");
            }
        }
        private void tbCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                log();


            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            tbNumber.Text = "";
            tbPassword.Text = "";
            tbCode.Text = "";
            dispatcher.Stop();
            code = "";
            tbNewCode.Text = "";
            tbPassword.Visibility = Visibility.Collapsed;
            tbCode.Visibility = Visibility.Collapsed;
            stackPassword.Visibility = Visibility.Collapsed;
            btnEntry.Visibility = Visibility.Collapsed;
            imgUpdate.Visibility = Visibility.Collapsed;
            stackCode.Visibility = Visibility.Collapsed;

        }
    }

}
