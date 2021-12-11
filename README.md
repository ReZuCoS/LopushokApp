# ИС "Lopushok"

Для выполнения дэмоэкзамена были получены исходные данные для информационной подсистемы. 
Подсистема состоит из desktop-приложения и базы данных.

Подсистема позволяет пользователю просматривать данные об агентах, добавлять/изменять/удалять данные агентов.

## Начало работы

Desktop-приложение припрепленно в виде проекта в данный репозиторий
База данных для приложения прикреплена в файле DatabaseScript.sql

### Необходимые условия

Для установки программного обеспечения требуется MSSQL Server с именем сервера соответствующим формату *имя_компьютера*\SQLEXPRESS и Visual Studio 2019

### Установка

Для установки требуется:
1. Скачать файлы проекта
2. Запустить DatabaseScript.sql скрипт для создания базы данных на локальном SQL-сервере
3. Запустить LopushokApp.sln файл средствами Microsoft Visual Studio
4. Нажать клавишу F5 для запуска приложения

### Альтернативная установка

1. Скачать файлы проекта
2. Запустить DatabaseScript.sql скрипт для создания базы данных на локальном SQL-сервере
3. Запустить LopushokApp.sln файл средствами Microsoft Visual Studio
4. Собрать проект
5. Запустить LopushokApp.exe файл, расположенный по пути *папка_установки*\PoprijonokApp\LopushokApp\bin\Debug\

## Автор
Цыганок Василий

### Дата
07.12.2021
