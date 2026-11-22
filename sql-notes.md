# SQL notes
> Учебная БД: PracticeDB. Скрипт создания — `init.sql`.
> Все запросы выполнялись в SSMS.

## Task 01 — Создание базы данных и таблиц
### SQL

```sql
-- Выполнен скрипт init.sql (Приложение А)
-- Создание базы данных
CREATE DATABASE PracticeDB;
GO
USE PracticeDB;
GO

-- Создание таблиц (пример для Departments)
CREATE TABLE Departments (
    DepartmentId   INT PRIMARY KEY IDENTITY(1,1),
    DepartmentName NVARCHAR(100) NOT NULL
);
-- ... и остальные таблицы (Employees, Projects, EmployeeProjects)
```
### Результат

База PracticeDB и 4 таблицы успешно созданы.

### Вывод

Изучил синтаксис DDL (CREATE DATABASE, CREATE TABLE). Понял, как работают внешние ключи (FOREIGN KEY) и ограничения (PRIMARY KEY, IDENTITY).

## Task 02 — INSERT: заполнение данными
### SQL

```sql
USE PracticeDB;
GO


INSERT INTO Departments (DepartmentName) VALUES
    (N'Разработка'),
    (N'Аналитика'),
    (N'Тестирование'),
    (N'Менеджмент');


INSERT INTO Employees (EmployeeName, Salary, HireDate, DepartmentId, ManagerId) VALUES
    (N'Анна Иванова',    65000, '2020-03-15', 1, NULL),
    (N'Пётр Сидоров',    72000, '2019-07-01', 1, NULL),
    (N'Мария Кузнецова', 55000, '2021-11-20', 1, NULL),
    (N'Иван Петров',     48000, '2023-02-10', 1, NULL),
    (N'Ольга Смирнова',  61000, '2020-09-05', 2, NULL),
    (N'Дмитрий Орлов',   58000, '2022-04-18', 2, NULL),
    (N'Елена Волкова',   52000, '2023-06-01', 2, NULL),
    (N'Сергей Морозов',  45000, '2021-12-12', 3, NULL),
    (N'Наталья Соколова',43000, '2024-01-15', 3, NULL),
    (N'Алексей Лебедев', 90000, '2018-05-25', 4, NULL);


INSERT INTO Projects (ProjectName, StartDate) VALUES
    (N'CRM System',      '2023-01-10'),
    (N'Mobile App',      '2023-05-01'),
    (N'Data Warehouse',  '2022-11-20'),
    (N'AI Assistant',    '2024-02-01'),
    (N'Legacy Refactor', '2021-06-15');


INSERT INTO EmployeeProjects (EmployeeId, ProjectId, Hours) VALUES
    (1, 1, 120), (1, 3, 80),
    (2, 1, 150), (2, 4, 60),
    (3, 2, 100), (4, 2, 90),
    (4, 5, 40),  (5, 3, 110),
    (6, 3, 70),  (6, 4, 50),
    (7, 2, 95),  (8, 5, 130),
    (10, 4, 200),(10, 1, 30);


SELECT 'Departments' AS TableName, COUNT(*) AS [Count] FROM Departments
UNION ALL
SELECT 'Employees', COUNT(*) FROM Employees
UNION ALL
SELECT 'Projects', COUNT(*) FROM Projects
UNION ALL
SELECT 'EmployeeProjects', COUNT(*) FROM EmployeeProjects;
```
### Результат
Departments: 4, Employees: 10, Projects: 5, EmployeeProjects: 14.

### Вывод
Понял, как работает множественный INSERT (один запрос на несколько строк). Убедился, что количество строк совпадает с ожидаемым.
## Task 03 — SELECT + WHERE: базовые выборки
### SQL

```sql
USE PracticeDB;
GO

-- 1. Сотрудники с зарплатой > 50000
SELECT EmployeeId, EmployeeName, Salary
FROM Employees
WHERE Salary > 50000;
GO

-- 2. Сотрудники, нанятые в 2023 году
SELECT EmployeeId, EmployeeName, HireDate
FROM Employees
WHERE YEAR(HireDate) = 2023;
GO

-- 3. Сотрудники, чьё имя начинается на «А»
SELECT EmployeeId, EmployeeName
FROM Employees
WHERE EmployeeName LIKE N'А%';
GO

-- 4. Сотрудники из отдела DepartmentId = 2
SELECT EmployeeId, EmployeeName, DepartmentId
FROM Employees
WHERE DepartmentId = 2;
GO

-- 5. Сотрудники с зарплатой в диапазоне 40000–60000
SELECT EmployeeId, EmployeeName, Salary
FROM Employees
WHERE Salary BETWEEN 40000 AND 60000;
GO
```
### Результат
* Сотрудники с зарплатой > 50000: Анна Иванова, Пётр Сидоров, Мария Кузнецова, Ольга Смирнова, Дмитрий Орлов, Елена Волкова, Алексей Лебедев
* Нанятые в 2023: Иван Петров, Елена Волкова
* Сотрудники, чьё имя начинается на «А»: Анна Иванова, Алексей Лебедев
* Сотрудники из отдела DepartmentId = 2: Ольга Смирнова, Дмитрий Орлов, Елена Волкова
* Сотрудники с зарплатой в диапазоне 40000–60000: Мария Кузнецова, Иван Петров, Дмитрий Орлов, Елена Волкова, Сергей Морозов, Наталья Соколова

### Вывод
Изучил оператор WHERE и логические условия. Понял, как работает LIKE для поиска по шаблону и BETWEEN для диапазонов.

## Task 04 — ORDER BY + TOP: сортировка и лимит
### SQL

```sql
USE PracticeDB;
GO

SELECT TOP 3 EmployeeId, EmployeeName, Salary
FROM Employees
ORDER BY Salary DESC;
GO

SELECT TOP 5 EmployeeId, EmployeeName, HireDate
FROM Employees
ORDER BY HireDate ASC;
GO

SELECT EmployeeId, EmployeeName, DepartmentId, Salary
FROM Employees
ORDER BY DepartmentId ASC, Salary DESC;
GO
```
### Результат
* Топ-3 по зарплате: Алексей Лебедев, Пётр Сидоров, Анна Иванова.

