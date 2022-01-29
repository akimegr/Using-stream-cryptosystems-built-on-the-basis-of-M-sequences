using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TILab3
{
    public partial class Form1 : Form
    {
        
        Random rnd = new Random();
        public Form1()
        {
            InitializeComponent();
            byte byte1 = 4;
            textBox1.Text = System.Convert.ToString(byte1);

            textBox7.Text = "Hello world!!!";
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)  // проверка ввода
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                    e.Handled = true;
            }
        }
 
        private void button1_Click(object sender, EventArgs e)  // обработчик нажатия кнопки шифрования
        {
            if (listBox1.Items.Count != 0)
            {
                int count = listBox1.Items.Count;   
                int bitsCount = Convert.ToInt32(textBox1.Text) - 1;
                int[] keys = new int[count];    // инициализация для создания int-ых ключей
                for (int i = 0; i < count; i++)  //цикл для прохода по каждому ключу
                {
                    keys[i] = 0;

                    int j = 0;
                    foreach (char letter in listBox1.Items[i].ToString())   // цикл для перевода двоичного ключа из листБокса в int
                    {
                        if (letter == '1')
                        {
                            keys[i] += 1 * (int)Math.Pow(2, (bitsCount - j));
                        }
                        else
                        {
                            keys[i] += 0;
                        }
                        j++;
                    }
                }
                int k = 0;
                textBox5.Text = "";
                foreach (char letter in textBox7.Text)  // цикл для шифрования каждого символа посредством xor на соответствующий ключ
                {
                    textBox5.Text += Convert.ToChar(letter ^ keys[k % count]);
                    k++;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)  // обработчик нажатия кнопки дешифрования (всё точь в точь, как у шифрования)
        {
            if (listBox1.Items.Count != 0)
            {

                int count = listBox1.Items.Count;
                int bitsCount = Convert.ToInt32(textBox1.Text) - 1;
                int[] keys = new int[count];
                for (int i = 0; i < count; i++)
                {
                    keys[i] = 0;

                    int j = 0;
                    foreach (char letter in listBox1.Items[i].ToString())
                    {
                        if (letter == '1')
                        {
                            keys[i] += 1 * (int)Math.Pow(2, (bitsCount - j));
                        }
                        else
                        {
                            keys[i] += 0;
                        }
                        j++;
                    }
                }
                int k = 0;
                textBox6.Text = "";
                foreach (char letter in textBox5.Text)
                {
                    textBox6.Text += Convert.ToChar(letter ^ keys[k % count]);
                    k++;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)   // обработчик изменения длины ключа
        {
            int num = Convert.ToInt32(textBox1.Text); 
            if(num > 8) // проверка соответствия длины диапазону
            {
                textBox1.Text = "8";
                num = 8;
            }
            else if(num < 4)
            {
                textBox1.Text = "4";
                num = 4;
            }

            textBox2.Text = "";
            checkedListBox1.Items.Clear();

            for(int i = 0; i < num; i++)    // создание соответствующиего кол-ва переключателей
            {
                checkedListBox1.Items.Add($"Bit {i + 1}");
                textBox2.Text += "" + rnd.Next(0, 2);
            }

            int[] indexes = { -1, -1, -1, -1 };
            switch (num)    // инициализация индексов для генератора ключей (по методичке)
            {
                case 4:
                    {
                        indexes[0] = 0;
                        indexes[1] = 3;
                        break; 
                    }
                case 5:
                    {
                        indexes[0] = 1;
                        indexes[1] = 4;
                        break;
                    }
                case 6:
                    {
                        indexes[0] = 0;
                        indexes[1] = 5;
                        break;
                    }
                case 7:
                    {
                        indexes[0] = 0;
                        indexes[1] = 5;
                        break;
                    }
                case 8:
                    {
                        indexes[0] = 0;
                        indexes[1] = 4;
                        indexes[2] = 5;
                        indexes[3] = 7;
                        break;
                    }
                    
            }
            for(int i = 0; i < 4; i++)  // включение соответствующих переключателей
            {
                if (indexes[i] != -1)
                {
                    checkedListBox1.SetItemChecked(indexes[i], true);
                }
                else
                    break;
            }
            checkedListBox1.SelectedIndex = indexes[0]; // для активации ивента

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)   // обработчик изменения кол-ва переключателей
        {
            int bitsCount = Convert.ToInt32(textBox1.Text);
            if (bitsCount != 8) // проверка на длину ключа, т.к. в 8-битном используются 4 переключателя вместо 2-ух
            {
                if (checkedListBox1.CheckedItems.Count > 2) // отключение переключателей, если их больше 2-ух
                {
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                        checkedListBox1.SetItemChecked(i, false);
                    checkedListBox1.SetItemChecked(checkedListBox1.SelectedIndex, true);
                }
                else if (checkedListBox1.CheckedItems.Count == 2) // генерация ключей, если 2 переключателя активны
                {
                    listBox1.Items.Clear();

                    byte[] bits = new byte[bitsCount];
                    int i = 0;
                    foreach (char num in textBox2.Text) // перевод строки в последовательность бит
                    {
                        if (num == '1')
                        {
                            bits[i] = 1;
                        }
                        else
                        {
                            bits[i] = 0;
                        }
                        i++;
                    }


                    listBox1.Items.Add((listBox1.Items.Count+1).ToString() + " " + textBox2.Text);

                    byte newBit = Convert.ToByte(bits[checkedListBox1.CheckedIndices[0]] ^ bits[checkedListBox1.CheckedIndices[1]]); // создание нового бита с помощью xor указанных переключателями бит

                    string key = "";    // строковое представление двоичного ключа
                    for (i = 0; i < bits.Length - 1; i++) // цикл для смещения битов в сторону большего разряда
                    {
                        bits[i] = bits[i + 1];
                        key += bits[i] % 2;
                    }
                    bits[bits.Length - 1] = newBit; // добавления нового бита в младший разряд
                    key += newBit;

                    for (i = 0; i < Math.Pow(2, bitsCount)+3; i++)
                    {
                        listBox1.Items.Add((listBox1.Items.Count+1).ToString() + " " + key);    // добавление ключа в список
                        newBit = Convert.ToByte(bits[checkedListBox1.CheckedIndices[0]] ^ bits[checkedListBox1.CheckedIndices[1]]); // создание нового бита с помощью xor указанных переключателями бит

                        key = "";
                        for (int j = 0; j < bits.Length - 1; j++)   // цикл для смещения битов в сторону большего разряда
                        {
                            bits[j] = bits[j + 1];
                            key += bits[j] % 2;
                        }
                        bits[bits.Length - 1] = newBit; // добавления нового бита в младший разряд
                        key += newBit;
                    }
                }

            }
            else    // всё то же самое только с использованием 4 переключателей для 8-битного ключа
            {
                if (checkedListBox1.CheckedItems.Count > 4)
                {
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                        checkedListBox1.SetItemChecked(i, false);
                    checkedListBox1.SetItemChecked(checkedListBox1.SelectedIndex, true);
                }
                else if (checkedListBox1.CheckedItems.Count == 4)
                {
                    listBox1.Items.Clear();

                    byte[] bits = new byte[bitsCount];
                    int i = 0;
                    foreach (char num in textBox2.Text)
                    {
                        if (num == '1')
                        {
                            bits[i] = 1;
                        }
                        else
                        {
                            bits[i] = 0;
                        }
                        i++;
                    }


                    listBox1.Items.Add(textBox2.Text);

                    byte newBit = Convert.ToByte(bits[checkedListBox1.CheckedIndices[0]] ^ bits[checkedListBox1.CheckedIndices[1]] ^ bits[checkedListBox1.CheckedIndices[2]] ^ bits[checkedListBox1.CheckedIndices[3]]);

                    string key = "";
                    for (i = 0; i < bits.Length - 1; i++)
                    {
                        bits[i] = bits[i + 1];
                        key += bits[i] % 2;
                    }
                    bits[bits.Length - 1] = newBit;
                    key += newBit;

                    for (i = 0; i < Math.Pow(2, bitsCount) - 2; i++)
                    {
                        listBox1.Items.Add(key);
                        newBit = Convert.ToByte(bits[checkedListBox1.CheckedIndices[0]] ^ bits[checkedListBox1.CheckedIndices[1]] ^ bits[checkedListBox1.CheckedIndices[2]] ^ bits[checkedListBox1.CheckedIndices[3]]);

                        key = "";
                        for (int j = 0; j < bits.Length - 1; j++)
                        {
                            bits[j] = bits[j + 1];
                            key += bits[j] % 2;
                        }
                        bits[bits.Length - 1] = newBit;
                        key += newBit;
                    }
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
