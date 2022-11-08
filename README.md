## О проекте
Пример реализации Api и тестов с поднятием БД в Docker (используя библиотек TestContainers и Respawn)

## Проекты для запуска
#### Api
Запуск HTTP-сервера

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