* Топ-5 самых давних: Алексей Лебедев, Пётр Сидоров, Ольга Смирнова, Анна Иванова, Мария Кузнецова.

* Сотрудники отсортированы по отделам (1, 2, 3, 4), внутри отдела — по убыванию зарплаты.

### Вывод
Научился использовать ORDER BY для сортировки по одному или нескольким столбцам (с указанием ASC/DESC). Понял, как работает TOP N для ограничения количества строк.

## Task 05 — Агрегация: COUNT, SUM, AVG, MIN, MAX
### SQL

```sql
USE PracticeDB;
GO

SELECT 
    COUNT(*) AS TotalEmployees,
    SUM(Salary) AS TotalSalary,
    ROUND(AVG(Salary), 2) AS AverageSalary,
    MIN(Salary) AS MinSalary,
    MAX(Salary) AS MaxSalary
FROM Employees;
GO

SELECT COUNT(*) AS EmployeesWithHighSalary
FROM Employees
WHERE Salary > 50000;
GO
```
### Результат
* TotalEmployees: 10

* TotalSalary: 589000.00

* AverageSalary: 58900.00

* MinSalary: 43000.00

* MaxSalary: 90000.00

* EmployeesWithHighSalary: 7

### Вывод
Изучил агрегатные функции. Узнал, что COUNT(*) считает все строки, а COUNT(column) — только строки, где значение не NULL. Понял, как работает ROUND для округления и CASE внутри COUNT для условного подсчета.

## Task 06 — GROUP BY: статистика по отделам
### SQL

```sql
USE PracticeDB;
GO

SELECT 
    DepartmentId,
    COUNT(*) AS EmployeeCount,
    ROUND(AVG(Salary), 2) AS AvgSalary,
    SUM(Salary) AS TotalSalary
FROM Employees
GROUP BY DepartmentId
ORDER BY AvgSalary DESC;
GO
```
### Результат

DepartmentId|EmployeeCount|AvgSalary|TotalSalary
---|---|---|---
4|1|90000.00|90000.00
1|4|60000.00|240000.00
2|3|57000.00|171000.00
3|2|44000.00|88000.00

### Вывод
Научился использовать GROUP BY для группировки строк по значению столбца. Понял, что агрегатные функции (COUNT, AVG, SUM) применяются к каждой группе отдельно. Также освоил сортировку результата группировки через ORDER BY.

## Task 07 — GROUP BY + HAVING: фильтрация групп
### SQL

```sql
USE PracticeDB;
GO

SELECT DepartmentId, COUNT(*) AS EmployeeCount
FROM Employees
GROUP BY DepartmentId
HAVING COUNT(*) > 2;
GO

SELECT DepartmentId, ROUND(AVG(Salary), 2) AS AvgSalary
FROM Employees
GROUP BY DepartmentId
HAVING AVG(Salary) > 55000;
GO

SELECT DepartmentId, SUM(Salary) AS TotalSalary
FROM Employees
GROUP BY DepartmentId
HAVING SUM(Salary) > 150000;
GO
```

### Результат
* Отделы с >2 сотрудниками: DepartmentId = 1 (4 сотрудника), DepartmentId = 2 (3 сотрудника).

* Отделы со средней зарплатой > 55000: DepartmentId = 1 (60000.00), DepartmentId = 4 (90000.00).

* Отделы с суммарной зарплатой > 150000: DepartmentId = 1 (240000.00), DepartmentId = 2 (171000.00).

### Вывод
Понял ключевое отличие WHERE от HAVING: WHERE фильтрует строки до группировки, а HAVING — после (применяется к результатам агрегатных функций). Научился использовать HAVING с COUNT, AVG и SUM.

## Task 08 — INNER JOIN: сотрудники и отделы
### SQL

```sql
USE PracticeDB;
GO

SELECT 
    e.EmployeeName,
    d.DepartmentName,
    e.Salary
FROM Employees e
INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
ORDER BY d.DepartmentName ASC, e.Salary DESC;
GO
```

### Результат
EmployeeName|DepartmentName|Salary
---|---|---|
Ольга Смирнова|Аналитика|61000.00
Дмитрий Орлов|Аналитика|58000.00
Елена Волкова|Аналитика|52000.00
Алексей Лебедев|Менеджмент|90000.00
Пётр Сидоров|Разработка|72000.00
Анна Иванова|Разработка|65000.00
Мария Кузнецова|Разработка|55000.00
Иван Петров|Разработка|48000.00
Сергей Морозов|Тестирование|45000.00
Наталья Соколова|Тестирование|43000.00

### Вывод
Научился использовать INNER JOIN для соединения двух таблиц по общему ключу (DepartmentId). Понял, что INNER JOIN возвращает только те строки, где есть совпадение в обеих таблицах.

## Task 09 — LEFT JOIN: сотрудники без проектов
### SQL

```sql
USE PracticeDB;
GO

SELECT 
    e.EmployeeId,
    e.EmployeeName
FROM Employees e
LEFT JOIN EmployeeProjects ep ON e.EmployeeId = ep.EmployeeId
WHERE ep.EmployeeId IS NULL;
GO
```

### Результат
EmployeeId|EmployeeName
---|---
9|Наталья Соколова

### Вывод
Разобрался, как работает LEFT JOIN: он возвращает все строки из левой таблицы (Employees), даже если в правой (EmployeeProjects) нет совпадений. В таких случаях правая часть заполняется NULL. Фильтр WHERE ep.EmployeeId IS NULL позволяет найти «сирот» — сотрудников без проектов.

## Task 10 — Множественный JOIN: сотрудники, отделы, проекты
### SQL

```sql
USE PracticeDB;
GO

SELECT 
    e.EmployeeName,
    d.DepartmentName,
    p.ProjectName,
    ep.Hours
FROM Employees e
INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
INNER JOIN EmployeeProjects ep ON e.EmployeeId = ep.EmployeeId
INNER JOIN Projects p ON ep.ProjectId = p.ProjectId
ORDER BY d.DepartmentName ASC, e.EmployeeName ASC, p.ProjectName ASC;
GO
```

### Результат

