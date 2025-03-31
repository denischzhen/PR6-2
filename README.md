# ПРАКТИЧЕСКАЯ РАБОТА №6

## СОЗДАНИЕ АВТОМАТИЗИРОВАННОГО UNIT-ТЕСТА

### Часть 2

## Скрипт базы данных и содержимое таблицы с пользователями

``` sql
CREATE DATABASE PR_6_2;
GO

USE PR_6_2;
GO

-- Таблица Клиенты
CREATE TABLE Clients (
    ClientID INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(100) NOT NULL,
    BirthDate DATE NOT NULL,
    PhoneNumber NVARCHAR(15),
    Email NVARCHAR(100) UNIQUE,
    RegistrationDate DATE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL
);
```
![image](bd.png)


## Результаты выполнения тестов
![image](results.png)

