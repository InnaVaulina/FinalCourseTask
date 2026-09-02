using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBLazorHybridClient.Client.Account.UserModel
{
    public static class RoleDescr
    {
        public static string[] roles =
            {
            "admin",
            "work",
            "blog",
            "progect",
            "service",
            "contact",
            "mainpage",
            "myrole",
            "users"
            };

        public static string[] functionName =
            {
            "Администратор",
            "Рабочий стол",
            "Блог",
            "Проекты",
            "Услуги",
            "Контакты",
            "Главная",
            "Моя страница",
            "Управление пользователями"
            };

        public static string[] deskr =
            {
            "все функции приложения",
            "работа с заявками клиентов",
            "ведение блога",
            "редактирование информации о проектах",
            "редактирование информации об услугах",
            "редактирование информации о контактах",
            "редактирование хедера сайта и информации о компании",
            "страница пользователя",
            "Управление пользователями системы, назначение ролей"
            };
    }
}
