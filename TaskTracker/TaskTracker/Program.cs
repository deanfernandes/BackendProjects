using System;
using System.Linq;
using System.Reflection;
using TaskTracker.Models;

namespace TaskTracker
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\nTask Tracker");
                Console.WriteLine("1. View all tasks");
                Console.WriteLine("2. View incomplete tasks");
                Console.WriteLine("3. View completed tasks");
                Console.WriteLine("4. Add new task");
                Console.WriteLine("5. Update task");
                Console.WriteLine("6. Delete task");
                Console.WriteLine("7. Exit");
                Console.Write("Choose an option: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1": ViewAllTasks(); break;
                    case "2": ReadTasksByCompletion(false); break;
                    case "3": ReadTasksByCompletion(true); break;
                    case "4": AddTask(); break;
                    case "5": UpdateTask();  break;
                    case "6": DeleteTask(); break;
                    case "7": return;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        static void ViewAllTasks()
        {
            using var db = new AppDbContext();
            var tasks = db.Tasks.OrderBy(t => t.Id).ToList();

            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
                return;
            }

            foreach (var task in tasks)
            {
                Console.WriteLine($"[{task.Id}] {task.Title} - {(task.IsCompleted ? "✓ Completed" : "⨯ Incomplete")}");
            }
        }

        static void AddTask()
        {
            Console.Write("Enter task title: ");
            string title = Console.ReadLine();

            Console.Write("Enter task description: ");
            string description = Console.ReadLine();

            var task = new TaskItem
            {
                Title = title,
                Description = description,
                IsCompleted = false
            };

            using var db = new AppDbContext();
            db.Tasks.Add(task);
            db.SaveChanges();

            Console.WriteLine("Task added successfully.");
        }

        static void UpdateTask()
        {
            Console.Write("Enter the ID of the task to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            using var db = new AppDbContext();
            var task = db.Tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                Console.WriteLine("Task not found.");
                return;
            }

            Console.Write($"New title (leave empty to keep '{task.Title}'): ");
            string newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
                task.Title = newTitle;

            Console.Write($"New description (leave empty to keep current): ");
            string newDesc = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newDesc))
                task.Description = newDesc;

            Console.Write($"Mark as completed? (y/n): ");
            string completeInput = Console.ReadLine();
            if (completeInput.Trim().ToLower() == "y")
                task.IsCompleted = true;

            db.SaveChanges();
            Console.WriteLine("Task updated.");
        }

        static void DeleteTask()
        {
            Console.Write("Enter the ID of the task to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            using var db = new AppDbContext();
            var task = db.Tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                Console.WriteLine("Task not found.");
                return;
            }

            Console.Write($"Are you sure you want to delete '{task.Title}'? (y/n): ");
            string confirm = Console.ReadLine();
            if (confirm.Trim().ToLower() != "y")
            {
                Console.WriteLine("Deletion cancelled.");
                return;
            }

            db.Tasks.Remove(task);
            db.SaveChanges();

            Console.WriteLine("Task deleted.");
        }

        static void ReadTasksByCompletion(bool completed)
        {
            using var db = new AppDbContext();
            var tasks = db.Tasks.Where(t => t.IsCompleted == completed).ToList();

            if (!tasks.Any())
            {
                Console.WriteLine(completed ? "No completed tasks." : "No incomplete tasks.");
                return;
            }

            foreach (var task in tasks)
            {
                Console.WriteLine($"Id: {task.Id}, Title: {task.Title}");
            }
        }
    }

}
