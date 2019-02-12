## Задача MongoRepositories

Ваша задача, внедрить работу с MongoDB в проект игры.
К счастью, вся работа с БД уже изолирована за интерфейсами репозиториев IUserRepository и IGameRepository.
Сейчас у этих интерфейсов есть реализации для хранения всех данных в памяти.
Вам нужно создать новые реализации, для хранения данных в MongoDB. Ниже вся задача разбита на этапы:

### UserEntity

Сначала нужно подготовить сами классы сущностей к тому, чтобы их мог сохранять и загружать из базы драйвер MongoDb.
Драйвер делает это с помощью Bson сериализатора. Можно проверить работу сериализации отдельно от работы с сервером Mongo.

1. Изучите, а потом запустите тест BsonSerializationTest.CanSerializeUser.
2. Чтобы тест заработал, нужно объяснить сериализатору, что десериализовывать объект нужно с помощью конструктора. 
Это можно сделать пометив конструктор UserEntity атрибутом [BsonConstructor]

### MongoUserRepository

Реализуйте в классе MongoUserRepository все методы. Проверьте работоспособность тестами на этот репозиторий.

### Users в ConsoleApp

Поменяйте InMemoryUserRepository на MongoUserRepository в ConsoleApp. 
Проверьте, что после окончания игры у пользователя обновляется количество сыгранных игр в БД.

### MongoGameRepository

Реализуйте в классе MongoGameRepository все методы. Проверьте работоспособность тестами на этот репозиторий.

### Games в ConsoleApp

Поменяйте InMemoryGameRepository на MongoGameRepository в ConsoleApp. 
Проверьте, что прерванная на середине игра сохраняет состояние в БД и при перезапуске продолжается.


## Статьи и документация

[.NET MongoDB Driver Reference](http://mongodb.github.io/mongo-csharp-driver/2.7/)

## Статьи про проектирование в Монго

Вводная о моделировании данных: https://docs.mongodb.com/manual/core/data-model-design/

Ограничение на размер документа 16 Мб и другие ограничения. https://docs.mongodb.com/manual/reference/limits/#BSON-Document-Size 

Атомарность: https://docs.mongodb.com/manual/core/data-model-operations/

О том, как принимать решения о проектировании БД:

* https://www.mongodb.com/blog/post/6-rules-of-thumb-for-mongodb-schema-design-part-1
* https://www.mongodb.com/blog/post/6-rules-of-thumb-for-mongodb-schema-design-part-2
* https://www.mongodb.com/blog/post/6-rules-of-thumb-for-mongodb-schema-design-part-3
