using DevExpress.Mvvm;
using MaterialDesignThemes.Wpf;
using Nav_View_Airplanes.Models;
using Nav_View_Airplanes.Services;
using Nav_View_Airplanes.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Nav_View_Airplanes.ViewModels
{
    public class mPersonViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly GetService _getService;
        public List<User> Dispatchers { get; set; } = new();
        public List<User> Users { get; set; } = new();
        public User SelectedDispatcher { get; set; }
        public Visibility ButtonsVisible { get; set; } = Visibility.Visible;
        public Visibility ChangeVisible { get; set; } = Visibility.Collapsed;
        public string TextButton { get; set; }

        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }


        public mPersonViewModel(PageService pageService, GetService getService)
        {
            _pageService = pageService;
            _getService = getService;
            Load();
        }

        public async void Load()
        {
            var context = await _getService.GetUsers();
            Users = context;
            foreach (var user in context)
            {
                if (user.Indicator == 1)
                {
                    Dispatchers.Add(user);
                }
            }
        }
        public DelegateCommand AddDispatcher => new(() =>
        {
            ButtonsVisible = Visibility.Collapsed;
            ChangeVisible = Visibility.Visible;
            TextButton = "Добавить";
        });

        public DelegateCommand HideChange => new(() =>
        {
            ButtonsVisible = Visibility.Visible;
            ChangeVisible = Visibility.Collapsed;
        });

        public DelegateCommand AddOrChange => new(() =>
        {
            if (!Surname.Equals("") && !Name.Equals("") && !Patronymic.Equals("") && !Login.Equals("") && !Password.Equals(""))
            {
                if (Users.Where(i => i.Login.Equals(Login)).SingleOrDefault() == null)
                {
                    User user = new User
                    {
                        Iduser = Users.Max(i => i.Iduser) + 1,
                        Surname = Surname,
                        Name = Name,
                        Patronymic = Patronymic,
                        Login = Login,
                        Password = Password,
                        Indicator = 1,
                    };
                    _getService.AddUser(user);
                    MessageBox.Show("Диспетчер добавлен", "Сообщение");
                    Load();
                }
                else
                {
                    MessageBox.Show("Пользователь с таким логином уже существует", "Внимание");
                }
            }
        });

        public DelegateCommand Back => new(() => _pageService.ChangePage(new MainPage()));

    }
}
