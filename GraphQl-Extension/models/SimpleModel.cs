namespace GraphQl_VFCM.models
{
    public class SimpleModelParameters
    {
        public SimpleModelParameters(string name, int age, char gender)
        {
            Name = name;
            Age = age;
            Gender = gender;
        }

        public string Name { get; set; }
        public int Age { get; set; }
        public char Gender { get; set; }
    }
    
    public class SimpleModelFields
    {
        public string FullName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }

    }

}