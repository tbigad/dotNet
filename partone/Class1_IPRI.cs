using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;


namespace IPR_I
{
    //класс для представления студента
    public class Student
    {
        public int _group { get; set; }
        public string _studName { get; set; }
        public DateTimeOffset _studAge { get; set; }
        public Stack<int> _marks { get; set; }

        public void AddVarible(int group, string name, DateTimeOffset age, Stack<int> marks)
        {
            _group = group;
            _studName = name;
            _studAge = age;
            _marks = marks;
        }

        private DateTimeOffset Random()
        {
            throw new NotImplementedException();
        }
    };
//содержит кфедру и студентов. 
    public class faculty:IEnumerable<Student>

    {
        public string department;
        public List<Student> students;

        public void Faculty()
        {
            students = new List<Student>();

        }

        // делаем класс перечислимым при перечислении будут возвращатся студенты
        public IEnumerator<Student> GetEnumerator()
        {
            foreach (Student stud in students)
            {
                yield return stud;
            }
        }

        public IEnumerator<Student> IEnumerable<Student>.GetEnumerator()
        {
            foreach (Student stud in students)
            {
                yield return stud;
            }        
        }

    };

    //Назовем класс Library, а содержать он будет коллекцию и методы
    public class Library : IEnumerable<faculty>
    {
        public Dictionary<int, faculty> library;

        public Library()
        {
            library = new Dictionary<int, faculty>();
        }

        //При перечислении будут возвращатся факультеты.
        public IEnumerator<faculty> GetEnumerator()
        {
            foreach (KeyValuePair<int, faculty> stud in library)
            {
                yield return stud.Value;
            }
        }
        //добавление
        public void AddStudent()
        {
            faculty _faculty = new faculty();
            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Finish");
                key = Console.ReadKey();
                Console.WriteLine();
                switch (key.KeyChar)
                {
                    case '1':
                                Console.WriteLine();
                                Console.Write("Enter department: ");
                                _faculty.department = Console.ReadLine();
                                Student _student = new Student();
                                Console.Write("Enter Student Name: ");
                                string _n = Console.ReadLine();
                                Console.Write("Enter barthday date (01.01.2001 for example): ");
                                DateTimeOffset _a = DateTimeOffset.Parse(Console.ReadLine());
                                Console.Write("Enter Student Group: ");
                                int _g = int.Parse(Console.ReadLine());
                                Stack<int> _m = new Stack<int>();
                                 Console.Write("Enter Student Marks (1,2,3,4): ");
                                _m.Push(int.Parse(Console.ReadLine()));
                                _student.AddVarible(_g, _n, _a, _m);
                                _faculty.students.Add(_student);
                                Console.WriteLine();
                                break;
                }
            } while (key.KeyChar != '2');


            
            
        }
        //отображение
        public void PrintStudents(int key)
        { 
           try
           {
               Console.WriteLine();
               Console.WriteLine("Department: ", library[key].department);
               foreach (Student stud in library[key].students)
               {
                   Console.WriteLine("Name: ", stud._studName);
                   Console.WriteLine("Group: ", stud._group);
                   Console.WriteLine("Marks: ");
                   foreach (int mark in stud._marks)
                   {
                       Console.Write(" ", mark);
                   }
                   Console.WriteLine("Born date: ", stud._studAge);
               }

           }
           catch
               (KeyNotFoundException) { Console.WriteLine("Wrong Key"); }
        }
        //сохраняем как текст
        public void SaveAsText()
        {
            using (Stream fwrite = File.Create("data.txt"))
            {
                using (StreamWriter sw = new StreamWriter(fwrite))
                {
                    sw.WriteLine(library.Count);
                    foreach (faculty fac in this)
                    {
                        sw.WriteLine(fac.department);
                        foreach (Student stud in fac.students)
                        {
                            sw.WriteLine(stud._studName);
                            sw.WriteLine(stud._group);
                            sw.WriteLine(stud._marks);
                            sw.WriteLine(stud._studAge);
                        }
                    }
                }
            }
        }
    };
}