# ServerOnly

## Пока реализованно только Авторизация и Аутентификация

## Для запуска проекта также понадобится БД postgres
### В Programm.cs можно найти строчку подключения
### Лично я использую docker для работы с бд
### Чтобы у вас все работало также как и у меня, просто введите эту строчку для создания docker контейнера с postgres

### docker run -p 5432:5432 --name postgres -e POSTGRES_PASSWORD=12345 -d postgres