EmployeeName	|DepartmentName|	ProjectName	|Hours
---|---|---|---
Дмитрий Орлов	|Аналитика	|AI Assistant	|50
Дмитрий Орлов	|Аналитика	|Data Warehouse|	70
Елена Волкова	|Аналитика	|Mobile App	|95
Ольга Смирнова	|Аналитика	|Data Warehouse	|110
Алексей Лебедев	|Менеджмент	|AI Assistant	|200
Алексей Лебедев|	Менеджмент	|CRM System	|30
Анна Иванова	|Разработка	|CRM System	|120
Анна Иванова	|Разработка	|Data Warehouse	|80
Иван Петров|	Разработка	|Legacy Refactor	|40
Иван Петров	|Разработка	|Mobile App	|90
Мария Кузнецова	|Разработка|	Mobile App	|100
Пётр Сидоров	|Разработка	|AI Assistant	|60
Пётр Сидоров	|Разработка	|CRM System	|150
Сергей Морозов	|Тестирование	|Legacy Refactor	|130

### Вывод
Научился соединять более двух таблиц с помощью последовательных INNER JOIN. Понял, как важно правильно указывать связи через ON (по общим ключам). Также убедился, что сортировка по нескольким столбцам (ORDER BY d.DepartmentName, e.EmployeeName, p.ProjectName) позволяет получить аккуратный и читаемый результат.

## Task 11 — Self-join: иерархия руководителей
### SQL

```sql
USE PracticeDB;
GO

SELECT 
    e.EmployeeName,
    m.EmployeeName AS ManagerName
FROM Employees e
LEFT JOIN Employees m ON e.ManagerId = m.EmployeeId
ORDER BY e.EmployeeName;
GO
```

### Результат

EmployeeName	|ManagerName
---|---
Алексей Лебедев|	NULL
Анна Иванова	|NULL
Дмитрий Орлов	|Ольга Смирнова
Елена Волкова	|NULL
Иван Петров	|Анна Иванова
Мария Кузнецова|	Пётр Сидоров
Наталья Соколова	|Алексей Лебедев
Ольга Смирнова	|NULL
Пётр Сидоров	|NULL
Сергей Морозов	|NULL

### Вывод
Научился использовать self-join — соединение таблицы с самой собой. Для этого мы используем два алиаса (e для сотрудника, m для руководителя) и связываем ManagerId с EmployeeId. LEFT JOIN нужен, чтобы сотрудники без руководителя тоже попали в результат (с NULL в поле ManagerName).

## Task 12 — Подзапросы в WHERE
### SQL

```sql
USE PracticeDB;
GO

SELECT EmployeeId, EmployeeName, Salary
FROM Employees
WHERE Salary > (SELECT AVG(Salary) FROM Employees);
GO

SELECT EmployeeId, EmployeeName, Salary
FROM Employees
WHERE Salary = (SELECT MAX(Salary) FROM Employees);
GO
```

### Результат
* Выше средней (средняя = 58900.00):

EmployeeId	|EmployeeName	|Salary
---|---|---
1	|Анна Иванова	|65000.00
2|	Пётр Сидоров	|72000.00
5	|Ольга Смирнова|	61000.00
10	|Алексей Лебедев	|90000.00

* С максимальной зарплатой:

EmployeeId	|EmployeeName	|Salary
---|---|---
10|	Алексей Лебедев	|90000.00

### Вывод
Понял, как работают подзапросы в WHERE: сначала выполняется внутренний запрос (например, находит среднюю или максимальную зарплату), а затем внешний запрос использует это значение для фильтрации. Это позволяет делать динамические сравнения без жестко заданных чисел.

## Task 13 — Скалярные подзапросы в SELECT
### SQL

```sql
USE PracticeDB;
GO

SELECT 
    e.EmployeeName,
    e.Salary,
    e.DepartmentId,
    (SELECT ROUND(AVG(Salary), 2) FROM Employees e2 WHERE e2.DepartmentId = e.DepartmentId) AS DeptAvgSalary,
    ROUND(e.Salary - (SELECT AVG(Salary) FROM Employees e3 WHERE e3.DepartmentId = e.DepartmentId), 2) AS Difference
FROM Employees e
ORDER BY e.DepartmentId, e.EmployeeName;
GO
```

### Результат

EmployeeName	|Salary|	DepartmentId|	DeptAvgSalary|	Difference
---|---|---|---|---
Анна Иванова|65000.00	|1	|60000.000000	|5000.000000
Иван Петров	|48000.00	|1|	60000.000000	|-12000.000000
Мария Кузнецова|	55000.00	|1	|60000.000000	|-5000.000000
Пётр Сидоров	|72000.00	|1	|60000.000000	|12000.000000
Дмитрий Орлов	|58000.00	|2	|57000.000000	|1000.000000
Елена Волкова	|52000.00	|2|	57000.000000|	-5000.000000
Ольга Смирнова	|61000.00	|2|	57000.000000	|4000.000000
Наталья Соколова	|43000.00|	3	|44000.000000|	-1000.000000
Сергей Морозов	|45000.00	|3|	44000.000000|	1000.000000
Алексей Лебедев	|90000.00|	4|	90000.000000|	0.000000

### Вывод
Научился использовать скалярный подзапрос в SELECT. Он выполняется для каждой строки внешнего запроса и возвращает одно значение (в нашем случае — среднюю зарплату по отделу сотрудника). Это позволяет сравнивать значения «на лету» без дополнительных JOIN.

## Task 14 — EXISTS / NOT EXISTS
### SQL

```sql
USE PracticeDB;
GO


SELECT e.EmployeeId, e.EmployeeName
FROM Employees e
WHERE EXISTS (
    SELECT 1 
    FROM EmployeeProjects ep 
    WHERE ep.EmployeeId = e.EmployeeId
);
GO


SELECT e.EmployeeId, e.EmployeeName
FROM Employees e
WHERE NOT EXISTS (
    SELECT 1 
    FROM EmployeeProjects ep 
    WHERE ep.EmployeeId = e.EmployeeId
);
GO
```

### Результат
* Сотрудники, у которых есть хотя бы один проект

