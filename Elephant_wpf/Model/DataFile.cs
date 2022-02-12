using System.Text.Json.Serialization;
namespace Elephant.Model
{
    public class DataFile
    {
        public string Name { get; set; }
        public DateTime CreateAt { get; set; }
        public List<TDCTag> Data { get; set; }

        public DataFile()
        {
            Name = Environment.UserName;
            CreateAt = DateTime.Now;
            Data = new List<TDCTag>();
        }
    }
}
