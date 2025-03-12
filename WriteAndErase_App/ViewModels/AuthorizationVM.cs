using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;
using WriteAndErase_App.Models;
using Avalonia.Controls.Shapes;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia;
using Avalonia.Threading;

namespace WriteAndErase_App.ViewModels
{
    class AuthorizationVM : ViewModelBase
    {
        private List<User> _listUser;

        public List<User> ListUser { get => _listUser; set => this.RaiseAndSetIfChanged(ref _listUser, value); }
        
        private string _login;

        public string Login { get => _login; set => this.RaiseAndSetIfChanged(ref _login, value); }

        private string _password;

        public string Password { get => _password; set => this.RaiseAndSetIfChanged(ref _password, value); }

        bool _TextBoxEnable = true;

        public bool TextBoxIsEnable { get => _TextBoxEnable; set => this.RaiseAndSetIfChanged(ref _TextBoxEnable, value); }

        public AuthorizationVM()
        {
            _listUser = MainWindowViewModel.myСonnection.Users.ToList();
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Tick += new EventHandler(StopTimer);
        }
        public void ToProductPage()
        {
            int id = 1;
            MainWindowViewModel.Instance.ContentPage = new ProductPage(id);
        }

        public async void ToAuthComplete()
        {
            int id = 0;
            CheckAuth(_login, _password, ref id);
            if (id != 0)
            {
                MainWindowViewModel.Instance.ContentPage = new ProductPage(id);
            }
            else
            {
                string Messege = "Проверьте корректность логина и пароля!";
                ButtonResult result1 = await MessageBoxManager.GetMessageBoxStandard("Внимание! Ошибка авторизавции!", 
                    Messege, ButtonEnum.Ok).ShowAsync();
            }
        }


        private void CheckAuth(string login, string password, ref int id)
        {
            User? user = _listUser.FirstOrDefault(x => x.Userlogin == login && x.Userpassword == password);
            bool foundUser = _listUser.Any(x => x.Userlogin == login && x.Userpassword == password);
            if (foundUser && user?.Userid != null)
            {
                id = user.Userid;
            }
            else
            {                
                CreateCaptha();
            }
        }

        #region Генерация капчи

        Canvas _Captcha;

        public Canvas Captcha { get => _Captcha; set => this.RaiseAndSetIfChanged(ref _Captcha, value); }

        public char rndLet()
        {
            Random rnd = new Random();
            int randomIndex = rnd.Next(0, 26);
            char letter = (char)('a' + randomIndex);
            return letter;
        }

        #region Свойства и переменные для элементов капчи        

        bool _ButtonVisibleAuth = true;
        bool _ButtonVisibleCheckedCaptchaKod = false;
        bool _TextBoxVisible = false;

        public bool ButtonVisibleAuth { get => _ButtonVisibleAuth; set => this.RaiseAndSetIfChanged(ref _ButtonVisibleAuth, value); }
        public bool ButtonVisibleCheckedCaptchaKod { get => _ButtonVisibleCheckedCaptchaKod; set => this.RaiseAndSetIfChanged(ref _ButtonVisibleCheckedCaptchaKod, value); }
        public bool TextBoxVisible { get => _TextBoxVisible; set => this.RaiseAndSetIfChanged(ref _TextBoxVisible, value); }

        string _Kod;
        public string Kod { get => _Kod; set => this.RaiseAndSetIfChanged(ref _Kod, value); }

        List<string> kodList = new List<string>();
        public List<string> KodList { get => kodList; set => kodList = this.RaiseAndSetIfChanged(ref kodList, value); }


        #endregion