EmployeeId|	EmployeeName
---|---
1	|Анна Иванова
2	|Пётр Сидоров
3	|Мария Кузнецова
4	|Иван Петров
5	|Ольга Смирнова
6	|Дмитрий Орлов
7	|Елена Волкова
8|	Сергей Морозов
10	|Алексей Лебедев

* Сотрудники, у которых нет проектов

EmployeeId	|EmployeeName
---|---
9	|Наталья Соколова

### Вывод
Разобрался с EXISTS и NOT EXISTS. В отличие от JOIN, они не дублируют строки и не выполняют соединение таблиц, а просто проверяют, возвращает ли подзапрос хотя бы одну строку. Результаты совпадают с заданиями 8 и 9, что подтверждает правильность логики.

## Task 15 — UNION / UNION ALL
### SQL

```sql
USE PracticeDB;
GO

-- UNION
SELECT EmployeeId, EmployeeName, Salary, HireDate
FROM Employees
WHERE Salary > 60000

UNION

SELECT EmployeeId, EmployeeName, Salary, HireDate
FROM Employees
WHERE YEAR(HireDate) < 2022;
GO

-- UNION ALL 
SELECT EmployeeId, EmployeeName, Salary, HireDate
FROM Employees
WHERE Salary > 60000

UNION ALL

SELECT EmployeeId, EmployeeName, Salary, HireDate
FROM Employees
WHERE YEAR(HireDate) < 2022;
GO

-- Сравнение
SELECT 'UNION' AS QueryType, COUNT(*) AS RovCount FROM (
    SELECT EmployeeId FROM Employees WHERE Salary > 60000
    UNION
    SELECT EmployeeId FROM Employees WHERE YEAR(HireDate) < 2022
) AS t1
UNION ALL
SELECT 'UNION ALL', COUNT(*) FROM (
    SELECT EmployeeId FROM Employees WHERE Salary > 60000
    UNION ALL
    SELECT EmployeeId FROM Employees WHERE YEAR(HireDate) < 2022
) AS t2;
GO
```
### Результат

QueryType	|RovCount
---|---
UNION	|6
UNION ALL	|10

### Вывод
Понял разницу: UNION удаляет дубликаты (медленнее), а UNION ALL оставляет все строки (быстрее). Важно, чтобы количество и типы столбцов в обоих запросах совпадали.

## Task 16 — UPDATE: изменение данных
### SQL

```sql
USE PracticeDB;
GO

-- Увеличение зарплаты сотрудников отдела 1 на 10%
SELECT EmployeeId, EmployeeName, Salary FROM Employees WHERE DepartmentId = 1;
UPDATE Employees SET Salary = Salary * 1.10 WHERE DepartmentId = 1;
SELECT EmployeeId, EmployeeName, Salary FROM Employees WHERE DepartmentId = 1;
GO

-- Увеличение зарплаты сотрудников, нанятых до 2022 года, на 5000
SELECT EmployeeId, EmployeeName, Salary, HireDate FROM Employees WHERE YEAR(HireDate) < 2022;
UPDATE Employees SET Salary = Salary + 5000 WHERE YEAR(HireDate) < 2022;
SELECT EmployeeId, EmployeeName, Salary, HireDate FROM Employees WHERE YEAR(HireDate) < 2022;
GO

-- Обновление HireDate одного сотрудника
SELECT EmployeeId, EmployeeName, HireDate FROM Employees WHERE EmployeeId = 4;
UPDATE Employees SET HireDate = '2023-03-01' WHERE EmployeeId = 4;
SELECT EmployeeId, EmployeeName, HireDate FROM Employees WHERE EmployeeId = 4;
GO
```

### Результат

* 1 До:

EmployeeId	|EmployeeName|	Salary
---|---|---
1	|Анна Иванова	|65000.00
2	|Пётр Сидоров	|72000.00
3	|Мария Кузнецова	|55000.00
4	|Иван Петров	|48000.00

* 1 После:

EmployeeId	|EmployeeName	|Salary
---|---|---
1	|Анна Иванова	|71500.00
2	|Пётр Сидоров	|79200.00
3	|Мария Кузнецова	|60500.00
4	|Иван Петров	|52800.00

* 2 До:

EmployeeId	|EmployeeName	|Salary	|HireDate
---|---|---|---
1	|Анна Иванова	|71500.00	|2020-03-15
2	|Пётр Сидоров	|79200.00	|2019-07-01
3	|Мария Кузнецова	|60500.00	|2021-11-20
5	|Ольга Смирнова	|61000.00	|2020-09-05
8	|Сергей Морозов	|45000.00	|2021-12-12
10	|Алексей Лебедев	|90000.00	|2018-05-25

* 2 После:

EmployeeId	|EmployeeName	|Salary	|HireDate
---|---|---|---
1	|Анна Иванова	|76500.00	|2020-03-15
2	|Пётр Сидоров	|84200.00	|2019-07-01
3	|Мария Кузнецова|	65500.00	|2021-11-20
5	|Ольга Смирнова	|66000.00	|2020-09-05
8	|Сергей Морозов|	50000.00	|2021-12-12
10	|Алексей Лебедев	|95000.00	|2018-05-25

* 3 До:

EmployeeId	|EmployeeName	|HireDate
---|---|---
4	|Иван Петров	|2023-02-10

* 3 После:

EmployeeId	|EmployeeName	|HireDate
---|---|---
4	|Иван Петров	|2023-03-01

### Вывод
Научился использовать UPDATE с условием WHERE. Понял, что важно сначала делать SELECT, чтобы не обновить лишние строки. Арифметические операции (* 1.10, + 5000) работают прямо в SET.

## Task 17 — DELETE: удаление данных
### SQL

