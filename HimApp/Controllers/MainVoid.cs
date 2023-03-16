using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HimApp.Controllers
{
    internal class MainVoid
    {
        public static string GenCode()
        {
            Random random = new Random();
            char[] SymbolCodeChar = new char[]
            {
                'A','a','B','b','C','c',
                'D','d','F','f','X','x',
                '0','1','2','3','4','5',
                '6','7','8','9','G','g',
                'Z','z','S','s','N','n',
                'V','v','Q','q','E','e',
                'T','t','J','j','L','l',
                'H','h','U','u','I','i',
                'P','p','M','m','K','k'
            };

            string ResCode = "";

            for (int i = 0; i <= 32; i++)
            {
                int RandomIndex = random.Next(0, SymbolCodeChar.Length - 1);
                ResCode += SymbolCodeChar[RandomIndex];
            }
            return ResCode;
        }

        public static void FatalErrorMessage(string error)
        {
            MessageBox.Show(error, "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ErrorMessage(string error) 
        {
            MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
        }

        public static void InformationMessage(string info)
        {
            MessageBox.Show(info, "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static void QuestionMessage(string question)
        {
            MessageBox.Show(question, "", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        public static void OnlyNumber(TextBox tb, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".")
                && !tb.Text.Contains(".")
                && tb.Text.Length != 0))
            {
                e.Handled = true;
            }
        }
        

    }
}
