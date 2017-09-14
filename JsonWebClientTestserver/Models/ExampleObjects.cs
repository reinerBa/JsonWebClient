namespace JsonWebClientTestserver.Models
{
    public class ExampleObjects
    {
    }
    public class Abc {
        public int? a { get; set; }
        public Bclass b { get; set; }
        public string c { get; set; }

        public override string ToString()
        {
            return $"Models.Abc a:{a},b:{b},c:'{c}'";
        }
    }

    public class Cba {
        public int? a { get; set; }
        public Bclass b { get; set; }
        public string c { get; set; }

        public override string ToString()
        {
            return $"Models.Cba a:{a},b:{b},c:'{c}'";
        }
    }

    public class Bclass
    {
        public double bb { get; set; }

        public Bclass() { }
        public Bclass(double bb)
        {
            this.bb = bb;
        }

        public override string ToString()
        {
            return $"Bclass.bb='{bb}'";
        }
    }
}