```sql
USE PracticeDB;
GO

-- Удалить назначения на проект с Hours = 0
SELECT COUNT(*) AS BeforeDelete FROM EmployeeProjects WHERE Hours = 0;
DELETE FROM EmployeeProjects WHERE Hours = 0;
SELECT COUNT(*) AS AfterDelete FROM EmployeeProjects WHERE Hours = 0;
GO

-- Удалить сотрудника по EmployeeId (ID = 4)
SELECT * FROM EmployeeProjects WHERE EmployeeId = 4;
SELECT * FROM Employees WHERE EmployeeId = 4;

DELETE FROM EmployeeProjects WHERE EmployeeId = 4;
DELETE FROM Employees WHERE EmployeeId = 4;

SELECT COUNT(*) AS EmployeeExists FROM Employees WHERE EmployeeId = 4;
GO

-- DELETE без WHERE
CREATE TABLE TestDelete (Id INT PRIMARY KEY IDENTITY(1,1), Name NVARCHAR(50));
INSERT INTO TestDelete (Name) VALUES ('Тест 1'), ('Тест 2'), ('Тест 3');
SELECT COUNT(*) AS BeforeTruncate FROM TestDelete;
DELETE FROM TestDelete;
SELECT COUNT(*) AS AfterTruncate FROM TestDelete;
DROP TABLE TestDelete;
GO
```

### Результат
* До удаления (Hours = 0): 0 строк (так как у нас в данных нет 0 часов). После: 0 строк.
* Иван Петров (ID 4) удален вместе со своими назначениями. SELECT COUNT(*) вернул 0.
* До: 3 строки, после DELETE без WHERE: 0 строк.

### Вывод
Научился удалять данные с учетом внешних ключей: сначала удаляем зависимые записи (из EmployeeProjects), потом основную (из Employees). Понял, что DELETE без WHERE удаляет все строки из таблицы (в отличие от TRUNCATE, который быстрее и сбрасывает IDENTITY).

## Task 18 — Транзакции: BEGIN TRAN / COMMIT / ROLLBACK
### SQL

```sql
USE PracticeDB;
GO

-- ROLLBACK
BEGIN TRANSACTION;
SELECT EmployeeId, EmployeeName, Salary FROM Employees WHERE EmployeeId = 1;
UPDATE Employees SET Salary = Salary + 10000 WHERE EmployeeId = 1;
SELECT EmployeeId, EmployeeName, Salary FROM Employees WHERE EmployeeId = 1;
ROLLBACK TRANSACTION;
SELECT EmployeeId, EmployeeName, Salary FROM Employees WHERE EmployeeId = 1;
GO

-- COMMIT
BEGIN TRANSACTION;
SELECT EmployeeId, EmployeeName, Salary FROM Employees WHERE EmployeeId = 1;
UPDATE Employees SET Salary = Salary + 10000 WHERE EmployeeId = 1;
SELECT EmployeeId, EmployeeName, Salary FROM Employees WHERE EmployeeId = 1;
COMMIT TRANSACTION;
SELECT EmployeeId, EmployeeName, Salary FROM Employees WHERE EmployeeId = 1;
GO
```

### Результат
* Тест 1 (ROLLBACK):
  * До: 65000.00
  * После UPDATE: 75000.00
  * После ROLLBACK: 65000.00 (изменение откатилось)

* Тест 2 (COMMIT):
  * До: 65000.00
  * После UPDATE: 75000.00
  * После COMMIT: 75000.00 (изменение сохранилось)

### Вывод
Понял, что такое транзакция: это набор операций, который выполняется как единое целое. ROLLBACK отменяет все изменения, сделанные в транзакции (как будто их не было). COMMIT сохраняет их навсегда. Это критически важно для финансовых систем, где ошибка может стоить денег.

## Task 19 — Представления (VIEW)
### SQL

```sql
USE PracticeDB;
GO

CREATE VIEW vw_EmployeeDetails AS
SELECT 
    e.EmployeeName,
    d.DepartmentName,
    p.ProjectName,
    ep.Hours
FROM Employees e
INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
INNER JOIN EmployeeProjects ep ON e.EmployeeId = ep.EmployeeId
INNER JOIN Projects p ON ep.ProjectId = p.ProjectId;
GO

SELECT * FROM vw_EmployeeDetails;
GO

DROP VIEW vw_EmployeeDetails;
GO
```

### Результат
**vw_EmployeeDetails** вернуло таблицу с колонками: *EmployeeName*, *DepartmentName*, *ProjectName*, *Hours* (те же данные, что в задании 10). После **DROP VIEW** представление удалено.

### Вывод
Представление — это виртуальная таблица, которая не хранит данные, а каждый раз выполняет запрос. Это удобно для инкапсуляции сложных JOIN и разграничения прав доступа (можно дать пользователю доступ к VIEW, а не к исходным таблицам).

## Task 20 — Индексы: создание и анализ
### SQL

```sql
USE PracticeDB;
GO

SET STATISTICS IO ON;
GO

-- ДО индексов
SELECT EmployeeId, EmployeeName, Salary, DepartmentId
FROM Employees
WHERE DepartmentId = 1 AND Salary > 50000;
GO

-- Создаем индексы
CREATE INDEX IX_Employees_DepartmentId ON Employees(DepartmentId);
CREATE INDEX IX_Employees_Salary ON Employees(Salary);
GO

-- ПОСЛЕ индексов
SELECT EmployeeId, EmployeeName, Salary, DepartmentId
FROM Employees
WHERE DepartmentId = 1 AND Salary > 50000;
GO

SET STATISTICS IO OFF;
GO
```

### Результат 

![alt text](image.png)

### Вывод
Индекс — это структура данных (обычно B-дерево), которая ускоряет поиск. Без индекса SQL Server читает всю таблицу (Table Scan), а с индексом — сразу находит нужные строки (Index Seek). Важно не создавать слишком много индексов, так как они замедляют INSERT/UPDATE/DELETE.

На таблице из 10 строк разница в logical reads минимальна (или отсутствует), так как вся таблица помещается на одну страницу. Однако на больших объемах данных индекс IX_Employees_DepartmentId позволил бы избежать Table Scan и сократить количество чтений.

## Task 21 — Хранимые процедуры
### SQL

