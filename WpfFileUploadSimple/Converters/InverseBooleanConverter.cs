using System;
using System.Globalization;
using System.Windows.Data;

//로그인 상태일때 사용자 이름 쓰는 칸 비활성화
namespace WpfFileUploadSimple.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is bool b && b);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}