using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryMethodClassLibrary
{
    public class Course : Entity
    {
        public Teacher Teacher { get; set; }
        public List<Student> Students { get; } = new List<Student>();
        public Course(int id, string name) : base(id, name)
        {
            this.Id = id;
            this.Name = name;
        }
        public void AddTeacher(Teacher teacher)
        {
            if (Teacher != null)
            {
                // У курса уже есть учитель, не можем добавить другого
                return;
            }
            Teacher = teacher;
            teacher.Courses.Add(this);
        }
        public void AddStudent(Student student)
        {
            if (Students.Contains(student))
            {
                // Студент уже добавлен в этот курс
                return;
            }
            Students.Add(student);
            student.Courses.Add(this);
        }
    }
}
