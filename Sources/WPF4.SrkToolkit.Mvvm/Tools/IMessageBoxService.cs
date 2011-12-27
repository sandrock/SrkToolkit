using System;
using System.Windows;

namespace SrkToolkit.Mvvm.Tools {
    public interface IMessageBoxService {
        MessageBoxResult Show(string messageBoxText);
        MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button);
        MessageBoxResult ShowDebug(string messageBoxText);
    }
}