```sql
USE PracticeDB;
GO

CREATE PROCEDURE sp_GetEmployeesByDepartment
    @DeptId INT
AS
BEGIN
    SELECT EmployeeId, EmployeeName, Salary
    FROM Employees
    WHERE DepartmentId = @DeptId;
END;
GO

EXEC sp_GetEmployeesByDepartment @DeptId = 1;
GO

CREATE PROCEDURE sp_GetAvgSalaryByDepartment
    @DeptId INT,
    @AvgSalary DECIMAL(10,2) OUTPUT
AS
BEGIN
    SELECT @AvgSalary = AVG(Salary)
    FROM Employees
    WHERE DepartmentId = @DeptId;
END;
GO

DECLARE @Avg DECIMAL(10,2);
EXEC sp_GetAvgSalaryByDepartment @DeptId = 1, @AvgSalary = @Avg OUTPUT;
SELECT @Avg AS AvgSalary;
GO
```
### Результат
sp_GetEmployeesByDepartment (DeptId = 1): 4 сотрудника (Анна, Пётр, Мария, Иван).
sp_GetAvgSalaryByDepartment (DeptId = 1): 60000.00 (средняя зарплата).

### Вывод
Хранимые процедуры позволяют сохранить SQL-код на сервере и вызывать его по имени. Это удобно для повторяющихся операций. OUTPUT-параметры позволяют возвращать значения (например, агрегаты) в вызывающую среду.

## Task 22 — Функции: скалярная и табличная
### SQL

```sql
USE PracticeDB;
GO

CREATE FUNCTION fn_GetAnnualSalary
(
    @MonthlySalary DECIMAL(10,2)
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @MonthlySalary * 12;
END;
GO

SELECT 
    EmployeeName,
    Salary AS MonthlySalary,
    dbo.fn_GetAnnualSalary(Salary) AS AnnualSalary
FROM Employees;
GO

CREATE FUNCTION fn_GetEmployeesBySalaryRange
(
    @Min DECIMAL(10,2),
    @Max DECIMAL(10,2)
)
RETURNS TABLE
AS
RETURN
(
    SELECT EmployeeId, EmployeeName, Salary
    FROM Employees
    WHERE Salary BETWEEN @Min AND @Max
);
GO

SELECT * FROM dbo.fn_GetEmployeesBySalaryRange(50000, 70000);
GO
```

### Результат
* Скалярная

EmployeeName	|MonthlySalary|	AnnualSalary
---|---|---
Анна Иванова	|65000.00	|780000.00
Пётр Сидоров	|72000.00	|864000.00
Мария Кузнецова	|55000.00	|660000.00
Иван Петров	|48000.00	|576000.00
Ольга Смирнова	|61000.00	|732000.00
Дмитрий Орлов	|58000.00	|696000.00
Елена Волкова	|52000.00	|624000.00
Сергей Морозов	|45000.00	|540000.00
Наталья Соколова	|43000.00	|516000.00
Алексей Лебедев	|90000.00	|1080000.00
* Табличная

EmployeeId	|EmployeeName	|Salary
---|---|---
1	|Анна Иванова	|65000.00
3	|Мария Кузнецова	|55000.00
5	|Ольга Смирнова|	61000.00
6	|Дмитрий Орлов	|58000.00
7|	Елена Волкова	|52000.00

### Вывод
Скалярная функция возвращает одно значение и может использоваться в SELECT. Табличная функция возвращает таблицу и используется в FROM. В отличие от процедур, функции можно вызывать прямо в запросах.

## Task 23 — Оконные функции: ROW_NUMBER, RANK, DENSE_RANK
### SQL

```sql
USE PracticeDB;
GO

SELECT 
    EmployeeName,
    DepartmentId,
    Salary,
    ROW_NUMBER() OVER (ORDER BY Salary DESC) AS RowNumber,
    RANK() OVER (PARTITION BY DepartmentId ORDER BY Salary DESC) AS RankInDept,
    DENSE_RANK() OVER (PARTITION BY DepartmentId ORDER BY Salary DESC) AS DenseRankInDept
FROM Employees
ORDER BY DepartmentId, Salary DESC;
GO
```
### Результат

EmployeeName	|DepartmentId	|Salary	|RowNumber	|RankInDept|	DenseRankInDept
---|---|---|---|---|---
Пётр Сидоров|	1|	72000.00|	2	|1|	1
Анна Иванова|	1	|65000.00|	3|	2|	2
Мария Кузнецова	|1	|55000.00|	6|	3|	3
Иван Петров	|1	|48000.00|	8|	4|	4|
Ольга Смирнова|	2|	61000.00	|4|	1|	1
Дмитрий Орлов|	2|	58000.00|	5|	2|	2
Елена Волкова|	2|	52000.00|	7|	3|	3
Сергей Морозов	|3|	45000.00|	9|	1	|1
Наталья Соколова|	3|	43000.00|	10|	2|	2
Алексей Лебедев	|4|	90000.00	|1|	1|	1

## Task 24 — Оконные агрегаты: SUM OVER, AVG OVER
### SQL

```sql
USE PracticeDB;
GO

SELECT 
    EmployeeName,
    DepartmentId,
    Salary,
    SUM(Salary) OVER (PARTITION BY DepartmentId) AS TotalDeptSalary,
    ROUND(AVG(Salary) OVER (PARTITION BY DepartmentId), 2) AS AvgDeptSalary,
    ROUND(Salary - AVG(Salary) OVER (PARTITION BY DepartmentId), 2) AS DifferenceFromAvg
FROM Employees
ORDER BY DepartmentId, Salary DESC;
GO
```
### Результат

EmployeeName	|DepartmentId	|Salary	|TotalDeptSalary	|AvgDeptSalary	DifferenceFromAvg
---|---|---|---|---
Пётр Сидоров	|1	|72000.00	|240000.00	|60000.000000	|12000.000000
Анна Иванова	|1	|65000.00	|240000.00	|60000.000000	|5000.000000
Мария Кузнецова	|1	|55000.00	|240000.00|	60000.000000	|-5000.000000
Иван Петров	|1	|48000.00	|240000.00	|60000.000000	|-12000.000000
Ольга Смирнова	|2	|61000.00	|171000.00	|57000.000000	|4000.000000
Дмитрий Орлов	|2	|58000.00	|171000.00	|57000.000000	|1000.000000
Елена Волкова	|2	|52000.00	|171000.00|	57000.000000	|-5000.000000
Сергей Морозов	|3	|45000.00	|88000.00	|44000.000000	|1000.000000
Наталья Соколова|	3|	43000.00|	88000.00|	44000.000000	|-1000.000000
Алексей Лебедев	|4	|90000.00	|90000.00	90000.000000	|0.000000

