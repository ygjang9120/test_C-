using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

//로그인 상태일떄만 로그아웃 버튼이 보이게끔 설정
namespace WpfFileUploadSimple.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool b && b) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}