## Работа с БД на примере MongoDB

WebGame - библиотека классов для реализации мультиплеерной игры Камень Ножницы Бумага.
ConsoleApp - консольное приложение для игры с компьютером.

В этой задаче вы доработаете приложение ConsoleApp так, чтобы оно хранило все данные в MongoDB.

К счастью, вся работа с БД уже изолирована за интерфейсами репозиториев IUserRepository и IGameRepository.
Сейчас у этих интерфейсов есть реализации для хранения всех данных в памяти.
Вам нужно создать новые реализации, для хранения данных в MongoDB. Ниже вся задача разбита на этапы:

### UserEntity и GameEntity

Сначала нужно подготовить сами классы сущностей к тому, чтобы их мог сохранять и загружать из базы драйвер MongoDb.
Драйвер делает это с помощью Bson сериализатора. Можно проверить работу сериализации отдельно от работы с сервером Mongo.

1. Изучите, а потом запустите тест BsonSerializationTest. UserEntity должен сериализоваться без ошибок, а GameEntity нет. Нужно настроить сериализацию GameEntity.
2. Добавьте атрибут [BsonConstructor] над конструктором с полным набором аргументов.
3. Добавьте атрибут [BsonElement] над всеми readonly свойствами. По умолчанию Mongo их игнорирует.
4. Запустите тест и проверьте, что теперь класс GameEntity успешно сериализуется 

Более подробно про настройку сериализации в Bson читайте в документации: http://mongodb.github.io/mongo-csharp-driver/2.7/reference/bson/mapping/

### MongoUserRepository

1. Реализуйте в классе MongoUserRepository все методы.  Вам поможет документация: http://mongodb.github.io/mongo-csharp-driver/2.7/reference/driver/crud/reading/ Проверьте работоспособность тестами на этот репозиторий.
2. С помощью MongoDB Compass убедитесь, что после выполнения тестов, в базе web-game-tests создаются коллекции и документы.

### Индекс по UserEntity.Login

1. Запустите тест SearchByLoginFast. Запомните, как долго он работал.
2. Ускорить операцию поиска по логину можно создав индекс. Логично сразу сделать индекс уникальным. Для простоты добавьте создание индекса в конструкторе репозитория. (MongoDB игнорирует создание повторного индекса).
Вам поможет документация http://mongodb.github.io/mongo-csharp-driver/2.7/reference/driver/admin/#indexes
3. Повторно запустите тест SearchByLoginFast и сравните время работы с первоначальным.
4. С помощью MongoDB Compass убедитесь, что после выполнения тестов, у коллекции users появился индекс и он используется.

### Users в ConsoleApp

1. Поменяйте InMemoryUserRepository на MongoUserRepository в ConsoleApp. Пример создания Mongo репозитория смотрите в тестах на репозитории.
2. С помощью MongoDB Compass проверьте, что после окончания игры у пользователя обновляется количество сыгранных игр в БД.


### MongoGameRepository

1. Реализуйте в классе MongoGameRepository все методы. Вам поможет документация: http://mongodb.github.io/mongo-csharp-driver/2.7/reference/driver/crud/reading/
2. Проверьте работоспособность тестами на этот репозиторий.
3. С помощью MongoDB Compass убедитесь, что после выполнения тестов, в базе web-game-tests создаются коллекции и документы.

### Games в ConsoleApp

1. Поменяйте InMemoryGameRepository на MongoGameRepository в ConsoleApp. 
2. Проверьте, что прерванная на середине игра сохраняет состояние в БД и при перезапуске продолжается.

### Бонус-задача «История игры»

Спроектируйте ещё одну сущность: GameTurnEntity — информация об одном завершённом туре игры (в рамках тура каждый игрок называет Камень Ножницы или Бумага, и определяется исход тура — победитель тура или ничья)
Объект этого типа возвращает GameEntity.FinishTurn().

Спроектируйте репозиторий этих сущностей. Добавьте его использование в ConsoleApp в двух местах: 

1. Сохраняйте эту сущность в БД в конце каждого тура.
2. В методе ShowScore получайте из репозитория последние 5 туров и показывайте информацию о том, кто что назвал и кто в итоге выиграл тур.

Убедитесь, что у вас есть правильный индекс, для того, чтобы запрос последних 5 туров работал быстро.


## Статьи и документация

[.NET MongoDB Driver Reference](http://mongodb.github.io/mongo-csharp-driver/2.7/)

## Статьи про проектирование в Монго

Вводные о моделировании данных: 

https://docs.mongodb.com/manual/core/data-modeling-introduction/
https://docs.mongodb.com/manual/core/data-model-design/

Ограничение на размер документа 16 Мб и другие ограничения. https://docs.mongodb.com/manual/reference/limits/#BSON-Document-Size 

О том, как принимать решения о проектировании БД:

* https://www.mongodb.com/blog/post/6-rules-of-thumb-for-mongodb-schema-design-part-1
* https://www.mongodb.com/blog/post/6-rules-of-thumb-for-mongodb-schema-design-part-2
* https://www.mongodb.com/blog/post/6-rules-of-thumb-for-mongodb-schema-design-part-3
