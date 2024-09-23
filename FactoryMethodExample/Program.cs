using FactoryMethodClassLibrary;

namespace FactoryMethodExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "info.txt";
            List<Entity> entities = new();
            LoadFromFile(filePath, entities);
            while (true)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Добавить учителя");
                Console.WriteLine("3. Добавить курс");
                Console.WriteLine("4. Добавить студента в курс");
                Console.WriteLine("5. Добавить учителя в курс");
                Console.WriteLine("6. Сохранить данные");
                Console.WriteLine("7. Показать связанные объекты");
                Console.WriteLine("8. Выход");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddStudent(entities);
                        break;
                    case "2":
                        AddTeacher(entities);
                        break;
                    case "3":
                        AddCourse(entities);
                        break;
                    case "4":
                        AddStudentToCourse(entities);
                        break;
                    case "5":
                        AddTeacherToCourse(entities);
                        break;
                    case "6":
                        SaveToFile(filePath, entities);
                        Console.WriteLine("Данные сохранены.");
                        break;
                    case "7":
                        ShowRelatedEntities(entities);
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Пожалуйста попробуйте снова.");
                        break;
                }
            }  
        }

        static void LoadFromFile(string filePath, List<Entity> entities)
        {
            Dictionary<int, Teacher> teachers = new();
            Dictionary<int, Student> students = new();
            Dictionary<int, Course> courses = new();

            // Читаем файл построчно
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');

                switch (parts[0])
                {
                    case "Teacher":
                        // Создаем учителя
                        int teacherId = int.Parse(parts[1]);
                        string teacherName = parts[2];
                        int experience = int.Parse(parts[3]);
                        var teacher = new Teacher(teacherId, teacherName, experience);
                        teachers[teacherId] = teacher;
                        entities.Add(teacher); // Добавляем учителя в список сущностей
                        break;

                    case "Student":
                        // Создаем студента
                        int studentId = int.Parse(parts[1]);
                        string studentName = parts[2];
                        var student = new Student(studentId, studentName);
                        students[studentId] = student;
                        entities.Add(student); // Добавляем студента в список сущностей
                        break;

                    case "Course":
                        // Создаем курс
                        int courseId = int.Parse(parts[1]);
                        string courseName = parts[2];
                        var course = new Course(courseId, courseName);
                        courses[courseId] = course;
                        entities.Add(course); // Добавляем курс в список сущностей
                        break;

                    case "TeacherCourse":
                        // Связываем учителя с курсом
                        int tId = int.Parse(parts[1]);
                        int cId = int.Parse(parts[2]);
                        if (teachers.ContainsKey(tId) && courses.ContainsKey(cId))
                        {
                            teachers[tId].AddCourse(courses[cId]); // Привязываем курс к учителю
                        }
                        break;

                    case "StudentCourse":
                        // Связываем студента с курсом
                        int sId = int.Parse(parts[1]);
                        int scId = int.Parse(parts[2]);
                        if (students.ContainsKey(sId) && courses.ContainsKey(scId))
                        {
                            students[sId].AddCourse(courses[scId]); // Привязываем курс к студенту
                        }
                        break;

                    case "CourseTeacher":
                        // Связываем курс с учителем
                        int ctCourseId = int.Parse(parts[1]);
                        int ctTeacherId = int.Parse(parts[2]);
                        if (courses.ContainsKey(ctCourseId) && teachers.ContainsKey(ctTeacherId))
                        {
                            courses[ctCourseId].AddTeacher(teachers[ctTeacherId]); // Привязываем учителя к курсу
                        }
                        break;

                    case "CourseStudent":
                        // Связываем курс со студентом
                        int csCourseId = int.Parse(parts[1]);
                        int csStudentId = int.Parse(parts[2]);
                        if (courses.ContainsKey(csCourseId) && students.ContainsKey(csStudentId))
                        {
                            courses[csCourseId].AddStudent(students[csStudentId]); // Привязываем студента к курсу
                        }
                        break;
                }
            }
        }

        static void ShowRelatedEntities(List<Entity> entities)
        {
            Console.WriteLine("Введите ID сущности:");
            int id = int.Parse(Console.ReadLine());

            var entity = entities.FirstOrDefault(e => e.Id == id);

            if (entity == null)
            {
                Console.WriteLine("Сущность с таким ID не найдена.");
                return;
            }

            if (entity is Course course)
            {
                Console.WriteLine($"Учитель: {course.Teacher?.Name ?? "Нет"}");
                Console.WriteLine("Студенты:");
                foreach (var student in course.Students)
                {
                    Console.WriteLine(student.Name);
                }
            }
            else if (entity is Teacher teacher)
            {
                Console.WriteLine("Курсы учителя:");
                foreach (var courseteach in teacher.Courses)
                {
                    Console.WriteLine(courseteach.Name);
                }
            }
            else if (entity is Student student)
            {
                Console.WriteLine("Курсы студента:");
                foreach (var coursestd in student.Courses)
                {
                    Console.WriteLine(coursestd.Id);
                }
            }
        }
        static void SaveToFile(string filePath, List<Entity> entities)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Сохраняем учителей
                foreach (var teacher in entities.OfType<Teacher>())
                {
                    writer.WriteLine($"Teacher,{teacher.Id},{teacher.Name},{teacher.Experience}");
                    // Сохраняем связь учителя с курсами
                    foreach (var course in teacher.Courses)
                    {
                        writer.WriteLine($"TeacherCourse,{teacher.Id},{course.Id}");
                    }
                }

                // Сохраняем студентов
                foreach (var student in entities.OfType<Student>())
                {
                    writer.WriteLine($"Student,{student.Id},{student.Name}");
                    // Сохраняем связь студента с курсами
                    foreach (var course in student.Courses)
                    {
                        writer.WriteLine($"StudentCourse,{student.Id},{course.Id}");
                    }
                }

                // Сохраняем курсы
                foreach (var course in entities.OfType<Course>())
                {
                    writer.WriteLine($"Course,{course.Id},{course.Name}");
                    // Сохраняем связь курса с учителем (если он есть)
                    if (course.Teacher != null)
                    {
                        writer.WriteLine($"CourseTeacher,{course.Id},{course.Teacher.Id}");
                    }
                    // Сохраняем связь курса со студентами
                    foreach (var student in course.Students)
                    {
                        writer.WriteLine($"CourseStudent,{course.Id},{student.Id}");
                    }
                }
            }

            Console.WriteLine("Данные успешно сохранены в файл.");
        }

        static void AddStudent(List<Entity> entities)
        {
            Console.Write("Введите ID студента: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Введите имя студента: ");
            string name = Console.ReadLine();

            entities.Add(EntityFactory.FactoryMethod("Student," + id + "," + name));
            Console.WriteLine("Студент добавлен.");
        }

        static void AddTeacher(List<Entity> entities)
        {
            Console.Write("Введите ID учителя: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Введите имя учителя: ");
            string name = Console.ReadLine();

            Console.WriteLine("Введите опыт учителя: ");
            int exp = int.Parse(Console.ReadLine());

            entities.Add(EntityFactory.FactoryMethod("Teacher," + id + "," + name + "," + exp));
            Console.WriteLine("Учитель добавлен.");
        }

        static void AddCourse(List<Entity> entities)
        {
            Console.Write("Введите ID курса: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Введите название курса: ");
            string name = Console.ReadLine();

            entities.Add(EntityFactory.FactoryMethod("Course," + id + "," + name));
            Console.WriteLine("Курс добавлен.");
        }

        static void AddTeacherToCourse(List<Entity> entities)
        {
            Console.Write("Введите ID курса: ");
            int courseId = int.Parse(Console.ReadLine());

            // Поиск объекта курса по его ID
            var course = entities.OfType<Course>().FirstOrDefault(c => c.Id == courseId);
            if (course == null)
            {
                Console.WriteLine("Курс с таким ID не найден.");
                return;
            }

            Console.Write("Введите ID учителя: ");
            int teacherId = int.Parse(Console.ReadLine());

            // Поиск объекта учителя по его ID
            var teacher = entities.OfType<Teacher>().FirstOrDefault(t => t.Id == teacherId);
            if (teacher == null)
            {
                Console.WriteLine("Учитель с таким ID не найден.");
                return;
            }

            // Добавляем учителя к курсу
            course.AddTeacher(teacher);
            Console.WriteLine("Учитель успешно добавлен к курсу.");
        }

        static void AddStudentToCourse(List<Entity> entities)
        {
            Console.Write("Введите ID курса: ");
            int courseId = int.Parse(Console.ReadLine());

            // Поиск объекта курса по его ID
            var course = entities.OfType<Course>().FirstOrDefault(c => c.Id == courseId);
            if (course == null)
            {
                Console.WriteLine("Курс с таким ID не найден.");
                return;
            }

            Console.Write("Введите ID студента: ");
            int studentId = int.Parse(Console.ReadLine());

            // Поиск объекта студента по его ID
            var student = entities.OfType<Student>().FirstOrDefault(s => s.Id == studentId);
            if (student == null)
            {
                Console.WriteLine("Студент с таким ID не найден.");
                return;
            }

            // Добавляем студента к курсу
            course.AddStudent(student);
            Console.WriteLine("Студент успешно добавлен к курсу.");
        } 
    }
}