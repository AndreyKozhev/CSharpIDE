using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Windows.Interop;

namespace CSharpIDE
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string Output = "Out.exe";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog OpenFile = new Microsoft.Win32.OpenFileDialog();
            OpenFile.Title = "Выберите файл для открытия";
            OpenFile.Multiselect = false;
            OpenFile.Filter = "CS|*.cs|XAML|.xaml";
            string file_name = OpenFile.FileName;
            if (OpenFile.ShowDialog() == true)
            {
                string strfilename = OpenFile.FileName;
                string filetext = File.ReadAllText(strfilename);
                Editor.Text = filetext;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog SaveFile = new Microsoft.Win32.SaveFileDialog();
            SaveFile.Title = "Выберите место для сохранения";
            SaveFile.Filter = "CS|*.cs|XAML|.xaml";
            if (SaveFile.ShowDialog() == true)
            {
                File.WriteAllText(SaveFile.FileName, Editor.Text);
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog SaveFile = new Microsoft.Win32.SaveFileDialog();
            SaveFile.Title = "Выберите место для создания файла";
            SaveFile.Filter = "CS|*.cs|XAML|.xaml";
            if (SaveFile.ShowDialog() == true)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(SaveFile.FileName))
                    {
                            sw.WriteLine("");
                    }
                }
                catch
                {
                    //Действия при ошибке
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            Editor.Paste();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Editor.Clear();
        }

        private void Run()
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();
            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = Output;
            CompilerResults results = icc.CompileAssemblyFromSource(parameters, Editor.Text);
            if (results.Errors.Count > 0)
            {
                string errors = "";

                foreach (CompilerError CompErr in results.Errors)
                {
                    errors = errors +
                                "Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";" +
                                Environment.NewLine + Environment.NewLine;
                    MessageBox.Show("Ошибки: " + errors);
                }
            }
            else
            {
                MessageBox.Show("Сборка прошла успешно!");
                try
                {
                    Process.Start(Output);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex);
                }
            }
        }

        private void Compile()
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();
            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = Output;
            CompilerResults results = icc.CompileAssemblyFromSource(parameters, Editor.Text);
            if (results.Errors.Count > 0)
            {
                string errors = "";

                foreach (CompilerError CompErr in results.Errors)
                {
                    errors = errors +
                                "Строка " + CompErr.Line +
                                ", Номер: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";" +
                                Environment.NewLine + Environment.NewLine;
                    MessageBox.Show("Ошибки: " + errors);
                }
            } else
            {
                MessageBox.Show("Сборка прошла успешно!");
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.Delete(Output);
            }
            finally
            {
                Run();
            }
        }
        private void Compile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.Delete(Output);
            }
            finally
            {
                Compile();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }
    }
}