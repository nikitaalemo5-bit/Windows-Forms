# Gordeev — Windows Forms / SQL Server

Учебный проект по темам авторизации, ролевого доступа и BindingNavigator.

## Перед запуском
1. Установите Visual Studio с компонентом **Разработка классических приложений .NET** и SQL Server Express/Developer.
2. Откройте `GordeevApp.csproj` в Visual Studio.
3. В SSMS по очереди выполните `Part1_Module4/01_Create_Module4DB.sql`, `Part2_Roles/02_Create_User_Data.sql`, `Part2_BindingNavigator/03_Create_DB_Users.sql`.
4. Проверьте `Data Source` в `LoginForm.cs`. По умолчанию: `localhost\SQLEXPRESS`, Windows Authentication.
5. Запустите проект.

## Демонстрационный вход
- Логин `admin`, пароль `Admin123!`
- Логин `user`, пароль `User123!`

## Важно
Это учебная заготовка для демонстрации UI и SQL CRUD. Для реального приложения замените сравнение паролей на безопасное хеширование с солью, добавьте журналирование и полноценную блокировку после трех неудачных попыток. Для отчёта вставьте собственные скриншоты после запуска приложения на своём компьютере; изображения из методички не подтверждают выполнение.
