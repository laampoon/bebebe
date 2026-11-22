using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace практика
{
    public class SQL
    {
        public void Swith(int i)
        {
            switch (i)
            {
                case 28: sql28(); break;
                case 29: sql29(); break;
                case 30: sql30(); break;
              
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }

        private readonly string _connectionString =
    Environment.GetEnvironmentVariable("PRACTICE_DB_CONNECTION")
    ?? Environment.GetEnvironmentVariable("PRACTICE_DB_CONNECTION", EnvironmentVariableTarget.User)
    ?? @"Server=localhost\SQLEXPRESS02;Database=PracticeDB;Trusted_Connection=True;TrustServerCertificate=True;";




        public void sql28()
        {
            Console.Clear();
            Console.WriteLine("Задание 28. C# + SQL: подключение и SELECT");
            Console.WriteLine("Что сделать: подключить консольное приложение к PracticeDB и выполнить запрос");
            Console.WriteLine("Требования:" + Environment.NewLine +
              "NuGet-пакет Microsoft.Data.SqlClient.Строка подключения — из переменной окружения PRACTICE_DB_CONNECTION" + Environment.NewLine +
              "Асинхронное открытие соединения (OpenAsync).Запрос SELECT EmployeeName, Salary FROM Employees." + Environment.NewLine +
              "Вывод через SqlDataReader.Обработка SqlException." + Environment.NewLine);


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

        public void sql29()
        {
            Console.Clear();
            Console.WriteLine("Задание 29. C# + SQL: параметризованный INSERT");
            Console.WriteLine("Что сделать: добавить сотрудника из C#-приложения.");
            Console.WriteLine("Требования:" + Environment.NewLine +
              "Метод AddEmployee(string name, decimal salary, int departmentId).Параметризованный запрос (@name, @salary, @deptId) — без конкатенации (защита от SQL-инъекций)." + Environment.NewLine +
              "Выполнить INSERT INTO Employees ....После вставки — SELECT для подтверждения." + Environment.NewLine +
              "Обработать ошибки (FK violation, дубликат)." + Environment.NewLine);

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

        public void sql30()
        {
            Console.Clear();
            Console.WriteLine("Задание 30. C# + SQL: мини-отчёт по отделам");
            Console.WriteLine("Что сделать: вывести отчёт по отделам из C#-приложения.");
            Console.WriteLine("Требования:" + Environment.NewLine +
              "Запрос SELECT d.DepartmentName, COUNT(e.EmployeeId) AS EmpCount, AVG(e.Salary) AS AvgSalary FROM Departments d LEFT JOIN Employees e ON d.DepartmentId = e.DepartmentId GROUP BY d.DepartmentName." + Environment.NewLine +
              "Выполнить через SqlCommand.ExecuteReader.Вывести в консоль таблицей с выравниванием." + Environment.NewLine +
              "Для NULL (отдел без сотрудников) — 0 или N/A.Обработать пустой результат." + Environment.NewLine);


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
}
   

