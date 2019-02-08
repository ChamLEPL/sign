using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Numerics;
using System.Diagnostics;

namespace Sign
{
    public partial class Form1 : Form
    {
        CalculateMethods m = new CalculateMethods();
        CodeMethods c = new CodeMethods();
        public Form1()
        {
            InitializeComponent();
        }
        //открытие исходного файла
        private void sourceFB_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();//Отображает диалоговое окно, позволяющее пользователю открыть файл.
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";//открывает только текстовые файлы
            //при нажатии OK текстовому полю  sourceFPTB присвается путь выбранного файла
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                sourceFPTB.Text = ofd.FileName;
            }
        }
        //открытие файла с подписью
        private void signFB_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                signFPTB.Text = ofd.FileName;
            }
        }
        // кнопка подписать
        private void signB_Click(object sender, EventArgs e)
        {
            if ((tB_p.Text.Length > 0) && (tB_q.Text.Length > 0) && (sourceFPTB.Text.Length > 0) && (signFPTB.Text.Length > 0))
            {
                long p = Convert.ToInt64(tB_p.Text);
                long q = Convert.ToInt64(tB_q.Text);
                if (m.IsPrime(p) && m.IsPrime(q))
                {
                    string hash = File.ReadAllText(sourceFPTB.Text).GetHashCode().ToString();//получаем хэш-код
                    long n = p * q;//второй сегмент ключей
                    long fe = (p - 1) * (q - 1);//функция эйлера
                    long _e = m.Calculate_e(fe);//первый сегмент открытого ключа
                    long d = m.Calculate_d(fe, _e);//первый октрытого ключа
                    List<string> result = c.RSA_Encode(hash, _e, n);//получаем закодированный хэш-код
                    StreamWriter sw = new StreamWriter(signFPTB.Text);//запись символов в поток в определенной кодировке
                    foreach (string item in result)
                        sw.WriteLine(item);
                    sw.Close();
                    //присваеваем значения полям
                    tB_n1.Text = n.ToString();
                    tB_e.Text = _e.ToString();
                    tB_d.Text = d.ToString();
                    tB_n.Text = n.ToString();
                   Process.Start(signFPTB.Text);//заупускаем файл
                }
                else
                    MessageBox.Show("p или q - не простые числа!");
            }
            else
                MessageBox.Show("Введите p и q и пути к файлам!");
        }
        //кнпока проверить подлинность
        private void checkSB_Click(object sender, EventArgs e)
        {
            if ((tB_d.Text.Length > 0) && (tB_n.Text.Length > 0) && (sourceFPTB.Text.Length > 0) && (signFPTB.Text.Length > 0))
            {
                long d = Convert.ToInt64(tB_d.Text);
                long n = Convert.ToInt64(tB_n.Text);
                List<string> input = new List<string>();
                StreamReader sr = new StreamReader(signFPTB.Text);// считывает символы из потока байтов в определенной кодировке.
                while (!sr.EndOfStream)//пока не считаем все символы из файла
                {
                    input.Add(sr.ReadLine());//то что считали заносим в список
                }
                sr.Close();
                string result = c.RSA_Decode(input, d, n);//расшифровка
                string hash = File.ReadAllText(sourceFPTB.Text).GetHashCode().ToString();//хэш исходника
                beforeCodeCache.Text =hash;
                afterCodeCache.Text = result;
                if (result.Equals(hash))
                    MessageBox.Show("Файл подлинный. Подпись верна.");
                else
                    MessageBox.Show("Внимание! Файл НЕ подлинный!!!");
            }
            else
                MessageBox.Show("Введите секретный ключ и пути к файлам!");
        }
    }
}