### Вывод
Понял, как работают оконные агрегаты (SUM ... OVER, AVG ... OVER). Главное отличие от GROUP BY в том, что они не сворачивают строки. Это позволяет видеть и индивидуальные значения (зарплату), и агрегаты по группе (сумму по отделу) в одной строке.

## Task 25 — CTE (Common Table Expressions)
### SQL

```sql
USE PracticeDB;
GO

-- 1. Переписываем запрос 12 через CTE
WITH AvgSalary AS (
    SELECT AVG(Salary) AS AvgSal FROM Employees
)
SELECT e.EmployeeId, e.EmployeeName, e.Salary
FROM Employees e, AvgSalary a
WHERE e.Salary > a.AvgSal;
GO

-- 2. CTE для топ-3 отделов по средней зарплате
WITH DeptAvg AS (
    SELECT 
        DepartmentId, 
        ROUND(AVG(Salary), 2) AS AvgSalary
    FROM Employees
    GROUP BY DepartmentId
)
SELECT TOP 3 DepartmentId, AvgSalary
FROM DeptAvg
ORDER BY AvgSalary DESC;
GO
```
### Результат
1. Сотрудники выше средней: (те же, что в задании 12: Анна, Пётр, Ольга, Алексей).
2. Топ-3 отдела по средней зарплате: Отдел 4 (90000.00), Отдел 1 (60000.00), Отдел 2 (57000.00).

### Вывод
CTE (WITH ... AS ...) делает запрос более читаемым, так как подзапрос выносится в начало и получает имя. Это особенно удобно, когда один и тот же подзапрос используется несколько раз или когда запрос становится очень сложным.

## Task 26 — CASE: условная логика в SELECT
### SQL

```sql
USE PracticeDB;
GO

SELECT 
    EmployeeName,
    Salary,
    CASE 
        WHEN Salary > 60000 THEN 'High'
        WHEN Salary >= 40000 AND Salary <= 60000 THEN 'Medium'
        ELSE 'Low'
    END AS SalaryLevel,
    HireDate,
    CASE 
        WHEN YEAR(HireDate) < 2020 THEN 'Senior'
        WHEN YEAR(HireDate) >= 2020 AND YEAR(HireDate) <= 2022 THEN 'Mid'
        ELSE 'Junior'
    END AS Tenure
FROM Employees
ORDER BY Salary DESC;
GO
```

### Результат

EmployeeName	|Salary	|SalaryLevel|	HireDate	|Tenure
---|---|---|---|---
Алексей Лебедев	|90000.00	|High	|2018-05-25|	Senior
Пётр Сидоров|	72000.00	|High	|2019-07-01	|Senior
Анна Иванова	|65000.00	|High	|2020-03-15|	Mid
Ольга Смирнова|	61000.00	|High	|2020-09-05|	Mid
Дмитрий Орлов	|58000.00|	Medium|	2022-04-18	|Mid
Мария Кузнецова	|55000.00	|Medium	|2021-11-20|	Mid
Елена Волкова	|52000.00	|Medium	|2023-06-01	|Junior
Иван Петров	|48000.00	|Medium	|2023-02-10	|Junior
Сергей Морозов	|45000.00	|Medium	|2021-12-12	|Mid
Наталья Соколова	|43000.00	|Medium	|2024-01-15	|Junior

### Вывод
CASE — это мощный инструмент для создания вычисляемых колонок. В отличие от IIF, он поддерживает несколько условий (WHEN ... THEN ... WHEN ... THEN ... ELSE ...), что позволяет строить сложные категории.

## Task 27 — Работа с датами: DATEADD, DATEDIFF, FORMAT
### SQL

```sql
USE PracticeDB;
GO

SELECT 
    EmployeeName,
    HireDate,
    DATEDIFF(YEAR, HireDate, GETDATE()) AS YearsOfService,
    DATEDIFF(MONTH, HireDate, GETDATE()) AS MonthsOfService,
    DATEADD(YEAR, 1, HireDate) AS FirstAnniversary,
    FORMAT(HireDate, 'dd.MM.yyyy') AS FormattedHireDate
FROM Employees
ORDER BY HireDate;
GO
```

### Результат

EmployeeName	|HireDate	|YearsOfService|	MonthsOfService	|FirstAnniversary	|FormattedHireDate
---|---|---|---|---|---
Алексей Лебедев	|2018-05-25	|8	|102|	2019-05-25	|25.05.2018
Пётр Сидоров	|2019-07-01	|7	|88	|2020-07-01|	01.07.2019
Анна Иванова	|2020-03-15	|6	|80|	2021-03-15	|15.03.2020
Ольга Смирнова	|2020-09-05	|6	|74	|2021-09-05	|05.09.2020
Мария Кузнецова	|2021-11-20	|5	|60|	2022-11-20	|20.11.2021
Сергей Морозов	|2021-12-12	|5	|59|	2022-12-12	|12.12.2021
Дмитрий Орлов	|2022-04-18	|4	|55|	2023-04-18	|18.04.2022
Иван Петров|	2023-02-10	|3	|45|	2024-02-10|	10.02.2023
Елена Волкова	|2023-06-01|	|3	41|	2024-06-01	|01.06.2023
Наталья Соколова	|2024-01-15	|2	|34|	2025-01-15|	15.01.2024

### Вывод
Изучил функции для работы с датами:

* DATEDIFF считает разницу между датами в указанных единицах (годы, месяцы, дни).
*D ATEADD добавляет указанный интервал к дате.
* FORMAT позволяет вывести дату в нужном строковом формате.

Это часто используется для расчета стажа, возраста и сроков.

## Task 28 — C# + SQL: подключение и SELECT
### C#

