// WpfFileUploadSimple/ViewModels/MainViewModel.cs

using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WpfFileUploadSimple.Models;
using WpfFileUploadSimple.Services;
using WpfFileUploadSimple.ViewModels.Base;

namespace WpfFileUploadSimple.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly DatabaseService _dbService = new DatabaseService();

        private string _userName;
        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(); } }

        private string _selectedFilePath;

        private string _selectedFileName = "선택된 파일 없음";
        public string SelectedFileName { get => _selectedFileName; private set { _selectedFileName = value; OnPropertyChanged(); } }

        private bool _isLoggedIn;
        public bool IsLoggedIn { get => _isLoggedIn; private set { _isLoggedIn = value; OnPropertyChanged(); } }

        public ObservableCollection<string> LogMessages { get; } = new ObservableCollection<string>();

        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand SelectFileCommand { get; }
        public ICommand UploadCommand { get; }

        public MainViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin, o => !IsLoggedIn);
            LogoutCommand = new RelayCommand(ExecuteLogout, o => IsLoggedIn);
            SelectFileCommand = new RelayCommand(ExecuteSelectFile, o => IsLoggedIn);
            UploadCommand = new RelayCommand(ExecuteUpload, o => IsLoggedIn && !string.IsNullOrEmpty(_selectedFilePath));
        }

        private void ExecuteLogin(object parameter)
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                MessageBox.Show("사용자 이름을 입력해주세요.");
                return;
            }
            IsLoggedIn = true;
            LogMessages.Clear();
            LogMessages.Insert(0, $"{UserName}님, 환영합니다. 업로드할 파일을 선택하세요.");
        }

        private void ExecuteLogout(object parameter)
        {
            UserName = "";
            _selectedFilePath = null;
            SelectedFileName = "선택된 파일 없음";
            IsLoggedIn = false;
            LogMessages.Clear();
        }

        private void ExecuteSelectFile(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _selectedFilePath = openFileDialog.FileName;
                SelectedFileName = Path.GetFileName(_selectedFilePath);
            }
        }

        private void ExecuteUpload(object parameter)
        {
            byte[] fileContent;
            try
            {
                fileContent = File.ReadAllBytes(_selectedFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"파일을 읽는 중 오류가 발생했습니다: {ex.Message}");
                return;
            }

            try
            {
                var fileInfo = new FileInfo(_selectedFilePath);
                var newLog = new UploadLog
                {
                    UploaderUserName = this.UserName,
                    OriginalFileName = fileInfo.Name,
                    SavedPath = _selectedFilePath,
                    FileSizeKB = Math.Round(fileInfo.Length / 1024.0, 2),
                    UploadTimestamp = DateTime.Now,
                    FileContent = fileContent
                };

                _dbService.SaveLog(newLog);
                LogMessages.Insert(0, $"[성공] '{UserName}'님이 '{newLog.OriginalFileName}' 파일의 내용을 DB에 저장했습니다.");
            }
            catch (Exception ex)
            {
                LogMessages.Insert(0, $"[실패] DB 저장 중 오류: {ex.Message}");
            }
            finally
            {
                _selectedFilePath = null;
                SelectedFileName = "선택된 파일 없음";
            }
        }
    }
}