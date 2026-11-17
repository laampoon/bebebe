*   152ddfa Resolve merge conflict in README
|\  
| * 641d351 Change README in feature branch
* | c41a288 Change README in main branch
|/  
* 8b05bbc Update README in main
* 4f2afee Update README in feature branch
* 91d7ee9 Add project documentation
* 0beefec Initial commit: add practice project
## Git log after rebase
42c08c8 docs: add rebase demo file
ac01e33 Merge pull request #1 from laampoon/feature/task-35
63abeb7 docs: describe git workflow
af604e7 docs: add contribution guidelines
a2861b3 chore: add gitignore and editorconfig
f3cb7d0 Add Git history notes
152ddfa Resolve merge conflict in README
c41a288 Change README in main branch
641d351 Change README in feature branch
8b05bbc Update README in main
4f2afee Update README in feature branch
91d7ee9 Add project documentation
0beefec Initial commit: add practice project

## Задание №47. Git Rebase -i (Squash)

### История до объединения

b7343ca (HEAD -> feature/squash-demo) docs: add fourth demo note
b33ba8a docs: add third demo note
a737fc6 docs: add second demo note
8f2cd33 docs: add squash demo file

### История после объединения

ef02d1f (HEAD -> feature/squash-demo) docs: combine four squash demo commits
d7d9769 (feature/rebase-demo) docs: save git log after rebase
42c08c8 docs: add rebase demo file
ac01e33 (origin/main, main) Merge pull request #1 from laampoon/feature/task-35

### Результат

Четыре коммита успешно объединены в один с помощью git rebase -i HEAD~4 и команды squash.

## Задание №48. Git: аннотированные теги

### Создание тега
Создан аннотированный тег v1.0.0 с сообщением "Release v1.0.0".

### Проверка
Команда git tag показала:
v1.0.0

Команда git show v1.0.0 подтвердила:
- Тег: v1.0.0
- Сообщение: Release v1.0.0
- Тег указывает на стабильный коммит ветки main.

### Отправка на GitHub
Выполнена команда git push origin v1.0.0.

Проверка git ls-remote --tags origin v1.0.0 подтвердила наличие тега в удалённом репозитории.

## Задание №49. Восстановление коммита через reflog

### 1. Создание тестового коммита
Создан файл reflog-demo.txt.

Команда:
git commit -m "test: add reflog recovery demo"

### 2. Удаление коммита
Выполнена команда:
git reset --hard HEAD~1

Коммит временно исчез из истории текущей ветки.

### 3. Поиск коммита через reflog
Выполнена команда:
git reflog

Коммит найден в записи HEAD@{1}.

### 4. Восстановление коммита

Для восстановления использована команда:

git reset --hard <хеш коммита>

Мой хеш: 82cceb3
Хеш Дианы: 8331ec9

### 5. Проверка
Выполнены команды:
git log --oneline -1
git status
git reflog

Восстановление прошло успешно. Тестовый коммит снова находится в HEAD, файл reflog-demo.txt восстановлен.

## Задание 50. Поиск ошибки с помощью git bisect

**Цель:** найти коммит, в котором появилась ошибка, используя команду `git bisect`.

**Выполнение:**

1. Создана отдельная ветка `feature/bisect-demo`.
2. Создан файл `BisectDemo.cs` с рабочей функцией `AddTwo`, которая прибавляет 2 к числу.
3. Созданы промежуточные коммиты.
4. В коммите `4aa8234` намеренно допущена ошибка: `number + 2` заменено на `number - 2`.
5. Созданы последующие контрольные коммиты.
6. Запущен поиск:
   - `git bisect start`
   - `git bisect bad`
   - `git bisect good fce1aff`
7. Проверяемые версии отмечались командами `git bisect good` и `git bisect bad`.
8. После поиска Git определил первый плохой коммит.

**Результат:**

Найден первый плохой коммит: `4aa8234` — `bug: break AddTwo calculation`.

Git вывел сообщение:
`4aa8234ad646b54bad382c3ef398067bafe4848b is the first 'bad' commit`.

**Журнал поиска:** `git bisect log`.

**Завершение:** выполнена команда `git bisect reset`, возвращающая HEAD к последнему коммиту исходной ветки поиска.

**Вывод:** с помощью бинарного поиска Git удалось определить коммит, в котором появилась ошибка, не проверяя вручную каждый коммит.