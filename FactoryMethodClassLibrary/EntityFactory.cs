using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryMethodClassLibrary
{
    public abstract class EntityFactory
    {
        //Частный случай фабричного метода
        public static Entity FactoryMethod(string line)
        {
            string[] parameters = line.Split(',');
            int id = Convert.ToInt32(parameters[1]);
            string name = parameters[2];
            if (parameters[0] == "Teacher")
            {
                int exp = Convert.ToInt32(parameters[3]);
                return new Teacher(id, name, exp);
            }
            if (parameters[0] == "Student")
            {

                return new Student(id, name);
            }
            if (parameters[0] == "Course")
            {
                return new Course(id, name);
            }
            throw new Exception();
        }
    }
}
