## О проекте
Пример реализации Api и тестов в Docker

## Проекты для запуска
#### Api
Запуск HTTP-сервера

## Генерация клиента
```shell
# Генерация происходит с использованием утилиты NSwag
# https://github.com/RicoSuter/NSwag

# Установить NSwag
npm install -g npm
npm i -g nswag

# Сгенерировать спецификацию open api (через swagger) и сохранить в файл
swagger.json

# Выполнить PowerShell-скрипт
GenerateClient.ps1
```

## Тесты
```shell
dotnet test
```

## Создание миграций БД

```shell
#Установка утилиты
dotnet tool install --global dotnet-ef

#Создание миграций схемы
dotnet ef migrations add InitialMigration --startup-project src\Api -p src\PopulationDbContext -c PopulationDbContext
```
