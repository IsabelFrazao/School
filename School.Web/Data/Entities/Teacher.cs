namespace School.Web.Data.Entities
{
    public class Teacher : Person
    {
        public override string ToString()
        {
            return $"{FullName}";
        }
    }
}
