using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryMethodClassLibrary
{
    public class Teacher : Entity
    {
        public int Experience { get; set; }
        public List<Course> Courses { get; } = new List<Course>();
        public Teacher(int id, string name, int exp) : base(id, name)
        {
            this.Name = name;
            this.Id = id;
            this.Experience = exp;
        }
        public bool AddCourse(Course course)
        {
            if (Courses.Contains(course))
            {
                return false; // Курс уже добавлен 
            }
            Courses.Add(course);
            course.Teacher = this;
            return true;
        }
    }
}
