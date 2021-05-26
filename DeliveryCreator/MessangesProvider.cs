﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeliveryCreator
{
    class MessagesProvider
    {
        public static void UnknownError(Exception ex)
        {
            MessageBox.Show("Неизвестная ошибка: " + ex?.Message, "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static void Error(Exception ex, string message)
        {
            var exMsg = ex.Message;
            var innerEx = ex.InnerException;
            while (innerEx != null)
            {
                exMsg += " " + innerEx.Message;
                innerEx = innerEx.InnerException;
            }

            MessageBox.Show($"{exMsg}{Environment.NewLine}Ошибка: {ex?.Message}.", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Successfully(string message)
        {
            MessageBox.Show(message, "Информация", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void NoResult(string message)
        {
            MessageBox.Show(message, "Информация",
                MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public static void IncorrectData(string message)
        {
            MessageBox.Show(message, "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static bool Confirm(string question)
        {
            return MessageBox.Show(question, "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}