        public void CreateCaptha()
        {
            ButtonVisibleAuth = false;
            Random rnd = new Random();
            SolidColorBrush color1 = new SolidColorBrush(Color.FromRgb(
                Convert.ToByte(rnd.Next(151, 256)),
                Convert.ToByte(rnd.Next(151, 256)),
                Convert.ToByte(rnd.Next(151, 256)))
            );
            SolidColorBrush color2 = new SolidColorBrush(Color.FromRgb(
                Convert.ToByte(rnd.Next(102, 150)),
                Convert.ToByte(rnd.Next(102, 150)),
                Convert.ToByte(rnd.Next(102, 150)))
            );
            SolidColorBrush color3 = new SolidColorBrush(Color.FromRgb(
                Convert.ToByte(rnd.Next(0, 101)),
                Convert.ToByte(rnd.Next(0, 101)),
                Convert.ToByte(rnd.Next(0, 101)))
            );

            Canvas canvas = new Canvas()
            {
                Width = 300,
                Height = 300,
                Background = color3,
            };

            for (int i = 0; i < 50; i++)
            {
                Line line = new Line()
                {
                    StartPoint = new Point(rnd.Next(300), rnd.Next(300)),
                    EndPoint = new Point(rnd.Next(300), rnd.Next(300)),
                    Stroke = color2,
                    StrokeThickness = 2,
                };
                canvas.Children.Add(line);
            }

            double lastX = 10;
            double lastY = 100;

            for (int i = 0; i < 3; i++)
            {
                TextBlock number = new TextBlock()
                {
                    Text = rnd.Next(10).ToString(),
                    FontSize = rnd.Next(20, 30),
                    Foreground = color1,
                    Padding = new Avalonia.Thickness(lastX, lastY)
                };

                KodList.Add(number.Text);

                if (rnd.Next(2) == 0) number.TextDecorations = TextDecorations.Strikethrough;
                else if (rnd.Next(2) == 1) number.FontStyle = FontStyle.Italic;
                else if (rnd.Next(2) == 2) number.FontWeight = FontWeight.Bold;

                lastX += number.FontSize + 10;
                lastY += rnd.Next(0, 50);

                TextBlock letter = new TextBlock()
                {
                    Text = rndLet().ToString(),
                    FontSize = rnd.Next(20, 30),
                    Foreground = color1,
                    Padding = new Avalonia.Thickness(lastX, lastY)
                };

                KodList.Add(letter.Text);

                if (rnd.Next(3) == 0) letter.TextDecorations = TextDecorations.Strikethrough;
                else if (rnd.Next(3) == 1) letter.FontStyle = FontStyle.Italic;
                else if (rnd.Next(3) == 2) letter.FontWeight = FontWeight.Bold;

                lastX += number.FontSize + 10;
                lastY -= rnd.Next(0, 50);

                canvas.Children.Add(letter);
                canvas.Children.Add(number);
            }

            Border border = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2),
                Width = 300,
                Height = 300
            };
            canvas.Children.Add(border);

            Captcha = canvas;
            TextBoxVisible = true;
            ButtonVisibleCheckedCaptchaKod = true;
        }

        #endregion

        public void CheckKod()
        {
            if (Kod == String.Join("", KodList) && Kod != "" && Login != "" && _listUser.Any(x => x.Userlogin == Login) 
                && Password != "" && _listUser.Any(x => x.Userpassword == Password))
            {
                TextBoxVisible = false;
                ButtonVisibleCheckedCaptchaKod = false;
                Captcha.IsVisible = false;
                Kod = "";
                KodList.Clear();
                ToAuthComplete();
            }
            else if (Kod != String.Join("", KodList) || Kod != "" || Password == "" || Login == "" || _listUser.Any(x => x.Userlogin != Login) 
                || _listUser.Any(x => x.Userpassword == Password))
            {
                StartTimer();
            }
        }

        #region Таймер
        DispatcherTimer timer = new DispatcherTimer();

        public void StartTimer()
        {
            timer.Start();
            Kod = "";
            KodList.Clear();
            TextBoxVisible = false;
            ButtonVisibleCheckedCaptchaKod = false;
            Captcha.IsVisible = false;
            Login = "";
            Password = "";
            TextBoxIsEnable = false;
        }

        public void StopTimer(object sender, EventArgs e)
        {
            timer.Stop();
            TextBoxIsEnable = true;
            TextBoxVisible = true;
            ButtonVisibleCheckedCaptchaKod = true;
            Captcha.IsVisible = true;
            CreateCaptha();
        }
        #endregion
    }
}
