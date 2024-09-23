using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryMethodClassLibrary
{
    public class Student : Entity
    {
        public List<Course> Courses { get; } = new List<Course>();
        public Student(int id, string name) : base(id, name)
        {
            this.Id = id;
            this.Name = name;
        }
        public bool AddCourse(Course course)
        {
            if (Courses.Contains(course))
            {
                return false; // Курс уже добавлен 
            }
            Courses.Add(course);
            course.Students.Add(this);
            return true;
        }
    }
}