```csharp
using Main.Common;
using Microsoft.Data.SqlClient;

namespace Main.Database;

[TaskInfo(28, "C# + SQL: подключение и SELECT",
    "Подключить консольное приложение к PracticeDB и выполнить запрос.",
    "NuGet-пакет Microsoft.Data.SqlClient",
    "Строка подключения — из переменной окружения PRACTICE_DB_CONNECTION",
    "Асинхронное открытие соединения (OpenAsync)",
    "Запрос SELECT EmployeeName, Salary FROM Employees",
    "Вывод через SqlDataReader",
    "Обработка SqlException")]
public class Task28_SqlSelect : ITask
{
    public void Run()
    {
        
        string connectionString = Environment.GetEnvironmentVariable("PRACTICE_DB_CONNECTION")
            ?? @"Server=localhost\SQLEXPRESS02;Database=PracticeDB;Trusted_Connection=True;TrustServerCertificate=True;";

        string query = "SELECT EmployeeName, Salary FROM Employees";

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("Сотрудники в базе данных:");
                    Console.WriteLine("-------------------------");
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        decimal salary = reader.GetDecimal(1);
                        Console.WriteLine($"{name,-20} | {salary,10:F2}");
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Ошибка SQL: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Общая ошибка: {ex.Message}");
        }
    }
}
```
### Резултат
Консольное приложение вывело список сотрудников с их зарплатами.
### Вывод
Научился подключаться к SQL Server из C# с помощью Microsoft.Data.SqlClient. Использовал SqlConnection, SqlCommand и SqlDataReader. Строку подключения взял из переменной окружения (с фолбэком для локального теста). Обработал возможные SqlException.
## Task 29 — C# + SQL: параметризованный INSERT
### C#

```csharp
using Main.Common;
using Microsoft.Data.SqlClient;

namespace Main.Database;

[TaskInfo(29, "C# + SQL: параметризованный INSERT",
    "Добавить сотрудника из C#-приложения.",
    "Метод AddEmployee(string name, decimal salary, int departmentId)",
    "Параметризованный запрос (@name, @salary, @deptId) — без конкатенации (защита от SQL-инъекций)",
    "Выполнить INSERT INTO Employees ...",
    "После вставки — SELECT для подтверждения",
    "Обработать ошибки (FK violation, дубликат)")]
public class Task29_SqlInsert : ITask
{
    private readonly string _connectionString = Environment.GetEnvironmentVariable("PRACTICE_DB_CONNECTION")
        ?? @"Server=localhost\SQLEXPRESS02;Database=PracticeDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public void Run()
    {
        
        AddEmployee("Тестовый Сотрудник", 50000m, 1);

       
        ShowEmployee("Тестовый Сотрудник");
    }

    private void AddEmployee(string name, decimal salary, int departmentId)
    {
        
        string query = @"INSERT INTO Employees (EmployeeName, Salary, HireDate, DepartmentId, ManagerId) 
                         VALUES (@name, @salary, GETDATE(), @deptId, NULL)";

        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                   
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@salary", salary);
                    command.Parameters.AddWithValue("@deptId", departmentId);

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"Добавлено строк: {rowsAffected}");
                }
            }
        }
        catch (SqlException ex)
        {
          
            Console.WriteLine($"Ошибка SQL: {ex.Message}");
            if (ex.Number == 547) 
                Console.WriteLine("Ошибка: такого отдела не существует.");
        }
    }

    private void ShowEmployee(string name)
    {
        string query = "SELECT EmployeeId, EmployeeName, Salary, DepartmentId FROM Employees WHERE EmployeeName = @name";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader.GetInt32(0)}, Имя: {reader.GetString(1)}, Зарплата: {reader.GetDecimal(2)}, Отдел: {reader.GetInt32(3)}");
                    }
                }
            }
        }
    }
}
```
### Результат
Сотрудник "Тестовый Сотрудник" успешно добавлен в базу.

### Вывод
Понял, что параметризованные запросы — это обязательное правило безопасности. Они предотвращают SQL-инъекции и автоматически экранируют специальные символы. Использовал ExecuteNonQuery для INSERT и SqlDataReader для проверки.
## Task 30 — C# + SQL: мини-отчёт по отделам
### C#

```csharp
using Main.Common;
using Microsoft.Data.SqlClient;

namespace Main.Database;

[TaskInfo(30, "C# + SQL: мини-отчёт по отделам",
    "Вывести отчёт по отделам из C#-приложения.",
    "Запрос с LEFT JOIN, COUNT, AVG и GROUP BY",
    "Выполнить через SqlCommand.ExecuteReader",
    "Вывести в консоль таблицей с выравниванием",
    "Для NULL (отдел без сотрудников) — 0 или N/A",
    "Обработать пустой результат")]
public class Task30_SqlReport : ITask
{
    private readonly string _connectionString = Environment.GetEnvironmentVariable("PRACTICE_DB_CONNECTION")
        ?? @"Server=localhost\SQLEXPRESS02;Database=PracticeDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public void Run()
    {
        string query = @"
            SELECT 
                d.DepartmentName, 
                COUNT(e.EmployeeId) AS EmpCount, 
                AVG(e.Salary) AS AvgSalary 
            FROM Departments d 
            LEFT JOIN Employees e ON d.DepartmentId = e.DepartmentId 
            GROUP BY d.DepartmentName
            ORDER BY d.DepartmentName";

        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    
                    Console.WriteLine($"{"Отдел",-15} | {"Сотрудников",12} | {"Средняя ЗП",12}");
                    Console.WriteLine(new string('-', 45));

                    bool hasRows = false;
                    while (reader.Read())
                    {
                        hasRows = true;
                        string deptName = reader.GetString(0);

                       
                        int empCount = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);

                        
                        string avgSalary = reader.IsDBNull(2)
                            ? "N/A"
                            : reader.GetDecimal(2).ToString("F2");

                        Console.WriteLine($"{deptName,-15} | {empCount,12} | {avgSalary,12}");
                    }

                    if (!hasRows)
                    {
                        Console.WriteLine("Нет данных для отображения.");
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Ошибка SQL: {ex.Message}");
        }
    }
}
```
### Результат
![alt text](image-1.png)
### Вывод
Научился выполнять сложные аналитические запросы (LEFT JOIN, GROUP BY, агрегаты) из C# и форматировать вывод в виде таблицы. Обработал случай NULL (через IsDBNull), чтобы программа не падала, если в отделе нет сотрудников.