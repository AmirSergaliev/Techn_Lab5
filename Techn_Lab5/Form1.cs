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
using Class_Lab5;

namespace Techn_Lab5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Данные для myProgram.log

            DateTime now_Date = DateTime.Now; 
            string nowdatetime = now_Date.ToString();
            string path1 = @"myProgram.log";
            string name = "Techn_Lab5, Вариант 40";
            string func_res = "Log2(y/x)";

            //Запись данных в myProgram.log
            StreamWriter file1 = new StreamWriter(path1); 
            file1.WriteLine("Название программы и номер варианта: " + name);
            file1.WriteLine("Дата и время начала выполнения расчёта: " + nowdatetime);
            file1.WriteLine("Рассчитываемая функция: " + func_res);

            //Создание файла ошибок "myErrors.log" 
            string path2 = @"myErrors.log";

            //Переменная для записи в файл ошибок
            StreamWriter file_Errors = new StreamWriter(path2);

            listBox1.Items.Clear();
            string Filename1 = "";

            //Для ФГх   x0 Nx хk!   
            string[] FG_X = textBox1.Text.Split(';');


            //Для Пу   Ny yi!
            string[] PY = textBox2.Text.Split(';');


            int count_data = 1;
            for(int j = 0; j < PY.Length; j++)
            {
                string yArr = PY[j].TrimStart(' ');
                string[] Y = yArr.Split(' ');
                for(int i = 0; i < FG_X.Length; i++)
                {
                    // Проверка на наборы
                    if(i != j)
                    {
                        continue;
                    }
                    string xArr = FG_X[i].TrimStart(' ');
                    string[] X = xArr.Split(' ');

                    string filename = Path.Combine(Environment.CurrentDirectory, "G" + string.Format("{0:d4}", count_data) + ".dat");
                    
                    double n = 0 ;
                    
                    try
                    {
                        using(StreamWriter fileRes = new StreamWriter(filename))
                        {
                            fileRes.WriteLine(func_res);

                            double x_0 = 0;
                            double y_0 = 0;
                            double x0 = Convert.ToDouble(X[0]);

                            //Считаем шаг как - (xk - x0)/Nx
                            double hx = ((Convert.ToDouble(X[2]) - x0)/Convert.ToDouble(X[1]));

                            int pointCount = 0;
                            Filename1 = filename;
                            //Создаю переменные, которые будут выводить в файл аргументы и значения функции
                            string x_add = "y/x".PadRight(8);
                            string y_add = "";

                            //Добавляем значения y, в цикле for j2 = 1, так как первое значение - это Ny
                            for(int j2 = 1; j2 <= Convert.ToInt32(Y[0]); j2++)
                            {
                                y_0 = Convert.ToDouble(Y[j2]);

                                y_add = (y_0).ToString("##0.0##").PadRight(8);

                                for(int i2 = 0; i2 <= Convert.ToInt32(X[1]); i2++)
                                {
                                    string func_result = "";

                                    x_0 = x0 + i2 * hx;

                                    //Расчет функции
                                    double result = Function.F(x_0, y_0);
                                    x_add += (x_0).ToString("##0.0##").PadRight(8);

                                    if (double.IsNaN(result) || (y_0 / x_0 < 0) || x_0 == 0 || y_0 == 0 )
                                    {
                                        func_result = ("Nan").PadRight(7);
                                    }
                                    else
                                    {
                                        func_result = result.ToString("#0.00000");
                                    }
                                    //Запись данных в листбокс
                                    listBox1.Items.Add("G" + count_data + " (" + x_0 + ";" + y_0 + ")" + " = " + func_result);
                                    y_add += func_result + " ";
                                    pointCount++;

                                   
                                }
                                if (j2 == 1)
                                {
                                    fileRes.WriteLine(x_add);
                                }

                                fileRes.WriteLine(y_add);
                            }
                            //Вывод в файл количества точек для х и y для аргументов функции G(x, y).
                            string countInfo = string.Format("Всего точек: {0}", pointCount);
                            fileRes.WriteLine(countInfo);

                            fileRes.Close();
                            file1.WriteLine(filename);
                            
                        }
                    }

                    //Запись в файл myErrors.log
                    catch(FormatException)
                    {
                        file_Errors.WriteLine(filename);
                        file_Errors.WriteLine(func_res);
                        bool canConvert1 = double.TryParse(PY[j], out n);
                        if (canConvert1 == false)
                        {
                            PY[j] = "Nan";
                        }
                        bool canConvert2 = double.TryParse(FG_X[j], out n);
                        if (canConvert2 == false)
                        {
                            FG_X[j] = "Nan";
                        }
                        file_Errors.WriteLine(string.Format("x={0}; y={1}", FG_X[i], PY[j]));
                        file_Errors.WriteLine("FormatException");
                    }
                    catch (DivideByZeroException)
                    {
                        
                        file_Errors.WriteLine(filename);
                        file_Errors.WriteLine(func_res);
                        file_Errors.WriteLine(string.Format("x={0}; y={1}", FG_X[i], PY[j]));

                        file_Errors.WriteLine("DivideByZeroException");
                    }
                    catch (OverflowException)
                    {
                        
                        file_Errors.WriteLine(filename);
                        file_Errors.WriteLine(func_res);

                        file_Errors.WriteLine(string.Format("x={0}; y={1}", FG_X[i], PY[j]));
                        file_Errors.WriteLine("OverflowException");
                    }
                    catch
                    {
                        
                        file_Errors.WriteLine(filename);
                        file_Errors.WriteLine(func_res);
                        file_Errors.WriteLine(string.Format("x={0}; y={1}", FG_X[i], PY[j]));
                        file_Errors.WriteLine("Incorrected values");
                    }
                    count_data++;


                }
            }
            file_Errors.Close();
            file1.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
