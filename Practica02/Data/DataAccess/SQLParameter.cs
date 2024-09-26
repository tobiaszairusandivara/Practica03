namespace Practica02.Data.DataAccess
{
    public class SQLParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public SQLParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
